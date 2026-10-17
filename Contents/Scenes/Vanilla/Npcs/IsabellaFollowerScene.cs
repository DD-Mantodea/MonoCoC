using MonoCoC.Attributes;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Enemies;
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
using System.Linq;
using MonoCoC.Utils;
using MonoCoC.Contents.Scenes.Vanilla.Places;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<IsabellaFollowerScene>("coc:isabella_follower")]
    public class IsabellaFollowerScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.IsabellaFollower);

        public int lastNursingCheck = 0;

        public string debugName => "Isabella";

        public string debugHint => "";

        public override void Entry()
        {
            IsabellaFollower();
        }

        public void Reset()
        {

            Save.SetInt("oviElixirAmount", 0);

            Save.SetIntList("babyNotVirgin", []);

            Save.SetBool("ghostJudged", false);
        }

        public bool IsabellaFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.IsabellaFollowerAccepted) == 1 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaCampDisabled) == 0;
        }

        public bool IsabellaAtCamp()
        {

            return IsabellaFollower() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIsabella) == 0;
        }

        public bool IsabellaAccent()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100)
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0)
                return true;

            return false;
        }

        public int IsabellaAffection(int mod = 0)
        {

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAffection, CoC.Instance.Flags.Get(GameFlagIds.IsabellaAffection) + mod);

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAffection) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.IsabellaAffection, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAffection) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.IsabellaAffection, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.IsabellaAffection);
        }

        public void IsabellaSprite()
        {

        }

        public void IsabellaKnockUpAttempt()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaPotencyState) > 0 && SceneInstance<IsabellaScene>.Get().Pregnancy.KnockUpChance(30 - SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren(), 90))
                SceneInstance<IsabellaScene>.Get().Pregnancy.KnockUp(PregnancyIds.PlayerFather, PregnancyStore.IncubationIsabella);
        }

        public void RegisterMacros()
        {

            RegisterMacro("isabellaonherb", CoC.Instance.Flags.Get(GameFlagIds.IsabellaPotencyState) == -1);

            RegisterMacro("isabellapregnancy", Player.Pregnancy.Event);

            RegisterMacro("w", (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 ? "v" : "w"));

            RegisterMacro("th", (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 ? "z" : "th"));
        }

        public void IsabellaMoovesInGreeting()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimesOfferedFollower) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.WarmSmileSpikesShield".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.You".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.SmileAdmitEnjoyTime".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.JokeMissedYouMore".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.SmirkWonderedMissedBody".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.RisingFullHeight".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.SashaysUpForHug".Localized());

                if (Player.Tallness < 80)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.CrushAroundHeadSmothering".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.FourNipplesOnChest".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.BlushSquirmEndureAffections".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.InviteHerToCamp".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), MoveTheBitchIn);

                AddButton(1, "Common.Button.No".Localized(), TurnDownIsabellaFollower);
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.ShieldWorkAtCamp".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMoovesInGreeting.StillWantsMovingIn".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Talk".Localized(), SceneInstance<IsabellaScene>.Get().TalkWithIsabella);

                AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), SceneInstance<IsabellaScene>.Get().NomOnMommaIzzysTits);

                if (Player.ShortestCockLength() <= SceneInstance<IsabellaScene>.Get().TinyPenis())
                    AddButton(2, "Common.Button.GetLicked".Localized(), SceneInstance<IsabellaScene>.Get().IzzyGivesSmallWangsFreeOral);
                else
                    AddButton(2, "Common.Button.GetLicked".Localized(), null);

                AddButton(3, "Common.Button.FightRape".Localized(), SceneInstance<IsabellaScene>.Get().FightIsabella);

                AddButton(4, "Common.Button.OfferOral".Localized(), SceneInstance<IsabellaScene>.Get().VolunteerToSlurpCowCunt);

                AddButton(5, "Common.Button.AcceptOffer".Localized(), MoveTheBitchIn);

                SetExitButton("Common.Button.Back".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaTimesOfferedFollower, CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimesOfferedFollower) + 1);
        }

        public void TurnDownIsabellaFollower()
        {
            ClearOutput();

            IsabellaAffection(-10);

            OutputText("Scenes.Isabellafollowerscene.TurnDownIsabellaFollower.PushBackFrustration".Localized());

            OutputText("Scenes.Isabellafollowerscene.TurnDownIsabellaFollower.SighsSlumpsAgainstShield".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), SceneInstance<IsabellaScene>.Get().TalkWithIsabella);

            AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), SceneInstance<IsabellaScene>.Get().NomOnMommaIzzysTits);

            if (Player.ShortestCockLength() <= SceneInstance<IsabellaScene>.Get().TinyPenis())
                AddButton(2, "Common.Button.GetLicked".Localized(), SceneInstance<IsabellaScene>.Get().IzzyGivesSmallWangsFreeOral);
            else
                AddButton(2, "Common.Button.GetLicked".Localized(), null);

            AddButton(3, "Common.Button.FightRape".Localized(), SceneInstance<IsabellaScene>.Get().FightIsabella);

            AddButton(4, "Common.Button.OfferOral".Localized(), SceneInstance<IsabellaScene>.Get().VolunteerToSlurpCowCunt);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void MoveTheBitchIn()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.MoveTheBitchIn.TearsShieldOut".Localized());

            OutputText("Scenes.Isabellafollowerscene.MoveTheBitchIn.CatchesStaringSultryLook".Localized());

            if (CampScene.HasCompanions())
                OutputText("Scenes.Isabellafollowerscene.MoveTheBitchIn.Newest".Localized());

            OutputText("Scenes.Isabellafollowerscene.MoveTheBitchIn.GatherMovePossessionsCamp".Localized());

            OutputText("Scenes.Isabellafollowerscene.MoveTheBitchIn.AddedToLoversMenu".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaCountdownToContraceptions, 240);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAffection, 100);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaFollowerAccepted, 1);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaPlainsDisabled, 1);

            ReturnToCamp(1);
        }

        public bool NursingCheck()
        {

            if (SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren() > 0)
            {
                if (View.Time.TotalTime == Math.Abs(lastNursingCheck))
                    return lastNursingCheck > 0;
                else
                    lastNursingCheck = (int)(View.Time.TotalTime);

                if (Random.Shared.Next(2) == 0)
                    return true;
                else
                {
                    lastNursingCheck *= -1;

                    return false;
                }
            }

            return false;
        }

        public void CallForFollowerIsabella()
        {
            ClearOutput();

            RegisterMacros();

            OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIsabella) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.CallTheCowGirl".Localized());

                if (NursingCheck())
                {
                    if (SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren() == 1)
                        OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.InArmsBelovedBaby".Localized(
                            SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender()
                        ));
                    else
                        OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.CarryingAChild".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.Isabella".Localized());

                    if (Player.Tallness < 72)
                        OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.PicksUpCrushingHug".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.CrushingHugTitsFlat".Localized());
                }

                OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.VatDoYouNeed".Localized());

                if (Player.HasCock())
                    if (Player.ShortestCockLength() > SceneInstance<IsabellaScene>.Get().TinyPenis())
                        OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.TooBigForBlowjobs".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaBlowjobsDisabled) == 0)
                            OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.AskHerNoSuck".Localized());
                        else
                            OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.AskToGiveBlowjobs".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.PutsDownMilkCanister".Localized());

                OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.HowCanSheHelp".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), IsabellasAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), IsabellaTalkMenu);

            AddButton(2, "Common.Button.Sex".Localized(), CampIzzySexMenu);

            AddButton(3, "Common.Button.Spar".Localized(), IsabellaSparMenu);

            if (SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren() > 0)
                AddButton(5, "Common.Button.Children".Localized(), ChildrenMenu);

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) < 0)
                AddRowButton(1, "Get Milk", GetMilk);

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100)
                AddRowButton(1, "Accent Coach", IsabellasAccentCoaching);
            else
                AddRowButton(1, "Accent Uncoach", IsabellaAccentUncoaching);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LaBova)) && (int)Player.Gender > 0)
            {
                OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.WouldDrinkProBova".Localized());

                AddRowButton(1, "GiveProBova", IsabellaBurps);
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)) && Player.IsPregnant())
            {
                OutputText("Scenes.Isabellafollowerscene.CallForFollowerIsabella.GiveOviElixir".Localized());

                AddRowButton(1, "GiveOviElixir", IsabellaTakesOviElixir);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIsabella) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIsabella) == 1)
            {
                AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);

                AddButton(14, "Common.Button.Back".Localized(), null );
            }
            else
                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void ChildrenMenu()
        {
            ClearOutput();

            if (SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren() == 1)
                OutputText("Scenes.Isabellafollowerscene.ChildrenMenu.OneBabyDayOld".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(0, "boy", "girl", "herm"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(0, "He", "She"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildAge(),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(0, "He", "She"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(0, "his", "her")
                ));
            else
            {
                OutputText("Scenes.Isabellafollowerscene.ChildrenMenu.YouHaveChildren".Localized(
                    SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren()
                ));

                OutputText("Scenes.Isabellafollowerscene.ChildrenMenu.BoyCount".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(1)
                ));

                OutputText("Scenes.Isabellafollowerscene.ChildrenMenu.GirlCount".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(2) + SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(4)
                ));

                OutputText("Scenes.Isabellafollowerscene.ChildrenMenu.HermaphroditeCount".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(3) + SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(5)
                ));
            }

            ClearButtons();

            if (SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(1) > 0)
                AddButton(0, "Common.Text.Son".Localized() + (SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(1) >= 2 ? "s" : ""), () => ChildrenMenu2((int)Gender.Male));
            else
                AddButton(0, "Common.Button.Sons".Localized(), null);

            if ((SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(2) + SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(4)) > 0)
                AddButton(1, "Common.Button.Daughter".Localized() + ((SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(2) + SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(4)) >= 2 ? "s" : ""), () => ChildrenMenu2((int)Gender.Female));
            else
                AddButton(1, "Common.Button.Daughters".Localized(), null);

            if ((SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(3) + SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(5)) > 0)
                AddButton(2, "Common.Text.Hermdaughter".Localized() + ((SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(3) + SceneInstance<IsabellaScene>.Get().GetIsabellaChildType(5)) >= 2 ? "s" : ""), () => ChildrenMenu2((int)Gender.Hermaphrodite));
            else
                AddButton(2, "Common.Button.Hermdaughters".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), CallForFollowerIsabella);
        }

        public void ChildrenMenu2(int childGender)
        {

            var randomChild = SceneInstance<IsabellaScene>.Get().ChooseRandomIsabellaChild(childGender switch
            {
                (int)Gender.Male => [IsabellaScene.OFFSPRING_HUMAN_BOYS],

                (int)Gender.Female => [IsabellaScene.OFFSPRING_HUMAN_GIRLS, IsabellaScene.OFFSPRING_COWGIRLS],

                _ => [IsabellaScene.OFFSPRING_HUMAN_HERMS, IsabellaScene.OFFSPRING_COWFUTAS]
            });

            RegisterMacro("isababyherm", childGender != (int)Gender.Male && childGender != (int)Gender.Female);

            RegisterMacro("isababyvirgin", Save.GetIntList("babyNotVirgin").Contains(randomChild));

            RegisterMacro("isababyboy", SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "boy","girl","girl"));

            RegisterMacro("isababyhim", SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "him", "her", "her"));

            RegisterMacro("isababyhis", SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "his", "her", "her"));

            RegisterMacro("isababyson", SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "son", "daughter", "daughter"));

            RegisterMacro("Isababyhe", SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "he", "she", "she"));

            OutputText("Scenes.Isabellafollowerscene.ChildrenMenu2.ZeroParagraphBreak".Localized(
                randomChild
            ));

            OutputText("Scenes.Isabellafollowerscene.ChildrenMenu2.HoldCloseAndRock".Localized(
                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "son", "daughter"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "him", "her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "his", "her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "He", "She"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(randomChild, "his", "her")
            ));

            ClearButtons();

            if (GameSettings.AllowBaby)
            {
                var notHornyEnoughForChild = Player.Lust < 33 || Player.Corruption + Player.Libido < 50;

                AddButton(0, "Common.Button.Cocknurse".Localized(), () => IsabellaChildCocknursingSelection(randomChild));

                AddButton(1, "Common.Button.Cunnilingus".Localized(), () => IsabellaChildCunnilingus(randomChild));

                AddButton(2, "Common.Button.Suckie".Localized(), () => IsabellaChildCockSucking(randomChild));
            }

            AddButton(14, "Common.Button.Leave".Localized(), CallForFollowerIsabella);
        }

        public void IsabellaChildCocknursingSelection(int child = 0)
        {
            ClearOutput();

            var x = Player.RandomCockThatFits(2, "width");

            var y = Player.RandomCockTooBig(2, "width");

            if (x >= 0 && y >= 0)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursingSelection.SmallerCockOrBigger".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her")
                ));

                ClearButtons();

                AddButton(0, "Common.Button.Small".Localized(), () => IsabellaChildCocknursing(x, child));

                AddButton(1, "Common.Button.Large".Localized(), () => IsabellaChildCocknursing(y, child));
            }
            else
            {
                x = (x < 0) ? Player.SmallestCockIndex() : x;

                IsabellaChildCocknursing(x, child);
            }
        }

        public void IsabellaChildCockSucking(int child = 0)
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCockSucking.CarryBabyToCushion".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCockSucking.LipsPressedToPalm".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCockSucking.PressTongueFlatAgainst".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCockSucking.FarCryFromFilling".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCockSucking.SnakeTongueDownShaft".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCockSucking.EnjoyedNeglectedOwnNeeds".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, 3f));

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) > 0 && !Save.GetBool("ghostJudged"))
                JudgmentalGhost();
            else
                ReturnToCamp(1);
        }

        public void IsabellaChildCocknursing(int x = 0, int child = 0)
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.GrippingSmallAdorableChild".Localized(
                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "boy", "girl"),

                Player.Mfn("paternal", "maternal", "parental"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "son", "daughter"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her")
            ));

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.TellComfortinglyNutrition".Localized(
                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her"),

                Player.CockDescript(x),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "son", "daughter"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "His", "Her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "he", "she")
            ));

            if (Player.Cocks[x].Thickness > 2)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.ShiversRunDownSpine".Localized(
                    Player.CockDescript(x),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "son", "daughter"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "His", "Her"),

                    Player.CockDescript(x),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "he", "she"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "he", "she"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "His", "Her")
                ));

                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.HeavenlyTouchLittleTongue".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "boy", "girl"),

                    Player.CockDescript(x),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her")
                ));

                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.BallsTenseUp".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.ShiversDownSpineAgain".Localized(
                    Player.CockDescript(x),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "son", "daughter"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her"),

                    Player.Mfn("paternal", "maternal", "parental")
                ));

                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.GruntsMoanPlungeDeep".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "son", "daughter"),

                    Player.CockDescript(x),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "he", "she")
                ));

                OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.BallsTenseUpAgain".Localized(
                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "boy", "girl"),

                    SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "his", "her")
                ));
            }

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCocknursing.WashChildClean".Localized(
                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her"),

                SceneInstance<IsabellaScene>.Get().GetIsabellaChildGender(child, "him", "her")
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, 3f));

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) > 0 && !Save.GetBool("ghostJudged"))
                JudgmentalGhost();
            else
                ReturnToCamp(1);
        }

        public void IsabellaChildCunnilingus(int child = 0)
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCunnilingus.HoldDaughterCloseKisses".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCunnilingus.CrouchBetweenThighs".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCunnilingus.FleshyTongueSinksIn".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaChildCunnilingus.WipeDryReturnMother".Localized());

            if (Save.GetIntList("babyNotVirgin").Contains(child))
                Save.SetIntList("babyNotVirgin", Save.GetIntList("babyNotVirgin").Append(child));

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, 2f));

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) > 0 && !Save.GetBool("ghostJudged"))
                JudgmentalGhost();
            else
                ReturnToCamp(1);
        }

        public void JudgmentalGhost()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.JudgmentalGhost.SoulTakerOnBaby".Localized());

            OutputText("Scenes.Isabellafollowerscene.JudgmentalGhost.AboutToCriticizeFetishes".Localized());

            OutputText("Scenes.Isabellafollowerscene.JudgmentalGhost.NotThatLoveExtremes".Localized());

            OutputText("Scenes.Isabellafollowerscene.JudgmentalGhost.BizarreChillGiddiness".Localized());

            OutputText("Scenes.Isabellafollowerscene.JudgmentalGhost.ThrillingConcludesDeadLong".Localized());

            Save.SetBool("ghostJudged", true);

            ReturnToCamp(1);
        }

        public void SendToFarm()
        {
            ClearOutput();

            IsabellaSprite();

            OutputText("Scenes.Isabellafollowerscene.SendToFarm.TeutonicTitMonsterFavor".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.SendToFarm.KnowsTheFarmAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.SendToFarm.YesKnowPlaceFarmdog".Localized());

            OutputText("Scenes.Isabellafollowerscene.SendToFarm.SwaysOffToLake".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmIsabella, 1);

            ReturnToCamp(1);
        }

        public void BackToCamp()
        {
            ClearOutput();

            IsabellaSprite();

            OutputText("Scenes.Isabellafollowerscene.BackToCamp.GambitPaysOff".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.BackToCamp.ComeBackKeepAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.BackToCamp.ComeBackKeep".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmIsabella, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void IsabellasAppearance()
        {
            ClearOutput();

            if (Player.IsPregnant())
            {
                var __sw0_1 = Player.Pregnancy.Event;

                if (__sw0_1 == 0 || __sw0_1 == 1)
                {
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTall".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.PotentBabymakingSkills".Localized());

                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.QuestionSeedPlanted".Localized());
                }

                if (__sw0_1 == 2)
                {
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallAgain".Localized());

                    if (Player.Libido >= 60)
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.WindBlowsWellLit".Localized());

                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.AskedOkaySwollenAnkles".Localized());
                }

                if (__sw0_1 == 3)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallThird".Localized());

                if (__sw0_1 == 4)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallFourth".Localized());

                if (__sw0_1 == 5)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallFifth".Localized());

                if (__sw0_1 == 6)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallSixth".Localized());

                if (__sw0_1 == 7)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallSeventh".Localized());

                if (__sw0_1 == 8)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.SleepsALotPregnancy".Localized());

                if (__sw0_1 == 9 || __sw0_1 == 10)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallEighth".Localized());

                if (true )
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallNinth".Localized());
            }
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellasAppearance.TwoMetersTallTenth".Localized());

            CallForFollowerIsabella();
        }

        public void CampIzzySexMenu()
        {

            ClearButtons();

            AddButton(2, "Common.Button.Hotdog".Localized(), null);

            AddButton(4, "Common.Button.Tentaclesex".Localized(), null);

            AddButton(5, "Common.Button.GetSucked".Localized(), null);

            AddButton(6, "Common.Button.FuckHer".Localized(), null);

            Action tentacle = null;

            if (IzzyTentacleRapeBool() && Player.Lust >= 33)
                tentacle = TentacleBoneFollowerIzzy;

            Action hotdog = null;

            Action bjToggle = null;

            var bjTogText = "";

            Action getSucked = null;

            if (Player.HasCock())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaBlowjobsDisabled) == 0)
                    bjTogText = "No BJs";
                else
                    bjTogText = "Plz BJs";

                bjToggle = ToggleIsabellasMorningWoodChopping;

                if (Player.ShortestCockLength() <= SceneInstance<IsabellaScene>.Get().TinyPenis() && Player.Lust >= 33)
                    getSucked = SceneInstance<IsabellaScene>.Get().IzzyGivesSmallWangsFreeOral;
            }

            OutputText("Scenes.Isabellafollowerscene.CampIzzySexMenu.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellafollowerscene.CampIzzySexMenu.MilkFromTheTap".Localized());

            if (tentacle != null)
                OutputText("Scenes.Isabellafollowerscene.CampIzzySexMenu.TentaclesShowGrandTime".Localized());

            if (getSucked != null)
                OutputText("Scenes.Isabellafollowerscene.CampIzzySexMenu.HaveHerSuckYou".Localized());

            OutputText("Scenes.Isabellafollowerscene.CampIzzySexMenu.UnderSkirtService".Localized());

            if (Player.Lust < 33)
                OutputText("Scenes.Isabellafollowerscene.CampIzzySexMenu.NotArousedEnough".Localized());

            if (Player.HasCock() && Player.Lust >= 33)
                hotdog = RepeatGermanBratwurstInCamp;

            Action fuckHer = null;

            if (Player.HasCockThatFits(164) && Player.Lust >= 33)
                fuckHer = FuckIsabella;

            AddButton(0, bjTogText, bjToggle);

            AddButton(1, "Combat.Ability.MinotaurLordMinotaurDrankMalk.Name".Localized(), SceneInstance<IsabellaScene>.Get().NomOnMommaIzzysTits);

            AddButton(2, "Common.Button.Hotdog".Localized(), hotdog);

            AddButton(3, "Common.Button.ServiceHer".Localized(), SceneInstance<IsabellaScene>.Get().VolunteerToSlurpCowCunt);

            AddButton(4, "Common.Button.Tentaclesex".Localized(), tentacle);

            AddButton(5, "Common.Button.GetSucked".Localized(), getSucked);

            AddButton(6, "Common.Button.FuckHer".Localized(), fuckHer);

            AddButton(14, "Common.Button.Back".Localized(), CallForFollowerIsabella);
        }

        public void IsabellasAccentCoaching()
        {
            ClearOutput();

            int temp = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingCooldown) > 1)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.ShakesHeadNein".Localized());

                CallForFollowerIsabella();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.OfferLessonsReduceAccent".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.YoursCloserToLand".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaAccentTrainingPercent, 1);
            }

            var result = Player.Intelligence / 5 + Random.Shared.Next(9);

            int accentChange = 0;

            if (result < 12)
            {
                temp = Random.Shared.Next(3);

                if (temp == 0)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.SpendHourLittleProgress".Localized());
                else
                    if (temp == 1)
                    {
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.NoProgressAtAll".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                    }
                    else
                    {
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.CantFocusAtAll".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.LeaveLessonStiffness".Localized());
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.LeaveLessonWetPanties".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.SentencePeriod".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
                    }
            }
            else
                if (result < 14)
                {
                    temp = Random.Shared.Next(2);

                    if (temp == 0)
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.HourOfDiction".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.WordChoiceCorrect".Localized());

                    accentChange = 4 + Random.Shared.Next(5);
                }
                else
                    if (result <= 20)
                    {
                        temp = Random.Shared.Next(2);

                        if (temp == 0)
                            OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.HourTeachingProperlyFlustered".Localized());
                        else
                            if (temp == 1)
                                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.WordChoiceCorrectAgain".Localized());

                        accentChange = 8 + Random.Shared.Next(8);
                    }
                    else
                    {
                        temp = Random.Shared.Next(2);

                        if (temp == 0)
                            OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.WorkOnLanguage".Localized());
                        else
                            OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.ReduceHerAccent".Localized());

                        accentChange = 12 + Random.Shared.Next(10);
                    }

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange >= 100)
                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.ProudlyLearnedEverything".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 90 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange >= 90)
                    OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.SpeaksWithoutAccent".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 75 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange >= 75)
                        OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.TwoSentencesNoMistake".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 50 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange >= 50)
                            OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.GainedControlSpeakProperly".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 25 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange >= 25)
                                OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.GettingBetterAtLessons".Localized());
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 10 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange >= 10)
                                    OutputText("Scenes.Isabellafollowerscene.IsabellasAccentCoaching.CatchesOwnMistakes".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAccentTrainingPercent, CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) + accentChange);

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.IsabellaAccentTrainingPercent, 100);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAccentTrainingCooldown, 4 + Random.Shared.Next(13));

            ReturnToCamp(1);
        }

        public void IsabellaAccentUncoaching()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IsabellaAccentUncoaching.BackToOldAccent".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaAccentUncoaching.AccentReverted".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAccentTrainingPercent, 0);

            CallForFollowerIsabella();
        }

        public void IsabellaMorningWakeupCall()
        {

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMorningFellatioCount, CoC.Instance.Flags.Get(GameFlagIds.IsabellaMorningFellatioCount) + 1);

            var x = Player.ShortestCockIndex();

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SighingDreamSexualTone".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.QuietSquishingNoise".Localized(
                Player.Mf("man", "herm")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMorningFellatioCount) == 1)
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.IsabellaFellatingYou".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.TraditionalGoodMorningBlowjobs".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.MassiveTongueMoldsAround".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.MischievousGaze".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.GroanLiftingHips".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.BallsAgainstChin".Localized(
                    Player.CockDescript(x)
                ));
            else
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.LipsSealAround".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.Sheath".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.Base".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SentencePeriod".Localized());
            }

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.BrushesHairBack".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.HumsSoftlySlowVibration".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.BallsClenchReleaseSeed".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.ClenchTightlyReleaseSeed".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.VacuumTightLips".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.CumAtSameTime".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.RestOfYour".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.OnePenisLeftUntouched".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SprayingAcrossBellyChest".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.PluralS".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.PumpsToEaseTension".Localized());
            }

            if (Player.CumQ() < 250)
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SlurpsAllDown".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.CantGetItAll".Localized());
                else
                {
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.GurglesVolumeJizzForced".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.TooSemenStuffed".Localized());

                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SlurpsEscapedCum".Localized());
                }

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.RedheadShudders".Localized());

            if (Player.CumQ() >= 2000)
                OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SpunkTainted".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.MilkAroundYourLegs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMorningFellatioCount) == 1)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.TastyTreatsAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.MmmPenisTastyTreats".Localized());
            }
            else
                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.CumHardForMomma".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.HopeYouDidntMind".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.DidntMindThatSnack".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SayEndParagraph".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaMorningWakeupCall.SlumpDownTensionOozes".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1.5f));

            Go<CampScene>();
        }

        public void ToggleIsabellasMorningWoodChopping()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaBlowjobsDisabled) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.ManageWoodMyself".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.ChangeMindAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.ChangeMind".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaBlowjobsDisabled, 1);
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.You".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.BlushAnd".Localized());
                else
                    if (Player.Corruption >= 66)
                        OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.SmirkAnd".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.StillInterested".Localized());

                if (Player.ShortestCockLength() > SceneInstance<IsabellaScene>.Get().TinyPenis())
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.OnlyOnceYouShrink".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.BattlingDemonsUnsated".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellasMorningWoodChopping.SayEnd".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaBlowjobsDisabled, 0);
            }

            CampIzzySexMenu();
        }

        public void RepeatGermanBratwurstInCamp()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.HelpBlowOffPressure".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimeSinceLastHotdogging) < 5 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimeSinceLastHotdogging) > 0)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.AlreadyTookCareAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.AlreadyTookCare".Localized());
            }
            else
                if (Player.CockArea(x) > 38)
                {
                    x = Player.BiggestCockIndex();

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.OhYesPentUp".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SeePentUpAgain".Localized());
                }
                else
                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.TracesFingerAccent".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.GleefullyMusesTracesFinger".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SayEndParagraph".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SheepishlyAffirmative".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.NotSeeingReasonAdmit".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.KnowingTwinkleAgreeIntuition".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.FoldsArmsAcrossBelly".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.FeelWeightOfShame".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.FeelMildlyAshamed".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ColorAtGettingCaught".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.DoesntDwellPivotsHips".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.StareOrUndress".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.CrooksFingerCommandsAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.CrooksFingerCommands".Localized());

            if (Player.CockArea(x) <= 38)
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.Little".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.Snake".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ApproachingSlowlyDrinkIn".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ContinuesGyrations".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.CockTwitchingForMe".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.NodCatchingWhiffAroma".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.DrawnUpByDick".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.HelpYouCum".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.FeelingMoreInControl".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.MommaWillTakeCare".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.CurvaceousStanceShifts".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.OtherPenisShoved".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.OtherPenisesShovedAside".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ArchesAndSlides".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 38)
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.DevouredByButtCheeks".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.MastRisingFromCrack".Localized());

            OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.NoShameCummingNow".Localized());

            if (Player.CockArea(x) <= 38)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.TeasesFlexesOneAccent".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.TeasesFlexesOne".Localized(
                        Player.CockDescript(x)
                    ));

                if (silly && (Player.LustPercent < 50 || Player.Libido <= 20))
                {
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.CockDeflatesPillowTalk".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, -99f));

                    ReturnToCamp(1);

                    return ;
                }

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SwitchesRhythm".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.StrugglingNotToCum".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.RoughlyLiftsUpTuts".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.DeniesPenetration".Localized(
                    Player.CockHead(x)
                ));

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.OwnerOfTightPucker".Localized(
                    Player.CockDescript(x)
                ));

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.NothingButWait".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.NothingButWaitAgain".Localized(
                        Player.CockDescript(x)
                    ));

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.InSpiteTeasesFriction".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.FrictionDespiteTeases".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SpendEveryOunce".Localized());

                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.PleasedExpressionBellyBloats".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.EventuallyGrowingPregnantCurve".Localized());

                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.WhimpersMurmursFertileBoy".Localized());
                }

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.WrithesInBlissForever".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.TeasesFlexAccent".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.TeasesFlexesOneThird".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                if (Player.Cocks[x].Length > 48)
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ReachAsHighAs".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.To".Localized(
                        Player.CockHead(x)
                    ));

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.DrawingAnguishedMoan".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.GrabBackOntoChair".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ReachesBetweenLegsTo".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SqueezeYourBalls".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.RubYourSoakedVulva".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.CaressYourTaint".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.TeasesSlightDisdain".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ShudderTryingHoldStill".Localized(
                    Player.CockHead(x),

                    Player.CockDescript(x)
                ));

                if (Player.CumQ() <= 200)
                    OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.AssGetsNiceGlaze".Localized());
                else
                    if (Player.CumQ() <= 500)
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.AssCompletelyGlazed".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.BackAndHairFlooded".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.ParagraphBreak".Localized());

                OutputText("Scenes.Isabellafollowerscene.RepeatGermanBratwurstInCamp.SmearsJismOnSkin".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaTimeSinceLastHotdogging, 1);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaTimesHotdogged, CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimesHotdogged) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            ReturnToCamp(1);
        }

        public bool IzzyTentacleRapeBool()
        {

            int counter = 0;

            var temp = Player.CockTotal();

            while (counter < Player.CountCocksOfType("TENTACLE") && temp > 0)
            {
                temp--;

                if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle") && Player.Cocks[temp].Length >= 24)
                    counter += 1;
            }

            if (counter >= 3)
                return true;
            else
                return false;
        }

        public void TentacleBoneFollowerIzzy()
        {
            ClearOutput();

            var t1 = -1;

            var t2 = -1;

            var t3 = -1;

            var t4 = -1;

            var t5 = -1;

            var t6 = -1;

            var t7 = -1;

            var t8 = -1;

            var t9 = -1;

            var t10 = -1;

            float counter = 0;

            var temp = Player.Cocks.Count;

            while (counter < Player.CountCocksOfType("TENTACLE") && temp > 0)
            {
                temp--;

                if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle") && Player.Cocks[temp].Length >= 24)
                {
                    counter += 1;

                    if (t1 == -1)
                        t1 = temp;
                    else
                        if (t2 == -1)
                            t2 = temp;
                        else
                            if (t3 == -1)
                                t3 = temp;
                            else
                                if (t4 == -1)
                                    t4 = temp;
                                else
                                    if (t5 == -1)
                                        t5 = temp;
                                    else
                                        if (t6 == -1)
                                            t6 = temp;
                                        else
                                            if (t7 == -1)
                                                t7 = temp;
                                            else
                                                if (t8 == -1)
                                                    t8 = temp;
                                                else
                                                    if (t9 == -1)
                                                        t9 = temp;
                                                    else
                                                        if (t10 == -1)
                                                            t10 = temp;
                }
            }

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.ExposeTentacleCocks".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.MeinGottSoHug".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.MyGodSoHug".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.SayEndParagraph".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.GuideSquirmingBunch".Localized(
                    Player.CockDescript(t1),

                    Player.CockDescript(t2)
                ));
            else
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.GuideSquirmingAgain".Localized(
                    Player.CockDescript(t1),

                    Player.CockDescript(t2)
                ));

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.EventuallySuccumbs".Localized());

            if (Player.CanFly() && Player.Strength >= 90)
            {
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.FlapWingsMobilizingMuscle".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.VaitGoingFallAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.WaitGoingToFall".Localized());

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.AssureHoldingHer".Localized());
            }

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.BringOtherTentacles".Localized(
                Player.CockDescript(t3),

                Player.CockDescript(t3),

                Player.CockDescript(t3)
            ));

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic || Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake || Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.LongExtensibleTongueLick".Localized());

            if (t4 >= 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.AnotherPrickUnused".Localized(
                    Player.CockDescript(t4),

                    Player.CockDescript(t4)
                ));

            if (t5 >= 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.StillHaveJunkLeft".Localized(
                    Player.CockDescript(t5),

                    Player.CockDescript(t5)
                ));

            if (t6 >= 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.TentaclePackerLeftBehind".Localized(
                    Player.CockDescript(t6),

                    Player.CockDescript(t5),

                    Player.CockDescript(t6),

                    Player.CockDescript(t6),

                    Player.CockDescript(t5)
                ));

            if (t7 >= 0)
            {
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.EntrancedByNudeBody".Localized(
                    Player.CockDescript(t7),

                    Player.CockDescript(t7)
                ));

                if (t8 >= 0)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.JoinedByEachPrick".Localized(
                        Player.CockDescript(t8)
                    ));

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.HandlingTitsPounding".Localized());

                if (t8 >= 0)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.FondlingPricksTremble".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.BoobFondlingPrickTrembles".Localized());

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.ParagraphBreak".Localized());
            }

            if (t9 >= 0)
            {
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.OnlySoMuchSpace".Localized());

                if (t10 >= 0)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.CocksLeftNothing".Localized(
                        Player.CockDescript(t9),

                        Player.CockDescript(t10)
                    ));
                else
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.LeftWithNothing".Localized(
                        Player.CockDescript(t9)
                    ));

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.TiredAndAroused".Localized());

            if (Player.CanFly() && Player.Strength >= 90)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.WingsAndYour".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.GroinFeelsSore".Localized());

            if (Player.CanFly() && Player.Strength >= 90)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.FeetBackOnSoil".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.ClimaxComingCloserSwooning".Localized());

            if (t4 >= 0)
            {
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.CommaSpace".Localized());

                if (t5 < 0)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.AndAgain".Localized());

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.HerColon".Localized());
            }

            if (t5 >= 0)
            {
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.CommaSpaceAgain".Localized());

                if (t7 < 0 || t10 < 0)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.AndAgain".Localized());

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.HerPussy".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.UntilLooksMonthsPregnant".Localized());
            }

            if (t7 >= 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.DrowningNipplesWithSpunk".Localized());

            if (t10 >= 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.CumOnFaceStaring".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.LusciousEyesAre".Localized());

            if (t10 < 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.Eventually".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.Immediately".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.MaskedInFluidUnload".Localized());

            if (t4 >= 0)
            {
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.HerOwnRectal".Localized());

                if (t5 >= 0)
                    OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.AndVaginal".Localized());

                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.SquirtsLookRidiculous".Localized());
            }

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.LustFueledFountain".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.Sack".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.Groin".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.FeelsEmptyDevoidFluid".Localized());

            OutputText("Scenes.Isabellafollowerscene.TentacleBoneFollowerIzzy.KeepHoldingWithMoment".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, .3f));

            ReturnToCamp(1);
        }

        public void MilktasticLacticLactation()
        {
            ClearOutput();

            RegisterMacros();

            OutputText("Scenes.Isabellafollowerscene.MilktasticLacticLactation.OhhMeinMilkers".Localized());

            OutputText("Scenes.Isabellafollowerscene.MilktasticLacticLactation.MoansDistractLookOver".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), IzzyMilkingMeinMilkersMya);

            AddButton(1, "Common.Button.No".Localized(), IzzyMilkYourselfDamnit);
        }

        public void IzzyMilkYourselfDamnit()
        {

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMilkedYet, 0);

            ReturnToCamp(1);
        }

        public void IzzyMilkingMeinMilkersMya()
        {
            ClearOutput();

            if (Player.Corruption < 50)
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.Concerned".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.Idly".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.SeeWhatTheMatter".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.LongSinceMilkedAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.AhhLongSinceMilked".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.SayEndParagraph".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.BiggerThanEver".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.ZinkCouldHelpAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.CouldHelpWithMilkers".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.MouthWatersDrinkingIdea".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.ProudWarrioressDistraught".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.UnbearableButQuickly".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.KindOfArousing".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.TripToWhitneysFarm".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya.ForgottenAllMilkers".Localized());

            IzzyMilkingMeinMilkersMya2();
        }

        public void IzzyMilkingMeinMilkersMya2()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.TakenPlaceInStall".Localized());

            if (Player.Thickness <= 50)
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.CalibratedSlenderForm".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.WeightierBulk".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.ProcessBeginsEarnestGroans".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.MeinUddersPopAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.MyUddersPopPlain".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.TrickleThenGush".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.HaventSeenYouWhile".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.LollingTongueNods".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.HadMuchToDo".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.FlowAbatesHarnessLowers".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.WhitneyReturnsWarmBottles".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.NodsEmphaticallyAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.NodsEmphaticallyPlain".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.You".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.ConsiderForbidding".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.LeftToSlakeThirst".Localized());

            OutputText("Scenes.Isabellafollowerscene.IzzyMilkingMeinMilkersMya2.WhatDoYouSay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.AllowIt".Localized(), AllowIzzyMilkerUse);

            AddButton(1, "Common.Button.Mine".Localized(), NoMilkingMilky);
        }

        public void AllowIzzyMilkerUse()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.AllowIzzyMilkerUse.FreeToComeWhenever".Localized());

            if (Player.Tallness < 72)
                OutputText("Scenes.Isabellafollowerscene.AllowIzzyMilkerUse.Face".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.AllowIzzyMilkerUse.Chest".Localized());

            OutputText("Scenes.Isabellafollowerscene.AllowIzzyMilkerUse.AssuresAlwaysComeFirst".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Isabellafollowerscene.AllowIzzyMilkerUse.HunchCatchAtMilkers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMilkedYet, -1);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.IsabellaMilk), 1);

            ReturnToCamp(1);
        }

        public void NoMilkingMilky()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.NoMilkingMilky.ArmAroundWaist".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Isabellafollowerscene.NoMilkingMilky.RoughlyGrab".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.NoMilkingMilky.GentlyCaress".Localized());

            OutputText("Scenes.Isabellafollowerscene.NoMilkingMilky.SensitiveNipplesLeftBreast".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Isabellafollowerscene.NoMilkingMilky.Painful".Localized());

            OutputText("Scenes.Isabellafollowerscene.NoMilkingMilky.ArousalAtTouchBlushing".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMilkedYet, -2);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.IsabellaMilk), 1);

            ReturnToCamp(1);
        }

        public void GetMilk()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.GetMilk.WantBottleOfMilk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkCooldown) > 0)
            {
                OutputText("Scenes.Isabellafollowerscene.GetMilk.ShakesHeadTellsYou".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) == -1)
                    OutputText("Scenes.Isabellafollowerscene.GetMilk.MilkGoneCheckBack".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.GetMilk.SoreWaitTillLater".Localized());

                CallForFollowerIsabella();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) == -1)
                OutputText("Scenes.Isabellafollowerscene.GetMilk.FetchesSpareBottle".Localized());
            else
            {
                OutputText("Scenes.Isabellafollowerscene.GetMilk.NodsEmptyBottleExpose".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetMilk.SmileWrylyTeaseNipples".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.GetMilk.FeelsGreatHurryAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.GetMilk.FeelsGreatHurryPlain".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetMilk.StimulationSetsOffFlows".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.GetMilk.Zat".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.GetMilk.That".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetMilk.FeltGoooodThankYou".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMilkedYet, -2);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMilkCooldown, 7 + Random.Shared.Next(4));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.IsabellaMilk), 1);

            ReturnToCamp(1);
        }

        public void AngryMurble()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.AngryMurble.MarbleInTow".Localized());

            OutputText("Scenes.Isabellafollowerscene.AngryMurble.ReasonMarbleCame".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaMurbleBleh, 2);

            Go<CampScene>();
        }

        public void IsabellaSparMenu()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaSparringIntro) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.SmilesSuggestSparringNods".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.GoodForBothAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.GoodForBothPlain".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.SeemsLogicalMemoryLane".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaSparringIntro, 1);
            }
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.SuggestSparringSession".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.AsksVhatStakesAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.HowShouldWeFight".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaSparMenu.SparLightOrHard".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Light".Localized(), () => Sparring(2));

            AddButton(1, "Common.Button.Hard".Localized(), () => Sparring(1));

            AddButton(14, "Common.Button.Back".Localized(), CallForFollowerIsabella);
        }

        public void Sparring(int type = 1)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaSparringIntro) == 1)
            {
                OutputText("Scenes.Isabellafollowerscene.Sparring.OldFurnitureBlunts".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaSparringIntro, 2);
            }
            else
                OutputText("Scenes.Isabellafollowerscene.Sparring.MakeShiftCamp".Localized());

            OutputText("Scenes.Isabellafollowerscene.Sparring.RapsShieldSteel".Localized());

            if (type == 1)
                OutputText("Scenes.Isabellafollowerscene.Sparring.PlayingForKeeps".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "isabella")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CurrentMonster.CreateStatusEffect(StatusEffectIds.Sparring, type, 0, 0, 0);

            CombatController.Instance!.Context.Enemy.Gems = 0;

            if (type == 2)
                CombatController.Instance!.Context.Enemy.Experience = CombatController.Instance!.Context.Enemy.Experience / 2;
        }

        public void IsabellaBurps()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LaBova));

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaProbovaBurpCount) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.InspectsBottleAccent".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.VellNuzzingIrreversible".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.NothingCantChangeBack".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.AssuranceWontChangeAspects".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.GulpsProBova".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.GurglingGut".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.LossForWords".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.EyesWidenRealizationStrikes".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Run".Localized(), RunAwayFromIzzyBurps);

                AddButton(1, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), GetIzzyBurped);
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.AnotherProBova".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.SureAboutZisAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.SureAboutThisHuh".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaBurps.WarnsBottomHemLifts".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Run".Localized(), RunAwayFromIzzyBurps);

                AddButton(1, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), GetIzzyBurped);
            }
        }

        public void RunAwayFromIzzyBurps()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaProbovaBurpCount) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.CommonSenseStepBack".Localized());

                OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.GaseousAssaultFades".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.VatYouGiveAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.WhatDidYouGive".Localized());

                OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.ComplainsLipDroopedPout".Localized());

                OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.ConcedesBlusterLost".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.BodyRejectedZisAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.MyBodyRejectedThis".Localized());

                CallForFollowerIsabella();
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.CommonSenseAgain".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.GaseousAssaultDiminishesAgain".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.GaseousAssaultDiminishesThird".Localized());

                OutputText("Scenes.Isabellafollowerscene.RunAwayFromIzzyBurps.NodSmilesMovesAway".Localized());

                CallForFollowerIsabella();
            }
        }

        public void GetIzzyBurped()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaProbovaBurpCount, CoC.Instance.Flags.Get(GameFlagIds.IsabellaProbovaBurpCount) + 1);

            var changed = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaProbovaBurpCount) == 1)
            {
                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.IgnoreWarningMoveHelp".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.LastSyllableCutOff".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.PressStomachHarder".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.BelchFrenzyCalms".Localized());

                if (Player.Body.Get<LowerBodyPart>().Id != BodyPartTypes.Hoofed || Player.Horns.Id.Path == "none" || Player.Horns.Id != BodyPartTypes.CowMinotaur)
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.StandAndFall".Localized());

                if (Player.Body.Get<LowerBodyPart>().Id != BodyPartTypes.Hoofed)
                {
                    if (Player.IsTaur())
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.GazeSettlesOnHooves".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.GazeSettlesOnPlaceholder".Localized(
                            Player.Feet(),

                            Player.Feet()
                        ));

                    changed = true;
                }

                if (Player.Horns.Id.Path == "none" || Player.Horns.Id != BodyPartTypes.CowMinotaur)
                {
                    if (Player.Horns.Id.Path != "none")
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.HornsQuiverReform".Localized(
                            Player.Mf("bull", "cow")
                        ));
                    else
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TwoNubsSproutForehead".Localized());

                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ThrowsBalanceOff".Localized());

                    changed = true;
                }

                if (Player.Ears.Id != BodyPartTypes.Cow)
                {
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.EarsWiggleMigrateHigher".Localized());

                    changed = true;
                }

                if (Player.Body.Get<TailPart>().Id != BodyPartTypes.Cow)
                {
                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TipOfTailReshapes".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TailCreepsOutArmor".Localized());

                    changed = true;
                }

                if (Player.HasVagina() && Player.BiggestBreastSize() < 15 && Player.BiggestLactation() < 2)
                {
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.UnbeknownstCowGasBreasts".Localized());

                    changed = true;
                }
                else
                    if (Player.HasVagina() && Player.BiggestLactation() >= 2)
                    {
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.BreastsUnderChin".Localized());

                        changed = true;
                    }

                if (Player.HasCock() && Player.BiggestCockLength() < 18)
                {
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.PressureBuildsCrotch".Localized());

                    if (Player.Balls < 0)
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TwoTesticlesFallScrotum".Localized());
                    else
                        if (Player.BallSize < 4)
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TesticlesEngorge".Localized());

                    changed = true;
                }

                if (changed && Player.HasVagina() && Player.Race != "cow-girl")
                {
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.WavesAwayGas".Localized());

                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.CantDisagree".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    {
                        if (Player.Tallness < 87)
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TallerTwinsAccent".Localized());
                        else
                            if (Player.Tallness < 95)
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.LookZatGrownAccent".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ZoughHeightDepartmentAccent".Localized());
                    }
                    else
                        if (Player.Tallness < 87)
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.TallerTwinsPlain".Localized());
                        else
                            if (Player.Tallness < 95)
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.LookAtThatGrown".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ThoughHeightDepartment".Localized());

                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.SayEndParagraph".Localized());
                }
                else
                    if (changed && Player.HasCock() && Player.Race != "cow-morph")
                    {
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.SayStart".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.NeinZisVontAccent".Localized());
                        else
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.NoThisIsAwful".Localized());

                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.CriesApproachesEyesLocked".Localized());

                        if (Player.BiggestCockLength() < 18)
                        {
                            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.NoBiggerAccent".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.NoBiggerPlain".Localized());

                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.RaiseEyebrowSelectiveWorry".Localized());

                            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ImportantMeinPussyAccent".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ImportantGirlParts".Localized());
                        }

                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ParagraphBreak".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.InspectsCuriously".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.GasHarmAccent".Localized());
                        else
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.GasDidntHarmYou".Localized());
                    }

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.ShufflesHoofFidgets".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.NuzzlingInevitable".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.DrawsBackBreath".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.FreeingArmPullingIn".Localized());

                if (Player.Body.Get<LowerBodyPart>().Id != BodyPartTypes.Hoofed || Player.Horns.Id.Path == "none" || Player.Horns.Id != BodyPartTypes.CowMinotaur || Player.Ears.Id != BodyPartTypes.Cow || Player.Body.Get<TailPart>().Id != BodyPartTypes.Cow || (Player.HasCock() && Player.BiggestCockLength() < 18))
                {
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.FamiliarCowBoyForm".Localized());

                    if ((!Player.HasCock() || Random.Shared.Next(2) == 0) && Player.HasVagina())
                        OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.BeamingEagernessSloshingBosom".Localized());
                    else
                        if (Player.HasCock())
                            OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.FrustrationDickGrows".Localized());
                }
                else
                    OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.FumesExacerbateDesire".Localized());

                OutputText("Scenes.Isabellafollowerscene.GetIzzyBurped.SmilesDrawsInApologize".Localized());
            }

            AddButton(0, "Common.Button.Yes".Localized(), AcceptCowpology);

            AddButton(1, "Common.Button.No".Localized(), () => DeclineIzzysCowBurpApology(changed));
        }

        public void DeclineIzzysCowBurpApology(bool changed)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaProbovaBurpCount) == 1)
            {
                OutputText("Scenes.Isabellafollowerscene.DeclineIzzysCowBurpApology.WeirdedOutToReassure".Localized());

                if (changed)
                    OutputText("Scenes.Isabellafollowerscene.DeclineIzzysCowBurpApology.HourLaterOldForm".Localized());
            }
            else
                OutputText("Scenes.Isabellafollowerscene.DeclineIzzysCowBurpApology.DespiteLustTurnDown".Localized());

            ReturnToCamp(1);
        }

        public void AcceptCowpology()
        {
            ClearOutput();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaProbovaBurpCount) == 1)
            {
                OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.FlashingSmirkBeckonStrip".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.PullTitsApartSlap".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.ExpertlyLicksFlicksUnderside".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.SensesAndRaisesHead".Localized());

                    if (Player.CumQ() < 100)
                        OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.SpurtDeepInThroat".Localized());
                    else
                        if (Player.CumQ() <= 500)
                            OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.WarmCumBulgingNeck".Localized());
                        else
                            if (Player.CumQ() <= 1000)
                                OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.EyesBulgeAtSemen".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.GenuineConcern".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.ParagraphBreak".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.PullsLipsAwayAccent".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.PullsLipsAwayPlain".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.HeftsHerBreast".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LowerToGround".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.StakesDouble".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LactationBreakingPointGrinds".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.PostOrgasmicLethargy".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.BeckonStripAgain".Localized());

                if (Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
                {
                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.PullTitsSlapAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LicksUndersideAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.SensesRaisesHeadAgain".Localized());

                    if (Player.CumQ() < 100)
                        OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.SpurtThroatAgain".Localized());
                    else
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.WarmCumNeckAgain".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.EyesBulgeAgain".Localized());
                            else
                                OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.GenuineConcernAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.ParagraphBreakAgain".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LipsAwayAccentAgain".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LipsAwayPlainAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.HeftsBreastAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LowerToGroundAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.StakesDoubleAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.BreakingPointAgain".Localized());

                    OutputText("Scenes.Isabellafollowerscene.AcceptCowpology.LethargyAgain".Localized());
                }
            }

            ReturnToCamp(1);
        }

        public void ReceiveAllTheCowTOngues()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.PullsIntoHerLap".Localized());

            if (Player.Tallness <= 78)
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.NearlyAbsorbedMoundsTitflesh".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.NuzzlesNeckCrushing".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.SlipsArmsAround".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.FemLubeTrickle".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.ChidesKneadingTitfleshAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.ChidesKneadingTitflesh".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.TryRestrainOverwhelming".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.EnoughOfThatAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.MmmEnoughOfThat".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.SaysTakingShouldersFlips".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.TimeForMainCourse".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.LicksLipsCowTongue".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.BlowsOnYourGroin".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.LaughsAbsorbentMaterialSoaks".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.MindfulLeakAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.WatchWhereYouLeak".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.MouthOnYourPussy".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.PryFingersOffArmrests".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.DiggingHerScalp".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.DeepestReaches".Localized());

            if (Player.GetClitLength() < 1)
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.ClitGrindingIntoNose".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.SoakedEnough".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.OrgasmCarriesOn".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.VellHowVasAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.WhatDidYouThink".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.AsksCoylyLickingFemspray".Localized());

            OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.ThumbsUpPassOut".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.ReceiveAllTheCowTOngues.ReturnsWithDrink".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void FuckIsabella()
        {
            ClearOutput();

            var x = Player.ShortestCockIndex();

            if (Player.Cocks[x].Length > SceneInstance<IsabellaScene>.Get().TinyPenis())
                x = Player.CockThatFits(164);

            var y = x + 1;

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.HelpBlowOffSteam".Localized(
                y
            ));

            if (Player.Cocks[x].Length <= SceneInstance<IsabellaScene>.Get().TinyPenis())
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.PullsOutPreparing".Localized(
                    y
                ));
            else
            {
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.CocksEyebrowCrossingArms".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.PartialToSmallAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.LikesThemTiny".Localized());
            }

            if (Player.Cocks[x].Length <= SceneInstance<IsabellaScene>.Get().TinyPenis())
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.OhJa".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.OhYeah".Localized());

                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.SaysStraighteningHerselfUp".Localized());

                if (Player.Tallness < 72)
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.TowerOverDiminutiveFrame".Localized());

                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.SentencePeriod".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.TinyCockIrresistible".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.CutestLittleCock".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.UrgeReconsider".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.ReliefMeinOwnAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.ReliefMyOwnPlain".Localized());
            }

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.ReachesBehindUndo".Localized(
                y
            ));

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.DropsToKneesCups".Localized(
                y
            ));

            if (Player.CockArea(x) < 50)
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.UtterlyEngulfingLength".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.SurroundingFairAmountShaft".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.WorksTitsAlong".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.GroanPleasureStrokeHair".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.NichtZoRough".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.NotSoRough".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.OpenThighsForYou".Localized(
                y
            ));

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.CoatsOnHerEntrance".Localized());

            if (Player.CockArea(x) < 80)
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.SoakedWithLube".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.RearingBackSlamGroin".Localized(
                y
            ));

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.PicksUpPaceSodden".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.TwistAroundAllFours".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.PerfectHoleForYou".Localized(
                y
            ));

            if (Player.CumQ() <= 250)
                OutputText("Scenes.Isabellafollowerscene.FuckIsabella.UntilUtterlySpent".Localized());
            else
                if (Player.CumQ() <= 1000)
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.CumSpurtsOut".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.UntilBellyBloatsCream".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.SentencePeriodParagraph".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.FinalSwatCollapses".Localized());

            if (Player.Cocks[x].Length <= SceneInstance<IsabellaScene>.Get().TinyPenis())
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.PrefersSmallAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.LoveTheTinyOnes".Localized());
            }
            else
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.WrongAboutBigAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.FuckIsabella.WrongAboutBigPlain".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabella.RubsHairAndDress".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            IsabellaKnockUpAttempt();

            Player.RefillHunger(20);

            ReturnToCamp(1);
        }

        public void FindIzzyMilking()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.FindIzzyMilking.WanderBackToBarn".Localized());

            ClearButtons();

            CoC.Instance.Flags.Set(GameFlagIds.FoundIsabellaAtFarmToday, 1);

            AddButton(0, "Common.Button.FuckHer".Localized(), FuckIsabellaInTheBarn);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void FuckIsabellaInTheBarn()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.SlipIntoBarnStall".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.SidleUpBehind".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.CriesAlarmLeanBack".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.ParagraphBreak".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.VhatYouThinkAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.ThinkDoingSneaking".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.TrappedBetweenCheeks".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.HelpMitMilkersAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.MmmHelpWithMilking".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.ReachAroundTubes".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.Es".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.PushingBetweenCheeks".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.BaskingAffectionsTurnsCatches".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.StrandsOfSaliva".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.EachMovementCheeksClench".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.BreakLongKissNuzzling".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.ThroatyMooUrges".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.VhantItAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.GiveToMePlain".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.LegsSpurYouOn".Localized());

            OutputText("Scenes.Isabellafollowerscene.FuckIsabellaInTheBarn.FingersDiveInEasy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IsabellaBarnFuckPartII);
        }

        public void IsabellaBarnFuckPartII()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.CollapsesShudderingTwitching".Localized());

            Player.RefillHunger(25);

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.DrainMouthfuls".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.GroansSingleLanguidMovement".Localized());

            if (Player.Cocks[Player.SmallestCockIndex()].Length <= SceneInstance<IsabellaScene>.Get().TinyPenis())
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.SlideUntilGroinsJoin".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.ZuchNiceFitAccent".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.SighsVaginalMusclesCaressing".Localized());
            }
            else
                if (Player.SmallestCockArea() < 40)
                {
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.CockEntersStretchingWalls".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.AchSoBig".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.ItsSoBig".Localized());

                    OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.GruntsCuntWorkingHard".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.BitesLipAtPrick".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.SehrTooBigAccent".Localized());
                    else
                        OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.TooHugeTearingApart".Localized());
                }

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.FarInRelaxing".Localized());

            if (Player.SmallestCockArea() <= 164)
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.UpToHiltWalls".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.UntilBattersWombGlobs".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.ArmsAroundShoulders".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.BreaksKissGraspBreast".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.DoEetHarderAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.FuckMeHarderAnimal".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.AhhhCummingShrieksPleasure".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.FaceOnChestGasps".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.ZhatWasAhAccent".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.ThatWasIncredible".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.GaspsBetweenRaggedBreaths".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.KnowHowToPlease".Localized());
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.ShowGirlGoodTime".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.SayEnd".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.RestOnMilkBosom".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaBarnFuckPartII.WhitneyOpensDoor".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -3f));

            Player.ChangeFatigue(-25);

            IsabellaKnockUpAttempt();

            ReturnToCamp(2);
        }

        public void IsabellaTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.IsabellaTalkMenu.WhatToDiscuss".Localized());

            if (Debug)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaTalkMenu.DebugLabel".Localized());

                if (IsabellaScene.isabellaOffspringData.Count > 0)
                {
                    var i = 0;

                    while (i < IsabellaScene.isabellaOffspringData.Count)
                    {
                        OutputText("Scenes.Isabellafollowerscene.IsabellaTalkMenu.TypeAgeDay".Localized(
                            IsabellaScene.isabellaOffspringData[i],

                            View.Time.Days - IsabellaScene.isabellaOffspringData[i + 1]
                        ));

                        i += 2;
                    }
                }
            }

            ClearButtons();

            AddButton(0, "Common.Button.JustTalk".Localized(), SceneInstance<IsabellaScene>.Get().TalkWithIsabella);

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaCountdownToContraceptions) == -1)
                AddButton(1, "Common.Button.Contraception".Localized(), ToggleIsabellaContraceptives);

            AddButton(14, "Common.Button.Back".Localized(), CallForFollowerIsabella);
        }

        public void IsabellaTalksAboutPotentialPregnancy()
        {

            RegisterMacros();

            OutputText("Scenes.Isabellafollowerscene.IsabellaTalksAboutPotentialPregnancy.HeadForNoticeIsabella".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaTalksAboutPotentialPregnancy.HopedToSeeYou".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaTalksAboutPotentialPregnancy.AskWhySurprised".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaTalksAboutPotentialPregnancy.BeatsHastyRetreat".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaTalksAboutPotentialPregnancy.TalkContraceptives".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaCountdownToContraceptions, -1);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaPotencyState, -1);
        }

        public void ToggleIsabellaContraceptives()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaPotencyState) == 1)
            {
                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.TellBestStartContraceptives".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.NodsReasonablyJaHard".Localized());

                if (SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren() > 0)
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.OnesToLookAfter".Localized(
                        NumberUtils.Num2Text(SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren())
                    ));

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.SayEnd".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.ThankUnderstandingHeadBack".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.NoLongerPregnant".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaPotencyState, -1);
            }
            else
            {
                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.TellStillWantsFamily".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.StillWantYou".Localized(
                    Player.ClothedOrNaked(" and staining your clothes")
                ));

                if (SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren() > 0)
                    OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.SmallNotEnough".Localized(
                        NumberUtils.Num2Text(SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren())
                    ));

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.EnjoyHugWriggleFree".Localized());

                OutputText("Scenes.Isabellafollowerscene.ToggleIsabellaContraceptives.CanNowGetPregnant".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IsabellaPotencyState, 1);
            }

            IsabellaTalkMenu();
        }

        public void IsabellaCowifyPrompt()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaCowmother) == 0)
            {
                OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.ChatNoticeSadExpression".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.SoundsLikeAWorry".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.FaceMorphsPleasedNervous".Localized());

                if (Player.HasKeyItem("Marae's Lethicite"))
                    OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.RecognizeShardOfLethicite".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.AskWhatCrystalIs".Localized());

                OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.LooksNervousUncertainBest".Localized());
            }
            else
                OutputText("Scenes.Isabellafollowerscene.IsabellaCowifyPrompt.WonderShouldTellUse".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), YesToIsabellaTF);

            AddButton(1, "Common.Button.No".Localized(), NoToIsabellaTF);
        }

        public void YesToIsabellaTF()
        {
            ClearOutput();

            OutputText("Scenes.Isabellafollowerscene.YesToIsabellaTF.AfterMuchThought".Localized());

            OutputText("Scenes.Isabellafollowerscene.YesToIsabellaTF.AdmitsTakesShard".Localized());

            OutputText("Scenes.Isabellafollowerscene.YesToIsabellaTF.DeclaresCrookedSmile".Localized());

            OutputText("Scenes.Isabellafollowerscene.YesToIsabellaTF.AgreeSuggestLieDown".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaCowmother, 1);

            Go<CampScene>();
        }

        public void NoToIsabellaTF()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaCowmother) == 0)
                OutputText("Scenes.Isabellafollowerscene.NoToIsabellaTF.StayWaySheIs".Localized());
            else
            {
                OutputText("Scenes.Isabellafollowerscene.NoToIsabellaTF.LeaveHerAsIs".Localized());

                IsabellaTalkMenu();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaCowmother, 0.5f);

            Go<CampScene>();
        }

        public void IsabellaTakesOviElixir()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir));

            if (Save.GetInt("oviElixirAmount", 0) == 0)
                OutputText("Scenes.Isabellafollowerscene.IsabellaTakesOviElixir.OfferBottleQuickenDevelopment".Localized());
            else
                if (Save.GetInt("oviElixirAmount", 0) == 1)
                    OutputText("Scenes.Isabellafollowerscene.IsabellaTakesOviElixir.OfferAnotherBottle".Localized());
                else
                    OutputText("Scenes.Isabellafollowerscene.IsabellaTakesOviElixir.TakeBottleHand".Localized());

            if (Player.Pregnancy.Incubation > 240)
                SceneInstance<IsabellaScene>.Get().Pregnancy.KnockUpForce(SceneInstance<IsabellaScene>.Get().Pregnancy.Type, Player.Pregnancy.Incubation - 240);
            else
                SceneInstance<IsabellaScene>.Get().Pregnancy.KnockUpForce(SceneInstance<IsabellaScene>.Get().Pregnancy.Type, 1);

            Save.SetInt("oviElixirAmount", Save.GetInt("oviElixirAmount", 0) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaPregnancyBoosted, 1);

            Go<CampScene>();
        }

        public string BabyMF(string male, string female, int gender)
        {

            if (gender == (int)Gender.Male)
                return male;
            else
                return female;
        }

        public void IsabellaGivesBirth()
        {

            RegisterMacros();

            var babyGender = 0;

            if (SceneInstance<IsabellaScene>.Get().Pregnancy.AllowHerm && Random.Shared.Next(10) < 1)
                babyGender = (int)Gender.Hermaphrodite;
            else
                babyGender = 1 + Random.Shared.Next(2);

            OutputText("Scenes.Isabellafollowerscene.IsabellaGivesBirth.LoudLowingSound".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaGivesBirth.ThereAsExpectedBoulder".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaGivesBirth.FinallyFinalBellowNewborn".Localized());

            OutputText("Scenes.Isabellafollowerscene.IsabellaGivesBirth.WrigglingInfantPerfectlyFormed".Localized(
                babyGender == 1 ? "boy" : (babyGender == 2 ? "Common.Text.Girl".Localized() : "Common.Text.Hermaphrodite".Localized()),

                BabyMF("He", "She", babyGender),

                BabyMF("his", "her", babyGender),

                BabyMF("his", "her", babyGender),

                BabyMF("him", "her", babyGender)
            ));

            OutputText("Scenes.Isabellafollowerscene.IsabellaGivesBirth.TakesSmileMotherlyDelight".Localized(
                BabyMF("him", "her", babyGender),

                BabyMF("son", "daughter", babyGender),

                BabyMF("he", "she", babyGender),

                BabyMF("he", "she", babyGender),

                BabyMF("he", "she", babyGender),

                BabyMF("son", "daughter", babyGender)
            ));

            OutputText("Scenes.Isabellafollowerscene.IsabellaGivesBirth.FetchBlanketWrapWarm".Localized(
                BabyMF("son", "daughter", babyGender)
            ));

            IsabellaScene.isabellaOffspringData.Add(babyGender);

            IsabellaScene.isabellaOffspringData.Add(View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaPregnancyBoosted, 0);

            SceneInstance<IsabellaScene>.Get().Pregnancy.KnockUpForce();
        }

    }
}