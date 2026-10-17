using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Seasonal
{
    [Register<NieveScene>("coc:nieve")]
    public class NieveScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Core.Nieve);

        public string debugName => "Nieve";

        public string debugHint => "";

        public int stage
        {
            get => Save.GetInt("stage", 0);

            set => Save.SetInt("stage", value);
        }

        public bool coalFound => Save.GetBool("coalFound");

        public override void Entry()
        {
            UpgradeNieve();
        }

        public void Reset()
        {

            Save.SetInt("stage", 0);

            Save.SetInt("gender", (int)Gender.Genderless);

            Save.SetString("face", "");

            Save.SetInt("age", -1);

            Save.SetBool("coalFound", false);

            Save.SetBool("seenSpear", false);

            Save.SetBool("weaponTalked", false);

            Save.SetBool("guardCamp", false);

            Save.SetBool("guardAsked", false);

            Save.SetBool("kidsPlayed", false);

            Save.SetBool("kidsPlayedSingular", false);

            Save.SetBool("virgin", true);

            Save.SetBool("analVirgin", true);
        }

        public void UpgradeNieve()
        {

            if (stage == 0 && CoC.Instance.Flags.Get(GameFlagIds.NieveStage) > 0)
            {
                Save.SetInt("stage", CoC.Instance.Flags.Get(GameFlagIds.NieveStage));

                Save.SetString("face", CoC.Instance.Flags.Get(GameFlagIds.NieveMouth) ?? "");

                Save.SetInt("gender", CoC.Instance.Flags.Get(GameFlagIds.NieveGender));

                CoC.Instance.Flags.Set(GameFlagIds.NieveStage, 0);
            }
        }

        public bool NieveAvailable()
        {

            return Holidays.IsWinter() || Player.HasPerk(PerkIds.AChristmasCarol);
        }

        public bool NieveFollower()
        {

            UpgradeNieve();

            return NieveAvailable() && stage == 5;
        }

        public bool IceGuardian()
        {

            return NieveFollower() && Save.GetBool("guardCamp");
        }

        public bool IsKid()
        {

            return Save.GetInt("age", 0) == (int)Age.Child;
        }

        public bool IsMale()
        {

            return Save.GetInt("gender", 0) == (int)Gender.Male;
        }

        public string NieveMF(string man = "", string woman = "")
        {

            if (IsMale())
                return man;
            else
                return woman;
        }

        public string NieveAge(string kid = "", string adult = "")
        {

            if (IsKid())
                return kid;
            else
                return adult;
        }

        public string NieveMbFg(string man, string shota, string woman, string loli)
        {

            return NieveAge(NieveMF(shota, loli), NieveMF(man, woman));
        }

        public void RegisterParserTags()
        {

            RegisterMacro("nhe", NieveMF("he", "she"));

            RegisterMacro("nhim", NieveMF("him", "her"));

            RegisterMacro("nhis", NieveMF("his", "her"));

            RegisterMacro("nhis2", NieveMF("his", "hers"));

            RegisterMacro("nhers", NieveMF("his", "hers"));

            RegisterMacro("snowchild", IsKid());

            RegisterMacro("coaleyes", Save.GetString("face") == "coal");
        }

        public string NieveEyes(string coal = "", string gems = "")
        {

            if (Save.GetString("face") == "coal")
                return coal;
            else
                return gems;
        }

        public void SnowLadyActive()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Nieve.SnowLadyActive.ChillPervadesTheAir".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.StandUpFromBedroll".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.SnowExclaim".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.DashIntoFrostyArea".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.LayDownInSnow".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.SittingUpInField".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.ThinkingOverLogistics".Localized());

            OutputText("Scenes.Nieve.SnowLadyActive.SnowAddedToMenu".Localized());

            stage = 1;

            Go<CampScene>();
        }

        public void NieveBuilding()
        {
            ClearOutput();

            RegisterParserTags();

            if (Save.GetInt("age", 0) == -1)
            {
                OutputText("Scenes.Nieve.NieveBuilding.LookingAtOddPatch".Localized());

                OutputText("Scenes.Nieve.NieveBuilding.FirstStepToDecide".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Adult".Localized(), NieveSnowBig);

                AddButton(1, "Common.Button.Child".Localized(), NieveSnowLittle);

                AddButton(14, "Common.Button.Back".Localized(), NieveSnowDelay);
            }
            else
                if (Save.GetInt("gender", 0) <= (int)Gender.Genderless)
                {
                    OutputText("Scenes.Nieve.NieveBuilding.OntoSnowyPatch".Localized());

                    OutputText("Scenes.Nieve.NieveBuilding.SetYourselfToMaking".Localized());

                    OutputText("Scenes.Nieve.NieveBuilding.FinallyTheHead".Localized());

                    if (IsKid())
                    {
                        OutputText("Scenes.Nieve.NieveBuilding.SuddenlyItStrikes".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Snowboy".Localized(), NieveSnowMan);

                        AddButton(1, "Common.Button.Snowgirl".Localized(), NieveSnowWoman);
                    }
                    else
                    {
                        OutputText("Scenes.Nieve.NieveBuilding.NaughtyThoughtStrikes".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Snowman".Localized(), NieveSnowMan);

                        AddButton(1, "Common.Button.Snowwoman".Localized(), NieveSnowWoman);
                    }

                    AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
                }
                else
                    if (Save.GetString("face") == "")
                    {
                        OutputText("Scenes.Nieve.NieveBuilding.ApproachItAgainLooking".Localized());

                        OutputText("Scenes.Nieve.NieveBuilding.StillNeedsEyesAnd".Localized());

                        if (Player.HasItemAnywhere(ItemRegistry.Create(ConsumableIds.Coal)))
                            OutputText("Scenes.Nieve.NieveBuilding.ButGemsWord".Localized());
                        else
                            if (coalFound)
                                OutputText("Scenes.Nieve.NieveBuilding.FoundSomeInThe".Localized());
                            else
                                OutputText("Scenes.Nieve.NieveBuilding.WhoKnowsHowCommon".Localized());

                        OutputText("Scenes.Nieve.NieveBuilding.DecideAreAcceptableStand".Localized());

                        var noCoalText = "You don't have any coal";

                        if (Player.HasItemInStorage(ConsumableIds.Coal))
                            noCoalText += " with you right now. You think there should still be some in storage.";
                        else
                            if (coalFound)
                                noCoalText += ". You remember finding some in the mountains before.";
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredMountain) > 0)
                                    noCoalText += ". It stands to reason that it would be more easily found in rocky areas.";

                        ClearButtons();

                        AddButton(0, "Items.Coal.Short".Localized(), NieveCoalEyes);

                        AddButton(1, "Common.Button.Gems".Localized(), NieveGemEyes);

                        AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
                    }
                    else
                        if (stage < 4)
                        {
                            OutputText("Scenes.Nieve.NieveBuilding.ApproachNearlyFinished".Localized());

                            if (Player.HasKeyItem("Carrot"))
                            {
                                OutputText("Scenes.Nieve.NieveBuilding.GotThePerfect".Localized());

                                OutputText("Scenes.Nieve.NieveBuilding.EnjoyThePresence".Localized());

                                stage = 4;

                                Player.RemoveKeyItem("Carrot");

                                OutputText("Scenes.Nieve.NieveBuilding.RemovedCarrotItem".Localized());

                                Go<CampScene>();
                            }
                            else
                            {
                                OutputText("Scenes.Nieve.NieveBuilding.YetToFind".Localized());

                                Go<CampScene>();
                            }
                        }
                        else
                        {
                            OutputText("Scenes.Nieve.NieveBuilding.IsDoneNothings".Localized());

                            Go<CampScene>();
                        }
        }

        public void NieveCoalEyes()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Coal));

            OutputText("Scenes.Nieve.NieveCoalEyes.HappenToHaveSome".Localized());

            OutputText("Scenes.Nieve.NieveCoalEyes.SplitTheCoal".Localized());

            Save.SetString("face", "coal");

            stage = 3;

            ReturnToCamp(1);
        }

        public void NieveGemEyes()
        {
            ClearOutput();

            if (Player.Gems >= 9)
            {
                OutputText("Scenes.Nieve.NieveGemEyes.TakingHandfulOfGems".Localized());

                Player.Gems -= 9;

                Save.SetString("face", "gems");

                stage = 3;
            }
            else
                OutputText("Scenes.Nieve.NieveGemEyes.OpenUpAndFrown".Localized());

            ReturnToCamp(1);
        }

        public void NieveSnowDelay()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveSnowDelay.AfterFewMoreMoments".Localized());

            Go<CampScene>();
        }

        public void NieveSnowBig()
        {

            Save.SetInt("age", (int)Age.Adult);

            NieveBuilding();
        }

        public void NieveSnowLittle()
        {

            Save.SetInt("age", (int)Age.Child);

            NieveBuilding();
        }

        public void NieveSnowWoman()
        {
            ClearOutput();

            if (IsKid())
                OutputText("Scenes.Nieve.NieveSnowWoman.SomethingStrikesYouAs".Localized());
            else
                OutputText("Scenes.Nieve.NieveSnowWoman.GrinMischievously".Localized());

            OutputText("Scenes.Nieve.NieveSnowWoman.StillNeedsAFace".Localized());

            ReturnToCamp(1);

            stage = 2;

            Save.SetInt("gender", (int)Gender.Female);
        }

        public void NieveSnowMan()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveSnowMan.DecideToLeaveIt".Localized());

            OutputText("Scenes.Nieve.NieveSnowMan.StillNeedsAFace".Localized());

            ReturnToCamp(1);

            stage = 2;

            Save.SetInt("gender", (int)Gender.Male);
        }

        public void FindACarrot()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.FindACarrot.ExploreTheFarm".Localized());

            OutputText("Scenes.Nieve.FindACarrot.WhitneyPassesByAsk".Localized());

            OutputText("Scenes.Nieve.FindACarrot.StashTheCarrot".Localized());

            OutputText("Scenes.Nieve.FindACarrot.GainedCarrotItem".Localized());

            Player.CreateKeyItem("Carrot", 0, 0, 0, 0);

            ReturnToCamp(1);
        }

        public void NieveComesToLife()
        {
            ClearOutput();

            RegisterParserTags();

            stage = 5;

            OutputText("Scenes.Nieve.NieveComesToLife.AwakenWithAShiver".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.NothingToIndicateAny".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.CreepingCloserPile".Localized());

            if (Save.GetInt("gender", 0) == (int)Gender.Male)
                OutputText("Scenes.Nieve.NieveComesToLife.AppearsToBeMale".Localized());
            else
                OutputText("Scenes.Nieve.NieveComesToLife.AppearsToBeFemale".Localized());

            if (IsKid())
                OutputText("Scenes.Nieve.NieveComesToLife.OnlyAChild".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.PureWhiteHairSpills".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.ReadyYourAndGive".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.SoftEscapeLips".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.RespondingCautiously".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.OrbsFallOnYou".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.NowYourTimeTo".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.FrownsSighsMournfully".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.FeelSorryCutOff".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.SaysHappilyExtending".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.RaiseAnEyebrow".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.LooksAroundLandscape".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.NodThoughYouSay".Localized());

            OutputText("Scenes.Nieve.NieveComesToLife.BeginsWord".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesKeepNieve);

            AddButton(1, "Common.Button.No".Localized(), NoNoKeepNieve);
        }

        public void YesKeepNieve()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.YesKeepNieve.ActuallyLaughAtQuestion".Localized());

            if (silly)
                OutputText("Scenes.Nieve.YesKeepNieve.FreeFollowerNoWorries".Localized());

            OutputText("Scenes.Nieve.YesKeepNieve.DoesntSeemThreat".Localized());

            OutputText("Scenes.Nieve.YesKeepNieve.BeamsYouCanGet".Localized());

            OutputText("Scenes.Nieve.YesKeepNieve.ReturnToCampProper".Localized());

            OutputText("Scenes.Nieve.YesKeepNieve.AddedToLoversMenu".Localized());

            ReturnToCamp(1);
        }

        public void NoNoKeepNieve()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NoNoKeepNieve.ShakeHeadOfCourse".Localized());

            OutputText("Scenes.Nieve.NoNoKeepNieve.LooksDisappointedNods".Localized());

            OutputText("Scenes.Nieve.NoNoKeepNieve.BlindingFlashOfLight".Localized());

            stage = -1;

            ReturnToCamp(1);
        }

        public void NieveCampDescs()
        {

            RegisterParserTags();

            OutputText("Scenes.Nieve.NieveCampDescs.ParagraphBreak".Localized());

            switch (View.Time.Hours)
            {
                case 6:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SittingCrossLegged".Localized());
                }

                break;

                case 7:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.GigglingAndLaughing".Localized());
                }

                break;

                case 8:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SittingInTheWhite".Localized());
                }

                break;

                case 9:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SprawledOutInThe".Localized());
                }

                break;

                case 10:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.IceSpiritSitsQuietly".Localized());
                }

                break;

                case 11:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.InYourCampPoking".Localized());
                }

                break;

                case 12:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.CompressingSnowballs".Localized());
                }

                break;

                case 13:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SittingCrossLeggedAgain".Localized());
                }

                break;

                case 14:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.InUsualSpotSeemingly".Localized());
                }

                break;

                case 15:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.IceSpiritWhirling".Localized());

                Save.SetBool("seenSpear", true);
                }

                break;

                case 16:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.AtFirstNowhereTo".Localized());
                }

                break;

                case 17:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.AppearsToHaveConstructed".Localized());
                }

                break;

                case 18:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SeemsToBeWorking".Localized());

                Save.SetBool("seenSpear", true);
                }

                break;

                case 19:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SittingAtTheEdge".Localized());
                }

                break;

                case 20:
                {
                OutputText("Scenes.Nieve.NieveCampDescs.SurprisinglyOutsideCold".Localized());
                }

                break;
            }

            if (true )
                if (CampGuards.IsGuard("Nieve"))
                    OutputText("Scenes.Nieve.NieveCampDescs.SittingOnTopOf".Localized());
                else
                    OutputText("Scenes.Nieve.NieveCampDescs.HearNieveSleeping".Localized());

            OutputText("Scenes.Nieve.NieveCampDescs.ParagraphBreakAgain".Localized());
        }

        public void ApproachNieve(bool fromCamp = false)
        {
            ClearOutput();

            RegisterParserTags();

            if (fromCamp)
            {
                OutputText("Scenes.Nieve.ApproachNieve.WaveAtNieve".Localized(
                    NieveMbFg("man", "boy", "woman", "girl")
                ));

                OutputText("Scenes.Nieve.ApproachNieve.StopsGivesFriendly".Localized());
            }

            OutputText("Scenes.Nieve.ApproachNieve.WhatCanForYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), NieveAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), () => NieveTalk());

            AddButton(2, "Common.Button.Sex".Localized(), NieveSexMenu);

            if (Save.GetBool("weaponTalked"))
                AddButton(4, Save.GetBool("guardCamp") ? "Common.Button.StopGuarding".Localized() : "Common.Button.GuardCamp".Localized(), NieveGuardToggle);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void NieveAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveAppearance.TallSkinIsPale".Localized(
                NieveMbFg("a dark blue cock that appears to be about nine inches long", "a thin, light blue cock that appears to be about four inches long", "a pair of dark blue pussy lips", "a puffy, light blue mound with a pristine slit")
            ));

            if (silly)
                OutputText("Scenes.Nieve.NieveAppearance.PresumeAButthole".Localized());

            OutputText("Scenes.Nieve.NieveAppearance.StomachFlatAndToned".Localized(
                NieveMbFg("as is his chest", "as is his chest", "and she possesses a pair of perky B-cup breasts", "as is her chest"),

                NieveMF("barely goes past his ears", "tumbles down past her shoulders")
            ));

            ApproachNieve();
        }

        public void NieveTalk(string current = "")
        {

            ClearButtons();

            if (Save.GetBool("seenSpear"))
                if (Save.GetBool("weaponTalked"))
                    AddNextButton("Common.Button.IceWeapon".Localized(), NieveWeaponTalk);
                else
                    AddNextButton("Items.Spear.Short".Localized(), NieveWeaponTalk);

            AddNextButton("Common.Button.Slave".Localized(), NieveSlaveTalk);

            AddNextButton("Codex.Category.Magic".Localized(), NieveMagicTalk);

            AddNextButton("Common.Button.Form".Localized(), NieveFormTalk);

            AddNextButton("Common.Button.BodyHeat".Localized(), NieveBodyHeatTalk);

            AddButton(14, "Common.Button.Back".Localized(), () => ApproachNieve());
        }

        public void NieveWeaponTalk()
        {
            ClearOutput();

            if (!Save.GetBool("weaponTalked"))
            {
                Save.SetBool("weaponTalked", true);

                if (View.Time.Hours == 15)
                    OutputText("Scenes.Nieve.NieveWeaponTalk.AskAboutTheSpear".Localized());
                else
                    if (View.Time.Hours == 18)
                        OutputText("Scenes.Nieve.NieveWeaponTalk.AskSpearAgain".Localized());
                    else
                        OutputText("Scenes.Nieve.NieveWeaponTalk.RememberingIceSpear".Localized());

                OutputText("Scenes.Nieve.NieveWeaponTalk.IceSpiritTurnsAround".Localized());

                OutputText("Scenes.Nieve.NieveWeaponTalk.CantHelpButWonder".Localized());

                OutputText("Scenes.Nieve.NieveWeaponTalk.FlurryOfSnowSurrounds".Localized(
                    NieveMbFg("ice spirit", "little boy", "ice spirit", "little girl")
                ));

                OutputText("Scenes.Nieve.NieveWeaponTalk.LettingHugLinger".Localized());

                OutputText("Scenes.Nieve.NieveWeaponTalk.WannaTryOne".Localized());

                OutputText("Scenes.Nieve.NieveWeaponTalk.DoWantNieveCraft".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Yes".Localized(), NieveWeaponChoose);
            }
            else
            {
                var weapon = IceWeapon.PlayerHasIceWeapon();

                if (weapon != "")
                {
                    OutputText("Scenes.Nieve.NieveWeaponTalk.FixUpYourWeapon".Localized(
                        weapon
                    ));

                    ClearButtons();

                    AddNextButton("Common.Button.Yes".Localized(), () => NieveWeaponGet());
                }
                else
                {
                    OutputText("Scenes.Nieve.NieveWeaponTalk.MakeIceWeapon".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.Yes".Localized(), NieveWeaponChoose);
                }
            }

            AddNextButton("Common.Button.No".Localized(), () => NieveTalk());
        }

        public void NieveWeaponChoose()
        {

            OutputText("Scenes.Nieve.NieveWeaponChoose.WhatTypeOfWeapon".Localized());

            ClearButtons();

            var i = 0;

            while (i < IceWeapon.TypeStrings.Length)
            {
                var weapon = Utils.Utils.CapitalizeFirstLetter(IceWeapon.TypeStrings[i]);

                AddNextButton(weapon, () => NieveWeaponGet(weapon));

                i += 1;
            }

            AddButton(14, "Common.Button.Back".Localized(), () => NieveTalk());
        }

        public void NieveWeaponGet(string weapon = "")
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IceWeaponTimer, 10);

            if (weapon == "")
            {
                OutputText("Scenes.Nieve.NieveWeaponGet.GraspsYourChanneling".Localized(
                    IceWeapon.PlayerHasIceWeapon()
                ));

                OutputText("Scenes.Nieve.NieveWeaponGet.AsGoodAsNew".Localized());

                ApproachNieve();
            }
            else
            {
                OutputText("Scenes.Nieve.NieveWeaponGet.SitsDownInSnow".Localized(
                    weapon.ToLower()
                ));

                OutputText("Scenes.Nieve.NieveWeaponGet.BeamsUpYouProudly".Localized(
                    weapon.ToLower()
                ));

                Player.Inventory.Add(ItemRegistry.Create(new Identifier("coc", "ice_" + weapon.ToLower())), 1);

                ApproachNieve();
            }
        }

        public void NieveSlaveTalk()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveSlaveTalk.AskIfShesReally".Localized());

            OutputText("Scenes.Nieve.NieveSlaveTalk.IceSpiritGivesYou".Localized());

            OutputText("Scenes.Nieve.NieveSlaveTalk.BitMoreComplicated".Localized());

            OutputText("Scenes.Nieve.NieveSlaveTalk.HandsOnHipsCocking".Localized());

            NieveTalk("Slave");
        }

        public void NieveMagicTalk()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveMagicTalk.SeenNieveUseMagic".Localized());

            OutputText("Scenes.Nieve.NieveMagicTalk.BeamsEyesClosed".Localized());

            OutputText("Scenes.Nieve.NieveMagicTalk.RollsEyesKnowWhat".Localized());

            OutputText("Scenes.Nieve.NieveMagicTalk.SomewhatAnswerIs".Localized());

            OutputText("Scenes.Nieve.NieveMagicTalk.OfCourseCantYou".Localized(
                NieveMbFg("chuckles", "giggles", "giggles", "giggles")
            ));

            NieveTalk("Magic");
        }

        public void NieveFormTalk()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveFormTalk.OccursToYouThat".Localized());

            OutputText("Scenes.Nieve.NieveFormTalk.RepliesWantedSoI".Localized());

            OutputText("Scenes.Nieve.NieveFormTalk.ProbablyHadSomeSort".Localized());

            OutputText("Scenes.Nieve.NieveFormTalk.ButDontHaveA".Localized());

            OutputText("Scenes.Nieve.NieveFormTalk.SuddenlyStepsForward".Localized());

            NieveTalk("Form");
        }

        public void NieveBodyHeatTalk()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveBodyHeatTalk.GivenMadeOfSnow".Localized());

            OutputText("Scenes.Nieve.NieveBodyHeatTalk.MightBeMore".Localized());

            OutputText("Scenes.Nieve.NieveBodyHeatTalk.OneWorryPutRest".Localized());

            NieveTalk("Body Heat");
        }

        public void NieveGuardToggle()
        {
            ClearOutput();

            if (Save.GetBool("guardCamp"))
            {
                OutputText("Scenes.Nieve.NieveGuardToggle.TellNieveThatDoesnt".Localized());

                OutputText("Scenes.Nieve.NieveGuardToggle.UnderstoodMaster".Localized());

                Save.SetBool("guardCamp", false);
            }
            else
            {
                if (!Save.GetBool("guardAsked"))
                    OutputText("Scenes.Nieve.NieveGuardToggle.AfterSeeingHerSkills".Localized());
                else
                    OutputText("Scenes.Nieve.NieveGuardToggle.AskIfCanKeep".Localized());

                OutputText("Scenes.Nieve.NieveGuardToggle.SmilesHappilyEagerly".Localized());

                Save.SetBool("guardAsked", true);

                Save.SetBool("guardCamp", true);
            }

            ApproachNieve();
        }

        public void NieveSexMenu()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveSexMenu.WhatWillYouDo".Localized());

            ClearButtons();

            if (Save.GetInt("gender", 0) == (int)Gender.Female)
            {
                var canFuck = Player.HasCock() || Player.GetClitLength() >= 3.5f;

                AddButton(0, "Common.Button.LickHer".Localized(), LickNieve);

                AddButton(1, "Common.Button.FuckHer".Localized(), (Save.GetBool("virgin") ? DefloralNieve : FuckNieve));

                AddButton(2, "Common.Button.Fuckherass".Localized(), AnalNieve);

                AddButton(3, "Common.Button.GetFucked".Localized(), NieveStrapon);
            }

            if (Save.GetInt("gender", 0) == (int)Gender.Male)
            {
                AddButton(0, "Common.Button.SuckHim".Localized(), SuckNieveOff);

                AddButton(1, "Common.Button.FuckHisAss".Localized(), AnalNieve);

                AddButton(2, "Common.Button.GetFucked".Localized(), NieveFucksYou);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => ApproachNieve());
        }

        public void LickNieve()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.LickNieve.ExplainYourIntentions".Localized());

            OutputText("Scenes.Nieve.LickNieve.GrabAroundTheWaist".Localized());

            OutputText("Scenes.Nieve.LickNieve.LaysDownInIcy".Localized());

            OutputText("Scenes.Nieve.LickNieve.LipsLeaveTheNipple".Localized());

            if (silly && Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Nieve.LickNieve.PauseAwkwardly".Localized());

                OutputText("Scenes.Nieve.LickNieve.AnotherPauseLetOut".Localized());

                OutputText("Scenes.Nieve.LickNieve.TakesYouBetterPart".Localized());

                if (CampScene.BuiltBarrel)
                {
                    OutputText("Scenes.Nieve.LickNieve.AWaterBarrel".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) == 1)
                        OutputText("Scenes.Nieve.LickNieve.AnemoneSpawnGiggles".Localized());
                    else
                        OutputText("Scenes.Nieve.LickNieve.TakesALadleFull".Localized());
                }
                else
                    OutputText("Scenes.Nieve.LickNieve.SplashesSomeWater".Localized());

                OutputText("Scenes.Nieve.LickNieve.AllowingYouToPull".Localized());

                OutputText("Scenes.Nieve.LickNieve.BothThoroughlyFlustered".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(-5 - Player.Sensitivity / 5)));
            }
            else
            {
                OutputText("Scenes.Nieve.LickNieve.LetsOutLittleGasps".Localized());

                if (!Player.HasLongTongue())
                    OutputText("Scenes.Nieve.LickNieve.DoesntGetFarBut".Localized());
                else
                    OutputText("Scenes.Nieve.LickNieve.EyesWidenYouSnake".Localized());

                OutputText("Scenes.Nieve.LickNieve.AfterMinuteOfThis".Localized());

                OutputText("Scenes.Nieve.LickNieve.NotQuiteWilling".Localized());

                OutputText("Scenes.Nieve.LickNieve.ExclaimsHeadBoltingUp".Localized());

                OutputText("Scenes.Nieve.LickNieve.RightWhenYoure".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Nieve.LickNieve.OrCloseToIt".Localized());

                OutputText("Scenes.Nieve.LickNieve.StraddlingYourFace".Localized());

                OutputText("Scenes.Nieve.LickNieve.TryProtestButAll".Localized());

                OutputText("Scenes.Nieve.LickNieve.NotWantingBeBuried".Localized());

                OutputText("Scenes.Nieve.LickNieve.ThankfullyDoesntTakeLong".Localized());

                OutputText("Scenes.Nieve.LickNieve.ToTopAllOff".Localized());

                OutputText("Scenes.Nieve.LickNieve.GetToYourFeet".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

                if (Player.Lust < 33)
                    Player.ApplyStats(new DynStat(GameStats.Lust, 33f));
            }

            ReturnToCamp(1);
        }

        public void SuckNieveOff()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.SuckNieveOff.LookingChillyCompanion".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.LeaningInGiveHim".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.KneelingLookUpWith".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.EvenAsYouLick".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.EnjoyingYouSoMuch".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.HissesScentedSteam".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.SlitherYourTongue".Localized());

            OutputText("Scenes.Nieve.SuckNieveOff.LettingOutDeepSigh".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            if (Player.Lust < 33)
                Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            ReturnToCamp(1);
        }

        public void DefloralNieve()
        {
            ClearOutput();

            RegisterMacro("analVirgin", Save.GetBool("analVirgin"));

            OutputText("Scenes.Nieve.DefloralNieve.SmilesTiltingHerHead".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.ConfirmYouDoNot".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.VoiceSincereAndAssured".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.BlushTouchesCheeks".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.GuidesHerIntoEmbrace".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.SheAssures".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.PulsingThrobOfCock".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.EyesFlutterTrembling".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.LetHerMove".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.PantsBrightSmile".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.FrontLegsYourHands".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.ClimaxTakesSurprise".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.SlumpsAgainstKissing".Localized());

            OutputText("Scenes.Nieve.DefloralNieve.LieAWhile".Localized());

            Save.SetBool("virgin", false);

            Player.Orgasm("DickVaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void FuckNieve()
        {
            ClearOutput();

            float lengthLimit = (IsKid() ? 7 : 10);

            var widthLimit = (IsKid() ? 1.5f : 2.5f);

            var x = Player.CockThatFits(lengthLimit, "length");

            if (x < 0 && Player.GetClitLength() < 3.5f)
                x = Player.SmallestCockIndex();

            RegisterMacro("cockclit", Player.CockClit(x));

            var length = (x < 0 ? Player.GetClitLength() : Player.Cocks[x].Length);

            OutputText("Scenes.Nieve.FuckNieve.LookYourNakedIcy".Localized());

            OutputText("Scenes.Nieve.FuckNieve.FirstOrderOntoHer".Localized());

            OutputText("Scenes.Nieve.FuckNieve.NotWastingAnyTime".Localized());

            if (x >= 0)
            {
                if (length < (IsKid() ? 4 : 6))
                    OutputText("Scenes.Nieve.FuckNieve.SaysCatchingBreath".Localized());
                else
                    if (length >= (IsKid() ? 14 : 24))
                        OutputText("Scenes.Nieve.FuckNieve.HasTakeABreath".Localized());
                    else
                        OutputText("Scenes.Nieve.FuckNieve.SmacksLipsAndTakes".Localized());
            }
            else
                if (length < (IsKid() ? 4 : 6))
                    OutputText("Scenes.Nieve.FuckNieve.SaysCatchingBreathAgain".Localized());
                else
                    if (length >= (IsKid() ? 14 : 24))
                        OutputText("Scenes.Nieve.FuckNieve.HasTakeBreathAgain".Localized());
                    else
                        OutputText("Scenes.Nieve.FuckNieve.SmacksLipsTakesAgain".Localized());

            OutputText("Scenes.Nieve.FuckNieve.SheDivesBack".Localized());

            if (x >= 0)
            {
                var __sw0_1 = Player.Cocks[x].Id;

                if (__sw0_1 == BodyPartTypes.Human)
                    OutputText("Scenes.Nieve.FuckNieve.CockSeemsToTake".Localized());

                if (__sw0_1 == BodyPartTypes.Horse)
                    OutputText("Scenes.Nieve.FuckNieve.FawnsOverHorsecock".Localized());

                if (__sw0_1 == BodyPartTypes.Demon)
                    OutputText("Scenes.Nieve.FuckNieve.PurpleDemonicShaft".Localized());

                if (__sw0_1 == new Identifier("coc", "tentacle"))
                    OutputText("Scenes.Nieve.FuckNieve.DoesNotSeemOff".Localized());

                if (__sw0_1 == BodyPartTypes.Cat)
                    OutputText("Scenes.Nieve.FuckNieve.BarbsOnYourCock".Localized());

                if (__sw0_1 == BodyPartTypes.Lizard)
                    OutputText("Scenes.Nieve.FuckNieve.FawnsOverScaled".Localized());

                if (__sw0_1 == BodyPartTypes.Anemone)
                    OutputText("Scenes.Nieve.FuckNieve.GigglesWheneverOne".Localized());

                if (__sw0_1 == BodyPartTypes.Kangaroo)
                    OutputText("Scenes.Nieve.FuckNieve.TakesNieveLittleWhile".Localized());

                if (true )
                    if (Player.HasKnot(x))
                    {
                        OutputText("Scenes.Nieve.FuckNieve.EyesTheKnot".Localized());

                        if (Player.Cocks[x].KnotThickness > widthLimit * 2 || length > lengthLimit)
                            OutputText("Scenes.Nieve.FuckNieve.EventuallyAdmitsDefeat".Localized());
                        else
                            OutputText("Scenes.Nieve.FuckNieve.TakesFewGood".Localized());
                    }
                    else
                        OutputText("Scenes.Nieve.FuckNieve.WorksOnMutantCock".Localized());
            }
            else
            {
                OutputText("Scenes.Nieve.FuckNieve.NuzzlesLavishes".Localized());

                if (length < 5)
                    OutputText("Scenes.Nieve.FuckNieve.JustLittleCock".Localized());
                else
                    if (length < 9)
                        OutputText("Scenes.Nieve.FuckNieve.JustLikeACock".Localized());
                    else
                        OutputText("Scenes.Nieve.FuckNieve.EvenMenGetJealous".Localized());

                OutputText("Scenes.Nieve.FuckNieve.PeriodSayEnd".Localized());
            }

            OutputText("Scenes.Nieve.FuckNieve.EventuallyCantHandle".Localized());

            if (x >= 0)
                OutputText("Scenes.Nieve.FuckNieve.QuiversAndSpurts".Localized());

            OutputText("Scenes.Nieve.FuckNieve.ComeDownFromYour".Localized());

            if (x < 0)
                OutputText("Scenes.Nieve.FuckNieve.HasBitOfFeminine".Localized());
            else
                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Nieve.FuckNieve.HasBitOfCum".Localized());
                else
                    if (Player.CumQ() <= 400)
                        OutputText("Scenes.Nieve.FuckNieve.HasASteadyStream".Localized());
                    else
                        if (Player.CumQ() < 600)
                            OutputText("Scenes.Nieve.FuckNieve.PattingHerStomach".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Nieve.FuckNieve.StomachSlightlyDistended".Localized());
                            else
                                OutputText("Scenes.Nieve.FuckNieve.StomachRatherLarge".Localized());

            OutputText("Scenes.Nieve.FuckNieve.WithAGrinTell".Localized());

            OutputText("Scenes.Nieve.FuckNieve.CommandFallsBackwards".Localized());

            if (x >= 0)
                OutputText("Scenes.Nieve.FuckNieve.WithYourCockClit".Localized());
            else
                OutputText("Scenes.Nieve.FuckNieve.WithYours".Localized());

            OutputText("Scenes.Nieve.FuckNieve.JustSlippingTheTip".Localized());

            OutputText("Scenes.Nieve.FuckNieve.RightAsSheOpens".Localized());

            if (length > lengthLimit)
                OutputText("Scenes.Nieve.FuckNieve.AtLeastFarIt".Localized());

            OutputText("Scenes.Nieve.FuckNieve.WetSchluckTheWords".Localized());

            OutputText("Scenes.Nieve.FuckNieve.TakeThisToThe".Localized());

            if (x >= 0)
            {
                if (length < (IsKid() ? 4 : 6))
                    OutputText("Scenes.Nieve.FuckNieve.MeagerPenisIsntMuch".Localized());
                else
                    if (length < (IsKid() ? 6 : 9))
                        OutputText("Scenes.Nieve.FuckNieve.AtThisAngleManage".Localized());
                    else
                        if (length < lengthLimit)
                            OutputText("Scenes.Nieve.FuckNieve.CuntFeelsLikeAn".Localized());
                        else
                            OutputText("Scenes.Nieve.FuckNieve.ThoughCantFitYour".Localized());
            }
            else
                if (length < (IsKid() ? 4 : 6))
                    OutputText("Scenes.Nieve.FuckNieve.OversizedClitIsnt".Localized());
                else
                    if (length < (IsKid() ? 6 : 9))
                        OutputText("Scenes.Nieve.FuckNieve.AtAngleManageAgain".Localized());
                    else
                        if (length < lengthLimit)
                            OutputText("Scenes.Nieve.FuckNieve.CuntFeelsAnAgain".Localized());
                        else
                            OutputText("Scenes.Nieve.FuckNieve.ThoughCantYourAgain".Localized());

            OutputText("Scenes.Nieve.FuckNieve.BodyTensesUnderneath".Localized());

            OutputText("Scenes.Nieve.FuckNieve.IcyColdChill".Localized());

            if (x < 0)
                OutputText("Scenes.Nieve.FuckNieve.SuddenlyFeelsWarm".Localized());
            else
            {
                OutputText("Scenes.Nieve.FuckNieve.AchesAndPulsesWith".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Nieve.FuckNieve.SpillingMeagerOffering".Localized());
                else
                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Nieve.FuckNieve.EjectingWhiteHotJizz".Localized());
                    else
                        if (Player.CumQ() < 400)
                            OutputText("Scenes.Nieve.FuckNieve.PaintingTheInsides".Localized());
                        else
                            if (Player.CumQ() < 700)
                                OutputText("Scenes.Nieve.FuckNieve.PearlyWhiteCumShoot".Localized());
                            else
                                OutputText("Scenes.Nieve.FuckNieve.EruptingInsideFilling".Localized());

                OutputText("Scenes.Nieve.FuckNieve.SentencePeriod".Localized());
            }

            OutputText("Scenes.Nieve.FuckNieve.SlumpBackwardsSplaying".Localized());

            OutputText("Scenes.Nieve.FuckNieve.NotesWithASmile".Localized());

            Player.Orgasm(x < 0 ? "Common.Button.Vaginal".Localized() : "Common.Button.Dick".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void NieveFucksYou()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveFucksYou.ArrivalAtNievesPart".Localized());

            OutputText("Scenes.Nieve.NieveFucksYou.ToWhichYouNod".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Nieve.NieveFucksYou.TellingHimYouFind".Localized());
            else
                OutputText("Scenes.Nieve.NieveFucksYou.LettingCreationKnow".Localized());

            OutputText("Scenes.Nieve.NieveFucksYou.BowsSubserviently".Localized());

            if (silly)
                OutputText("Scenes.Nieve.NieveFucksYou.HeSayHisKind".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Nieve.NieveFucksYou.IsDrippingWet".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Nieve.NieveFucksYou.AlsoRockHard".Localized());

            if (Player.HasVagina())
            {
                ClearButtons();

                AddButton(0, "Common.Button.Anal".Localized(), TakeNieveAnal);

                AddButton(1, "Common.Button.Vaginal".Localized(), TakeNieveVaginal);
            }
            else
                TakeNieveAnal();
        }

        public void TakeNieveAnal()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.TakeNieveAnal.LetYourWintryKnow".Localized());

            OutputText("Scenes.Nieve.TakeNieveAnal.AfterFewExcitingMinutes".Localized());

            Player.ButtChange(IsKid() ? 4 : 9, true, true, false);

            OutputText("Scenes.Nieve.TakeNieveAnal.IncreasesTempoUntil".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Nieve.TakeNieveAnal.BendsForwardKeeping".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Nieve.TakeNieveAnal.ContinuesToPoundAway".Localized());

            OutputText("Scenes.Nieve.TakeNieveAnal.GruntAsHeProds".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Nieve.TakeNieveAnal.BodyUndulatingSeizing".Localized());

                if (Player.CumQ() < 200)
                    OutputText("Scenes.Nieve.TakeNieveAnal.LeavingSmallPuddle".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Nieve.TakeNieveAnal.StartingToDress".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Nieve.TakeNieveAnal.SoakingVaginaMakes".Localized());

            OutputText("Scenes.Nieve.TakeNieveAnal.SentencePeriod".Localized());

            OutputText("Scenes.Nieve.TakeNieveAnal.BothRestInPosition".Localized());

            Player.Orgasm("All");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void TakeNieveVaginal()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.TakeNieveVaginal.AlreadyWetFromThe".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Nieve.TakeNieveVaginal.GiveHimDirtyLook".Localized());
            else
                OutputText("Scenes.Nieve.TakeNieveVaginal.AskToyIfHe".Localized());

            OutputText("Scenes.Nieve.TakeNieveVaginal.ObligesWithAQuickly".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Nieve.TakeNieveVaginal.GentlyMovingOutOf".Localized());

            OutputText("Scenes.Nieve.TakeNieveVaginal.SlamsHisBlueRod".Localized());

            Player.CuntChange(IsKid() ? 4 : 9, true, true, false);

            if (Player.HasCock())
                OutputText("Scenes.Nieve.TakeNieveVaginal.DuringPoundingGrabs".Localized());

            if (Player.LactationQ() >= 10)
                OutputText("Scenes.Nieve.TakeNieveVaginal.ReachesOutWithFree".Localized());

            OutputText("Scenes.Nieve.TakeNieveVaginal.EnjoyTheRide".Localized());

            OutputText("Scenes.Nieve.TakeNieveVaginal.OneLastTimeCum".Localized());

            OutputText("Scenes.Nieve.TakeNieveVaginal.ComeToYourSenses".Localized());

            OutputText("Scenes.Nieve.TakeNieveVaginal.AnotherHourPasses".Localized());

            Player.Orgasm("All");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void NieveStrapon()
        {
            ClearOutput();

            OutputText("Scenes.Nieve.NieveStrapon.WellEvenThoughShes".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.WontBeAProblem".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.IceSpiritWavesHands".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.GoodForYou".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.SoManyPossibilities".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.TellNieveSitUp".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.SaysNothingButSmirk".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.WheneverYoureReady".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.GivesHipsALittle".Localized());

            Player.CuntChange(7, true, true, false);

            OutputText("Scenes.Nieve.NieveStrapon.NoBetterHandholds".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.ColdStartingToPermeate".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.DoesBestToMilk".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.ForHowLongYoure".Localized());

            OutputText("Scenes.Nieve.NieveStrapon.LooksLikeShesGoing".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void AnalNieve()
        {
            ClearOutput();

            float lengthSmall = (IsKid() ? 4.5f : 6.0f);

            float girthSmall = (IsKid() ? 1.0f : 1.5f);

            float lengthLarge = (IsKid() ? 6.0f : 8.0f);

            float girthLarge = (IsKid() ? 1.5f : 2.5f);

            OutputText("Scenes.Nieve.AnalNieve.PullingTheIcyClosely".Localized(
                IsMale()&& !IsKid()
            ));

            OutputText("Scenes.Nieve.AnalNieve.LingersAroundAdmiring".Localized());

            OutputText("Scenes.Nieve.AnalNieve.BlueTendrilIsShock".Localized(
                Player.BiggestCockLength() < lengthSmall,

                girthSmall,

                lengthLarge,

                girthLarge,

                lengthSmall,

                NieveMbFg("man", "boy", "woman", "girl")
            ));

            OutputText("Scenes.Nieve.AnalNieve.YoursToEnjoyTurns".Localized(
                !IsMale()&& !IsKid(),

                !IsMale() && !IsKid(),

                lengthLarge
            ));

            OutputText("Scenes.Nieve.AnalNieve.TakeMeImYours".Localized());

            OutputText("Scenes.Nieve.AnalNieve.DragYourLengthOut".Localized(
                IsMale(),

                IsMale()
            ));

            Save.SetBool("analVirgin", false);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void NieveCampKids()
        {
            ClearOutput();

            var kidTypes = new List<string>();

            var kidString = "";

            int typeCount = 0;

            var breakdown = CampScene.GetCampKidBreakdown();

            int totalKids = breakdown.total;

            if (breakdown.dragon     > 0)
                kidTypes.Add("dragon");

            if (breakdown.shark      > 0)
                kidTypes.Add("shark");

            if (breakdown.harpy      > 0)
                kidTypes.Add("harpy");

            if (breakdown.cow        > 0)
                kidTypes.Add("cow");

            if (breakdown.ant        > 0)
                kidTypes.Add("ant");

            if (breakdown.drider     > 0)
                kidTypes.Add("drider");

            if (breakdown.anemone    > 0)
                kidTypes.Add("anemone");

            if (breakdown.salamander > 0)
                kidTypes.Add("salamander");

            typeCount = Math.Min(kidTypes.Count, 3);

            kidString = Utils.Utils.FormatStringArray(Utils.Utils.RandChoices(typeCount, kidTypes.ToArray()));

            OutputText("Scenes.Nieve.NieveCampKids.GlancingOutAtCamp".Localized());

            if (typeCount == 1)
                OutputText("Scenes.Nieve.NieveCampKids.ChildrenCountLine".Localized(
                    kidString == "salamander" ? null  : "your " + kidString + " child" + (totalKids > 1 ? "ren" : "")
                ));
            else
                OutputText("Scenes.Nieve.NieveCampKids.AreYourChildrenAlike".Localized(
                    kidString
                ));

            OutputText("Scenes.Nieve.NieveCampKids.ParentalInstinctsProtect".Localized());

            OutputText("Scenes.Nieve.NieveCampKids.EnjoyingChildishJoy".Localized(
                totalKids == 1 ? kidString + " child" : "children"
            ));

            Save.SetBool("kidsPlayed", true);

            Save.SetBool("kidsPlayedSingular", totalKids == 1);

            Go<CampScene>();
        }

        public void NieveIsOver()
        {
            ClearOutput();

            ClearButtons();

            RegisterParserTags();

            if (stage < 5)
            {
                OutputText("Scenes.Nieve.NieveIsOver.WakeUpAYawn".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.NotAHugeLoss".Localized());

                stage = 0;
            }
            else
            {
                OutputText("Scenes.Nieve.NieveIsOver.WakeUpYawnAgain".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.TurnToWinterWonderland".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.YoureHereWasConcerned".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.FamiliarJinglingFills".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.EverythingLooksTheExact".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.PickUpFrozenTear".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Nieve.NieveIsOver.EvenTearUpYourself".Localized());
                else
                    OutputText("Scenes.Nieve.NieveIsOver.EvenFeelLittleSad".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.SnowmanCollapsesMelting".Localized());

                OutputText("Scenes.Nieve.NieveIsOver.TearOfNieve".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Nieve.NieveIsOver.TearOfAFriend".Localized());

                if (!Player.HasKeyItem("Nieve's Tear"))
                {
                    OutputText("Scenes.Nieve.NieveIsOver.GainedNievesTear".Localized());

                    Player.CreateKeyItem("Nieve's Tear", 1, 0, 0, 0);
                }

                stage = 0;
            }

            Go<CampScene>();
        }

        public void FixNieve()
        {
            ClearOutput();

            if (Save.GetInt("gender", 0) <= (int)Gender.Genderless)
            {
                OutputText("Scenes.Nieve.FixNieve.SomethingHasGoneWrong".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), () => FixNieveGender(1));

                AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), () => FixNieveGender(2));

                return ;
            }
            else
                if (Save.GetString("face") == "")
                {
                    OutputText("Scenes.Nieve.FixNieve.SomethingGoneWrongAgain".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Gems".Localized(), () => FixNieveMouth(0));

                    AddButton(1, "Items.Coal.Short".Localized(), () => FixNieveMouth(1));

                    return ;
                }
                else
                    if (Save.GetInt("age", 0) < 0)
                    {
                        OutputText("Scenes.Nieve.FixNieve.UpgradedWithAge".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Adult".Localized(), () => FixNieveAge(0));

                        AddButton(1, "Common.Button.Child".Localized(), () => FixNieveAge(1));

                        return ;
                    }
                    else
                        NieveReturnsPartII();
        }

        public void FixNieveGender(int arg = 1)
        {

            Save.SetInt("gender", arg);

            FixNieve();
        }

        public void FixNieveMouth(int arg = 1)
        {

            if (arg == 0)
                Save.SetString("face", "gems");
            else
                Save.SetString("face", "coal");

            FixNieve();
        }

        public void FixNieveAge(int arg = 0)
        {

            Save.SetInt("age", arg);

            FixNieve();
        }

        public void ReturnOfNieve()
        {
            ClearOutput();

            RegisterParserTags();

            OutputText("Scenes.Nieve.ReturnOfNieve.AwakeInTheMorning".Localized());

            OutputText("Scenes.Nieve.ReturnOfNieve.HeartSkipsABeat".Localized());

            OutputText("Scenes.Nieve.ReturnOfNieve.HaventHeardBells".Localized());

            OutputText("Scenes.Nieve.ReturnOfNieve.GlanceAroundCampsite".Localized());

            OutputText("Scenes.Nieve.ReturnOfNieve.SinkDownIntoSnow".Localized());

            OutputText("Scenes.Nieve.ReturnOfNieve.WhisperToTheTear".Localized());

            OutputText("Scenes.Nieve.ReturnOfNieve.NothingLeftOf".Localized());

            FixNieve();
        }

        public void NieveReturnsPartII()
        {
            ClearOutput();

            RegisterParserTags();

            OutputText("Scenes.Nieve.NieveReturnsPartII.SighResigningYourself".Localized());

            OutputText("Scenes.Nieve.NieveReturnsPartII.ThatsSeeIt".Localized());

            OutputText("Scenes.Nieve.NieveReturnsPartII.FigureMovesItsOwn".Localized());

            OutputText("Scenes.Nieve.NieveReturnsPartII.BreakingTheHug".Localized());

            OutputText("Scenes.Nieve.NieveReturnsPartII.HoldCloseThankful".Localized());

            OutputText("Scenes.Nieve.NieveReturnsPartII.SnowSpiritClasping".Localized());

            OutputText("Scenes.Nieve.NieveReturnsPartII.NodYoullTakeWhat".Localized());

            stage = 5;

            ReturnToCamp(1);
        }

    }
}