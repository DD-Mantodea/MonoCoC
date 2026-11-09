using MonoCoC.Assets;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.World;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using MonoCoC.UI.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using static MonoCoC.UI.Layout.LayoutConstants;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Framework;
using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Data;

namespace MonoCoC.UI.Components.Views
{
    public abstract class GameView : HjsonView
    {
        private readonly System.Collections.Generic.List<Button> _topMenuButtons = [];

        private Component? _navigation;

        private Button? _menuButton;

        private string _gameOverTitle = "";

        protected abstract override string ScreenFile { get; }

        protected override UIViewContract Contract => GameViewContract.Value;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (UserInput.IsJustPress(Keys.Escape))
                ToggleMenu();

            var scene = Scene.Current;

            var hide = scene is Contents.Scenes.Vanilla.Options

                or Contents.Scenes.Vanilla.SaveLoad

                or Contents.Scenes.Vanilla.AchievementsScene

                or Contents.Scenes.Vanilla.CreditsScene

                or Contents.Scenes.Vanilla.InstructionsScene

                or Contents.Scenes.Vanilla.Places.PlacesScene

                or Contents.Scenes.Vanilla.CharCreation;

            if (hide && _navigation is not null)
                _navigation.Visible = false;

            if (_menuButton is not null)
                _menuButton.Visible = !hide;

            var player = CoC.Instance.Player;

            for (var index = 0; index < _topMenuButtons.Count; index++)
            {
                var button = _topMenuButtons[index];

                button.Visible = !hide;

                // 閸樼喓澧?MENU_LEVEL 閸欘亜婀崣顖氬磳缁?閸欘垰濮炵仦鐐粹偓?閸欘垯鎷辨稉鎾绘毐閺冭埖妯夌粈?
                if (index == 3)
                    button.Visible = !hide && player != null

                        && (player.CanLevelUp() || player.CanBuyStats() || player.CanBuyPerks());
            }
        }

        // 閸樼喓澧?MENU_STATS -> playerInfo.displayStats
        private static void OpenStats()
        {
            var previous = Scene.Current;

            PlayerStats.HiddenByMenu = true;

            Contents.Scenes.Vanilla.StatsScene.Show(() =>
            {
                // 娴犲骸鍙炬禒鏍€婇弽蹇涖€夐棃銏ｇ箻閸?Stats 閺冭绱滲ack 娑撳秴绨查崶鐐插煂鐠囥儵銆婇弽蹇涖€夐棃?
                if (previous is Contents.Scenes.Vanilla.PerksScene
                    or Contents.Scenes.Vanilla.PlayerAppearance)
                {
                    Scene.Go<Contents.Scenes.Vanilla.CampScene>();

                    return;
                }

                if (previous != null)
                    Scene.Go(previous);
                else
                    Scene.Go<Contents.Scenes.Vanilla.CampScene>();
            });
        }

        // 濞撳憡鍨欓崘鍛淬€婇弽?Data閿涙矮绗夐弰顖欏瘜閼挎粌宕熼惃?Data閿涘矁绻戦崶鐐存鎼存柨娲栧〒鍛婂灆
        private static void OpenData()
        {
            PlayerStats.HiddenByMenu = true;

            Contents.Scenes.Vanilla.SaveLoad.FromMainMenu = false;

            Scene.Go(SceneInstance<Contents.Scenes.Vanilla.SaveLoad>.Instance);
        }

        public GameView()
        {
            Instance = this;

            Initialize(CreateEvents(), CreateWidgets());

            Time.HourPassed += OnHourPassed;

            TimeAwareRegistry.ReloadVanilla();

            BindParts();
        }

        protected override void OnBuilt(View root, UIDocument document) => BindParts();

        private static UIWidgetRegistry CreateWidgets()
        {
            var widgets = new UIWidgetRegistry();

            widgets.Register("game:sprite", _ => new UIImage(null));

            widgets.Register("game:image", _ => new UIImage(null));

            widgets.Register("game:dungeon_map", _ => new DungeonMapPanel());

            widgets.Register("game:mini_map", _ => new DungeonMiniMapPanel());

            widgets.Register("game:multi_combat_stats", _ => new MultiCombatStats());

            widgets.Register("game:target_pointer", _ =>
                new UIImage(ManagerInstance<TextureManager>.Instance["selection"]));

            return widgets;
        }

        private void BindParts()
        {
            Background = RequirePart<Component>("game.background");

            Output = RequirePart<TextArea>("game.output");

            ButtonGrid = RequirePart<Grid>("game.buttons");

            PlayerStats = RequirePart<PlayerStats>("game.playerStats");

            Input = RequirePart<TextBox>("game.input");

            CombatStats = RequirePart<CombatStats>("game.combatStats");

            MultiCombatStats = RequirePart<MultiCombatStats>("game.multiCombatStats");

            Sprite = RequirePart<UIImage>("game.sprite");

            Image = RequirePart<UIImage>("game.image");

            DungeonMap = RequirePart<DungeonMapPanel>("game.dungeonMap");

            DungeonMiniMap = RequirePart<DungeonMiniMapPanel>("game.miniMap");

            _targetPointer = RequirePart<UIImage>("game.targetPointer");

            GameOverLabel = RequirePart<Label>("game.gameOver");

            if (GameOverActive)
            {
                GameOverLabel.Text(_gameOverTitle);

                GameOverLabel.Visible = true;
            }

            _navigation = RequirePart<Component>("game.navigation");

            _menuButton = RequirePart<Button>("game.menuButton");

            Output.BackgroundColor = Color.White * 0.4f;

            Output.Padding = TextZonePadding;

            Output.AutoScroll = false;

            PlayerStats.Visible = !Contents.Scenes.Vanilla.Options.HidePlayerStats;

            _targetPointer.Color = new Color(0x3D, 0x35, 0x2F);

            _topMenuButtons.Clear();

            var topMenu = RequirePart<Grid>("game.topMenu");

            foreach (var child in topMenu.LayoutData.Children)
                if (child is Button button)
                    _topMenuButtons.Add(button);

            Buttons = new Button[SlotCount];

            for (var index = 0; index < ButtonGrid.LayoutData.Children.Count && index < Buttons.Length; index++)
                if (ButtonGrid.LayoutData.Children[index] is Button button)
                {
                    button.Visible = false;

                    Buttons[index] = button;
                }
        }

        private static UIEventRegistry CreateEvents()
        {
            var events = new UIEventRegistry();

            events.RegisterEvent("game.main_menu", () => UIManager.Instance.Root.SwitchView(new MainView()));

            events.RegisterEvent("game.data", () => OpenData());

            events.RegisterEvent("game.stats", () => OpenStats());

            events.RegisterEvent("game.level_up", () => Scene.Go(SceneInstance<Contents.Scenes.Vanilla.LevelUpScene>.Instance));

            events.RegisterEvent("game.perks", () => Scene.Go(SceneInstance<Contents.Scenes.Vanilla.PerksScene>.Instance));

            events.RegisterEvent("game.appearance", () => Scene.Go(SceneInstance<Contents.Scenes.Vanilla.PlayerAppearance>.Instance));

            events.RegisterEvent("game.toggle_menu", () => Instance?.ToggleMenu());

            return events;
        }

        public void ToggleMenu()
        {
            if (_navigation is not null)
                _navigation.Visible = !_navigation.Visible;
        }

        public void ApplySceneLayout(Scene scene)
        {
            SwitchFor(scene);
        }

        public static void SwitchFor(Scene scene)
        {
            if (UIManager.Instance?.Root is null || Instance is null)
                return;

            var wantCamp = scene is Contents.Scenes.Vanilla.CampScene;

            if (wantCamp && Instance is not CampView)
                UIManager.Instance.Root.SwitchView(new CampView());
            else if (!wantCamp && Instance is not DefaultGameView)
                UIManager.Instance.Root.SwitchView(new DefaultGameView());
        }

        public static GameView CreateDefault() => new DefaultGameView();

        public MultiCombatStats MultiCombatStats = null!;

        private UIImage _targetPointer = null!;

        public void ShowCombatStats(Enemy enemy)
        {
            _targetPointer.Visible = false;

            CombatStats.Show(enemy);

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        public void ShowCombatStats(System.Collections.Generic.IReadOnlyList<Enemy> enemies, int selected, Action<int> select)
        {
            CombatStats.Hide();

            MultiCombatStats.Refresh(enemies, selected, select);

            var panelTop = CombatStatsY + selected * (EnemyStatsPanel.PanelHeight + MultiCombatStats.Gap);

            _targetPointer.LayoutData.Position.X = CombatStatsX + (EnemyStatsPanel.PanelWidth - 20) / 2;

            _targetPointer.LayoutData.Position.Y = panelTop - 4;

            _targetPointer.Visible = enemies.Count > 1 && selected >= 0 && selected < enemies.Count;

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        public void HideCombatStats()
        {
            CombatStats.Hide();

            Output.Size(TextZoneWidth, TextZoneHeight);
        }

        // 閸樼喓澧?EventParser.gameOver 閻ㄥ嫮鏅棃銏ゅ劥閸掑棴绱版潻娑樺弳閵嗗苯鍑″璁抽閵嗗秶濮搁幀渚婄礉
        // 閹跺﹥鍨弬?鐏炵偞鈧囨桨閺夎儻顔€娴ｅ秶绮扮仦鍛厬閻?GAME OVER 婢堆冪摟閵?
        public bool GameOverActive { get; private set; }

        public Label GameOverLabel;

        public void ShowGameOver(string title)
        {
            GameOverActive = true;

            _gameOverTitle = title;

            CombatStats.Hide();

            StatsVisible = false;

            // GameOverLabel 閺勵垰婀敮鍐ㄧ湰閺冭泛鍨卞铏规畱閿涙艾绔风仦鈧稊瀣閿涘牅绶ユ俊?REPL 闁插本鐥呯捄鎴ｇ箖娑撯偓鐢嶇礆閸欘垵鍏樻潻妯绘Ц null
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

            // 閸樼喓澧?InfestedChameleonGirlScene 閺?TimeAwareInterface閿涙俺闊╂稉濠冩箒妲屾濂旂€靛嫮鏁撻搹顐ｆ濮ｅ繐鐨弮鑸靛腹鏉╂稐绔村▎?
            if (CoC.Instance?.Player?.HasStatusEffect(
                    Contents.Datas.Vanilla.StatusEffectIds.ParasiteEel) == true)

                SceneInstance<

                    Contents.Scenes.Vanilla.Areas.Bog.InfestedChameleonGirlScene>.Get().ParasiteUpdate();

            // NPC 閼奉亜绻侀惃鍕洴婵炵媴绱欓崢鐔哄濮ｅ繋閲?NPC 閻?PregnancyStore.timeChange閿?
            Contents.Npcs.NpcPregnancy.Tick();

            // 閸樼喓澧?TimeAware 濞叉儳褰傞敍鍦梫entParser.hx:235 / :268-274閿涘绱?
            // TamanisDaughters / Kelly 閸?TimeAwareRegistry.ReloadVanilla 闁插本鏁為崘灞烩偓?
            // 閺堫亜鐤勯悳?timeChangeLarge 鏉╂柨娲?true 閸氬海娈戦妴灞炬畯閸嬫粎鐡?Next閵嗗秹妲﹂崚妤嬬礄閸樼喓澧?EventParser.hx:268-274閿涘绱?
            // 鏉╂柨娲栭崐鐓庡帥閻?TimeAware.cs 濮瑰洦鈧眹鈧?
            _ = TimeAwareRegistry.DispatchHour(hour);
        }

        public static bool Ready => Instance != null;

        public static void SetPlayerStatsVisible(bool visible)
        {
            Instance?.PlayerStats.Visible = visible;
        }

        public Component Background;

        public TextArea Output;

        public Grid ButtonGrid;

        public Button[] Buttons = new Button[SlotCount];

        public UIImage Sprite;

        public UIImage Image;

        public DungeonMapPanel DungeonMap;

        public DungeonMiniMapPanel DungeonMiniMap;

        public TextBox Input;

        public PlayerStats PlayerStats;

        public CombatStats CombatStats;

        public bool StatsVisible = true;

        public bool ArrowsVisible = true;

        public bool SpriteVisible = true;

        public bool ImageVisible = true;
    }
}

