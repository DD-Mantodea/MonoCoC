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
using MonoCoC.UI.Framework;
using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Data;
using System.Collections.Generic;

namespace MonoCoC.UI.Components.Views
{
    public abstract class GameView : HjsonView
    {
        protected readonly List<Button> _topMenuButtons = [];

        protected Component? _navigation;

        protected Button? _menuButton;

        protected string _gameOverTitle = "";

        public bool DataMenuVisible { get; private set; }

        public static bool CanQuickSave => Instance?.DataMenuVisible == true;

        public static bool CanQuickLoad =>
            Instance?.DataMenuVisible == true || Configuration.OptionStore.Instance.Get("quickloadAnywhere", 0) > 0;

        protected abstract override string ScreenFile { get; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Scene.ReapplyActionsIfNeeded();

            if (UserInput.IsJustPress(Keys.Escape))
                ToggleMenu();

            var scene = Scene.Current;

            var hide = scene is Contents.Scenes.Vanilla.SaveLoad

                or Contents.Scenes.Vanilla.AchievementsScene

                or Contents.Scenes.Vanilla.CreditsScene

                or Contents.Scenes.Vanilla.InstructionsScene

                or Contents.Scenes.Vanilla.Places.PlacesScene

                or Contents.Scenes.Vanilla.CharCreation;

            if (hide && _navigation is not null)
                _navigation.Visible = false;

            _menuButton?.Visible = !hide;

            var player = CoC.Instance.Player;

            DataMenuVisible = !hide && player is { Loaded: true } &&
                              !Contents.Combat.CombatController.InCombat &&
                              !Contents.Dungeons.DungeonSession.InDungeon;

            for (var index = 0; index < _topMenuButtons.Count; index++)
            {
                var button = _topMenuButtons[index];

                button.Visible = !hide;

                if (index == 3)
                    button.Visible = !hide && player != null

                        && (player.CanLevelUp() || player.CanBuyStats() || player.CanBuyPerks());
            }
        }

        public GameView()
        {
            Instance = this;

            Time.HourPassed += OnHourPassed;

            TimeAwareRegistry.ReloadVanilla();
        }

        [UIComponent("game:sprite")]
        private static Component CreateSprite() => new UIImage(null);

        [UIComponent("game:image")]
        private static Component CreateImage() => new UIImage(null);

        [UIComponent("game:dungeon_map")]
        private static Component CreateDungeonMap() => new DungeonMapPanel();

        [UIComponent("game:mini_map")]
        private static Component CreateMiniMap() => new DungeonMiniMapPanel();

        [UIComponent("game:multi_combat_stats")]
        private static Component CreateMultiCombatStats() => new MultiCombatStats();

        [UIComponent("game:target_pointer")]
        private static Component CreateTargetPointer() =>
            new UIImage(ManagerInstance<TextureManager>.Instance["selection"]);

        public void ToggleMenu()
        {
            _navigation?.Visible = !_navigation.Visible;
        }

        public MultiCombatStats? MultiCombatStats = null!;

        protected UIImage? _targetPointer = null!;

        public void ShowCombatStats(Enemy enemy)
        {
            _targetPointer?.Visible = false;

            CombatStats?.Show(enemy);

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        public void ShowCombatStats(IReadOnlyList<Enemy> enemies, int selected, Action<int> select)
        {
            CombatStats?.Hide();

            MultiCombatStats?.Refresh(enemies, selected, select);

            var panelTop = CombatStatsY + selected * (EnemyStatsPanel.PanelHeight + MultiCombatStats.Gap);

            if (_targetPointer is not null)
            {
                _targetPointer.LayoutData.Position.X = CombatStatsX + (EnemyStatsPanel.PanelWidth - 20) / 2;

                _targetPointer.LayoutData.Position.Y = panelTop - 4;

                _targetPointer.Visible = enemies.Count > 1 && selected >= 0 && selected < enemies.Count;
            }

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        public void HideCombatStats()
        {
            CombatStats?.Hide();

            Output.Size(TextZoneWidth, TextZoneHeight);
        }

        public bool GameOverActive { get; private set; }

        public Label? GameOverLabel;

        public void ShowGameOver(string title)
        {
            GameOverActive = true;

            _gameOverTitle = title;

            CombatStats?.Hide();

            StatsVisible = false;

            if (GameOverLabel != null)
            {
                GameOverLabel.Text(title);

                GameOverLabel.Visible = true;
            }

            Output.Size(TextZoneWidth, TextZoneHeight);
        }

        public void HideGameOver()
        {
            GameOverActive = false;

            _gameOverTitle = "";

            StatsVisible = true;

            GameOverLabel?.Visible = false;
        }

        public static GameView? Instance;

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

        public static bool Ready => Instance != null;

        public static void SetPlayerStatsVisible(bool visible)
        {
            if (Instance?.PlayerStats is not null)
                Instance.PlayerStats.Visible = visible;
        }

        public Component Background;

        public TextArea Output;

        public Grid ButtonGrid;

        public Button[] Buttons = new Button[SlotCount];

        public UIImage? Sprite;

        public UIImage? Image;

        public DungeonMapPanel? DungeonMap;

        public DungeonMiniMapPanel? DungeonMiniMap;

        public TextBox Input;

        public PlayerStats PlayerStats;

        public CombatStats? CombatStats;

        public bool StatsVisible = true;

        public bool ArrowsVisible = true;

        public bool SpriteVisible = true;

        public bool ImageVisible = true;
    }
}

