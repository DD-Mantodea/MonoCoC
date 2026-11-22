using MonoCoC.Assets;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.World;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using static MonoCoC.UI.Layout.LayoutConstants;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Data;
using System.Collections.Generic;

namespace MonoCoC.UI.Components.Views
{
    public abstract class GameView : HjsonView
    {
        public static GameView? Current { get; private set; }

        public static void SwitchTo<T>() where T : View, new()
        {
            var view = ViewInstance<T>.Get();

            UIManager.Instance.Root.SwitchView(view);

            Current = view as GameView;
        }

        public static void SwitchTo(GameView view)
        {
            UIManager.Instance.Root.SwitchView(view);

            Current = view;
        }

        public static void SwitchTo<T>(Context<T> context) where T : GameView<T>, new()
        {
            var view = ViewInstance<T>.Get();

            UIManager.Instance.Root.SwitchView(view);

            Current = view;

            view.ApplyContext(context);
        }

        public bool DataMenuVisible { get; private set; }

        public static bool CanQuickSave => Current?.DataMenuVisible == true;

        public static bool CanQuickLoad =>
            Current?.DataMenuVisible == true || Configuration.OptionStore.Instance.Get("quickloadAnywhere", 0) > 0;

        protected abstract override string ScreenFile { get; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Scene.ReapplyActionsIfNeeded();

            if (UserInput.HasFocus && UserInput.IsJustPress(Keys.Escape))
                ToggleMenu();

            var player = CoC.Instance.Player;

            DataMenuVisible = player is { Loaded: true } &&
                              !Contents.Combat.CombatController.InCombat &&
                              !Contents.Dungeons.DungeonSession.InDungeon;
        }

        public GameView()
        {
            Current = this;

            Time.HourPassed += OnHourPassed;

            TimeAwareRegistry.ReloadVanilla();
        }

        public void Write(string text)
        {
            Output?.Append(Scene.Parser.Parse(text));
        }

        public void ClearOutput()
        {
            Output?.Clear();
        }

        public void ClearButtons() => Scene.ClearButtons();

        public Button? AddButton(int slot, string text, Action? onClick = null, string hint = "", string hintHeader = "") =>
            Scene.AddButton(slot, text, onClick, hint, hintHeader);

        public Button? AddButtonDisabled(int slot, string text, string hint = "", string hintHeader = "") =>
            Scene.AddButtonDisabled(slot, text, hint, hintHeader);

        public virtual void ToggleMenu()
        {
            MenuBarView.Toggle();
        }

        private static readonly Time SharedTime = new();

        public Time Time => SharedTime;

        private void OnHourPassed(int hour)
        {
            CoC.Instance?.Player?.TickPregnancy();

            CorruptedGladeTime.OnHourPassed(hour);

            if (CoC.Instance?.Player?.HasStatusEffect(
                    Contents.Datas.Vanilla.StatusEffectIds.ParasiteEel) == true)

                SceneInstance<

                    Contents.Scenes.Vanilla.Areas.Bog.InfestedChameleonGirlScene>.Get().ParasiteUpdate();

            Contents.Npcs.NpcPregnancy.Tick();

            _ = TimeAwareRegistry.DispatchHour(hour);
        }

        public static bool Ready => Current != null;

        public static void SetPlayerStatsVisible(bool visible)
        {
            if (Current?.PlayerStats is not null)
                Current.PlayerStats.Visible = visible;
        }

        public Component Background;

        public TextArea Output;

        public Grid ButtonGrid;

        public Button[] Buttons = new Button[SlotCount];

        public UIImage? Sprite;

        public UIImage? Image;

        public TextBox Input;

        public PlayerStats PlayerStats;

        public bool StatsVisible = true;

        public bool ArrowsVisible = true;

        public bool SpriteVisible = true;

        public bool ImageVisible = true;
    }

    public abstract class GameView<T> : GameView where T : GameView<T>
    {
        public virtual void ApplyContext(Context<T> context) { }
    }
}

