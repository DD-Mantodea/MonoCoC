using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register<AlrauneScene>("coc:alraune")]
    public class AlrauneScene : Scene
    {

        public bool metAlraune = false;

        public bool eatenAss = false;

        public int questAsked = 0;

        public int vineTimer = 0;

        public int alrauneKilled = 0;

        public bool vinesTaken = false;

        public const int ASKEDAL = 1 << 0;

        public const int ASKEDAM = 1 << 1;

        public const int ASKEDAR = 1 << 2;

        public const int ASKEDJO = 1 << 3;

        public const int ASKEDKH = 1 << 4;

        public const int ASKEDKT = 1 << 5;

        public const int ASKEDRA = 1 << 6;

        public const int ASKEDSH = 1 << 7;

        public const int ASKEDHO = 1 << 8;

        public const int RATCLUE = 1 << 9;

        public static SaveContent Save => SaveContent(SceneIds.Swamp.Alraune);

        public override void Entry()
        {
            AlrauneEncounter();
        }

        public bool AskedShouldra => (Save.GetInt("questAsked", 0) & ASKEDSH) != 0;

        public int AskedTotal()
        {

            var total = NumberUtils.CountSetBits(Save.GetInt("questAsked", 0));

            if ((Save.GetInt("questAsked", 0) & RATCLUE) != 0)
                total--;

            return total;
        }

        public void AlrauneMeet()
        {

            OutputText("Scenes.Alraunescene.AlrauneMeet.PleasantSmellCatches".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.FeraReleased) > 1 ? "You're certainly no stranger to alluring flora, or the dangers they pose. " : ""));

            OutputText("Scenes.Alraunescene.AlrauneMeet.BeyondShrubbery".Localized());

            OutputText("Scenes.Alraunescene.AlrauneMeet.PetalsQuake".Localized());

            OutputText("Scenes.Alraunescene.AlrauneMeet.DoNotBeAfraid".Localized());

            Save.SetBool("metAlraune", true);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryAlraune);

            ClearButtons();

            AddNextButton("Common.Button.Talk".Localized(), AlrauneMeetTalk);

            AddNextButton("Common.Button.Fight".Localized(), AlrauneFight);

            AddButton(14, "Common.Button.Leave".Localized(), () => AlrauneMeetLeave());
        }

        public void AlrauneMeetTalk()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneMeetTalk.IfSheDoesntWant".Localized());

            OutputText("Scenes.Alraunescene.AlrauneMeetTalk.BrushingHairFromFace".Localized());

            OutputText("Scenes.Alraunescene.AlrauneMeetTalk.WithASighDrops".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fight".Localized(), AlrauneFight);

            AddNextButton("Common.Button.Flirt".Localized(), () => AlrauneFlirt(true));
        }

        public void AlrauneMeetLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneMeetLeave.SixthSense".Localized());

            OutputText("Scenes.Alraunescene.AlrauneMeetLeave.PlantWomanCrossesArms".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Talk".Localized(), AlrauneMeetTalk);

            AddNextButton("Common.Button.Fight".Localized(), AlrauneFight);

            AddButton(14, "Common.Button.Leave".Localized(), null);
        }

        public void AlrauneEncounter()
        {
            ClearOutput();

            if (!Save.GetBool("metAlraune"))
            {
                AlrauneMeet();

                return ;
            }

            OutputText("Scenes.Alraunescene.AlrauneEncounter.SweetFamiliarSmell".Localized());

            OutputText("Scenes.Alraunescene.AlrauneEncounter.BlackVelvetRisesUp".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Talk".Localized(), AlrauneTalk);

            AddNextButton("Common.Button.Fight".Localized(), AlrauneFight);

            if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !((Save.GetInt("questAsked", 0) & ASKEDAL) != 0))
                AddNextButton("Common.Button.Vines".Localized(), AskAlraune);
        }

        public void AlrauneTalk()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneTalk.IsntYourFirstRun".Localized());

            OutputText("Scenes.Alraunescene.AlrauneTalk.IsThatSo".Localized());

            ClearButtons();

            if (!Save.GetBool("vinesTaken") && Player.Armor.Id != ArmorIds.VineArmorBehavior)
                AddNextButton("Common.Button.ABackRub".Localized(), AlrauneBackrub);

            AddNextButton("Common.Button.AFight".Localized(), AlrauneFight);

            AddNextButton("Common.Button.Flirting".Localized(), () => AlrauneFlirt());
        }

        public void AlrauneFlirt(bool first = false)
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFlirt.GothicBeauty".Localized(
                first ? "Let's not get hasty now, you aren't trying to run. Perhaps, even, you'd rather get closer" : "Some relief and relaxation seems ideal for her, and you aren't one the shy away from the thought"));

            OutputText("Scenes.Alraunescene.AlrauneFlirt.MusesObsidianAppendage".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirt.StumbleAndFall".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirt.SubstantiallyMoreWary".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirt.ChucklesToHerself".Localized());

            if (Player.Lust < 33)
                Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            ClearButtons();

            AddNextButton("Common.Button.Sex".Localized(), AlrauneFlirtSex);

            AddNextButton("Common.Button.Cunnilingus".Localized(), AlrauneCunnilingus);

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonRimming".Localized(), AlrauneRimming);
        }

        public void AlrauneFlirtSex()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex.ComesNaturally".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex.ShruggingAtYourPerhaps".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex.ApprehensivelyComply".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex.AsksLiftingHand".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex.HandsSlideAcross".Localized());

            DoNext(() => AlrauneFlirtSex2());
        }

        public void AlrauneFlirtSex2()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.DespiteFightOrFlight".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.PlantGirlBreaksKiss".Localized(
                 Player.CockThatFits(3, "width") < 0 ? "The squirming of her thighs around your immense member" + (Player.Cocks.Count > 1 ? "s" : "") + " suddenly feel considerably more stimulating. Warm nectar drools from her depths, leaving a slick trail as she moves her pelvis back and forth across your shaft. You begin to twitch and shudder, far too acutely aware of the sensations now. Is it the kiss, you wonder? You curse your lack of a tool compact enough to thrust inside her." : "She backs up her hips and angles the tip of your " + Player.CockDescript(Player.CockThatFits(3, "width")) + " against her pussy, sending shivers up your spine. A satisfied sigh escapes her as she sinks down, engulfing your member. Compulsively, you shudder and feel a desire to ejaculate, twitching under the stimulation. Is it the kiss, you wonder? Drug-laced lips aside, her velvet walls could bring any man to the brink."
            ));

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.AlrauneWhispers".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.IncapableOfDenying".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.GigglesAndSighs".Localized(
                Player.Body.Get<AnalPart>().Looseness > 0 ? "] A dark tendril prods at your backside, causing you to jump. The vine wriggles against your meager resistance before plunging forth. [say:" : ""));

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.NectarDripsDown".Localized(
                Player.Body.Get<AnalPart>().Looseness > 0 ? "The thick, black tentacle pushing in and out of your [asshole] presses deliberately against your prostate, urging you to burst. " : "",

                Player.CockThatFits(3, "width") < 0 ? "spraying everything your body has upon the walls of her flower" : "filling her insides with drop after drop"));

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex2.PlantGirlMovesHips".Localized(
                Player.CockThatFits(3, "width") < 0 ? "The pressure exerted from her thighs" : "The slimy, semen-soaked passage of her hot depths"));

            Player.Orgasm("Dick");

            DoNext(() => AlrauneFlirtSex3());
        }

        public void AlrauneFlirtSex3()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex3.BlinkRecovering".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex3.SaysSmilingAtYou".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFlirtSex3.GroggilyObey".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AlrauneCunnilingus()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus.CantHelpBut".Localized());

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus.LeanInCloser".Localized());

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus.FaceMereInches".Localized());

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus.GetStartedWithVines".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Next".Localized(), AlrauneCunnilingus2);

            AddNextButton("Common.Button.NotPrepared".Localized(), AlrauneCunnilingusLeave);
        }

        public void AlrauneCunnilingus2()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus2.MomentsAfterStops".Localized(
                Player.BiggestBreastSize() == 0 ? "wrapped around where your breasts would be if you had them, squeezing at your body" : "wrapped around your [breasts], tightly squeezing them"));

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus2.NowEscalatedThings".Localized());

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus2.VinesReachVulva".Localized(
                Player.Vaginas.Count > 0 && Player.Vaginas[0].Wetness > 0 ? "thoroughly lubricated" : "dry and painfully sensitive"));

            Player.CuntChange(5, true, true);

            DoNext(() => AlrauneCunnilingus3());
        }

        public void AlrauneCunnilingus3()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus3.RemoveLipsFrom".Localized(
                Player.HasLongTongue() ? "penetrate her deeply with your excessively long tongue, causing her to let out a surprised gasp of pleasure as you deliberately brush your tongue ever so slightly against her cervix." : "press your [tongue] into her as far as you can manage."));

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus3.SurprisinglyLittleTime".Localized());

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus3.HereForAWhile".Localized());

            OutputText("Scenes.Alraunescene.AlrauneCunnilingus3.LayingOnHerFlower".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AlrauneCunnilingusLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneCunnilingusLeave.VinesNotSureWhat".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AlrauneRimming()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneRimming.AfterMomentOfThought".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming.NodInAgreement".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming.PlantYourFirmly".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming.SlightlyIncreasedForce".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming.ProbablyWhatSheSaid".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Next".Localized(), AlrauneRimming2);

            AddNextButton("Common.Button.NotPrepared".Localized(), AlrauneRimmingLeave);
        }

        public void AlrauneRimming2()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneRimming2.FewMomentsAfterStarts".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming2.BeforeSheFinishesSpeaking".Localized(
                Player.HasLongTongue() ? "deep" : "",

                Player.HasLongTongue() ? "even" : "",

                Player.HasLongTongue() ? "gently thrusting it back and forth inside of her while" : "",

                !Save.GetBool("eatenAss") ? "is... surprisingly pleasant, in a strange way." : "is just as oddly pleasant as you'd expect from a creature like her."));

            OutputText("Scenes.Alraunescene.AlrauneRimming2.IncreasedEnthusiasm".Localized(
                Player.HasLongTongue() ? "tonguefuck" : "kiss at"));

            DoNext(() => AlrauneRimming3());
        }

        public void AlrauneRimming3()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneRimming3.VinesPickUpSpeed".Localized(
                Player.HasLongTongue()

                    ? "thrust your tongue as deep into her as you can go, eventually hitting a dead end, which causes her to let out a surprised and blissful-sounding gasp. [say: Well well, I didn't expect you to reach that far. Interesting organ you pack in that mouth of yours, plaything. Now keep going.] Obeying her command, you thrust back and forth into her noticeably tight ass at an increased pace, focusing on forcefully bottoming out. Each time you hit her limit, she squirms in delight and delivers a stinging lash to your rear. After a bit of going as deep as is physically possible, the entirety of her rear hole is coated in your saliva, letting you tonguefuck her faster and faster."

                    : "use your hands to squeeze her ass and flick your tongue inside her in every direction, twirling it and spinning it to massage the inside of her pleasant-tasting hole. Hearing her let out the occasional moan, you decide to change it up a bit. Though you may not be able to reach very far, you dexterously and skillfully pleasure her as you thrust back and forth as deep as you can go, your saliva making it gradually easier and easier to increase your pace."));

            OutputText("Scenes.Alraunescene.AlrauneRimming3.LessTimeThanExpected".Localized(
                Player.HasLongTongue() ? "the entire length of" : ""));

            OutputText("Scenes.Alraunescene.AlrauneRimming3.NotOneAndDone".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming3.OnAndOnBoth".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming3.GivesAChuckle".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimming3.AgreeingWithPaleBeauty".Localized());

            Player.Orgasm("All");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AlrauneRimmingLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneRimmingLeave.DontKnowWhatIn".Localized());

            OutputText("Scenes.Alraunescene.AlrauneRimmingLeave.MouthCoveredInThick".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AlrauneBackrub()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneBackrub.BackRubOfCourse".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrub.GivesFunnyLook".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrub.AcrossGroundToHer".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrub.ShouldntBeThere".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrub.TurnsHeadConcerned".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrub.FlutteringYourEyes".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrub.SeeSomethingInside".Localized());

            ClearButtons();

            AddNextButton("Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), AlrauneBackrubTake);

            AddNextButton("Common.Button.Run".Localized(), AlrauneBackrubRun);
        }

        public void AlrauneBackrubTake()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneBackrubTake.ReachIntoShadows".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrubTake.ShuddersAndMoans".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrubTake.PanicSetsIn".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrubTake.GothicWomanSmirking".Localized());

            OutputText("Scenes.Alraunescene.AlrauneBackrubTake.BeforeYouCanStomp".Localized());

            if (Player.Body.Get<SkinPart>().Id == BodyPartTypes.Goo)
            {
                OutputText("Scenes.Alraunescene.AlrauneBackrubTake.AsTheySetConstriction".Localized());

                OutputText("Scenes.Alraunescene.AlrauneBackrubTake.StandInShock".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Alraunescene.AlrauneBackrubTake.AsTheySetIn".Localized(
                    Player.UpperGarment.Id == UndergarmentIds.SeaShirt ? ", only to find it tearing its way through your undershirt" : ""));

                OutputText("Scenes.Alraunescene.AlrauneBackrubTake.BurningEnds".Localized());

                OutputText("Scenes.Alraunescene.AlrauneBackrubTake.ObtainedObsidianVines".Localized());

                GetVines();
            }
        }

        public void GetVines(Action nextFunc = null)
        {

            if (nextFunc == null)
                nextFunc = () => ReturnToCamp(1);

            Save.SetBool("vinesTaken", true);

            if (Player.UpperGarment.Id == UndergarmentIds.SeaShirt)
                Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);

            var items = new List<Item> { 
            Player.SetArmor((Armor)ItemRegistry.Create(ArmorIds.VineArmorBehavior)),

            Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeLowerwear),

            Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear),
         };

            items = items.Where(it => it != null).ToList();

            if (items.Count > 0)

                InventoryScene.TakeItems(items.ToArray(), nextFunc);
            else
                nextFunc();
        }

        public void AlrauneBackrubRun()
        {

            OutputText("Scenes.Alraunescene.AlrauneBackrubRun.OminousDarknessDrawn".Localized());

            DoNext(() => AlrauneFight());
        }

        public void AlrauneFight()
        {

            var monster = EnemyRegistry.Create(EnemyIds.Alraune);

            monster.OnDefeated = _ => AlrauneDefeated(monster is AlrauneEnemy alraune && alraune.Rooted);

            monster.OnWon = _ => AlrauneWon();

            CombatScene.Start(monster, () => Go<CampScene>(), canFlee: false);
        }

        internal static void AlrauneDefeatedHook(bool rooted) => new AlrauneScene().AlrauneDefeated(rooted);

        internal static void AlrauneWonHook() => new AlrauneScene().AlrauneWon();

        public void AlrauneWon()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneWon.SaysLookingDown".Localized(
                silly && Player.Lust >= Player.MaxLust

                    ? " [say: How ironic, too, that you lost because you were 'battle-hardened'.][pg]And with that, you kind of welcome death. "

                    : "[pg]"));

            OutputText("Scenes.Alraunescene.AlrauneWon.WrapsVinesAround".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWon.UnderstandableAsThatMay".Localized());

            if (Random.Shared.Next(100) == 0)
            {
                OutputText("Scenes.Alraunescene.AlrauneWon.ClosesShadowyEyes".Localized());

                CombatScene.CleanupAfterCombat();

                return ;
            }

            OutputText("Scenes.Alraunescene.AlrauneWon.SaysWithMoreThan".Localized());

            ClearButtons();

            AddNextButton("Common.Button.KeepTalking".Localized(), AlrauneWonTalking);

            AddNextButton("Common.Button.Sex".Localized(), AlrauneWonSex);
        }

        public void AlrauneWonTalking()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneWonTalking.IfAnybodyKnowsSharpness".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonTalking.PalePlantChuckles".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonTalking.ReallyTeenageGirl".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonTalking.TakeBackWhatSaid".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonTalking.SomethingTellsYou".Localized());

            DoNext(() => AlrauneWonSex());
        }

        public void AlrauneWonSex()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneWonSex.GrabsYourShoulders".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonSex.UpsideDownPerspective".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonSex.LeansOverTorso".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonSex.SoonFollowedOpenAir".Localized());

            OutputText("Scenes.Alraunescene.AlrauneWonSex.KeptPartiallyUpsideDown".Localized(
                Player.LongestCockLength() < 7 ? "completely" : "as much as possible",

                Player.LongestCockLength() > 5 ? "throat and tongue are marvelous, both writhing against" : "tongue is marvelous, wrapping around",

                CoC.Instance.Flags.Get(GameFlagIds.FeraReleased) > 1 ? " Would this make Fera jealous?" : ""));

            Player.Orgasm(Player.HasCock() ? "Dick" : (Player.HasVagina() ? "Vaginal" : "Anal"));

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void AlrauneWonWake()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneWonWake.WakeGlancingDeliriously".Localized());

            ReturnToCamp(1);
        }

        public static bool LustVictory() =>

            CombatController.Instance?.LustVictory() ?? CombatScene.LastOutcome == CombatOutcome.LustVictory;

        public void AlrauneDefeated(bool rooted)
        {
            ClearOutput();

            if (rooted)
            {
                if (LustVictory())
                    OutputText("Scenes.Alraunescene.AlrauneDefeated.UnableToBearDaunting".Localized());
                else
                    OutputText("Scenes.Alraunescene.AlrauneDefeated.WiltingAtOppressiveMight".Localized());
            }
            else
                OutputText("Scenes.Alraunescene.AlrauneDefeated.BroughtToTheBrink".Localized());

            ClearButtons();

            RegisterMacro("rooted", rooted);

            AddButton(0, "Common.Button.Fuck".Localized(), () => AlrauneFuck(rooted));

            AddButton(1, "Common.Button.VineAction".Localized(), () => AlrauneVines(rooted));

            AddButton(2, "Common.Button.DeepVine".Localized(), () => AlrauneCervicalPenetration(rooted));

            AddButton(3, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), AlrauneOral);

            var sillyAural = silly && Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox;

            if (sillyAural || (OptionStore.Instance.Get("gore", 0) > 0))
            {
                var aural = AddButton(4,"Common.Button.Aural".Localized(), AlrauneAural);

                if (!sillyAural && !HasEarfuckingTail())
                    _=Player.HasCock() && (Player.Cocks[Player.ThinnestCock()]?.Thickness ?? 0.0f) > 2;            }

            if (Player.HasMultiTails())
                AddButton(5, "Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(10, "Common.Button.Kill".Localized(), AlrauneKill);

            if (Player.Armor.Id != ArmorIds.VineArmorBehavior)
                AddButton(11, "Common.Button.Insides".Localized(), AlrauneInsides);

            AddButton(14, "Common.Button.Leave".Localized(), () => CombatScene.CleanupAfterCombat());
        }

        public void AlrauneCervicalPenetration(bool rooted)
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.ParagraphBreak".Localized());

            if (rooted)
            {
                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.ApproachingSinkingWoman".Localized(
                    LustVictory() ? "In her sexually overwhelmed" : "Despite her worn and battered",

                    LustVictory() ? " Flushed with arousal" : " Beaten into submission"));

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.TwigsAndLeavesRustle".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.BlackVelvetSays".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.NeedMoreThanThat".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.AnotherVineSpiraling".Localized());

                Player.CuntChange(2.5f, false, true);

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.WithAddedGirth".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.PressesHerAssault".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.ManyMinutesPass".Localized());
            }
            else
            {
                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.StrideTowardCrawling".Localized(
                    LustVictory() ? " Flushed with arousal" : " Beaten into submission"));

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.TwigsRustleAgain".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.BlackVelvetSaysAgain".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.NeedMoreAgain".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.AnotherVineSpiralingAgain".Localized());

                Player.CuntChange(2.5f, false, true);

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.WithAddedGirthAgain".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.PressesHerAssaultAgain".Localized());

                OutputText("Scenes.Alraunescene.AlrauneCervicalPenetration.ManyMinutesPassAgain".Localized());
            }

            Player.ChangeFatigue(20);

            CombatScene.CleanupAfterCombat();
        }

        public void AlrauneOral()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneOral.WeakenedGirlWatches".Localized(
                LustVictory() ? "yearning" : "tired"));

            OutputText("Scenes.Alraunescene.AlrauneOral.LabiaSmushedAgainst".Localized());

            OutputText("Scenes.Alraunescene.AlrauneOral.AsIfNotObvious".Localized());

            OutputText("Scenes.Alraunescene.AlrauneOral.VinesCrawlUpYour".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AlrauneFuck(bool rooted)
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFuck.TakingFirmHold".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck.MumblesFinallyObeying".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck.SlightBlushShows".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck.ThatllDo".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck.BringYourselfDown".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Next".Localized(), AlrauneFuck2);

            AddNextButton("Common.Button.Leave".Localized(), AlrauneFuckLeave);
        }

        public void AlrauneFuckLeave()
        {

            OutputText("Scenes.Alraunescene.AlrauneFuckLeave.PonderingThatPossibility".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuckLeave.AlrauneCriesHome".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Okay".Localized(), AlrauneFuck2);

            AddNextButton("Common.Button.Leave".Localized(), AlrauneFuckLeave2);
        }

        public void AlrauneFuckLeave2()
        {

            OutputText("Scenes.Alraunescene.AlrauneFuckLeave2.MockHerAndBid".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AlrauneFuck2()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFuck2.BrushOffTheThought".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck2.MovingAlongWield".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck2.YellsEntirelyBroken".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck2.SingleHeavyThrust".Localized(
                Player.ThickestCockThickness() > 3 ? "groan through grit teeth" : "moan",

                CoC.Instance.Flags.Get(GameFlagIds.MetMarae) > 0 ? "Marae's" : "the gods'",

                Player.ThickestCockThickness() > 3 ? "The squelching sound of tearing plant fibers causes momentary concern, but her rolling eyes suggest she is in some sort of heaven. " : ""));

            OutputText("Scenes.Alraunescene.AlrauneFuck2.PullBackAndBuck".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck2.HowRudeThisIs".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck2.PumpYourHipsMore".Localized());

            OutputText("Scenes.Alraunescene.AlrauneFuck2.ReleasingTheVines".Localized());

            Player.Orgasm("Dick");

            DoNext(() => AlrauneFuck3());
        }

        public void AlrauneFuck3()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneFuck3.TakesQuiteADegree".Localized());

            CombatScene.CleanupAfterCombat();

            if (Player.Armor.Id == ArmorIds.GooArmor)
            {
                OutputText("Scenes.Alraunescene.AlrauneFuck3.ArmorDistendsForward".Localized());

                OutputText("Scenes.Alraunescene.AlrauneFuck3.AlternativeIsSticky".Localized(
                    Player.ThickestCockThickness() > 3 ? " and sap" : ""));

                OutputText("Scenes.Alraunescene.AlrauneFuck3.SapphireSlimeEngulfs".Localized());

            }
        }

        public void AlrauneVines(bool rooted)
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneVines.ComeOutOnTop".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVines.UndeterredInhumanBack".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVines.ForcefullyShoveHerFace".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVines.SeparateOneVine".Localized());

            ClearButtons();

            AddNextButton("Common.Button.VineDildo".Localized(), AlrauneVinesDildo);

            AddNextButton("Common.Button.VineGrind".Localized(), AlrauneVinesGrind);
        }

        public void AlrauneVinesDildo()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneVinesDildo.ExtremelyLightWeight".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesDildo.TellRebelliousBitch".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesDildo.SoundsDownrightGiddy".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesDildo.HeatBuildingAsRubs".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesDildo.StandUpFromPosition".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void AlrauneVinesGrind()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneVinesGrind.ExtremelyLightWeight".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesGrind.TellRebelliousBitch".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesGrind.SoundsDownrightGiddy".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesGrind.HeatBuildingBetweenLegs".Localized());

            OutputText("Scenes.Alraunescene.AlrauneVinesGrind.StandUpFromPosition".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public bool HasEarfuckingTail()
        {

            return Player.HasPrehensileTail() && !Player.HasFurryTail() && (new[] { BodyPartTypes.Mouse, BodyPartTypes.Imp }!.Contains(Player.Body.Get<TailPart>().Id));
        }

        public void AlrauneAural()
        {
            ClearOutput();

            RegisterMacro("tailbrain", HasEarfuckingTail() || (Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox && silly));

            RegisterMacro("kitsunebrain", Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox && silly);

            bool humanCock = false;

            bool hasKnot = false;

            bool longCock = false;

            if (Player.HasCock())
            {
                var thinnestCock = Player.ThinnestCock();

                RegisterMacro("auralcock", Player.CockDescript(thinnestCock));

                RegisterMacro("auralcocktype", Player.CockMultiNoun(thinnestCock));

                CockPart auralCock = Player.Cocks[thinnestCock] ?? null;

                humanCock = auralCock?.Id == BodyPartTypes.Human;

                hasKnot = auralCock?.HasKnot ?? false;

                longCock = 7 <= (auralCock?.Length ?? 0.0f);
            }

            OutputText("Scenes.Alraunescene.AlrauneAural.GraspingSoftPale".Localized());

            OutputText("Scenes.Alraunescene.AlrauneAural.HuffsInResponse".Localized());

            OutputText("Scenes.Alraunescene.AlrauneAural.StrugglesToPieceWords".Localized(
                humanCock ? "[auralcock]" : "[auralcocktype]"));

            OutputText("Scenes.Alraunescene.AlrauneAural.MyPetunias".Localized(
                longCock ? "impacting the inside with" : "fully hilting"));

            if (silly && Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
            {
                OutputText("Scenes.Alraunescene.AlrauneAural.FurTouchesEveryNook".Localized());

                OutputText("Scenes.Alraunescene.AlrauneAural.BrokenSheWeeps".Localized());
            }
            else
                if (HasEarfuckingTail())
                {
                    OutputText("Scenes.Alraunescene.AlrauneAural.NumbedAsSheAppears".Localized());

                    OutputText("Scenes.Alraunescene.AlrauneAural.ShruggingNothingMore".Localized());
                }
                else
                {
                    OutputText("Scenes.Alraunescene.AlrauneAural.FreedomToSkullRape".Localized());

                    OutputText("Scenes.Alraunescene.AlrauneAural.LimpPlantWhoreStares".Localized(
                        hasKnot && longCock ? ", splintering open an even bigger hole as a result of your [knot]," : ""));

                    Player.Orgasm("Dick");
                }

            CombatScene.CleanupAfterCombat();
        }

        public void AlrauneInsides()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AlrauneInsides.NotingHoleWhereSpine".Localized());

            OutputText("Scenes.Alraunescene.AlrauneInsides.ExamineWhatYouveTaken".Localized());

            OutputText("Scenes.Alraunescene.AlrauneInsides.PanicSetsIn".Localized());

            if (Player.Body.Get<SkinPart>().Id == BodyPartTypes.Goo)
            {
                OutputText("Scenes.Alraunescene.AlrauneInsides.AsTheySetConstriction".Localized());

                OutputText("Scenes.Alraunescene.AlrauneInsides.StandInShock".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Alraunescene.AlrauneInsides.AsTheySetIn".Localized(
                    Player.UpperGarment.Id == UndergarmentIds.SeaShirt ? "true" : "false"));

                OutputText("Scenes.Alraunescene.AlrauneInsides.BurningEnds".Localized());

                OutputText("Scenes.Alraunescene.AlrauneInsides.ObtainedObsidianVines".Localized());

                GetVines(CombatScene.CleanupAfterCombat);
            }
        }

        public void AlrauneKill()
        {
            ClearOutput();

            if (Player.Weapon.Id == WeaponIds.DullScimitar)
                OutputText("Scenes.Alraunescene.AlrauneKill.ProductOf".Localized());
            else
                if (Player.Weapon.IsStaff())
                    OutputText("Scenes.Alraunescene.AlrauneKill.ChannelingArcaneAbilities".Localized());
                else
                    if (Player.Weapon.IsBlunt())
                        OutputText("Scenes.Alraunescene.AlrauneKill.SwingYourIntoHer".Localized());
                    else
                        if (Player.Weapon.IsAxe())
                        {
                            OutputText("Scenes.Alraunescene.AlrauneKill.RaiseYourAndAdjust".Localized());

                            Flags.Set(GameFlagIds.CampCabinWoodResources, Math.Min(
                                CampScene.MaxWoodSupply(),

                                Flags.Get(GameFlagIds.CampCabinWoodResources) + 1));
                        }
                        else
                            if (silly && Player.Weapon.IsKatana())
                            {
                                OutputText("Scenes.Alraunescene.AlrauneKill.SheathedBladeUponSubduing".Localized());

                                OutputText("Scenes.Alraunescene.AlrauneKill.OnYourHonor".Localized());

                                OutputText("Scenes.Alraunescene.AlrauneKill.InAFlash".Localized());

                                OutputText("Scenes.Alraunescene.AlrauneKill.FromEarthSheCame".Localized());

                                OutputText("Scenes.Alraunescene.AlrauneKill.SheatheYourSword".Localized());
                            }
                            else
                                if (Player.Weapon.IsBladed())
                                    OutputText("Scenes.Alraunescene.AlrauneKill.HoldingTipToHead".Localized());
                                else
                                    if (Player.Weapon.IsFirearm())
                                        OutputText("Scenes.Alraunescene.AlrauneKill.PressTheBarrel".Localized(
                                            Save.GetInt("alrauneKilled", 0) > 0 ? " Just in case, you take another shot, eviscerating the already-mutilated contents of her head." : ""));
                                    else
                                        if (Player.Weapon.IsWhip())
                                        {
                                            OutputText("Scenes.Alraunescene.AlrauneKill.TimeToFinishThis".Localized());

                                            OutputText("Scenes.Alraunescene.AlrauneKill.MoansErotically".Localized());

                                            OutputText("Scenes.Alraunescene.AlrauneKill.StrikeAgainEliciting".Localized());

                                            OutputText("Scenes.Alraunescene.AlrauneKill.BegsGodDamnIt".Localized());

                                            OutputText("Scenes.Alraunescene.AlrauneKill.SquelchingNoises".Localized());
                                        }
                                        else
                                            OutputText("Scenes.Alraunescene.AlrauneKill.LooseningYourJoints".Localized());

            Save.SetInt("alrauneKilled", Save.GetInt("alrauneKilled", 0) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void AskAlraune()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskAlraune.EscapeIsntNecessary".Localized(
                Player.DryadScore() > 2 ? "and floral-mutated " : ""));

            OutputText("Scenes.Alraunescene.AskAlraune.ForcedToFeedParasite".Localized());

            OutputText("Scenes.Alraunescene.AskAlraune.BecomeAKin".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDAL);

            ClearButtons();

            AddNextButton("Common.Button.RemoveVines".Localized(), () => AskAlrauneAnswer(true));

            AddNextButton("Common.Button.MoveOn".Localized(), () => AskAlrauneAnswer(false));
        }

        public void AskAlrauneAnswer(bool remove)
        {
            ClearOutput();

            if (remove)
            {
                OutputText("Scenes.Alraunescene.AskAlrauneAnswer.VinesExorcised".Localized());

                OutputText("Scenes.Alraunescene.AskAlrauneAnswer.SighingLeansForward".Localized());
            }
            else
                OutputText("Scenes.Alraunescene.AskAlrauneAnswer.ThatsAllYoullBe".Localized());

            DoNext(() => AlrauneFight());
        }

        public void AskAmily()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskAmily.AfterAllThatTime".Localized());

            OutputText("Scenes.Alraunescene.AskAmily.SqueaksRubbingVines".Localized());

            OutputText("Scenes.Alraunescene.AskAmily.AmilyFarFromConfident".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDAM);

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskAmily.IfOnlySomething".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AskArian()
        {
            ClearOutput();

            OutputText((silly

                ? "Scenes.Alraunescene.AskArian.WaltzingAroundTheLizan"

                : "Scenes.Alraunescene.AskArian.ConciseExplanation").Localized());

            OutputText("Scenes.Alraunescene.AskArian.TwiddlesThumbsNervously".Localized());

            OutputText("Scenes.Alraunescene.AskArian.TalentedDedicatedMystic".Localized());

            OutputText("Scenes.Alraunescene.AskArian.CouldEnchantYourTalisman".Localized());

            OutputText("Scenes.Alraunescene.AskArian.SelfImmolation".Localized());

            OutputText("Scenes.Alraunescene.AskArian.StammeringChangesMind".Localized());

            OutputText("Scenes.Alraunescene.AskArian.EffortRegardlessOfEfficacy".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDAR);

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskArian.IfOnlySomething".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AskJojo()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskJojo.PiousAndPureMonk".Localized());

            OutputText("Scenes.Alraunescene.AskJojo.LooksUpAndDown".Localized());

            OutputText("Scenes.Alraunescene.AskJojo.ComplyPresentingArm".Localized());

            OutputText("Scenes.Alraunescene.AskJojo.AdmitsSaddened".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDJO);

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskJojo.IfOnlySomething".Localized());

            ClearButtons();

            AddNextButton("Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), null );

            AddNextButton("Common.Button.Nah".Localized(), () => ReturnToCamp(1));
        }

        public void AskKiha()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskKiha.AfterSoMuchTime".Localized());

            OutputText("Scenes.Alraunescene.AskKiha.SheSaysFirmly".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDKH);

            ClearButtons();

            AddNextButton("Common.Button.Okay".Localized(), () => AskKihaAnswer(false));

            AddNextButton("Common.Button.No".Localized(), () => AskKihaAnswer(true));
        }

        public void AskKihaAnswer(bool noplease)
        {
            ClearOutput();

            if (noplease)
                OutputText("Scenes.Alraunescene.AskKihaAnswer.RaiseArmsDefensively".Localized());

            OutputText("Scenes.Alraunescene.AskKihaAnswer.InhalesDeeplyThen".Localized());

            OutputText("Scenes.Alraunescene.AskKihaAnswer.ExclaimsGenuineFear".Localized());

            OutputText("Scenes.Alraunescene.AskKihaAnswer.BlushesLooksAway".Localized());

            OutputText("Scenes.Alraunescene.AskKihaAnswer.FlameRetardantVines".Localized());

            OutputText("Scenes.Alraunescene.AskKihaAnswer.WhyYouNeedMe".Localized());

            OutputText("Scenes.Alraunescene.AskKihaAnswer.VeryLikelyExplanation".Localized());

            Player.ChangeFatigue(20);

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskKihaAnswer.IfOnlySomething".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AskKitsune()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskKitsune.TemptingThoughItMight".Localized());

            OutputText("Scenes.Alraunescene.AskKitsune.QuestionsBewildered".Localized());

            OutputText("Scenes.Alraunescene.AskKitsune.MedicalConjuresThoughts".Localized());

            OutputText("Scenes.Alraunescene.AskKitsune.PutsHandUpTo".Localized());

            OutputText("Scenes.Alraunescene.AskKitsune.ThinkMatterOf".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.MansionVisited) > 0 ? "She won't be able to take you to a mansion to get railed and milked for hours on end" : "She can't take you over to play with her and her sisters"));

            OutputText("Scenes.Alraunescene.AskKitsune.RemoveThemThen".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDKT);

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), AskKitsuneYes);

            AddNextButton("Common.Button.No".Localized(), AskKitsuneNo);
        }

        public void AskKitsuneYes()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskKitsuneYes.EitherItWorks".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneYes.KitsuneWalksUpClose".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneYes.NoFurtherFanfare".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneYes.SheYanksAgain".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneYes.YetAgainSheYanks".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneYes.SaysWipingBrow".Localized());

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskKitsuneYes.IfOnlySomething".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fine".Localized(), null);

            AddNextButton("Common.Button.No".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void AskKitsuneNo()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskKitsuneNo.SheHardlyKnows".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneNo.KitsuneSighs".Localized());

            OutputText("Scenes.Alraunescene.AskKitsuneNo.DiscoverYourself".Localized());

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskKitsuneNo.IfOnlySomething".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AskRathazul()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskRathazul.PresentVineWrappedArm".Localized());

            OutputText("Scenes.Alraunescene.AskRathazul.StaresForAMoment".Localized());

            OutputText("Scenes.Alraunescene.AskRathazul.ExplainingTheSituation".Localized(
                Player.Corruption >= 66 || Flags.Get(GameFlagIds.PcFetish) % 10 >= 1 ? " You'd very much prefer to prance nude of your own free will." : ""));

            OutputText("Scenes.Alraunescene.AskRathazul.OldRatHuffs".Localized());

            OutputText("Scenes.Alraunescene.AskRathazul.AfterMoreSilentReading".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDRA);

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskRathazul.IfOnlySomething".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AskShouldra()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskShouldra.WraithMaterializes".Localized());

            OutputText("Scenes.Alraunescene.AskShouldra.SavesOnTheExplanation".Localized());

            OutputText("Scenes.Alraunescene.AskShouldra.EyesGlimmeringWithGold".Localized());

            OutputText("Scenes.Alraunescene.AskShouldra.ActuallySoundLogic".Localized());

            OutputText("Scenes.Alraunescene.AskShouldra.YellsTheGhost".Localized());

            OutputText("Scenes.Alraunescene.AskShouldra.VinesFollowedSuit".Localized());

            OutputText("Scenes.Alraunescene.AskShouldra.BeforeYouCanReact".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDSH);

            if (AskedTotal() > 2)
                OutputText("Scenes.Alraunescene.AskShouldra.IfOnlySomething".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AskHolli()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskHolli.SpawnOfMarae".Localized());

            OutputText("Scenes.Alraunescene.AskHolli.DryadRunsHandThrough".Localized(
                 Player.Mf("handsome", "beautiful"),

                 Player.Mf("handsome", "beautiful")
            ));

            OutputText("Scenes.Alraunescene.AskHolli.FlatteringMaybe".Localized());

            OutputText("Scenes.Alraunescene.AskHolli.GroansAndWhines".Localized());

            DoNext(() => ReturnToCamp(1));

            ClearButtons();

            AddNextButton("Common.Button.Remove".Localized(), AskHolliRemove);

            AddNextButton("Scenes.CorruptedGlade.NeverMind".Localized(), AskHolliEh);
        }

        public void AskHolliRemove()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskHolliRemove.ArentGoingToBe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 1)
            {
                OutputText("Scenes.Alraunescene.AskHolliRemove.SheMopes".Localized());

                OutputText("Scenes.Alraunescene.AskHolliRemove.ShellHaveToWait".Localized());

                OutputText("Scenes.Alraunescene.AskHolliRemove.SaysSheepishly".Localized());

                OutputText("Scenes.Alraunescene.AskHolliRemove.VeryWellYouKnow".Localized());
            }
            else
            {
                OutputText("Scenes.Alraunescene.AskHolliRemove.NoFunAtAll".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.HolliFuckedToday) == 1 ? "At least you've done your due-diligence in pleasing me today, so I'm feeling in a giving mood." : "You haven't even done your due-diligence in keeping me fertilized. I expect better care to be given to the daughter of a goddess. See to it that you pleasure me far more frequently, and"));

                OutputText("Scenes.Alraunescene.AskHolliRemove.SuchAs".Localized());

                OutputText("Scenes.Alraunescene.AskHolliRemove.DryadGrinsAsBegins".Localized());

                OutputText("Scenes.Alraunescene.AskHolliRemove.QuiteAnAssortment".Localized());

                OutputText("Scenes.Alraunescene.AskHolliRemove.FoldsArmsAndSays".Localized());
            }

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | ASKEDHO);

            DoNext(() => ReturnToCamp(1));
        }

        public void AskHolliEh()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.AskHolliEh.GrinsShakingHer".Localized());

            OutputText("Scenes.Alraunescene.AskHolliEh.WinksAtYou".Localized());

        }

        public void HolliSolution()
        {
            ClearOutput();

            Player.SetArmor(Armor.Nothing);

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 1)
            {
                OutputText("Scenes.Alraunescene.HolliSolution.HaveTheCloth".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.SlightlySurprisedToSee".Localized());

                Player.ConsumeItem(ConsumableIds.SlimyClothoth, 1);

                ClearButtons();

                AddNextButton("Common.Button.DoIt".Localized(), HolliSolutionDo);

                AddNextButton("Common.Button.RubMe".Localized(), HolliSolutionRub);
            }
            else
            {
                OutputText("Scenes.Alraunescene.HolliSolution.CarryTheAssortment".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.SaysLookingDown".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.HesitateAWondering".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.SmirksBitingLip".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.PushAndSlideGooey".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.CompletelyAndTotallyNude".Localized());

                OutputText("Scenes.Alraunescene.HolliSolution.LaughingDryadTosses".Localized());

                Player.ConsumeItem(ConsumableIds.SlimyClothoth, 5);

                Player.ConsumeItem(ConsumableIds.SnakeOil, 5);

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney), 3);

                var milkNeeded = 3;

                while (milkNeeded > 0)
                {
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurMilk)))
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.MinotaurMilk));
                    else
                        if (Player.HasItem(ConsumableIds.IsabellaMilk))
                            Player.ConsumeItem(ConsumableIds.IsabellaMilk);
                        else
                            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));
                            else
                                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

                    milkNeeded--;
                }

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void HolliSolutionDo()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.HolliSolutionDo.LittleFanfareNeeded".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionDo.RubsBreastsWhileBiting".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionDo.LookingYourselfOver".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionDo.DryadShrugsAndSmiles".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionDo.SurprisinglySimpleSolution".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void HolliSolutionRub()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.HolliSolutionRub.WentThroughAllTrouble".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.HolliSmilesGladly".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.BeckonsYouToHer".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.HowWasThisMeant".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.TryToVoiceIssue".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.LookingYourselfOver".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.DryadShrugsAndSmiles".Localized());

            OutputText("Scenes.Alraunescene.HolliSolutionRub.SurprisinglySimpleSolution".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RatClue()
        {
            ClearOutput();

            OutputText("Scenes.Alraunescene.RatClue.StrollThroughCamp".Localized());

            OutputText("Scenes.Alraunescene.RatClue.ExplainsUsheringOver".Localized());

            OutputText("Scenes.Alraunescene.RatClue.SoundLogicEvidently".Localized());

            OutputText("Scenes.Alraunescene.RatClue.StatesFlipsThroughSome".Localized());

            OutputText("Scenes.Alraunescene.RatClue.WorstCaseScenario".Localized());

            Save.SetInt("questAsked", Save.GetInt("questAsked", 0) | RATCLUE);

            PlayerMenu();
        }

    }
}