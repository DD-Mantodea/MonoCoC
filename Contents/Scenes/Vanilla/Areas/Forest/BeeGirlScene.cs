using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<BeeGirlScene>("coc:bee_girl")]
    public class BeeGirlScene : Scene
    {

        public static BeeGirlAttitude attitude
        {
            get => (BeeGirlAttitude)CoC.Instance.Flags.Get(GameFlagIds.BeeGirlAttitude);

            set => CoC.Instance.Flags.Set(GameFlagIds.BeeGirlAttitude, (int)value);
        }

        public static int conversation
        {
            get => CoC.Instance.Flags.Get(GameFlagIds.BeeGirlConversation);

            set => CoC.Instance.Flags.Set(GameFlagIds.BeeGirlConversation, value);
        }

        public static bool badEndWarning
        {
            get => CoC.Instance.Flags.Get(GameFlagIds.BeeGirlBadEndWarning) != 0;

            set => CoC.Instance.Flags.Set(GameFlagIds.BeeGirlBadEndWarning, value ? 1 : 0);
        }

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
        {
            ["nude"] = "bare, exposed body",
            ["straps"] = "solid black stockings and leather chest-wraps",
            ["leaf"] = "leafy wrappings",
            ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
        };

        public static SaveContent Save => SaveContent(SceneIds.Forest.BeeGirl);

        public override void Entry()
        {
            BeeEncounter();
        }

        public void SetTalked()
        {

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlStatus, (int)BeeGirlAttitude.Talked);
        }

        public void BeeEncounter()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounter.ApproachEdgeScent".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryGiantbees, false, true);

            if (Player.HasKeyItem("Traveler's Guide") && Player.Intelligence / 2 > Random.Shared.Next(40))
            {
                OutputText("Scenes.BeeGirl.BeeEncounter.RememberGuidePassage".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounter.DoYouProceed".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), () => BeeEncounterSelect()); AddButton(1, "No", () => ReturnToCamp(1));
            }
            else
                BeeEncounterSelect(false);
        }

        public void BeeEncounterSelect(bool clearScreen = true)
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterSelect.ComesIntoView".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && Player.CockArea(0) > 100 && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0)
            {
                BeeEncounterWithExgartuan();

                return;
            }

            if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasStatusEffect(StatusEffectIds.WormPlugged))
            {
                BeeEncounterWithWorms();

                return;
            }

            var isBeeMorph = Player.Race == "bee-morph";

            if (Player.HasCock() && (Player.CockArea(0) >= 50 || Player.Cocks[0].Id == BodyPartTypes.Bee || isBeeMorph))
            {
                OutputText("Scenes.BeeGirl.BeeEncounterSelect.InTheLightAgain".Localized());

                BeeEncounterAsBeeMorphMaleOrGiantCock(isBeeMorph);
            }
            else
                if (isBeeMorph)
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterSelect.InTheLightAgain".Localized());

                    BeeEncounterAsBeeMorphFemale();
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatLosses) + CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithoutRape) >= 5)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatLosses) > CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithoutRape))
                            BeeEncounterSheBeatsYouRegularly();
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithoutRape) >= CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape))
                                BeeEncounterSheFearsYou();
                            else
                                BeeEncounterSheDesiresYou();
                    }
                    else
                    {
                        switch (attitude)
                        {
                            case BeeGirlAttitude.PlayerAfraid:
                                {
                                    BeeEncounterAfraid();
                                }

                                break;

                            case BeeGirlAttitude.PlayerVoluntaryEgging:
                                {
                                    BeeEncounterAfraidRepeat();
                                }

                                break;

                            case BeeGirlAttitude.PlayerDisgusted:
                                {
                                    BeeEncounterDisgusted();
                                }

                                break;

                            case BeeGirlAttitude.PlayerDuty:
                                {
                                    BeeEncounterDuty();
                                }

                                break;
                        }

                        if (true)
                        {
                            OutputText("Scenes.BeeGirl.BeeEncounterSelect.BendingSmileApproach".Localized());

                            if ((Player.LibidoPercent + Player.CorAdjustedDown() < 140) || Random.Shared.Next(2) == 0)
                            {
                                OutputText("Scenes.BeeGirl.BeeEncounterSelect.BarelyStopGleeful".Localized());

                                ClearButtons();

                                AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

                                AddButton(1, "Common.Button.Talk".Localized(), BeeTalk);

                                AddButton(14, "Common.Button.Leave".Localized(), () => BeeLeave());
                            }
                            else
                                BeeEncounterClassic(false);
                        }
                    }
        }

        public void BeeLeave()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeLeave.DecidingAskingFor".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterAsBeeMorphMaleOrGiantCock(bool isBeeMorph)
        {

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphMaleOrGiantCock.FaceOpensSmile".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphMaleOrGiantCock.StillInDaze".Localized());

            if (Player.BiggestCockLength() > 50 || Player.Cocks[0].Id == BodyPartTypes.Bee)
                OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphMaleOrGiantCock.LetMeHelpMember".Localized());
            else
                OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphMaleOrGiantCock.ComeHereLittleOne".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphMaleOrGiantCock.OpensArmsAccept".Localized());

            if ((Player.LibidoPercent + Player.CorAdjustedDown() < 140) || Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphMaleOrGiantCock.BarelyHoldBack".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

                AddButton(1, "Common.Button.Sex".Localized(), () => BeeSexForCocks());

                AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
            }
            else
                BeeSexForCocks(false);
        }

        public void BeeEncounterAsBeeMorphFemale()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.FaceHugeGrin".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.ShakeHeadClearing".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.NotSureSpendMore".Localized());
            else
                if (Player.Corruption <= 66)
                    OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.CatchStaring".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.GrinAtThought".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.WhatWillYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(1, "Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.ButtonPlay".Localized(), BeeMaidenPlay);

            AddButton(14, "Common.Button.Leave".Localized(), BeeEncounterAsBeeMorphFemaleLeave);
        }

        public void BeeEncounterAsBeeMorphFemaleLeave()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemaleLeave.ShakeHeadDisappointed".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterAfraid()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterAfraid.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraid.FaceSmileBuzzing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(1, "Common.Button.HaveSex".Localized(), BeeEncounterAfraidFirstTimeSex);

            AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
        }

        public void BeeEncounterAfraidFirstTimeSex()
        {
            ClearOutput();

            attitude = BeeGirlAttitude.PlayerVoluntaryEgging;

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.SmileNodCrook".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.LooksLikeHumans".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.LookBackLegsCarried".Localized());

            var cockIndex = Player.CockThatFits(50);

            if (cockIndex >= 0)
            {
                if (Player.HasVagina())
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.SmileExaminesAsks".Localized(
                         Player.MultiCockDescriptLight(),

                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.NeedsMoreStimulation".Localized(
                         Player.VaginaDescript(0),

                         Player.CockDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.DualSensations".Localized());
                }
                else
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.RubbingHandUpDown".Localized(
                         Player.CockDescript(cockIndex)
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.CuntVice".Localized(
                         Player.CockDescript(cockIndex),

                         Player.CockDescript(cockIndex)
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.RoughSlickTexture".Localized(
                         Player.CockDescript(cockIndex)
                    ));
                }
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.BeeGigglesNeverTried".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.WrapArmsInsectoid".Localized(
                         Player.VaginaDescript(0)
                    ));

                    Player.CuntChange(25, true);

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.PartnerMassages".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.RubAbdomenOvipositor".Localized(
                         Player.VaginaDescript(0)
                    ));
                }
                else
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.BrushHairPointOut".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.AfterMinutesCupped".Localized(
                         Player.ButtDescript()
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.ShiverProbe".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidFirstTimeSex.UnableRegainWits".Localized());
                }

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterAfraidRepeat()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeat.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeat.FaceSmileBuzzing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(1, "Common.Button.HaveSex".Localized(), BeeEncounterAfraidRepeatSex);

            AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
        }

        public void BeeEncounterAfraidRepeatSex()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.NodStripReclines".Localized());

            if (Player.HasCock())
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.FingersAlongCocks".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.SlippingFingersAbout".Localized(
                     Player.VaginaDescript(0)
                ));
            else
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.SlippingFingerInside".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.SlightShudderKnowing".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.PullsBackPulls".Localized());

            if (Player.AnalCapacity() < 25)
            {
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.InsertingOrganSlow".Localized());

                if (Player.Anal.Looseness == 0)
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.LosingAnalVirginity".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.AssholeLooser".Localized());

                Player.ButtChange(25, true);

                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.PainGivesWay".Localized());
            }
            else
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.ExperiencePrepared".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.OvipositorAllInside".Localized());

            if (Player.Tallness <= 50)
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.TinyBodyAgainst".Localized());
            else
                if (Player.Tallness <= 68)
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.BlowsIntoEars".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.SmallBodyRubs".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.FeelBeeSeedFlow".Localized());

            var cockIndex = Player.CockThatFits(50);

            if (cockIndex >= 0)
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.LeftWantingMore".Localized(
                     Player.CockDescript(cockIndex),

                     Player.AssDescript(),

                     Player.CockDescript(cockIndex)
                ));

            if (Player.HasVagina())
                OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.CockLeftWanting".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.RemainConnected".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterAfraidRepeatSex.EggsRunOut".Localized());

            if (!Player.IsButtPregnant())
            {
                Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation);

                Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
            }

            Player.Orgasm("Anal");

            Player.SlimeFeed();

            var __sw0_0 = Random.Shared.Next(10);

            if (__sw0_0 == 0)
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.WhiteBook), 1);

                ReturnToCamp(1);
            }

            if (__sw0_0 == 1 || __sw0_0 == 2)
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.OviElixir), 1);

                ReturnToCamp(1);
            }

            if (__sw0_0 == 3 || __sw0_0 == 4)
            {
                Player.Inventory.Add(ItemRegistry.Create(UseableIds.BeeChitin), 1);

                ReturnToCamp(1);
            }

            if (true)
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PureHoney), 1);

                ReturnToCamp(1);
            }
        }

        public void BeeEncounterDisgusted()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterDisgusted.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterDisgusted.FaceFrown".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
        }

        public void BeeEncounterDuty()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterDuty.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterDuty.FaceSmileBuzzingDies".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(1, "Common.Button.Talk".Localized(), BeeEncounterDutyTalk);

            AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
        }

        public void BeeEncounterDutyTalk()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterDutyTalk.HandmaidenHappyTalk".Localized());

            if (Player.Intelligence < 50)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 0.5f));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), FreeHoneyEvent);

            AddButton(1, "Common.Button.No".Localized(), BeeEncounterDutyLeave);
        }

        public void BeeEncounterDutyLeave()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterDutyLeave.DeclineTakeLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterSheFearsYou()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterSheFearsYou.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheFearsYou.MouthWidePanic".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheFearsYou.WhatWillYouDoWith".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
        }

        public void BeeEncounterSheDesiresYou()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterSheDesiresYou.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheDesiresYou.MouthWidePanic".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheDesiresYou.WhatWillYouDoWith".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

            AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
        }

        public void BeeEncounterSheBeatsYouRegularly()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularly.InTheLight".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularly.MouthBreaksGrin".Localized());

            if ((Player.LibidoPercent + Player.CorAdjustedDown() < 70) || Random.Shared.Next(4) == 0)
            {
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularly.GreatDifficultyStop".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightTheBeeGirl);

                AddButton(1, "Common.Button.Talk".Localized(), () => BeeEncounterSheBeatsYouRegularlyTalk());

                AddButton(14, "Common.Button.Leave".Localized(), BeeLeave);
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularly.UnableControlThrow".Localized());

                BeeEncounterSheBeatsYouRegularlyLastChance();
            }
        }

        public void BeeEncounterSheBeatsYouRegularlyTalk(bool clearScreen = true)
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyTalk.SmilesGentle".Localized());

            BeeEncounterSheBeatsYouRegularlyLastChance();
        }

        public void BeeEncounterSheBeatsYouRegularlyLastChance()
        {

            if ((Player.LibidoPercent + Player.CorAdjustedDown() < 70) || Random.Shared.Next(4) == 0)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), () => BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs());

                AddButton(1, "Common.Button.No".Localized(), BeeEncounterSheBeatsYouRegularlyDontLetHer);
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyLastChance.WilesNoReason".Localized());

                BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs(false);
            }
        }

        public void BeeEncounterSheBeatsYouRegularlyDontLetHer()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyDontLetHer.ShakeOffWiles".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs(bool clearScreen = true)
        {
            ClearOutput();

            if (!(clearScreen))
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.ParagraphBreak".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.BreathlesslyYes".Localized());

            if (Player.HasBreasts())
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.GripSqueezeChest".Localized(
                     Player.NippleDescript(0)
                ));
            else
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.OneHandPulled".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.HappyBuzzViscous".Localized(
                 Player.ButtDescript()
            ));

            if (Player.HasCock())
            {
                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.FarMoreInterested".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.ThatsOneManyMore".Localized(
                     Player.MultiCockDescriptLight()
                ));
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.NotWhatAttracts".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.ThatsOneManyMoreAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.FingersPlayingBody".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.NoIdeaHowLong".Localized());
                }

            OutputText("Scenes.BeeGirl.BeeEncounterSheBeatsYouRegularlyAndYouLetHerLaysEggs.LastEggsBowels".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));

            Player.SlimeFeed();

            if (Random.Shared.Next(2) == 0)
            {
                Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation, 1);

                Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
            }

            Player.ButtChange(25, true);

            DoNext(() => ReturnToCamp(4));
        }

        public void BeeEncounterWithExgartuan()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterWithExgartuan.BendingSmile".Localized());

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.BeeGirl.BeeEncounterWithExgartuan.CockWrigglesFree".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounterWithExgartuan.EyesWidenShocked".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounterWithExgartuan.TooSurprisedResist".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), null);

                AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeEncounterWithExgartuan.EyesWidenShockedAgain".Localized());

                var beeGirl = EnemyRegistry.Create(EnemyIds.BeeGirl);

                beeGirl.OnDefeated = hpVictory => BeeGirlPCVictory(hpVictory);

                beeGirl.OnWon = _ => BeeRapesYou();

                CombatScene.Start(beeGirl, () => Go<CampScene>());
            }
        }

        public void BeeEncounterWithWorms()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeEncounterWithWorms.SlowlyWalkHaze".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterWithWorms.HandsOnShoulders".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterWithWorms.DripsByMyQueen".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterWithWorms.LiftsOffHovering".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterWithWorms.AloneArousedDress".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(0.5f * Player.Libido + 20)));

            DoNext(() => ReturnToCamp(1));
        }

        public void FightTheBeeGirl()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.FightTheBeeGirl.ClearHeadResolve".Localized());

            var beeGirl = EnemyRegistry.Create(EnemyIds.BeeGirl);

            beeGirl.OnDefeated = hpVictory => BeeGirlPCVictory(hpVictory);

            beeGirl.OnWon = _ => BeeRapesYou();

            CombatScene.Start(beeGirl, () => Go<CampScene>());
        }

        public void BeeEncounterClassic(bool clearScreen = true)
        {
            ClearOutput();

            if (attitude == BeeGirlAttitude.TalkedAndLeft || attitude == BeeGirlAttitude.TalkedAndLeftTwice)
                attitude = BeeGirlAttitude.Talked;

            BeeEncounterClassicSex(false);
        }

        public void BeeEncounterClassicSex(bool postCombat = true)
        {

            if (attitude == BeeGirlAttitude.TalkedAndLeft || attitude == BeeGirlAttitude.TalkedAndLeftTwice)
                attitude = BeeGirlAttitude.Talked;

            if (Player.IsTaur())
            {
                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LostControlTrot".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BeatsWingsDrags".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.NoWarningInserts".Localized());

                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.OvipositorPumps".Localized());

                if (Player.BiggestCockLength() >= 100 || Player.Gender == Gender.Genderless)
                {
                    if (Player.Gender == Gender.Male)
                    {
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.YouGrasp".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.OneOf".Localized());

                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.EngorgedJerkOff".Localized());

                        if (Player.Balls == 0)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CockQuiveringLap".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CockQuiveringSemen".Localized());

                        if (Player.CumQ() < 100)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.SpurtMoreCumAgain".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BallsFirehouse".Localized());
                    }
                    else
                        if (Player.Gender == Gender.Hermaphrodite)
                        {
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.GraspEngorgedJerk".Localized(
                                 Player.VaginaDescript(0)
                            ));

                            if (Player.Balls == 0)
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CockQuiveringUntended".Localized(
                                     Player.VaginaDescript(0)
                                ));
                            else
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CockQuiveringSemenAgain".Localized());

                            if (Player.CumQ() < 100)
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.SpurtMoreCumAgain".Localized());
                            else
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.FirehouseCoating".Localized(
                                     Player.SMultiCockDesc()
                                ));
                        }
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.PleasureLegsBuckle".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HoursFillingStinger".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AwakenHindHoney".Localized());
                }
                else
                {
                    if (Player.Gender == Gender.Male)
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.InabilityReach".Localized());
                    else
                        if (Player.Gender == Gender.Female)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.InabilityReachAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));
                        else
                            if (Player.Gender == Gender.Hermaphrodite)
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.InabilityReachThird".Localized());

                    if (Player.BiggestBreastSize() > 1)
                    {
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BeginPounding".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                        if (Player.HasFuckableNipples())
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.FingersSlipFemcum".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.NoStimulation".Localized());
                    }

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.ParagraphBreak".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.StimulationPainful".Localized());

                    if (Player.Gender == Gender.Male)
                    {
                        ShowImage("beegirl-loss-male");

                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LegsWrapBodyAgain".Localized());

                        if (Player.Balls == 0)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CumViolentlyThird".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CumViolentlySpurting".Localized());

                        if (Player.CumQ() < 100)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.SpurtMoreCumThird".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BallsFirehouseAgain".Localized());
                    }
                    else
                        if (Player.Gender == Gender.Female)
                        {
                            ShowImage("beegirl-loss-female");

                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LegsCurlHindAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));

                            if (Player.GetClitLength() < 3)
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AwkwardMinistrationsThird".Localized(
                                     Player.VaginaDescript(0)
                                ));
                            else
                                OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.QuicklyFindsClitThird".Localized(
                                     Player.VaginaDescript(0)
                                ));

                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HindLegsCoatedThird".Localized());
                        }
                        else
                            if (Player.Gender == Gender.Hermaphrodite)
                                if (Random.Shared.Next(2) == 0)
                                {
                                    ShowImage("beegirl-loss-male");

                                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LegsWrapBodyAgain".Localized());

                                    if (Player.Balls == 0)
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CumViolentlyThird".Localized());
                                    else
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CumViolentlySpurtingAgain".Localized());

                                    if (Player.CumQ() < 100)
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.SpurtMoreCumFourth".Localized());
                                    else
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BallsFirehouseThird".Localized());

                                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.StillNotSatisfied".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                    if (Player.GetClitLength() < 3)
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AwkwardMinistrationsThird".Localized(
                                             Player.VaginaDescript(0)
                                        ));
                                    else
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.QuicklyFindsClitThird".Localized(
                                             Player.VaginaDescript(0)
                                        ));

                                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HindLegsCoatedAgain".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LegsCurlHindAgain".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                    if (Player.GetClitLength() < 3)
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AwkwardMinistrationsThird".Localized(
                                             Player.VaginaDescript(0)
                                        ));
                                    else
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.QuicklyFindsClitThird".Localized(
                                             Player.VaginaDescript(0)
                                        ));

                                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HindLegsCoatedThird".Localized());

                                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.StillNotSatisfiedAgain".Localized());

                                    if (Player.Balls == 0)
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CumViolentlyThird".Localized());
                                    else
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CumViolentlySpurtingThird".Localized());

                                    if (Player.CumQ() < 100)
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.SpurtMoreCumFifth".Localized());
                                    else
                                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BallsFirehouseFourth".Localized());
                                }

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HoursFillingStingerAgain".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AwakenCoveredHoney".Localized());
                }

                Player.Orgasm("Generic");

                if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                    Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

                Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));

                Player.SlimeFeed();

                if (Random.Shared.Next(3) == 0)
                {
                    Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation, 1);

                    Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
                }

                Player.ButtChange(25, true);
            }
            else
                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LostControlFalls".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HocksAndKnees".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.WordStomach".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.NorProtestStinger".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.WordMember".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.WordMembers".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.ForcefullyRapesHumming".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HoursFillingStingerThird".Localized());

                    Player.SlimeFeed();

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AwakenHoneyScent".Localized());

                    Player.ButtChange(25, true);

                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));

                    if (Random.Shared.Next(3) == 0)
                    {
                        Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation, 1);

                        Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
                    }
                }
                else
                {
                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.BlissfullyThrowArms".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.LayGiddySmile".Localized());

                    if (Player.Vaginas.Count == 0)
                    {
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.WritheMoanNoRelief".Localized());

                        if (Player.TotalNipples() > 1)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.NipplesSwollenRed".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.Period".Localized());
                    }
                    else
                    {
                        if (Player.Vaginas[0].Wetness < 2)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.PuffyVulva".Localized());

                        if (Player.Vaginas[0].Wetness >= 2 && Player.Vaginas[0].Wetness < 4)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.GrowingPotent".Localized(
                                 Player.VaginaDescript(0)
                            ));

                        if (Player.Vaginas[0].Wetness >= 4)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.InflamingNeed".Localized(
                                 Player.VaginaDescript(0)
                            ));
                    }

                    if (Player.BiggestBreastSize() > 1)
                    {
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CoolHardnessStartles".Localized());

                        if (Player.BiggestLactation() > 1)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HummingMilkSplatters".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.PeriodAgain".Localized());
                    }

                    if (Player.BreastRows.Count > 1)
                    {
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.CoolHardnessRunning".Localized());

                        if (Player.BiggestLactation() > 1)
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.MilkSplashFlower".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.PeriodThird".Localized());
                    }

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.ButtClenchesWet".Localized());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HandGuided".Localized(
                             Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.PeriodBreak".Localized());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.FingersDanceClit".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.FingersExploreEvery".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.HipsWrithing".Localized());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.ConstantHoneyDevouring".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.AndConstantHoney".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.DriveOrgasmHours".Localized());

                    OutputText("Scenes.BeeGirl.BeeEncounterClassicSex.EventuallyNoLongerFeeding".Localized());

                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));

                    Player.SlimeFeed();

                    if (Random.Shared.Next(3) == 0)
                    {
                        Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation, 1);

                        Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
                    }

                    Player.ButtChange(25, true);
                }

            if (postCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void BeeSexForCocks(bool clearScreen = true)
        {
            ClearOutput();

            if (badEndWarning == true && Random.Shared.Next(2) == 0)
            {
                BeeDroneBadEnd();

                return;
            }

            var giantCockIndex = -1;

            var x = 0;

            while (x < Player.Cocks.Count)
            {
                if (Player.Cocks[x].Length < 24 || Player.CockArea(x) < 100)
                {
                    x += 1;

                    continue;
                }

                giantCockIndex = x;

                break;
            }

            if (giantCockIndex != -1)
            {
                OutputText("Scenes.BeeGirl.BeeSexForCocks.RushForwardFlopping".Localized(
                     Player.CockDescript(giantCockIndex)
                ));

                OutputText("Scenes.BeeGirl.BeeSexForCocks.SetsDownFlower".Localized());

                OutputText("Scenes.BeeGirl.BeeSexForCocks.LongingLook".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => BeeSexForCocksPart2(giantCockIndex));
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeSexForCocks.ThrowIntoArms".Localized());

                OutputText("Scenes.BeeGirl.BeeSexForCocks.FingersOverCocks".Localized());

                ShowImage("beegirl-loss-male");

                OutputText("Scenes.BeeGirl.BeeSexForCocks.StillDeserveGood".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.BeeGirl.BeeSexForCocks.OneHandShaft".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeSexForCocks.StartingStroking".Localized(
                         Player.CockDescript(1),

                         Player.Cocks.Count > 2 ? "This doesn't stop her from performing the same treatment on the whole of your " + Player.MultiCockDescript() + ", alternating between each of your manhoods. " : ""
                    ));

                OutputText("Scenes.BeeGirl.BeeSexForCocks.HandsCoveredCum".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Corruption, -2f));

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SpecialHoney), 1);

                ReturnToCamp(4);
            }
        }

        public void BeeSexForCocksPart2(int giantCockIndex)
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart2.AllEncouragement".Localized(
                 Player.CockDescript(giantCockIndex),

                 Player.CockDescript(giantCockIndex)
            ));

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart.BeePeeksLength".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Bee)
            {
                OutputText("Scenes.BeeGirl.BeeSexForCocksPart.PainNeverGoes".Localized());

                badEndWarning = true;
            }
            else
                OutputText("Scenes.BeeGirl.BeeSexForCocksPart.HoneySoothing".Localized(
                     Player.CockDescript(giantCockIndex)
                ));

            ShowImage("beegirl-loss-male");

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart.SatisfiedGlazed".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.BeeGirl.BeeSexForCocksPart.WordSplash".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.BeeGirl.BeeSexForCocksPart.WordWave".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeSexForCocksPart.WordTorrent".Localized());

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart.CumFromTip".Localized(
                 Player.CockDescript(giantCockIndex)
            ));

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart.FirstOrgasmNothing".Localized());

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart2.BeeEnergyRunsOut".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Bee)
                OutputText("Scenes.BeeGirl.BeeSexForCocksPart.KnowTheDeal".Localized());
            else
                if (Player.Race == "bee-morph")
                    OutputText("Scenes.BeeGirl.BeeSexForCocksPart.TakeThisAfter".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeSexForCocksPart.LotsSpecialHoney".Localized());

            OutputText("Scenes.BeeGirl.BeeSexForCocksPart.HandingBottleWink".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, -3f));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SpecialHoney), 1);

            ReturnToCamp(4);
        }

        public void BeeDroneBadEnd()
        {

            OutputText("Scenes.BeeGirl.BeeDroneBadEnd.HelloAgainSmile".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEnd.LookConfused".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEnd.OfferIntrigues".Localized());

            if (Player.Body.Get<WingsPart>().Id == BodyPartTypes.BeeLikeLarge)
                OutputText("Scenes.BeeGirl.BeeDroneBadEnd.SpreadWingsTakesHand".Localized());
            else
            {
                OutputText("Scenes.BeeGirl.BeeDroneBadEnd.AcceptHoneyBottle".Localized());

                if (Player.Body.Get<WingsPart>().Id == BodyPartTypes.None)
                    OutputText("Scenes.BeeGirl.BeeDroneBadEnd.BackTinglesWings".Localized());
                else
                    if (Player.Body.Get<WingsPart>().Id == BodyPartTypes.BeeLikeSmall)
                        OutputText("Scenes.BeeGirl.BeeDroneBadEnd.WingsGrowLarger".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeDroneBadEnd.WingsItchyFallOff".Localized());

                OutputText("Scenes.BeeGirl.BeeDroneBadEnd.NoddingApprovalHands".Localized());
            }

            OutputText("Scenes.BeeGirl.BeeDroneBadEnd.TakesHighOverTrees".Localized());

            DoNext(() => BeeDroneBadEndPart2());
        }

        public void BeeDroneBadEndPart2()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.BeforeLongYellow".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart2.NowFreeMoveOn".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart2.MostInterestingThing".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.ListenFascinationPain".Localized());

            DoNext(() => BeeDroneBadEndPart3());
        }

        public void BeeDroneBadEndPart3()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart3.EnterQueenChamber".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart3.FemalesOutnumber".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.AhYesQueen".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.OtherMajorAbdomen".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.RoomLargeDome".Localized());

            DoNext(() => BeeDroneBadEndPart4());
        }

        public void BeeDroneBadEndPart4()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.WelcomeToHive".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.WordsSweetHoney".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.EachInchPush".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.AttentionDrawnMoment".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart4.QueenSaysStinger".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.CantThinkOnlyThrust".Localized());

            OutputText("Scenes.BeeGirl.BeeDroneBadEndPart.OfCourseAgree".Localized());

            GameOver();
        }

        public void BeeTalk()
        {
            ClearOutput();

            if (attitude < BeeGirlAttitude.Talked)
            {
                attitude = BeeGirlAttitude.Talked;

                OutputText("Scenes.BeeGirl.BeeTalk.StopsBuzzingTakenAback".Localized());

                OutputText("Scenes.BeeGirl.BeeTalk.WhereElseLayEggs".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.BeeGirl.BeeTalk.SureNoGoodBody".Localized());

                if (Player.Corruption >= 33 && Player.Corruption <= 66)
                    OutputText("Scenes.BeeGirl.BeeTalk.HerOfferIntriguesAgain".Localized());

                if (Player.Corruption > 66)
                    OutputText("Scenes.BeeGirl.BeeTalk.LustTintedEyesAgain".Localized());

                OutputText("Scenes.BeeGirl.BeeTalk.DoYouAcceptOfferAgain".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), () => BeeEncounterClassic());

                AddButton(1, "Common.Button.Leave".Localized(), () => BeeLeave());
            }
            else
                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.BeeGirl.BeeTalk.CaughtPantsDown".Localized());

                    OutputText("Scenes.BeeGirl.BeeTalk.DoYouCollectHoney".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Yes".Localized(), FreeHoneyEvent);

                    AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
                }
                else
                {
                    if (attitude == BeeGirlAttitude.TalkedAndLeft)
                        OutputText("Scenes.BeeGirl.BeeTalk.YoureBackEggs".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeTalk.CocksHeadYouAgain".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.BeeGirl.BeeTalk.SureNoGoodUrging".Localized());

                    if (Player.Corruption >= 33 && Player.Corruption <= 66)
                        OutputText("Scenes.BeeGirl.BeeTalk.HerOfferIntriguesAgain".Localized());

                    if (Player.Corruption > 66)
                        OutputText("Scenes.BeeGirl.BeeTalk.LustTintedEyesAgain".Localized());

                    OutputText("Scenes.BeeGirl.BeeTalk.DoYouAcceptOfferAgain".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Yes".Localized(), () => BeeEncounterClassic());

                    AddButton(1, "Common.Button.No".Localized(), () => BeeEncounterRefusedHerEggs());
                }
        }

        public void BeeEncounterRefusedHerEggs()
        {

            switch (attitude)
            {
                case BeeGirlAttitude.TalkedAndLeftTwice:
                    {
                        OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggs.HandmaidenStandsUp".Localized());

                        OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggs.StopWonderWhy".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 25)));

                        ClearButtons();

                        AddButton(0, "Scenes.BeeGirl.BeeEncounterRefusedHerEggs.ButtonAfraid".Localized(), BeeEncounterRefusedHerEggsAfraid);

                        AddButton(1, "Scenes.BeeGirl.BeeEncounterRefusedHerEggs.ButtonDisgusted".Localized(), BeeEncounterRefusedHerEggsDisgusted);

                        AddButton(2, "Scenes.BeeGirl.BeeEncounterRefusedHerEggs.ButtonDuty".Localized(), BeeEncounterRefusedHerEggsDuty);

                        AddButton(14, "Common.Button.Leave".Localized(), BeeEncounterRefusedHerEggsLeave);

                        return;
                    }

                case BeeGirlAttitude.TalkedAndLeft:
                    {
                        attitude = BeeGirlAttitude.TalkedAndLeftTwice;
                    }

                    break;
            }

            if (true)
                attitude = BeeGirlAttitude.TalkedAndLeft;

            OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggs.FaceFallsRefusal".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 25)));

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterRefusedHerEggsAfraid()
        {

            attitude = BeeGirlAttitude.PlayerAfraid;

            OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggsAfraid.MoveAwayExplain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 25)));

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterRefusedHerEggsDisgusted()
        {

            attitude = BeeGirlAttitude.PlayerDisgusted;

            OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggsDisgusted.FindsIdeaDisgusting".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 25)));

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterRefusedHerEggsDuty()
        {

            attitude = BeeGirlAttitude.PlayerDuty;

            OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggsDuty.ExplainChampion".Localized());

            OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggsDuty.NodsSmilesUnderstanding".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 25)));

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeEncounterRefusedHerEggsLeave()
        {

            OutputText("Scenes.BeeGirl.BeeEncounterRefusedHerEggsLeave.NotDealRightNow".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 25)));

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineBeeTalk()
        {

            OutputText("Scenes.BeeGirl.DeclineBeeTalk.PoutsSingSong".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((20 + Player.Libido / 15))));

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeMaidenPlay()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeMaidenPlay.NodWalkFlower".Localized(
                 Player.AssDescript()
            ));

            if (Player.Corruption <= 33)
                OutputText("Scenes.BeeGirl.BeeMaidenPlay.SitNervously".Localized());
            else
                if (Player.Corruption <= 66)
                    OutputText("Scenes.BeeGirl.BeeMaidenPlay.CantHelpAnticipation".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeMaidenPlay.BarelyKeepFromJumping".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.BeeGirl.BeeMaidenPlay.RepeatAfterMe".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Wetness() <= 1)
                    OutputText("Scenes.BeeGirl.BeeMaidenPlay.WordBarely".Localized());
                else
                    if (Player.Wetness() <= 3)
                        OutputText("Scenes.BeeGirl.BeeMaidenPlay.WordEasily".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeMaidenPlay.WordLiberally".Localized());

                OutputText("Scenes.BeeGirl.BeeMaidenPlay.CoverWithFluids".Localized());

                ShowImage("beegirl-loss-female");

                OutputText("Scenes.BeeGirl.BeeMaidenPlay.RaisesDrenchedHand".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.SensitivityPercent < 40)
                    OutputText("Scenes.BeeGirl.BeeMaidenPlay.ManageToHoldOn".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeMaidenPlay.TooMuchMindBlank".Localized(
                         Player.HasBreasts() ? "Paying special attention to your " + Appearance.AllBreastsDescript(Player) : ""
                    ));

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 0.5f));
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeMaidenPlay.PartnerConfused".Localized());

                OutputText("Scenes.BeeGirl.BeeMaidenPlay.ThinkingOfferLead".Localized());

                OutputText("Scenes.BeeGirl.BeeMaidenPlay.ChitinFingersPlaying".Localized());

                OutputText("Scenes.BeeGirl.BeeMaidenPlay.SuddenStimulationJarring".Localized());
            }

            Player.Orgasm("Anal");

            DoNext(() => BeeMaidenConversation());
        }

        public void BeeMaidenConversation()
        {
            ClearOutput();

            if (conversation > 2 && !Player.IsPureEnough(20))
                conversation = 2;

            switch (conversation)
            {
                case 0:
                    {
                        conversation = 1;

                        OutputText("Scenes.BeeGirl.BeeMaidenConversation.AfterChanceRecover".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PureHoney), 1);

                        ReturnToCamp(1);
                    }

                    break;

                case 1:
                    {
                        conversation = 2;

                        OutputText("Scenes.BeeGirl.BeeMaidenConversation.OnceRecoveredStretch".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PureHoney), 1);

                        ReturnToCamp(1);
                    }

                    break;

                case 2:
                    {
                        conversation = 3;

                        OutputText("Scenes.BeeGirl.BeeMaidenConversation.AwhileLaterLeaning".Localized());

                        if (Player.Corruption > 20)
                            OutputText("Scenes.BeeGirl.BeeMaidenConversation.WishCouldGivePlace".Localized());
                        else
                        {
                            OutputText("Scenes.BeeGirl.BeeMaidenConversation.BeenThinkingFeelBad".Localized());

                            OutputText("Scenes.BeeGirl.BeeMaidenConversation.LooksBackExcited".Localized());
                        }

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PureHoney), 1);

                        ReturnToCamp(1);
                    }

                    break;

                case 3:
                    {
                        conversation = 4;

                        OutputText("Scenes.BeeGirl.BeeMaidenConversation.RelaxingFamiliarScene".Localized());

                        OutputText("Scenes.BeeGirl.BeeMaidenConversation.SoundedOminous".Localized());

                        AddButton(0, "Common.Button.Yes".Localized(), () => BeeMaidenFertileBeeBadEnd()); AddButton(1, "No", () => BeeMaidenConversationRejectCandy());
                    }

                    break;
            }

            if (true)
            {
                OutputText("Scenes.BeeGirl.BeeMaidenConversation.LieDownFlower".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => BeeMaidenFertileBeeBadEnd()); AddButton(1, "No", () => BeeMaidenConversationRejectCandy());
            }
        }

        public void BeeMaidenFertileBeeBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEnd.DeepBreathCandy".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEnd.EnergyFadesChanges".Localized(
                 Player.HasVagina() ? "something fundamental about your " + Player.VaginaDescript(0) + " changes, and you" : "you feel something split open on your previously bare growing and"
            ));

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEnd.TransformationEnds".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEnd.ComeOnExcited".Localized());

            DoNext(() => BeeMaidenFertileBeeBadEndPart2());
        }

        public void BeeMaidenFertileBeeBadEndPart2()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.BeforeLongYellow".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart2.NowFreeMoveOn".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart2.MostInterestingThing".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.ListenExplainsInner".Localized());

            DoNext(() => BeeMaidenFertileBeeBadEndPart3());
        }

        public void BeeMaidenFertileBeeBadEndPart3()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart3.EnterQueenChamber".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart3.FemalesOutnumber".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.AhYesQueen".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.OtherMajorAbdomen".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.RoomLargeDome".Localized());

            DoNext(() => BeeMaidenFertileBeeBadEndPart4());
        }

        public void BeeMaidenFertileBeeBadEndPart4()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.OverwhelmingVoice".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.SmallPartPanics".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.AttentionDrawn".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.NodWalkOverTool".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.QueenOvipositorUnloads".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.AnotherOrgasmPasses".Localized());

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.NotMuchLongerFilled".Localized());

            DoNext(() => BeeMaidenFertileBeeBadEndPart5());
        }

        public void BeeMaidenFertileBeeBadEndPart5()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.BeeMaidenFertileBeeBadEndPart.MemberOfFertileCast".Localized());

            GameOver();
        }

        public void BeeMaidenConversationRejectCandy()
        {

            OutputText("Scenes.BeeGirl.BeeMaidenConversationRejectCandy.FeltOffReject".Localized(
                 Player.ClothedOrNaked("put your [armor] back on and ")
            ));

            DoNext(() => ReturnToCamp(1));
        }

        public void FreeHoneyEvent()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.BeeGirl.FreeHoneyEvent.MullOfferPursue".Localized());

            OutputText("Scenes.BeeGirl.FreeHoneyEvent.HereTakeThisVial".Localized());

            OutputText("Scenes.BeeGirl.FreeHoneyEvent.TakeVialAbsently".Localized());

            OutputText("Scenes.BeeGirl.FreeHoneyEvent.ForceOrgasmSplatters".Localized());

            OutputText("Scenes.BeeGirl.FreeHoneyEvent.WavesStretchesPack".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PureHoney), 1);

            ReturnToCamp(1);
        }

        public void SeduceBeeGirl()
        {

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.BeeGirl.SeduceBeeGirl.PantingStandDefiant".Localized());

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.DrawsNearSmile".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.BeeGirl.SeduceBeeGirl.LeaningBetweenKnees".Localized());
                else
                    OutputText("Scenes.BeeGirl.SeduceBeeGirl.SlidingToFront".Localized());

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.TongueOverLength".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.BeeGirl.SeduceBeeGirl.SlidingDownBalls".Localized());

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.FingersPlayLips".Localized());

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.PullingBackReleases".Localized(
                     Player.CockDescript(0),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.CryOutVenom".Localized());

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.PressureRisingBase".Localized());

                OutputText("Scenes.BeeGirl.SeduceBeeGirl.CollapseStopsTwitching".Localized());

                Player.SlimeFeed();

                Player.Orgasm("Generic");
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void BeeRapesYou()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatLosses, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatLosses) + 1);

            var sexed = false;

            if (Player.IsTaur())
            {
                if (Player.Health < 1)
                {
                    OutputText("Scenes.BeeGirl.BeeRapesYou.SmirksApproaches".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.TestsInsidesAss".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.IncapacitatedWalksFront".Localized());

                    if (Player.Gender == Gender.Male && Player.Balls > 0)
                    {
                        OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndSeventh".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeBusiesAgain".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.StomachChurnsFourMoreFifth".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.ChitinArmsStrokingFifth".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.SensationTooMuchAgain".Localized(
                             Appearance.CockNoun(Player.Cocks[0].Id)
                        ));

                        OutputText("Scenes.BeeGirl.BeeRapesYou.UnderstandProblem".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.ShudderBeatingContinues".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.PainVenomOverwhelms".Localized());

                        if (Player.CumQ() > 1500)
                        {
                            OutputText("Scenes.BeeGirl.BeeRapesYou.AwakeMassivePuddle".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.DespiteEverythingOddlyGoodSixth".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.BeeGirl.BeeRapesYou.AwakePuddleCum".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.DespiteEverythingOddlyGoodSixth".Localized());
                        }

                        Player.Orgasm("Dick");

                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));
                    }
                    else
                        if (Player.Gender == Gender.Male && Player.Balls == 0)
                        {
                            OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndSeventh".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeBusiesAgain".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.StomachChurnsFourMoreFifth".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.ChitinArmsStrokingFifth".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.SensationTooMuchAgain".Localized(
                                 Appearance.CockNoun(Player.Cocks[0].Id)
                            ));

                            OutputText("Scenes.BeeGirl.BeeRapesYou.UnderstandProblemAgain".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.CombinationLustPain".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.AwakeDespiteEverything".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Lust, 100f));
                        }
                        else
                            if (Player.Gender == Gender.Female)
                            {
                                OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndThird".Localized(
                                     Player.VaginaDescript(0)
                                ));

                                OutputText("Scenes.BeeGirl.BeeRapesYou.PaysNoAttention".Localized());

                                OutputText("Scenes.BeeGirl.BeeRapesYou.StomachChurnsFourMoreFifth".Localized());

                                OutputText("Scenes.BeeGirl.BeeRapesYou.ChitinArmsStrokingFifth".Localized());

                                OutputText("Scenes.BeeGirl.BeeRapesYou.SensationTooMuchClit".Localized());

                                OutputText("Scenes.BeeGirl.BeeRapesYou.UnderstandProblemThird".Localized());

                                OutputText("Scenes.BeeGirl.BeeRapesYou.MalevolentContinueAgain".Localized());

                                if (Player.Wetness() == 5)
                                {
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.AwakeMassiveFemcum".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.DespiteEverythingOddlyGoodSixth".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.AwakePuddleFemcum".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.DespiteEverythingOddlyGoodSixth".Localized());
                                }

                                Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Lust, 100f));
                            }
                            else
                                if (Player.Gender == Gender.Hermaphrodite)
                                {
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndFourth".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeCock".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.StomachChurnsFourMoreFifth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ChitinArmsStrokingFifth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.SensationsTooMuch".Localized(
                                         Appearance.CockNoun(Player.Cocks[0].Id),

                                         Player.VaginaDescript(0)
                                    ));

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.UnderstandProblemFourth".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                    if (Player.Balls > 0)
                                        OutputText("Scenes.BeeGirl.BeeRapesYou.AndBalls".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.PeriodBreak".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.BlowsLandClit".Localized());

                                    if (Player.CumQ() > 1500 || Player.Wetness() >= 5)
                                    {
                                        OutputText("Scenes.BeeGirl.BeeRapesYou.AwakeMassiveVarious".Localized());

                                        OutputText("Scenes.BeeGirl.BeeRapesYou.DespiteEverythingOddlyGoodSixth".Localized());
                                    }
                                    else
                                    {
                                        OutputText("Scenes.BeeGirl.BeeRapesYou.AwakePuddleVarious".Localized());

                                        OutputText("Scenes.BeeGirl.BeeRapesYou.DespiteEverythingOddlyGoodSixth".Localized());
                                    }

                                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Lust, 100f));
                                }
                                else
                                {
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndFifth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeAsshole".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.StomachChurnsFourMoreFifth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ChitinArmsStrokingFifth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.SensationsTooMuchBegin".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.UnderstandProblemFifth".Localized(
                                         Player.AssDescript()
                                    ));

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.MalevolentContinueAgain".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.AwakeCoupleHours".Localized());

                                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Lust, 100f));
                                }
                }
                else
                {
                    OutputText("Scenes.BeeGirl.BeeRapesYou.SmilesSwaysHips".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.TestsInsidesAssAgain".Localized());

                    if (Player.Gender == Gender.Male && Player.Balls > 0)
                    {
                        OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndSeventh".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeBusiesInserting".Localized());

                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.BeeGirl.BeeRapesYou.ItAsIts".Localized());
                        else
                            OutputText("Scenes.BeeGirl.BeeRapesYou.ThemAsTheyre".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.TrappedUnderneathWeight".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.SeemsUnderstandHelp".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.PeculiarVibrations".Localized());

                        if (Player.CumQ() >= 1500)
                            OutputText("Scenes.BeeGirl.BeeRapesYou.MassiveCumDeep".Localized());

                        OutputText("Scenes.BeeGirl.BeeRapesYou.ContinuesPumpingPassOut".Localized());
                    }
                    else
                        if (Player.Gender == Gender.Male)
                        {
                            OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndSeventh".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeBusiesThird".Localized(
                                 Appearance.CockNoun(Player.Cocks[0].Id)
                            ));

                            OutputText("Scenes.BeeGirl.BeeRapesYou.SeemsUnderstandHelpAgain".Localized());

                            OutputText("Scenes.BeeGirl.BeeRapesYou.ContinuesPumpingPassOutAgain".Localized());
                        }
                        else
                            if (Player.Gender == Gender.Female)
                            {
                                OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndEighth".Localized(
                                     Player.VaginaDescript(0)
                                ));

                                OutputText("Scenes.BeeGirl.BeeRapesYou.SeesSprayStrokes".Localized(
                                     Player.VaginaDescript(0)
                                ));

                                OutputText("Scenes.BeeGirl.BeeRapesYou.SeemsUnderstandHelpThird".Localized(
                                     Player.VaginaDescript(0)
                                ));

                                if (Player.Wetness() == 5)
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.MassiveCumDeepAgain".Localized());

                                OutputText("Scenes.BeeGirl.BeeRapesYou.ContinuesPumpingRear".Localized());
                            }
                            else
                                if (Player.Gender == Gender.Hermaphrodite)
                                {
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndNinth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.CantSeeCockAgain".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.SensationTooMuchThird".Localized(
                                         Appearance.CockNoun(Player.Cocks[0].Id),

                                         Player.VaginaDescript(0)
                                    ));

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.SeemsUnderstandHelpFourth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.DoesThisHums".Localized());

                                    if (Player.CumQ() >= 1500)
                                        OutputText("Scenes.BeeGirl.BeeRapesYou.MassiveCumDeepThird".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ContinuesPumpingRearAgain".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.BeeGirl.BeeRapesYou.ReturnsRearEndTenth".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.PaysNoAttentionAgain".Localized());

                                    OutputText("Scenes.BeeGirl.BeeRapesYou.SeemsUnderstandHelpFifth".Localized());
                                }

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Lust, 100f));

                    Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation, 1);

                    Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
                }

                Player.SlimeFeed();

                if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                    Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                if ((Player.HasStatusEffect(StatusEffectIds.ParalyzeVenom) || Player.HasStatusEffect(StatusEffectIds.LustVenom)) && !sexed && Player.CockTotal() == 1)
                {
                    OutputText("Scenes.BeeGirl.BeeRapesYou.StandsDirectlyOver".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.RealityKicksIn".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.MoreBeeGirlsAppear".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.SlowlyGetsToFeet".Localized());

                    CombatScene.CleanupAfterCombat();

                    OutputText("Scenes.BeeGirl.BeeRapesYou.HoursLaterWakeFree".Localized());

                    sexed = true;

                    Player.Orgasm("Dick");

                    return;
                }

                if (Player.Gender == Gender.Male)
                {
                    if (Player.Lust >= Player.MaxLust)
                        OutputText("Scenes.BeeGirl.BeeRapesYou.OvercomeLustThrow".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeRapesYou.OvercomeWoundsUnable".Localized());

                    ShowImage("beegirl-loss-male");

                    OutputText("Scenes.BeeGirl.BeeRapesYou.HoldsCloseStinger".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.BeeGirl.BeeRapesYou.WordMember".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.BeeGirl.BeeRapesYou.WordMembers".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.ForcefullyRapesHumming".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.HoursFillingStinger".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.AwakenHoneyScent".Localized());

                    Player.SlimeFeed();

                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));

                    if (Random.Shared.Next(3) == 0)
                    {
                        Player.StartButtPregnancy(PregnancyIds.BeeEggs, PregnancyIds.BeeIncubation, 1);

                        Save.SetInt("timesEgged", Save.GetInt("timesEgged", 0) + 1);
                    }

                    Player.ButtChange(25, true, true, false);

                    CombatScene.CleanupAfterCombat();

                    return;
                }

                if (Player.Gender == Gender.Female)
                {
                    if (Player.LustPercent < 100)
                        OutputText("Scenes.BeeGirl.BeeRapesYou.BarelyConsciousLookUp".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeRapesYou.OvercomeDesireLookUp".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.WearyWeakBetrays".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.SharpPainStinger".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.IsTaur())
                        OutputText("Scenes.BeeGirl.BeeRapesYou.WordRearLegs".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeRapesYou.WordLegs".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.ThighsSlickClit".Localized());

                    if (Player.IsTaur())
                    {
                        OutputText("Scenes.BeeGirl.BeeRapesYou.SlipsDownChest".Localized(
                             Appearance.AllBreastsDescript(Player),

                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.BeeGirl.BeeRapesYou.JuicesMix".Localized());
                    }

                    OutputText("Scenes.BeeGirl.BeeRapesYou.SwiftMotionPussy".Localized());

                    ShowImage("beegirl-loss-female");

                    if (Player.IsTaur())
                        OutputText("Scenes.BeeGirl.BeeRapesYou.AfterWhilePussyDisappears".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeRapesYou.FeelStinger".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.SlidingNetherLips".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.IsTaur())
                        OutputText("Scenes.BeeGirl.BeeRapesYou.SuddenPainClit".Localized());
                    else
                        OutputText("Scenes.BeeGirl.BeeRapesYou.MoansMuffled".Localized());

                    OutputText("Scenes.BeeGirl.BeeRapesYou.HarderToBreatheHoney".Localized());

                    Player.SlimeFeed();

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f));

                    CombatScene.CleanupAfterCombat();

                    return;
                }

                if (Player.Gender == Gender.Hermaphrodite || Player.Gender == Gender.Genderless)
                {
                    OutputText("Scenes.BeeGirl.BeeRapesYou.DefeatedLittleToDo".Localized());

                    DoNext(() => BeeEncounterClassicSex());
                }
            }
        }

        public void BeeGirlPCVictory(bool hpVictory)
        {
            ClearOutput();

            if (hpVictory)
            {
                OutputText("Scenes.BeeGirl.BeeGirlPCVictory.SmileSatisfactionCollapses".Localized());

                OutputText("Scenes.BeeGirl.BeeGirlPCVictory.WhatDoYouDoAgain".Localized());
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeGirlPCVictory.SmileSatisfactionSpreads".Localized());

                OutputText("Scenes.BeeGirl.BeeGirlPCVictory.WhatDoYouDoAgain".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            ClearButtons();

            AddButton(0, "Common.Button.UseCock".Localized(), null);

            AddButton(1, "Common.Button.UseCocks".Localized(), null);

            AddButton(2, "Common.Button.UseVagina".Localized(), null);

            AddButton(3, "Common.Button.HermStyle".Localized(), null);

            AddButton(4, "Common.Button.DildoRape".Localized(), null);

            AddButton(5, "Common.Button.Breastfeed".Localized(), null);

            AddButton(6, "Common.Button.Naga".Localized(), null);

            AddButton(7, "Common.Button.SelfEgg".Localized(), null);

            AddButton(8, "Common.Button.Layyoureggs".Localized(), null);

            if (Player.HasCock())
                AddButton(0, "Common.Button.UseCock".Localized(), RapeTheBeeGirlWithADick);

            if (Player.CockTotal() > 1)
                AddButton(1, "Common.Button.UseCocks".Localized(), RapeTheBeeMultiCockStuff);

            if (Player.HasVagina())
                AddButton(2, "Common.Button.UseVagina".Localized(), RapeABeeGirlWithYourVagina);

            if (Player.IsHerm())
                AddButton(3, "Common.Button.HermStyle".Localized(), FutaRapesBeeGirl);

            if (Player.HasKeyItem("Deluxe Dildo"))
                AddButton(4, "Common.Button.DildoRape".Localized(), BeeGirlsGetsDildoed);

            if (Player.HasStatusEffect(StatusEffectIds.Feeder) || Player.LactationQ() >= 500)
                AddButton(5, "Common.Button.Breastfeed".Localized(), MilkAndHoneyAreKindaFunny);

            if (Player.IsNaga() && Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                AddButton(6, "Common.Button.Naga".Localized(), CorruptNagaBitchesRapeABee);

            if ((Player.IsCorruptEnough(75) || Player.HasPerk(PerkIds.Pervert) || Player.HasPerk(PerkIds.Sadist)) && Player.Strength >= 60 && (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake || Player.HasCock() || Player.HasVagina() || Player.BiggestBreastSize() >= 4))
                AddButton(7, "Common.Button.SelfEgg".Localized(), BeeGirlRapeForTheDistinguishedGentleman);

            if (Player.CanOvipositSpider() && (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs || Player.Body.Get<FacePart>().Id == BodyPartTypes.SpiderFangs))
                AddButton(8, "Common.Button.Layyoureggs".Localized(), LayEggsInABeeSpiderLike);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => LeaveAfterDefeating(hpVictory));
        }

        public void LeaveAfterDefeating(bool hpVictory)
        {

            if (hpVictory)
                CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithoutRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithoutRape) + 1);
            else
                CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void RapeTheBeeMultiCockStuff()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            if (Player.CockTotal() >= 5 && Player.BiggestBreastSize() > 2 && Player.Vaginas.Count > 0 && Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.ApproachEdgesAway".Localized(
                     Player.MultiCockDescript(),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.ShiftUpward".Localized());

                ShowImage("beegirl-win-male");

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.BeginPushForward".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.UnhinderedFuck".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.LetGoArms".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.HeadsRub".Localized(
                     Appearance.CockNoun(BodyPartTypes.Human)
                ));

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.AbdomenPressingRear".Localized());

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();

                return;
            }

            if (Player.CockTotal() == 2 && Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.TurnsAroundScampers".Localized());

                ShowImage("beegirl-win-male");

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.ForcingOntoBack".Localized(
                     Player.MultiCockDescriptLight(),

                     Player.MultiCockDescriptLight()
                ));

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.AbdomenDownSmack".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.DecideOtherGift".Localized(
                     Player.CockDescript(1)
                ));

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.ShrieksBliss".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeMultiCockStuff.LetGoHands".Localized());

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();

                return;
            }
            else
                BeeAlternate();

            return;
        }

        public void RapeTheBeeGirlWithADick()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            if (Player.IsTaur())
            {
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.PlopsOntoFlower".Localized());
                else
                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.PlopsBarelyConscious".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ReachStrokeHair".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.PullBackConsider".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.NotLongGrinding".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SweetSubstanceHeats".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.HoneyGoneQuickly".Localized());

                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SqueezeMassageMounds".Localized());

                if (Player.CockArea(x) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
                {
                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.LooksUnderArm".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.OtherHandBalls".Localized(
                             Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.OtherHandPullsOpen".Localized(
                             Player.CockDescript(x)
                        ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.MinistrationsStop".Localized());

                    ShowImage("beegirl-win-male");

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ThinkRideHorse".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.GetAnswerVice".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.GentleAtFirstAgain".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SensationIncredibleMilk".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.BallsSpectacular".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.CuntConstrictsCums".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.IncreasinglyDifficult".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.AndBallsConstrict".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.PeriodBreak".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SeedPropelled".Localized(
                         Appearance.CockNoun(Player.Cocks[x].Id)
                    ));

                    if (Player.Balls == 0)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.DribblesHoneySquirts".Localized());
                    else
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SpraysInside".Localized());
                        else
                            OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SpraysOverfilling".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ComeDownAttention".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.WatchingLowerBodyAgain".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.OnceComfortableCampAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.LooksUnderArmDisappointed".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.GigglesRetractsTongue".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.DoesntSwingBeneath".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.GripSurprisinglyStrong".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.GentleAtFirstAgain".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.FeelsWonderfulPumping".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.VibrationsNumberOn".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ThingInAsshole".Localized());

                    if (Player.Tallness * (5 / 6) < Player.Cocks[Player.BiggestCockIndex()].Length)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.TongueWrapsJerk".Localized(
                             Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.TongueWrapsSlides".Localized(
                             Player.CockDescript(x)
                        ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.CombinedSensationsUnload".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0 && Player.Tallness * (5 / 6) < Player.Cocks[Player.BiggestCockIndex()].Length)
                    {
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.BallsContractPropel".Localized());

                        if (Player.CumQ() >= 750)
                            OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.DontStopFirehose".Localized());
                    }
                    else
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.BallsContractPropelAgain".Localized());

                            if (Player.CumQ() >= 750)
                                OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.DontStopFirehoseAgain".Localized());
                        }
                        else
                            OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.AssholeTightens".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ReleasesGraspThud".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.TakePityPickUp".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.WatchingLowerBodyAgain".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.OnceComfortableCampAgain".Localized());
                }

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            }
            else
                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.FirmlyGraspingThighs".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.HardensUnderBelly".Localized(
                             Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.DisrobeRevealing".Localized(
                             Player.CockDescript(x)
                        ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.WastingNoTime".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.PenetratingSlickSnatch".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.CrammingDoubleStuff".Localized(
                             Player.MultiCockDescript()
                        ));

                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.CrammingTwoDouble".Localized(
                             NumberUtils.Num2Text(Player.CockTotal()),

                             Player.MultiCockDescript()
                        ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SwoonsFlopsBack".Localized());

                    ShowImage("beegirl-win-male");

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.BeginPoundingSplatter".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.BeyondLimitLoseControl".Localized());

                    if (Player.Gender == Gender.Hermaphrodite && !Player.IsTaur())
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ReachFingerRoughly".Localized(
                             Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.QuiveringQuimReleases".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.PluralS".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.StaggeringMarvel".Localized());

                    if (Player.CumQ() >= 250 && Player.CumQ() < 500)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SmileProudlyStream".Localized());

                    if (Player.CumQ() >= 500 && Player.CumQ() < 1000)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.SmileProudlyHandiwork".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.GigglePoorVictim".Localized());

                    Player.Orgasm("Dick");

                    CombatScene.CleanupAfterCombat();
                }
                else
                {
                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.ApproachEdgesAway".Localized());

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.BeginPushForward".Localized(
                         Player.CockDescript(x)
                    ));

                    ShowImage("beegirl-win-male");

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.UnhinderedFuck".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.BeeGirl.RapeTheBeeGirlWithADick.LetGoArms".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    Player.Orgasm("Dick");

                    CombatScene.CleanupAfterCombat();
                }
        }

        public void RapeABeeGirlWithYourVagina()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            if (Player.IsTaur())
            {
                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.PlopsOntoFlower".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.MakeIntentionsClear".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.MaidenConfusedDisplay".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.WhileSheCanReach".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.NotEvenPenetration".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.ParagraphBreakAgain".Localized());

                ShowImage("beegirl-win-female");

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.DespiteAbilityReach".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.HazeFrustrationLeap".Localized());

                if (Player.BiggestBreastSize() > 4)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.LegsReleaseHead".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.LegsReleaseCatch".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.BodyStiffens".Localized());

                if (Player.HasFuckableNipples() || Player.LactationQ() >= 20)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.OozeFluidsConstant".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.ObviouslyUnderstands".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.PeculiarSensationChitin".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.NotDoneFingerReplaced".Localized());

                if (Player.HasLongTail())
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.FlailingTailHeld".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.OtherHandVagina".Localized());

                if (Player.Anal.Looseness >= 2)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.NotWantingBrace".Localized(
                         Player.AssDescript()
                    ));

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.FindsUterusProbes".Localized());

                if (Player.Wetness() >= 5)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.VaginaRipplesHurts".Localized());
                else
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.VaginaRipplesHurtsAgain".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.AfterglowSomething".Localized());

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.TrotUpFlower".Localized());
            }
            else
            {
                ShowImage("beegirl-win-female");

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.FirmlyGraspingThighs".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.AndFragment".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.WasteNoTimePush".Localized(
                     Player.VaginaDescript(0)
                ));

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.ReturnFavorDive".Localized());

                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.TasteSweetSexy".Localized());
                }
                else
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.ParagraphBreakAgain".Localized());

                if (Player.Vaginas[0].Wetness < 3)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.TongueKeepsUp".Localized());

                if (Player.Vaginas[0].Wetness >= 3 && Player.Vaginas[0].Wetness < 5)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.HipsWiggleWrithe".Localized());

                if (Player.Vaginas[0].Wetness >= 5)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.HipsQuiverGrind".Localized());

                if (Player.GetClitLength() > 3 && Player.GetClitLength() < 5)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.PistonHugeClit".Localized());

                if (Player.GetClitLength() >= 5)
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.ShoveMassiveClit".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.OrgasmNoTimeCoating".Localized());
                else
                    OutputText("Scenes.BeeGirl.RapeABeeGirlWithYourVagina.NoTimeBothOrgasm".Localized());
            }

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void FutaRapesBeeGirl()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.FirmlyGraspingThighs".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.IdlyFondle".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.DisrobeRevealingAll".Localized(
                     Appearance.AllBreastsDescript(Player),

                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.StepForwardsStraddling".Localized());

            if (Player.Vaginas[0].Wetness <= 1)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.BecomesPuffyMoist".Localized(
                     Player.VaginaDescript(0)
                ));

            if (Player.Vaginas[0].Wetness > 1 && Player.Vaginas[0].Wetness < 4)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.SquicksWetly".Localized(
                     Player.VaginaDescript(0)
                ));

            if (Player.Vaginas[0].Wetness >= 4)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.GushesDripping".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.PlantCrotchFace".Localized());

            if (Player.Balls >= 2)
                if (Player.BallSize < 5)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.BallsDroopEyes".Localized());
                else
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.BallsObscureFace".Localized(
                         Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.HappyComplianceGrind".Localized());

            if (Player.CumQ() < 75)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.EnjoyingCutLoose".Localized());

            if (Player.CumQ() >= 75 && Player.CumQ() < 500)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.SmearingPreCum".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.FewDropsPre".Localized());

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.NearlyBounceAlarm".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.CumQ() >= 100)
            {
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.MorePreDrips".Localized());

                if (Player.Balls >= 2)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.BallsAnd".Localized());

                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.BeeBitchFace".Localized());
            }

            if ((Player.BreastRows.Count > 0) && (Player.BiggestBreastSize() >= 2))
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.FreeHandTend".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.ParagraphBreak".Localized());

            OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.MoanEcstasyClamping".Localized(
                 Player.VaginaDescript(0),

                 Player.VaginaDescript(0)
            ));

            if ((Player.BreastRows.Count > 0) && (Player.BiggestBreastSize() >= 2))
            {
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.PinchNippleHard".Localized());

                if (Player.BiggestLactation() > 1 && Player.BiggestLactation() <= 2)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.SquirtingMilkMidsection".Localized());

                if (Player.BiggestLactation() > 2 && Player.BiggestLactation() <= 3)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.SprayingMilkBitch".Localized());

                if (Player.BiggestLactation() > 3)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.HosingMilkEverywhere".Localized());

                if (Player.BiggestLactation() <= 1)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.Period".Localized());
            }

            if (Player.Balls >= 2)
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.WarmthMaleOrgasm".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.GoingToExplodeIdea".Localized());

            if (Player.CumQ() < 25)
            {
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.EruptPulsingRibbons".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.SpentRiseTongue".Localized());
            }

            if (Player.CumQ() >= 25 && Player.CumQ() < 250)
            {
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.EruptPulsingHands".Localized());

                if (Player.Balls >= 2)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.WithEmptyBalls".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.RiseOffHerShiver".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.WalkAwaySated".Localized());
            }

            if (Player.CumQ() >= 250 && Player.CumQ() < 500)
            {
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.EruptsWrithingHands".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.RiseMidOrgasmPaint".Localized());
                else
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.AimAtOvalFace".Localized());

                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.FlexesEachLoad".Localized());
            }

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.EruptsLikeVolcano".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.StandUpTongueFree".Localized());
                else
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.WordYou".Localized());

                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.PlungeDeepThroat".Localized());

                if (Player.Balls >= 2)
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.BallsEventuallyEmpty".Localized());
                else
                    OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.CumSupplyEmpties".Localized());

                OutputText("Scenes.BeeGirl.FutaRapesBeeGirl.TurnBackSoaked".Localized());
            }

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void BeeGirlRapeForTheDistinguishedGentleman()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.MaidenStaggersBack".Localized());
            else
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.KneesBuckleFingering".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.LostInThoughtWhere".Localized());

            if (Player.Speed < 50)
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.BracingStemLiftsLegs".Localized());
            else
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.BracingStemLiftsLegsAgain".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.ParagraphBreak".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.GrippingAbdomenFuzz".Localized());

            var choices = new List<int>();

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                choices.Add(0);

            if (Player.HasCock())
                choices.Add(1);

            if (Player.HasVagina())
                choices.Add(2);

            if (Player.BiggestBreastSize() >= 4)
                choices.Add(3);

            var select = choices[Random.Shared.Next(choices.Count)];

            if (select == 0)
            {
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.HandUpDownGrowth".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.SprayingFuzzyAbdomen".Localized());

                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.BeforeEnoughAhh".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 2f));
            }
            else
                if (select == 1)
                {
                    OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.ResistanceEndsGasp".Localized(
                         Player.CockDescript(0)
                    ));

                    if (Player.CockArea(0) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                        OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.BarelyWincesBump".Localized());

                    OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.WorkUpRhythm".Localized());
                }
                else
                    if (select == 2)
                        OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.TongueLollsBuzzing".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));
                    else
                        if (select == 3)
                            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.AbatingThrashing".Localized(
                                 Player.BreastDescript(0),

                                 Player.NippleDescript(0)
                            ));

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.PullBodyApart".Localized());

            if (Player.HasCock())
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.StrokeYour".Localized(
                     Appearance.CockNoun(BodyPartTypes.Human)
                ));
            else
                if (Player.HasVagina())
                    OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.RubYour".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.TweakNipples".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.LaughingClimax".Localized());

            if (Player.HasCock() || Player.LactationQ() >= 100 || Player.Wetness() >= 5)
                OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.SprayStickyFluids".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.PeriodBreak".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlRapeForTheDistinguishedGentleman.DamnEarDamage".Localized());

            if (Player.Speed < 50)
                Player.ChangeFatigue(20);

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void BeeAlternate()
        {
            ClearOutput();

            var sorted = Player.CockIndicesBySize();

            var x = sorted[0];

            var y = sorted.Count > 1 ? sorted[1] : sorted[0];

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.BeeGirl.BeeAlternate.DazedStumblesGrab".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.ParagraphBreakSixth".Localized());

                if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                    OutputText("Scenes.BeeGirl.BeeAlternate.LoosenUnleashLargest".Localized());

                if (Player.HasKnot(x))
                    OutputText("Scenes.BeeGirl.BeeAlternate.LoosenUnleashTaking".Localized());

                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.BeeGirl.BeeAlternate.LoosenUnleashEquine".Localized());

                if (Player.Cocks[x].Id == BodyPartTypes.Demon || Player.Cocks[x].TypeIndex > 4)
                    OutputText("Scenes.BeeGirl.BeeAlternate.LoosenUnleashGlans".Localized());

                ShowImage("beegirl-win-male");

                OutputText("Scenes.BeeGirl.BeeAlternate.EnjoyLoosening".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.BeeGirl.BeeAlternate.ParagraphBreakSixth".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.SmallerInHand".Localized(
                     Player.CockDescript(y),

                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.BeeGirl.BeeAlternate.WithdrawingOnceMore".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.BeeGirl.BeeAlternate.ParagraphBreakSixth".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.DrawingNearlyOut".Localized(
                     Player.CockDescript(y)
                ));

                OutputText("Scenes.BeeGirl.BeeAlternate.PleasantQuickening".Localized());
            }
            else
            {
                OutputText("Scenes.BeeGirl.BeeAlternate.FallingDazed".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.ParagraphBreakSixth".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.CloseEnoughGrabArms".Localized(
                     Player.MultiCockDescriptLight(),

                     Appearance.VaginaDescript(CurrentMonster, 0),

                     Appearance.VaginaDescript(CurrentMonster, 0)
                ));

                ShowImage("beegirl-win-male");

                OutputText("Scenes.BeeGirl.BeeAlternate.TakingSatisfiesIdea".Localized(
                     Player.CockDescript(y)
                ));

                if (Player.AverageCockThickness() > 2)
                    OutputText("Scenes.BeeGirl.BeeAlternate.StretchesSqueezing".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.ShakesScreams".Localized(
                     Player.MultiCockDescriptLight(),

                     Appearance.VaginaDescript(CurrentMonster, 0)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.BeeGirl.BeeAlternate.BallsHitAbdomen".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeAlternate.Period".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.ParagraphBreakSixth".Localized());

                if (Player.Cocks.Count >= 3)
                    OutputText("Scenes.BeeGirl.BeeAlternate.PoundRealizeJuices".Localized(
                         Appearance.VaginaDescript(CurrentMonster, 0),

                         Player.MultiCockDescriptLight(),

                         Player.MultiCockDescriptLight(),

                         Player.CockDescript(2),

                         Appearance.VaginaDescript(CurrentMonster, 0)
                    ));

                OutputText("Scenes.BeeGirl.BeeAlternate.ComplaintsFade".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.BeeGirl.BeeAlternate.YourBalls".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeAlternate.YourProstate".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.PlungeLargeRelease".Localized());

                if (Player.Cocks.Count >= 3)
                {
                    OutputText("Scenes.BeeGirl.BeeAlternate.BatteredGripsDown".Localized(
                         Appearance.VaginaDescript(CurrentMonster, 0)
                    ));

                    if (Player.Cocks.Count == 4)
                        OutputText("Scenes.BeeGirl.BeeAlternate.NotInsideShakes".Localized(
                             Player.CockDescript(3)
                        ));

                    if (Player.Cocks.Count > 4)
                        OutputText("Scenes.BeeGirl.BeeAlternate.NotInsideShake".Localized());
                }

                OutputText("Scenes.BeeGirl.BeeAlternate.ParagraphBreakSixth".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.BothSmellSex".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.BeeGirl.BeeAlternate.StuffingCocksArmor".Localized());
                else
                    OutputText("Scenes.BeeGirl.BeeAlternate.GloriousSpentDangling".Localized());

                OutputText("Scenes.BeeGirl.BeeAlternate.LeaveExhaustedWoman".Localized());
            }

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));

            CombatScene.CleanupAfterCombat();
        }

        public void CorruptNagaBitchesRapeABee()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            OutputText("Scenes.BeeGirl.CorruptNagaBitchesRapeABee.NowUnableFlit".Localized());

            OutputText("Scenes.BeeGirl.CorruptNagaBitchesRapeABee.StrugglesCoil".Localized());

            OutputText("Scenes.BeeGirl.CorruptNagaBitchesRapeABee.StareReptilianGaze".Localized());

            OutputText("Scenes.BeeGirl.CorruptNagaBitchesRapeABee.ShutsEyesFear".Localized());

            OutputText("Scenes.BeeGirl.CorruptNagaBitchesRapeABee.AsksHazilyStaring".Localized());

            OutputText("Scenes.BeeGirl.CorruptNagaBitchesRapeABee.TakesFewMinutesRelish".Localized());

            DoNext(() => NagaRapesPt2TheExtremeContinuationOfAwesome());
        }

        public void NagaRapesPt2TheExtremeContinuationOfAwesome()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(3) == 0))
            {
                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.ByNow".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.EachOf".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.YourCocks".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.IsWord".Localized());
                else
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.AreWord".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.AchingReleaseTearOff".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.TheyFreeFrom".Localized());
                else
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.ItFreesFrom".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.SlitBetweenLegs".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.PluralS".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.PeriodBreak".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.DropsToKneesVacant".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.OneOf".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.SlurpingDrivingWild".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.HandsCupSack".Localized());
                else
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.NotWord".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.EvenTakesToHilt".Localized(
                     Player.CockDescript(0),

                     (int)(Player.Cocks[0].Length)
                ));

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.HigherClimb".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.SackClenchesAs".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.ExplodeInsideChest".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.BurstSettlingBelly".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.RopeAfterRope".Localized());
                    else
                        if (Player.CumQ() < 2500)
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WavesPumpingBelly".Localized());
                        else
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.UnendingStream".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.EyesRollPullOut".Localized());

                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.CheckUnconsciousForm".Localized());
            }
            else
                if (Player.Gender == Gender.Female || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                {
                    if (Player.Vaginas[0].Wetness < 2)
                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WordDamp".Localized());
                    else
                        if (Player.Vaginas[0].Wetness < 4)
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WordWet".Localized());
                        else
                            if (Player.Vaginas[0].Wetness < 5)
                                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WordDripping".Localized());
                            else
                                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WordSoaking".Localized());

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WithLubricationDemand".Localized(
                         Player.VaginaDescript(0),

                         Appearance.BiggestBreastSizeDescript(Player)
                    ));

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.EyelidsFlutter".Localized());

                    if (Player.GetClitLength() > 1)
                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.EmergentSheath".Localized());

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.ParagraphBreak".Localized());

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.CryOutGSpot".Localized());

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.UrgeToPee".Localized());

                    if (Player.Vaginas[0].Wetness < 3)
                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.FewDrops".Localized());
                    else
                        if (Player.Vaginas[0].Wetness < 4)
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.AGush".Localized());
                        else
                            if (Player.Vaginas[0].Wetness < 5)
                                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.WaveAfterWave".Localized());
                            else
                                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.Buckets".Localized());

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.FemaleEjaculateSluice".Localized());

                    OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.BlinkBreakGlare".Localized());
                }
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.FantasiesCourse".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.HandOnHead".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.SingleFingerUnderside".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.EachOfShafts".Localized());
                        else
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.YourShaft".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.PulsingMovementInside".Localized());

                        if (Player.Cocks[0].Length > 12)
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.SurprisingElasticity".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.Period".Localized());

                        if (Player.Cocks.Count > 1)
                        {
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.LetGoFitted".Localized());

                            if (Player.CockTotal() > 2)
                                OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.FewPumpsEach".Localized());

                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.DrivingCloser".Localized());
                        }

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.ParagraphBreakAgain".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.GulpingSlurping".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.Jacking".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.FingerFuckingTandem".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.TrappedGazeTower".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.PressFlatHand".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.ExpressionBlankVacant".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.DespiteHypnoticMouth".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.PullArmBackWrist".Localized());

                        OutputText("Scenes.BeeGirl.NagaRapesPt2TheExtremeContinuationOfAwesome.PullHandOutWipe".Localized());
                    }

            if ((int)Player.Gender > 0)
                Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));

            CombatScene.CleanupAfterCombat();
        }

        public void BeeGirlsGetsDildoed()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.AdvanceOnYour".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.WordHapless".Localized());
            else
                OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.WordHorny".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.PullFavoriteToy".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.EyesLiddedYank".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.LittleStimulationGrows".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.DildoPlugsOvipositor".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.GetsThicker".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.CriesEcstasyGoo".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.ReleasePassesOut".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.FeelKindOfBad".Localized());
            else
                OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.LetterY".Localized());

            OutputText("Scenes.BeeGirl.BeeGirlsGetsDildoed.ReallyNeedCamp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((20 + Player.Libido / 5 + Player.Corruption / 10))));

            CombatScene.CleanupAfterCombat();
        }

        public void MilkAndHoneyAreKindaFunny()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlCombatWinsWithRape, CoC.Instance.Flags.Get(GameFlagIds.BeeGirlCombatWinsWithRape) + 1);

            Player.SlimeFeed();

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.SitUpDazed".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.WhatGoesWithHoney".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.PondersIsSex".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.ChuckleMilkGreat".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.WhatIsMilk".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.ChucklingLouder".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.SuckTitFindOut".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.QuestionsWhatWasThat".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.WasMilkGood".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.PleadsMore".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.OfferMoreHoney".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.NodsSuckingNipple".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.HoldJarPussy".Localized());

            OutputText("Scenes.BeeGirl.MilkAndHoneyAreKindaFunny.ShinyBlackVulva".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f), new DynStat(GameStats.Lust, -50f));

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void LayEggsInABeeSpiderLike()
        {
            ClearOutput();

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.StandOverCatch".Localized());

            if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs || Player.Body.Get<FacePart>().Id == BodyPartTypes.SpiderFangs)
                OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.WordFanged".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.SmileBreaksQuivers".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.LowerTorsoToward".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.ReverseAbovePinning".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.SteppingCloserGlances".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.EyesFocusPucker".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.SinkingFangsAntennae".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.LatheringHoney".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.PowerfulWetSlap".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.LewdGroanWrapArms".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.EachCockFragment".Localized());
                else
                    OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.YourVagina".Localized());

                OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.BlissStickyTrail".Localized());
            }

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.ShudderFirstEgg".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.GroansOrgasmicNote".Localized());

            if (Player.FertilizedEggs() == 0)
                OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.WordUnfertilized".Localized());
            else
                OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.WordFertilized".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.EggsLastOne".Localized());

            OutputText("Scenes.BeeGirl.LayEggsInABeeSpiderLike.PullFreeUnsteady".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

    }

    public enum BeeGirlAttitude
    {
        Unknown = 0,

        Talked = 1,

        TalkedAndLeft = 2,

        TalkedAndLeftTwice = 3,

        PlayerAfraid = 4,

        PlayerVoluntaryEgging = 5,

        PlayerDisgusted = 6,

        PlayerDuty = 7,
    }
}