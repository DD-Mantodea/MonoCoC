using Microsoft.Xna.Framework.Input;
using MonoCoC.Registry;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.Assets;
using MonoCoC.Contents.Scenes.Vanilla;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Contents.Input
{
    // 可注册的快捷键定义。mod 通过 HotkeyRegistry.Register 添加自己的快捷键。
    public sealed class HotkeyDefinition(Identifier id, string name, string description,

        Keys defaultPrimary, Keys defaultSecondary, Action? action)
    {
        public readonly Identifier Id = id;

        public readonly string Name = name;

        public readonly string Description = description;

        public readonly Keys DefaultPrimary = defaultPrimary;

        public readonly Keys DefaultSecondary = defaultSecondary;

        public readonly Action? Action = action;

        public Keys Primary = defaultPrimary;

        public Keys Secondary = defaultSecondary;
    }

    // 快捷键注册表：原版快捷键在 Hotkeys.Register 中注册；
    // mod 可在自己的初始化代码里调用 Register 追加。
    public static class HotkeyRegistry
    {
        private static readonly List<HotkeyDefinition> _all = [];

        private static readonly Dictionary<Identifier, HotkeyDefinition> _byId = [];

        public static IReadOnlyList<HotkeyDefinition> All => _all;

        public static HotkeyDefinition Register(Identifier id, string name, string description,

            Keys defaultPrimary, Keys defaultSecondary = Keys.None, Action? action = null)
        {
            if (_byId.TryGetValue(id, out var existing))
                return existing;

            var definition = new HotkeyDefinition(id, name, description, defaultPrimary, defaultSecondary, action);

            _all.Add(definition);

            _byId[id] = definition;

            return definition;
        }

        public static HotkeyDefinition Register(string id, string name, string description,

            Keys defaultPrimary, Keys defaultSecondary = Keys.None, Action? action = null) =>

            Register(Identifier.Parse(id), name, description, defaultPrimary, defaultSecondary, action);

        public static bool TryGet(Identifier id, out HotkeyDefinition definition) =>

            _byId.TryGetValue(id, out definition!);

        private static string ConfigPath => Path.Combine(Paths.Root, "hotkeys.json");

        public static void Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return;

                var root = Hjson.HjsonValue.Parse(File.ReadAllText(ConfigPath));

                if (root is not Hjson.JsonObject obj)
                    return;

                foreach (var definition in _all)
                {
                    if (!obj.TryGetValue(definition.Id.ToString(), out var value) || value is not Hjson.JsonObject entry)
                        continue;

                    if (entry.TryGetValue("primary", out var primary) && Enum.TryParse<Keys>((string)primary, out var primaryKey))
                        definition.Primary = primaryKey;

                    if (entry.TryGetValue("secondary", out var secondary) && Enum.TryParse<Keys>((string)secondary, out var secondaryKey))
                        definition.Secondary = secondaryKey;
                }
            }
            catch
            {
                // 配置损坏时保留默认键位
            }
        }

        private static void Save()
        {
            try
            {
                var root = new Hjson.JsonObject();

                foreach (var definition in _all)
                    root[definition.Id.ToString()] = new Hjson.JsonObject
                    {
                        ["primary"] = definition.Primary.ToString(),
                        ["secondary"] = definition.Secondary.ToString(),
                    };

                File.WriteAllText(ConfigPath, root.ToString());
            }
            catch
            {
                // 存盘失败不影响本次运行
            }
        }

        public static HotkeyDefinition? PendingBinding { get; private set; }

        public static bool PendingPrimary { get; private set; }

        public static void BeginBind(HotkeyDefinition definition, bool primary)
        {
            PendingBinding = definition;

            PendingPrimary = primary;
        }

        public static void Reset()
        {
            foreach (var definition in _all)
            {
                definition.Primary = definition.DefaultPrimary;

                definition.Secondary = definition.DefaultSecondary;
            }

            Save();
        }

        public static void Clear()
        {
            foreach (var definition in _all)
            {
                definition.Primary = Keys.None;

                definition.Secondary = Keys.None;
            }

            Save();
        }

        public static string KeyName(Keys key) => key == Keys.None ? "Unbound" : key.ToString();

        public static void Update()
        {
            if (UIManager.Instance?.FocusedComponent is TextBox)
                return;

            // 启动阶段 Scene/UI 尚未就绪时，按键状态可能被判定为首次按下
            if (UIManager.Instance?.Root == null || MonoCoC.Contents.Scenes.Scene.Current == null)
                return;

            if (PendingBinding != null)
            {
                foreach (var key in UserInput.CurrentKeyState.GetPressedKeys())
                {
                    if (UserInput.PreviousKeyState.IsKeyDown(key))
                        continue;

                    Assign(PendingBinding, PendingPrimary, key);

                    PendingBinding = null;

                    Options.RefreshControls();

                    return;
                }

                return;
            }

            foreach (var definition in _all)
                if (definition.Primary != Keys.None && UserInput.IsJustPress(definition.Primary)
                    || definition.Secondary != Keys.None && UserInput.IsJustPress(definition.Secondary))
                {
                    definition.Action?.Invoke();

                    return;
                }
        }

        private static void Assign(HotkeyDefinition target, bool primary, Keys key)
        {
            foreach (var definition in _all)
            {
                if (definition == target)
                    continue;

                if (definition.Primary == key)
                    definition.Primary = Keys.None;

                if (definition.Secondary == key)
                    definition.Secondary = Keys.None;
            }

            if (primary)
                target.Primary = key;
            else
                target.Secondary = key;

            Save();
        }
    }
}