using Microsoft.Xna.Framework.Input;
using MonoCoC.Registry;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.Assets;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Contents.Input
{
    // 可注册的快捷键定义。mod 通过 HotkeyRegistry.Register 添加自己的快捷键。
    public class HotkeyDefinition(Identifier id, string name, string description,

        Keys defaultPrimary, Keys defaultSecondary, Action? action, Func<bool>? canExecute = null)
    {
        public readonly Identifier Id = id;

        public readonly string Name = name;

        public readonly string Description = description;

        public readonly Keys DefaultPrimary = defaultPrimary;

        public readonly Keys DefaultSecondary = defaultSecondary;

        public readonly Action? Action = action;

        public readonly Func<bool>? CanExecute = canExecute;

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

            Keys defaultPrimary, Keys defaultSecondary = Keys.None, Action? action = null,
            Func<bool>? canExecute = null)
        {
            if (_byId.TryGetValue(id, out var existing))
                return existing;

            var definition = new HotkeyDefinition(id, name, description, defaultPrimary, defaultSecondary, action, canExecute);

            RemoveConflicts(definition);

            _all.Add(definition);

            _byId[id] = definition;

            return definition;
        }

        public static HotkeyDefinition Register(string id, string name, string description,

            Keys defaultPrimary, Keys defaultSecondary = Keys.None, Action? action = null,
            Func<bool>? canExecute = null) =>

            Register(Identifier.Parse(id), name, description, defaultPrimary, defaultSecondary, action, canExecute);

        public static bool TryGet(Identifier id, out HotkeyDefinition definition) =>

            _byId.TryGetValue(id, out definition!);

        private static string ConfigPath => Path.Combine(Paths.Root, "hotkeys.json");

        public static void Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return;

                var root = MonoCoC.HjsonParser.Parse(File.ReadAllText(ConfigPath));

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

                NormalizeConflicts();
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

        private static string _notice = "";

        // UI 取走一次性提示（冲突解绑 / 清空），取走即清空。
        public static string TakeNotice()
        {
            var notice = _notice;

            _notice = "";

            return notice;
        }

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

            _notice = "View.Options.Controls.ResetNotice".Localized();

            Save();
        }

        public static void Clear()
        {
            foreach (var definition in _all)
            {
                definition.Primary = Keys.None;

                definition.Secondary = Keys.None;
            }

            _notice = "View.Options.Controls.ClearedNotice".Localized();

            Save();
        }

        public static string KeyName(Keys key) =>
            key == Keys.None ? "View.Options.Controls.Unbound".Localized() : key.ToString();

        public static void Update()
        {
            if (!UserInput.HasFocus)
                return;

            if (UIManager.Instance?.InputBlock is not null)
                return;

            if (UIManager.Instance?.FocusedComponent is TextBox)
                return;

            // 待命绑定必须排在 Scene 守卫之前：主菜单打开 Options 时 Scene.Current 是 null，
            // 放到守卫后面会导致“点了 chip 却按什么都没反应、Esc 也退不出”（2026-11-16 报过）。
            if (PendingBinding != null)
            {
                foreach (var key in UserInput.CurrentKeyState.GetPressedKeys())
                {
                    if (UserInput.PreviousKeyState.IsKeyDown(key))
                        continue;

                    // Esc 取消待命，不写入任何键位。
                    if (key == Keys.Escape)
                    {
                        PendingBinding = null;

                        MonoCoC.UI.Components.Views.OptionsView.RefreshControls();

                        return;
                    }

                    Assign(PendingBinding, PendingPrimary, key);

                    PendingBinding = null;

                    MonoCoC.UI.Components.Views.OptionsView.RefreshControls();

                    return;
                }

                return;
            }

            // 启动阶段 Scene/UI 尚未就绪时，按键状态可能被判定为首次按下
            if (UIManager.Instance?.Root == null || Scenes.Scene.Current == null)
                return;

            foreach (var definition in _all)
                if ((definition.CanExecute?.Invoke() ?? true) &&
                    (definition.Primary != Keys.None && UserInput.IsJustPress(definition.Primary)
                     || definition.Secondary != Keys.None && UserInput.IsJustPress(definition.Secondary)))
                {
                    definition.Action?.Invoke();

                    return;
                }
        }

        private static void RemoveConflicts(HotkeyDefinition definition)
        {
            foreach (var other in _all)
            {
                if (definition.Primary != Keys.None && other.Primary == definition.Primary)
                    other.Primary = Keys.None;

                if (definition.Primary != Keys.None && other.Secondary == definition.Primary)
                    other.Secondary = Keys.None;

                if (definition.Secondary != Keys.None && other.Primary == definition.Secondary)
                    other.Primary = Keys.None;

                if (definition.Secondary != Keys.None && other.Secondary == definition.Secondary)
                    other.Secondary = Keys.None;
            }
        }

        private static void NormalizeConflicts()
        {
            for (var index = _all.Count - 1; index >= 0; index--)
            {
                var definition = _all[index];

                for (var previous = 0; previous < index; previous++)
                {
                    var other = _all[previous];

                    if (definition.Primary != Keys.None && other.Primary == definition.Primary)
                        other.Primary = Keys.None;

                    if (definition.Primary != Keys.None && other.Secondary == definition.Primary)
                        other.Secondary = Keys.None;

                    if (definition.Secondary != Keys.None && other.Primary == definition.Secondary)
                        other.Primary = Keys.None;

                    if (definition.Secondary != Keys.None && other.Secondary == definition.Secondary)
                        other.Secondary = Keys.None;
                }
            }
        }

        private static void Assign(HotkeyDefinition target, bool primary, Keys key)
        {
            var unbound = new List<string>();

            foreach (var definition in _all)
            {
                if (definition == target)
                    continue;

                if (definition.Primary == key)
                {
                    definition.Primary = Keys.None;

                    unbound.Add(definition.Name);
                }

                if (definition.Secondary == key)
                {
                    definition.Secondary = Keys.None;

                    unbound.Add(definition.Name);
                }
            }

            if (primary)
                target.Primary = key;
            else
                target.Secondary = key;

            if (unbound.Count > 0)
                _notice = "View.Options.Controls.UnboundNotice".Localized(KeyName(key), string.Join(", ", unbound));

            Save();
        }
    }
}
