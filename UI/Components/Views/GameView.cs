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

                // 闁告鍠撴晶?MENU_LEVEL 闁告瑯浜滃﹢顏堝矗椤栨艾纾崇紒?闁告瑯鍨版慨鐐典沪閻愮补鍋?闁告瑯鍨幏杈ㄧ▔閹剧粯姣愰柡鍐煐濡绮?
                if (index == 3)
                    button.Visible = !hide && player != null

                        && (player.CanLevelUp() || player.CanBuyStats() || player.CanBuyPerks());
            }
        }

        // 闁告鍠撴晶?MENU_STATS -> playerInfo.displayStats
        private static void OpenStats()
        {
            var previous = Scene.Current;

            PlayerStats.HiddenByMenu = true;

            Contents.Scenes.Vanilla.StatsScene.Show(() =>
            {
                // 濞寸姴楠搁崣鐐閺嶎厹鈧﹪寮借箛娑栤偓澶愭閵忥絿绠婚柛?Stats 闁哄啳顔愮槐婊瞐ck 濞戞挸绉寸花鏌ュ炊閻愭彃鐓傞悹鍥ュ劦閵嗗﹪寮借箛娑栤偓澶愭?
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

        // 婵炴挸鎲￠崹娆撳礃閸涙番鈧﹪寮?Data闁挎稒鐭粭澶愬及椤栨瑥鐦滈柤鎸庣矊瀹曠喖鎯?Data闁挎稑鐭佺换鎴﹀炊閻愬瓨顦ч幖瀛樻煥濞叉牕銆掗崨濠傜亞
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

            CombatStats = Part<CombatStats>("game.combatStats");

            MultiCombatStats = Part<MultiCombatStats>("game.multiCombatStats");

            Sprite = Part<UIImage>("game.sprite");

            Image = Part<UIImage>("game.image");

            DungeonMap = Part<DungeonMapPanel>("game.dungeonMap");

            DungeonMiniMap = Part<DungeonMiniMapPanel>("game.miniMap");

            _targetPointer = Part<UIImage>("game.targetPointer");

            GameOverLabel = Part<Label>("game.gameOver");

            if (GameOverActive && GameOverLabel is not null)
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

            _targetPointer?.Color = new Color(0x3D, 0x35, 0x2F);

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

        public MultiCombatStats? MultiCombatStats = null!;

        private UIImage? _targetPointer = null!;

        public void ShowCombatStats(Enemy enemy)
        {
            if (_targetPointer is not null)
                _targetPointer.Visible = false;

            CombatStats?.Show(enemy);

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        public void ShowCombatStats(System.Collections.Generic.IReadOnlyList<Enemy> enemies, int selected, Action<int> select)
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

            // GameOverLabel 闁哄嫷鍨板﹢顏嗘暜閸愩劎婀伴柡鍐硾閸ㄥ崬顕欓搹瑙勭暠闁挎稒鑹剧粩椋庝沪閳ь剚绋婄€ｎ亜顤呴柨娑樼墔缁躲儲淇?REPL 闂佹彃鏈惀鍛崉閹达絿绠栧☉鎾亾閻㈩垎宥囩闁告瑯鍨甸崗妯绘交濡粯笑 null
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

            // 闁告鍠撴晶?InfestedChameleonGirlScene 闁?TimeAwareInterface闁挎稒淇洪棅鈺傜▔婵犲啯绠掑Σ灞绢殜婵傛梻鈧潧瀚弫鎾绘惞椤愶絾顦ф慨锝呯箰閻剟寮懜闈涜吂閺夆晜绋愮粩鏉戔枎?
            if (CoC.Instance?.Player?.HasStatusEffect(
                    Contents.Datas.Vanilla.StatusEffectIds.ParasiteEel) == true)

                SceneInstance<

                    Contents.Scenes.Vanilla.Areas.Bog.InfestedChameleonGirlScene>.Get().ParasiteUpdate();

            // NPC 闁煎浜滅换渚€鎯冮崟顐创濠电偟濯寸槐娆撳储閻斿搫顣兼慨锝呯箣闁?NPC 闁?PregnancyStore.timeChange闁?
            Contents.Npcs.NpcPregnancy.Tick();

            // 闁告鍠撴晶?TimeAware 婵炲弶鍎宠ぐ鍌炴晬閸︽entParser.hx:235 / :268-274闁挎稑顧€缁?
            // TamanisDaughters / Kelly 闁?TimeAwareRegistry.ReloadVanilla 闂佹彃鏈弫鐐哄礃鐏炵儵鍋?
            // 闁哄牜浜滈悿鍕偝?timeChangeLarge 閺夆晜鏌ㄥú?true 闁告艾娴峰▓鎴﹀Υ鐏炵偓鐣柛瀣矌閻?Next闁靛棗绉瑰Σ锕傚礆濡ゅ绀勯柛妯煎枔婢?EventParser.hx:268-274闁挎稑顧€缁?
            // 閺夆晜鏌ㄥú鏍磹閻撳骸甯ラ柣?TimeAware.cs 婵懓娲﹂埀顒佺湽閳?
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

