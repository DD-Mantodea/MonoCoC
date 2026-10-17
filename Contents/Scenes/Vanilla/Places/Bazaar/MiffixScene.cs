using MonoCoC.Attributes;
using System;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Contents.StatusEffects;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<MiffixScene>("coc:miffix")]
    public class MiffixScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Bazaar.Miffix);

        public override void Entry()
        {
            TimeToTellPlan();
        }

        public bool TimeChange()
        {
            Save.SetInt("timeUntilPlan", Math.Max(0, Save.GetInt("timeUntilPlan", 0) - 1));

            if (Save.GetBool("toldPlan") && Save.GetInt("timeUntilLegsBroken", 0) > 0)
                Save.SetInt("timeUntilLegsBroken", Math.Max(0, Save.GetInt("timeUntilLegsBroken", 0) - 1));

            return false;
        }

        public bool TimeChangeLarge() => false;

        public void Reset()
        {

            Save.SetInt("timesLost", 0);

            Save.SetString("playerName", "");

            Save.SetBool("beatDemonfist", false);

            Save.SetInt("donatedToMiffix", 0);

            Save.SetBool("metMiffixPost", false);

            Save.SetInt("timeUntilPlan", 0);

            Save.SetBool("learnedOfDemonFist", false);

            Save.SetString("origFace", "");

            Save.SetInt("timeUntilLegsBroken", 0);

            Save.SetBool("toldPlan", false);

            Save.SetBool("killedMiffix", false);
        }

        public bool TimeToTellPlan()
        {

            return Save.GetInt("donatedAmount", 0) >= 1000 && Save.GetInt("timeUntilPlan", 0) == 0 && Save.GetBool("toldPlan") == false;
        }

        public bool PlayerAcceptedPlan()
        {

            return Save.GetString("origFace", "") != "";
        }

        public bool PlayerRejectedPlan()
        {

            return Save.GetBool("toldPlan") && Save.GetString("origFace", "") == "";
        }

        public bool MiffixIsGone()
        {

            return Save.GetBool("killedMiffix") ? Save.GetBool("killedMiffix") : Save.GetBool("toldPlan") == true && Save.GetInt("timeUntilLegsBroken", 0) == 0;
        }

        public void MiffixPostTalk()
        {
            ClearOutput();

            if (!Save.GetBool("metMiffixPost"))
            {
                OutputText("Scenes.Miffixscene.MiffixPostTalk.ApproachesDestituteImp".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostTalk.HelloAgain".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostTalk.TellsName".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostTalk.NodsUnsure".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostTalk.FewSecondsPass".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostTalk.ShouldGiveGems".Localized());

                Save.SetBool("metMiffixPost", true);

                MiffixBegMenu();
            }
            else
            {
                if (TimeToTellPlan())
                {
                    OutputText("Scenes.Miffixscene.MiffixPostTalk.BeggingSpot".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPostTalk.BeginningToThink".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPostTalk.FurrowsBrows".Localized());

                    MiffixTellPlan();
                }

                MiffixBegDescription();

                MiffixTalkTopics();
            }
        }

        public void MiffixBegDescription()
        {

            if (Save.GetInt("donatedAmount", 0) > 500)
            {
                OutputText("Scenes.Miffixscene.MiffixBegDescription.LittleBeggingSpot".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegDescription.WelcomeFirst".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegDescription.StillDestitute".Localized());
            }
            else
                if (Save.GetInt("donatedAmount", 0) > 100)
                {
                    OutputText("Scenes.Miffixscene.MiffixBegDescription.ApproachBeggingSpot".Localized());

                    OutputText("Scenes.Miffixscene.MiffixBegDescription.NodsAtYou".Localized());

                    OutputText("Scenes.Miffixscene.MiffixBegDescription.LookingUp".Localized());
                }
                else
                    if (Save.GetInt("donatedAmount", 0) > 50)
                        OutputText("Scenes.Miffixscene.MiffixBegDescription.ApproachMiffixSitting".Localized());
        }

        public void MiffixPostPlanDescription()
        {

            OutputText("Scenes.Miffixscene.MiffixPostPlanDescription.LooksTense".Localized());

            if (Player.Body.Get<FacePart>().Id.Path == Save.GetString("origFace", ""))
            {
                OutputText("Scenes.Miffixscene.MiffixPostPlanDescription.WavesHeResponds".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostPlanDescription.TriesToTalk".Localized());

                SceneInstance<BazaarScene>.Get().EnterTheBazaar();
            }
            else
            {
                OutputText("Scenes.Miffixscene.MiffixPostPlanDescription.WavesAgain".Localized());

                OutputText("Scenes.Miffixscene.MiffixPostPlanDescription.RubsHandsTrembling".Localized());

                ClearButtons();

                AddNextButton("Common.Button.GoShopping".Localized(), () => StartPlanForReal(true));

                AddNextButton("Common.Button.Nope".Localized(), () => StartPlanForReal(false));
            }
        }

        public void StartPlanForReal(bool choice)
        {
            ClearOutput();

            if (choice)
            {
                OutputText("Scenes.Miffixscene.StartPlanForReal.TellsReady".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.BarelyWait".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.WhateverHeSays".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.StopsSuddenly".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.InterruptsAsk".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.StopsTalking".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.SighsTellsContinue".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.PicksUpFrom".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.GladClarified".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.RunsIntoAlley".Localized());

                PlanForReal2();
            }
            else
            {
                OutputText("Scenes.Miffixscene.StartPlanForReal.NotReadyYet".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.TalkSomeoneElse".Localized());

                OutputText("Scenes.Miffixscene.StartPlanForReal.TurnsAndLeaves".Localized());

                SceneInstance<BazaarScene>.Get().EnterTheBazaar();
            }
        }

        public void PlanForReal2()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.PlanForReal2.MerchantWelcome".Localized());

            OutputText("Scenes.Miffixscene.PlanForReal2.NodsAtDemon".Localized());

            OutputText("Scenes.Miffixscene.PlanForReal2.DemonAsksHoarse".Localized());

            ClearButtons();

            AddButton(0, "Combat.Ability.SharkManThrustGrab.Name".Localized(), () => PlanTakeKey(0));

            AddButton(1, "Common.Button.LeapIn".Localized(), () => PlanTakeKey(1));

            AddButton(2, "Combat.Spell.Blind.Name".Localized(), () => PlanTakeKey(2));
        }

        public void PlanTakeKey(int opt)
        {
            ClearOutput();

            switch (opt)
            {
                case 0:
                {
                OutputText("Scenes.Miffixscene.PlanTakeKey.ForgoesSubtlety".Localized());

                if (Player.Strength > 80)
                {
                    OutputText("Scenes.Miffixscene.PlanTakeKey.ChargesThrough".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.InSeconds".Localized());
                }
                else
                {
                    OutputText("Scenes.Miffixscene.PlanTakeKey.ChargesThroughAgain".Localized());

                    Player.HealthChange(-15);

                    OutputText("Scenes.Miffixscene.PlanTakeKey.MerchantScreams".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.StomachSinks".Localized());

                    Player.HealthChange(-55);
                }
                }

                break;

                case 1:
                {
                OutputText("Scenes.Miffixscene.PlanTakeKey.CloseEnough".Localized());

                if (Player.Strength > 80)
                {
                    OutputText("Scenes.Miffixscene.PlanTakeKey.LeapsOver".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.PickedRightKey".Localized());
                }
                else
                {
                    OutputText("Scenes.Miffixscene.PlanTakeKey.ReadyToLeap".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.SprintsBest".Localized());

                    Player.HealthChange(-15);

                    OutputText("Scenes.Miffixscene.PlanTakeKey.MerchantStepsBack".Localized());

                    Player.HealthChange(-30);

                    OutputText("Scenes.Miffixscene.PlanTakeKey.LacksAgility".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.CrawlsToBelt".Localized());
                }
                }

                break;

                case 2:
                {
                OutputText("Scenes.Miffixscene.PlanTakeKey.BitOfMagic".Localized());

                if (Player.Intelligence > 80)
                {
                    OutputText("Scenes.Miffixscene.PlanTakeKey.CallsMerchantOver".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.ScansJewelry".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.IgnoresThreats".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.RaisesEyebrows".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.GritsTeeth".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.TwoFingers".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.GuardsTurn".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.DemonWhispers".Localized());

                    OutputText("Scenes.Miffixscene.PlanTakeKey.GigIsUp".Localized());
                }
                }

                break;
            }
        }

        public void ReturnToAlley()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.ReturnToAlley.HumorsMiffix".Localized());

            OutputText("Scenes.Miffixscene.ReturnToAlley.LetsGoHands".Localized());

            OutputText("Scenes.Miffixscene.ReturnToAlley.AnotherChance".Localized());

            OutputText("Scenes.Miffixscene.ReturnToAlley.ShrugWord".Localized());

            OutputText("Scenes.Miffixscene.ReturnToAlley.ForcingHimself".Localized());

            MiffixTellPlan2();
        }

        public void MiffixTellPlan()
        {
            ClearOutput();

            Save.SetBool("toldPlan", true);

            OutputText("Scenes.Miffixscene.MiffixTellPlan.AttemptsGreet".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan.WerentWorried".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan.BazaarNoGuards".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan.GoodToKnow".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan.TapsFingersNervously".Localized());

            MiffixTellPlan2();
        }

        public void MiffixTellPlan2()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixTellPlan2.RelaxesWidensSmile".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan2.JustToldYou".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan2.VagueWasntI".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan2.GettingAwfully".Localized());

            if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                OutputText("Scenes.Miffixscene.MiffixTellPlan2.PrettyMuch".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan2.CoughsUncomfortably".Localized());

            OutputText("Scenes.Miffixscene.MiffixTellPlan2.RubsChin".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Sure".Localized(), () => MiffixPlanAnswer(0));

            AddNextButton("Common.Button.Nope".Localized(), () => MiffixPlanAnswer(1));

            if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                AddNextButton("Common.Button.KillHim".Localized(), () => MiffixPlanAnswer(2));
        }

        public void MiffixPlanAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.LooksSkeptical".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.ResignsToIt".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.ClearsThroat".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GesturesSit".Localized());

                MiffixPlanStart();
                }

                break;

                case 1:
                {
                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.LooksSkepticalAgain".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.SighsShakesHead".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.CantDoThat".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.HellJustHaveTo".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.IfIDont".Localized());

                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TrailsOff".Localized());

                if (Save.GetInt("timeUntilLegsBroken", 0) == 0)
                    Save.SetInt("timeUntilLegsBroken", 120);

                SceneInstance<BazaarScene>.Get().EnterTheBazaarAndMenu();
                }

                break;

                case 2:
                {
                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.EnoughOfThis".Localized());

                if (Player.Weapon.IsFist())
                {
                    OutputText("Scenes.Miffixscene.MiffixPlanAnswer.CracksKnuckles".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPlanAnswer.FuckMeWhisper".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TurnsToLeave".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPlanAnswer.BarelyHurts".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GrabsArm".Localized());

                    OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GoesLimp".Localized());

                    if (Player.IsNaga())
                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.NoMeansToStomp".Localized());
                    else
                    {
                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.WalksToUnconscious".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.JobDoneWalksOut".Localized());
                    }
                }
                else
                    if (Player.Weapon.Is2HBlunt())
                    {
                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.BrandishesWeapon".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.FuckMeWhisperAgain".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TakesDeepBreath".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.DeliversJab".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.ScramblesUp".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GroansTurns".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.HoldsAloft".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.DefiantResigned".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.CrushesSkull".Localized());

                        OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TakesChances".Localized());
                    }
                    else
                        if (Player.Weapon.Is2HSword())
                        {
                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.BrandishesWeaponAgain".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.FuckMeWhisperThird".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TakesDeepBreathAgain".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.NotQuickEnough".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.ScramblesUpAgain".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GroansTurnsAgain".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.HoldsAloftAgain".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.DefiantResignedAgain".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.SlicesNeck".Localized());

                            OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TakesChancesAgain".Localized());
                        }
                        else
                            if (Player.Weapon.IsRanged() && !Player.Weapon.IsFirearm())
                            {
                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.CracksKnucklesAgain".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.FuckMeWhisperFourth".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.TurnsToLeaveAgain".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.BarelyHurtsAgain".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GrabsArmAgain".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.GoesLimpAgain".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.ScramblesUpThird".Localized());

                                OutputText("Scenes.Miffixscene.MiffixPlanAnswer.QuickAndClean".Localized());
                            }

                SceneInstance<BazaarScene>.Get().EnterTheBazaarAndMenu();
                }

                break;
            }
        }

        public void MiffixPlanStart()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixPlanStart.MerchantStaring".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlanStart.LinenFlapping".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlanStart.DespiteEfforts".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlanStart.MerchantRunsAtYou".Localized());

            MiffixPlan2();
        }

        public void MiffixPlan2()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixPlan2.ThroughRows".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan2.LeapsOutStall".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan2.HeartSinks".Localized());

            MiffixPlan3();
        }

        public void MiffixPlan3()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixPlan3.OnePersonHoped".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan3.NamePlate".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan3.EyesWithScorn".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan3.CringeKnow".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan3.RaisesEyebrow".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan3.ImpMovingConfidently".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan3.SteelsYourself".Localized());

            Player.AddStatusEffect(new StatusEffect { Id = StatusEffectIds.ImaginaryMiffix });

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "imaginary_miffix")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void MiffixPlan4()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixPlan4.ReturnsLockbox".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan4.ScoundrelFaced".Localized());

            CombatScene.CleanupAfterCombat();

            MiffixPlan5();
        }

        public void MiffixPlan5()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixPlan5.ImpSmiling".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.RemainsSilent".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.DeepBreathStare".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.TakesItAll".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.PatronizingTone".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.NodsIrritated".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.NotDoingThis".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.GroanWord".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.BeenInvesting".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.RememberSign".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.NodWord".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.SecondInvestment".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.RaisesBrow".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.ForGatekeeper".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.QuickToPointOut".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan5.LawEnforcement".Localized());

            MiffixPlan6();
        }

        public void MiffixPlan6()
        {
            ClearOutput();

            Save.SetString("origFace", Player.Body.Get<FacePart>().Id.Path);

            OutputText("Scenes.Miffixscene.MiffixPlan6.MovesTowards".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.OddJobs".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.InterjectsWhy".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.GrumblesAgreement".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.ReturnItOfCourse".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.GarnerLethice".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.SmilesWord".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.InterestingStill".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.PointsAtGems".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan6.SettlesIt".Localized());

            MiffixPlan7();
        }

        public void MiffixPlan7()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.MiffixPlan7.AbsolutelyNot".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan7.WhatTheHell".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan7.StepsBackStunned".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan7.LongLook".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan7.CantBlameYou".Localized());

            OutputText("Scenes.Miffixscene.MiffixPlan7.FinalProtest".Localized());

            if (Save.GetInt("timeUntilLegsBroken", 0) == 0)
                Save.SetInt("timeUntilLegsBroken", 120);
        }

        public void ThatCantBeHowItHappened()
        {
            ClearOutput();

            OutputText("Scenes.Miffixscene.ThatCantBeHowItHappened.ShakesHead".Localized());

            OutputText("Scenes.Miffixscene.ThatCantBeHowItHappened.ShrugWord".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "imaginary_miffix")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void MiffixTalkTopics()
        {

            ClearButtons();

            AddNextButton("Common.Button.Money".Localized(), () => MiffixTalkAnswers(0));

            AddNextButton("Common.Button.Champion".Localized(), () => MiffixTalkAnswers(1));

            AddNextButton("Common.Button.PastLife".Localized(), () => MiffixTalkAnswers(2));
        }

        public void MiffixTalkAnswers(int topic = 0)
        {

            switch (topic)
            {
                case 0:
                {
                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.CoughsAsks".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.DecidedToMake".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.AppreciateIt".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.FairPoint".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.BreakMyBones".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.NodsBack".Localized());

                MiffixBegMenu();

                return ;
                }

                case 1:
                {
                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.TellsAboutFight".Localized());

                if (Player.ShortName == Save.GetString("playerName"))
                    OutputText("Scenes.Miffixscene.MiffixTalkAnswers.NoOracleNeeded".Localized());
                else
                    OutputText("Scenes.Miffixscene.MiffixTalkAnswers.NoShit".Localized(
                        Save.GetString("playerName")
                    ));

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.ChucklesTrailsOff".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.SoundsLikeHeHas".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.LuckNotKind".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.SmilesTellsImp".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.LeansInAsks".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.DemonfistFirstBet".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.PrettyInteresting".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.KickSomeoneDown".Localized());

                OutputText("Scenes.Miffixscene.MiffixTalkAnswers.CompulsiveGambling".Localized());
                }

                break;
            }

            MiffixTalkTopics();
        }

        public void MiffixBegMenu()
        {

            ClearButtons();

            AddNextButton("Common.Button.WhatFor".Localized(), () => MiffixBegAnswers(0));

            AddNextButton("10", () => MiffixBegAnswers(1));

            AddNextButton("50", () => MiffixBegAnswers(2));

            AddNextButton("100", () => MiffixBegAnswers(3));

            AddNextButton("500", () => MiffixBegAnswers(4));

            AddNextButton("1000", () => MiffixBegAnswers(5));

            AddNextButton("Common.Button.Nothing".Localized(), () => MiffixBegAnswers(6));
        }

        public void MiffixBegAnswers(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Miffixscene.MiffixBegAnswers.AsksWhatHeNeeds".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.WorriedDemonfist".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.LosingBrokeMore".Localized());

                MiffixBegMenu();
                }

                break;

                case 1:
                {
                OutputText("Scenes.Miffixscene.MiffixBegAnswers.HandfulGems".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.TakesGemsThanks".Localized());

                Player.Gems -= 10;

                Save.SetInt("donatedAmount", Save.GetInt("donatedAmount", 0) + 10);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Miffixscene.MiffixBegAnswers.RespectableAmount".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.TakesGemsSmiling".Localized());

                Player.Gems -= 50;

                Save.SetInt("donatedAmount", Save.GetInt("donatedAmount", 0) + 50);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Miffixscene.MiffixBegAnswers.FatPouch".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.CountsSurreptitiously".Localized());

                Player.Gems -= 100;

                Save.SetInt("donatedAmount", Save.GetInt("donatedAmount", 0) + 100);
                }

                break;

                case 4:
                {
                OutputText("Scenes.Miffixscene.MiffixBegAnswers.Ask500Gems".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.ScratchesHead".Localized());

                MiffixBegMenu();

                return ;
                }

                case 5:
                {
                OutputText("Scenes.Miffixscene.MiffixBegAnswers.StrugglesGetting".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.Incredulous".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.ScratchesHeadAgain".Localized());

                MiffixBegMenu();

                return ;
                }
            }

            if (Save.GetInt("donatedAmount", 0) >= 1000 && Save.GetInt("timeUntilPlan", 0) == 0)
            {
                Save.SetInt("timeUntilPlan", 48);

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.ShiftyEyes".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.CollectedEnough".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.GetsUpGathering".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.HaveNoIdea".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.PreparationsInDays".Localized());

                OutputText("Scenes.Miffixscene.MiffixBegAnswers.MumblingGets".Localized());
            }

            SceneInstance<BazaarScene>.Get().EnterTheBazaar();
        }

    }
}