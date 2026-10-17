using MonoCoC.Assets;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static MonoCoC.UI.Layout.LayoutConstants;
using MonoCoC.Attributes;
using MonoCoC.Assets.ContentManagers;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<CharCreation>("coc:char_creation")]
    public class CharCreation : Scene
    {
        protected Gender Gender = Gender.Male;

        public static bool PendingAscension;

        public override void Entry()
        {
            if (PendingAscension)
            {
                PendingAscension = false;

                AscensionMenu();

                return;
            }

            NewGameGo();
        }

        protected virtual void NewGameGo()
        {
            Configuration.OptionStore.ResetLocalOptions();

            // 原版 CharCreation.hx:1623：新游戏直接标记为当前存档版本，避免进营地触发升级向导
            CoC.Instance.Flags.Set(GameFlagIds.ModSaveVersion, CampScene.CurrentSaveVersion);

            ClearOutput();

            OutputText("CharCreation.NewGameGo.YouGrewTheSmall".Localized());

            OutputText("CharCreation.NewGameGo.WhatYourName".Localized());

            ClearButtons();

            AddButton(0, "Common.OK".Localized(), () => ChooseName());

            GenericNamePrompt();

            if (Flags.Get(GameFlagIds.NewGamePlusLevel) == 0)
            {
                    Player.AutoSave = false;

                CoC.Instance.NewPlayer();
            }

            Player.CharCreation = true;

            ApplyAscensionReset();

            if (IsNewCharacter())
            {
                Player.Strength = 15;

                Player.Toughness = 15;

                Player.Speed = 15;

                Player.Intelligence = 15;

                Player.Sensitivity = 15;

                Player.Libido = 15;

                Player.Experience = Flags.Get(GameFlagIds.NewGamePlusBonusStoredXp);

                Player.Level = 1;

                Player.Gems = Flags.Get(GameFlagIds.NewGamePlusBonusStoredItems);
            }

            Player.Corruption = 0;

            Player.Hunger = 80;

            Player.Lust = 15;

            Player.Health = Player.MaxHealth;

            var hair = Player.Body.Get<HairPart>();

            hair.Length = 5;

            hair.Color = "brown";

            hair.Id = BodyPartTypes.Normal;

            var skin = Player.Body.Get<SkinPart>();

            skin.Id = BodyPartTypes.Plain;

            skin.DescKey = "Skin.Desc.skin";

            skin.Adj = "";

            skin.Tone = "light";

            Player.Body.Get<UnderBodyPart>().Restore();

            Player.Body.Get<NeckPart>().Restore();

            Player.Body.Get<RearBodyPart>().Restore();

            var lowerBody = Player.Body.Get<LowerBodyPart>();

            lowerBody.Id = BodyPartTypes.Human;

            lowerBody.LegCount = 2;

            Player.Body.Get<FacePart>().Id = BodyPartTypes.Human;

            var eyes = Player.Body.Get<EyesPart>();

            eyes.Count = 2;

            var tail = Player.Body.Get<TailPart>();

            if (IsNewCharacter())
                tail.Id = BodyPartTypes.None;

            Player.Body.Get<TonguePart>().Id = BodyPartTypes.Human;

            if (IsNewCharacter())
                Player.Femininity = 50;

            var beard = Player.Body.Get<BeardPart>();

            beard.Length = 0;

            beard.Id = BodyPartTypes.Normal;

            Player.Tone = 50;

            Player.Thickness = 50;

            Player.Body.Get<ArmsPart>().Claws.Tone = "";

            if (IsNewCharacter())
            {
                Player.Balls = 0;

                Player.BallSize = 0;
            }

            Player.HoursSinceCum = 0;

            Player.CumMultiplier = 1;

            Player.Anal.Looseness = 0;

            Player.Anal.Wetness = 0;

            Player.Anal.Fullness = 0;

            Player.Fertility = 5;

            Player.Fatigue = 0;

            var horns = Player.Body.Get<HornsPart>();

            if (IsNewCharacter())
            {
                horns.Value = 0;

                Player.Tallness = 60;

                tail.Venom = 0;

                tail.Recharge = 0;
            }
            else if (tail.Id != BodyPartTypes.Fox && tail.Id != BodyPartTypes.SpiderAbdomen && tail.Id != BodyPartTypes.BeeAbdomen && tail.Id != BodyPartTypes.Scorpion)
            {
                tail.Venom = 0;

                tail.Recharge = 0;
            }

            Player.Body.Get<WingsPart>().Id = BodyPartTypes.None;

            if (eyes.Id == BodyPartTypes.Basilisk)
                eyes.Id = BodyPartTypes.Lizard;

            View.Time.Days = 0;

            View.Time.Hours = 0;
        }

        protected virtual bool IsNewCharacter() => Flags.Get(GameFlagIds.NewGamePlusLevel) == 0;

        protected virtual void ChooseName(bool retry = false)
        {
            var input = GetInput().Trim();

            if (input.Length == 0)
            {
                if (!retry)
                {
                    OutputText("CharCreation.ChooseName.YouMustSelectName".Localized());

                    AddButton(0, "Common.OK".Localized(), () => ChooseName(true));
                }

                return;
            }

            ClearOutput();

            Player.ShortName = input.Length > InputNameMaxChars ? input[..InputNameMaxChars] : input;

            if (_reincarnating)
            {
                _reincarnating = false;

                ClearButtons();

                OutputText("CharCreation.Reincarnate.AfterLookingAroundThe".Localized());

                DoNext(RouteToGenderChoiceReincarnation);

                return;
            }

            if (Flags.Get(GameFlagIds.LethiceDefeated) > 0)
            {
                OutputText("CharCreation.ChooseName.YouShallKnownNow".Localized());

                AgeChoice();

                return;
            }

            ClearButtons();

            AgeChoice();
        }

        protected virtual void AgeChoice()
        {
            ClearOutput();

            OutputText("CharCreation.AgeChoice.HowOldAreYou".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.AgeChoice.Button1".Localized(), () => GenericGenderChoice(Age.Child));

            AddButton(1, "CharCreation.AgeChoice.Button2".Localized(), () => GenericGenderChoice(Age.Teen));

            AddButton(2, "CharCreation.AgeChoice.Button3".Localized(), () => GenericGenderChoice(Age.Adult));

            AddButton(3, "CharCreation.AgeChoice.Button4".Localized(), () => GenericGenderChoice(Age.Elder));

            SetExitButton("Common.Back".Localized(), NewGameGo);
        }

        protected virtual void GenericGenderChoice(Age age)
        {
            Player.Age = age;

            Player.StartingAge = age;

            ClearOutput();

            OutputText("CharCreation.GenericGenderChoice.AreYou".Localized());

            ClearButtons();

            AddButton(0, $"CharCreation.GenericGenderChoice.{(Player.IsChild() ? "Boy" : "Man")}".Localized(), IsAMan);

            AddButton(1, $"CharCreation.GenericGenderChoice.{(Player.IsChild() ? "Girl" : "Woman")}".Localized(), IsAWoman);

            if (Flags.Get(GameFlagIds.HermUnlocked) > 0)
            {
                OutputText("CharCreation.GenericGenderChoice.HermaphroditeYou'veUnlockedHermaphrodite".Localized());

                AddButton(2, "CharCreation.GenericGenderChoice.Button1".Localized(), IsAHerm);
            }

            SetExitButton("Common.Back".Localized(), AgeChoice);
        }

        protected virtual void IsAMan()
        {
            Gender = Gender.Male;

            ClearOutput();

            var suffix = Player.Age switch
            {
                Age.Child => "TextChild",

                Age.Teen => "TextTeen",

                Age.Elder => "TextElder",

                _ => "TextAdult"
            };

            OutputText($"CharCreation.IsAMan.{suffix}".Localized());

            OutputText("CharCreation.IsAMan.WhatTypeBuildYou".Localized());

            ClearButtons();

            var thick = Player.Age == Age.Child ? "CharCreation.IsAMan.Button3Child" : "CharCreation.IsAMan.Button3";

            var girly = Player.Age == Age.Elder ? "CharCreation.IsAMan.Button4Elder" : "CharCreation.IsAMan.Button4";

            AddButton(0, "CharCreation.IsAMan.Button1".Localized(), () => SelectBuild(Build.Small, Gender.Male));

            AddButton(1, "CharCreation.IsAMan.Button2".Localized(), () => SelectBuild(Build.Medium, Gender.Male));

            AddButton(2, thick.Localized(), () => SelectBuild(Build.Large, Gender.Male));

            AddButton(3, girly.Localized(), () => SelectBuild(Build.Andro, Gender.Male));

            SetExitButton("Common.Back".Localized(), () => GenericGenderChoice(Player.Age));
        }

        protected virtual void IsAWoman()
        {
            Gender = Gender.Female;

            ClearOutput();

            var suffix = Player.Age switch
            {
                Age.Child => "TextChild",

                Age.Teen => "TextTeen",

                Age.Elder => "TextElder",

                _ => "TextAdult"
            };

            OutputText($"CharCreation.IsAWoman.{suffix}".Localized());

            OutputText("CharCreation.IsAWoman.WhatTypeBuildYou".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.IsAWoman.Button1".Localized(), () => SelectBuild(Build.Small, Gender.Female));

            AddButton(1, "CharCreation.IsAWoman.Button2".Localized(), () => SelectBuild(Build.Medium, Gender.Female));

            AddButton(2, "CharCreation.IsAWoman.Button3".Localized(), () => SelectBuild(Build.Large, Gender.Female));

            AddButton(3, "CharCreation.IsAWoman.Button4".Localized(), () => SelectBuild(Build.Andro, Gender.Female));

            SetExitButton("Common.Back".Localized(), () => GenericGenderChoice(Player.Age));
        }

        protected virtual void IsAHerm()
        {
            Gender = Gender.Hermaphrodite;

            ClearOutput();

            var suffix = Player.Age switch
            {
                Age.Child => "TextChild",

                Age.Teen => "TextTeen",

                Age.Elder => "TextElder",

                _ => "TextAdult"
            };

            OutputText($"CharCreation.IsAHerm.{suffix}".Localized());

            OutputText("CharCreation.IsAHerm.WhatTypeBuildYou".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.IsAHerm.Button1".Localized(), () => SelectBuild(Build.Small, Gender.Female));

            AddButton(1, "CharCreation.IsAHerm.Button2".Localized(), () => SelectBuild(Build.Medium, Gender.Female));

            AddButton(2, "CharCreation.IsAHerm.Button3".Localized(), () => SelectBuild(Build.Large, Gender.Female));

            AddButton(3, "CharCreation.IsAHerm.Button4".Localized(), () => SelectBuild(Build.Andro, Gender.Female));

            AddButton(5, "CharCreation.IsAHerm.Button5".Localized(), () => SelectBuild(Build.Small, Gender.Male));

            AddButton(6, "CharCreation.IsAHerm.Button6".Localized(), () => SelectBuild(Build.Medium, Gender.Male));

            AddButton(7, "CharCreation.IsAHerm.Button7".Localized(), () => SelectBuild(Build.Large, Gender.Male));

            AddButton(8, "CharCreation.IsAHerm.Button8".Localized(), () => SelectBuild(Build.Andro, Gender.Male));

            SetExitButton("Common.Back".Localized(), () => GenericGenderChoice(Player.Age));
        }

        protected virtual void SelectBuild(Build build, Gender buildGender) => ApplyStats(Gender, Player.Age, build, buildGender);

        protected virtual void ApplyStats(Gender gender, Age age, Build build, Gender buildGender)
        {
            var rules = ManagerInstance<CharCreationRulesManager>.Instance.Rules;

            var ageStats = rules.Ages[age];

            var buildStats = rules.Builds[build];

            var mods = buildStats.Mods(buildGender)[age];

            Player.Strength = ageStats.Strength + buildStats.Strength;

            Player.Toughness = ageStats.Toughness + buildStats.Toughness;

            Player.Speed = ageStats.Speed + buildStats.Speed;

            Player.Intelligence = ageStats.Intelligence;

            Player.Sensitivity = ageStats.Sensitivity;

            Player.Libido = ageStats.Libido;

            Player.Lust = ageStats.Lust;

            var bonuses = rules.StatBonuses[gender];

            for (var index = 0; index < ageStats.Bonus; index++)
            {
                var stat = bonuses[index % bonuses.Length];

                Player.Stats.SetBase(stat, Player.Stats.GetBase(stat) + 1f);
            }

            Player.ClearGender();

            if (gender == Gender.Male)
            {
                Player.Balls = 2;

                Player.BallSize = ageStats.BallSize;

                Player.CreateCock(ageStats.CockLength, ageStats.CockThickness);
            }
            else if (gender == Gender.Female)
            {
                Player.Balls = 0;

                Player.BallSize = 0;

                Player.CreateVagina();

                Player.SetClitLength(ageStats.ClitLength);
            }
            else
            {
                Player.Balls = 2;

                Player.BallSize = ageStats.BallSize;

                Player.CreateVagina();

                Player.SetClitLength(ageStats.ClitLength);

                Player.CreateCock(ageStats.CockLength, ageStats.CockThickness);
            }

            Player.Body.Get<HairPart>().Length = rules.DefaultHairLength[gender];

            if (Player.BreastRows.Count == 0)
                Player.CreateBreastRow(mods.Breasts);
            else
                Player.BreastRows[0].BreastRating = mods.Breasts;

            Player.Body.Hips = mods.Hips;

            Player.Body.Butt = mods.Butt;

            Player.Femininity = mods.Femininity;

            Player.Tone = (buildGender == Gender.Female ? buildStats.SetToneFemale : buildStats.SetToneMale) ?? ageStats.Tone(gender) + buildStats.Tone;

            Player.Thickness = buildStats.Thickness;

            Player.NippleLength = ageStats.NippleLength;

            Player.Tallness = ageStats.Tallness(gender);

            Player.Fertility = ageStats.Fertility(gender);

            SexualOrientation();
        }

        protected virtual void SexualOrientation()
        {
            ClearOutput();

            OutputText("CharCreation.SexualOrientation.WhatYourSexualOrientation".Localized());

            OutputText("CharCreation.SexualOrientation.TheMoreAttractedYou".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.SexualOrientation.Button1".Localized(), () => PickOrientation(100));

            AddButton(1, "CharCreation.SexualOrientation.Button2".Localized(), () => PickOrientation(0));

            AddButton(2, "CharCreation.SexualOrientation.Button3".Localized(), () => PickOrientation(50));

            SetExitButton("Common.Back".Localized(), () => GenericGenderChoice(Player.Age));
        }

        protected virtual void PickOrientation(int orientation)
        {
            Player.SexOrientation = orientation;

            ChooseComplexion();
        }

        protected virtual void ChooseComplexion()
        {
            ClearOutput();

            OutputText("CharCreation.ChooseComplexion.WhatYourComplexion".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.ChooseComplexion.Button1".Localized(), () => SetComplexion("light"));

            AddButton(1, "CharCreation.ChooseComplexion.Button2".Localized(), () => SetComplexion("fair"));

            AddButton(2, "CharCreation.ChooseComplexion.Button3".Localized(), () => SetComplexion("olive"));

            AddButton(3, "CharCreation.ChooseComplexion.Button4".Localized(), () => SetComplexion("dark"));

            AddButton(4, "CharCreation.ChooseComplexion.Button5".Localized(), () => SetComplexion("ebony"));

            AddButton(5, "CharCreation.ChooseComplexion.Button6".Localized(), () => SetComplexion("mahogany"));

            AddButton(6, "CharCreation.ChooseComplexion.Button7".Localized(), () => SetComplexion("russet"));

            SetExitButton("Common.Back".Localized(), SexualOrientation);
        }

        protected virtual void SetComplexion(string choice)
        {
            Player.Body.Get<SkinPart>().Tone = choice;

            Player.Body.Get<ArmsPart>().Claws.Tone = "";

            ClearOutput();

            OutputText("CharCreation.SetComplexion.WhatColorYourHair".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.SetComplexion.Button1".Localized(), () => SetHair("blonde"));

            AddButton(1, "CharCreation.SetComplexion.Button2".Localized(), () => SetHair("brown"));

            AddButton(2, "CharCreation.SetComplexion.Button3".Localized(), () => SetHair("black"));

            AddButton(3, "CharCreation.SetComplexion.Button4".Localized(), () => SetHair("red"));

            AddButton(4, "CharCreation.SetComplexion.Button5".Localized(), () => SetHair("gray"));

            AddButton(5, "CharCreation.SetComplexion.Button6".Localized(), () => SetHair("white"));

            AddButton(6, "CharCreation.SetComplexion.Button7".Localized(), () => SetHair("auburn"));

            SetExitButton("Common.Back".Localized(), ChooseComplexion);
        }

        protected virtual void SetHair(string choice)
        {
            Player.Body.Get<HairPart>().Color = choice;

            GenericStyleCustomizeMenu();
        }

        protected virtual void GenericStyleCustomizeMenu()
        {
            ClearOutput();

            OutputText("CharCreation.GenericStyleCustomizeMenu.YouCanFinalizeYour".Localized());

            RawOutputText("CharCreation.GenericStyleCustomizeMenu.Height".Localized(MeasurementsUtils.BriefHeight(Player.Tallness)));

            RawOutputText("CharCreation.GenericStyleCustomizeMenu.SkinTone".Localized(Player.Body.Get<SkinPart>().Tone));

            RawOutputText("CharCreation.GenericStyleCustomizeMenu.HairColor".Localized(Player.Body.Get<HairPart>().Color));

            if (Player.HasCock())
                RawOutputText("CharCreation.GenericStyleCustomizeMenu.CockSizeLongThick".Localized(MeasurementsUtils.InchesOrCentimetres(Player.Cocks[0].Length), MeasurementsUtils.InchesOrCentimetres(Player.Cocks[0].Thickness)));

            RawOutputText("CharCreation.GenericStyleCustomizeMenu.BreastSize".Localized(Player.BreastCup(0)));

            ClearButtons();

            AddButton(0, "CharCreation.GenericStyleCustomizeMenu.Button1".Localized(), MenuSkinComplexion);

            AddButton(1, "CharCreation.GenericStyleCustomizeMenu.Button2".Localized(), MenuHairColor);

            AddButton(2, "CharCreation.GenericStyleCustomizeMenu.Button3".Localized(), MenuBeardSettings)
                .DisableIf(Player.Gendered("m", "f") == "f", "You're too feminine to grow a beard.")
                .DisableIf(Player.IsChild(), "You're too young to grow a beard.");

            AddButton(3, "CharCreation.GenericStyleCustomizeMenu.Button4".Localized(), SetHeight);

            AddMeasurementSystemToggle(GenericStyleCustomizeMenu, 4);

            AddButton(5, "CharCreation.GenericStyleCustomizeMenu.Button5".Localized(), MenuCockLength)
                .DisableIf(!Player.HasCock(), "You don't have a cock.");

            AddButton(6, "CharCreation.GenericStyleCustomizeMenu.Button6".Localized(), MenuBreastSize);

            AddButton(7, "CharCreation.GenericStyleCustomizeMenu.Button7".Localized(), SetHairLength);

            AddButton(9, "CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), ChooseEndowment);

            SetExitButton("Common.Back".Localized(), () => SetComplexion(Player.Body.Get<SkinPart>().Tone));
        }

        protected virtual void MenuSkinComplexion()
        {
            ClearOutput();

            OutputText("CharCreation.MenuSkinComplexion.WhatYourComplexion".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.MenuSkinComplexion.Button1".Localized(), () => ConfirmComplexion("light"));

            AddButton(1, "CharCreation.MenuSkinComplexion.Button2".Localized(), () => ConfirmComplexion("fair"));

            AddButton(2, "CharCreation.MenuSkinComplexion.Button3".Localized(), () => ConfirmComplexion("olive"));

            AddButton(3, "CharCreation.MenuSkinComplexion.Button4".Localized(), () => ConfirmComplexion("dark"));

            AddButton(4, "CharCreation.MenuSkinComplexion.Button5".Localized(), () => ConfirmComplexion("ebony"));

            AddButton(5, "CharCreation.MenuSkinComplexion.Button6".Localized(), () => ConfirmComplexion("mahogany"));

            AddButton(6, "CharCreation.MenuSkinComplexion.Button7".Localized(), () => ConfirmComplexion("russet"));

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void ConfirmComplexion(string complexion)
        {
            Player.Body.Get<SkinPart>().Tone = complexion;

            GenericStyleCustomizeMenu();
        }

        protected virtual void MenuHairColor()
        {
            ClearOutput();

            OutputText("CharCreation.MenuHairColor.WhatYourHairColor".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.MenuHairColor.Button1".Localized(), () => ChooseHairColor("blonde"));

            AddButton(1, "CharCreation.MenuHairColor.Button2".Localized(), () => ChooseHairColor("brown"));

            AddButton(2, "CharCreation.MenuHairColor.Button3".Localized(), () => ChooseHairColor("black"));

            AddButton(3, "CharCreation.MenuHairColor.Button4".Localized(), () => ChooseHairColor("red"));

            AddButton(4, "CharCreation.MenuHairColor.Button5".Localized(), () => ChooseHairColor("gray"));

            AddButton(5, "CharCreation.MenuHairColor.Button6".Localized(), () => ChooseHairColor("white"));

            AddButton(6, "CharCreation.MenuHairColor.Button7".Localized(), () => ChooseHairColor("auburn"));

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void ChooseHairColor(string color)
        {
            Player.Body.Get<HairPart>().Color = color;

            GenericStyleCustomizeMenu();
        }

        protected virtual void MenuBeardSettings()
        {
            ClearOutput();

            OutputText("CharCreation.MenuBeardSettings.YouCanChooseYour".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.MenuBeardSettings.Button1".Localized(), MenuBeardStyle);

            AddButton(1, "CharCreation.MenuBeardSettings.Button2".Localized(), MenuBeardLength);

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void MenuBeardStyle()
        {
            ClearOutput();

            OutputText("CharCreation.MenuBeardStyle.WhatBeardStyleWould".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.MenuBeardStyle.Button1".Localized(), () => ChooseBeardStyle("normal"));

            AddButton(1, "CharCreation.MenuBeardStyle.Button2".Localized(), () => ChooseBeardStyle("goatee"));

            AddButton(2, "CharCreation.MenuBeardStyle.Button3".Localized(), () => ChooseBeardStyle("cleancut"));

            AddButton(3, "CharCreation.MenuBeardStyle.Button4".Localized(), () => ChooseBeardStyle("mountainman"));

            SetExitButton("Common.Back".Localized(), MenuBeardSettings);
        }

        protected virtual void ChooseBeardStyle(string style)
        {
            var beard = Player.Body.Get<BeardPart>();

            beard.Id = new Identifier("coc", style);

            if (style == "cleancut")
                beard.Length = 0;
            else if (beard.Length <= 0)
                beard.Length = 1;

            MenuBeardSettings();
        }

        protected virtual void MenuBeardLength()
        {
            ClearOutput();

            OutputText("CharCreation.MenuBeardLength.HowLongWouldYou".Localized());

            OutputText("CharCreation.MenuBeardLength.NoteBeardWillSlowly".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.MenuBeardLength.Button1".Localized(), () => ChooseBeardLength(0));

            AddButton(1, "CharCreation.MenuBeardLength.Button2".Localized(), () => ChooseBeardLength(1));

            AddButton(2, "CharCreation.MenuBeardLength.Button3".Localized(), () => ChooseBeardLength(2));

            AddButton(3, "CharCreation.MenuBeardLength.Button4".Localized(), () => ChooseBeardLength(3));

            AddButton(4, "CharCreation.MenuBeardLength.Button5".Localized(), () => ChooseBeardLength(4));

            AddButton(5, "CharCreation.MenuBeardLength.Button6".Localized(), () => ChooseBeardLength(5));

            AddButton(6, "CharCreation.MenuBeardLength.Button7".Localized(), () => ChooseBeardLength(6));

            SetExitButton("Common.Back".Localized(), MenuBeardSettings);
        }

        protected virtual void ChooseBeardLength(float length)
        {
            var beard = Player.Body.Get<BeardPart>();

            beard.Length = length;

            if (length <= 0)
                beard.Id = BodyPartTypes.CleanCut;

            MenuBeardSettings();
        }

        protected virtual void AddMeasurementSystemToggle(Action returnTo, int position = 4)
        {
            var name = GameSettings.Metric ? "Common.UseImperial" : "Common.UseMetric";

            AddButton(position, name.Localized(), () =>
            {
                GameSettings.Metric = !GameSettings.Metric;

                returnTo();
            });
        }

        protected virtual void SetHeight()
        {
            ClearOutput();

            OutputText("CharCreation.SetHeight.SetYourHeight".Localized());

            OutputText("CharCreation.SetHeight.Text2".Localized());

            ClearButtons();

            PromptInput(text: GameSettings.Metric ? "170" : "67");

            AddButton(0, "Common.OK".Localized(), ConfirmHeight);

            AddMeasurementSystemToggle(SetHeight, 4);

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void ConfirmHeight()
        {
            if (!float.TryParse(GetInput(), out var value) || value <= 0)
            {
                OutputText("CharCreation.ConfirmHeight.PleaseInputYourHeight".Localized());

                return;
            }

            var inches = GameSettings.Metric ? value / 2.54f : value;

            if (inches < 42f)
            {
                OutputText("CharCreation.ConfirmHeight.ThatBelowYourMinimum".Localized());

                return;
            }

            if (inches > 96f)
            {
                OutputText("CharCreation.ConfirmHeight.ThatAboveYourMaximum".Localized());

                return;
            }

            Player.Tallness = inches;

            GenericStyleCustomizeMenu();
        }

        protected virtual void SetHairLength()
        {
            ClearOutput();

            OutputText("CharCreation.SetHairLength.SetYourHairLength".Localized());

            OutputText("CharCreation.SetHairLength.Text2".Localized());

            ClearButtons();

            PromptInput(text: Player.Body.Get<HairPart>().Length.ToString("0.#"));

            AddButton(0, "Common.OK".Localized(), ConfirmHairLength);

            AddMeasurementSystemToggle(SetHairLength, 4);

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void ConfirmHairLength()
        {
            if (!float.TryParse(GetInput(), out var value) || value < 0)
            {
                OutputText("CharCreation.ConfirmHairLength.PleaseInputLength".Localized());

                return;
            }

            var inches = GameSettings.Metric ? value / 2.54f : value;

            if (inches > 96f)
            {
                OutputText("CharCreation.ConfirmHairLength.ThatAboveYourMaximum".Localized());

                return;
            }

            Player.Body.Get<HairPart>().Length = inches;

            GenericStyleCustomizeMenu();
        }

        protected virtual void MenuCockLength()
        {
            ClearOutput();

            OutputText("CharCreation.MenuCockLength.YouCanChooseCock".Localized());

            OutputText("CharCreation.MenuCockLength.CockTypeAndSize".Localized());

            ClearButtons();

            PromptInput(text: Player.Cocks[0].Length.ToString("0.#"));

            AddButton(0, "Common.OK".Localized(), ChooseCockLength);

            AddMeasurementSystemToggle(MenuCockLength, 4);

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void ChooseCockLength()
        {
            if (!float.TryParse(GetInput(), out var value) || value <= 0)
            {
                OutputText("CharCreation.MenuCockLength.CockTypeAndSize".Localized());

                return;
            }

            var inches = GameSettings.Metric ? value / 2.54f : value;

            if (inches < 1f || inches > 20f)
            {
                OutputText("CharCreation.MenuCockLength.CockTypeAndSize".Localized());

                return;
            }

            Player.Cocks[0].Length = inches;

            GenericStyleCustomizeMenu();
        }

        protected virtual void MenuBreastSize()
        {
            ClearOutput();

            OutputText("CharCreation.MenuBreastSize.YouCanChooseBreast".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.MenuBreastSize.Button1".Localized(), () => ChooseBreastSize(0));

            AddButton(1, "CharCreation.MenuBreastSize.Button2".Localized(), () => ChooseBreastSize(1));

            AddButton(2, "CharCreation.MenuBreastSize.Button3".Localized(), () => ChooseBreastSize(2));

            AddButton(3, "CharCreation.MenuBreastSize.Button4".Localized(), () => ChooseBreastSize(3));

            AddButton(4, "CharCreation.MenuBreastSize.Button5".Localized(), () => ChooseBreastSize(4));

            AddButton(5, "CharCreation.MenuBreastSize.Button6".Localized(), () => ChooseBreastSize(5));

            SetExitButton("Common.Back".Localized(), GenericStyleCustomizeMenu);
        }

        protected virtual void ChooseBreastSize(float size)
        {
            if (Player.BreastRows.Count == 0)
                Player.CreateBreastRow(size);
            else
                Player.BreastRows[0].BreastRating = size;

            GenericStyleCustomizeMenu();
        }

        protected virtual void ChooseEndowment()
        {
            ClearOutput();

            OutputText("CharCreation.ChooseEndowment.EveryPersonBornWith".Localized());

            ClearButtons();

            var choices = new List<(string Label, string Perk)>
            {
                ("CharCreation.ChooseEndowment.Button2", "strong"),

                ("CharCreation.ChooseEndowment.Button3", "tough"),

                ("CharCreation.ChooseEndowment.Button4", "fast"),

                ("CharCreation.ChooseEndowment.Button5", "smart"),

                ("CharCreation.ChooseEndowment.Button6", "lusty"),

                ("CharCreation.ChooseEndowment.Button7", "sensitive"),

                ("CharCreation.ChooseEndowment.Button8", "frigid")
            };

            if (Player.HasCock())
            {
                choices.Add(("CharCreation.ChooseEndowment.Button9", "big_cock"));

                choices.Add(("CharCreation.ChooseEndowment.Button10", "messy_orgasms"));
            }

            if (Player.HasVagina())
            {
                choices.Add(("CharCreation.ChooseEndowment.Button11", "big_tits"));

                choices.Add(("CharCreation.ChooseEndowment.Button12", "big_clit"));

                choices.Add(("CharCreation.ChooseEndowment.Button13", "fertile"));

                choices.Add(("CharCreation.ChooseEndowment.Button14", "wet_pussy"));
            }

            var owned = 0;

            var slot = 0;

            foreach (var (Label, Perk) in choices)
            {
                var perk = Perk;

                if (!Player.HasPerk(new Identifier("coc", perk)))
                    AddButton(slot++, Label.Localized(), () => ConfirmEndowment(perk));
                else
                {
                    AddButtonDisabled(slot++, Label.Localized(), "CharCreation.ChooseEndowment.Disabled".Localized());

                    owned++;
                }
            }

            if (owned >= 4)
                AddButton(14, "CharCreation.ChooseEndowment.Button1".Localized(), ChooseHistory);
        }

        protected virtual void ConfirmEndowment(string perk)
        {
            ClearOutput();

            switch (perk)
            {
                case "strong":
                    OutputText("CharCreation.ConfirmEndowment.AreYouStrongerThan".Localized());

                    OutputText("CharCreation.ConfirmEndowment.StrengthIncreasesYourCombat".Localized());

                    break;

                case "tough":
                    OutputText("CharCreation.ConfirmEndowment.AreYouUnusuallyTough".Localized());

                    OutputText("CharCreation.ConfirmEndowment.ToughnessGivesYouMore".Localized());

                    break;

                case "fast":
                    OutputText("CharCreation.ConfirmEndowment.AreYouVeryQuick".Localized());

                    OutputText("CharCreation.ConfirmEndowment.SpeedMakesEasierEscape".Localized());

                    break;

                case "smart":
                    OutputText("CharCreation.ConfirmEndowment.AreYouQuickLearner".Localized());

                    OutputText("CharCreation.ConfirmEndowment.IntellectCanHelpYou".Localized());

                    break;

                case "lusty":
                    OutputText("CharCreation.ConfirmEndowment.YouHaveUnusuallyHigh".Localized());

                    OutputText("CharCreation.ConfirmEndowment.LibidoAffectsHowQuickly".Localized());

                    break;

                case "sensitive":
                    OutputText("CharCreation.ConfirmEndowment.YourSkinUnusuallySensitive".Localized());

                    OutputText("CharCreation.ConfirmEndowment.SensitivityAffectsHowEasily".Localized());

                    break;

                case "frigid":
                    OutputText("CharCreation.ConfirmEndowment.AreYouLessReceptive".Localized());

                    OutputText("CharCreation.ConfirmEndowment.SensitivityAffectsHowEasilyTouches".Localized());

                    break;

                case "big_cock":
                    OutputText("CharCreation.ConfirmEndowment.YouHaveBigCock".Localized());

                    OutputText("CharCreation.ConfirmEndowment.BiggerCockWillMake".Localized());

                    break;

                case "messy_orgasms":
                    OutputText("CharCreation.ConfirmEndowment.AreYourOrgasmsParticularly".Localized());

                    OutputText("CharCreation.ConfirmEndowment.HigherCumMultiplierWill".Localized());

                    break;

                case "big_tits":
                    OutputText("CharCreation.ConfirmEndowment.AreYourBreastsBigger".Localized());

                    OutputText("CharCreation.ConfirmEndowment.LargerBreastsWillAllow".Localized());

                    break;

                case "big_clit":
                    OutputText("CharCreation.ConfirmEndowment.YouHaveBigClit".Localized());

                    OutputText("CharCreation.ConfirmEndowment.LargeEnoughClitMay".Localized());

                    break;

                case "fertile":
                    OutputText("CharCreation.ConfirmEndowment.YourFamilyParticularlyFertile".Localized());

                    OutputText("CharCreation.ConfirmEndowment.HighFertilityWillCause".Localized());

                    break;

                case "wet_pussy":
                    OutputText("CharCreation.ConfirmEndowment.DoesYourPussyGet".Localized());

                    OutputText("CharCreation.ConfirmEndowment.VaginalWetnessWillMake".Localized());

                    break;
            }

            ClearButtons();

            AddButton(0, "CharCreation.ConfirmEndowment.Button1".Localized(), () => SetEndowment(perk));

            AddButton(1, "CharCreation.ConfirmEndowment.Button2".Localized(), ChooseEndowment);
        }

        protected virtual void SetEndowment(string perk)
        {
            var amount = Player.IsChild() ? 3 : 5;

            switch (perk)
            {
                case "strong":
                    Player.Strength += amount;

                    Player.Tone += 7;

                    Player.Thickness += 3;

                    break;

                case "tough":
                    Player.Toughness += amount;

                    Player.Tone += 5;

                    Player.Thickness += 5;

                    Player.Health = Player.MaxHealth;

                    break;

                case "fast":
                    Player.Speed += amount;

                    Player.Tone += 10;

                    break;

                case "smart":
                    Player.Intelligence += amount;

                    Player.Thickness -= 5;

                    break;

                case "lusty":
                    Player.Libido += amount;

                    break;

                case "sensitive":
                    Player.Sensitivity += amount;

                    break;

                case "frigid":
                    Player.Sensitivity -= 5;

                    break;

                case "big_cock":
                    Player.Femininity -= 5;

                    if (Player.Cocks.Count > 0)
                    {
                        var cock = Player.Cocks[0];

                        cock.Length += 2;

                        cock.Thickness = MathF.Floor(((cock.Length / 5f) - 0.1f) * 10f) / 10f;
                    }

                    break;

                case "messy_orgasms":
                    Player.Femininity -= 2;

                    Player.CumMultiplier = 1.5f;

                    break;

                case "big_tits":
                    Player.Femininity += 5;

                    if (Player.BreastRows.Count > 0)
                        Player.BreastRows[0].BreastRating += Player.IsChild() ? 1 : 2;

                    break;

                case "big_clit":
                    Player.Femininity -= 5;

                    Player.SetClitLength(1);

                    break;

                case "fertile":
                    Player.Femininity += 5;

                    Player.Fertility += 25;

                    Player.Body.Hips += 2;

                    break;

                case "wet_pussy":
                    Player.Femininity += 7;

                    if (Player.Vaginas.Count > 0)
                        Player.Vaginas[0].Wetness = 2;

                    break;
            }

            Player.CreatePerk(new Identifier("coc", perk), PerkValue(perk));

            ChooseHistory();
        }

        public static float PerkValue(string perk) => perk switch
        {
            "big_cock" => 1.25f,

            "messy_orgasms" => 1.25f,

            "big_tits" => 1.5f,

            "big_clit" => 1.25f,

            "fertile" => 1.5f,

            "wet_pussy" => 2f,

            _ => 0f
        };

        protected virtual void ChooseHistory()
        {
            ClearOutput();

            if (Flags.Get(GameFlagIds.HistoryPerkSelected) == 2)
                OutputText("CharCreation.ChooseHistory.NewHistoryPerksAre".Localized());

            if (Player.Age == Age.Elder)
                OutputText("CharCreation.ChooseHistory.BeforeYouBecameChampion".Localized());
            else
                OutputText("CharCreation.ChooseHistory.BeforeYouBecameChampionYou".Localized());

            ClearButtons();

            var labels = Player.Age == Age.Elder

                ? new[] { "ElderButton1", "ElderButton2", "ElderButton3", "ElderButton4", "ElderButton5", "ElderButton6", "ElderButton7", "ElderButton8", "ElderButton9", "ElderButton10", "ElderButton11", "ElderButton12" }

                : ["Button2", "Button3", "Button4", "Button5", "Button6", "Button7", "Button8", "Button9", "Button10", "Button11", "Button12", "Button13"];

            var perks = new[] { "history_alchemist", "history_fighter", "history_fortune", "history_healer", "history_religious", "history_scholar", "history_slacker", "history_thief", "history_slut", "history_smith", "history_whore", "history_paladin" };

            var owned = 0;

            for (var index = 0; index < perks.Length; index++)
            {
                var perk = perks[index];

                var label = "CharCreation.ChooseHistory." + labels[index];

                if (!Player.HasPerk(new Identifier("coc", perk)))
                    AddButton(index, label.Localized(), () => ConfirmHistory(perk));
                else
                {
                    AddButtonDisabled(index, label.Localized(), "CharCreation.ChooseHistory.Disabled".Localized());

                    owned++;
                }
            }

            if (owned >= 1)
                AddButton(14, "CharCreation.ChooseHistory.Button1".Localized(), VirginPrompt);
        }

        protected virtual void ConfirmHistory(string perk)
        {
            ClearOutput();

            var elder = Player.Age == Age.Elder;

            var key = perk switch
            {
                "history_alchemist" => elder ? "Text1" : "Text2",

                "history_fighter" => elder ? "Text3" : "Text4",

                "history_fortune" => elder ? "Text5" : "Text6",

                "history_healer" => elder ? "Text7" : "Text8",

                "history_religious" => elder ? "Text9" : "Text10",

                "history_scholar" => elder ? "Text11" : "Text24",

                "history_slacker" => "Text12",

                "history_thief" => elder ? "Text13" : "Text14",

                "history_slut" => elder ? "Text15" : "Text16",

                "history_smith" => elder ? "Text17" : "Text18",

                "history_whore" => elder ? "Text19" : "Text20",

                "history_paladin" => "Text21",

                _ => "Text23"
            };

            OutputText(("CharCreation.ConfirmHistory." + key).Localized());

            if (perk == "history_paladin")
                OutputText("CharCreation.ConfirmHistory.YouAreMoreResistant".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.ConfirmHistory.Button1".Localized(), () => SetHistory(perk));

            AddButton(1, "CharCreation.ConfirmHistory.Button2".Localized(), ChooseHistory);
        }

        protected virtual void SetHistory(string perk)
        {
            Player.CreatePerk(new Identifier("coc", perk));

            if (perk is "history_fighter" or "history_thief" or "history_whore")
                Player.Gems += 50;

            if (perk == "history_fortune")
                Player.Gems += 250;

            if (Player.Age == Age.Elder)
                GrantElderHistory(perk);

            Player.UpdateItemStacks();

            Flags.Set(GameFlagIds.HistoryPerkSelected, 1);

            VirginPrompt();
        }

        public static Item CreateItem(string id) => ItemRegistry.Create(new Identifier("coc", id));

        public static void GrantItem(string id, int quantity = 1) => Player.AddItem(CreateItem(id), quantity);

        public static void GrantBackpack(int capacity)
        {
            if (!Player.HasKeyItem("Backpack"))
            {
                Player.CreateKeyItem("Backpack", capacity);

                return;
            }

            var current = Player.KeyItemV1("Backpack");

            if (current < capacity)
                Player.AddKeyValue("Backpack", 1, capacity - current);
        }

        public static void GrantElderHistory(string perk)
        {
            switch (perk)
            {
                case "history_alchemist":
                    GrantBackpack(3);

                    GrantItem("healing_pill", 5);

                    Player.Intelligence += 20;

                    Player.Gems += 250;

                    break;

                case "history_fighter":
                    if (Player.Weapon.IsUnarmed())
                        Player.SetWeapon((Weapon)CreateItem("spear"));

                    if (Player.Shield == Shield.Nothing)
                        Player.SetShield((Shield)CreateItem("buckler"));

                    if (Player.Armor == Armor.Nothing || Player.Armor.Id == new Identifier("coc", "comfy_clothes"))
                        Player.SetArmor((Armor)CreateItem("leather_armor"));

                    Player.Strength += 30;

                    Player.Toughness += 20;

                    Player.Speed += 15;

                    Player.Gems += 150;

                    break;

                case "history_fortune":
                    GrantBackpack(1);

                    Player.Gems += 1500;

                    break;

                case "history_healer":
                    Player.AddStatusEffect(new StatusEffects.StatusEffect
                    {
                        Id = new Identifier("coc", "knows_heal")
                    });

                    Player.Toughness += 40;

                    Player.Gems += 250;

                    break;

                case "history_religious":
                    if (Player.Armor == Armor.Nothing || Player.Armor.Id == new Identifier("coc", "comfy_clothes"))
                        Player.SetArmor((Armor)CreateItem("modest_robes"));

                    Player.Strength += 10;

                    Player.Toughness += 10;

                    Player.Speed += 10;

                    Player.Intelligence += 5;

                    break;

                case "history_scholar":
                    GrantItem("white_book");

                    GrantItem("black_book");

                    Player.Intelligence += 40;

                    Player.Gems += 250;

                    break;

                case "history_slacker":
                    Player.Gems += 50;

                    break;

                case "history_thief":
                    if (Player.Weapon.IsUnarmed())
                        Player.SetWeapon((Weapon)CreateItem("dagger"));

                    if (Player.Armor == Armor.Nothing || Player.Armor.Id == new Identifier("coc", "comfy_clothes"))
                        Player.SetArmor((Armor)CreateItem("leather_pants"));

                    GrantBackpack(2);

                    Player.Speed += 30;

                    Player.Gems += 450;

                    break;

                case "history_slut":
                    // 原版 MasteryLib.hx:11：Tease 熟练度的 id 是 "Tease"（大写 T）；
                    // 小写写法会与 UrtaQuestScene 里的 "Tease" 分裂成两条互不相认的熟练度
                    Player.AddMastery(new Identifier("coc", "Tease"), 2);

                    Player.Toughness += 5;

                    Player.Speed += 10;

                    Player.Sensitivity += 15;

                    Player.Libido += 15;

                    break;

                case "history_smith":
                    if (Player.Weapon.IsUnarmed())
                        Player.SetWeapon((Weapon)CreateItem("mace"));

                    if (Player.Shield == Shield.Nothing)
                        Player.SetShield((Shield)CreateItem("buckler"));

                    if (Player.Armor == Armor.Nothing || Player.Armor.Id == new Identifier("coc", "comfy_clothes"))
                        Player.SetArmor((Armor)CreateItem("leather_armor"));

                    Player.Strength += 15;

                    Player.Toughness += 15;

                    Player.Gems += 250;

                    break;

                case "history_whore":
                    Player.AddMastery(new Identifier("coc", "Tease"), 3);

                    Player.Sensitivity += 10;

                    Player.Libido += 10;

                    Player.Gems += 750;

                    break;

                case "history_paladin":
                    if (Player.Weapon.IsUnarmed())
                        Player.SetWeapon((Weapon)CreateItem("mace"));

                    if (Player.Shield == Shield.Nothing)
                        Player.SetShield((Shield)CreateItem("kite_shield"));

                    if (Player.Armor == Armor.Nothing || Player.Armor.Id == new Identifier("coc", "comfy_clothes"))
                        Player.SetArmor((Armor)CreateItem("chainmail"));

                    Player.Strength += 20;

                    Player.Toughness += 20;

                    Player.AddMastery(new Identifier("coc", "shield"), 1);

                    break;
            }
        }

        protected virtual void VirginPrompt()
        {
            ClearOutput();

            OutputText("CharCreation.VirginPrompt.AreYouStillVirgin".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.VirginPrompt.Button1".Localized(), () => VirginChoice(true, true))
                .DisableIf(Player.HasPerk(PerkIds.HistorySlut) && !Player.HasCock(), "As slutty as you were, there's no way that's true.");

            AddButton(1, "CharCreation.VirginPrompt.Button2".Localized(), () => VirginChoice(false, true))
                .DisableIf(!Player.HasVagina(), "You don't have a vagina.");

            AddButton(2, "CharCreation.VirginPrompt.Button3".Localized(), () => VirginChoice(true, false));

            AddButton(3, "CharCreation.VirginPrompt.Button4".Localized(), () => VirginChoice(false, false))
                .DisableIf(!Player.HasVagina(), "You don't have a vagina.");
        }

        protected virtual void VirginChoice(bool vaginal, bool anal)
        {
            if (!vaginal && Player.HasVagina())
            {
                Player.LostVirginity = true;

                Player.Vaginas[0].Virgin = false;

                Player.Vaginas[0].Looseness += 1;
            }

            if (!anal)
            {
                Player.LostVirginity = true;

                Player.Anal.Virgin = false;

                Player.Anal.Looseness += 1;
            }

            CompleteCharacterCreation();
        }

        protected virtual void CompleteCharacterCreation()
        {
            ClearOutput();

            Player.Health = Player.MaxHealth;

            if (!IsNewCharacter())
            {
                StartTheGame();

                return;
            }

            Options.SelectPane(3);

            Options.OnStart = StartTheGame;

            Go<Options>();
        }

        protected virtual void Arrival()
        {
            ClearOutput();

            View.Time.Hours = 11;

            Player.Lust += 15;

            if (Player.IsChild())
            {
                OutputText("CharCreation.Arrival.YouArePreparedForWhat".Localized());

                OutputText("CharCreation.Arrival.TheWalkTheTaintedCave".Localized());

                OutputText("CharCreation.Arrival.TheEntranceTheCave".Localized());

                OutputText(Player.Gender == Gender.Female ? "CharCreation.Arrival.TheCaveUnusuallyWarmAnd".Localized() : "CharCreation.Arrival.TheCaveUnusuallyWarm".Localized());
            }
            else
            {
                OutputText("CharCreation.Arrival.YouArePreparedFor".Localized());

                OutputText("CharCreation.Arrival.TheWalkTheTainted".Localized());

                OutputText("CharCreation.Arrival.TheEntranceTheCaveGapes".Localized());

                OutputText("CharCreation.Arrival.TheCaveUnusuallyWarmAndDamp".Localized());

                OutputText(Player.Gender == Gender.Female ? "CharCreation.Arrival.AndYourBodySeems".Localized() : "CharCreation.Arrival.AndYourBodyReacts".Localized());

                OutputText("CharCreation.Arrival.YouWereWarnedThis".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.OK".Localized(), ArrivalPartTwo);
        }

        protected virtual void ArrivalPartTwo()
        {
            ClearOutput();

            View.Time.Hours = 18;

            Player.Lust += 40;

            Player.Corruption += 2;

            OutputText("CharCreation.ArrivalPartTwo.YouWakeWithSplitting".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("CharCreation.ArrivalPartTwo.TheUrgeChaseDown".Localized());
            else
                OutputText("CharCreation.ArrivalPartTwo.TheUrgeRamYour".Localized());

            OutputText("CharCreation.ArrivalPartTwo.TheImpSaysWouldn't".Localized());

            ClearButtons();

            AddButton(0, "Common.OK".Localized(), ArrivalPartThree);
        }

        protected virtual void ArrivalPartThree()
        {
            ClearOutput();

            Player.Lust -= 30;

            if (Player.IsChild())
                OutputText("CharCreation.ArrivalPartThree.TheImpShakesTheEmpty".Localized());
            else
                OutputText("CharCreation.ArrivalPartThree.TheImpShakesThe".Localized());

            OutputText("CharCreation.ArrivalPartThree.TheImpSays".Localized());

            OutputText("CharCreation.ArrivalPartThree.YourPleasureDefeatingThe".Localized());

            ClearButtons();

            AddButton(0, "Common.OK".Localized(), ArrivalPartFour);
        }

        protected virtual void ArrivalPartFour()
        {
            ClearOutput();

            OutputText(Player.IsChild() ? "CharCreation.ArrivalPartFour.YouLookAroundThe".Localized() : "CharCreation.ArrivalPartFour.YouLookAroundSurveying".Localized());

            ClearButtons();

            AddButton(0, "Common.OK".Localized(), EnterCamp);
        }

        protected virtual void StartTheGame()
        {
            Player.StartingRace = Player.GetRace();

            Player.Loaded = true;

            Player.CharCreation = false;

            if (IsNewCharacter())
            {
                var lower = Player.Femininity >= 50

                    ? (Player.IsChild() ? "child_panties" : "comfy_panties")

                    : "comfy_loincloth";

                Player.SetUndergarment(GetUndergarment(lower));

                if (Player.BiggestBreastSize() >= 2)
                    Player.SetUndergarment(GetUndergarment("comfy_bra"));
                else if (Player.Femininity >= 50 && ((Player.BiggestBreastSize() >= 1 && Player.IsChild()) || Player.IsTeen()))
                    Player.SetUndergarment(GetUndergarment("training_bra"));
                else
                    Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);
            }

            Player.Health = Player.MaxHealth;

            Flags.Set(GameFlagIds.CharCreationDone, 1);

            FinishInput();

            ClearButtons();

            Current = this;

            Arrival();
        }

        public static Undergarment GetUndergarment(string id) => (Undergarment)ItemRegistry.Create(new Identifier("coc", id));

        private bool _reincarnating;

        private static readonly Identifier[] AscensionPerks =

        [
            PerkIds.AscensionDesires,

            PerkIds.AscensionEndurance,

            PerkIds.AscensionFertility,

            PerkIds.AscensionFortune,

            PerkIds.AscensionMoralShifter,

            PerkIds.AscensionMysticality,

            PerkIds.AscensionTolerance,

            PerkIds.AscensionVirility,

            PerkIds.AscensionWisdom,

            PerkIds.AscensionMartiality,
        ];

        private static readonly int[] AscensionPerkMaxLevels = [10, 10, 15, -1, 10, 10, 20, 15, 5, 10];

        private static readonly Identifier[] PermablePerks =

        [
            PerkIds.Flexibility, PerkIds.Incorporeality, PerkIds.SatyrSexuality,

            PerkIds.Lustserker, PerkIds.CorruptedNinetails, PerkIds.EnlightenedNinetails,

            PerkIds.Bloodhound, PerkIds.MaraesGiftButtslut, PerkIds.MaraesGiftFertility,

            PerkIds.MaraesGiftProfractory, PerkIds.MaraesGiftStud, PerkIds.PurityBlessing,

            PerkIds.Hellfire, PerkIds.FireLord, PerkIds.Dragonfire,

            PerkIds.Androgyny, PerkIds.MagicalFertility, PerkIds.MagicalVirility,

            PerkIds.MilkMaid, PerkIds.Misdirection, PerkIds.MysticLearnings,

            PerkIds.RapierTraining, PerkIds.PotentPregnancy, PerkIds.PotentProstate,

            PerkIds.ParasiteQueen, PerkIds.ThickSkin, PerkIds.TerrestrialFire,

            PerkIds.MermaidChant,
        ];

        private void ApplyAscensionReset()
        {
            if (Flags.Get(GameFlagIds.NewGamePlusLevel) == 0)
                return;

            // 原版 CharCreation.hx:410-440：新转生流程重置非永久内容
            var keepPerks = Player.Perks
                .Where(p => IsAscensionPerk(p.Id))
                .Select(p => (p.Id, p.Values))
                .ToArray();

            Player.RemovePerks();

            foreach (var (id, values) in keepPerks)
                Player.CreatePerk(id, values[0], values[1], values[2], values[3]);

            Player.RemoveMasteries(false);

            Identifier[] keepKeys =

            [
                KeyItemIds.CampChest,

                KeyItemIds.CampMurkyChest,

                KeyItemIds.CampOrnateChest,

                KeyItemIds.EquipmentRackWeapons,

                KeyItemIds.EquipmentRackArmor,

                KeyItemIds.EquipmentRackShields,

                KeyItemIds.EquipmentStorageJewelryBox,
            ];

            foreach (var key in Player.KeyItems.Keys.ToArray())
                if (!keepKeys.Contains(key))
                    Player.RemoveKeyItem(key);

            Player.Storage.Clear();

            Player.RebuildStorage();
        }

        private static bool IsPermable(Identifier perk) => PermablePerks.Contains(perk);

        private static bool IsAscensionPerk(Identifier perk) =>

            AscensionPerks.Contains(perk) || (IsPermable(perk) && Player.Perkv4(perk) > 0);

        public void AscensionMenu()
        {
            ClearOutput();

            DisplayHeader("CharCreation.Ascension.Header".Localized());

            OutputText("CharCreation.Ascension.Story".Localized());

            OutputText("CharCreation.Ascension.Points".Localized((int)Player.AscensionPerkPoints));

            OutputText("CharCreation.Ascension.Done".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.Ascension.PerkSelect".Localized(), AscensionPerkMenu,

                "CharCreation.Ascension.PerkSelectHint".Localized());

            AddButton(1, "CharCreation.Ascension.PermPerks".Localized(), AscensionPermeryMenu,

                "CharCreation.Ascension.PermPerksHint".Localized());

            AddButton(2, "CharCreation.Ascension.Respec".Localized(), RespecLevelPerks,

                "CharCreation.Ascension.RespecHint".Localized());

            AddButton(3, "CharCreation.Ascension.PermMastery".Localized(), AscensionMasteryMenu,

                "CharCreation.Ascension.PermMasteryHint".Localized());

            var unlockedAge = Flags.Get(GameFlagIds.UnlockedAscensionAge) == 1;

            AddButton(4, "CharCreation.Ascension.Rename".Localized(), RenamePrompt,

                "CharCreation.Ascension.RenameHint".Localized());

            var ageButton = AddButton(5, "CharCreation.Ascension.ChangeAge".Localized(), () => DebugAgeChange(true),

                unlockedAge

                    ? "CharCreation.Ascension.ChangeAgeHint".Localized()

                    : "CharCreation.Ascension.ChangeAgeUnlock".Localized());

            if (!unlockedAge && Player.AscensionPerkPoints < 10 && ageButton != null)
            {
                ageButton.Enabled = false;

                ageButton.DisabledReason = "CharCreation.Ascension.ChangeAgeLocked".Localized();

                ageButton.RefreshVisual();
            }

            AddButton(9, "CharCreation.Ascension.Reincarnate".Localized(), ReincarnatePrompt,

                "CharCreation.Ascension.ReincarnateHint".Localized());
        }

        private void AscensionPerkMenu()
        {
            ClearOutput();

            OutputText("CharCreation.Ascension.PerkMenuText".Localized());

            OutputText("CharCreation.Ascension.Points".Localized((int)Player.AscensionPerkPoints));

            ClearButtons();

            for (var i = 0; i < AscensionPerks.Length; i++)
            {
                var perk = AscensionPerks[i];

                var max = AscensionPerkMaxLevels[i];

                var name = PerkName(perk);

                AddButton(i, name, () => AscensionPerkSelection(perk, max),

                    PerkDesc(perk));
            }

            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());
        }

        private static string PerkName(Identifier perk) =>

            ("Perks." + ToPascal(perk.Path) + ".Name").Localized();

        private static string PerkDesc(Identifier perk) =>

            ("Perks." + ToPascal(perk.Path) + ".Desc").Localized();

        private static string ToPascal(string path) =>

            string.Concat(path.Split('_').Select(part => part.Length == 0 ? part : char.ToUpperInvariant(part[0]) + part[1..]));

        private void AscensionPerkSelection(Identifier perk, int maxLevel)
        {
            ClearOutput();

            var level = (int)Player.Perkv1(perk);

            var cost = Math.Min(level + 1, 5);

            OutputText("CharCreation.Ascension.PerkEffect".Localized(PerkDesc(perk)));

            OutputText(maxLevel < 0

                ? "CharCreation.Ascension.PerkLevelNoMax".Localized(level)

                : "CharCreation.Ascension.PerkLevel".Localized(level, maxLevel));

            OutputText(level >= maxLevel && maxLevel > 0

                ? "CharCreation.Ascension.PerkCostNa".Localized()

                : "CharCreation.Ascension.PerkCost".Localized(cost));

            OutputText("CharCreation.Ascension.Points".Localized((int)Player.AscensionPerkPoints));

            ClearButtons();

            if (Player.AscensionPerkPoints >= cost && (level < maxLevel || maxLevel < 0))
                AddButton(0, "CharCreation.Ascension.AddLevel".Localized(), () => AddAscensionPerk(perk, maxLevel));

            AddButton(14, "CharCreation.Ascension.Back".Localized(), AscensionPerkMenu);
        }

        private void AddAscensionPerk(Identifier perk, int maxLevel)
        {
            var level = (int)Player.Perkv1(perk);

            var cost = Math.Min(level + 1, 5);

            if (Player.AscensionPerkPoints < cost)
                return;

            Player.AscensionPerkPoints -= cost;

            if (Player.HasPerk(perk))
                Player.AddPerkValue(perk, 0, 1);
            else
                Player.CreatePerk(perk, 1, 0, 0, 0);

            AscensionPerkSelection(perk, maxLevel);
        }

        private static int PermanentizeCost() =>

            PermablePerks.Count(p => Player.Perkv4(p) > 0) + 1;

        private void AscensionPermeryMenu()
        {
            ClearOutput();

            OutputText("CharCreation.Ascension.PermeryText".Localized());

            OutputText("CharCreation.Ascension.PermeryCost".Localized(PermanentizeCost()));

            OutputText("CharCreation.Ascension.Points".Localized((int)Player.AscensionPerkPoints));

            ClearButtons();

            var slot = 0;

            foreach (var perk in Player.Perks.ToArray())
            {
                if (!IsPermable(perk.Id))
                    continue;

                var perked = perk;

                AddNextButton(PerkName(perked.Id), () => PermanentizePerk(perked.Id),

                    perked.Values[3] > 0

                        ? "CharCreation.Ascension.Permed".Localized()

                        : PerkDesc(perked.Id));

                slot++;
            }

            AddButton(14, "CharCreation.Ascension.Back".Localized(), AscensionMenu);
        }

        private void PermanentizePerk(Identifier perk)
        {
            if (Player.AscensionPerkPoints < PermanentizeCost() || Player.Perkv4(perk) > 0)
                return;

            Player.AscensionPerkPoints -= PermanentizeCost();

            Player.AddPerkValue(perk, 3, 1);

            AscensionPermeryMenu();
        }

        private void RespecLevelPerks()
        {
            ClearOutput();

            if (Player.AscensionPerkPoints < 5)
            {
                OutputText("CharCreation.Ascension.RespecNoPoints".Localized((int)Player.AscensionPerkPoints));

                DoNext(AscensionMenu);

                return;
            }

            if (Player.PerkPoints == Player.Level - 1)
            {
                OutputText("CharCreation.Ascension.RespecNoNeed".Localized());

                DoNext(AscensionMenu);

                return;
            }

            Player.AscensionPerkPoints -= 5;

            Player.PerkPoints = Player.Level - 1;

            var ascendPerks = Player.Perks
                .Where(p => IsAscensionPerk(p.Id))
                .Select(p => (p.Id, p.Values))
                .ToArray();

            Player.RemovePerks();

            foreach (var (id, values) in ascendPerks)
                Player.CreatePerk(id, values[0], values[1], values[2], values[3]);

            OutputText("CharCreation.Ascension.RespecDone".Localized());

            DoNext(AscensionMenu);
        }

        private void RenamePrompt()
        {
            ClearOutput();

            OutputText("CharCreation.Ascension.RenameText".Localized());

            GenericNamePrompt();

            ClearButtons();

            AddButton(0, "Common.OK".Localized(), ConfirmRename);

            AddButton(14, "CharCreation.Ascension.Back".Localized(), AscensionMenu);
        }

        private void ConfirmRename()
        {
            var input = GetInput().Trim();

            if (input.Length == 0)
            {
                RenamePrompt();

                return;
            }

            Player.ShortName = input.Length > InputNameMaxChars ? input[..InputNameMaxChars] : input;

            AscensionMenu();
        }

        private void DebugAgeChange(bool inAscension = false)
        {
            if (inAscension && Flags.Get(GameFlagIds.UnlockedAscensionAge) == 0)
            {
                Player.AscensionPerkPoints -= 10;

                Flags.Set(GameFlagIds.UnlockedAscensionAge, 1);
            }

            ClearOutput();

            var current = Player.Age switch
            {
                Age.Child => "CharCreation.Ascension.AgeChild".Localized(),

                Age.Teen => "CharCreation.Ascension.AgeTeen".Localized(),

                Age.Elder => "CharCreation.Ascension.AgeElder".Localized(),

                _ => "CharCreation.Ascension.AgeAdult".Localized(),
            };

            OutputText("CharCreation.Ascension.AgeText".Localized(current));

            ClearButtons();

            AddButton(0, "CharCreation.AgeChoice.Button1".Localized(), () => DebugSetAge(Age.Child, inAscension));

            AddButton(1, "CharCreation.AgeChoice.Button2".Localized(), () => DebugSetAge(Age.Teen, inAscension));

            AddButton(2, "CharCreation.AgeChoice.Button3".Localized(), () => DebugSetAge(Age.Adult, inAscension));

            AddButton(3, "CharCreation.AgeChoice.Button4".Localized(), () => DebugSetAge(Age.Elder, inAscension));

            AddButton(14, "CharCreation.Ascension.Back".Localized(), AscensionMenu);
        }

        private void DebugSetAge(Age newAge, bool inAscension)
        {
            Player.Age = newAge;

            Player.StartingAge = newAge;

            Player.Health = Player.MaxHealth;

            ClearOutput();

            OutputText("CharCreation.Ascension.AgeDone".Localized());

            if (inAscension)
                DoNext(AscensionMenu);
            else
                DoNext(AscensionMenu);
        }

        private static int AscensionMasteryCost() =>

            1 + Player.Masteries.Count(m => m.Permed);

        private void AscensionMasteryMenu()
        {
            ClearOutput();

            OutputText("CharCreation.Ascension.MasteryText".Localized());

            OutputText("CharCreation.Ascension.PermeryCost".Localized(AscensionMasteryCost()));

            OutputText("CharCreation.Ascension.Points".Localized((int)Player.AscensionPerkPoints));

            ClearButtons();

            foreach (var mastery in Player.Masteries.ToArray())
            {
                var current = mastery;

                AddNextButton(current.Id.Path, () => AscensionMasteryPerm(current.Id),

                    current.Permed

                        ? "CharCreation.Ascension.MasteryPermed".Localized()

                        : current.Id.Path + " mastery");
            }

            AddButton(14, "CharCreation.Ascension.Back".Localized(), AscensionMenu);
        }

        private void AscensionMasteryPerm(Identifier mastery)
        {
            var cost = AscensionMasteryCost();

            if (Player.AscensionPerkPoints < cost)
                return;

            if (!Player.PermMastery(mastery))
                return;

            Player.AscensionPerkPoints -= cost;

            AscensionMasteryMenu();
        }

        public void PromptAscend()
        {
            ClearOutput();

            var level = Flags.Get(GameFlagIds.NewGamePlusLevel);

            var label = level switch
            {
                0 => "New Game+",

                1 => "New Game++",

                2 => "New Game+++",

                _ => "New Game+" + (level + 1),
            };

            OutputText("CharCreation.Ascension.PromptStart".Localized() + "[b:" + label + "]");

            OutputText("CharCreation.Ascension.PromptReset".Localized());

            OutputText("CharCreation.Ascension.PromptHuman".Localized());

            OutputText("CharCreation.Ascension.PromptProceed".Localized());

            ClearButtons();

            AddButton(0, "Common.Yes".Localized(), () =>
            {
                PendingAscension = true;

                Go<CharCreation>();
            });

            AddButton(1, "Common.No".Localized(), () => Go<CampScene>());
        }

        private void ReincarnatePrompt()
        {
            ClearOutput();

            OutputText("CharCreation.Ascension.ReincarnatePrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Yes".Localized(), Reincarnate);

            AddButton(1, "Common.No".Localized(), AscensionMenu);
        }

        private void Reincarnate()
        {
            Flags.Set(GameFlagIds.NewGamePlusLevel, Flags.Get(GameFlagIds.NewGamePlusLevel) + 1);

            _reincarnating = true;

            NewGameGo();
        }

        private void RouteToGenderChoiceReincarnation()
        {
            ClearOutput();

            GenericGenderChoice(Player.Age);
        }

        protected virtual void EnterCamp() => Go<CampScene>();
    }
}
