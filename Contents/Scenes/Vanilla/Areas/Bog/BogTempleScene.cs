using MonoCoC.Attributes;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<BogTempleScene>("coc:temple")]
    public class BogTempleScene : Scene
    {

        public const int SEENSCULP = 1 << 1;

        public const int SEENALTAR = 1 << 2;

        public const int SEENSTATU = 1 << 3;

        public const int SEENBOOKS = 1 << 4;

        public const int SEENAHOLE = 1 << 5;

        public const int SEENPUDDL = 1 << 6;

        public const int SEENBALCO = 1 << 7;

        public const int SEENENTER = 1 << 8;

        public static SaveContent Save => SaveContent(SceneIds.Bog.BogTemple);

        public string debugName => "Bog Temple";

        public string debugHint => "";

        public static MarielleScene marielle => SceneInstance<MarielleScene>.Get();

        public override void Entry()
        {
            TempleEncounter();
        }

        public void Reset()
        {

            Save.SetBool("foundTemple", false);

            Save.SetBool("shieldTaken", false);

            Save.SetInt("inspection", 0);

            Save.SetBool("seenBalcony", false);

            Save.SetFloat("excludeExplore", 0);

            Save.SetInt("timesVisited", 0);

            Save.SetInt("timesPrayed", 0);

            Save.SetInt("timesBathed", 0);
        }

        public void TempleFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleFirstTime.HipDeepMuddy".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstTime.FollowingSigns".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstTime.ThickCanopy".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstTime.WadeCloser".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Enter".Localized(), TempleFirstEnter);

            AddNextButton("Common.Button.Leave".Localized(), TempleFirstLeave);
        }

        public void TempleFirstEnter()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleFirstEnter.WalksIvyCovered".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.ItsATemple".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.CarefulNotTrip".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.StoneSculpture".Localized((silly && Player.Intelligence < 20 ? "" : " it. You have never seen this script before, and it's mostly obscured")));

            OutputText("Scenes.Bogtemple.TempleFirstEnter.FigureMightHave".Localized((silly && Player.Intelligence < 20 ? "" : " it. You have never seen this script before, and it's mostly obscured")));

            OutputText("Scenes.Bogtemple.TempleFirstEnter.SconcesRusty".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.FurtherInvestigate".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.TryingConcentrate".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.BlinksSounds".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.TickingStops".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstEnter.DontKnowWhere".Localized((Save.GetBool("shieldTaken") || Player.HasKeyItem("Old Eldritch Tome") ? "You begin to realize what this might be" : "What a strange contraption you have found"), (Save.GetBool("shieldTaken") || Player.HasKeyItem("Old Eldritch Tome") ? "manipulating time around its user" : "somehow freezing animals around its user. Maybe even people")));

            OutputText("Scenes.Bogtemple.TempleFirstEnter.PlacesOnTop".Localized());

            Save.SetBool("foundTemple", true);

            ClearButtons();

            AddNextButton("Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), TempleFirstTake);

            AddNextButton("Common.Button.LeaveIt".Localized(), TempleFirstDontTake);

            if (silly)
                AddNextButton("Common.Button.BurnIt".Localized(), TempleFirstBurn);
        }

        public void TempleFirstTake()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleFirstTake.AfterPondering".Localized((Save.GetBool("shieldTaken") || Player.HasKeyItem("Old Eldritch Tome") ? "A source of time magic like this should be pretty" : "Whatever magic inhabits it might become")));

            OutputText("Scenes.Bogtemple.TempleFirstTake.PocketingDevice".Localized());

            Save.SetBool("shieldTaken", true);

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.KiteShield), 1);

            ReturnToCamp(1);

        }

        public void TempleFirstDontTake()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleFirstDontTake.DecidesAgainst".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstDontTake.TurningAround".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TempleFirstBurn()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleFirstBurn.TryAsYouMight".Localized());

            ClearButtons();

            AddNextButton("Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), TempleFirstTake);

            AddNextButton("Common.Button.LeaveIt".Localized(), TempleFirstDontTake);
        }

        public void TempleFirstLeave()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleFirstLeave.BetterLeftAlone".Localized());

            OutputText("Scenes.Bogtemple.TempleFirstLeave.TurnsOnHeel".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TempleEncounter()
        {

            Save.SetInt("timesVisited", Save.GetInt("timesVisited", 0) + 1);

            marielle.SetVisitTime(View.Time.Hours + View.Time.Minutes / 60f);

            if (!Save.GetBool("foundTemple"))
                TempleFirstTime();
            else
                if (Save.GetInt("timesVisited", 0) >= 5 && MarielleScene.Save.GetInt("state", 0) == 0)
                    marielle.Meet();
                else
                    if (MarielleScene.Save.GetInt("state", 0) > 0 && MarielleScene.Save.GetInt("openDate", 0) == 0)
                        marielle.Open();
                    else
                        TempleMenu();
        }

        public void TempleMenu(bool output = true, bool arriving = true)
        {
            ClearOutput();

            if (output)
            {
                if (arriving)
                    OutputText("Scenes.Bogtemple.TempleMenu.SurroundingsFamiliar".Localized((Save.GetInt("timesVisited", 0) > 8 ? "even the birds don't complain" : "save for the few birds you see fluttering away, loudly complaining")));

                OutputText("Scenes.Bogtemple.TempleMenu.TakesLookAround".Localized());

                OutputText("Scenes.Bogtemple.TempleMenu.TimeAndWeather".Localized());

                if (MarielleScene.Save.GetInt("state", 0) > 0)
                    OutputText("Scenes.Bogtemple.TempleMenu.ParagraphPlaceholder".Localized(
                        View.Time.IsTimeBetween(22.5f, 6.5f) ? "Marielle's sizeable half-tent occupying the breach is dark and silent. Peeking into it, you spy the undead girl tucked into a bedroll between mountains of fabric rolls and boxes holding various sewing and camping supplies. She's asleep." : "Marielle, the undead seamstress, has occupied the breach with a sizeable half-tent stretched over it from the outside, making it the entrance to her tailor boutique. The girl herself is sitting at a table pushed up against a low remainder of the facade, the space around her littered with a multitude of sewing tools, boxes, and rolls of fabric, thread, and ribbons. Her usual dress provides more than enough decency, while a pair of old pince-nez and a lit oil lamp help her concentrate. She " + (MarielleScene.Save.GetInt("commissionTime", 0) > View.Time.TotalTime ? "doesn't notice you, her attention entirely dedicated to working on the article you requested." : "seems engrossed in something, and whether she has noticed your entry or not, you're not sure.")
                    ));
            }

            ClearButtons();

            AddNextButton("Common.Button.Marielle".Localized(), marielle.Encounter);

            AddNextButton("Common.Button.Inspect".Localized(), () => TempleInspect());

            AddNextButton("Common.Button.Relax".Localized(), TempleRelax);

            AddNextButton("Common.Button.Pray".Localized(), TemplePray);

            if ((Save.GetInt("inspection", 0) & SEENPUDDL) != 0)
                AddNextButton("Common.Button.Bathe".Localized(), TempleBathe);

            AddButton(14, "Common.Button.Leave".Localized(), TempleLeave);
        }

        public void TempleInspect(int choice = -1)
        {
            ClearOutput();

            switch (choice)
            {
                case -1:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.FewPoints".Localized());
                }

                break;

                case 0:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.MostProminent".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.HairAndDress".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.HerSmile".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.DifficultToWrest".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.ScriptUnfamiliar".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.AltarUnpretentious".Localized((silly && Player.Corruption > 90 ? "Large enough to sacrifice a child on" : "As large as a dining table")));

                OutputText("Scenes.Bogtemple.TempleInspect.FrontBearsImage".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.NotMuch".Localized());

                if (!Save.GetBool("shieldTaken"))
                {
                    OutputText("Scenes.Bogtemple.TempleInspect.MetalDisc".Localized());

                    DoNext(() => TempleInspect());

                    AddNextButton("Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), TempleFirstTake);

                    return ;
                }
                }

                break;

                case 2:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.AlongWalls".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.SaintsAccomplished".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.WeaponsNotOnly".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.DontSeemToBe".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.StoneShelves".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.LeatherSpines".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.CatchesEye".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.SizeableEnough".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.LittleCloser".Localized());

                if (marielle.MarielleAvailable(false, false))
                {
                    OutputText("Scenes.Bogtemple.TempleInspect.JustOutside".Localized());

                    if (View.Time.IsTimeBetween(22.5f, 6.5f))
                    {
                        OutputText("Scenes.Bogtemple.TempleInspect.TuckedAsleep".Localized());

                        OutputText("Scenes.Bogtemple.TempleInspect.KeepsDesk".Localized());

                        OutputText("Scenes.Bogtemple.TempleInspect.ClutteredChests".Localized());
                    }
                    else
                        if (MarielleScene.Save.GetInt("commissionTime", 0) > View.Time.TotalTime)
                        {
                            OutputText("Scenes.Bogtemple.TempleInspect.SittingWorking".Localized());

                            OutputText("Scenes.Bogtemple.TempleInspect.DontThinkShould".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Bogtemple.TempleInspect.SittingImmersed".Localized());

                            OutputText("Scenes.Bogtemple.TempleInspect.InsideTent".Localized());

                            if (MarielleScene.Save.GetInt("insulted", 0) != 0 && MarielleScene.Save.GetInt("insulted", 0) != 2)
                            {
                                OutputText("Scenes.Bogtemple.TempleInspect.FreezesNeedle".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.ItsCluttered".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Bogtemple.TempleInspect.LipsParted".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.AllSheSays".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.RestatesHalting".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.MarielleSays".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.WellIShall".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.ProbablyIs".Localized());

                                OutputText("Scenes.Bogtemple.TempleInspect.SeamstressAttention".Localized());

                                SceneInstance<MarielleScene>.Get().MarielleMenu();

                                return ;
                            }
                        }
                }
                else
                    OutputText("Scenes.Bogtemple.TempleInspect.NoObviousMarks".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.StrewnTemple".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.CrystalClear".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.ChiefAmong".Localized());

                if ((Save.GetInt("inspection", 0) & SEENPUDDL) != 0)
                    OutputText("Scenes.Bogtemple.TempleInspect.PleasantBath".Localized());
                else
                {
                    OutputText("Scenes.Bogtemple.TempleInspect.DeepEnough".Localized());

                    OutputText("Scenes.Bogtemple.TempleInspect.MossConquered".Localized());
                }
                }

                break;

                case 6:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.InnerBalconies".Localized());

                if (Player.CanFly() || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Naga)
                {
                    OutputText("Scenes.Bogtemple.TempleInspect.ParagraphPlaceholder".Localized((Player.CanFly() ? "The winding staircase that once lead up to them may be broken, but that's no hindrance to you. Flapping your [wings], you promptly take to the air before coming to perch yourself on the balustrade. Something small scurries away as you land" : "The winding staircase that once lead up to them is crumbled, but you think you can wrap yourself around one of the columns and climb it. You pick one that looks undamaged enough, then carefully make your slithering way upwards. Something small scurries away as you reach for the balustrade and pull yourself over it")));

                    OutputText("Scenes.Bogtemple.TempleInspect.CatchSight".Localized());

                    OutputText("Scenes.Bogtemple.TempleInspect.SlittedWindows".Localized());

                    OutputText("Scenes.Bogtemple.TempleInspect.InCornerFinds".Localized());

                    OutputText("Scenes.Bogtemple.TempleInspect.OtherThanThat".Localized());

                    Save.SetBool("seenBalcony", true);
                }
                else
                    OutputText("Scenes.Bogtemple.TempleInspect.WindingStaircase".Localized());
                }

                break;

                case 7:
                {
                OutputText("Scenes.Bogtemple.TempleInspect.EntranceLeast".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.WideAndHigh".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.HeavyDoors".Localized());

                OutputText("Scenes.Bogtemple.TempleInspect.LittleInWay".Localized());
                }

                break;
            }

            Save.SetInt("inspection", Save.GetInt("inspection", 0) | 1 << (choice + 1));

            ClearButtons();

            AddNextButton("Common.Button.Sculpture".Localized(), () => TempleInspect(0));

            AddNextButton("Common.Button.Altar".Localized(), () => TempleInspect(1));

            AddNextButton("Common.Button.Statuettes".Localized(), () => TempleInspect(2));

            AddNextButton("Common.Button.Books".Localized(), () => TempleInspect(3));

            AddNextButton("Common.Button.Hole".Localized(), () => TempleInspect(4));

            AddNextButton("Common.Button.Puddles".Localized(), () => TempleInspect(5));

            AddNextButton("Common.Button.Balconies".Localized(), () => TempleInspect(6));

            AddNextButton("Common.Button.Entrance".Localized(), () => TempleInspect(7));

            AddButton(14, "Common.Button.Back".Localized(), () => TempleMenu(true, false));

            if (choice >= 0)
                DisableButton(choice, "You just looked at that.");
        }

        public void TempleRelax()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleRelax.RestAWhile".Localized());

            OutputText("Scenes.Bogtemple.TempleRelax.SereneAtmosphere".Localized());

            OutputText("Scenes.Bogtemple.TempleRelax.LightlyStretch".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

            Player.ChangeFatigue(-30);

            Player.HealthChange(Player.MaxHealth / 10);

            ClearButtons();

            AddNextButton("Areas.Bog.Name".Localized(), TempleBogExplore);

            AddNextButton("Areas.Swamp.Name".Localized(), null );

            AddNextButton("Common.Button.Camp".Localized(), TempleRelaxCamp);
        }

        public void TempleBogExplore()
        {

            Save.SetFloat("excludeExplore", View.Time.TotalTime);

        }

        public void TempleRelaxCamp()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleRelaxCamp.HeadsBack".Localized());

            OutputText("Scenes.Bogtemple.TempleRelaxCamp.RetracingSteps".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TemplePray()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TemplePray.ThisIsTemple".Localized(
                Player.IsReligious() ? (Player.HasPerk(PerkIds.HistoryDEUSVULT) ? "and although this nameless deity is not your goddess, she definitely looks like the sort to oppose the demons and their corruption, rather than be on their side." : (Player.WasElder() ? "and although you've devoted your entire life so far to other gods, you're not opposed to praying to this one, here." : "though you don't know what the elders and priests in Ingnam would say if they saw you praying to this nameless goddess.")) : (Player.Corruption > 45 ? "though you're not sure what you're hoping for. Do you seek to become a better person, or what other reason would you have to pray to such an obviously good-natured deity?" : "and this goddess here has likely not had anyone else praying to her in ages.")
            ));

            OutputText("Scenes.Bogtemple.TemplePray.KneelingDown".Localized());

            OutputText("Scenes.Bogtemple.TemplePray.SilentlyLets".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.TempleBlessing))
            {
                OutputText("Scenes.Bogtemple.TemplePray.SecondsPass".Localized());

                OutputText("Scenes.Bogtemple.TemplePray.QuietGasp".Localized());

                OutputText("Scenes.Bogtemple.TemplePray.StillKneeling".Localized());

                OutputText("Scenes.Bogtemple.TemplePray.DustsOff".Localized());

                Player.CreateStatusEffect(StatusEffectIds.TempleBlessing, 24, 1, 0, 0);
            }
            else
            {
                OutputText("Scenes.Bogtemple.TemplePray.SecondsStretch".Localized());

                OutputText("Scenes.Bogtemple.TemplePray.FeelNoDifferent".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1.5f));

            Save.SetInt("timesPrayed", Save.GetInt("timesPrayed", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TempleBathe()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleBathe.TravelingHarsh".Localized());

            OutputText("Scenes.Bogtemple.TempleBathe.WaterWarm".Localized());

            if (marielle.MarielleAvailable(false, false))
            {
                if (MarielleScene.Save.GetInt("commissionTime", 0) > View.Time.TotalTime && View.Time.IsTimeBetween(6.5f, 22.5f))
                    OutputText("Scenes.Bogtemple.TempleBathe.GlanceMarielle".Localized());
                else
                    OutputText("Scenes.Bogtemple.TempleBathe.GlanceOver".Localized((View.Time.IsTimeBetween(22.5f, 6.5f) ? "Marielle's silent tent, her lamp extinguished and the girl asleep somewhere behind her wares and materials. You doubt it would wake her up if you took a bath here--it's far enough away, so unless you decide to make a ruckus, you won't disturb her sleep." : "Marielle, who is apparently sketching something onto a stack of notes, pencil flying over the paper in swift, silent motions. She probably won't mind, or maybe even notice, if you take a bath here.")));

                ClearButtons();

                AddNextButton("Common.Button.BatheAlone".Localized(), () => TempleBatheSolo());

                AddNextButton("Common.Button.InviteHer".Localized(), marielle.Bathe);

                AddButton(14, "Common.Button.Back".Localized(), () => TempleBathBack());
            }
            else
                TempleBatheSolo(false);
        }

        public void TempleBathBack()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleBathBack.DontFeelLike".Localized());

            TempleMenu(false);
        }

        public void TempleBatheSolo(bool clear = true)
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleBatheSolo.LayNeatly".Localized());

            OutputText("Scenes.Bogtemple.TempleBatheSolo.SkinTingle".Localized());

            OutputText("Scenes.Bogtemple.TempleBatheSolo.StopThat".Localized());

            OutputText("Scenes.Bogtemple.TempleBatheSolo.AboutTime".Localized());

            OutputText("Scenes.Bogtemple.TempleBatheSolo.CrispContrast".Localized());

            OutputText("Scenes.Bogtemple.TempleBatheSolo.EventuallyReady".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.6f));

            Player.ChangeFatigue(-50);

            Player.HasGottenWashed();

            Save.SetInt("timesBathed", Save.GetInt("timesBathed", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TempleLeave()
        {
            ClearOutput();

            OutputText("Scenes.Bogtemple.TempleLeave.OutOfTemple".Localized());

            OutputText("Scenes.Bogtemple.TempleLeave.SteelingYourself".Localized());

            DoNext(() => ReturnToCamp(1));
        }

    }
}