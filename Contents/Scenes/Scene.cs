using MonoCoC.Assets;
using MonoCoC.Macros;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Scripting;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Framework;
using MonoCoC.Configuration;
using static MonoCoC.UI.Layout.LayoutConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;

namespace MonoCoC.Contents.Scenes
{
    public abstract class Scene : IIdentifiable
    {
        public static readonly ScriptContext ScriptContext = new();

        public static readonly Parser Parser = new(ScriptContext);

        public static readonly int[][] RowSlots = [[0, 1, 2, 3, 4], [5, 6, 7, 8, 9], [10, 11, 12, 13, 14]];

        public static readonly int[][] ColumnSlots = [[0, 5, 10], [1, 6, 11], [2, 7, 12], [3, 8, 13], [4, 9, 14]];

        public static SceneState State { get; } = new();

        public static IReadOnlyList<SceneAction> Actions => State.Actions;

        public static GameView View => GameView.Instance!;

        public static TextArea Output => View.Output;

        public static Button[] Buttons => View.Buttons;

        public static Player Player => CoC.Instance.Player;

        public static GameFlags Flags => CoC.Instance.Flags;

        public static SaveContent SaveContent(Identifier sceneId) => SceneSaveRegistry.For(sceneId);

        public static Enemies.Enemy? _currentMonster;

        public static Enemies.Enemy? CurrentMonster
        {
            get => _currentMonster ?? CombatController.Instance?.Context?.Enemy;

            set => _currentMonster = value;
        }

        static Scene()
        {
            ScriptContext.Resolver = ResolveTag;
        }

        public static ScriptValue? ResolveTag(string name) =>

            MacroRegistry.TryResolve(name, out var value) ? value : null;

        public static ScriptValue InvalidCockIndex(int rawIndex) =>

            ScriptValue.FromString($"<b>(Attempt To Parse an Invalid Cock {rawIndex})</b>");

        public static ScriptValue? MonsterTag(Func<Enemies.Enemy, string> selector)
        {
            var monster = CurrentMonster;

            return monster == null ? ScriptValue.FromString("") : ScriptValue.FromString(selector(monster));
        }

        public static bool InDungeon;

        public static bool InRoomedDungeon;

        public static string InRoomedDungeonName = "";

        public static Action? InRoomedDungeonResume;

        public static bool Debug;

        public static Scene? Current;

        public Identifier Id { get; set; }

        public static string KeySegment(string segment) =>

            string.Join("", segment.Split('_').Select(w => w.Length == 0 ? w : char.ToUpperInvariant(w[0]) + w[1..]));

        public string Name
        {
            get
            {
                var key = $"Scenes.{string.Join(".", Id.Path.Split('.').Select(KeySegment))}.Name";

                var value = key.Localized();

                return value == key ? Id.Path : value;
            }
        }

        public abstract void Entry();

        public static void AwardAchievement(Identifier achievementId, bool display = true, bool newlineBefore = false, bool newlineAfter = true)
        {
            var achievement = ManagerInstance<AchievementManager>.Instance.Find(achievementId)

                ?? throw new InvalidOperationException($"成就数据缺失: {achievementId}");

            if (!ManagerInstance<AchievementManager>.Instance.Unlock(achievement.Id))
                return;

            if (!display)
                return;

            if (newlineBefore)
                OutputText("Common.Text.Pg".Localized());

            OutputText("<font color=\"#000080\"><b>" +

                AchievementIds.UnlockedFormatKey.Localized() + " " + achievement.TitleKey.Localized() +

                "</b></font>");

            if (newlineAfter)
                OutputText("Common.Text.Pg".Localized());
        }

        public static void GameOver(bool clear = false)
        {
            if (clear)
                ClearOutput();

            ClearButtons();

            var line = $"GameOver.Title.{Random.Shared.Next(5)}";

            OutputText("[pg]<b>" + line.Localized() + "</b>");

            Flags.Set(GameFlagIds.TimesBadEnded, Flags.Get(GameFlagIds.TimesBadEnded) + 1);

            CombatController.Instance?.Context?.Enemies.Clear();

            CombatController.InCombat = false;

            CombatController.Instance = null;

            View.HideCombatStats();

            AwardAchievement(AchievementIds.GameOver, display: true, newlineBefore: false);

            if (OptionStore.Instance.Get("hardcore", 0) > 0)
            {
                OutputText("[pg]" + "GameOver.Hardcore.SaveDeleted".Localized());

                var hardcoreSlot = OptionStore.Instance.Get("hardcoreSlot", 0);

                if (hardcoreSlot > 0)
                    try
                    {
                        Saving.SaveManager.Delete("CoC_" + hardcoreSlot);
                    }
                    catch (Exception)
                    {

                    }

                Player.GameOver = true;
            }

            View.ShowGameOver("GAME OVER");

            AddButton(0, "GameOver.Button.Title".Localized(), BackToTitle,

                "GameOver.Button.TitleHint".Localized());

            AddButton(1, "GameOver.Button.Nightmare".Localized(), WakeFromBadEnd,

                "GameOver.Button.NightmareHint".Localized());
        }

        public static void BackToTitle()
        {
            View.HideGameOver();

            UIManager.Instance.Root.SwitchView(new MainView());
        }

        public static void WakeFromBadEnd()
        {
            View.HideGameOver();

            ClearOutput();

            ClearButtons();

            OutputText("GameOver.Nightmare.Can'tIt'sAllJust".Localized());

            OutputText("GameOver.Nightmare.YouWakeAndScream".Localized());

            if (Flags.Get(GameFlagIds.TimesBadEnded) >= 2)
                OutputText("GameOver.Nightmare.Again".Localized());

            if (OptionStore.Instance.Get("survival", 0) > 0)
                Player.Hunger = 40;

            if (OptionStore.Instance.Get("realistic", 0) > 0)
            {
                var limit = 18f + Player.Strength / 2f + Player.Tallness / 4f;

                if (Player.BallSize > limit)
                {
                    OutputText("GameOver.Nightmare.ShrinkBalls".Localized());

                    Player.BallSize = 14f + Player.Strength / 2f + Player.Tallness / 4f;
                }
            }

            Player.GameOver = false;

            Player.Health = Player.MaxHealth;

            Player.Fatigue = 0;

            Player.ClearCombatStatuses();

            var time = View.Time;

            time.Days += 1;

            time.Hours = CampScene.WakeupTime();

            time.Minutes = 0;

            var penalty = 1 + (int)(GameSettings.Difficulty * 0.5f);

            if (GameSettings.Difficulty <= -2 || Debug)
                penalty = 0;

            if (penalty > 0)
            {
                Player.Gems = Math.Max(0, Player.Gems - (int)(Player.Gems / 10f) * penalty);

                Player.Experience = Math.Max(0, Player.Experience - (int)(Player.Level * 10f) * penalty);

                if (Player.Strength > 20)
                    Player.Strength -= (float)Math.Ceiling(Player.Strength * 0.02f) * penalty;

                if (Player.Toughness > 20)
                    Player.Toughness -= (float)Math.Ceiling(Player.Toughness * 0.02f) * penalty;

                if (Player.Speed > 20)
                    Player.Speed -= (float)Math.Ceiling(Player.Speed * 0.02f) * penalty;

                if (Player.Intelligence > 20)
                    Player.Intelligence -= (float)Math.Ceiling(Player.Intelligence * 0.02f) * penalty;
            }

            Go<CampScene>();
        }

        protected virtual void ReturnToPrevious() =>

            Go<TelAdre>();

        public static string Cap(string text) =>

            string.IsNullOrEmpty(text) ? text : char.ToUpperInvariant(text[0]) + text[1..];

        public static MacroScope? _sceneTags;

        public virtual void OnLeave()
        {
        }

        public static void Go(Scene scene)
        {

            Current?.OnLeave();

            _sceneTags?.Dispose();

            _sceneTags = null;

            ScriptContext.Clear();

            Current = scene;

            _sceneTags = MacroRegistry.BeginScope(scene);

            GameView.Instance?.ApplySceneLayout(scene);

            scene.Entry();

            UIManager.Instance?.MarkSceneChanged();
        }

        public static void Go<T>() where T : Scene, new() => Go(SceneInstance<T>.Get());

        public static void PlayerMenu()
        {
            if (CombatController.InCombat)
            {
                CombatController.Instance?.ShowMenu();

                return;
            }

            if (DungeonSession.InDungeon && DungeonScene.ActiveContext != null)
            {
                DungeonScene.Resume();

                return;
            }

            Go<CampScene>();
        }

        public static void OutputText(string text) => Output.Append(Parser.Parse(text));

        public static void RawOutputText(string text) => Output.Append(text);

        public static void DisplayHeader(string text)
        {
            // 原版 Output.hx:115 formatHeader：36 号 + 下划线；这里用本项目括号标签
            OutputText("[size:36;georgia]" + text + "[sizeend]\n\n");
        }

        public static void ClearOutput(bool clearAll = true)
        {
            Output.Clear();

            Parser.Reset();

            if (clearAll)
            {
                FinishInput();

                ClearButtons();
            }
        }

        public static void ResetParsers(bool full = false)
        {
            Parser.Reset();

            if (full)
                ScriptContext.Clear();
        }

        public static void SetScopedMacro(string name, Func<ScriptValue?> factory)
        {
            if (_sceneTags != null)
            {
                _sceneTags.Set(name, factory);

                return;
            }

            var value = factory();

            if (value != null)
                ScriptContext.Set(name, value.Value);
        }

        public static void RegisterMacro(string tag, ScriptValue value) => SetScopedMacro(tag, () => value);

        public static void RegisterMacro(string name, Func<string> tagFactory) =>

            SetScopedMacro(name, () => ScriptValue.FromString(tagFactory()));

        public static void RegisterMacro(string tag, string value) => SetScopedMacro(tag, () => ScriptValue.FromString(value));

        public static void RegisterMacro(string tag, int value) => SetScopedMacro(tag, () => ScriptValue.FromInt(value));

        public static void RegisterMacro(string tag, float value) => SetScopedMacro(tag, () => ScriptValue.FromFloat(value));

        public static void RegisterMacro(string tag, bool value) => SetScopedMacro(tag, () => ScriptValue.FromBool(value));

        public static void ClearButtons(bool resetButton = true)
        {
            State.ClearActions();

            foreach (var button in Buttons)
                button.Reset(resetButton);
        }

        public static void HideMenus()
        {
            foreach (var button in Buttons)
                button.Reset(false);
        }

        public static Button? ButtonAt(int slot) => slot >= 0 && slot < Buttons.Length ? Buttons[slot] : null;

        public static Button? AddButton(int slot, string text, Action? onClick = null, string hint = "", string hintHeader = "")
        {
            var button = ButtonAt(slot);

            var action = State.SetAction(slot, new SceneAction
            {
                Label = text ?? "",
                OnClick = onClick,
                Enabled = onClick != null,
                Visible = true,
                Hint = hint,
                HintHeader = hintHeader,
            });

            if (button == null)
                return null;

            button.Label.Text(action.Label);

            button.OnClick = action.OnClick;

            button.Enabled = action.Enabled;

            button.Visible = true;

            button.Hint = action.Hint;

            button.HintHeader = action.HintHeader;

            button.RefreshVisual();

            return button;
        }

        public static Button? AddButtonDisabled(int slot, string text, string hint = "", string hintHeader = "")
        {
            var button = AddButton(slot, text, null, hint, hintHeader);

            var action = State.FindAction(slot);

            if (action is not null)
            {
                action.Enabled = false;

                action.OnClick = null;
            }

            if (button != null)
            {
                button.Enabled = false;

                button.RefreshVisual();
            }

            return button;
        }

        public static Button? AddNextButton(string text, Action? onClick = null, string hint = "") => AddButton(FirstFree(0, Buttons.Length - 1), text, onClick, hint);

        public static Button? AddNextButtonDisabled(string text, string hint = "", string hintHeader = "") =>

            AddButtonDisabled(FirstFree(0, Buttons.Length - 1), text, hint, hintHeader);

        protected static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        protected static bool Silly => silly;

        protected static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        protected static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        protected static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        protected static bool hyper => OptionStore.Instance.Get("hyper", 0) > 0;

        protected static bool noFur => !Player.HasFur();

        protected static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1f);

        // 原版 scenes/Camp.hx:155 returnToCamp(timeUsed)
        // 长途模式下，短时间返回营地会回到刚才的探索点（最多 LongHaulMaxExplorations 次）。
        public static void ReturnToCamp(int hours)
        {
            var exploration = SceneInstance<ExplorationScene>.Get();

            if (CampScene.LongHaulTimesExplored < CampScene.LongHaulMaxExplorations
                && hours < 2

                && exploration.currArea != null

                && Player.IsLongHaul())
            {
                ClearOutput();

                OutputText("Scenes.Camp.ReturnToCamp.LongHaul".Localized());

                CampScene.LongHaulTimesExplored += 1;

                exploration.currArea();

                return;
            }

            CampScene.LongHaulReset();

            ClearOutput();

            exploration.doneMermaidWalk = false;

            Player.RemoveStatusEffect(StatusEffectIds.MermaidWatered);

            Player.RemoveStatusEffect(StatusEffectIds.MermaidDehydrated);

            OutputText(hours == 1

                ? "Scenes.Camp.ReturnToCamp.HourPasses".Localized()

                : "Scenes.Camp.ReturnToCamp.HoursPass".Localized(Utils.Utils.Num2Text(hours)));

            HideMenus();

            View.Time.PassTime(hours);

            Go<CampScene>();
        }

        public const int AnyGender = -1;

        public static Button? SexButton(Button? button, Gender gender) => SexButton(button, (int)gender);

        public static Button? SexButton(Button? button, int gender)
        {
            if (button == null)
                return null;

            var player = CoC.Instance?.Player;

            if (player == null)
                return button;

            var ok = gender switch
            {
                AnyGender => !player.IsGenderless(),

                (int)Gender.Male => player.IsMale(),

                (int)Gender.Female => player.IsFemale(),

                (int)Gender.Hermaphrodite => player.IsHerm(),

                _ => true
            };

            if (!ok)
            {
                button.Enabled = false;

                button.RefreshVisual();
            }

            return button;
        }

        public static Button? AddLimitedButton(int[] allowedSlots, string text, Action? onClick = null)
        {
            foreach (var slot in allowedSlots)
            {
                if (ButtonAt(slot) is not { Visible: false })
                    continue;

                return AddButton(slot, text, onClick);
            }

            return null;
        }

        public static Button? AddRowButton(int row, string text, Action? onClick = null) => AddLimitedButton(RowSlots[row], text, onClick);

        public static Button? AddColButton(int column, string text, Action? onClick = null) => AddLimitedButton(ColumnSlots[column], text, onClick);

        public static Button? SetExitButton(string text, Action? onClick = null) => AddButton(Buttons.Length - 1, text, onClick);

        public static void HintButton(int slot, string hint)
        {
            if (ButtonAt(slot) is { } button)
                button.Hint = hint;
        }

        public static void RemoveButton(int slot)
        {
            var button = ButtonAt(slot);

            if (button == null)
                return;

            button.Visible = false;

            button.OnClick = null;
        }

        public static void DisableButton(int slot)
        {
            if (ButtonAt(slot) is { } button)
                button.Enabled = false;
        }

        public static void DisableButton(int slot, string reason)
        {
            if (ButtonAt(slot) is { } button)
            {
                button.Enabled = false;

                button.DisabledReason = reason;
            }
        }

        public static void DisableButtonByLabel(string label, string reason = "")
        {
            var button = Buttons.FirstOrDefault(b => b.Label?.RichTextLayout?.Text == label)

                         ?? Buttons.FirstOrDefault(b => b.Label?.RichTextLayout?.Text?.Contains(label) == true);

            if (button == null)
                return;

            button.Enabled = false;

            if (!string.IsNullOrEmpty(reason))
                button.DisabledReason = reason;
        }

        public static void DisableButtonIf(int slot, bool condition, string reason = "")
        {
            if (condition)
                DisableButton(slot, reason);
        }

        public static void HideButton(int slot)
        {
            if (ButtonAt(slot) is { } button)
                button.Visible = false;
        }

        public static void EnableButton(int slot)
        {
            if (ButtonAt(slot) is { } button)
            {
                button.Enabled = true;

                button.DisabledReason = "";
            }
        }

        public static void ClickButton(int slot)
        {
            ButtonAt(slot)?.OnClick?.Invoke();
        }

        public static void ClickButton(Button? button) => button?.OnClick?.Invoke();

        public static void DoNext(Action? next) => SetExitButton("Common.Button.Next".Localized(), next);

        public static void DoYesNo(Action yes, Action no)
        {
            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), yes);

            AddButton(1, "Common.Button.No".Localized(), no);
        }

        public static void ShowStats() => View.StatsVisible = true;

        public static void HideStats() => View.StatsVisible = false;

        public static void HideUpDown() => View.ArrowsVisible = false;

        public static void StatScreenRefresh()
        {
        }

        public static void GenericNamePrompt(string defaultName = "") => PromptInput(text: defaultName);

        public static void PromptInput(int? x = null, int? y = null, int? width = null, string text = "")
        {
            var area = Output.Rectangle;

            var measured = 0;

            try
            {
                if (Output.Text.RichTextLayout is { } rich && !string.IsNullOrEmpty(rich.Text))
                    measured = rich.Measure(Output.ViewWidth).Y;
            }
            catch (Exception)
            {
                measured = 0;
            }

            var inputX = x ?? area.X + Output.Padding.Left;

            var inputY = y ?? area.Y + Output.Padding.Top + measured + 3 - Output.ScrollOffset;

            View.Input.Size(width ?? InputWidth, InputHeight);

            View.Input.Position(inputX, inputY);

            View.Input.Text = text;

            View.Input.CursorTo(text.Length);

            View.Input.Visible = true;

            View.Input.Active = true;
        }

        public static string GetInput() => View.Input.Text;

        public static void SetInput(string text = "")
        {
            View.Input.Text = text;

            View.Input.CursorTo(text.Length);
        }

        public static string FinishInput()
        {
            var text = View.Input.Text;

            View.Input.Active = false;

            View.Input.Visible = false;

            return text;
        }

        public static void ClearInput() => View.Input.ClearText();

        public static void ResetInput()
        {
            ClearInput();

            View.Input.Visible = true;

            View.Input.Active = true;
        }

        public static void OnInputChanged(Action<string> handler) => View.Input.OnTextChanged = handler;

        public static void SpriteSelect(string? textureID)
        {
            var texture = textureID == null ? null : ManagerInstance<TextureManager>.Instance[textureID];

            View.Sprite.Texture = texture;

            View.Sprite.Visible = texture != null;

            if (texture != null)
                View.Sprite.Size(texture.Width, texture.Height);
        }

        public static void ShowImage(string id, int x = 0, int y = 0)
        {
            if (ManagerInstance<TextureManager>.Instance[id] != null)
                ImageSelect(id, x, y);
        }

        public static void ImageSelect(string? textureID, int x = 0, int y = 0)
        {
            var texture = textureID == null ? null : ManagerInstance<TextureManager>.Instance[textureID];

            View.Image.Texture = texture;

            View.Image.Position(x, y);

            View.Image.Visible = texture != null;

            if (texture != null)
                View.Image.Size(texture.Width, texture.Height);
        }

        public static int FirstFree(int from, int to)
        {
            for (var slot = from; slot <= to; slot++)
                if (ButtonAt(slot) is { Visible: false })
                    return slot;

            return -1;
        }
    }
}