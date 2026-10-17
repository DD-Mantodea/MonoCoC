using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Internals;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<IvorySuccubusScene>("coc:ivory_succubus_scene")]
    public class IvorySuccubusScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Forest.IvorySuccubus);

        public string debugName => "Ivory Succ";

        public string debugHint => "";

        public int ivoryBreasts = 2;

        public int ivoryHeight = 73;

        public static bool IsRetarded() =>

            Player.HasPerk(PerkIds.BroBrains)

            || Player.HasPerk(PerkIds.BimboBrains)

            || Player.HasPerk(PerkIds.FutaFaculties);

        public static string SexSwitch(string male, string female, string herm = "", string neuter = "")
        {
            if (herm == "")
                herm = male;

            if (neuter == "")
                neuter = herm;

            return Player.Gender switch
            {
                Gender.Genderless => neuter,

                Gender.Male => male,

                Gender.Female => female,

                Gender.Hermaphrodite => herm,

                _ => "<b>Gender error!</b>"
            };
        }

        public override void Entry()
        {
            IvorySetup();
        }

        public void Reset()
        {

            Save.SetBool("met", false);

            Save.SetInt("deadHookers", 0);

            Save.SetInt("timesLost", 0);
        }

        public void IvorySetup()
        {

            ivoryBreasts = Random.Shared.Next(5);

            ivoryHeight = Random.Shared.Next(8) + 70;

            CurrentMonster = EnemyRegistry.Create(new Identifier("coc", "ivory_succubus"));

            CurrentMonster!.Body.Get<BreastRowPart>().BreastRating = Random.Shared.Next(5);

            CurrentMonster.Tallness = ivoryHeight;
        }

        public void RegisterMacros()
        {

            RegisterMacro("ivorybreasts", () =>
            {
                return CurrentMonster!.BreastDescript(0);
            });

            RegisterMacro("ivoryheight", new[] { "[if (metric) {around 180 cm|just under six feet}]", "[if (metric) {around 180 cm|six feet}]", "[if (metric) {around 190 cm|just over 6 feet}]", "[if (metric) {nearly 2 meters|almost 6 and a half feet}]" }[(int)Math.Floor((CurrentMonster!.Tallness - 70) / 2)]);

            RegisterMacro("timeslost", Save.GetInt("timesLost", 0));
        }

        public void Encounter()
        {
            ClearOutput();

            IvorySetup();

            RegisterMacros();

            if (!Save.GetBool("met"))
            {
                OutputText("Scenes.Ivorysuccubusscene.Encounter.RemainingAttentive".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Encounter.StandingAtop".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Encounter.FeminineVoice".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 1 ? "Marae is " + (CoC.Instance.Flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0 ? "Scenes.Helfollower.HeliaPhoenixes.QueenDead".Localized() : "Common.Text.BlissfullyLostInHerself".Localized()) + ". Her roots no longer fight" : "all these many years of work have whittled away Marae's influence to but a whimper"));

                Player.UnlockCodexEntry(GameFlagIds.CodexEntrySuccubus);

                Save.SetBool("met", true);

                ClearButtons();

                AddNextButton("Common.Button.Fight".Localized(), () => CombatScene.Start(CurrentMonster, () => Go<CampScene>(), canFlee: false));

                AddNextButton("Common.Button.Accept".Localized(), BadEnd);
            }
            else
            {
                OutputText("Scenes.Ivorysuccubusscene.Encounter.AnArrogantGiggleCatches".Localized(
                    RandomUtils.Choice(
                    ["An arrogant giggle catches your attention, and you spot an ivory succubus gliding toward you at a substantial speed, giving you just enough reaction time to move out of the way before she lunges her demonic heels down.[pg]She lands heavy, scraping the ground with her heel-spikes, but otherwise graceful. [say: It is not just beauty, oh Champion.] She turns with a half-lidded expression and wry grin. [say: When you give up your soul, you will be stronger, faster, and oh so perfect.]",
                    "The snap of a whip steals your attention. An ivory succubus smirks at you as you gaze upon her form. [say: I am a sculpted work of art. Submit--you'll love to experience this.]",

                    "Shining overhead, an ivory demoness glides toward you, casting light-producing spells in some sort of attempt to dazzle. She flips around as she descends and lands gracefully before striking a pose. [if (silly) {[say:Now get the clap!] She pauses, wincing as she rethinks her words. [say: Er--I mean, get to clapping!]|[say:Now clap!] she commands.}] At the sight of your combat stance, she huffs. [say: No appreciation of the arts!]",

                    "[say: Trash,] you hear, taking notice of a demonic presence ahead. [say: No beauty, no grace, no tact, and no value,] the ivory succubus says as she grinds her heel into an imp's groin. [say: Such a grotesque dead-end.] The imp whimpers in pain as he takes the abuse. The demoness looks up and spots you, before grinning and pulling her whip taut. [say: Ah, much better. You, at least, have the potential for beauty.}[pg]The imp limply crawls away as the succubus chooses her new target."])));

                CombatScene.Start(CurrentMonster!, () => Go<CampScene>());
            }
        }

        public void BadEnd()
        {
            ClearOutput();

            if (IsRetarded() && silly)
            {
                OutputText("Scenes.Ivorysuccubusscene.BadEnd.AtFirstDeny".Localized());

                OutputText("Scenes.Ivorysuccubusscene.BadEnd.ConsiderConcede".Localized());

                OutputText("Scenes.Ivorysuccubusscene.BadEnd.DemonessSmiles".Localized());

                GameOver();
            }
            else
            {
                OutputText("Scenes.Ivorysuccubusscene.BadEnd.PerfectionBeauty".Localized());

                OutputText("Scenes.Ivorysuccubusscene.BadEnd.SuccubusGrins".Localized());

                DoNext(() => BadEnd2());
            }
        }

        public void BadEnd2()
        {
            ClearOutput();

            RegisterMacro("perfect", new[] { "pale", "alabaster", "ashen" }.Contains(Player.Body.Get<SkinPart>().Tone) && Player.HasPlainSkin() && Player.Thickness < Player.Tone && Player.Femininity >= 50 && new[] { "human", "elf", "dryad", "demon", "demon-morph" }.Contains(Player.Race));

            OutputText("Scenes.Ivorysuccubusscene.BadEnd2.ComingUp".Localized());

            OutputText("Scenes.Ivorysuccubusscene.BadEnd2.InsideSee".Localized());

            OutputText("Scenes.Ivorysuccubusscene.BadEnd2.IncubusContinues".Localized());
            // 原版 IvorySuccubusScene.hx:128 即为 if (/*lidellium placeholder*/ false)：上游未实装 Liddellium 剧情
            if (false)
                OutputText("Scenes.Ivorysuccubusscene.BadEnd2.ParagraphBreak".Localized());
            else
                OutputText("Scenes.Ivorysuccubusscene.BadEnd2.BringsHand".Localized(
                    Player.DogScore() >= 4 || Player.WolfScore() >= 4

                        ? "Chuckling, he says, [say: A wolf to be styled like dear Hwulf, yes. Beauty will find even you, mutt.]"

                        : "[say: You will find glorious beauty in my artistic guidance.]"));

            GameOver();
        }

        public void Won(bool hpvictory = true)
        {
            ClearOutput();

            Save.SetInt("timesLost", Save.GetInt("timesLost", 0) + 1);

            OutputText("Scenes.Ivorysuccubusscene.Won.FightGivesOut".Localized(
                hpvictory ? "Common.Text.Strength".Localized() : "Common.Text.Will".Localized(),

                Player.HasPerk(PerkIds.SlimeCore)

                    ? " The pressure  threatening to stab her demonic spike into your precious core adds another layer of menace alongside her smug smirk."

                    : ""));

            if (Player.HasCock())
            {
                OutputText("Scenes.Ivorysuccubusscene.Won.WithdrawsHeel".Localized());

                if (Player.CockThatFits(6, "length") != -1)
                {
                    OutputText("Scenes.Ivorysuccubusscene.Won.MocksHolding".Localized());

                    if (Player.BiggestCockLength() >= 10)
                        OutputText("Scenes.Ivorysuccubusscene.Won.IvoryScowls".Localized());
                }
                else
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Ivorysuccubusscene.Won.MusesKicks".Localized());
                    else
                        if (Player.BiggestCockLength() >= 10)
                            OutputText("Scenes.Ivorysuccubusscene.Won.VulgarSays".Localized());
                        else
                        {
                            OutputText("Scenes.Ivorysuccubusscene.Won.WorthyQuestion".Localized());

                            var __sw0_4 = Player.Cocks[0].Id;

                            if (__sw0_4 == BodyPartTypes.Anemone)
                                OutputText("Scenes.Ivorysuccubusscene.Won.SadIsntIt".Localized());

                            if (__sw0_4 == BodyPartTypes.Demon)
                                OutputText("Scenes.Ivorysuccubusscene.Won.MightBeFlattered".Localized());

                            if (__sw0_4 == BodyPartTypes.Cat)
                                OutputText("Scenes.Ivorysuccubusscene.Won.AlreadyKnew".Localized(hpvictory ? "Common.Text.Might".Localized() : "Common.Text.Wiles".Localized()));

                            if (__sw0_4 == BodyPartTypes.Dog || __sw0_4 == BodyPartTypes.Wolf)
                                OutputText("Scenes.Ivorysuccubusscene.Won.AsMuchDog".Localized());

                            if (__sw0_4 == BodyPartTypes.Horse)
                                OutputText("Scenes.Ivorysuccubusscene.Won.StrangelyPathetic".Localized());

                            if (__sw0_4 == BodyPartTypes.Human)
                                OutputText("Scenes.Ivorysuccubusscene.Won.WorldBrimming".Localized());

                            if (true)
                                OutputText("Scenes.Ivorysuccubusscene.Won.ExoticPathetic".Localized());

                            OutputText("Scenes.Ivorysuccubusscene.Won.SayEndMark".Localized());
                        }

                OutputText("Scenes.Ivorysuccubusscene.Won.IvoryAbuse".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Won.PaleExpression".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Won.ContentSigh".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Won.ClawsSqueeze".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Won.SuccubusAsks".Localized());

                Player.Orgasm("Dick");
            }
            else
                if (Player.HasVagina())
                {

                    RegisterMacro("lostVirginity", Player.LostVirginity);

                    OutputText("Scenes.Ivorysuccubusscene.Won.DragsFoot".Localized());

                    OutputText("Scenes.Ivorysuccubusscene.Won.ToRelief".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.Beg".Localized(), WonBeg);

                    AddNextButton("Common.Button.Submit".Localized(), WonSubmit);

                    return;
                }
                else
                {
                    OutputText("Scenes.Ivorysuccubusscene.Won.PullingBack".Localized());

                    OutputText("Scenes.Ivorysuccubusscene.Won.WhipTaut".Localized());

                    OutputText("Scenes.Ivorysuccubusscene.Won.PainLossControl".Localized());

                    OutputText("Scenes.Ivorysuccubusscene.Won.ChucklesMockingly".Localized());

                    OutputText("Scenes.Ivorysuccubusscene.Won.ObedientlyRepeat".Localized());

                    OutputText("Scenes.Ivorysuccubusscene.Won.KneelingStares".Localized());

                    Player.ChangeFatigue(10);
                }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            ClearButtons();

            AddNextButton("Common.Button.Accept".Localized(), BadEnd);

            AddNextButton("Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), Deny);
        }

        public void WonBeg()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.WonBeg.AsksNotFuck".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonBeg.SwallowingPride".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonBeg.BitesLip".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonBeg.WaitsAsksAgain".Localized());

            WonVag2();
        }

        public void WonSubmit()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.WonSubmit.SpreadingFingers".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonSubmit.TailWithdraws".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonSubmit.RegardlessSense".Localized(
                Player.Tallness < ivoryHeight + 6

                    ? "Her sultry eyes stare longingly at you before her lips are suddenly upon your own, and her lengthy demon tongue explores your mouth. It's a strangely deep and passionate kiss that feels as though it could melt you. "

                    : ""));

            Player.CuntChange(4, false);

            OutputText("Scenes.Ivorysuccubusscene.WonSubmit.RightsHerself".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonSubmit.FluidGushes".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonSubmit.TurnsHead".Localized());

            Player.Orgasm("Vaginal");

            WonVag2();
        }

        public void WonVag2()
        {

            OutputText("Scenes.Ivorysuccubusscene.WonVag2.NowLittle".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonVag2.MeeklyNod".Localized());

            OutputText("Scenes.Ivorysuccubusscene.WonVag2.GigglingBeckoning".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            ClearButtons();

            AddNextButton("Common.Button.Accept".Localized(), BadEnd);

            AddNextButton("Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), Deny);
        }

        public void Deny()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.Deny.ShallNotSuccumb".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void Defeated(bool hpvictory = true)
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.Defeated.CollapsesIn".Localized(
                hpvictory ? "Common.Text.ThrownBackByYourAssault".Localized() : "Common.Text.FlushedAndOverwhelmedByYourEroticism".Localized()));

            ClearButtons();

            AddNextButton(SexSwitch("Cock", "Pussy", "Genital", "Genital") + "Common.Text.Worship".Localized(), Worship);

            AddNextButton("Common.Button.Ride".Localized(), Riding);

            AddNextButton("Common.Button.Anal".Localized(), Anal);

            AddNextButton("Common.Button.Lash".Localized(), Lash);

            AddNextButton("Common.Button.Kill".Localized(), Kill);

            AddButton(14, "Common.Button.Leave".Localized(), () => SexLeave());
        }

        public void Worship()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.Worship.ArrogantClaim".Localized());

            OutputText("Scenes.Ivorysuccubusscene.Worship.ThrustingClear".Localized());

            OutputText("Scenes.Ivorysuccubusscene.Worship.CrackInPride".Localized());

            OutputText("Scenes.Ivorysuccubusscene.Worship.FlushedSqueezes".Localized(
                ivoryBreasts < 2 ? "meager " : (ivoryBreasts > 3 ? "pillowy " : "")));

            OutputText("Scenes.Ivorysuccubusscene.Worship.SaysRelief".Localized());

            OutputText("Scenes.Ivorysuccubusscene.Worship.PullingAway".Localized());

            OutputText("Scenes.Ivorysuccubusscene.Worship.EuphoriaClears".Localized());

            OutputText("Scenes.Ivorysuccubusscene.Worship.RollsOff".Localized());

            Player.Orgasm("All");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void Riding()
        {
            ClearOutput();

            if (Player.IsTaur() && silly)
            {
                OutputText("Scenes.Ivorysuccubusscene.Riding.StraightToPoint".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.WellThisWas".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.ForgotPurse".Localized());

                CombatScene.CleanupAfterCombat();
            }

            if (Player.HasCock())
            {
                OutputText("Scenes.Ivorysuccubusscene.Riding.AfterEffort".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.CrawlingOver".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.SlidesOver".Localized(
                    Player.Cocks[Player.BiggestCockIndex()].Id != BodyPartTypes.Human ? Player.CockMultiNoun(Player.BiggestCockIndex()) : ""));

                if (Player.CockTotal() > 1)
                {
                    ClearButtons();

                    AddNextButton("Common.Button.Dp".Localized(), () => RidingCock(true));

                    AddNextButton("Common.Button.Dpp".Localized(), () => RidingCock(false));
                }
                else
                    RidingCock2();
            }
            else
            {

                RegisterMacro("looseness", Player.IsGenderless() ? Player.Anal.Looseness : Player.Vaginas[0].Looseness);

                RegisterMacro("wetness", Player.IsGenderless() ? Player.Anal.Wetness : Player.Vaginas[0].Wetness);

                RegisterMacro("virgin", Player.IsGenderless() ? Player.Anal.Virgin : Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin);

                OutputText("Scenes.Ivorysuccubusscene.Riding.WhatYoullHave".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.WatchingAppendage".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.DemonicTongue".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.FinallySinks".Localized());

                if (Player.HasVagina())
                    Player.CuntChange(4, true);
                else
                    Player.ButtChange(4, true, false);

                OutputText("Scenes.Ivorysuccubusscene.Riding.AdjustComfort".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.InvitingKiss".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.BucksHips".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Riding.SultrySplendor".Localized());

                Player.Orgasm("VaginalAnal");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void RidingCock(bool DP)
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.RidingCock.LeansForward".Localized(
                DP ? "Common.Text.SuppleBottom".Localized() : "Common.Text.PussyStretchingThePliableOrificeTo".Localized()));

            RidingCock2(DP);
        }

        public void RidingCock2(bool DP = false)
        {

            OutputText("Scenes.Ivorysuccubusscene.RidingCock2.InstinctivelyBucks".Localized(DP ? "s" : ""));

            OutputText("Scenes.Ivorysuccubusscene.RidingCock2.ContinuesRocking".Localized());

            OutputText("Scenes.Ivorysuccubusscene.RidingCock2.WrithingBeneath".Localized());

            OutputText("Scenes.Ivorysuccubusscene.RidingCock2.CatchesBreath".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void Anal()
        {
            ClearOutput();

            if (!Player.HasCock())
            {
                OutputText("Scenes.Ivorysuccubusscene.Anal.DescendingCups".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Anal.IgnoringRequest".Localized());

                ClearButtons();

                AddNextButton("Common.Button.ManIt".Localized(), AnalMan);

                AddNextButton("Common.Button.TailFuck".Localized(), AnalTail);

                AddNextButton("Common.Button.Leave".Localized(), AnalLeave);
            }
            else
            {
                OutputText("Scenes.Ivorysuccubusscene.Anal.DescendingCupsAgain".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Anal.AcclimatedBegins".Localized((Player.Tallness > ivoryHeight + 7 ? "holding her head to your [chest]" : (Player.Tallness < ivoryHeight ? "Common.Text.RestingYourHeadInHerBosom".Localized() : "Common.Text.PlacingYourForeheadAgainstHersReadily".Localized())), (Player.CockTotal() > 1 && Player.LongestCockLength() < 6 ? "Common.Text.Cocks".Localized() : "Common.Text.Cock".Localized())));

                OutputText("Scenes.Ivorysuccubusscene.Anal.ChangingTechnique".Localized(
                Player.IsInMountains || Player.IsInHighMountains ? " and gravel" : ""));

                var __sw0_1 = true;

                if (__sw0_1 == Player.IsInMountains || __sw0_1 == Player.IsInHighMountains)
                    OutputText("Scenes.Ivorysuccubusscene.Anal.PebblesWord".Localized());

                if (__sw0_1 == Player.IsInSwamp || __sw0_1 == Player.IsInBog)
                    OutputText("Scenes.Ivorysuccubusscene.Anal.MudWord".Localized());

                if (__sw0_1 == Player.IsInDesert)
                    OutputText("Scenes.Ivorysuccubusscene.Anal.SandWord".Localized());

                if (true)
                    OutputText("Scenes.Ivorysuccubusscene.Anal.SoilWord".Localized());

                OutputText("Scenes.Ivorysuccubusscene.Anal.DemonCoughs".Localized(
                (Player.LongestCockLength() < 6 && Player.CockTotal() > 2) || Player.CockTotal() > 1

                    ? "snakes lovingly around your [chest] and back, inspiring you to grab the rope-like appendage and bend it around your bundle of penises, then shove the tip inside. Unperturbed, it continues to wriggle and slide"

                    : "wriggles and slides"));

                OutputText("Scenes.Ivorysuccubusscene.Anal.AfterManyMinutes".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void AnalMan()
        {
            ClearOutput();

            Player.CreateCock();

            Player.Cocks[0].Length = Random.Shared.Next(4) + 4;

            Player.Cocks[0].Thickness = Random.Shared.Next(2);

            OutputText("Scenes.Ivorysuccubusscene.AnalMan.EarlierTemptation".Localized());

            OutputText("Scenes.Ivorysuccubusscene.AnalMan.OvercomeWild".Localized());

            OutputText("Scenes.Ivorysuccubusscene.AnalMan.StillDriven".Localized());

            OutputText("Scenes.Ivorysuccubusscene.AnalMan.BlinksRealizing".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 4f), new DynStat(GameStats.Sensitivity, 5f), new DynStat(GameStats.Libido, 3f));

            CombatScene.CleanupAfterCombat();
        }

        public void AnalTail()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.AnalTail.TailBrushesChin".Localized());

            if (!Player.HasTailInsteadOfLegs())
            {
                var __sw0_1 = Player.Body.Get<TailPart>().Id;

                if (__sw0_1 == BodyPartTypes.Lizard || __sw0_1 == BodyPartTypes.Salamander || __sw0_1 == BodyPartTypes.Draconic || __sw0_1 == BodyPartTypes.Cockatrice)
                    OutputText("Scenes.Ivorysuccubusscene.AnalTail.ScalyWord".Localized());

                if (__sw0_1 == BodyPartTypes.Demonic || __sw0_1 == BodyPartTypes.Imp)
                    OutputText("Scenes.Ivorysuccubusscene.AnalTail.LeatheryWord".Localized());

                if (__sw0_1 == BodyPartTypes.Mouse)
                    OutputText("Scenes.Ivorysuccubusscene.AnalTail.FleshyWord".Localized());

                if (true)
                    OutputText("Scenes.Ivorysuccubusscene.AnalTail.FurryWord".Localized());
            }

            OutputText("Scenes.Ivorysuccubusscene.AnalTail.MakesItKnown".Localized());

            OutputText("Scenes.Ivorysuccubusscene.AnalTail.CompressionSoothing".Localized());

            OutputText("Scenes.Ivorysuccubusscene.AnalTail.PushesAway".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f), new DynStat(GameStats.Lust, 20f));

            CombatScene.CleanupAfterCombat();
        }

        public void AnalLeave()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.AnalLeave.AsSated".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f), new DynStat(GameStats.Lust, 10f));

            CombatScene.CleanupAfterCombat();
        }

        public void Lash()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.Lash.WeaponSmarts".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Continue".Localized(), LashContinue);

            AddNextButton("Common.Button.Stop".Localized(), LashStop);
        }

        public void LashContinue()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.LashContinue.CouldntKid".Localized());

            var weaponId = Player.Weapon.Id;

            if (weaponId == ItemIds.LethicesWhip)
                OutputText("Scenes.Ivorysuccubusscene.LashContinue.SearingMarks".Localized());
            else if (weaponId == ItemIds.SilverWhip)
                OutputText("Scenes.Ivorysuccubusscene.LashContinue.NastyWelts".Localized());
            else if (weaponId == ItemIds.SuccubusWhip)
                OutputText("Scenes.Ivorysuccubusscene.LashContinue.DoubtlesslyExcites".Localized());
            else
                OutputText("Scenes.Ivorysuccubusscene.LashContinue.RedStreaks".Localized());

            OutputText("Scenes.Ivorysuccubusscene.LashContinue.AnotherLash".Localized());

            OutputText("Scenes.Ivorysuccubusscene.LashContinue.TurnsToFace".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void LashStop()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.LashStop.FeelsSufficient".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void Kill()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.Kill.WillPassOn".Localized());

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Ivorysuccubusscene.Kill.OneSwiftMotion".Localized());
            else if (Player.Weapon.IsScythe())
                OutputText("Scenes.Ivorysuccubusscene.Kill.SwingingNeck".Localized());
            else if (Player.Weapon.Id == ItemIds.SilverWhip)
                OutputText("Scenes.Ivorysuccubusscene.Kill.TwirlSnap".Localized());
            else if (Player.Weapon.IsWhip())
                OutputText("Scenes.Ivorysuccubusscene.Kill.CordSlings".Localized());
            else if (Player.Weapon.IsKatana() && silly)
                OutputText("Scenes.Ivorysuccubusscene.Kill.PerfectCut".Localized());
            else if (Player.Weapon.IsAxe())
                OutputText("Scenes.Ivorysuccubusscene.Kill.GrippingWeapon".Localized());
            else if (Player.Weapon.IsStaff())

                OutputText("Scenes.Ivorysuccubusscene.Kill.YourLandsSharplyUpon".Localized());
            else if (Player.Weapon.IsBlunt())
                OutputText("Scenes.Ivorysuccubusscene.Kill.CrudeEffective".Localized());
            else if (Player.Weapon.IsFirearm())
                OutputText("Scenes.Ivorysuccubusscene.Kill.LeaningFire".Localized());
            else if (Player.Weapon.IsSpear() || Player.Weapon.IsPolearm())
                OutputText("Scenes.Ivorysuccubusscene.Kill.SingleThrust".Localized());
            else if (Player.Weapon.IsBladed())
                OutputText("Scenes.Ivorysuccubusscene.Kill.HeavyLunge".Localized());
            else
                OutputText("Scenes.Ivorysuccubusscene.Kill.DoesNotResist".Localized());

            Save.SetInt("deadHookers", Save.GetInt("deadHookers", 0) + 1);

            Player.UpgradeDeusVult();

            CombatScene.CleanupAfterCombat();
        }

        public void SexLeave()
        {
            ClearOutput();

            OutputText("Scenes.Ivorysuccubusscene.SexLeave.TemptationsWont".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }
}