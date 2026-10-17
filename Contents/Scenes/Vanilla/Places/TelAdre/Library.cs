using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<Library>("coc:library")]
    public class Library : Scene
    {

        public override void Entry()
        {
            VisitZeMagesTower();
        }

        public void VisitZeMagesTower()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeenToLibrary) == 0)
                FirstTowerVisit();
            else
                TowerFollowUpVisits();

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeenToLibrary) == 0 || View.Time.Hours <= 17)
            {
                AddButton(1, "Common.Button.YouOkay".Localized(), YouOkayBuddy);

                if (CoC.Instance.Flags.Get(GameFlagIds.DominikaDrama) > 0)
                    AddButton(2, "Common.Button.Mali".Localized(), TalkToMali);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesVisitedMali) > 0)
                AddButton(2, "Common.Button.Mali".Localized(), TalkToMali);

            AddButton(0, "Common.Button.Study".Localized(), StudyInTA);

            if (View.Time.Hours <= 17)
                AddNextButton("Scenes.Faerie.GladesScene.ButtonHelp".Localized(), LibraryHelp);

            if (Player.HasKeyItem("Old Manor Books"))
                AddNextButton("Common.Button.GiveBooks".Localized(), GiveManorBooks);

            CoC.Instance.Flags.Set(GameFlagIds.TimesBeenToLibrary, CoC.Instance.Flags.Get(GameFlagIds.TimesBeenToLibrary) + 1);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<TelAdre>());
        }

        public void FirstTowerVisit()
        {
            ClearOutput();

            OutputText("Scenes.Library.FirstTowerVisit.MakesWay".Localized());

            OutputText("Scenes.Library.FirstTowerVisit.FrontEntryway".Localized());

            OutputText("Scenes.Library.FirstTowerVisit.SingleRoom".Localized());

            if (View.Time.Hours <= 17)
            {
                OutputText("Scenes.Library.FirstTowerVisit.ManReading".Localized());

                CommonQuinnTroduction();
            }
            else
            {
                OutputText("Scenes.Library.FirstTowerVisit.NoOneHere".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.TimesBeenToLibrary, -2);
            }
        }

        public void TowerFollowUpVisits()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeenToLibrary) == -1)
            {
                if (View.Time.Hours <= 17)
                {
                    OutputText("Scenes.Library.TowerFollowUpVisits.ReturnsTower".Localized());

                    CommonQuinnTroduction();
                }
                else
                {
                    OutputText("Scenes.Library.TowerFollowUpVisits.NoOneAsBefore".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.TimesBeenToLibrary, -2);
                }

                return ;
            }

            if (View.Time.Hours <= 17)
            {
                OutputText("Scenes.Library.TowerFollowUpVisits.ReturnsTowerAgain".Localized());

                OutputText("Scenes.Library.TowerFollowUpVisits.NoticesArrival".Localized());
            }
            else
            {
                OutputText("Scenes.Library.TowerFollowUpVisits.ReturnsTowerThird".Localized());

                OutputText("Scenes.Library.TowerFollowUpVisits.CouldRead".Localized());
            }
        }

        public void CommonQuinnTroduction()
        {

            OutputText("Scenes.Library.CommonQuinnTroduction.NotReady".Localized());

            OutputText("Scenes.Library.CommonQuinnTroduction.SaysSorry".Localized());

            OutputText("Scenes.Library.CommonQuinnTroduction.DressOdd".Localized());

            if (Player.IsAdult())
                OutputText("Scenes.Library.CommonQuinnTroduction.NotMuchOlder".Localized());

            if (Player.IsChild())
                OutputText("Scenes.Library.CommonQuinnTroduction.NotYoung".Localized());

            if (Player.IsTeen())
                OutputText("Scenes.Library.CommonQuinnTroduction.NotMiddleAged".Localized());

            if (Player.IsElder())
                OutputText("Scenes.Library.CommonQuinnTroduction.ListlessExpression".Localized());

            OutputText("Scenes.Library.CommonQuinnTroduction.UnsureFeel".Localized());

            OutputText("Scenes.Library.CommonQuinnTroduction.QuinnConcludes".Localized());
        }

        public void StudyInTA()
        {
            ClearOutput();

            if (View.Time.Hours <= 17)
            {
                OutputText("Scenes.Library.StudyInTA.AsksQuinn".Localized());

                OutputText("Scenes.Library.StudyInTA.QuinnSighs".Localized());

                OutputText("Scenes.Library.StudyInTA.QuinnPauses".Localized());

                ClearButtons();

                AddButton(14, "Common.Button.Back".Localized(), () => Go<TelAdre>());
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeenToLibrary) == -1)
                    OutputText("Scenes.Library.StudyInTA.DecidesStudy".Localized());
                else
                    OutputText("Scenes.Library.StudyInTA.WithoutQuinn".Localized());

                OutputText("Scenes.Library.StudyInTA.ReadsLiterature".Localized());

                if (Random.Shared.Next(3) == 0)
                {
                    OutputText("Scenes.Library.StudyInTA.RandomBook".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 3 + Random.Shared.Next(4)));

                    if (Player.Intelligence >= 25 && !Player.HasStatusEffect(StatusEffectIds.KnowsArouse))
                    {
                        OutputText("Scenes.Library.StudyInTA.BlinkSurprise".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.KnowsArouse, 0, 0, 0, 0);
                    }
                    else
                        if (Player.Intelligence >= 30 && !Player.HasStatusEffect(StatusEffectIds.KnowsHeal))
                        {
                            OutputText("Scenes.Library.StudyInTA.BlinkSurpriseAgain".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.KnowsHeal, 0, 0, 0, 0);
                        }
                        else
                            if (Player.Intelligence >= 40 && !Player.HasStatusEffect(StatusEffectIds.KnowsMight))
                            {
                                OutputText("Scenes.Library.StudyInTA.BlinkSurpriseThird".Localized());

                                Player.CreateStatusEffect(StatusEffectIds.KnowsMight, 0, 0, 0, 0);
                            }
                            else
                                if (Player.Intelligence >= 25 && !Player.HasStatusEffect(StatusEffectIds.KnowsCharge))
                                {
                                    OutputText("Scenes.Library.StudyInTA.BlinkSurpriseFourth".Localized());

                                    Player.CreateStatusEffect(StatusEffectIds.KnowsCharge, 0, 0, 0, 0);
                                }
                                else
                                    if (Player.Intelligence >= 30 && !Player.HasStatusEffect(StatusEffectIds.KnowsBlind))
                                    {
                                        OutputText("Scenes.Library.StudyInTA.BlinkSurpriseFifth".Localized());

                                        Player.CreateStatusEffect(StatusEffectIds.KnowsBlind, 0, 0, 0, 0);
                                    }
                                    else
                                        if (Player.Intelligence >= 40 && !Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                                        {
                                            OutputText("Scenes.Library.StudyInTA.BlinkSurpriseSixth".Localized());

                                            Player.CreateStatusEffect(StatusEffectIds.KnowsWhitefire, 0, 0, 0, 0);
                                        }
                }
                else
                    if ((Player.Libido > 75 || !Player.IsPureEnough(75) || Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties) || Player.HasPerk(PerkIds.BroBrains)) && Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Library.StudyInTA.PicksBook".Localized());
                    else
                        OutputText("Scenes.Library.StudyInTA.RandomBookAgain".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => ReturnToCamp(1));
            }
        }

        public void YouOkayBuddy()
        {
            ClearOutput();

            OutputText("Scenes.Library.YouOkayBuddy.BitPerturbed".Localized());

            OutputText("Scenes.Library.YouOkayBuddy.AnswersQuestion".Localized());

            OutputText("Scenes.Library.YouOkayBuddy.ClarifiesConcern".Localized());

            OutputText("Scenes.Library.YouOkayBuddy.QuinnScoffs".Localized());

            OutputText("Scenes.Library.YouOkayBuddy.FinallyExposition".Localized());

            Go<TelAdre>();
        }

        public void TalkToMali()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesVisitedMali) == 0)
            {
                OutputText("Scenes.Library.TalkToMali.MentionsToQuinn".Localized());

                OutputText("Scenes.Library.TalkToMali.AscendsStairs".Localized());

                OutputText("Scenes.Library.TalkToMali.TakesMoments".Localized());

                OutputText("Scenes.Library.TalkToMali.QuinnGestures".Localized());

                OutputText("Scenes.Library.TalkToMali.MaliClosesEyes".Localized());

                OutputText("Scenes.Library.TalkToMali.LeansBack".Localized());

                OutputText("Scenes.Library.TalkToMali.IsThatAll".Localized());

                OutputText("Scenes.Library.TalkToMali.MaliStands".Localized());

                OutputText("Scenes.Library.TalkToMali.StepsForward".Localized());

                OutputText("Scenes.Library.TalkToMali.SheExclaims".Localized());

                OutputText("Scenes.Library.TalkToMali.NoBusiness".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
                if ((Player.WeaponName == "inscribed spellblade" || Player.HasItem(WeaponIds.Spellblade)) && CoC.Instance.Flags.Get(GameFlagIds.MaliTakenBlade) == 0)
                {
                    OutputText("Scenes.Library.TalkToMali.HereToSee".Localized());

                    OutputText("Scenes.Library.TalkToMali.MaliAsksItem".Localized());

                    OutputText("Scenes.Library.TalkToMali.InspectsItem".Localized());

                    OutputText("Scenes.Library.TalkToMali.ExplainsGrin".Localized());

                    OutputText("Scenes.Library.TalkToMali.ComeBackSoon".Localized());

                    OutputText("Scenes.Library.TalkToMali.ConclusionBanner".Localized());

                    if (Player.Weapon.Id == WeaponIds.Spellblade)
                        Player.SetUnarmed();
                    else
                        Player.ConsumeItem(WeaponIds.Spellblade);

                    CoC.Instance.Flags.Set(GameFlagIds.MaliTakenBlade, 1);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Library.TalkToMali.HereToSeeAgain".Localized());

                    OutputText("Scenes.Library.TalkToMali.MaliSmiles".Localized());

                    OutputText("Scenes.Library.TalkToMali.OnWayOut".Localized());

                    DoNext(() => ReturnToCamp(1));
                }

            CoC.Instance.Flags.Set(GameFlagIds.TimesVisitedMali, CoC.Instance.Flags.Get(GameFlagIds.TimesVisitedMali) + 1);
        }

        public void LibraryHelp()
        {
            ClearOutput();

            OutputText("Scenes.Library.LibraryHelp.WorkChore".Localized());

            OutputText("Scenes.Library.LibraryHelp.PlaceholderText".Localized((Player.HasFur() ? "[say: You fucking people have the gall to act like you're doing me a favor by fixing your own fuck-up!] he exclaims in irritation. [say: No, I'm not open to assistance, not from the likes of your kind.]" : "[say: What a laugh, to offer your service without any understanding of the systems in place. The time and energy it would take to explain how these problems are recorded and indexed, how they're organized, and simply assuring that you aren't going to make matters worse could not possibly yield more expedient remedy of the issue than if I alone continue,] he says in one long and irritated breath. [say: In short, no.]")));

            OutputText("Scenes.Library.LibraryHelp.UnexpectedHelp".Localized());

            Go<TelAdre>();
        }

        public void GiveManorBooks()
        {
            ClearOutput();

            OutputText("Scenes.Library.GiveManorBooks.LibraryMakesSense".Localized());

            OutputText("Scenes.Library.GiveManorBooks.WantsCollection".Localized());

            OutputText("Scenes.Library.GiveManorBooks.DespiteSarcasm".Localized());

            OutputText("Scenes.Library.GiveManorBooks.AddsDryly".Localized());

            OutputText("Scenes.Library.GiveManorBooks.LeavesHanging".Localized());

            Player.RemoveKeyItem("Old Manor Books");

            Go<TelAdre>();
        }

    }
}