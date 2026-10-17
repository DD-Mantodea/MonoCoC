using MonoCoC.Assets;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.World;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using MonoCoC.UI.Layout;
using Microsoft.Xna.Framework;
using System;
using static MonoCoC.UI.Layout.LayoutConstants;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Framework;
using MonoCoC.Contents.Scenes;

namespace MonoCoC.UI.Components.Views
{
    public class GameView : View
    {
        private readonly System.Collections.Generic.List<Button> _topMenuButtons = [];

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var scene = Scene.Current;

            var hide = scene is MonoCoC.Contents.Scenes.Vanilla.Options

                or MonoCoC.Contents.Scenes.Vanilla.SaveLoad

                or MonoCoC.Contents.Scenes.Vanilla.AchievementsScene

                or MonoCoC.Contents.Scenes.Vanilla.CreditsScene

                or MonoCoC.Contents.Scenes.Vanilla.InstructionsScene

                or MonoCoC.Contents.Scenes.Vanilla.Places.PlacesScene

                or MonoCoC.Contents.Scenes.Vanilla.CharCreation;

            var player = MonoCoC.CoC.Instance.Player;

            for (var index = 0; index < _topMenuButtons.Count; index++)
            {
                var button = _topMenuButtons[index];

                button.Visible = !hide;

                // 原版 MENU_LEVEL 只在可升级/可加属性/可买专长时显示
                if (index == 3)
                    button.Visible = !hide && player != null

                        && (player.CanLevelUp() || player.CanBuyStats() || player.CanBuyPerks());
            }
        }

        // 原版 MENU_STATS -> playerInfo.displayStats
        private static void OpenStats()
        {
            var previous = Scene.Current;

            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            Contents.Scenes.Vanilla.StatsScene.Show(() =>
            {
                // 从其他顶栏页面进入 Stats 时，Back 不应回到该顶栏页面
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

        // 游戏内顶栏 Data：不是主菜单的 Data，返回时应回游戏
        private static void OpenData()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            Contents.Scenes.Vanilla.SaveLoad.FromMainMenu = false;

            Scene.Go(Contents.Scenes.SceneInstance<Contents.Scenes.Vanilla.SaveLoad>.Instance);
        }

        public GameView()
        {
            Instance = this;

            Background = new UIImage(ManagerInstance<TextureManager>.Instance["background1"])
                .Size(ScreenWidth, ScreenHeight)
                .CanHit(false)
                .Of(this);

            // 原版 MainView 顶部菜单：Main Menu / Data / Stats / Perks / Appearance
            var topItems = new (string Key, System.Action Action)[]
            {
                ("Scenes.TopMenu.MainMenu", () => UIManager.Instance.Root.SwitchView(new MainView())),

                ("Scenes.TopMenu.Data", OpenData),

                ("Scenes.TopMenu.Stats", OpenStats),

                ("Scenes.TopMenu.LevelUp", () => Scene.Go(Contents.Scenes.SceneInstance<Contents.Scenes.Vanilla.LevelUpScene>.Instance)),

                ("Scenes.TopMenu.Perks", () => Scene.Go(Contents.Scenes.SceneInstance<Contents.Scenes.Vanilla.PerksScene>.Instance)),

                ("Scenes.TopMenu.Appearance", () => Scene.Go(Contents.Scenes.SceneInstance<Contents.Scenes.Vanilla.PlayerAppearance>.Instance)),
            };

            for (var index = 0; index < topItems.Length; index++)
            {
                var item = topItems[index];

                var button = new MonoCoC.UI.Components.Button("button0", item.Key.Localized())
                    .Size(160, 40)
                    .Position(index * 164, 5)
                    .Of(this);

                button.OnClick = item.Action;

                _topMenuButtons.Add(button);
            }

            PlayerStats = new PlayerStats()
                .Size(205, 602)
                .Position(0, TextZoneY)
                .Of(this);

            // 原版从主菜单进入设置时 hideStats()
            PlayerStats.Visible = !MonoCoC.Contents.Scenes.Vanilla.Options.HidePlayerStats;

            Output = new TextArea(UIFonts.Get("body"))
                .Size(TextZoneWidth, TextZoneHeight)
                .Position(TextZoneX, TextZoneY)
                .Of(this);

            Output.BackgroundColor = Color.White * 0.4f;

            Output.Padding = TextZonePadding;

            Output.AutoScroll = false;

            Time.HourPassed += OnHourPassed;

            // NewGame 走这里：清空小时派发注册表并重新注册（Load 在 SaveData.Apply 里做同样的事）。
            TimeAwareRegistry.ReloadVanilla();

            ButtonGrid = new Grid(BottomRows, BottomColumns)
                .Size(BottomWidth, BottomHeight)
                .Position(BottomX, BottomY)
                .Of(this);

            for (var index = 0; index < Buttons.Length; index++)
            {
                var button = NextButton()
                    .GridIndex(index)
                    .Size(ButtonWidth, ButtonHeight)
                    .LayoutType(LayoutType.Relative)
                    .Of(ButtonGrid);

                button.Visible = false;

                Buttons[index] = button;
            }

            Sprite = new UIImage(null)
                .Size(80, 80)
                .Position(4, 716)
                .CanHit(false)
                .Of(this);

            Sprite.Visible = false;

            Image = new UIImage(null)
                .CanHit(false)
                .Of(this);

            Image.Visible = false;

            // 地牢地图覆盖层（原版 DungeonTileView 的对应物），默认隐藏
            DungeonMap = new DungeonMapPanel()
                .CanHit(false)
                .Of(this);

            DungeonMap.Visible = false;

            // 常驻小地图（原版 MinimapView.minidungeonMap）：左下角，进地牢显示
            DungeonMiniMap = new DungeonMiniMapPanel()
                .Size(205, 132)
                .Position(0, 668)
                .CanHit(false)
                .Of(this);

            DungeonMiniMap.Visible = false;

            Input = new TextBox(UIFonts.Get("input"));

            Input.BackgroundColor = Color.White;

            Input.TextColor = Color.Black;

            Input.Cursor.CursorColor = Color.Black;

            Input
                .Size(InputWidth, InputHeight)
                .Position(TextZoneX + TextZonePadding, TextZoneY + TextZonePadding)
                .Of(this);

            Input.Visible = false;

            CombatStats = new CombatStats()
                .Size(CombatStatsWidth, CombatStatsHeight)
                .Position(CombatStatsX, CombatStatsY)
                .Of(this);

            CombatStats.Visible = false;

            MultiCombatStats = new MultiCombatStats()
                .Size(EnemyStatsPanel.PanelWidth, EnemyStatsPanel.PanelHeight * 3 + MultiCombatStats.Gap * 2)
                .Position(CombatStatsX, CombatStatsY)
                .Of(this);

            MultiCombatStats.Visible = false;

            // 选中目标指示三角：单独挂在 GameView 上，绝不参与敌人面板的布局
            _targetPointer = new UIImage(ManagerInstance<TextureManager>.Instance["selection"])
                .Size(20, 19)
                .Position(CombatStatsX + (EnemyStatsPanel.PanelWidth - 20) / 2, CombatStatsY + 4)
                .Of(this);

            _targetPointer.Visible = false;

            _targetPointer.Color = new Color(0x3D, 0x35, 0x2F);
        }

        public MultiCombatStats MultiCombatStats = null!;

        private readonly UIImage _targetPointer;

        public void ShowCombatStats(Enemy enemy)
        {
            _targetPointer.Visible = false;

            CombatStats.Show(enemy);

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        // 多敌人：右侧按顺序显示每个敌人面板，点击切换目标
        public void ShowCombatStats(System.Collections.Generic.IReadOnlyList<Enemy> enemies, int selected, Action<int> select)
        {
            CombatStats.Hide();

            MultiCombatStats.Refresh(enemies, selected, select);

            // 三角最上面距面板最上面 4px
            var panelTop = CombatStatsY + selected * (EnemyStatsPanel.PanelHeight + MultiCombatStats.Gap);

            _targetPointer.LayoutData.Position.X = CombatStatsX + (EnemyStatsPanel.PanelWidth - 20) / 2;
            // 三角上边距面板上边框 4px
            // 三角的「上边」在面板上边框上方 4px，三角向下覆盖到面板内部
            _targetPointer.LayoutData.Position.Y = panelTop - 4;

            _targetPointer.Visible = enemies.Count > 1 && selected >= 0 && selected < enemies.Count;

            Output.Size(CombatTextZoneWidth, TextZoneHeight);
        }

        public void HideCombatStats()
        {
            CombatStats.Hide();

            Output.Size(TextZoneWidth, TextZoneHeight);
        }

        // 原版 EventParser.gameOver 的界面部分：进入「已死亡」状态，
        // 把战斗/属性面板让位给居中的 GAME OVER 大字。
        public bool GameOverActive { get; private set; }

        public Label GameOverLabel;

        public void ShowGameOver(string title)
        {
            GameOverActive = true;

            CombatStats.Hide();

            StatsVisible = false;

            // GameOverLabel 是在布局时创建的：布局之前（例如 REPL 里没跑过一帧）可能还是 null
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

            StatsVisible = true;

            GameOverLabel?.Visible = false;
        }

        public static GameView? Instance;

        public Time Time = new();

        private void OnHourPassed(int hour)
        {
            CoC.Instance?.Player?.TickPregnancy();

            CorruptedGladeTime.OnHourPassed(hour);

            // 原版 InfestedChameleonGirlScene 是 TimeAwareInterface：身上有鳗鱼寄生虫时每小时推进一次
            if (CoC.Instance?.Player?.HasStatusEffect(
                    Contents.Datas.Vanilla.StatusEffectIds.ParasiteEel) == true)

                Contents.Scenes.SceneInstance<

                    Contents.Scenes.Vanilla.Areas.Bog.InfestedChameleonGirlScene>.Get().ParasiteUpdate();

            // NPC 自己的妊娠（原版每个 NPC 的 PregnancyStore.timeChange）
            Contents.Npcs.NpcPregnancy.Tick();

            // 原版 TimeAware 派发（EventParser.hx:235 / :268-274）：
            // TamanisDaughters / Kelly 在 TimeAwareRegistry.ReloadVanilla 里注册。
            // 未实现 timeChangeLarge 返回 true 后的「暂停等 Next」队列（原版 EventParser.hx:268-274），
            // 返回值先由 TimeAware.cs 汇总。
            _ = TimeAwareRegistry.DispatchHour(hour);
        }

        public static bool Ready => Instance != null;

        public static void SetPlayerStatsVisible(bool visible)
        {
            if (Instance != null)
                Instance.PlayerStats.Visible = visible;
        }

        public UIImage Background;

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
