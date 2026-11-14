using MonoCoC.Contents.Creation;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.UI.Animations;
using MonoCoC.UI.Styles;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace MonoCoC.UI.Components.Views
{
    public class CharCreationView : HjsonView
    {
        private enum Step
        {
            Identity,
            Body,
            Appearance,
            Origin,
            Experience,
            Modes,
            Summary
        }

        private readonly Component[] _panels = new Component[7];

        private readonly float[] _tabAnimation = new float[7];

        private Button[] _tabs = [];

        private TextBox _name = null!;

        private Label _summary = null!;

        private Label _errorLabel = null!;

        private SegmentedControl _difficultySelector = null!;

        private SegmentedControl _hardcoreSelector = null!;

        private TextBox _nameBox = null!;

        private SegmentedControl _skinSelector = null!;

        private SegmentedControl _hairSelector = null!;

        private NumericStepper _heightSelector = null!;

        private NumericStepper _cockSelector = null!;

        private SegmentedControl _breastSelector = null!;

        private NumericStepper _hairLengthSelector = null!;

        private SegmentedControl _beardStyleSelector = null!;

        private SegmentedControl _beardLengthSelector = null!;

        private ChoiceWheel _historyOptions = null!;

        private ChoiceWheel _endowmentOptions = null!;

        private Component _beardStyleLabel = null!;

        private Component _beardLengthLabel = null!;

        private Label _historyDetail = null!;

        private Label _endowmentDetail = null!;

        private ChoiceWheel _virginityOptions = null!;

        private Label _virginityDetail = null!;

        private DividerDecoration _historyDivider = null!;

        private DividerDecoration _endowmentDivider = null!;

        private DividerDecoration _virginityDivider = null!;

        private SegmentedControl _ageSelector = null!;

        private SegmentedControl _genderSelector = null!;

        private SegmentedControl _buildSelector = null!;

        private SegmentedControl _orientationSelector = null!;

        private Step _step = Step.Identity;

        private bool _started;

        private readonly AnimationPlayer _animations = new();

        public CharCreationDraft Draft { get; private set; } = new();

        protected override string ScreenFile => "Views\\CharCreationView.hjson";

        public CharCreationView()
        {
            Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _animations.Update(gameTime);

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (var index = 0; index < _tabs.Length; index++)
            {
                var target = index == (int)_step ? 1f : 0f;

                _tabAnimation[index] = MathHelper.Lerp(
                    _tabAnimation[index],
                    target,
                    MathHelper.Clamp(delta * 12f, 0f, 1f));

                ApplyTabVisual(_tabs[index], _tabAnimation[index]);
            }
        }

        public void EnsureStarted()
        {
            if (_started)
                return;

            _started = true;

            if (!CoC.Instance.Player.Loaded)
                CoC.Instance.NewPlayer();

            CoC.Instance.Player.CharCreation = true;

            Show(Step.Identity);
        }

        [UIRequired("charCreation.background")]
        private void BindBackground(Component part)
        {
        }

        [UIRequired("creation.tabs")]
        private void BindTabs(Component part) =>
            _tabs = part.LayoutData.Children.OfType<Button>().ToArray();

        [UIRequired("creation.panel.identity")]
        private void BindIdentity(Component part) => _panels[(int)Step.Identity] = part;

        [UIRequired("creation.panel.body")]
        private void BindBody(Component part) => _panels[(int)Step.Body] = part;

        [UIRequired("creation.panel.appearance")]
        private void BindAppearance(Component part) => _panels[(int)Step.Appearance] = part;

        [UIRequired("creation.panel.origin")]
        private void BindOrigin(Component part) => _panels[(int)Step.Origin] = part;

        [UIRequired("creation.panel.experience")]
        private void BindExperience(Component part) => _panels[(int)Step.Experience] = part;

        [UIRequired("creation.panel.modes")]
        private void BindModesPanel(Component part) => _panels[(int)Step.Modes] = part;

        [UIRequired("creation.panel.summary")]
        private void BindSummaryPanel(Component part) => _panels[(int)Step.Summary] = part;

        [UIRequired("creation.error")]
        private void BindError(Label part) => _errorLabel = part;

        [UIRequired("creation.difficulty")]
        private void BindDifficulty(SegmentedControl part)
        {
            _difficultySelector = part;

            _difficultySelector.ValueChanged = index =>
            {
                Draft.Difficulty = index - 2;
                _errorLabel.Text("");
            };
        }

        [UIRequired("creation.hardcore")]
        private void BindHardcore(SegmentedControl part)
        {
            _hardcoreSelector = part;

            _hardcoreSelector.ValueChanged = index =>
            {
                Draft.Hardcore = index == 1;
                _errorLabel.Text("");
            };
        }

        [UIRequired("creation.name")]
        private void BindName(TextBox part)
        {
            _name = part;

            _name.OnTextChanged = value => Draft.Name = value;
        }

        [UIRequired("creation.summary.text")]
        private void BindSummary(Label part) => _summary = part;

        [UIRequired("creation.age")]
        private void BindAgeSelector(SegmentedControl part)
        {
            _ageSelector = part;

            _ageSelector.ValueChanged = index =>
            {
                SetAge(index switch
                {
                    0 => Age.Child,
                    1 => Age.Teen,
                    3 => Age.Elder,
                    _ => Age.Adult
                });
            };
        }

        [UIRequired("creation.gender")]
        private void BindGenderSelector(SegmentedControl part)
        {
            _genderSelector = part;

            _genderSelector.ValueChanged = index =>
            {
                SetGender(index switch
                {
                    1 => Gender.Female,
                    2 => Gender.Hermaphrodite,
                    _ => Gender.Male
                });
            };

            RefreshGenderOptions();
        }

        private void RefreshGenderOptions()
        {
            if (_genderSelector is null)
                return;

            var unlocked = CoC.Instance.Flags.Get(GameFlagIds.HermUnlocked) > 0;

            var keys = unlocked
                ? new[] { "Views.CharCreation.Opt.Male", "Views.CharCreation.Opt.Female", "Views.CharCreation.Opt.Herm" }
                : ["Views.CharCreation.Opt.Male", "Views.CharCreation.Opt.Female"];

            _genderSelector.SetOptions(keys.Select(key => key.Localized()).ToArray(), Draft.Gender switch
            {
                Gender.Female => 1,
                Gender.Hermaphrodite when unlocked => 2,
                _ => 0
            });
        }

        [UIRequired("creation.build")]
        private void BindBuildSelector(SegmentedControl part)
        {
            _buildSelector = part;

            _buildSelector.ValueChanged = ApplyBuildSelection;
        }

        [UIRequired("creation.orientation")]
        private void BindOrientationSelector(SegmentedControl part)
        {
            _orientationSelector = part;

            _orientationSelector.ValueChanged = index =>
            {
                SetOrientation(index switch
                {
                    0 => 100,
                    1 => 0,
                    _ => 50
                });
            };
        }

        [UIRequired("creation.skin")]
        private void BindSkinSelector(SegmentedControl part)
        {
            _skinSelector = part;

            _skinSelector.ValueChanged = index =>
            {
                Draft.SkinTone = index switch
                {
                    1 => "fair",
                    2 => "olive",
                    3 => "dark",
                    4 => "ebony",
                    5 => "mahogany",
                    6 => "russet",
                    _ => "light"
                };

                Refresh();
            };
        }

        [UIRequired("creation.hair")]
        private void BindHairSelector(SegmentedControl part)
        {
            _hairSelector = part;

            _hairSelector.ValueChanged = index =>
            {
                Draft.HairColor = index switch
                {
                    1 => "brown",
                    2 => "black",
                    3 => "red",
                    4 => "gray",
                    5 => "white",
                    6 => "auburn",
                    _ => "blonde"
                };

                Refresh();
            };
        }

        [UIRequired("creation.height")]
        private void BindHeightSelector(NumericStepper part)
        {
            _heightSelector = part;

            Draft.Height = part.Value;

            _heightSelector.ValueChanged = value =>
            {
                Draft.Height = value;

                Refresh();
            };
        }

        [UIRequired("creation.cock")]
        private void BindCockSelector(NumericStepper part)
        {
            _cockSelector = part;

            Draft.CockLength = part.Value;

            _cockSelector.ValueChanged = value =>
            {
                Draft.CockLength = value;

                Refresh();
            };
        }

        [UIRequired("creation.breast")]
        private void BindBreastSelector(SegmentedControl part)
        {
            _breastSelector = part;

            _breastSelector.ValueChanged = index =>
            {
                Draft.BreastSize = index;

                Refresh();
            };
        }

        [UIRequired("creation.hairLength")]
        private void BindHairLengthSelector(NumericStepper part)
        {
            _hairLengthSelector = part;

            Draft.HairLength = part.Value;

            _hairLengthSelector.ValueChanged = value =>
            {
                Draft.HairLength = value;

                Refresh();
            };
        }

        [UIRequired("creation.beardStyle")]
        private void BindBeardStyleSelector(SegmentedControl part)
        {
            _beardStyleSelector = part;

            _beardStyleSelector.ValueChanged = index =>
            {
                Draft.BeardStyle = index;

                Refresh();
            };
        }

        [UIRequired("creation.beardLength")]
        private void BindBeardLengthSelector(SegmentedControl part)
        {
            _beardLengthSelector = part;

            _beardLengthSelector.ValueChanged = index =>
            {
                Draft.BeardLength = index;

                Refresh();
            };
        }

        [UIRequired("creation.history.normal")]
        private void BindHistoryOptions(ChoiceWheel part)
        {
            _historyOptions = part;

            _historyOptions.SelectionChanged = SetHistory;
        }

        [UIRequired("creation.endowment.options")]
        private void BindEndowmentOptions(ChoiceWheel part)
        {
            _endowmentOptions = part;

            _endowmentOptions.SelectionChanged = SetEndowment;

            _endowmentOptions.Available = condition => condition switch
            {
                "cock" => Draft.Gender is Gender.Male or Gender.Hermaphrodite,
                "vagina" => Draft.Gender is Gender.Female or Gender.Hermaphrodite,
                _ => true
            };
        }

        [UIRequired("creation.beardStyle.label")]
        private void BindBeardStyleLabel(Component part) => _beardStyleLabel = part;

        [UIRequired("creation.beardLength.label")]
        private void BindBeardLengthLabel(Component part) => _beardLengthLabel = part;

        [UIRequired("creation.history.detail")]
        private void BindHistoryDetail(Label part) => _historyDetail = part;

        [UIRequired("creation.endowment.detail")]
        private void BindEndowmentDetail(Label part) => _endowmentDetail = part;

        [UIRequired("creation.virginity")]
        private void BindVirginityOptions(ChoiceWheel part)
        {
            _virginityOptions = part;

            _virginityOptions.Available = condition => condition switch
            {
                "vagina" => Draft.Gender is Gender.Female or Gender.Hermaphrodite,
                "not_slut" => !(Draft.PendingHistory.Path == "coc:history_slut" &&
                                Draft.Gender == Gender.Female),
                _ => true
            };

            _virginityOptions.SelectionChanged = id =>
            {
                SetVirginity(id switch
                {
                    "lost_vaginal" => 1,
                    "lost_anal" => 2,
                    "lost_both" => 3,
                    _ => 0
                });

                if (_virginityDetail is not null)
                    _virginityDetail.Text(_virginityOptions.FormatDescription(_virginityOptions.SelectedDescriptionKey.Localized()));
            };
        }

        [UIRequired("creation.virginity.detail")]
        private void BindVirginityDetail(Label part) => _virginityDetail = part;

        [UIRequired("creation.history.divider")]
        private void BindHistoryDivider(DividerDecoration part) => _historyDivider = part;

        [UIRequired("creation.endowment.divider")]
        private void BindEndowmentDivider(DividerDecoration part) => _endowmentDivider = part;

        [UIRequired("creation.virginity.divider")]
        private void BindVirginityDivider(DividerDecoration part) => _virginityDivider = part;

        protected override void OnPartsBound()
        {
            _historyOptions.CurrentGender = Draft.Gender.ToString();
            _endowmentOptions.CurrentGender = Draft.Gender.ToString();
            _virginityOptions.CurrentGender = Draft.Gender.ToString();

            _historyOptions.CurrentAge = Draft.Age.ToString();
            _endowmentOptions.CurrentAge = Draft.Age.ToString();
            _virginityOptions.CurrentAge = Draft.Age.ToString();

            _historyOptions.PrewarmIcon();
            _endowmentOptions.PrewarmIcon();
            _virginityOptions.PrewarmIcon();

            Draft.Difficulty = MonoCoC.Configuration.OptionStore.Instance.Get("difficulty", 0);
            Draft.Hardcore = MonoCoC.Configuration.OptionStore.Instance.Get("hardcore", 0) > 0;

            _difficultySelector.SelectedIndex = Draft.Difficulty + 2;
            _hardcoreSelector.SelectedIndex = Draft.Hardcore ? 1 : 0;

            RefreshBuildOptions();

            RefreshBreastOptions();

            if (_historyOptions.SelectedId is { } history)
                SetHistory(history);

            if (_endowmentOptions.SelectedId is { } endowment)
                SetEndowment(endowment);

            Show(_lastStep);
        }

        [UIEvent("creation.tab.identity")]
        private void ShowIdentity() => Show(Step.Identity);

        [UIEvent("creation.tab.body")]
        private void ShowBody() => Show(Step.Body);

        [UIEvent("creation.tab.appearance")]
        private void ShowAppearance() => Show(Step.Appearance);

        [UIEvent("creation.tab.origin")]
        private void ShowOrigin() => Show(Step.Origin);

        [UIEvent("creation.tab.experience")]
        private void ShowExperience() => Show(Step.Experience);

        [UIEvent("creation.tab.modes")]
        private void ShowModes() => Show(Step.Modes);

        [UIEvent("creation.tab.summary")]
        private void ShowSummary() => Show(Step.Summary);

        [UIEvent("creation.nav.back")]
        private void Back()
        {
            if (_step > Step.Identity)
                Show(_step - 1);
        }

        [UIEvent("creation.nav.next")]
        private void Next()
        {
            if (_step < Step.Summary)
                Show(_step + 1);
        }

        private void ClearError()
        {
            _errorLabel.Text("");
            _errorLabel.LayoutData.Alpha = 1f;
            _animations.StopAll();
        }

        private void ShowError(string text)
        {
            _errorLabel.Text(text);
            _errorLabel.LayoutData.Alpha = 0f;

            _animations.StopAll();
            _animations.Play(new FloatAnimation(0f, 1f, value => _errorLabel.LayoutData.Alpha = value, 0.22f)
            {
                Easing = Easing.EaseOutCubic
            });
        }

        [UIEvent("creation.nav.confirm")]
        private void Confirm()
        {
            ClearError();

            if (string.IsNullOrWhiteSpace(Draft.Name))
            {
                Show(Step.Identity);

                ShowError("Views.CharCreation.NameRequired".Localized());

                _name.Active = true;

                return;
            }

            if (_historyOptions.SelectedLocked)
            {
                Show(Step.Origin);

                ShowError("Views.CharCreation.LockedConfirm".Localized());

                return;
            }

            if (Draft.PendingHistory.Path == "coc:history_slut" &&
                Draft.Gender == Gender.Female &&
                Draft.VirginityState == 0)
            {
                Show(Step.Origin);

                ShowError("Views.CharCreation.VirginityConflict".Localized());

                return;
            }

            if (_endowmentOptions.SelectedLocked || _virginityOptions.SelectedLocked)
            {
                Show(Step.Experience);

                ShowError("Views.CharCreation.LockedConfirm".Localized());

                return;
            }

            ApplyCharacter();

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<CampScene>.Get());
        }

        private void ApplyCharacter()
        {
            var player = CoC.Instance.Player;

            player.Level = 1;

            var rules = MonoCoC.Assets.ManagerInstance<MonoCoC.Assets.ContentManagers.CharCreationRulesManager>.Instance.Rules;

            var ageStats = rules.Ages[Draft.Age];

            var buildStats = rules.Builds[Draft.Build];

            var mods = buildStats.Mods(Draft.BuildGender)[Draft.Age];

            player.Strength = ageStats.Strength + buildStats.Strength;

            player.Toughness = ageStats.Toughness + buildStats.Toughness;

            player.Speed = ageStats.Speed + buildStats.Speed;

            player.Intelligence = ageStats.Intelligence;

            player.Sensitivity = ageStats.Sensitivity;

            player.Libido = ageStats.Libido;

            player.Lust = ageStats.Lust;

            var bonuses = rules.StatBonuses[Draft.Gender];

            for (var index = 0; index < ageStats.Bonus; index++)
            {
                var stat = bonuses[index % bonuses.Length];

                player.Stats.SetBase(stat, player.Stats.GetBase(stat) + 1f);
            }

            player.Body.Hips = mods.Hips;

            player.Body.Butt = mods.Butt;

            player.Femininity = mods.Femininity;

            player.Tone = (Draft.BuildGender == Gender.Female ? buildStats.SetToneFemale : buildStats.SetToneMale) ?? ageStats.Tone(Draft.Gender) + buildStats.Tone;

            player.Thickness = buildStats.Thickness;

            player.NippleLength = ageStats.NippleLength;

            player.Fertility = ageStats.Fertility(Draft.Gender);

            Draft.ApplyTo(player);

            player.Health = player.MaxHealth;

            player.SexOrientation = Draft.Orientation;

            if (Draft.VirginityState is 2 or 3)
            {
                player.LostVirginity = true;

                player.Anal.Virgin = false;

                player.Anal.Looseness += 1;
            }

            MonoCoC.Configuration.OptionStore.Instance.Set("difficulty", Draft.Difficulty);

            MonoCoC.Configuration.OptionStore.Instance.Set("hardcore", Draft.Hardcore ? 1 : 0);
        }

        [UIEvent("creation.age.child")]
        private void AgeChild() => SetAge(Age.Child);

        [UIEvent("creation.age.teen")]
        private void AgeTeen() => SetAge(Age.Teen);

        [UIEvent("creation.age.adult")]
        private void AgeAdult() => SetAge(Age.Adult);

        [UIEvent("creation.age.elder")]
        private void AgeElder() => SetAge(Age.Elder);

        [UIEvent("creation.gender.male")]
        private void GenderMale() => SetGender(Gender.Male);

        [UIEvent("creation.gender.female")]
        private void GenderFemale() => SetGender(Gender.Female);

        [UIEvent("creation.gender.herm")]
        private void GenderHerm() => SetGender(Gender.Hermaphrodite);

        [UIEvent("creation.build.small")]
        private void BuildSmall() => SetBuild(Build.Small);

        [UIEvent("creation.build.medium")]
        private void BuildMedium() => SetBuild(Build.Medium);

        [UIEvent("creation.build.large")]
        private void BuildLarge() => SetBuild(Build.Large);

        [UIEvent("creation.build.andro")]
        private void BuildAndro() => SetBuild(Build.Andro);

        [UIEvent("creation.orientation.male")]
        private void OrientationMale() => SetOrientation(100);

        [UIEvent("creation.orientation.female")]
        private void OrientationFemale() => SetOrientation(0);

        [UIEvent("creation.orientation.bi")]
        private void OrientationBi() => SetOrientation(50);

        [UIEvent("creation.skin.light")]
        private void SkinLight() => SetSkin("light");

        [UIEvent("creation.skin.fair")]
        private void SkinFair() => SetSkin("fair");

        [UIEvent("creation.skin.olive")]
        private void SkinOlive() => SetSkin("olive");

        [UIEvent("creation.skin.dark")]
        private void SkinDark() => SetSkin("dark");

        [UIEvent("creation.skin.ebony")]
        private void SkinEbony() => SetSkin("ebony");

        [UIEvent("creation.hair.blonde")]
        private void HairBlonde() => SetHair("blonde");

        [UIEvent("creation.hair.brown")]
        private void HairBrown() => SetHair("brown");

        [UIEvent("creation.hair.black")]
        private void HairBlack() => SetHair("black");

        [UIEvent("creation.hair.red")]
        private void HairRed() => SetHair("red");

        [UIEvent("creation.hair.white")]
        private void HairWhite() => SetHair("white");

        [UIEvent("creation.height.down")]
        private void HeightDown() => SetHeight(Draft.Height - 2);

        [UIEvent("creation.height.up")]
        private void HeightUp() => SetHeight(Draft.Height + 2);

        [UIEvent("creation.history.confirm")]
        private void ConfirmHistorySelection()
        {
            if (Draft.PendingHistory == default)
                return;

            if (_historyOptions.SelectedLocked)
            {
                _historyDetail.Text("Views.CharCreation.LockedConfirm".Localized());

                return;
            }

            Draft.History = Draft.PendingHistory;

            Refresh();
        }

        [UIEvent("creation.endowment.confirm")]
        private void ConfirmEndowmentSelection()
        {
            if (Draft.PendingEndowment == default)
                return;

            if (_endowmentOptions.SelectedLocked)
            {
                _endowmentDetail.Text("Views.CharCreation.LockedConfirm".Localized());

                return;
            }

            Draft.Endowment = Draft.PendingEndowment;

            Refresh();
        }

        [UIEvent("creation.history.alchemist")]
        private void HistoryAlchemist() => SetHistory("coc:history_alchemist");

        [UIEvent("creation.history.fortune")]
        private void HistoryFortune() => SetHistory("coc:history_fortune");

        [UIEvent("creation.history.healer")]
        private void HistoryHealer() => SetHistory("coc:history_healer");

        [UIEvent("creation.history.religious")]
        private void HistoryReligious() => SetHistory("coc:history_religious");

        [UIEvent("creation.history.slacker")]
        private void HistorySlacker() => SetHistory("coc:history_slacker");

        [UIEvent("creation.history.thief")]
        private void HistoryThief() => SetHistory("coc:history_thief");

        [UIEvent("creation.history.slut")]
        private void HistorySlut() => SetHistory("coc:history_slut");

        [UIEvent("creation.history.smith")]
        private void HistorySmith() => SetHistory("coc:history_smith");

        [UIEvent("creation.history.whore")]
        private void HistoryWhore() => SetHistory("coc:history_whore");

        [UIEvent("creation.history.paladin")]
        private void HistoryPaladin() => SetHistory("coc:history_paladin");

        [UIEvent("creation.history.fighter")]
        private void HistoryFighter() => SetHistory("coc:history_fighter");

        [UIEvent("creation.history.scholar")]
        private void HistoryScholar() => SetHistory("coc:history_scholar");

        [UIEvent("creation.endowment.strong")]
        private void EndowmentStrong() => SetEndowment("coc:strong");

        [UIEvent("creation.endowment.tough")]
        private void EndowmentTough() => SetEndowment("coc:tough");

        [UIEvent("creation.endowment.fast")]
        private void EndowmentFast() => SetEndowment("coc:fast");

        [UIEvent("creation.endowment.smart")]
        private void EndowmentSmart() => SetEndowment("coc:smart");

        [UIEvent("creation.endowment.lusty")]
        private void EndowmentLusty() => SetEndowment("coc:lusty");

        [UIEvent("creation.endowment.sensitive")]
        private void EndowmentSensitive() => SetEndowment("coc:sensitive");

        [UIEvent("creation.endowment.frigid")]
        private void EndowmentFrigid() => SetEndowment("coc:frigid");

        [UIEvent("creation.endowment.big_cock")]
        private void EndowmentBigCock() => SetEndowment("coc:big_cock");

        [UIEvent("creation.endowment.messy")]
        private void EndowmentMessy() => SetEndowment("coc:messy_orgasms");

        [UIEvent("creation.endowment.big_tits")]
        private void EndowmentBigTits() => SetEndowment("coc:big_tits");

        [UIEvent("creation.endowment.big_clit")]
        private void EndowmentBigClit() => SetEndowment("coc:big_clit");

        [UIEvent("creation.endowment.fertile")]
        private void EndowmentFertile() => SetEndowment("coc:fertile");

        [UIEvent("creation.endowment.wet_pussy")]
        private void EndowmentWetPussy() => SetEndowment("coc:wet_pussy");

        [UIEvent("creation.virgin.vaginal")]
        private void VirginVaginal() => SetVirginity(1);

        [UIEvent("creation.virgin.anal")]
        private void VirginAnal() => SetVirginity(2);

        [UIEvent("creation.virgin.both")]
        private void VirginBoth() => SetVirginity(3);

        [UIEvent("creation.virgin.yes")]
        private void VirginYes() => SetVirginity(0);

        [UIEvent("creation.virgin.no")]
        private void VirginNo() => SetVirginity(1);

        [UIEvent("creation.hardcore.toggle")]
        private void ToggleHardcore()
        {
            Draft.Hardcore = !Draft.Hardcore;

            Refresh();
        }

        private void SetAge(Age age)
        {
            Draft.Age = age;

            _historyOptions.CurrentAge = age.ToString();
            _endowmentOptions.CurrentAge = age.ToString();
            _virginityOptions.CurrentAge = age.ToString();

            Draft.Height = age switch
            {
                Age.Child => 130,
                Age.Teen => 160,
                Age.Elder => 168,
                _ => 170
            };

            RefreshBuildOptions();

            RefreshBreastOptions();

            Refresh();
        }

        private void SetGender(Gender gender)
        {
            Draft.Gender = gender;

            _historyOptions.CurrentGender = gender.ToString();
            _endowmentOptions.CurrentGender = gender.ToString();
            _virginityOptions.CurrentGender = gender.ToString();

            _endowmentOptions?.Refresh();

            RefreshBuildOptions();

            RefreshBreastOptions();

            Refresh();
        }

        private void ApplyBuildSelection(int index)
        {
            if (Draft.Gender == Gender.Hermaphrodite)
            {
                Draft.BuildGender = index < 4 ? Gender.Female : Gender.Male;

                Draft.Build = (index % 4) switch
                {
                    0 => Build.Small,
                    2 => Build.Large,
                    3 => Build.Andro,
                    _ => Build.Medium
                };
            }
            else
            {
                Draft.BuildGender = Draft.Gender;

                Draft.Build = index switch
                {
                    0 => Build.Small,
                    2 => Build.Large,
                    3 => Build.Andro,
                    _ => Build.Medium
                };
            }

            Refresh();
        }

        private void RefreshBuildOptions()
        {
            if (_buildSelector is null)
                return;

            string[] keys;

            if (Draft.Gender == Gender.Hermaphrodite)
            {
                keys =
                [
                    "Views.CharCreation.Build.FemSlender",
                    "Views.CharCreation.Build.FemAverage",
                    Draft.Age == Age.Child ? "Views.CharCreation.Build.FemChubby" : "Views.CharCreation.Build.FemCurvy",
                    Draft.Age == Age.Elder ? "Views.CharCreation.Build.FemMasculine" : "Views.CharCreation.Build.FemTomboy",
                    "Views.CharCreation.Build.MasLean",
                    "Views.CharCreation.Build.MasAverage",
                    Draft.Age == Age.Child ? "Views.CharCreation.Build.MasChubby" : "Views.CharCreation.Build.MasThick",
                    Draft.Age == Age.Elder ? "Views.CharCreation.Build.MasFeminine" : "Views.CharCreation.Build.MasGirly"
                ];
            }
            else
            {
                Draft.BuildGender = Draft.Gender;

                if (Draft.Gender == Gender.Female)
                {
                    keys =
                    [
                        "Views.CharCreation.Build.FemaleSlender",
                        "Views.CharCreation.Build.FemaleAverage",
                        Draft.Age == Age.Child ? "Views.CharCreation.Build.FemaleChubby" : "Views.CharCreation.Build.FemaleCurvy",
                        Draft.Age == Age.Elder ? "Views.CharCreation.Build.FemaleMasculine" : "Views.CharCreation.Build.FemaleTomboy"
                    ];
                }
                else
                {
                    keys =
                    [
                        "Views.CharCreation.Build.MaleLean",
                        "Views.CharCreation.Build.MaleAverage",
                        Draft.Age == Age.Child ? "Views.CharCreation.Build.MaleChubby" : "Views.CharCreation.Build.MaleThick",
                        Draft.Age == Age.Elder ? "Views.CharCreation.Build.MaleFeminine" : "Views.CharCreation.Build.MaleGirly"
                    ];
                }
            }

            var labels = keys.Select(key => key.Localized()).ToArray();

            var selected = Draft.Gender == Gender.Hermaphrodite
                ? ((Draft.BuildGender == Gender.Female ? 0 : 4) + Draft.Build switch
                {
                    Build.Small => 0,
                    Build.Large => 2,
                    Build.Andro => 3,
                    _ => 1
                })
                : Draft.Build switch
                {
                    Build.Small => 0,
                    Build.Large => 2,
                    Build.Andro => 3,
                    _ => 1
                };

            _buildSelector.SetOptions(labels, selected);
        }

        private void RefreshBreastOptions()
        {
            if (_breastSelector is null)
                return;

            var rules = MonoCoC.Assets.ManagerInstance<MonoCoC.Assets.ContentManagers.CharCreationRulesManager>.Instance.Rules;

            var mods = rules.Builds[Draft.Build].Mods(Draft.BuildGender)[Draft.Age];

            var keys = new System.Collections.Generic.List<string> { "Views.CharCreation.Opt.Flat" };

            if (mods.Femininity >= 35f) keys.Add("Views.CharCreation.Opt.ACup");
            if (mods.Femininity >= 40f) keys.Add("Views.CharCreation.Opt.BCup");
            if (mods.Femininity >= 50f) keys.Add("Views.CharCreation.Opt.CCup");
            if (mods.Femininity >= 60f) keys.Add("Views.CharCreation.Opt.DCup");
            if (mods.Femininity >= 70f) keys.Add("Views.CharCreation.Opt.DDCup");

            _breastSelector.SetOptions(keys.Select(key => key.Localized()).ToArray(), Math.Min(Draft.BreastSize, keys.Count - 1));
        }

        private void SetBuild(Build build)
        {
            Draft.Build = build;

            Refresh();
        }

        private void SetOrientation(int orientation)
        {
            Draft.Orientation = orientation;

            Refresh();
        }

        private void SetSkin(string skin)
        {
            Draft.SkinTone = skin;

            Refresh();
        }

        private void SetHair(string hair)
        {
            Draft.HairColor = hair;

            Refresh();
        }

        private void SetHeight(float height)
        {
            Draft.Height = Math.Clamp(height, 100, 240);

            Refresh();
        }

        private void SetHistory(string history)
        {
            Draft.PendingHistory = history;

            _virginityOptions.Refresh();

            UpdateHistoryDetail();

            Refresh();
        }

        private void SetEndowment(string endowment)
        {
            Draft.PendingEndowment = endowment;

            UpdateEndowmentDetail();

            Refresh();
        }

        private static void UpdateDivider(ChoiceWheel wheel, DividerDecoration divider)
        {
            if (divider is null)
                return;

            divider.OrnamentText = wheel.SelectedLocked ? "Locked" : "✦";
        }

        private void UpdateHistoryDetail()
        {
            if (_historyDetail is null)
                return;

            var key = _historyOptions.SelectedDescriptionKey;

            _historyDetail.Text(_historyOptions.FormatDescription(key.Localized()));
        }

        private void UpdateEndowmentDetail()
        {
            if (_endowmentDetail is null)
                return;

            var key = _endowmentOptions.SelectedDescriptionKey;

            _endowmentDetail.Text(_endowmentOptions.FormatDescription(key.Localized()));
        }

        private void SetVirginity(int state)
        {
            Draft.VirginityState = state;

            Draft.IsVirgin = state == 0;

            Refresh();
        }

        private void SetVirgin(bool virgin)
        {
            Draft.IsVirgin = virgin;

            Refresh();
        }

        private static Step _lastStep = Step.Identity;

        private void Show(Step step)
        {
            _lastStep = step;

            _step = step;

            for (var index = 0; index < _panels.Length; index++)
                if (_panels[index] is not null)
                    _panels[index].Visible = index == (int)step;

            Refresh();
        }

        private void Refresh()
        {
            if (_summary is null)
                return;

            var hasVagina = Draft.Gender is Gender.Female or Gender.Hermaphrodite;

            _virginityOptions.Refresh();

            var showBeard = Draft.Age != Age.Child && Draft.Gender != Gender.Female;

            _beardStyleLabel.Visible = showBeard;

            _beardStyleSelector.Visible = showBeard;

            _beardLengthLabel.Visible = showBeard;

            _beardLengthSelector.Visible = showBeard;

            _historyOptions.SetElder(Draft.Age == Age.Elder);

            _summary.Text(
                "Views.CharCreation.Summary.Name".Localized(Draft.Name) + "\n" +
                "Views.CharCreation.Summary.Age".Localized(Draft.Age) + "\n" +
                "Views.CharCreation.Summary.Gender".Localized(Draft.Gender) + "\n" +
                "Views.CharCreation.Summary.Build".Localized(Draft.Build) + "\n" +
                "Views.CharCreation.Summary.Height".Localized(FormatLength(Draft.Height)) + "\n" +
                "Views.CharCreation.Summary.Complexion".Localized(Draft.SkinTone) + "\n" +
                "Views.CharCreation.Summary.Hair".Localized(Draft.HairColor, FormatLength(Draft.HairLength)) + "\n" +
                "Views.CharCreation.Summary.CockLength".Localized(FormatLength(Draft.CockLength)) + "\n" +
                "Views.CharCreation.Summary.BreastSize".Localized(Draft.BreastSize) + "\n" +
                "Views.CharCreation.Summary.Virginity".Localized(Draft.VirginityState switch
                {
                    1 => "Views.CharCreation.Summary.LostVaginal".Localized(),
                    2 => "Views.CharCreation.Summary.LostAnal".Localized(),
                    3 => "Views.CharCreation.Summary.LostBoth".Localized(),
                    _ => "Views.CharCreation.Summary.Virgin".Localized()
                }) + "\n" +
                "Views.CharCreation.Summary.History".Localized(Draft.History == default
                    ? "Views.CharCreation.Summary.None".Localized()
                    : Draft.History.Path) + "\n" +
                "Views.CharCreation.Summary.Endowment".Localized(Draft.Endowment == default
                    ? "Views.CharCreation.Summary.None".Localized()
                    : Draft.Endowment.Path));
        }

        private static string FormatLength(float centimetres)
        {
            if (GameSettings.Metric)
                return $"{centimetres:0.#} " + "Views.CharCreation.Unit.Cm".Localized();

            return $"{centimetres / 2.54f:0.#} " + "Views.CharCreation.Unit.Inch".Localized();
        }

        private void ApplyTabVisual(Button button, float progress)
        {
            button.Selected = progress >= 0.5f;

            button.FontSizeOverride = (int)Math.Round(20 + 4 * progress);

            button.LayoutData.Position.X = 28 - 20 * progress;

            button.RefreshVisual();
        }
    }
}
