using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Dungeons
{

    public static class DesertCaveRooms
    {
        [Register("coc:desert_cave_throne_unlocked")]
        public static bool ThroneUnlocked(DungeonContext context) =>

            CoC.Instance.Flags.Get(GameFlagIds.SandwitchThroneUnlocked) > 0;

        [Register("coc:desert_cave_exit_allowed")]
        public static bool ExitAllowed(DungeonContext context)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.SandwitchMobDefeated) == 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel) < 3)
                return true;

            return CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed)

                + CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) > 0;
        }

        [Register("coc:desert_cave_room_entrance")]
        public static void RoomEntrance(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomEntrance.StrangeGatewayHeader".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SanuraDisabled) > 0)
                context.Write("Scenes.DesertCave.RoomEntrance.SquareStoneDoorwayAgain".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.MetSanura) == 0)
            {
                context.Write("Scenes.DesertCave.RoomEntrance.SquareStoneDoorwayFirst".Localized());

                context.Write("Scenes.DesertCave.RoomEntrance.SphinxShadowLooms".Localized());

                context.Write("Scenes.DesertCave.RoomEntrance.SphinxPadsOver".Localized());

                context.Write("Scenes.DesertCave.RoomEntrance.RiddleChallengeSpeech".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MetSanura, 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredWitchDungeon) == 0)
                {
                    context.Write("Scenes.DesertCave.RoomEntrance.NewDungeonNotice".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.DiscoveredWitchDungeon, 1);
                }

                Scenes.Scene.AddButton(0, "Common.Button.Appearance".Localized(), () => SphinxAppearance(context));

                Scenes.Scene.AddButton(1, "Common.Button.RiddleGame".Localized(), () => RiddleGameGo(context));

                Scenes.Scene.AddButton(2, "Common.Button.UhFight".Localized(), () => FuckItAttack(context, () => RoomEntrance(context)));
            }
            else
            {
                var submitted = CoC.Instance.Flags.Get(GameFlagIds.TimesSubmittedToSanura)

                    + CoC.Instance.Flags.Get(GameFlagIds.TimesWinfuckedSanura);

                if (submitted > 0)
                {
                    context.Write("Scenes.DesertCave.RoomEntrance.ApproachSanura".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.BeatenSanuraCount) > 0)
                        context.Write("Scenes.DesertCave.RoomEntrance.SkipFormalitiesTaunt".Localized());
                }
                else
                    context.Write("Scenes.DesertCave.RoomEntrance.SanuraPacesDoorframe".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Sanura".Localized(), () => SphinxMenu(context));
            }
        }

        [Register("coc:desert_cave_room_cave_commons")]
        public static void RoomCaveCommons(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomCaveCommons.CavernousCommonsHeader".Localized());

            context.Write("Scenes.DesertCave.RoomCaveCommons.DancingLightsSwirl".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandwitchMobDefeated) == 0)
            {
                context.Write("Scenes.DesertCave.RoomCaveCommons.SpellcastersMobForms".Localized());

                context.Write("Scenes.DesertCave.RoomCaveCommons.BarrierBlocksExit".Localized());

                context.StartCombat(EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "sand_witch_mob")));

                return;
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchMobVictoryShown) == 0)
                {
                    context.Write("Scenes.DesertCave.YoYouBeatUpSomeSandWitchesYOUMONSTER.WitchesCollapseInPuddle".Localized());

                    context.Write("Scenes.DesertCave.YoYouBeatUpSomeSandWitchesYOUMONSTER.NursingTheirWounds".Localized());

                    context.Write("Scenes.DesertCave.YoYouBeatUpSomeSandWitchesYOUMONSTER.FranticOrgyOnFloor".Localized());

                    context.Write("Scenes.DesertCave.YoYouBeatUpSomeSandWitchesYOUMONSTER.StandAloneSoleVictor".Localized());

                    context.Write("Scenes.DesertCave.YoYouBeatUpSomeSandWitchesYOUMONSTER.TimeToCelebrate".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.SandWitchMobVictoryShown, 1);
                }
                else
                    context.Write("Scenes.DesertCave.RoomCaveCommons.DefeatedWomenResume".Localized());

                if (CoC.Instance.Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.SandWitchMobFucked) == 0)
                    Scenes.Scene.AddButton(0, "Common.Button.ForcedLick".Localized(), () => ForceCunnilingusRimjobClitAndNipple(context));
            }

            context.Write("Scenes.DesertCave.RoomCaveCommons.TunnelsEastWestSouth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel) >= 3
                && CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed) + CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) <= 0)

                context.Write("Scenes.DesertCave.RoomCaveCommons.BarrierBlocksSouth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandwitchThroneUnlocked) == 0)
                context.Write("Scenes.DesertCave.RoomCaveCommons.StoneDoorwayBlocksNorth".Localized());
            else
                context.Write("Scenes.DesertCave.RoomCaveCommons.OpenDoorwayAltar".Localized());
        }

        public static void ForceCunnilingusRimjobClitAndNipple(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.EyeDefeatedMob");

            W(p.Health < 1

                ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ThroughTornRobes"

                : "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SquirmingMotionsNoting");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.TheyMay");

            W(p.Corruption < 33

                ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.MisguidedInFighting"

                : p.Corruption < 66

                    ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.OverzealousDefendingHome"

                    : "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.CrappyFighters");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.StirringInNethers");

            var called = "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.MilkWitchesCalled".Localized()
                .Replace("[if (silly) {b|w}]", Configuration.OptionStore.Instance.Get("silly", 0) == 1 ? "b" : "w");

            context.Write(called);

            W(p.HasTailInsteadOfLegs()

                ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SlitherWord"

                : p.IsTaur()

                    ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ClopWord"

                    : p.IsGoo()

                        ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.OozeWord"

                        : p.IsDrider()

                            ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ClatterWord"

                            : "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.StrideWord");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.UpToNearestOne");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.LikePlayDirector");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ManyGirlsRemain");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ShortestHumiliatingJob");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.WitchAsksPlaintively");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.KneelsTannedDispenser");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.GrabTallerOfTwo");

            W(p.GetClitLength() < 3

                ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SlurpsIntoMouth"

                : p.GetClitLength() < 12

                    ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SlurpsThickCock"

                    : "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.DivesOntoButton");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ThunderclapOfPleasure");

            W(p.Wetness() >= 4

                ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.MuskyGooOnFaces"

                : p.Wetness() >= 2

                    ? "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SluicingSecretions"

                    : "Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.DribblingGirlCum");

            if (p.HasCock())
            {
                W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.CockFiresVirileSpunk");

                if (p.CumQ() >= 500)
                {
                    W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SkinTurnsSoupyWhite");

                    if (p.CumQ() >= 1500)
                        W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.PoolingIntoPuddle");

                    if (p.CumQ() >= 2500)
                        W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.PuddlesFormLake");

                    W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SentencePeriod");
                }
            }

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.FlopOntoBack");

            if (p.LactationQ() >= 200)
                W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.DrinkTheirFill");

            if (p.LactationQ() >= 1000)
                W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.LacticCapacity");
            else if (p.LactationQ() < 200)
                W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.SlowlySuckleYou");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.ComeDownFromHigh");

            W("Scenes.DesertCave.ForceCunnilingusRimjobClitAndNipple.AfterMoraleBoost");

            p.Orgasm("Generic");

            p.Sensitivity = Math.Max(0, p.Sensitivity - 1);

            CoC.Instance.Flags.Set(GameFlagIds.SandWitchMobFucked, 1);

            Scenes.Scene.AddButton(0, "Common.Button.Continue".Localized(), () => RoomCaveCommons(context));
        }

        [Register("coc:desert_cave_room_west_hall_one")]
        public static void RoomWestHall1(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomWestHall1.WesternWarrensHallHeader".Localized());

            context.Write("Scenes.DesertCave.RoomWestHall1.IlluminationThinnerHere".Localized());
        }

        [Register("coc:desert_cave_room_west_hall_two")]
        public static void RoomWestHall2(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomWestHall2.WestHallHeader".Localized());

            context.Write("Scenes.DesertCave.RoomWestHall2.TunnelEndsSandstone".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EssrayleEscapedDungeon) == 0
                && CoC.Instance.Flags.Get(GameFlagIds.MetEssy) > 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.EssyMetInDungeon, 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.ToldMotherToReleaseEssy) > 0)
                {
                    context.Write("Scenes.DesertCave.RoomWestHall2.AttentionDrawnEssrayle".Localized());

                    Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
                    {
                        var essy = new Scenes.Vanilla.Areas.Forest.EssrayleScene();

                        Scenes.Scene.Go(essy);

                        essy.EssyWitchVictory();

                        CoC.Instance.Flags.Set(GameFlagIds.EssrayleEscapedDungeon, 1);
                    });

                    return;
                }

                context.Write("Scenes.DesertCave.RoomWestHall2.EssrayleInPot".Localized());

                Scenes.Scene.AddButton(0, "Npcs.Essrayle.Name".Localized(), () =>
                {
                    var essy = new Scenes.Vanilla.Areas.Forest.EssrayleScene();

                    Scenes.Scene.Go(essy);

                    essy.ApproachTrappedEssy();
                });
            }
        }

        [Register("coc:desert_cave_room_play_room")]
        public static void RoomPlayRoom(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomPlayRoom.PlayRoomHeader".Localized());

            context.Write("Scenes.DesertCave.RoomPlayRoom.ChildrenPlayRoom".Localized());
        }

        [Register("coc:desert_cave_room_nursery")]
        public static void RoomNursery(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomNursery.NurseryHeader".Localized());

            context.Write("Scenes.DesertCave.RoomNursery.NurseryNothingInteresting".Localized());
        }

        [Register("coc:desert_cave_room_lust_room")]
        public static void RoomLustRoom(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomLustRoom.LustRoomHeader".Localized());

            context.Write("Scenes.DesertCave.RoomLustRoom.RoomHoldsPregnantWomen".Localized(NumberUtils.Num2Text(Random.Shared.Next(6) + 5)));

            if (CoC.Instance.Player.HasCock() && CoC.Instance.Player.Lust >= 33)
                Scenes.Scene.AddButton(0, "Common.Button.FuckWitches".Localized(), () => KnockUpSomeDoubleStuffedSandWitches(context));
            else
                Scenes.Scene.AddButtonDisabled(0, "Fuck Witches", "This scene requires you to have a cock and sufficient arousal.");
        }

        public static void KnockUpSomeDoubleStuffedSandWitches(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.KnockUpSomeDoubleStuffedSandWitches.ShedArmorForSluts".Localized());

            context.Write("Scenes.DesertCave.KnockUpSomeDoubleStuffedSandWitches.PregnantWitchesCrawl".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 33);

            if (player.CockThatFits(50) < 0)
                context.Write("Scenes.DesertCave.KnockUpSomeDoubleStuffedSandWitches.TooBigNotice".Localized());

            if (player.CockThatFits(50) >= 0)
                Scenes.Scene.AddButton(0, "Common.Button.FuckOne".Localized(), () => FuckOneSandWitch(context));
            else
                Scenes.Scene.AddButtonDisabled(0, "Fuck One", "You're too big to fit.");

            if (player.CockThatFits(50) >= 0 && player.CockThatFits2(50) >= 0)
                Scenes.Scene.AddButton(1, "Common.Button.FuckTwo".Localized(), () => FuckTwoPregWitches(context));
            else
                Scenes.Scene.AddButtonDisabled(1, "Fuck Two", "This scene requires you to have two fitting cocks.");

            if (player.CockThatFits(50) >= 0)
                Scenes.Scene.AddButton(2, "Common.Button.FuckEmAll".Localized(), () => FuckAllThePregWitches(context));
            else
                Scenes.Scene.AddButtonDisabled(2, "Fuck'EmAll", "You're too big to fit.");

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => RoomLustRoom(context));
        }

        public static void FuckAllThePregWitches(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var multi = false;

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.WitchesCrawlForward".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.CocksRiseIntoAir".Localized());

            if (player.Cocks.Count == 1 || player.CockThatFits2(50) < 0)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SlideShaftAlongWitch".Localized());
            else if (player.Cocks.Count < 3)
            {
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.PushTwoWitchesTogether".Localized());

                multi = true;
            }
            else
            {
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.GatherThreeBentWitches".Localized());

                multi = true;
            }

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ShaftInsertionBegins".Localized());

            if (multi)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ShaftSwallowedByDepths".Localized());

            if (multi)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.WitchesPluralSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.RockingHipsRhythm".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheirFinalThey".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.HerThighIt".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.VelvetyWetDepths".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheirStartThey".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.HerStartIt".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.BlissfulCriesFillRoom".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheirClauseThey".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.HerClauseIt".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.QuadrupleBreastsJiggle".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.PregnantWord".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.BelliesSwayPlural".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.BellySwaysSingular".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.EachRammingMotion".Localized());

            if (multi)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.MusclesTenseSetup".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheirFinalThey".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.HerThighIt".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ThighsCalvesTense".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheirBodiesThey".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.HerBodyIt".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ToTheLimitPhrase".Localized());

            if (player.BiggestBreastSize() > 3 && player.BiggestBreastSize() < 10)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.YourTitsBounceGrinding".Localized());
            else if (player.BiggestBreastSize() >= 10 && player.BiggestBreastSize() < 20)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.HeavyTitsSway".Localized());
            else if (player.BiggestBreastSize() >= 20)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.MammariesPressDownAss".Localized());

            if (player.BreastRows.Count >= 2 && player.BreastRows[1].BreastRating >= 3)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SecondBreastRowSlap".Localized());

            if (player.BreastRows.Count >= 3)
            {
                var third = player.BreastRows[2].BreastRating;

                if (third >= 3 && third < 20)
                {
                    context.Write("Scenes.DesertCave.FuckAllThePregWitches.LowestBreastsSquashed".Localized());

                    if (multi)
                        context.Write("Scenes.DesertCave.FuckAllThePregWitches.WitchesPluralSuffix".Localized());

                    context.Write("Scenes.DesertCave.FuckAllThePregWitches.MuscularThighsSuffix".Localized());
                }
                else if (third >= 20)
                {
                    context.Write("Scenes.DesertCave.FuckAllThePregWitches.LowestBreastsRestFloor".Localized());

                    if (multi)
                        context.Write("Scenes.DesertCave.FuckAllThePregWitches.WitchesPluralSuffix".Localized());

                    context.Write("Scenes.DesertCave.FuckAllThePregWitches.AlmostSupportWeight".Localized());

                    context.Write(multi

                        ? "Scenes.DesertCave.FuckAllThePregWitches.TheirFinalThey".Localized()

                        : "Scenes.DesertCave.FuckAllThePregWitches.HerThighIt".Localized());

                    context.Write("Scenes.DesertCave.FuckAllThePregWitches.ThighsAgainstMammaries".Localized());
                }
            }

            if (player.LactationQ() >= 200)
            {
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.TitsBeginLeaking".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.TannedBodiesImpaled".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.WitchTannedBackside".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.OtherWitchesSuckle".Localized());
            }

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.CumStartsMoving".Localized());

            if (multi)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.OrgasmCreepsUp".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.GirlsCuntsOnCocks".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.GirlCuntOnCock".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.PleasureBecomesOverwhelming".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.PussiesPlural".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.PussySingular".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.PoundingSeemFragment".Localized());

            if (!multi)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.SenseImminentRelease".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheyCraveFragment".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.ItCravesFragment".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ContractionsSuckShaft".Localized());

            if (multi)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.FerventReleaseBuild".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.ThemGetBabyBatter".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.ItGetsBabyBatter".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.SentencePeriodAgain".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ParagraphStartThe".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.ShaftsUndersidesSwell".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.ShaftUndersideSwells".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.ManSeedFloodsLength".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.WitchesCunts".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.WitchCunt".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.PressForwardAlignTip".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TipsAgainstCervixes".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.TipAgainstCervix".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.CumBeginsErupting".Localized());

            if (player.CumQ() < 1000)
            {
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.PlasterPussyInsides".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.TheirPussiesThey".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.HerPussyIt".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.CumSquirtsIntoWomb".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.TheirWombsThey".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.HerWombIt".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.PullOutAfterCum".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.StreamsPlural".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.StreamSingular".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.CumLeftBehind".Localized());
            }
            else
            {
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.HornyWitchSetup".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.WitchesAreNotThey".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.WitchIsNotIt".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.DelugeExplodesGenitals".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.TheirWombsFillBalloons".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.HerWombFillsBalloon".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.EyesRollInPleasure".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.TheirBelliesSwellThey".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.HerBellySwellsIt".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.BelliesSwellLikeNeighbors".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.FinishSpraying".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.SidePluralSuffix".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.TheirFinalThey".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.ItsSingularIt".Localized());

                context.Write("Scenes.DesertCave.FuckAllThePregWitches.FinalLoadsPour".Localized());

                context.Write(multi

                    ? "Scenes.DesertCave.FuckAllThePregWitches.WitchesAssesGrouped".Localized()

                    : "Scenes.DesertCave.FuckAllThePregWitches.WhorishWitchBack".Localized());
            }

            context.Write(multi

                ? "Scenes.DesertCave.FuckAllThePregWitches.TheyFallPlural".Localized()

                : "Scenes.DesertCave.FuckAllThePregWitches.SheFallsSingular".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.RestWitchesRaiseCunts".Localized());

            if (player.CockThatFits2(50) >= 0)
                context.Write("Scenes.DesertCave.FuckAllThePregWitches.FinalWitchDoubleCocks".Localized());

            context.Write("Scenes.DesertCave.FuckAllThePregWitches.StandInJizzPool".Localized());

            player.Orgasm("Generic");

            player.Sensitivity = Math.Max(0, player.Sensitivity - 3);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomLustRoom(context));
        }

        public static void FuckTwoPregWitches(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var tentacles = player.Cocks.Count(c => c.Id.Path == "tentacle");

            var x = player.CockThatFits(50);

            var y = player.CockThatFits2(50);

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.WitchesBecomeFleshOrbs".Localized(CoC.Instance.Player.CockDescript(y)));

            context.Write(tentacles >= 2

                ? "Scenes.DesertCave.FuckTwoPregWitches.TentaclesGrabArms".Localized()

                : "Scenes.DesertCave.FuckTwoPregWitches.PullTwoWitchesDown".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.WitchesLieBeforeYou".Localized());

            context.Write(player.HasTailInsteadOfLegs()

                ? "Scenes.DesertCave.FuckTwoPregWitches.TailWrapsAroundLegs".Localized()

                : "Scenes.DesertCave.FuckTwoPregWitches.StraddleBothWitches".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.LegsRemainOnFloor".Localized());

            context.Write(player.BiggestBreastSize() >= 15

                ? "Scenes.DesertCave.FuckTwoPregWitches.CocksSlideCleavage".Localized()

                : "Scenes.DesertCave.FuckTwoPregWitches.LegsRestOnShoulders".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.SentencePeriod".Localized());

            if (player.Pregnancy.Incubation > 0 && player.Pregnancy.Incubation < 200)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.LegsBendForBelly".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.CocksSlapTheirPussies".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.ReadyToFulfillNeeds".Localized());

            var typeX = player.Cocks[x].Id.Path;

            var typeY = player.Cocks[y].Id.Path;

            if (typeX == "horse" && typeY == "horse")
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.EquineTipsPressLips".Localized());
            else if (typeX == "dog" && typeY == "dog")
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.CaninePricksPartFolds".Localized());
            else if (typeX == "tentacle" && typeY == "tentacle")
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.GreenCocksWeaveAround".Localized());
            else if (typeX == "demon" && typeY == "demon")
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.BumpyRimsFeelLovely".Localized());
            else
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.PressCockheadOnLabia".Localized());

            if (player.Cocks.Count >= 4)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.EyePuckeredAssholes".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.EmbeddedBetweenTwoSluts".Localized());

            if (player.Cocks[x].Length < 20 && player.Cocks[y].Length < 20)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.ReachBothWombs".Localized());
            else
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.DelveInchByInch".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.EightBreastsWobble".Localized());

            if (player.LactationQ() >= 200)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.BreastsSprayMilk".Localized());

            if (player.BiggestBreastSize() >= 5 && player.BiggestBreastSize() < 20)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.ThrustsMakeBreastsBounce".Localized());
            else if (player.BiggestBreastSize() >= 20)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.BreastsSwayAndSlap".Localized());

            if (player.BreastRows.Count >= 3 && player.BreastRows[2].BreastRating >= 20)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.LowerBreastsRestFloor".Localized());

            if (player.Balls > 0)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.BallsTenseNearClimax".Localized());

            context.Write("Scenes.DesertCave.FuckTwoPregWitches.WitchesFingerThemselves".Localized());

            if (player.CumQ() < 500)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.GirlsYelpAtSeed".Localized());
            else if (player.CumQ() < 1000)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.MilkBagsCryOut".Localized());
            else if (player.CumQ() < 3000)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.BelliesScreamInEcstasy".Localized());
            else
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.WitchesShockedAtSwell".Localized());

            if (player.Cocks.Count >= 4)
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.CumDripsFromAnuses".Localized());

            if (player.CumQ() < 1000)
            {
                if (!player.HasTailInsteadOfLegs())
                    context.Write("Scenes.DesertCave.FuckTwoPregWitches.WeakLegsFragment".Localized());
                else
                {
                    var body = "Scenes.DesertCave.FuckTwoPregWitches.WeakenedBodyFragment".Localized()
                        .Replace("[if (isNaga) {snake|[if (isMer) {fish|tail-like}]}]",

                            player.IsNaga() ? "snake" : (player.LowerBodyType().GetString("body") == "mermaid" ? "Common.Text.Fish".Localized() : "Common.Text.TailLike".Localized()));

                    context.Write(body);
                }

                context.Write("Scenes.DesertCave.FuckTwoPregWitches.CocksSlideOut".Localized());
            }
            else
                context.Write("Scenes.DesertCave.FuckTwoPregWitches.CumPoolEasesExit".Localized());

            player.Orgasm("Generic");

            player.Sensitivity = Math.Max(0, player.Sensitivity - 2);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomLustRoom(context));
        }

        public static void FuckOneSandWitch(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var tentacles = player.Cocks.Count(c => c.Id.Path == "tentacle");

            var x = player.CockThatFits(50);

            if (x < 0) x = player.SmallestCockIndex();

            context.Write(tentacles == 0

                ? "Scenes.DesertCave.FuckOneSandWitch.FlipWitchOnBack".Localized()

                : "Scenes.DesertCave.FuckOneSandWitch.TentacleGrabsAnkle".Localized());

            context.Write(tentacles > 1

                ? "Scenes.DesertCave.FuckOneSandWitch.SecondTentacleOtherAnkle".Localized()

                : "Scenes.DesertCave.FuckOneSandWitch.HandGrabsOtherAnkle".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.DualLabiaDrippingJuices".Localized());

            var multi = false;

            if (tentacles >= 2 && player.Cocks.Count >= 3 && player.Cocks[x].Id.Path != "tentacle")
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.TentaclesGripHerThighs".Localized());

                multi = true;
            }
            else if (tentacles >= 2 && player.Cocks[x].Id.Path != "tentacle" && player.CockThatFits2(50) >= 0 && player.Cocks.Count >= 4)
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.AlignCocksSecondPussy".Localized());

                multi = true;
            }
            else
                context.Write("Scenes.DesertCave.FuckOneSandWitch.HandsGraspHerHips".Localized(CoC.Instance.Player.CockDescript(x)));

            context.Write("Scenes.DesertCave.FuckOneSandWitch.MoansArouseOnlookers".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.ThrustsRockHerTorso".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckOneSandWitch.PluralPoundSuffix".Localized()

                : "Scenes.DesertCave.FuckOneSandWitch.SingularPoundsSuffix".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.DroolDripsDownCheek".Localized());

            if (player.BiggestBreastSize() >= 4 && player.BiggestBreastSize() < 15)
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.BreastsBounceWhileFucking".Localized());

                if (player.Pregnancy.Incubation > 0 && player.Pregnancy.Incubation < 200)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.BreastsSlapSwollenBelly".Localized());
                else if (player.BreastRows.Count >= 2)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.LowerBreastsSlapBelly".Localized());
            }
            else if (player.BiggestBreastSize() >= 15)
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.MassiveChestsJiggle".Localized());

                if (player.BreastRows.Count == 1)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.HeavyMilkSacsSway".Localized());
                else
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.GlobesRestOnLegs".Localized());

                context.Write("Scenes.DesertCave.FuckOneSandWitch.WitchesGawkAtChest".Localized());
            }

            context.Write("Scenes.DesertCave.FuckOneSandWitch.SensationsBuildTowardClimax".Localized());

            context.Write(multi

                ? "Scenes.DesertCave.FuckOneSandWitch.ShaftsWordPlural".Localized()

                : "Scenes.DesertCave.FuckOneSandWitch.ShaftWordSingular".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.ForcesYourOwnOrgasm".Localized());

            if (player.Balls > 0)
                context.Write("Scenes.DesertCave.FuckOneSandWitch.BallsTenseAnticipation".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.GrabNipplesLikeReins".Localized());

            if (player.BiggestBreastSize() >= 15)
                context.Write("Scenes.DesertCave.FuckOneSandWitch.TitsWeighOnArms".Localized());
            else
                context.Write("Scenes.DesertCave.FuckOneSandWitch.NipplesSprayMilk".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.CumRisesWithinYou".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.FuckOneSandWitch.PossessiveLetterSFourth".Localized());

            context.Write("Scenes.DesertCave.FuckOneSandWitch.GirlSquealsAtThickening".Localized());

            if (player.CumQ() < 500)
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.FeelShaftWithinHer".Localized());

                if (multi)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.PluralPoundSuffix".Localized());

                context.Write("Scenes.DesertCave.FuckOneSandWitch.ShaftPressesCervix".Localized());
            }
            else if (player.CumQ() < 1500)
                context.Write("Scenes.DesertCave.FuckOneSandWitch.ScreamsAsSpermFills".Localized());
            else if (player.CumQ() < 5000)
                context.Write("Scenes.DesertCave.FuckOneSandWitch.TearsOfJoyStream".Localized());
            else
                context.Write("Scenes.DesertCave.FuckOneSandWitch.HeadShakesFromPleasure".Localized());

            if (player.HasVagina())
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.VaginaTensesClitThrobs".Localized());

                if (player.Wetness() >= 4)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.JuiceSplashesOntoLegs".Localized());

                if (player.Balls > 0)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.SackSoakedFromCum".Localized());
            }

            if (player.CumQ() < 1500)
                context.Write("Scenes.DesertCave.FuckOneSandWitch.PullOutSofteningLoins".Localized());
            else
            {
                context.Write("Scenes.DesertCave.FuckOneSandWitch.PullOutWhileShaft".Localized());

                if (player.Cocks.Count > 1)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.PossessiveLetterSFourth".Localized());

                context.Write("Scenes.DesertCave.FuckOneSandWitch.ContinueWord".Localized());

                if (player.Cocks.Count == 1)
                    context.Write("Scenes.DesertCave.FuckOneSandWitch.PossessiveLetterSFourth".Localized());

                context.Write("Scenes.DesertCave.FuckOneSandWitch.CumRopesSplashBelly".Localized(CoC.Instance.Player.CockDescript(x)));
            }

            player.Orgasm("Generic");

            player.Sensitivity = Math.Max(0, player.Sensitivity - 2);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomLustRoom(context));
        }

        [Register("coc:desert_cave_room_pharmacy")]
        public static void RoomPharmacy(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomPharmacy.PharmacyHeader".Localized());

            context.Write("Scenes.DesertCave.RoomPharmacy.TinyCozyNook".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandwitchThroneUnlocked) == 0)
            {
                context.Write("Scenes.DesertCave.RoomPharmacy.FloorLeverMachinery".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.PullLever".Localized(), () =>
                {
                    CoC.Instance.Flags.Set(GameFlagIds.SandwitchThroneUnlocked, 1);

                    context.Write("Scenes.DesertCave.RoomPharmacy.LeverPulled".Localized());

                    context.Refresh();
                });
            }

            Scenes.Scene.AddButton(1, "Common.Button.BrownPill".Localized(), () => TakeBarrenPills(context));

            Scenes.Scene.AddButton(2, "Common.Button.PinkPill".Localized(), () => TakeFertilePills(context));
        }

        [Register("coc:desert_cave_room_east_hall_one")]
        public static void RoomEastHall1(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomEastHall1.EastHallHeaderWest".Localized());

            context.Write("Scenes.DesertCave.RoomEastHall1.SandstoneTunnelStraight".Localized());
        }

        [Register("coc:desert_cave_room_east_hall_two")]
        public static void RoomEastHall2(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomEastHall2.EastHallHeaderEast".Localized());

            context.Write("Scenes.DesertCave.RoomEastHall2.HallEndsBareWall".Localized());
        }

        [Register("coc:desert_cave_room_sleeping_chamber")]
        public static void RoomSleepingChamber(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomSleepingChamber.SleepingChamberHeader".Localized());

            context.Write("Scenes.DesertCave.RoomSleepingChamber.RowsOfBeds".Localized());
        }

        [Register("coc:desert_cave_room_bathroom")]
        public static void RoomBathroom(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomBathroom.BathRoomHeader".Localized());

            context.Write("Scenes.DesertCave.RoomBathroom.SweetDairyScent".Localized());

            context.Write("Scenes.DesertCave.RoomBathroom.MilkFillsBasin".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MetMilkSlave, 1);

            Scenes.Scene.AddButton(0, "Common.Button.BathTime".Localized(), () => MilkBathsAhoy(context));
        }

        [Register("coc:desert_cave_room_cum_witch_bedroom")]
        public static void RoomCumWitchBedroom(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomCumWitchBedroom.CumWitchBedroomHeader".Localized());

            context.Write("Scenes.DesertCave.RoomCumWitchBedroom.SaltyPungentSmell".Localized());
        }

        [Register("coc:desert_cave_room_cum_witch_office")]
        public static void RoomCumWitchOffice(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomCumWitchOffice.CumWitchOfficeHeader".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) > 0)
            {
                context.Write("Scenes.DesertCave.RoomCumWitchOffice.CumWitchPoundsSister".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Volunteer".Localized(), () => VolunteerConfirmation(context));

                if (CoC.Instance.Flags.Get(GameFlagIds.BeenBlessedByCumWitch) == 0)
                    Scenes.Scene.AddButton(1, "Common.Button.Blessing".Localized(), () => FriendlyCumWitchBlessing(context));
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.CumWitchDefeated) == 0)
            {
                context.Write("Scenes.DesertCave.RoomCumWitchOffice.EnterToSlappingFlesh".Localized());

                context.Write("Scenes.DesertCave.RoomCumWitchOffice.WitchQuiversBeneath".Localized());

                context.Write("Scenes.DesertCave.RoomCumWitchOffice.OddWitchMuses".Localized());

                if (CoC.Instance.Player.HasVagina())
                    context.Write("Scenes.DesertCave.RoomCumWitchOffice.ServeAsMother".Localized());
                else if (CoC.Instance.Player.HasCock())
                    context.Write("Scenes.DesertCave.RoomCumWitchOffice.ServeAsCumPump".Localized());
                else
                    context.Write("Scenes.DesertCave.RoomCumWitchOffice.ServeOtherWays".Localized());

                context.Write("Scenes.DesertCave.RoomCumWitchOffice.SayEnd".Localized());

                context.Write("Scenes.DesertCave.RoomCumWitchOffice.FutanariCastsSpell".Localized());

                context.StartCombat(EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "cum_witch")));
            }
            else
            {
                context.Write("Scenes.DesertCave.RoomCumWitchOffice.RoomReeksOfSpunk".Localized());

                if (CoC.Instance.Player.Lust >= 33)
                {
                    context.Write("Scenes.DesertCave.RoomCumWitchOffice.CouldPullCumWitch".Localized());

                    Scenes.Scene.AddButton(0, "Common.Button.Sex".Localized(), () => CumWitchDefeated(context));
                }
            }
        }

        [Register("coc:desert_cave_room_sacrifical_altar")]
        public static void RoomSacrificalAltar(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomSacrificalAltar.SacrificialAltarHeader".Localized());

            context.Write("Scenes.DesertCave.RoomSacrificalAltar.SacredChamberSignificance".Localized());

            context.Write("Scenes.DesertCave.RoomSacrificalAltar.DoorwaysNorthSouth".Localized());
        }

        [Register("coc:desert_cave_room_sand_mother_throne")]
        public static void RoomSandMotherThrone(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.RoomSandMotherThrone.SandMotherThroneHeader".Localized());

            context.Write("Scenes.DesertCave.RoomSandMotherThrone.SwirlingMagicalLights".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) > 0)
            {
                context.Write("Scenes.DesertCave.SandMotherStuffGOA.InclinesHeadInGreeting".Localized());

                FriendlySandMotherMenu(context);

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.SandMotherDefeated) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.SandMotherVictoryPending) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.SandMotherVictoryPending, 0);

                    SandMotherDefeatMenu(context);

                    return;
                }

                context.Write("Scenes.DesertCave.SandMotherStuffGOA.GlaresButCannotAct".Localized());

                context.Write("Scenes.DesertCave.SandMotherStuffGOA.CringesWaryOfYou".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchLootTaken) >= 5)
                    context.Write("Scenes.DesertCave.SandMotherStuffGOA.StockEmptyWaitRestock".Localized());
                else
                    context.Write("Scenes.DesertCave.SandMotherStuffGOA.UnstoppableTheft".Localized());

                context.Write("Scenes.DesertCave.SandMotherStuffGOA.SentencePeriod".Localized());

                SandMotherPOMenu(context);

                return;
            }

            context.Write("Scenes.DesertCave.SandMotherStuffGOA.QueenReclinesOnThrone".Localized());

            context.Write("Scenes.DesertCave.SandMotherStuffGOA.SteepledFingersQuestion".Localized());

            context.Write(CoC.Instance.Player.Corruption >= 66

                ? "Scenes.DesertCave.SandMotherStuffGOA.SenseSoulNotDemon".Localized()

                : "Scenes.DesertCave.SandMotherStuffGOA.NoCorruptionYetSuspect".Localized());

            context.Write("Scenes.DesertCave.SandMotherStuffGOA.ExplainOrBeScourged".Localized());

            context.Write("Scenes.DesertCave.SandMotherStuffGOA.PoisedToAttackChoose".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Explain".Localized(), () => ExplainYourSelfToZeSandBitch(context));

            Scenes.Scene.AddButton(1, "Common.Button.Fight".Localized(), () => FightTheSandWitchScene(context));
        }

        public static void ExplainYourSelfToZeSandBitch(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.TellsOfIngnamPortal");

            W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.QueenListensClosesEyes");

            W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.QueenJerksUpStaring");

            if (p.Corruption >= 15 + p.Intelligence)
            {
                W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.AdmitsBeingSandMother");

                W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.StepsOutBrandishingScepter");

                W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.NoWayOutFight");

                context.StartCombat(EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "sand_mother")));

                return;
            }

            W("Scenes.DesertCave.ExplainYourSelfToZeSandBitch.TaleRingsTrueAcceptance");

            p.Experience += 200;

            CoC.Instance.Flags.Set(GameFlagIds.SandWitchesFriendly, 1);

            FriendlySandMotherMenu(context);
        }

        public static void TalkDownTheMother(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.TalkDownTheMother.CalmlyDeclaresNoDemon");

            W(p.Corruption < 33

                ? "Scenes.DesertCave.TalkDownTheMother.ChampionWord"

                : p.Corruption < 66

                    ? "Scenes.DesertCave.TalkDownTheMother.BeholdenToNoOne"

                    : "Scenes.DesertCave.TalkDownTheMother.UnstoppableWord");

            W("Scenes.DesertCave.TalkDownTheMother.NoDemonYoke");

            W("Scenes.DesertCave.TalkDownTheMother.EnchantressAdmitsCleverSpy");

            W("Scenes.DesertCave.TalkDownTheMother.AsksWhatYouNeed");

            p.Experience += 200;

            CoC.Instance.Flags.Set(GameFlagIds.SandWitchesFriendly, 1);

            FriendlySandMotherMenu(context);
        }

        public static void SandMotherDefeatMenu(DungeonContext context)
        {
            Scenes.Scene.AddButton(0, "Common.Button.TalkHerDown".Localized(), () => TalkDownTheMother(context));

            if (CoC.Instance.Player.HasVagina())
                Scenes.Scene.AddButton(1, "Common.Button.Scissor".Localized(), () => ScissorAndDrink(context));

            if (CoC.Instance.Player.CockThatFits(50) >= 0)
                Scenes.Scene.AddButton(2, "Common.Button.FuckHer".Localized(), () => FuckTheSandMothersCunt(context));

            if (CoC.Instance.Player.Cocks.Count(c => c.Id.Path == "tentacle") >= 5)
                Scenes.Scene.AddButton(3, "Common.Button.Tentaclefuck".Localized(), () => TentacleGangBang(context));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scenes.Scene.Go<Scenes.Vanilla.CampScene>());
        }

        public static void FriendlySandMotherMenu(DungeonContext context)
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            Scenes.Scene.AddButton(0, "Common.Button.Appearance".Localized(), () => SandMotherAppearance(context));

            Scenes.Scene.AddButton(1, "Common.Button.Talk".Localized(), () => SandMotherTalk(context));

            // 原版 :36-37 友好状态下的药剂买卖
            var labova = Items.ItemRegistry.Create(new MonoCoC.Registry.Identifier("coc", "labova"));

            var lactaid = Items.ItemRegistry.Create(new MonoCoC.Registry.Identifier("coc", "lactaid"));

            Scenes.Scene.AddButton(5,

                "Scenes.DesertCave.FriendlySandMotherMenu.BuyLaBova".Localized(),

                () => BuyPotion(context, new MonoCoC.Registry.Identifier("coc", "labova"), 25),

                labova.DescriptionKey.Localized());

            Scenes.Scene.AddButton(6,

                "Scenes.DesertCave.FriendlySandMotherMenu.BuyLactaid".Localized(),

                () => BuyPotion(context, new MonoCoC.Registry.Identifier("coc", "lactaid"), 50),

                lactaid.DescriptionKey.Localized());

            // 原版 :39-41 释放奴隶
            if ((flags.Get(GameFlagIds.EssyMetInDungeon) > 0 && flags.Get(GameFlagIds.ToldMotherToReleaseEssy) == 0)
                || (flags.Get(GameFlagIds.MetMilkSlave) > 0

                    && !Scenes.SceneInstance<Scenes.Vanilla.Npcs.MilkWaifuScene>.Get().MilkSlave()))

                Scenes.Scene.AddButton(7,

                    "Scenes.DesertCave.FriendlySandMotherMenu.FreeSlaves".Localized(),

                    () => SlavesDiscussion(context),

                    "Scenes.DesertCave.FriendlySandMotherMenu.FreeSlavesHint".Localized());

            if (player.Lust >= 33)
                Scenes.Scene.AddButton(8, "Common.Button.Sex".Localized(), () => SandMotherSexMenu(context));
            else
                Scenes.Scene.AddButtonDisabled(8, "Common.Button.Sex".Localized(),

                    "Scenes.DesertCave.FriendlySandMotherMenu.NotArousedEnough".Localized());

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => RoomSandMotherThrone(context));
        }

        // 原版 :2513 buyPotion()
        public static void BuyPotion(DungeonContext context, MonoCoC.Registry.Identifier itemId, int price)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            if (itemId.Path == "lactaid")
            {
                var politeness = player.Corruption < 33

                    ? "Scenes.DesertCave.BuyPotion.Politely"

                    : player.Corruption < 66

                        ? "Scenes.DesertCave.BuyPotion.Plainly"

                        : "Scenes.DesertCave.BuyPotion.Rudely";

                W("Scenes.DesertCave.BuyPotion.LactaidRequest".Localized(politeness));

                W("Scenes.DesertCave.BuyPotion.LactaidOffer");

                W("Scenes.DesertCave.BuyPotion.LactaidDecline".Localized(price));
            }
            else
            {
                var blush = player.Corruption < 33

                    ? "Scenes.DesertCave.BuyPotion.WithBlush"

                    : player.Corruption < 66

                        ? "Scenes.DesertCave.BuyPotion.Uncertainly"

                        : "Scenes.DesertCave.BuyPotion.KnowingSmile";

                W("Scenes.DesertCave.BuyPotion.LaBovaAsk".Localized(blush));

                W("Scenes.DesertCave.BuyPotion.LaBovaQuestions");

                W(player.Corruption > 66

                    ? "Scenes.DesertCave.BuyPotion.ShakeHeadAlluring"

                    : "Scenes.DesertCave.BuyPotion.ShakeHead");

                W("Scenes.DesertCave.BuyPotion.LaBovaWarn".Localized(price));
            }

            if (player.Gems >= price)
                Scenes.Scene.AddButton(0, "Common.Button.Yes".Localized(), () => BuyPotionConfirm(context, itemId, price));
            else
                Scenes.Scene.AddButtonDisabled(0, "Common.Button.Yes".Localized(),

                    "Scenes.DesertCave.BuyPotion.CannotAfford".Localized());

            Scenes.Scene.AddButton(1, "Common.Button.No".Localized(), () => FriendlySandMotherMenu(context));
        }

        // 原版 :2541 buyPotionConfirm()
        public static void BuyPotionConfirm(DungeonContext context, MonoCoC.Registry.Identifier itemId, int price)
        {
            var player = CoC.Instance.Player;

            player.Gems -= price;

            var item = Items.ItemRegistry.Create(itemId);

            player.Inventory.Add(item, 1);

            Scenes.Scene.ClearOutput();

            context.Write("Scenes.DesertCave.BuyPotionConfirm.Bought".Localized(item.LongName));

            Scenes.Scene.StatScreenRefresh();

            FriendlySandMotherMenu(context);
        }

        // 原版 :2546 slavesDiscussion()
        public static void SlavesDiscussion(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var flags = CoC.Instance.Flags;

            if (flags.Get(GameFlagIds.EssyMetInDungeon) > 0 && flags.Get(GameFlagIds.ToldMotherToReleaseEssy) == 0)
                Scenes.Scene.AddButton(0,

                    "Scenes.DesertCave.SlavesDiscussion.Essrayle".Localized(),

                    () => Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.EssrayleScene>.Get().AskMotherToReleaseEssy(),

                    "Scenes.DesertCave.SlavesDiscussion.EssrayleHint".Localized());

            if (flags.Get(GameFlagIds.MetMilkSlave) > 0
                && !Scenes.SceneInstance<Scenes.Vanilla.Npcs.MilkWaifuScene>.Get().MilkSlave())

                Scenes.Scene.AddButton(1,

                    "Scenes.DesertCave.SlavesDiscussion.MilkSlave".Localized(),

                    () => FreeSlaves(context),

                    "Scenes.DesertCave.SlavesDiscussion.MilkSlaveHint".Localized());

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => FriendlySandMotherMenu(context));
        }

        // 原版 :2560 freeSlaves()
        public static void FreeSlaves(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.FreeSlaves.Intro");

            W("Scenes.DesertCave.FreeSlaves.MotherScoffs");

            if (player.Intelligence <= 20)
            {
                W("Scenes.DesertCave.FreeSlaves.TooDumbToArgue");

                Scenes.Scene.AddNextButton("Common.Button.Next".Localized(),

                    () => Scenes.Scene.Go<Scenes.Vanilla.CampScene>());

                return;
            }

            W("Scenes.DesertCave.FreeSlaves.Lecture");

            W("Scenes.DesertCave.FreeSlaves.WalkToBath");

            W("Scenes.DesertCave.FreeSlaves.SlaveGreets");

            W("Scenes.DesertCave.FreeSlaves.SlaveStruggles");

            W("Scenes.DesertCave.FreeSlaves.MotherExplains");

            Scenes.Scene.AddButton(0, "Common.Button.Yes".Localized(), () => YesDemandMilkRelease(context));

            Scenes.Scene.AddButton(1, "Common.Button.No".Localized(), () => NoDemandMilkRelease(context));

            Scenes.Scene.AddButton(2,

                "Scenes.DesertCave.FreeSlaves.GimmeHer".Localized(),

                () => GimmeTheMilkSlave(context),

                "Scenes.DesertCave.FreeSlaves.GimmeHerHint".Localized());
        }

        // 原版 :2604 yesDemandMilkRelease()
        public static void YesDemandMilkRelease(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            context.Write("Scenes.DesertCave.FreeSlaves.YesDemand".Localized());

            context.EnterRoom("coc:desert_cave_room_bathroom");
        }

        // 原版 :2612 noDemandMilkRelease()
        public static void NoDemandMilkRelease(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            context.Write("Scenes.DesertCave.FreeSlaves.NoDemand".Localized());

            context.EnterRoom("coc:desert_cave_room_sand_mother_throne");
        }

        // 原版 :2619 gimmeDatDeliciousMilkWaifu...
        public static void GimmeTheMilkSlave(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.FreeSlaves.GimmeIntro".Localized());

            if (player.Gems < 2000)
                context.Write("Scenes.DesertCave.FreeSlaves.HaventGotThatMuch".Localized());

            Scenes.Scene.AddButton(0,

                "Scenes.DesertCave.FreeSlaves.TooExpensive".Localized(),

                () => TwoExpensiveForMe(context),

                "Scenes.DesertCave.FreeSlaves.TooExpensiveHint".Localized());

            if (player.Gems >= 2000)
                Scenes.Scene.AddButton(1,

                    "Scenes.DesertCave.FreeSlaves.BuyHer".Localized(),

                    () => BuyHer(context),

                    "Scenes.DesertCave.FreeSlaves.BuyHerHint".Localized());
        }

        // 原版 :2637 TwoExpensive4Me()
        public static void TwoExpensiveForMe(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            context.Write("Scenes.DesertCave.FreeSlaves.TooExpensiveText".Localized());

            context.EnterRoom("coc:desert_cave_room_bathroom");
        }

        // 原版 :2645 BuyHer()
        public static void BuyHer(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            CoC.Instance.Player.Gems -= 2000;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.FreeSlaves.BuyHerHandsGems");

            W("Scenes.DesertCave.FreeSlaves.BuyHerMotherAgrees");

            W("Scenes.DesertCave.FreeSlaves.BuyHerMistressQuestion");

            W("Scenes.DesertCave.FreeSlaves.BuyHerMotherFrees");

            W("Scenes.DesertCave.FreeSlaves.BuyHerMasterQuestion");

            W("Scenes.DesertCave.FreeSlaves.BuyHerYourName");

            Scenes.Scene.AddNextButton("Common.Button.Next".Localized(),

                () => Scenes.SceneInstance<Scenes.Vanilla.Npcs.MilkWaifuScene>.Get().ArriveWithLacticWaifuAtCamp());
        }

        public static void SandMotherAppearance(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var regal = "Scenes.DesertCave.SandMotherAppearance.RegalCovenLeader".Localized()
                .Replace("[if (metric) {250 centimeters|eight feet}]",

                    Configuration.OptionStore.Instance.Get("metric", 0) == 1 ? "Common.Text.Centimeters".Localized() : "Common.Text.EightFeet".Localized());

            context.Write(regal);

            context.Write("Scenes.DesertCave.SandMotherAppearance.PiercingGhostlyGlare".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => FriendlySandMotherMenu(context));
        }

        public static void SandMotherTalk(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchLeaveMeAlone) == 0)
                Scenes.Scene.AddButton(0, "Common.Button.StopAttacking".Localized(), () => UnfriendlyWitchToggle(context));
            else
                Scenes.Scene.AddButton(0, "Common.Button.StartAttacking".Localized(), () => UnfriendlyWitchToggle(context));

            Scenes.Scene.AddButton(1, "Common.Button.SandMothers".Localized(), () => AskHowSandMothersAreChosen(context));

            Scenes.Scene.AddButton(2, "Common.Button.CumWitches".Localized(), () => DiscussCumWitches(context));

            Scenes.Scene.AddButton(3, "Common.Button.CovenStatus".Localized(), () => CurrentStateOfInterwebs(context));

            Scenes.Scene.AddButton(4, "Common.Button.History".Localized(), () => SandWitchHistory(context));

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => FriendlySandMotherMenu(context));
        }

        public static void AskHowSandMothersAreChosen(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.AskHowChosen");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.PoliticsOfCourse");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.HumanSuffixPresent");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.HumanSuffixPast");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.GangsAndCliques");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.WhenCovenGrowsBig");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.AskAboutRivalAnger");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.DiscontentCantFester");

            W("Scenes.DesertCave.AskHowSandMothersAreChosen.ThankAndMull");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherTalk(context));
        }

        public static void SandWitchHistory(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.SandWitchHistory.InquireWitchHistory");

            W("Scenes.DesertCave.SandWitchHistory.MotherFrownsRecallsPast");

            W("Scenes.DesertCave.SandWitchHistory.AskAboutFactions");

            W("Scenes.DesertCave.SandWitchHistory.StorytellerTellsFlight");

            W("Scenes.DesertCave.SandWitchHistory.MusingsOnRefugees");

            W("Scenes.DesertCave.SandWitchHistory.RegalWitchDeniesInbreeding");

            W("Scenes.DesertCave.SandWitchHistory.HandDipsToBelly");

            W("Scenes.DesertCave.SandWitchHistory.AskHowTheyResist");

            W("Scenes.DesertCave.SandWitchHistory.BeesExplainPurity");

            W("Scenes.DesertCave.SandWitchHistory.AskToDistillHoney");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherTalk(context));
        }

        public static void CurrentStateOfInterwebs(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.CurrentStateOfInterwebs.InquireCovenState");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.SituationGoodOnWhole");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.WeAreManyIndeed");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.MightyForceIndeed");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.NotIdleWhileWaiting");

            W(CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0

                ? "Scenes.DesertCave.CurrentStateOfInterwebs.WaitOnAlliesGrowing"

                : "Scenes.DesertCave.CurrentStateOfInterwebs.SeekAerialAllies");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.SayEnd");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.AskBattleSuitability");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.TheyWontKillYou");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.SummaryWeAreMighty");

            if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) > 0)
                W("Scenes.DesertCave.CurrentStateOfInterwebs.BreachCitadelWalls");

            W("Scenes.DesertCave.CurrentStateOfInterwebs.AskIfMoreQuestions");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherTalk(context));
        }

        public static void DiscussCumWitches(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.DiscussCumWitches.AskWhyCumWitches");

            W("Scenes.DesertCave.DiscussCumWitches.OrderAroseFromNeed");

            W("Scenes.DesertCave.DiscussCumWitches.MotherFidgetsBlushing");

            W("Scenes.DesertCave.DiscussCumWitches.ExplainGruelingTrials");

            W("Scenes.DesertCave.DiscussCumWitches.SoundsGruelingRemark");

            if (CoC.Instance.Player.Corruption + CoC.Instance.Player.Libido > 100)
                W("Scenes.DesertCave.DiscussCumWitches.PotentiallyFunClause");

            W("Scenes.DesertCave.DiscussCumWitches.AskSheTriedOut");

            W("Scenes.DesertCave.DiscussCumWitches.SheTriedOutToo");

            W("Scenes.DesertCave.DiscussCumWitches.AlreadyConvincedHer");

            if (CoC.Instance.Flags.Get(GameFlagIds.MoreCumWitches) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.CumWitchesFightable) == 0)
                {
                    W("Scenes.DesertCave.DiscussCumWitches.SendThemHunting");

                    Scenes.Scene.AddButton(0, "Common.Button.SendOut".Localized(), () => SendOutCumWitch(context));
                }
                else
                {
                    W("Scenes.DesertCave.DiscussCumWitches.WitchesProwlSands");

                    Scenes.Scene.AddButton(0, "Common.Button.KeepThemIn".Localized(), () => KeepCumWitchesIn(context));
                }
            }
            else
            {
                W("Scenes.DesertCave.DiscussCumWitches.AskToBolsterNumbers");

                Scenes.Scene.AddButton(0, "Common.Button.BolsterThem".Localized(), () => MoreCumWitchesPlease(context));
            }

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => SandMotherTalk(context));
        }

        public static void SendOutCumWitch(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.SendOutCumWitch.RequestSendThemOut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CumWitchesFightable, 1);

            SendOutOrKeepInEnding(context);
        }

        public static void KeepCumWitchesIn(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.KeepCumWitchesIn.RequestHoldThemBack".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CumWitchesFightable, 0);

            SendOutOrKeepInEnding(context);
        }

        public static void SendOutOrKeepInEnding(DungeonContext context)
        {
            context.Write("Scenes.DesertCave.SendOutOrKeepInEnding.HonorRequestForNow".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MoreCumWitches, 1);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherTalk(context));
        }

        public static void MoreCumWitchesPlease(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.MoreCumWitchesPlease.WishForMoreWitches");

            W("Scenes.DesertCave.MoreCumWitchesPlease.MotherRefusesOutright");

            W("Scenes.DesertCave.MoreCumWitchesPlease.AskIfMoreHumane");

            W("Scenes.DesertCave.MoreCumWitchesPlease.DuneMotherProtests");

            W("Scenes.DesertCave.MoreCumWitchesPlease.EfficientButDegrading");

            W("Scenes.DesertCave.MoreCumWitchesPlease.MatriarchFlabbergasted");

            W("Scenes.DesertCave.MoreCumWitchesPlease.WouldntHurtToAsk");

            W("Scenes.DesertCave.MoreCumWitchesPlease.MotherSummonsCumWitch");

            W("Scenes.DesertCave.MoreCumWitchesPlease.HermaphroditeQuestions");

            W("Scenes.DesertCave.MoreCumWitchesPlease.CovenLeaderExplainsIdea");

            W("Scenes.DesertCave.MoreCumWitchesPlease.CumWitchDelighted");

            W("Scenes.DesertCave.MoreCumWitchesPlease.AnnounceTrialsAtOnce");

            W("Scenes.DesertCave.MoreCumWitchesPlease.NewCumWitchesNotice");

            CoC.Instance.Flags.Set(GameFlagIds.MoreCumWitches, 1);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherTalk(context));
        }

        public static void SandMotherCowedSexMenu(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (CoC.Instance.Player.CockThatFits(50) >= 0)
                Scenes.Scene.AddButton(3, "Common.Button.FuckHer".Localized(), () => FuckTheSandMothersCunt(context));

            if (CoC.Instance.Player.Cocks.Count(c => c.Id.Path == "tentacle") >= 5)
                Scenes.Scene.AddButton(2, "Common.Button.TentacleBang".Localized(), () => TentacleGangBang(context));

            if (CoC.Instance.Player.HasVagina())
                Scenes.Scene.AddButton(1, "Common.Button.Scissor".Localized(), () => ScissorAndDrink(context));

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => SandMotherPOMenu(context));
        }

        public static void TentacleGangBang(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var tentacles = p.Cocks.Count(c => c.Id.Path == "tentacle");

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.TentacleGangBang.DiscardArmorWithGlee");

            W(flags.Get(GameFlagIds.TimesTentacledSandMother) == 0

                ? "Scenes.DesertCave.TentacleGangBang.CannotEndThisWay"

                : "Scenes.DesertCave.TentacleGangBang.NotAgain");

            W("Scenes.DesertCave.TentacleGangBang.SayEnd");

            W(flags.Get(GameFlagIds.TimesTentacledSandMother) == 0

                ? "Scenes.DesertCave.TentacleGangBang.AssumesYouServeDemons"

                : "Scenes.DesertCave.TentacleGangBang.AssumesSheHasSay");

            W("Scenes.DesertCave.TentacleGangBang.NoSenseCorrectingHer");

            W("Scenes.DesertCave.TentacleGangBang.FirstTentacleFindsSkin");

            W("Scenes.DesertCave.TentacleGangBang.GaspsWrappedInCocks");

            W("Scenes.DesertCave.TentacleGangBang.BoundQueenOnDisplay");

            W("Scenes.DesertCave.TentacleGangBang.TwinTentaclesThrustTogether");

            W("Scenes.DesertCave.TentacleGangBang.ToolPastGlossedLips");

            W("Scenes.DesertCave.TentacleGangBang.FifthCockSlidesDown");

            if (tentacles >= 6)
            {
                W("Scenes.DesertCave.TentacleGangBang.YouStillHave");

                W(tentacles >= 7

                    ? "Scenes.DesertCave.TentacleGangBang.UntendedShaftsRemainPlural"

                    : "Scenes.DesertCave.TentacleGangBang.UntendedShaftRemainsSingular");

                W("Scenes.DesertCave.TentacleGangBang.AllHolesFull");

                W(tentacles == 6

                    ? "Scenes.DesertCave.TentacleGangBang.LeftoverOneSingular"

                    : "Scenes.DesertCave.TentacleGangBang.LeftoverOnesPlural");

                W("Scenes.DesertCave.TentacleGangBang.ThreadThroughHerFingers");

                W(tentacles == 6

                    ? "Scenes.DesertCave.TentacleGangBang.HeldStillHandjobOne"

                    : "Scenes.DesertCave.TentacleGangBang.HeldStillHandjobTwo");

                W("Scenes.DesertCave.TentacleGangBang.PartRelaxesIntoWank");
            }

            if (tentacles >= 10)
                W("Scenes.DesertCave.TentacleGangBang.SevenMembersAccounted");

            W("Scenes.DesertCave.TentacleGangBang.LeansOverPreyAdmiring");

            W("Scenes.DesertCave.TentacleGangBang.BulgesRiseUpShafts");

            if (tentacles >= 6)
                W("Scenes.DesertCave.TentacleGangBang.SeedSoaksJerkingHands");

            W("Scenes.DesertCave.TentacleGangBang.FirstExplosionsOfRelief");

            W("Scenes.DesertCave.TentacleGangBang.PullOutCumDeluge");

            if (flags.Get(GameFlagIds.SandWitchesCowed) == 0)
            {
                W("Scenes.DesertCave.TentacleGangBang.WitchesCowedPeaceRuined");

                flags.Set(GameFlagIds.SandWitchesCowed, 1);
            }
            else
                W("Scenes.DesertCave.TentacleGangBang.QueenSlutNeverOld");

            flags.Set(GameFlagIds.TimesTentacledSandMother, flags.Get(GameFlagIds.TimesTentacledSandMother) + 1);

            p.Orgasm("Generic");

            p.Corruption += 1;

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherCowedSexMenu(context));
        }

        public static void FuckTheSandMothersCunt(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var x = p.CockThatFits(50);

            var y = p.CockThatFits2(50);

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.FuckTheSandMothersCunt.AdmirePrizeDisrobe");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.BootyPoppinPraise");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.SecondSlapOtherSide");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.QueenBegsToFinish");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.MocksProtestedDenial");

            W(p.Tallness >= 90

                ? "Scenes.DesertCave.FuckTheSandMothersCunt.NapeOfNeckSpot"

                : "Scenes.DesertCave.FuckTheSandMothersCunt.OnHerStomachSpot");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.CupsSwollenMammaries");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.GrabAssFondlePussy");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.SecondCockBothHoles");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.LabiaCaressCockheads");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.CockheadsWord");
            else
                context.Write(p.CockHead(x));

            W("Scenes.DesertCave.FuckTheSandMothersCunt.FreezesThenPullsAway");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.DragHipsDownPenetrate");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.ToolsWord");
            else
                context.Write(p.CockDescript(x));

            W("Scenes.DesertCave.FuckTheSandMothersCunt.MoansTossingBleachedHair");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.SLetterFifth");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.HipsClapAgainstButt");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.StatuesqueLadyIsPutty");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.EsLetterAgain");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.PussyQuiversAnticipation");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.MotherBegsForMore");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.DemandsBitchStyleBegging");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.CockHardensAtDebasement");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.RuttingRoughlyNoCare");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.ThrustsClapLoinsErupt");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.SemenFloodsCreamsCunt");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.SLetterFifth");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.AsWord");

            W(y < 0

                ? "Scenes.DesertCave.FuckTheSandMothersCunt.ClampsDownIt"

                : "Scenes.DesertCave.FuckTheSandMothersCunt.ClampDownThey");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.FemlubeSpraysAcrossFeet");

            if (p.CumQ() >= 500)
            {
                W("Scenes.DesertCave.FuckTheSandMothersCunt.FloodsHerTwatRepeatedly");

                if (y >= 0)
                    W("Scenes.DesertCave.FuckTheSandMothersCunt.SLetterFifth");

                W("Scenes.DesertCave.FuckTheSandMothersCunt.CumBubblesOntoGround");

                if (p.CumQ() >= 2000)
                    W("Scenes.DesertCave.FuckTheSandMothersCunt.GlobsFallBellySwells");
            }

            W("Scenes.DesertCave.FuckTheSandMothersCunt.EveryOuncePumpedIn");

            if (y >= 0)
                W("Scenes.DesertCave.FuckTheSandMothersCunt.SLetterFifth");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.AdmiresSaltyMark");

            W("Scenes.DesertCave.FuckTheSandMothersCunt.SighsSlumpsInPuddles");

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed) == 0)
            {
                W("Scenes.DesertCave.FuckTheSandMothersCunt.WitchesCowedPeaceRuined");

                CoC.Instance.Flags.Set(GameFlagIds.SandWitchesCowed, 1);
            }
            else
                W("Scenes.DesertCave.FuckTheSandMothersCunt.QueenSlutNeverOld");

            p.Orgasm("Dick");

            p.Corruption += 1;

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherCowedSexMenu(context));
        }

        public static void ScissorAndDrink(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.ScissorAndDrink.HeatStirsObservingHer");

            W("Scenes.DesertCave.ScissorAndDrink.GrowlSqueezeHeavyChest");

            W("Scenes.DesertCave.ScissorAndDrink.QueenPantsWontBreak");

            W("Scenes.DesertCave.ScissorAndDrink.GropesTopTitsMilk");

            W("Scenes.DesertCave.ScissorAndDrink.TauntsMilkProductivity");

            W("Scenes.DesertCave.ScissorAndDrink.GuidesHandToArmor");

            if (p.Tallness < 85)
                W("Scenes.DesertCave.ScissorAndDrink.LargerWord");
            else if (p.Tallness > 105)
                W("Scenes.DesertCave.ScissorAndDrink.SmallerWord");

            W("Scenes.DesertCave.ScissorAndDrink.SlickBodiesEntwined");

            W("Scenes.DesertCave.ScissorAndDrink.NipsHerLowerLip");

            W("Scenes.DesertCave.ScissorAndDrink.SteamingDoubledLabia");

            W("Scenes.DesertCave.ScissorAndDrink.VulvaGrindsTwinTwats");

            W("Scenes.DesertCave.ScissorAndDrink.MilkFlowIntensifies");

            if (p.Wetness() >= 5)
                W("Scenes.DesertCave.ScissorAndDrink.ExplosiveMilkSpray");
            else if (p.Wetness() >= 4)
                W("Scenes.DesertCave.ScissorAndDrink.HeavyMilkGush");
            else if (p.Wetness() >= 3)
                W("Scenes.DesertCave.ScissorAndDrink.ThickMilkPour");
            else if (p.Wetness() >= 2)
                W("Scenes.DesertCave.ScissorAndDrink.SteadyMilkDrool");
            else
                W("Scenes.DesertCave.ScissorAndDrink.MilkDripDuringClimax");

            W("Scenes.DesertCave.ScissorAndDrink.CheeksBulgeWithMilk");

            p.RefillHunger(40);

            W("Scenes.DesertCave.ScissorAndDrink.ArmsPressFaceChest");

            W("Scenes.DesertCave.ScissorAndDrink.SeparatesFromDrainedTeat");

            W("Scenes.DesertCave.ScissorAndDrink.MotherSighsInPuddles");

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed) == 0)
            {
                W("Scenes.DesertCave.ScissorAndDrink.WitchesCowedPeaceRuined");

                CoC.Instance.Flags.Set(GameFlagIds.SandWitchesCowed, 1);
            }
            else
                W("Scenes.DesertCave.ScissorAndDrink.QueenSlutNeverOld");

            p.Orgasm("Vaginal");

            p.Corruption += 1;

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherCowedSexMenu(context));
        }

        public static void SandMotherSexMenu(DungeonContext context)
        {
            Scenes.Scene.AddButton(0, "Common.Button.FuckHer".Localized(), () => FriendlySandMotherFuck(context));

            Scenes.Scene.AddButton(1, "Common.Button.GetMilkFill".Localized(), () => LesboMilkFilling(context));

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => FriendlySandMotherMenu(context));
        }

        public static void TakeBarrenPills(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (CoC.Instance.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Contraceptives))
                context.Write("Scenes.DesertCave.TakeBarrenPills.AlreadyContracepted".Localized());
            else
            {
                context.Write("Scenes.DesertCave.TakeBarrenPills.SwallowBarrenPill".Localized());

                if (CoC.Instance.Player.IsPregnant())
                    context.Write("Scenes.DesertCave.TakeBarrenPills.PregnancyUnaffected".Localized());

                context.Write("Scenes.DesertCave.TakeBarrenPills.EmptinessConfirms".Localized());

                if (!CoC.Instance.Player.HasVagina())
                    context.Write("Scenes.DesertCave.TakeBarrenPills.FineIfGrowVagina".Localized());

                CoC.Instance.Player.CreateStatusEffect(Datas.Vanilla.StatusEffectIds.Contraceptives);
            }

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomPharmacy(context));
        }

        public static void TakeFertilePills(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (!CoC.Instance.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Contraceptives))
                context.Write("Scenes.DesertCave.TakeFertilePills.NoContraceptiveToCancel".Localized());
            else
            {
                context.Write("Scenes.DesertCave.TakeFertilePills.PillCancelsContraceptive".Localized());

                if (!CoC.Instance.Player.HasVagina())
                    context.Write("Scenes.DesertCave.TakeFertilePills.IfYouGrowVagina".Localized());

                CoC.Instance.Player.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Contraceptives);
            }

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomPharmacy(context));
        }

        public static void DrinkNFap(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.DrinkNFap.CallEbonyWoman".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.BathSlutBegsMilk".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.LickNippleTaste".Localized());

            if (player.HasVagina())
                context.Write("Scenes.DesertCave.DrinkNFap.StrokeVulvaIdly".Localized());
            else
                context.Write("Scenes.DesertCave.DrinkNFap.StrokeCockIdly".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.LewdMoanDrawnOut".Localized());

            if (player.HasCock())
                context.Write("Scenes.DesertCave.DrinkNFap.CockSwellingRevealed".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.SentencePeriod".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.ExcitedMoanWormsOut".Localized());

            if (player.Gender == Creatures.Gender.Hermaphrodite)
                context.Write("Scenes.DesertCave.DrinkNFap.CockThrobsInHand".Localized());
            else if (player.Gender == Creatures.Gender.Male)
                context.Write("Scenes.DesertCave.DrinkNFap.CockThrobsHarder".Localized());

            if (player.Gender == Creatures.Gender.Hermaphrodite)
            {
                context.Write("Scenes.DesertCave.DrinkNFap.FondleEachMember".Localized());

                context.Write("Scenes.DesertCave.DrinkNFap.OffHandRubsGash".Localized());

                context.Write("Scenes.DesertCave.DrinkNFap.PussyJuicesMixMilk".Localized());
            }
            else if (player.Gender == Creatures.Gender.Female)
                context.Write("Scenes.DesertCave.DrinkNFap.PussyJuicesMixMilk".Localized());
            else
            {
                context.Write("Scenes.DesertCave.DrinkNFap.FondleMembersRhythm".Localized());

                context.Write("Scenes.DesertCave.DrinkNFap.CradleSackFeelBalls".Localized());
            }

            context.Write("Scenes.DesertCave.DrinkNFap.MilkSprayHitsShoulders".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.TubOverflowsTide".Localized());

            if (player.HasCock())
            {
                context.Write("Scenes.DesertCave.DrinkNFap.CocksReleaseLoad".Localized());

                context.Write("Scenes.DesertCave.DrinkNFap.SeedTorrentLiftsCocks".Localized());

                context.Write("Scenes.DesertCave.DrinkNFap.TubContentsStainGirl".Localized());
            }

            if (player.HasVagina())
                context.Write("Scenes.DesertCave.DrinkNFap.CunnyClenchesInside".Localized());

            context.Write("Scenes.DesertCave.DrinkNFap.LowCooContentment".Localized());

            player.ChangeFatigue(-15);

            player.RefillHunger(30);

            player.Orgasm("Generic");

            player.Sensitivity = Math.Max(0, player.Sensitivity - 3);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomBathroom(context));
        }

        public static void MilkBathsAhoy(DungeonContext context)
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.MilkBathsAhoy.QuickBathDecision".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.SlaveReachesNipples".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.MilkFillsTub".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.MassageMilkIntoSkin".Localized());

            context.Write(player.HasCock()

                ? "Scenes.DesertCave.MilkBathsAhoy.CockToken".Localized()

                : player.HasVagina()

                    ? "Scenes.DesertCave.MilkBathsAhoy.VaginaToken".Localized()

                    : "Scenes.DesertCave.MilkBathsAhoy.ButtToken".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.ResistTouchingYourself".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.EbonyWomanLollsTongue".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.MilkSlavePullsBack".Localized());

            context.Write("Scenes.DesertCave.MilkBathsAhoy.CouldMasturbateInTub".Localized());

            player.ChangeFatigue(-10);

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => FinishMilkBath(context));

            if (player.Gender == Creatures.Gender.Genderless)
                Scenes.Scene.AddButtonDisabled(1, "DrinkNFap", "This scene requires you to have genitals.");
            else
                Scenes.Scene.AddButton(1, "Common.Button.Drinknfap".Localized(), () => DrinkNFap(context));

            Scenes.Scene.AddButton(2, "Common.Button.MilkGirl".Localized(), () => GrabTheMilkGirl(context));
        }

        public static void FinishMilkBath(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.FinishMilkBath.SoakInTub".Localized());

            var player = CoC.Instance.Player;

            player.Lust = Math.Min(player.MaxLust, player.Lust + 10);

            player.ChangeFatigue(-50);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomBathroom(context));
        }

        public static void VolunteerConfirmation(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.VolunteerConfirmation.SureWantVolunteer".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Yes".Localized(), () => ReallyVolunteer(context));

            Scenes.Scene.AddButton(1, "Common.Button.No".Localized(), () => RoomCumWitchOffice(context));
        }

        public static void ReallyVolunteer(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            if (player.HasCock() && (player.Gender == Creatures.Gender.Male || Random.Shared.Next(2) == 0))
            {
                CumWitchCumPumpBadEnd(context, true);

                return;
            }

            TurnIntoASammitch(context, true);
        }

        public static void CumWitchCumPumpBadEnd(DungeonContext context, bool volunteered)
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (!volunteered)
            {
                context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.LaidLowBy".Localized());

                context.Write(player.Health < 1

                    ? "Scenes.DesertCave.CumWitchCumPumpBadEnd.YourWounds".Localized()

                    : "Scenes.DesertCave.CumWitchCumPumpBadEnd.LustCoursingVeins".Localized());

                context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.SlumpAgainstDesk".Localized());

                context.Write(player.Health < 1

                    ? "Scenes.DesertCave.CumWitchCumPumpBadEnd.StruggleToRise".Localized()

                    : "Scenes.DesertCave.CumWitchCumPumpBadEnd.StruggleToGetCrotch".Localized());

                context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.SentencePeriod".Localized());
            }
            else
                context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.TellWitchVolunteer".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.RobedBeautyWhispers".Localized(CoC.Instance.Player.Mf("male", "hermaphroditic")));

            if (player.CumQ() > 1000)
                context.Write(player.Balls > 0

                    ? "Scenes.DesertCave.CumWitchCumPumpBadEnd.VirileSwollenTestes".Localized()

                    : "Scenes.DesertCave.CumWitchCumPumpBadEnd.VirileBodyBreeder".Localized());
            else
                context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.PatheticVirility".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.MagicSuitsBreeding".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.SlenderFingersTrailing".Localized());

            context.Write(player.Health < 1

                ? "Scenes.DesertCave.CumWitchCumPumpBadEnd.GruntAndStrain".Localized()

                : "Scenes.DesertCave.CumWitchCumPumpBadEnd.GroanTouchYourself".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.RufflesHairAndTuts".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.RollsUpSleeves".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.SnappingForceBands".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.SoftTouchesTrace".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.RubsBackUpHips".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.CaptorInstructsRelax".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.WitchWhispersRelaxed".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.StrainToStayAwake".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.PlacidFeelingsFill".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.WaitWhatBrowFurrows".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.SparksGlimmerFlicker".Localized());

            context.Write("Scenes.DesertCave.CumWitchCumPumpBadEnd.LectureOfInstructions".Localized());

            player.Orgasm("Generic");

            player.Libido += 100;

            player.Sensitivity += 100;

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => BeACumPumpPartII(context));
        }

        public static void BeACumPumpPartII(DungeonContext context)
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.BeACumPumpPartII.SnapAndYawn".Localized());

            if (player.Balls == 0)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.WaitNeverHadBalls".Localized(CoC.Instance.Player.Mf("stud", "cum donor")));
            else if (player.CumQ() < 3000)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.GentleShakeAndSwell".Localized());
            else
                context.Write("Scenes.DesertCave.BeACumPumpPartII.GentlePatPerfection".Localized());

            if (player.Balls < 2)
                player.Balls = 2;

            context.Write("Scenes.DesertCave.BeACumPumpPartII.LustyWomanExhales".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.SLetterAgain".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.WickedWitchReleases".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.NaggingWorriesSurge".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.FamiliarWomanApproaches".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.WarinessYouSit".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.EsLetter".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.WiltingFromSurprise".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.ExplainsInitiation".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.NaughtyPumpScolding".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.ComeToSmileHappy".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.BlondeBombClearsThroat".Localized(CoC.Instance.Player.Mf("wizard", "witch")));

            context.Write("Scenes.DesertCave.BeACumPumpPartII.MistressHandsBook".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.FirstPartComplete".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.TeacherPushesWitch".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.ClosesEyesChant".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.NearlyCavesIn".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.CriesStopBoner".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.SLetterAgain".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.SurprisedByYourWill".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.OliveSkinnedBeauty".Localized());

            if (player.SmallestCockArea() >= 100)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.EasedByShrinking".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.HugeTitsCrushFace".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.BeACumPumpPartII.ButtSmearedWithGoo".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.SurprisingRidingSkill".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.BlowsInSeconds".Localized());

            context.Write("Scenes.DesertCave.BeACumPumpPartII.SputteringWitchThanks".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => CumPumpVolumeThree(context));
        }

        public static void CumPumpVolumeThree(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.CumPumpVolumeThree.ChampionBrainwashed".Localized());

            context.Write(CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2

                ? "Scenes.DesertCave.CumPumpVolumeThree.DestroyingWord".Localized()

                : "Scenes.DesertCave.CumPumpVolumeThree.RescuingWord".Localized());

            context.Write("Scenes.DesertCave.CumPumpVolumeThree.MaraeFamiliarityDemons".Localized());

            Scenes.Scene.GameOver();
        }

        public static void TurnIntoASammitch(DungeonContext context, bool volunteered)
        {
            var player = CoC.Instance.Player;

            if (!volunteered)
            {
                context.Write("Scenes.DesertCave.TurnIntoASammitch.HelplesslyYou".Localized());

                if (player.Lust >= player.MaxLust)
                    context.Write(player.HasVagina()

                        ? "Scenes.DesertCave.TurnIntoASammitch.DigHandsIntoLoins".Localized()

                        : "Scenes.DesertCave.TurnIntoASammitch.DigFingerIntoAsshole".Localized());
                else
                    context.Write("Scenes.DesertCave.TurnIntoASammitch.TryToRiseFails".Localized());

                context.Write("Scenes.DesertCave.TurnIntoASammitch.SorceressReleasesStaff".Localized());

                context.Write("Scenes.DesertCave.TurnIntoASammitch.QuietVoiceBarelyHeard".Localized());

                context.Write("Scenes.DesertCave.TurnIntoASammitch.CaptorFlashesSmile".Localized());
            }
            else
                context.Write("Scenes.DesertCave.TurnIntoASammitch.SorceressSmilesGiggles".Localized());

            if (!player.IsPregnant())
                context.Write("Scenes.DesertCave.TurnIntoASammitch.GetFirstDaughter".Localized());
            else
                context.Write("Scenes.DesertCave.TurnIntoASammitch.HaveFunWhileWaiting".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.PlentyOfTimeWaiting".Localized());

            if (!player.HasVagina())
            {
                context.Write("Scenes.DesertCave.TurnIntoASammitch.GesturesSalmonLight".Localized());

                player.CreateVagina();

                player.SetClitLength(0.25f);
            }

            context.Write("Scenes.DesertCave.TurnIntoASammitch.FutanariStrokes".Localized());

            if (player.Lust / player.MaxLust < 0.65f)
                context.Write("Scenes.DesertCave.TurnIntoASammitch.StrokesToFullHardness".Localized());
            else if (player.Lust / player.MaxLust < 0.85f)
                context.Write("Scenes.DesertCave.TurnIntoASammitch.CumDrizzlesFromTip".Localized());
            else
                context.Write("Scenes.DesertCave.TurnIntoASammitch.GatherDrippingCum".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.LicksLipsInAnticipation".Localized());

            if (player.Lust >= player.MaxLust && player.HasVagina() && !player.Vaginas[0].Virgin)
                context.Write("Scenes.DesertCave.TurnIntoASammitch.HandKnuckleDeep".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.SentencePeriod".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.KneelingBeforeHelpless".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.SpellLikeGunshot".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.SorceressAsksBetter".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.WitchMovesYourHand".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.CooingGreatSister".Localized());

            if (player.BreastRows.Count < 2)
                context.Write("Scenes.DesertCave.TurnIntoASammitch.FourBigMilkyTits".Localized());
            else if (player.LactationQ() < 200)
                context.Write("Scenes.DesertCave.TurnIntoASammitch.PerfectMilkers".Localized());
            else
                context.Write("Scenes.DesertCave.TurnIntoASammitch.FourBigTitsMilked".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.FirstWitchCumDose".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.GigglingAgreeOnBench".Localized());

            if (player.IsPregnant())
                context.Write("Scenes.DesertCave.TurnIntoASammitch.ForgotAlreadyPregnant".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.ThinkingGetsYouWet".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.WitchSlapsBottom".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.TwoHugeTitsRest".Localized());

            if (player.HasCock())
                context.Write("Scenes.DesertCave.TurnIntoASammitch.CockThrobsBeneath".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.BulgingNutsOnButt".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.WitchStrokesHair".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.ShiversNowImpaled".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.SpoogeBubblesUp".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.ExpectsOrgasmPullsOut".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.SoftPebblyTexture".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.StayJoinedAtHips".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.OtherWomanConfused".Localized());

            context.Write("Scenes.DesertCave.TurnIntoASammitch.PlacatesFourBreasted".Localized());

            player.Orgasm("Vaginal");

            player.Libido += 100;

            player.Sensitivity += 100;

            Scenes.Scene.ClearButtons();

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => ChicksLoseToSandWitchesBadEndFinale(context));
        }

        public static void ChicksLoseToSandWitchesBadEndFinale(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.ChicksLoseToSandWitchesBadEndFinale.SomeTimeLater".Localized());

            context.Write("Scenes.DesertCave.ChicksLoseToSandWitchesBadEndFinale.RubHandsAcrossWomb".Localized());

            context.Write("Scenes.DesertCave.ChicksLoseToSandWitchesBadEndFinale.SwallowCreamySpunk".Localized());

            context.Write("Scenes.DesertCave.ChicksLoseToSandWitchesBadEndFinale.MassageQuakingTestes".Localized());

            Scenes.Scene.GameOver();
        }

        public static void FriendlyCumWitchBlessing(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.AskForBlessing".Localized());

            context.Write(player.Corruption < 33

                ? "Scenes.DesertCave.FriendlyCumWitchBlessing.BlushingLead".Localized()

                : player.Corruption < 66

                    ? "Scenes.DesertCave.FriendlyCumWitchBlessing.NervousLead".Localized()

                    : "Scenes.DesertCave.FriendlyCumWitchBlessing.BoldLead".Localized());

            context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.RemoveArmorAnticipating".Localized());

            context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.SorceressCirclesYou".Localized());

            if (player.Gender == Creatures.Gender.Genderless)
            {
                context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.LackingGenderRefusal".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomCumWitchOffice(context));

                return;
            }

            if (player.Gender == Creatures.Gender.Male)
                context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.BlessEnhancedVolume".Localized());
            else if (player.Gender == Creatures.Gender.Female)
                context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.BlessSuperhumanFertility".Localized());
            else
                context.Write("Scenes.DesertCave.FriendlyCumWitchBlessing.OfferHermaphroditeChoice".Localized());

            if (player.HasCock() && !player.HasPerk(Datas.Vanilla.PerkIds.MagicalVirility))
                Scenes.Scene.AddButton(0, "CharCreation.AscensionPerkMenu.Button8".Localized(), () => CumWitchBlessed(context, true));
            else
                Scenes.Scene.AddButtonDisabled(0, "Virility", "You already possess this blessing.");

            if (player.HasVagina() && !player.HasPerk(Datas.Vanilla.PerkIds.MagicalFertility))
                Scenes.Scene.AddButton(1, "CharCreation.AscensionPerkMenu.Button3".Localized(), () => CumWitchBlessed(context, false));
            else
                Scenes.Scene.AddButtonDisabled(1, "Fertility", "You already possess this blessing.");

            Scenes.Scene.AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => ChangeMindAboutBlessings(context));
        }

        public static void ChangeMindAboutBlessings(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.ChangeMindAboutBlessings.DeclineAndDress".Localized());

            context.Write("Scenes.DesertCave.ChangeMindAboutBlessings.SeductressDisappointed".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomCumWitchOffice(context));
        }

        public static void CumWitchBlessed(DungeonContext context, bool virility)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.CumWitchBlessed.KneelAtPerfectHeight".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.FondleHeavyPackage".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.AdmireThenLick".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.ThroatItPlunges".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.LookUpInnocently".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.HumOfAssent".Localized());

            if (player.Body.Get<BodyParts.Vanilla.TonguePart>().Id.Path != "human")
                context.Write("Scenes.DesertCave.CumWitchBlessed.TongueFollowsUrethra".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.AnnouncesBlessingIncoming".Localized());

            if (virility)
                context.Write("Scenes.DesertCave.CumWitchBlessed.DripYourOwnSeed".Localized());
            else
            {
                context.Write("Scenes.DesertCave.CumWitchBlessed.YourVaginaPrefix".Localized());

                if (player.Wetness() < 3)
                    context.Write("Scenes.DesertCave.CumWitchBlessed.DripWord".Localized());
                else if (player.Wetness() < 4)
                    context.Write("Scenes.DesertCave.CumWitchBlessed.DroolWord".Localized());
                else if (player.Wetness() < 5)
                    context.Write("Scenes.DesertCave.CumWitchBlessed.LeakWord".Localized());
                else
                    context.Write("Scenes.DesertCave.CumWitchBlessed.SprayWord".Localized());

                context.Write("Scenes.DesertCave.CumWitchBlessed.SecretionsHitGround".Localized());
            }

            context.Write("Scenes.DesertCave.CumWitchBlessed.TipOpensHolyFluid".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.FlopBackHosedDown".Localized());

            context.Write(virility

                ? "Scenes.DesertCave.CumWitchBlessed.BallsToken".Localized()

                : "Scenes.DesertCave.CumWitchBlessed.WombWord".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.GotWhatYouAsked".Localized());

            context.Write("Scenes.DesertCave.CumWitchBlessed.BoldTagOpen".Localized());

            if (virility)
            {
                if (!player.HasPerk(Datas.Vanilla.PerkIds.MagicalVirility))
                {
                    context.Write("Scenes.DesertCave.CumWitchBlessed.VirilityUnlockNotice".Localized());

                    player.CreatePerk(Datas.Vanilla.PerkIds.MagicalVirility);
                }
                else if (player.Perkv1(Datas.Vanilla.PerkIds.MagicalVirility) < 3)
                {
                    context.Write("Scenes.DesertCave.CumWitchBlessed.VirilityUpgradeNotice".Localized());

                    player.AddPerkValue(Datas.Vanilla.PerkIds.MagicalVirility, 1, 1);
                }
            }
            else
                if (!player.HasPerk(Datas.Vanilla.PerkIds.MagicalFertility))
                {
                    context.Write("Scenes.DesertCave.CumWitchBlessed.FertilityUnlockNotice".Localized());

                    player.CreatePerk(Datas.Vanilla.PerkIds.MagicalFertility);
                }
                else if (player.Perkv1(Datas.Vanilla.PerkIds.MagicalFertility) < 3)
                {
                    context.Write("Scenes.DesertCave.CumWitchBlessed.FertilityUpgradeNotice".Localized());

                    player.AddPerkValue(Datas.Vanilla.PerkIds.MagicalFertility, 1, 1);
                }

            context.Write("Scenes.DesertCave.CumWitchBlessed.BoldTagClose".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BeenBlessedByCumWitch, 1);

            player.Orgasm("Generic");

            player.Libido += 1;

            player.Sensitivity = Math.Max(0, player.Sensitivity - 5);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomCumWitchOffice(context));
        }

        public static void CumWitchDefeated(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write(Enemies.Vanilla.CumWitchEnemy.LastHpVictory

                ? "Scenes.DesertCave.CumWitchDefeated.WitchCollapsesTattered".Localized()

                : "Scenes.DesertCave.CumWitchDefeated.WitchCollapsesShredding".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CumWitchDefeated, 1);

            if (player.HasCock())
            {
                if (player.BiggestCockArea() > 50)
                    Scenes.Scene.AddButton(0, "Common.Button.TooBigFuck".Localized(), () => MaleTooBigVictorySex(context));

                if (player.CockThatFits(50) >= 0)
                    Scenes.Scene.AddButton(1, "Common.Button.FuckHer".Localized(), () => MenFuckCumWitch(context));
            }

            if (player.Cocks.Count(c => c.Id.Path == "tentacle") >= 3)
                Scenes.Scene.AddButton(2, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), () => TentacleVictoryGangbang(context));

            if (player.HasVagina())
                Scenes.Scene.AddButton(3, "Common.Button.Ladysex".Localized(), () => LadyVictorySex(context));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => DeclineSandWitchScene(context));
        }

        public static void MenFuckCumWitch(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.ShucksArmorEager".Localized());

            context.Write(player.Lust / player.MaxLust < 0.5f

                ? "Scenes.DesertCave.MenFuckUpSomeCumWitch.HalfHardWord".Localized()

                : "Scenes.DesertCave.MenFuckUpSomeCumWitch.RockHardWord".Localized());

            context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.PulsesWithReadiness".Localized());

            context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.TauntsUntilCutOff".Localized());

            var fits = player.CockThatFits(50) >= 0;

            var x = player.CockThatFits(50);

            var y = player.CockThatFits2(50);

            context.Write((fits

                ? "Scenes.DesertCave.MenFuckUpSomeCumWitch.AllTheWayPhrase"

                : "Scenes.DesertCave.MenFuckUpSomeCumWitch.MostOfPrefix").Localized());

            context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SheathedDeepInside".Localized(CoC.Instance.Player.CockDescript(x)));

            if (player.HasVagina())
                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.TwoCanPlayGame".Localized());

            if (player.Gender == Creatures.Gender.Hermaphrodite)
            {
                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.PivotsButtFacing".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.RidesRebelliousCow".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.ParagraphBreak".Localized());

                context.Write(player.Balls > 0

                    ? "Scenes.DesertCave.MenFuckUpSomeCumWitch.SweatyNutsacksSlide".Localized()

                    : "Scenes.DesertCave.MenFuckUpSomeCumWitch.SweatyNutsackGlides".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.FeverishFuckingCrescendo".Localized());

                if (player.CumQ() > 2000)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.OutlastsHerMagics".Localized());
                else if (player.CumQ() > 1000)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.BothBulgeBellies".Localized());
                else
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SheCumsLongerFills".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.WithdrawsFromPussy".Localized());

                player.Pregnancy.Type = new MonoCoC.Registry.Identifier("coc", "sand_witch");

                player.Pregnancy.Incubation = 90;

                player.Orgasm("Dick");

                player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
            }
            else if (player.CockThatFits2(50) >= 0)
            {
                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.ScowlPullsHand".Localized(CoC.Instance.Player.CockDescript(y)));

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.GrapplesYetSqueezes".Localized(CoC.Instance.Player.CockDescript(x), CoC.Instance.Player.CockDescript(y)));

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.WeakGrabSlapped".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.HermCreamTorrent".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.AsksReadyBitch".Localized());

                if (player.CumQ() < 250)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SloppyPourMess".Localized());
                else if (player.CumQ() < 500)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.GushesSoakingHoles".Localized());
                else if (player.CumQ() < 1000)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.GushesWithForce".Localized());
                else
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.WashesTidalWave".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.GirlCockPaints".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.UnsheathesDoubleDongs".Localized());

                player.Orgasm("Dick");
            }
            else
            {
                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.ScoldsNaughtyWench".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SentencePeriodSayEnd".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.LubeOozingEntrance".Localized());

                context.Write(player.Corruption < 50

                    ? "Scenes.DesertCave.MenFuckUpSomeCumWitch.HintOfCompassion".Localized()

                    : "Scenes.DesertCave.MenFuckUpSomeCumWitch.DeignsGivePleasure".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.IncreasesTempoLovemaking".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.OnyxThighsCrossBehind".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.KissesNeckWhispers".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.BrushesBlondTresses".Localized(CoC.Instance.Player.CockDescript(x)));

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.LegsTrembleBegging".Localized(CoC.Instance.Player.CockDescript(x), CoC.Instance.Player.CockDescript(x)));

                if (player.CumQ() < 700)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.UnceasingFloodBulgy".Localized());
                else
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.CunnyFloodedDome".Localized());

                if (player.CumQ() >= 1500)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.OverfullSpoutsSemen".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.WitchSighsRubs".Localized());

                if (player.CumQ() >= 700)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SwollenWord".Localized());

                context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SmearsWastedGoo".Localized());

                if (player.Corruption < 33)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.HopesSheLearned".Localized());
                else if (player.Corruption < 66)
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.WondersIfLearns".Localized());
                else
                    context.Write("Scenes.DesertCave.MenFuckUpSomeCumWitch.SmirksHopesNot".Localized());

                player.Orgasm("Dick");
            }

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => CumWitchDefeated(context));
        }

        public static void LadyVictorySex(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.LadyVictorySex.DisrobesFeminineSigh".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.PlantsFootOnHip".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.ProtestsBeingTeased".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.MercifulReleasesBalls".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.WipesHandScolds".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.SinksOnFirmPole".Localized());

            player.CuntChange(50, true, true, false);

            context.Write("Scenes.DesertCave.LadyVictorySex.SemiConsciousStirs".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.PantsTooSoon".Localized());

            if (player.Wetness() >= 5)
                context.Write("Scenes.DesertCave.LadyVictorySex.GushesDrenchingRiver".Localized());

            if (player.LactationQ() >= 50)
                context.Write("Scenes.DesertCave.LadyVictorySex.MilkSpoutsBosom".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.RidesBuckingBronco".Localized());

            context.Write("Scenes.DesertCave.LadyVictorySex.RecoversLegsShaking".Localized());

            player.Pregnancy.Type = new MonoCoC.Registry.Identifier("coc", "sand_witch");

            player.Pregnancy.Incubation = 90;

            player.Orgasm("Vaginal");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => CumWitchDefeated(context));
        }

        public static void MaleTooBigVictorySex(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var x = player.BiggestCockIndex();

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.SlipsOutSlowly".Localized(CoC.Instance.Player.CockDescript(x)));

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.ShockedBiggerThanMe".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.GripsRootBrushesNose".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.PushesWitchFlat".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.SmokeyLookGasps".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.WitchSmilesOneBetter".Localized(CoC.Instance.Player.CockHead(x)));

            if (player.Balls > 0)
                context.Write("Scenes.DesertCave.MaleTooBigVictorySex.FondleBallsWord".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.LiftsFeetHigh".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.CoosKeepGoing".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.ThrustsFirstInches".Localized(CoC.Instance.Player.CockHead(x)));

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.JigglesAsscheeks".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.SilencesBeggingWitch".Localized());

            if (player.Balls > 0)
                context.Write("Scenes.DesertCave.MaleTooBigVictorySex.BallsGrowingTight".Localized());

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.ClimaxHammersFace".Localized(CoC.Instance.Player.CockDescript(x), CoC.Instance.Player.CockHead(x)));

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.DemandsMoreFriction".Localized(CoC.Instance.Player.CockDescript(x)));

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.NotDoneLongShot".Localized(CoC.Instance.Player.CockHead(x)));

            context.Write("Scenes.DesertCave.MaleTooBigVictorySex.PatsCockAffectionately".Localized(CoC.Instance.Player.CockDescript(x)));

            player.Orgasm("Dick");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => CumWitchDefeated(context));
        }

        public static void TentacleVictoryGangbang(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var tentacles = player.Cocks.Count(c => c.Id.Path == "tentacle");

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.SorceressEyesQuestioningly".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.ErectionsPoisedStrike".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.RelaxesCockBundle".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.DeniedCockArches".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.PullsThroatProng".Localized());

            if (tentacles >= 4)
                context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.FourthPrickTwines".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.EbonyBackArches".Localized());

            if (tentacles >= 4)
                context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.SplatsFrottingCockTip".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.FirehoseNotOrgasm".Localized());

            context.Write(player.Balls > 0

                ? "Scenes.DesertCave.TentacleVictoryGangbangCumWitch.BallsTrembleCargo".Localized()

                : "Scenes.DesertCave.TentacleVictoryGangbangCumWitch.BodyTremblesCargo".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.GrabsEbonyTits".Localized());

            if (tentacles >= 4)
                context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.FourthPrickBlasts".Localized());

            if (player.CumQ() >= 500)
                context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.SpunkWavesWashIn".Localized());

            if (player.CumQ() >= 1000)
                context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.SplurtsInundateBelly".Localized());

            context.Write("Scenes.DesertCave.TentacleVictoryGangbangCumWitch.RetractsShaftsSmirks".Localized());

            player.Orgasm("Generic");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => CumWitchDefeated(context));
        }

        public static void SphinxSubmissionOptions(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            CoC.Instance.Flags.Set(GameFlagIds.TimesSubmittedToSanura,

                CoC.Instance.Flags.Get(GameFlagIds.TimesSubmittedToSanura) + 1);

            context.Write("Scenes.DesertCave.SphinxSubmissionOptions.ObeyingRiddleDefeat".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.GetPegged".Localized(), () => SanuraPegged(context));

            if (CoC.Instance.Player.HasCock())
            {
                Scenes.Scene.AddButton(1, "Common.Button.GetBlown".Localized(), () => SanuraGetBlown(context, true));

                Scenes.Scene.AddButton(2, "Common.Button.Pawjob".Localized(), () => SanuraLionpaws(context, true));
            }

            Scenes.Scene.AddButton(3, "Common.Button.ArcaneToying".Localized(), () => SanuraCunnilingus(context, true));
        }

        public static void SanuraPegged(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.StartingDisrobePhrase".Localized());

            if (player.HasCock())
            {
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.UnleashesCocksHeat".Localized());

                context.Write(player.Cocks.Count == 1

                    ? "Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.MemberIt".Localized()

                    : "Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.MembersThey".Localized());

                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.SwayingBackForth".Localized());
            }
            else if (player.HasVagina())
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.BaresVaginaHeat".Localized());
            else
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.BaresRearHeat".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.HeatFansLibido".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.CommandsAssumePosition".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.ConfusedByPosition".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.RollsEyesExplains".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.MissingPenisRemark".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.CasualSoRetort".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.WithoutPenisDoubts".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.RealityIsPlaything".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.GlancesUpAbove".Localized());

            if (player.AnalCapacity() >= 100)
            {
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.AssholeStretchesMagic".Localized());

                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.SphinxPraisesYou".Localized());
            }
            else if (player.AnalCapacity() >= 40)
            {
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.StretchedToLimit".Localized());

                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.ElectricBallsBottomOut".Localized());
            }
            else
            {
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.LittleRoomInside".Localized());

                context.Write(player.Body.Get<BodyParts.Vanilla.AnalPart>().Virgin

                    ? "Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.VirginConfessionNod".Localized()

                    : "Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.CurseAboutTightness".Localized());
            }

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.RectumWillBeGaped".Localized());

            player.ButtChange(75, true, true, false);

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.StraightensAndHilts".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.ReluctantlyPullsOut".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.DrawlsAndLurches".Localized());

            if (player.HasCock())
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.PreCumDanglesDown".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.SexualFrenzyRutting".Localized());

            if (player.HasCock())
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.ProstateMilkedByFlare".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.FauxBallsSwelling".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.GutsAudiblyGurgle".Localized());

            context.Write(player.HasCock()

                ? "Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.ProstateTriggersClimax".Localized()

                : "Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.ClimaxOnHerExit".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.SwollenHeadStuck".Localized());

            if (player.HasCock())
                context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.DraggedThroughPuddle".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.EjectsWithMightyLurch".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.CollapseInSpentHeap".Localized());

            context.Write("Scenes.DesertCave.FenPutsHisShittyFetishInYoSphinx.GivesDopeyThumbsUp".Localized());

            player.Orgasm("Anal");

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
            {
                Scenes.Scene.View.Time.PassTime(1);

                Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
            });
        }

        public static void SanuraGetBlown(DungeonContext context, bool submit = false)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.GetBlown.StripAndUnleashCocks".Localized());

            if (player.Balls > 0)
                context.Write("Scenes.DesertCave.GetBlown.HandCupsYourBalls".Localized());

            context.Write("Scenes.DesertCave.GetBlown.SphinxDropsToKnees".Localized());

            context.Write("Scenes.DesertCave.GetBlown.SphinxBeginsLicking".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.GetBlown.EachOfPrefix".Localized());

            context.Write("Scenes.DesertCave.GetBlown.YourDickWord".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.GetBlown.PossessiveSLetterFourteenth".Localized());

            context.Write("Scenes.DesertCave.GetBlown.MassageUntilBursting".Localized());

            context.Write("Scenes.DesertCave.GetBlown.MuffledPleaToHold".Localized());

            context.Write("Scenes.DesertCave.GetBlown.ReleasesPrickSuddenly".Localized());

            context.Write("Scenes.DesertCave.GetBlown.FingerBrushesTip".Localized());

            context.Write(player.Balls > 0

                ? "Scenes.DesertCave.GetBlown.BallsEmptyPhrase".Localized()

                : "Scenes.DesertCave.GetBlown.BodyEmptiesPhrase".Localized());

            context.Write("Scenes.DesertCave.GetBlown.CumsOntoEagerFace".Localized());

            context.Write(player.Cocks.Count > 1

                ? "Scenes.DesertCave.GetBlown.EachOfYourPrefix".Localized()

                : "Scenes.DesertCave.GetBlown.YourPrefixAgain".Localized());

            context.Write("Scenes.DesertCave.GetBlown.DrippingCockWord".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.GetBlown.PossessiveSLetterFourteenth".Localized());

            context.Write("Scenes.DesertCave.GetBlown.OffersSecondRound".Localized());

            context.Write("Scenes.DesertCave.GetBlown.NodAndShudderMagic".Localized());

            context.Write("Scenes.DesertCave.GetBlown.SaltyLiquidOnSand".Localized());

            context.Write(player.Balls > 0

                ? "Scenes.DesertCave.GetBlown.ChurnWord".Localized()

                : "Scenes.DesertCave.GetBlown.BodyChurnsPhrase".Localized());

            context.Write("Scenes.DesertCave.GetBlown.SeeminglyEndlessSeed".Localized());

            context.Write("Scenes.DesertCave.GetBlown.SanuraSlurpsTip".Localized());

            if (player.Cocks.Count >= 3)
            {
                context.Write(player.Cocks.Count == 2

                    ? "Scenes.DesertCave.GetBlown.YourPrefixAgain".Localized()

                    : "Scenes.DesertCave.GetBlown.TwoOfYourPrefix".Localized());

                context.Write("Scenes.DesertCave.GetBlown.EtherealHandsGripOthers".Localized(NumberUtils.Num2Text(CoC.Instance.Player.CockTotal() - 1)));

                if (player.Cocks.Count >= 5)
                    context.Write("Scenes.DesertCave.GetBlown.BarelyTimeToAdjust".Localized());
            }

            context.Write("Scenes.DesertCave.GetBlown.SphinxWorksWonders".Localized());

            context.Write("Scenes.DesertCave.GetBlown.SecondOrgasmOverload".Localized());

            context.Write("Scenes.DesertCave.GetBlown.CumGeysersRainDown".Localized());

            context.Write("Scenes.DesertCave.GetBlown.AsksAboutEnjoyment".Localized());

            player.Orgasm("Dick");

            if (submit)
                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
                {
                    Scenes.Scene.View.Time.PassTime(1);

                    Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
                });
            else
            {
                Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));

                Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SphinxMenu(context));
            }
        }

        public static void SanuraLionpaws(DungeonContext context, bool skipped = false)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var pawjobs = CoC.Instance.Flags.Get(GameFlagIds.Pawjobs);

            var softStart = player.Lust / player.MaxLust < 0.5f && pawjobs == 0;

            if (skipped)
            {
                context.Write("Scenes.DesertCave.Lionpaws.SanuraReluctantSmirk".Localized());

                context.Write(player.Cocks.Count == 1

                    ? "Scenes.DesertCave.Lionpaws.CockItAgain".Localized()

                    : "Scenes.DesertCave.Lionpaws.CockTheyAgain".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.SphinxEntrancedBySway".Localized());

                if (player.Cocks.Count > 1)
                    context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.SentencePeriod".Localized());
            }
            else if (pawjobs == 0)
                context.Write("Scenes.DesertCave.Lionpaws.SanuraRaisesEyebrows".Localized());
            else
            {
                context.Write("Scenes.DesertCave.Lionpaws.SanuraCockedHeadAsk".Localized());

                if (player.BiggestCockArea() > 100)
                    context.Write("Scenes.DesertCave.Lionpaws.LittleElseForMonster".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.SanuraExaminesPaw".Localized());
            }

            context.Write("Scenes.DesertCave.Lionpaws.SanuraAsksYouKneel".Localized());

            if (softStart)
            {
                context.Write("Scenes.DesertCave.Lionpaws.KneelingBeforeSanura".Localized());

                if (player.Cocks.Count > 1)
                    context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.BeginWord".Localized());

                if (player.Cocks.Count == 1)
                    context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.HardensInAnticipation".Localized());
            }

            context.Write("Scenes.DesertCave.Lionpaws.SphinxBendsToCrotch".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.OfYourPrefix".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.VariousWord".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.PrickWord".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.WithSoftSkilledTongue".Localized());

            if (softStart)
            {
                context.Write("Scenes.DesertCave.Lionpaws.SoonOralMinistrations".Localized());

                if (player.Cocks.Count > 1)
                    context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.ErectThrobbingHunger".Localized());
            }

            context.Write("Scenes.DesertCave.Lionpaws.ExpertStrokesCoatMember".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.GlisteningVeilSaliva".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.ApparentlyDeemingPrefix".Localized());

            context.Write(player.Cocks.Count > 1 ? "Common.Text.Them".Localized() : "Common.Text.It".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.SanuraLiftsForeleg".Localized());

            if (softStart)
                context.Write("Scenes.DesertCave.Lionpaws.SurprisedAtPawAdvance".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.PlayfullyTapsYourCock".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.OneOfPrefix".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.YourShaftWord".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.PressingCockToWaist".Localized());

            if (player.SmallestCockArea() < 100)
                context.Write("Scenes.DesertCave.Lionpaws.MemberSlipsBetweenDigits".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.FluffyFurTicklesYou".Localized());

            if (!skipped && pawjobs == 0)
                context.Write("Scenes.DesertCave.Lionpaws.PeculiarWonderfulFeeling".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.SphinxSucklesOtherCocks".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.WarmthCloudingYourMind".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.ImperfectPawjobThirst".Localized());

            if (player.Cocks.Count > 1)
                context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.SentencePeriod".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.GrabAnkleToContinue".Localized());

            if (player.CumQ() < 250)
                context.Write("Scenes.DesertCave.Lionpaws.FinalPushesFindRelease".Localized());
            else
            {
                context.Write("Scenes.DesertCave.Lionpaws.TorrentBurstsFromCock".Localized());

                if (player.Cocks.Count > 1)
                    context.Write("Scenes.DesertCave.Lionpaws.PossessiveSLetterTenth".Localized());

                context.Write("Scenes.DesertCave.Lionpaws.GeyserFloodOfSemen".Localized());
            }

            context.Write("Scenes.DesertCave.Lionpaws.ExhaustedAfterOrgasm".Localized());

            if (pawjobs > 0)
                context.Write("Scenes.DesertCave.Lionpaws.AsAlwaysPhrase".Localized());

            context.Write("Scenes.DesertCave.Lionpaws.SanuraSendsYouOff".Localized());

            context.Write(player.CumQ() < 250

                ? "Scenes.DesertCave.Lionpaws.LegWord".Localized()

                : "Scenes.DesertCave.Lionpaws.CleaningStainedFur".Localized());

            player.Orgasm("Dick");

            player.Sensitivity = Math.Max(0, player.Sensitivity - 2);

            if (skipped)
                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
                {
                    Scenes.Scene.View.Time.PassTime(1);

                    Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
                });
            else
            {
                Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));

                Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SphinxMenu(context));
            }
        }

        public static void SanuraCunnilingus(DungeonContext context, bool submitted = false)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var intro = "Scenes.DesertCave.SanuraCunnilingus.SphinxReclinesOnSandstone".Localized(submitted)
                .Replace("[if (isnaked) {{saunter over|shed your [armor], exposing more [skinfurscales], until entirely nude}}]",

                    player.IsNaked()

                        ? "saunter over"

                        : "shed your [armor], exposing more [skinfurscales], until entirely nude")
                .Replace("[if ({0}) {{[say:I hope you get my suggestion,]|[say:I'm yours for the taking,]}}]",

                    submitted

                        ? "[say:I hope you get my suggestion,]"

                        : "[say:I'm yours for the taking,]");

            context.Write(intro);

            context.Write("Scenes.DesertCave.SanuraCunnilingus.EncouragingGlanceDuringLick".Localized());

            context.Write("Scenes.DesertCave.SanuraCunnilingus.SmirkingEncouragementTaunt".Localized());

            player.ButtChange(10, true, true, false);

            context.Write("Scenes.DesertCave.SanuraCunnilingus.EtherealCockAnusThrust".Localized());

            context.Write("Scenes.DesertCave.SanuraCunnilingus.SecondArcanePhallusForms".Localized());

            context.Write("Scenes.DesertCave.SanuraCunnilingus.StretchedAssUrgesRelease".Localized());

            context.Write("Scenes.DesertCave.SanuraCunnilingus.MagicGripSnapsLoudly".Localized());

            context.Write("Scenes.DesertCave.SanuraCunnilingus.ReleasedButTrembling".Localized());

            context.Write("Scenes.DesertCave.SanuraCunnilingus.AffectionateHeadPat".Localized());

            player.Orgasm("Anal");

            if (submitted)
                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
                {
                    Scenes.Scene.View.Time.PassTime(1);

                    Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
                });
            else
            {
                Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));

                Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SphinxMenu(context));
            }
        }

        public static void FinishSanuraSubmission(DungeonContext context)
        {
            Scenes.Scene.View.Time.PassTime(1);

            Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
        }

        public static void PawThatPussy(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => context.Write(key.Localized());

            W("Scenes.DesertCave.PawThatPussy.SashaysToWaitingSphinx");

            W("Scenes.DesertCave.PawThatPussy.PinnedUnderHerPaw");

            W("Scenes.DesertCave.PawThatPussy.UncertainAnticipation");

            W("Scenes.DesertCave.PawThatPussy.PullsHerBackFur");

            p.CuntChange(5, true, true, false);

            W("Scenes.DesertCave.PawThatPussy.FullPawInside");

            W("Scenes.DesertCave.PawThatPussy.ExhaustedInSandTogether");

            p.Orgasm("Vaginal");

            Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SphinxMenu(context));
        }

        public static void FuckDatSphinx(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.FuckDatSphinx.InviteSphinxToFun".Localized(CoC.Instance.Player.Mf("handsome", "beautiful")));

            if (player.HasCock())
            {
                Scenes.Scene.AddButton(0, "Common.Button.FuckHer".Localized(), () => FuckDatLionPussah(context));

                Scenes.Scene.AddButton(1, "Common.Button.GetBlown".Localized(), () => SanuraGetBlown(context, false));

                Scenes.Scene.AddButton(2, "Common.Button.Pawjob".Localized(), () => SanuraLionpaws(context, true));
            }

            if (player.HasVagina())
                Scenes.Scene.AddButton(3, "Common.Button.ForceDildos".Localized(), () => ForceDildos(context));

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => SphinxMenu(context));
        }

        public static void FuckDatLionPussah(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var biggest = player.BiggestCockIndex();

            var knot = player.HasKnot(biggest);

            context.Write("Scenes.DesertCave.FuckDatLionPussah.DisrobeAndStretch".Localized());

            if (player.BiggestCockArea() < 6)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.GigglesAtSmallCock".Localized());
            else if (player.BiggestCockArea() < 30)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.PurrsAtModestSize".Localized());
            else if (player.BiggestCockArea() < 100)
            {
                context.Write("Scenes.DesertCave.FuckDatLionPussah.PurrsAtLargeDick".Localized());

                if (player.Balls > 0)
                    context.Write("Scenes.DesertCave.FuckDatLionPussah.BallsWord".Localized());
                else if (player.HasVagina())
                    context.Write("Scenes.DesertCave.FuckDatLionPussah.CuntWord".Localized());
                else
                    context.Write("Scenes.DesertCave.FuckDatLionPussah.AssholeWord".Localized());

                context.Write("Scenes.DesertCave.FuckDatLionPussah.PraisesTuftedCock".Localized());
            }
            else
                context.Write("Scenes.DesertCave.FuckDatLionPussah.ShockedAtMonsterCock".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.ManhoodHardensUnderGaze".Localized());

            context.Write(player.TotalNipples() == 2

                ? "Scenes.DesertCave.FuckDatLionPussah.BothWord".Localized()

                : "Scenes.DesertCave.FuckDatLionPussah.EachWord".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.NipplesStiffenToo".Localized());

            context.Write(player.HasVagina()

                ? "Scenes.DesertCave.FuckDatLionPussah.CuntTightensAndGushes".Localized()

                : "Scenes.DesertCave.FuckDatLionPussah.AssholeClenchesShiver".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.SheGivesInstructions".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.KneelBeforeLionpussy".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.MischievousWarmUpOffer".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.GripsBreastWhileRiding".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.CountsRhythmThenThrusts".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.ThrustsInTimeAgain".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.ThreeScreamingOrgasms".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.BuriesCockToHilt".Localized());

            if (knot)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.KnotSlipsAndLocks".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.CumPaintsInnerWalls".Localized());

            if (player.CumQ() < 250)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.SpunkDribblesIntoSand".Localized());
            else if (player.CumQ() < 750)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.CumEscapesHerFolds".Localized());
            else if (player.CumQ() < 3000)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.BellyEnlargesWithSpunk".Localized());
            else if (player.CumQ() < 10000)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.BellyLooksPregnant".Localized());
            else
                context.Write("Scenes.DesertCave.FuckDatLionPussah.BellySwellingContinues".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.BothCollapseSpent".Localized());

            if (knot)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.KnotMakesLyingAwkward".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.SlowRecoveryPanting".Localized());

            if (knot)
                context.Write("Scenes.DesertCave.FuckDatLionPussah.KnotFinallyDeflates".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.GatherStrengthToRise".Localized());

            context.Write("Scenes.DesertCave.FuckDatLionPussah.SanuraRecomposesDryly".Localized());

            player.Orgasm("Dick");

            Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SphinxMenu(context));
        }

        public static void ForceDildos(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.ForceDildos.FacesStonePillar".Localized());

            if (player.LactationQ() >= 200)
                context.Write("Scenes.DesertCave.ForceDildos.TrickleOfFluids".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.SentencePeriod".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.SphinxKneelsBehindYou".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.TeasingLicksAndFingers".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.TipFinallyEnters".Localized());

            if (player.HasVagina())
                player.CuntChange(10, true, true, false);
            else
                player.ButtChange(10, true, true, false);

            context.Write("Scenes.DesertCave.ForceDildos.SensitiveWallsIgnite".Localized());

            if (player.HasVagina())
            {
                context.Write("Scenes.DesertCave.ForceDildos.CuntAdjustsToIntruders".Localized());

                player.ButtChange(10, true, true, false);
            }
            else
                context.Write("Scenes.DesertCave.ForceDildos.ThirdDildoUnnoticed".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.ThreeEnergyDildos".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.BoilingFloodInside".Localized());

            context.Write("Scenes.DesertCave.ForceDildos.DildosFadeAway".Localized());

            player.Orgasm("VaginalAnal");

            Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SphinxMenu(context));
        }

        public static void GrabTheMilkGirl(DungeonContext context)
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.CallMilkSlaveOver".Localized(CoC.Instance.Player.Mf("M-master?", "M-mistress?")));

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.FlashGrinGrabTits".Localized(CoC.Instance.Player.Mf("M-master?", "M-mistress?")));

            if (player.BiggestBreastSize() >= 8)
                context.Write("Scenes.DesertCave.GrabTheMilkGirl.PressBreastsTogether".Localized());

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.SentencePeriod".Localized());

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.SheMoansQuietly".Localized());

            context.Write(player.Gender switch
            {
                Creatures.Gender.Male => "Scenes.DesertCave.GrabTheMilkGirl.CockToken".Localized(),

                Creatures.Gender.Female => "Scenes.DesertCave.GrabTheMilkGirl.VaginaToken".Localized(),

                Creatures.Gender.Hermaphrodite => "Scenes.DesertCave.GrabTheMilkGirl.CockAndVaginaJoin".Localized(),

                _ => "Scenes.DesertCave.GrabTheMilkGirl.SexlessCrotchBurns".Localized()
            });

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.GentlePushRestArms".Localized());

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.SheTensesAtTouch".Localized());

            context.Write(player.Corruption < 70

                ? "Scenes.DesertCave.GrabTheMilkGirl.PullBackSheReaches".Localized()

                : "Scenes.DesertCave.GrabTheMilkGirl.FingersSlideInEasily".Localized());

            context.Write("Scenes.DesertCave.GrabTheMilkGirl.SlaveTurnsAway".Localized());

            if (player.HasCock())
            {
                context.Write("Scenes.DesertCave.GrabTheMilkGirl.MilkGirlHandThigh".Localized());

                if (player.Balls > 0)
                    context.Write("Scenes.DesertCave.GrabTheMilkGirl.HandCupsBalls".Localized());

                context.Write("Scenes.DesertCave.GrabTheMilkGirl.KissesDownYourBack".Localized());
            }

            if (player.HasVagina())
            {
                context.Write("Scenes.DesertCave.GrabTheMilkGirl.FingersTouchVaginaSlit".Localized());

                if (player.BiggestBreastSize() >= 1)
                    context.Write("Scenes.DesertCave.GrabTheMilkGirl.SheCupsYourBreasts".Localized());

                if (player.LactationQ() >= 200)
                    context.Write("Scenes.DesertCave.GrabTheMilkGirl.MilkSpurtEscapes".Localized());
            }

            player.Lust = Math.Min(player.MaxLust, player.Lust + 33);

            if (player.HasCock())
                Scenes.Scene.AddButton(0, "Scenes.Goblin.Button.DickFuck".Localized(), () => FuckMilkbabeWithPenor(context));

            if (player.HasVagina() && player.BiggestBreastSize() >= 3)
                Scenes.Scene.AddButton(1, "Common.Button.LadyFuck".Localized(), () => LadyFucks(context));

            if (player.Gender == Creatures.Gender.Genderless)
                Scenes.Scene.AddButton(2, "Common.Button.DontFuck".Localized(), () => DontFuckMilkBathBabe(context));
            else
                Scenes.Scene.AddButton(2, "Common.Button.DontFuck".Localized(), () => DontFuckMilkBathBabe(context));
        }

        public static void DontFuckMilkBathBabe(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.DontFuckMilkBathBabe.AllowGirlToContinue".Localized());

            player.HealthChange(player.MaxHealth * 0.33f);

            player.ChangeFatigue(-20);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomBathroom(context));
        }

        public static void FuckMilkbabeWithPenor(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var x = player.CockThatFits(50);

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.PullSlutAgainstYou".Localized(CoC.Instance.Player.CockDescript(x)));

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.SlideUnderMilkSea".Localized());

            if (x < 0) x = player.SmallestCockIndex();
            var area = player.CockArea(x);

            if (area < 30)
                context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.HiltInOneStroke".Localized());
            else if (area < 50)
                context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.PushInchesDeep".Localized());
            else
                context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.ExcessDickInCream".Localized());

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.SheHooksLegsRock".Localized());

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.BathSlutMoansPraise".Localized());

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.PickUpThrustPace".Localized());

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.AllowGirlReleaseQuiver".Localized());

            context.Write("Scenes.DesertCave.FuckMilkbabeWithPenor.BodyFeelsRefreshed".Localized());

            player.Orgasm("Dick");

            player.ChangeFatigue(-15);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomBathroom(context));
        }

        public static void LadyFucks(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var player = CoC.Instance.Player;

            context.Write("Scenes.DesertCave.LadyFucks.TurnInMilkyPool".Localized());

            if (player.LactationQ() >= 200)
                context.Write("Scenes.DesertCave.LadyFucks.TrickleOfMilk".Localized());

            context.Write("Scenes.DesertCave.LadyFucks.HandSlipsUpBody".Localized());

            context.Write(player.LactationQ() >= 200

                ? "Scenes.DesertCave.LadyFucks.SkilledMilking".Localized()

                : "Scenes.DesertCave.LadyFucks.SentencePeriod".Localized());

            context.Write("Scenes.DesertCave.LadyFucks.LeanBackOnRim".Localized());

            if (player.LactationQ() >= 200)
                context.Write("Scenes.DesertCave.LadyFucks.MilkyWord".Localized());

            context.Write("Scenes.DesertCave.LadyFucks.TeatsWorkedSkillfully".Localized());

            context.Write("Scenes.DesertCave.LadyFucks.StrangePressureBuilds".Localized());

            if (player.LactationQ() >= 200)
                context.Write("Scenes.DesertCave.LadyFucks.SprayMilkOnMaid".Localized());

            context.Write("Scenes.DesertCave.LadyFucks.FingersThroughHair".Localized());

            context.Write("Scenes.DesertCave.LadyFucks.RefreshedAfterMilking".Localized());

            player.Orgasm(player.LactationQ() >= 200 ? "Common.Button.Tits".Localized() : "Common.Button.Vaginal".Localized());

            player.ChangeFatigue(-15);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => RoomBathroom(context));
        }

        public record Riddle(string Key, (string Label, bool Correct)[] Answers);

        public static readonly Riddle[] Riddles =

        [
            new("Scenes.DesertCave.RiddleOne.OldFavoriteRiddle", [("A Sphinx", false), ("A Centaur", false), ("A Man", true), ("Stilts", false)]),

            new("Scenes.DesertCave.RiddleTwo.SimpleRiddleChallenge", [("A River", true), ("A Whisper", false), ("A Nail", false), ("A Bunny-Girl", false)]),

            new("Scenes.DesertCave.RiddleThree.MorbidRiddleOffer", [("Poison", false), ("A Condom", false), ("Arrows", false), ("A Coffin", true)]),

            new("Scenes.DesertCave.RiddleFour.ClassicHungerRiddle", [("A Faerie", false), ("Fire", true), ("A Dog", false), ("Disease", false)]),

            new("Scenes.DesertCave.RiddleFive.GoblinFavoriteRiddle", [("Men", false), ("Women", false), ("Wine", true), ("Cheese", false)]),

            new("Scenes.DesertCave.RiddleSix.SadFleetingRiddle", [("A Goblin", false), ("A Candle", true), ("A Boat", false), ("An Arrow", false)]),

            new("Scenes.DesertCave.RiddleSeven.SandWitchRiddle", [("A Shadow", true), ("A Dog", false), ("Water", false), ("The Breeze", false)]),

            new("Scenes.DesertCave.RiddleEight.MelodyOnBreezeRiddle", [("A Demon", false), ("Religion", false), ("An Idea", false), ("An Echo", true)]),

            new("Scenes.DesertCave.RiddleNine.ManyLivesRiddle", [("A Dick", false), ("Water", true), ("A Voice", false), ("Faith", false)]),

            new("Scenes.DesertCave.RiddleTen.DeviousMakerRiddle", [("Disease", false), ("Counterfeits", true), ("Piss In A River", false), ("Tattered Scrolls", false)]),

            new("Scenes.DesertCave.RiddleEleven.NagaFavoriteRiddle", [("Treasure Chest", false), ("Pure Honey", false), ("Eggs", true), ("Booze Bottles", false)]),

            new("Scenes.DesertCave.RiddleTwelve.HeavenAndEarthRiddle", [("Fallen Stars", false), ("Angels", false), ("Sand", true), ("Rain", false)])
        ];

        public static readonly List<int> Chosen = [];

        public static int Current;

        public static int CorrectCount;

        public static void SphinxMenu(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.AddButton(0, "Common.Button.Appearance".Localized(), () => SphinxAppearance(context));

            Scenes.Scene.AddButton(1, "Common.Button.RiddleGame".Localized(), () => RiddleGameGo(context));

            if (CoC.Instance.Flags.Get(GameFlagIds.BeatenSanuraCount) > 0)
                Scenes.Scene.AddButton(2, "Common.Button.FuckHer".Localized(), () => FuckDatSphinx(context));

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => RoomEntrance(context));
        }

        public static void SphinxAppearance(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.SphinxAppearance.SanuraStandsBeforeYou".Localized());

            context.Write("Scenes.DesertCave.SphinxAppearance.FelineEarsBlackHair".Localized());

            context.Write("Scenes.DesertCave.SphinxAppearance.BlueBraAndLoincloth".Localized());

            context.Write("Scenes.DesertCave.SphinxAppearance.FourPawsAndTail".Localized());

            context.Write("Scenes.DesertCave.SphinxAppearance.SoftCupBreasts".Localized());

            context.Write("Scenes.DesertCave.SphinxAppearance.IntimatesBeneathTail".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => SphinxMenu(context));
        }

        public static void RiddleGameGo(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Chosen.Clear();

            CorrectCount = 0;

            context.Write("Scenes.DesertCave.RiddleGameGo.MarvelousGameInvite".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => RiddlePicker(context));
        }

        public static void RiddleReturn(DungeonContext context) => ShowRiddle(context);

        public static void RiddlePicker(DungeonContext context)
        {
            var options = Enumerable.Range(0, Riddles.Length).Where(i => !Chosen.Contains(i)).ToArray();

            if (options.Length == 0)
            {
                SphinxMenu(context);

                return;
            }

            Current = options[Random.Shared.Next(options.Length)];

            Chosen.Add(Current);

            ShowRiddle(context);
        }

        public static void ShowRiddle(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write(Riddles[Current].Key.Localized());

            var answers = Riddles[Current].Answers;

            for (var index = 0; index < answers.Length; index++)
            {
                var (label, correct) = answers[index];

                Scenes.Scene.AddButton(index, label, () =>
                {
                    if (Current == 0 && label == "A Sphinx")
                    {
                        Scenes.Scene.ClearOutput();

                        context.Write("Scenes.DesertCave.RiddleOneSphinx.SphinxNarrowsEyes".Localized());

                        Chosen.Remove(Current);

                        Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => RiddlePicker(context));

                        return;
                    }

                    if (correct)
                        AnswerCorrect(context);
                    else
                        AnswerWrong(context);
                });
            }

            Scenes.Scene.AddButton(5, "Common.Button.UhAttack".Localized(), () => FuckItAttack(context, () => ShowRiddle(context)));
        }

        public static void AnswerCorrect(DungeonContext context)
        {
            CorrectCount++;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (CorrectCount < 3)
            {
                context.Write("Scenes.DesertCave.AnswerCorrect.SphinxAdmitsCorrect".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => RiddlePicker(context));

                return;
            }

            var first = CoC.Instance.Flags.Get(GameFlagIds.BeatenSanuraCount) == 0;

            context.Write(first

                ? "Scenes.DesertCave.AnswerCorrect.SphinxGaspsAtAnswer".Localized()

                : "Scenes.DesertCave.AnswerCorrect.SanuraPoutsForVictory".Localized());

            if (first)
                context.Write("Scenes.DesertCave.AnswerCorrect.SphinxCollectsHerself".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BeatenSanuraCount,

                CoC.Instance.Flags.Get(GameFlagIds.BeatenSanuraCount) + 1);

            context.Write("Scenes.DesertCave.AnswerCorrect.PlayerDecisionPrompt".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.FuckHer".Localized(), () => FuckDatSphinx(context));

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => RoomEntrance(context));
        }

        public static void AnswerWrong(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.AnswerWrong.ProudSmileWrongAnswer".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Submit".Localized(), () => SphinxSubmissionOptions(context));

            Scenes.Scene.AddButton(1, "Common.Button.UhAttack".Localized(), () => FuckItAttack(context, () => ShowRiddle(context)));
        }

        public static void FuckItAttack(DungeonContext context, Action onNo)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.Riddle.ConfirmAttack".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Yes".Localized(), () => ReallyAttack(context));

            Scenes.Scene.AddButton(1, "Common.Button.No".Localized(), onNo);
        }

        public static void ReallyAttack(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.ReallyAttack.ChargeThroughSphinx".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SanuraDisabled, 1);

            Scenes.Scene.AddButton(0, "Common.Button.Enter".Localized(), () => RoomEntrance(context));
        }

        public static void SandWitchMobLost()
        {
            var p = CoC.Instance.Player;

            if (p.HasCock() && p.BiggestCockArea() >= 6)
            {
                MemeberedFolksFindTrueWuv();

                return;
            }

            if (!p.HasCock())
            {
                FemaleGirlsLoseToSammitches();

                return;
            }

            GetMockedForSmallDongBySammitchMob();
        }

        public static void SandWitchMobNotBadEnd()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobNotBadEnd.WakeOutsideWithBelongings".Localized());

            CoC.Instance.Player.Orgasm("Vaginal");

            CoC.Instance.Player.Libido += 1;

            CoC.Instance.Player.Sensitivity = Math.Max(0, CoC.Instance.Player.Sensitivity - 1);

            DungeonSession.End();

            Scenes.Scene.View.Time.PassTime(1);

            Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
        }

        public static void SandWitchMobBadEnd()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.WakeOnBedTransformed".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.GreetingMorningSister".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.SisterNotRelated".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.CommonsAgainWarning".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.ToldToAvoidCommons".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.CloakedGirlPullsBlanket".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.HelpsOutOfBed".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.MonthsLaterHeading".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.LearnedToBeWitch".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.WanderDesertPracticingIntro".Localized());

            Scenes.Scene.OutputText("Scenes.DesertCave.SandWitchMobBadEnd.PracticeSpellOffer".Localized());

            CoC.Instance.Player.Health = CoC.Instance.Player.MaxHealth;

            Scenes.Scene.GameOver();
        }

        public static void DeclineSandWitchScene(DungeonContext? context = null)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.DesertCave.DeclineSandWitch.DustsOffLeaves".Localized());

            if (Combat.CombatController.Instance != null)
                Combat.CombatController.Instance.Context.ReturnToCamp();
            else
                Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
        }

        public static void FightTheSandWitchScene(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.FightTheSandWitch.BrandishWeaponAdvance".Localized());

            context.StartCombat(EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "sand_mother")));
        }

        public static void FemaleGirlsLoseToSammitches()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var friendly = CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) > 0;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.WitchesSpreadLegsApart");

            W(p.HasMuzzle()

                ? "Scenes.DesertCave.FemaleGirlsLoseToSammitches.MuzzleFuckedLikeCock"

                : "Scenes.DesertCave.FemaleGirlsLoseToSammitches.GrindsPussiesOnLips");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.ThrashArmsPinnedDown");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.TwoWitchesLeaveRoom");

            if (!p.HasVagina())
            {
                W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.OilSplitsFleshOpen");

                p.CreateVagina();

                p.SetClitLength(0.25f);
            }

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.StreamsRunOverBody");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.GirlsHoldBackPleasure");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.PussyHungryForTouch");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.WitchesFetchSexToys");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.PregnantSlutRoleplay");

            W(p.HasMuzzle()

                ? "Scenes.DesertCave.FemaleGirlsLoseToSammitches.MuzzleFuckSelfPlay"

                : "Scenes.DesertCave.FemaleGirlsLoseToSammitches.GrindingOnYourFace");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.FourOthersWhineEnjoyment");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.WitchesPickFakeCocks");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.AttentionBackToWitches");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.HandsFreeFingerPussy");

            Scenes.Scene.OutputText(
                "Scenes.DesertCave.FemaleGirlsLoseToSammitches.BetweenWord".Localized()

                + " " + (p.BiggestBreastSize() >= 1

                    ? "Scenes.DesertCave.FemaleGirlsLoseToSammitches.OnWord".Localized()

                    : "Scenes.DesertCave.FemaleGirlsLoseToSammitches.OnWord".Localized())

                + " " + "Scenes.DesertCave.FemaleGirlsLoseToSammitches.YourChestWord".Localized());

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.HurryUpPickCock");

            W(friendly

                ? "Scenes.DesertCave.FemaleGirlsLoseToSammitches.SlutWord"

                : "Scenes.DesertCave.FemaleGirlsLoseToSammitches.InitiateWord");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.GettingRandySayEnd");

            if (friendly)
                W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.PonderWordToyFocus");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.WitchesGatherFakeCocks");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.GripCocksBothSides");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.HoverAndSlamDown");

            p.CuntChange(30, true, true, false);

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.FullAndBlissedPounding");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.CummingOnWitchCrotch");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.DraconicCockWitchHelps");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.ImpalesAssholeDeep");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.LicksLipsPoutsTease");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.CumsWithoutBuildUp");

            W("Scenes.DesertCave.FemaleGirlsLoseToSammitches.NextHoursAreBlur");

            p.Orgasm("Vaginal");

            p.Libido += 1;

            if (friendly)
                SandWitchMobNotBadEnd();
            else
                SandWitchMobBadEnd();
        }

        public static void GetMockedForSmallDongBySammitchMob()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var friendly = CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) > 0;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.WitchMocksSmallCock");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.FlickingFingerMassage");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.PrecumBeadsOnTip");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.TryNotToCum");

            if (p.CumQ() < 250)
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.SmallSquirtDirtiesFingers");
            else if (p.CumQ() < 500)
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.SpurtsGloopWitchHand");
            else
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.CumBarrageOnFace");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.MockShrinkingCock");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.ForceMouthOpenPour");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.WarmFeelingMakesHard");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.DrugMakesBodySensitive");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.DenyAdvancesUntilCum");

            if (p.Balls > 0)
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.BallsNotEnoughTime");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.WateryCumBetweenToes");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.FootOrderedToLick");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.LickSoleUntilClean");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.NextWitchKneelsDown");

            W(p.IsBiped()

                ? "Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.KneelBetweenYourLegs"

                : "Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.KneelToYourSide");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.DirtyCockInHand");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.HuffsTakesDeepBreath");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.HotBreathOnCock");

            if (p.Balls > 0 && p.HasVagina())
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.CupsBallsThenVagina");
            else if (p.Balls > 0)
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.CupsTenderBalls");
            else if (p.HasVagina())
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.CaressesPussyLips");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.CumDownWitchThroat");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.WitchesTakeTurnsDraining");

            p.Orgasm("Dick");

            p.Libido += 1;

            p.Sensitivity = Math.Max(0, p.Sensitivity - 1);

            if (friendly)
            {
                W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.WakeOutsideDungeonHours");

                SandWitchMobNotBadEnd();

                return;
            }

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.LiveAsWitchBitch");

            W("Scenes.DesertCave.GetMockedForSmallDongBySammitchMob.NewWitchesSavingGrace");

            p.Health = p.MaxHealth;

            Scenes.Scene.GameOver();
        }

        public static void MemeberedFolksFindTrueWuv()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            if (p.Health < 1)
                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.SlumpingUnderInjuries");
            else
                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.HowCanYouResist");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.ShortWitchAppraisesYou");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CannotAllowLeave");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.PainFadesNumbingWarmth");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.FondledAndFucked");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.AndFondled");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.SentencePeriodAgain");

            if (p.HasVagina())
                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.PussyWetsUndergarments");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.TattooedGirlEmerges");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CovenCannotLetLeave");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.FearTheyWillKill");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.PetiteWitchPurrsReassurance");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.PalmLeavesCockPleading");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.LeaderPromisesPatience");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.HandDiscoversOneCock");

            if (p.Cocks.Count == 2)
                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.AnotherGrabsOtherCock");
            else if (p.Cocks.Count > 2)
                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.OthersLatchOntoRest");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.StrokingStartsTooSoon");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.WorkOnYourStamina");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.BerniceDragsCagedCart");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.DaraAnswersInCharge");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.DaraShoosWitchesApproaches");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.DaraExplainsFoundGirl");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.DaraReleasesVineCunt");

            W(p.Cocks.Count > 1

                ? "Scenes.DesertCave.MemeberedFolksFindTrueWuv.AWord"

                : "Scenes.DesertCave.MemeberedFolksFindTrueWuv.YourWord");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.SmearsFlaccidPenis");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.TentacleBeastAppearance");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CreatureSpeaksQuadruplets");

            W(p.Balls > 0

                ? "Scenes.DesertCave.MemeberedFolksFindTrueWuv.BallsBrewFuckbatter"

                : "Scenes.DesertCave.MemeberedFolksFindTrueWuv.BodyBrewsFuckbatter");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CreatureApologizesMoans");

            if (p.Cocks.Count > 1)
            {
                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CunniesSnakeOtherPhallus");

                if (p.Cocks.Count > 2)
                    W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.EsSuffix");

                W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.GleefullySlurpsHardness");

                W(p.Cocks.Count == 2

                    ? "Scenes.DesertCave.MemeberedFolksFindTrueWuv.ItselfWord"

                    : "Scenes.DesertCave.MemeberedFolksFindTrueWuv.ThemselvesWord");
            }

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CuntSlurpingNoisily");

            W(p.Cocks.Count > 1

                ? "Scenes.DesertCave.MemeberedFolksFindTrueWuv.PistonSuffix"

                : "Scenes.DesertCave.MemeberedFolksFindTrueWuv.PistonWord");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.AndBobSplattering");

            W(p.Cocks.Count > 1

                ? "Scenes.DesertCave.MemeberedFolksFindTrueWuv.TheirWord"

                : "Scenes.DesertCave.MemeberedFolksFindTrueWuv.ItsWord");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.SalivaDripsOnCrotch");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CreatureCoreWobblesJello");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.GruntCumToVoices");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.OffersDrinkToHeal");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.CuntTrailsJuicesCheek");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv.NodIntoPussyGrows");

            p.Orgasm("Dick");

            p.Libido += 5;

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), MemeberedFolksFindTrueWuv2);
        }

        public static void MemeberedFolksFindTrueWuv2()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.OneWeekLaterHeading");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.DoorCracksOpenLight");

            W(p.Cocks.Count == 1

                ? "Scenes.DesertCave.MemeberedFolksFindTrueWuv2.FightingOverYourCock"

                : "Scenes.DesertCave.MemeberedFolksFindTrueWuv2.MilkingYourCocks");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.ForcedWetDreams");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.CumDrunkOnLove");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.JuiceGushesBetweenThighs");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.DaraSlamsDoorCollapses");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv2.BodyTrainedByCaptivity");

            p.Orgasm("Dick");

            p.Libido += 5;

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), MemeberedFolksFindTrueWuv3);
        }

        public static void MemeberedFolksFindTrueWuv3()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv3.WakeInToyRoom");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv3.NightWithOphelia");

            W("Scenes.DesertCave.MemeberedFolksFindTrueWuv3.OpheliaDeemedPurified");

            p.Orgasm("Dick");

            p.Libido += 5;

            Scenes.Scene.GameOver();
        }

        public static void SandMotherPOMenu(DungeonContext context)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchLeaveMeAlone) == 0)
                Scenes.Scene.AddButton(0, "Common.Button.StopAttacking".Localized(), () => UnfriendlyWitchToggle(context));
            else
                Scenes.Scene.AddButton(0, "Common.Button.StartAttacking".Localized(), () => UnfriendlyWitchToggle(context));

            if (CoC.Instance.Player.Lust >= 33)
            {
                if (CoC.Instance.Player.HasVagina())
                    Scenes.Scene.AddButton(1, "Common.Button.Scissor".Localized(), () => ScissorAndDrink(context));

                if (CoC.Instance.Player.Cocks.Count(c => c.Id.Path == "tentacle") >= 5)
                    Scenes.Scene.AddButton(2, "Common.Button.TentacleBang".Localized(), () => TentacleGangBang(context));

                if (CoC.Instance.Player.CockThatFits(50) >= 0)
                    Scenes.Scene.AddButton(3, "Common.Button.FuckHer".Localized(), () => FuckTheSandMothersCunt(context));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchLootTaken) < 5)
            {
                Scenes.Scene.AddButton(5, "Common.Button.GetLabova".Localized(), () => TakeLaBovaOrLactaid(context, false));

                Scenes.Scene.AddButton(6, "Common.Button.GetLactaid".Localized(), () => TakeLaBovaOrLactaid(context, true));
            }

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scenes.Scene.Go<Scenes.Vanilla.CampScene>());
        }

        public static void UnfriendlyWitchToggle(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => context.Write(key.Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchLeaveMeAlone) == 0)
            {
                W("Scenes.DesertCave.UnfriendlyWitchToggle.DemandTheyCease");

                W("Scenes.DesertCave.UnfriendlyWitchToggle.FavorWithdrawn");

                W(CoC.Instance.Player.Corruption < 50

                    ? "Scenes.DesertCave.UnfriendlyWitchToggle.NotWithoutProvocation"

                    : "Scenes.DesertCave.UnfriendlyWitchToggle.DoubtThatWouldStop");

                W("Scenes.DesertCave.UnfriendlyWitchToggle.CongratulateHerSlap");

                W("Scenes.DesertCave.UnfriendlyWitchToggle.ReedyPleading");

                CoC.Instance.Flags.Set(GameFlagIds.SandWitchLeaveMeAlone, 1);
            }
            else
            {
                W("Scenes.DesertCave.UnfriendlyWitchToggle.WelcomePesteringAgain");

                W("Scenes.DesertCave.UnfriendlyWitchToggle.MumblesAndRelents");

                W("Scenes.DesertCave.UnfriendlyWitchToggle.WouldntDreamOfIt");

                CoC.Instance.Flags.Set(GameFlagIds.SandWitchLeaveMeAlone, 0);
            }

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherPOMenu(context));
        }

        public static void TakeLaBovaOrLactaid(DungeonContext context, bool lactaid)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DesertCave.TakeLaBovaOrLactaid.CircleTowardChests".Localized());

            var item = Items.ItemRegistry.Create(
                // 物品 id 必须是语言无关的标识符，不能本地化
                new MonoCoC.Registry.Identifier("coc", lactaid ? "lactaid" : "labova"));

            CoC.Instance.Player.Inventory.Add(item, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SandWitchLootTaken,

                CoC.Instance.Flags.Get(GameFlagIds.SandWitchLootTaken) + 1);

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => SandMotherPOMenu(context));
        }

        public static void FriendlySandMotherFuck(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var times = flags.Get(GameFlagIds.TimesFriendlyFuckedSandMother);

            var x = p.CockThatFits(50);

            var y = p.CockThatFits2(50);

            void W(string key) => context.Write(key.Localized());

            if (times == 0)
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.AsksInPrivate");

                W("Scenes.DesertCave.FriendlySandMotherFuck.BlushingSorceress");

                W("Scenes.DesertCave.FriendlySandMotherFuck.CommentsOnTeats");

                W("Scenes.DesertCave.FriendlySandMotherFuck.AdmitsHerNeeds");
            }
            else
                W("Scenes.DesertCave.FriendlySandMotherFuck.OffersHerHelp");

            W("Scenes.DesertCave.FriendlySandMotherFuck.CastsRuneWard");

            W("Scenes.DesertCave.FriendlySandMotherFuck.OrdersUndress");

            W("Scenes.DesertCave.FriendlySandMotherFuck.YouUndressAlone");

            W("Scenes.DesertCave.FriendlySandMotherFuck.LiftsRobesHem");

            W("Scenes.DesertCave.FriendlySandMotherFuck.ServiceFromThrone");

            W("Scenes.DesertCave.FriendlySandMotherFuck.StepsUpEager");

            if (!p.IsTaur())
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.HoldingCockIn");

                W(y >= 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.OneWord"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.YourWord");

                W("Scenes.DesertCave.FriendlySandMotherFuck.HandWord");

                if (y >= 0)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.AnotherHandToo");

                W("Scenes.DesertCave.FriendlySandMotherFuck.MeatyCockWobbles");

                W(y < 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.ItWord"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.ThemWord");

                W("Scenes.DesertCave.FriendlySandMotherFuck.TwinPussiesContact");
            }
            else
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.DickWord");

                if (y >= 0)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.WombPluralSuffix");

                W("Scenes.DesertCave.FriendlySandMotherFuck.CockBobsBeneathBelly");

                W(y < 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.NosesFragment"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.TheyNoseFragment");

                W("Scenes.DesertCave.FriendlySandMotherFuck.TwinPussiesContact");
            }

            if (p.Tallness >= 90)
                W("Scenes.DesertCave.FriendlySandMotherFuck.TallNeedsKneeling");
            else if (p.Tallness <= 48)
                W("Scenes.DesertCave.FriendlySandMotherFuck.TooShortToReach");

            context.Write("Scenes.DesertCave.FriendlySandMotherFuck.LubeGlossedGates".Localized(p.CockHead(x)));

            if (y >= 0)
                W("Scenes.DesertCave.FriendlySandMotherFuck.OtherCockTip");

            W("Scenes.DesertCave.FriendlySandMotherFuck.HotMoistureGobbles");

            if (y >= 0)
                W("Scenes.DesertCave.FriendlySandMotherFuck.WombPluralSuffix");

            W("Scenes.DesertCave.FriendlySandMotherFuck.SighOfRelief");

            context.Write((y >= 0

                ? "Scenes.DesertCave.FriendlySandMotherFuck.TwoCocksFragment"

                : "Scenes.DesertCave.FriendlySandMotherFuck.YourParamWord").Localized(p.CockDescript(x)));

            W("Scenes.DesertCave.FriendlySandMotherFuck.SentencePeriodThird");

            if (p.IsTaur())
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.WhinnyRestForelegs");

                W(p.Tallness < 78

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.ForelegShoulders"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.ThroneTop");

                W("Scenes.DesertCave.FriendlySandMotherFuck.PumpsHerHard");

                if (y >= 0)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.WombPluralSuffix");

                W("Scenes.DesertCave.FriendlySandMotherFuck.GrabsPlatinumHair");
            }
            else
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.GroanRestHands");

                W(p.Tallness < 60

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.BottomTits"

                    : p.Tallness < 84

                        ? "Scenes.DesertCave.FriendlySandMotherFuck.TopTits"

                        : "Scenes.DesertCave.FriendlySandMotherFuck.HandsToShoulders");

                W("Scenes.DesertCave.FriendlySandMotherFuck.ManhoodThrustsDeep");

                if (y >= 0)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.WombPluralSuffix");

                W("Scenes.DesertCave.FriendlySandMotherFuck.ShudderingSorceress");

                W(p.Tallness < 72

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.KissToSilence"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.SucklesHeavyTit");

                W("Scenes.DesertCave.FriendlySandMotherFuck.FingersThroughHair");

                W("Scenes.DesertCave.FriendlySandMotherFuck.SecretionsRefill");
            }

            W("Scenes.DesertCave.FriendlySandMotherFuck.SucklingHeatGrip");

            W("Scenes.DesertCave.FriendlySandMotherFuck.LurchingConvulsions");

            if (y >= 0)
                W("Scenes.DesertCave.FriendlySandMotherFuck.WombPluralSuffix");

            W(y < 0

                ? "Scenes.DesertCave.FriendlySandMotherFuck.YLetter"

                : "Scenes.DesertCave.FriendlySandMotherFuck.IesSuffix");

            if (p.HasVagina())
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.InsidesGrindTight");

                if (p.Wetness() >= 4)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.SquirtingWord");

                W("Scenes.DesertCave.FriendlySandMotherFuck.PassageWord");
            }

            if (p.LactationQ() >= 200)
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.MilkEscapesNipples");

                if (p.LactationQ() < 200)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.LeaksWord");
                else if (p.LactationQ() < 1000)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.DripsWord");
                else
                    W("Scenes.DesertCave.FriendlySandMotherFuck.EruptsWord");

                W("Scenes.DesertCave.FriendlySandMotherFuck.SplattersThrone");
            }

            W("Scenes.DesertCave.FriendlySandMotherFuck.HeatRadiatesGroin");

            W("Scenes.DesertCave.FriendlySandMotherFuck.ParagraphBreak");

            if (p.Cocks.Count > 2 || (p.Cocks.Count == 2 && y < 0))
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.LoveBatterFires");

                if (p.Cocks.Count > 3 || (p.Cocks.Count == 3 && y < 0))
                    W("Scenes.DesertCave.FriendlySandMotherFuck.EsSuffix");

                W("Scenes.DesertCave.FriendlySandMotherFuck.SyrupyStrandsMix");
            }

            if (p.CumQ() < 250)
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.NumerousSpurts");

                W(y >= 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.PluralAreFragment"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.IsFragment");

                W("Scenes.DesertCave.FriendlySandMotherFuck.PainfulSquirting");

                W(y < 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.FatWhiteDrop"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.FatWhiteDrops");
            }
            else if (p.CumQ() < 1000)
                W("Scenes.DesertCave.FriendlySandMotherFuck.HeavyFuckBatter");
            else
            {
                W("Scenes.DesertCave.FriendlySandMotherFuck.FountainingJizz");

                W(y >= 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.PluralGoFragment"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.GoesFragment");

                W("Scenes.DesertCave.FriendlySandMotherFuck.StraightToWomb");

                if (y >= 0)
                    W("Scenes.DesertCave.FriendlySandMotherFuck.WombPluralSuffix");

                W("Scenes.DesertCave.FriendlySandMotherFuck.PregnantBellyRounds");

                W(y >= 0

                    ? "Scenes.DesertCave.FriendlySandMotherFuck.PluralGrowFragment"

                    : "Scenes.DesertCave.FriendlySandMotherFuck.GrowsFragment");

                W("Scenes.DesertCave.FriendlySandMotherFuck.SpoogeJetsHigh");
            }

            W("Scenes.DesertCave.FriendlySandMotherFuck.LegsNearlyFold");

            W("Scenes.DesertCave.FriendlySandMotherFuck.HopesForEncore");

            flags.Set(GameFlagIds.TimesFriendlyFuckedSandMother, times + 1);

            p.Orgasm("Generic");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => FriendlySandMotherMenu(context));
        }

        public static void LesboMilkFilling(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var times = flags.Get(GameFlagIds.TimesFriendlyFuckedSandMother);

            flags.Set(GameFlagIds.TimesFriendlyFuckedSandMother, times + 1);

            void W(string key) => context.Write(key.Localized());

            if (times == 0)
            {
                W("Scenes.DesertCave.LesboMilkFilling.CheckPrivacyThenInquire");

                W("Scenes.DesertCave.LesboMilkFilling.MotherBlushesFansCheek");

                W("Scenes.DesertCave.LesboMilkFilling.RemarkOnHerTeats");

                W("Scenes.DesertCave.LesboMilkFilling.SheAdmitsHerNeeds");
            }
            else
                W("Scenes.DesertCave.LesboMilkFilling.CheckPrivacyThenOffer");

            W("Scenes.DesertCave.LesboMilkFilling.PeelArmorDownEagerly");

            if (p.BiggestBreastSize() >= 10)
                W("Scenes.DesertCave.LesboMilkFilling.SwaySeductivePose");

            W("Scenes.DesertCave.LesboMilkFilling.FreeFromBindings");

            W(p.BiggestBreastSize() >= 2

                ? "Scenes.DesertCave.LesboMilkFilling.CupBothMammaries"

                : "Scenes.DesertCave.LesboMilkFilling.StrokeHandsDownChest");

            W("Scenes.DesertCave.LesboMilkFilling.StrideCloserClause");

            W("Scenes.DesertCave.LesboMilkFilling.SpellcasterCoosComeHere");

            if (p.IsTaur())
                W("Scenes.DesertCave.LesboMilkFilling.PranceUpThenDiverted");
            else
            {
                W("Scenes.DesertCave.LesboMilkFilling.CloseInDivertedAgain");

                if (p.Tallness > 72)
                    W("Scenes.DesertCave.LesboMilkFilling.GlowSuggestsHerMagic");
            }

            W("Scenes.DesertCave.LesboMilkFilling.TipDragsAcrossClit");

            W("Scenes.DesertCave.LesboMilkFilling.BiteLipStifleMoan");

            p.CuntChange(3, true, true, false);

            W("Scenes.DesertCave.LesboMilkFilling.YouPhrasePrefix");

            W(p.IsTaur()

                ? "Scenes.DesertCave.LesboMilkFilling.SqueezeOwnNipples"

                : "Scenes.DesertCave.LesboMilkFilling.GrabHerHead");

            W("Scenes.DesertCave.LesboMilkFilling.PullAwayThrustBack");

            if (!p.IsTaur())
                W("Scenes.DesertCave.LesboMilkFilling.GlowingHandHoldsYou");

            W("Scenes.DesertCave.LesboMilkFilling.HearFingersWorking");

            W("Scenes.DesertCave.LesboMilkFilling.PurrsWithLewdSounds");

            p.CuntChange(6, true, true, false);

            W(p.Pregnancy.Incubation == 0

                ? "Scenes.DesertCave.LesboMilkFilling.WombFloodingFull"

                : "Scenes.DesertCave.LesboMilkFilling.PregnantWombOverflows");

            p.CuntChange(9, true, true, false);

            W("Scenes.DesertCave.LesboMilkFilling.HissInDelight");

            W(p.IsTaur()

                ? "Scenes.DesertCave.LesboMilkFilling.WishToRubBelly"

                : "Scenes.DesertCave.LesboMilkFilling.RubSwollenBelly");

            W("Scenes.DesertCave.LesboMilkFilling.VaginaComesAlive");

            W("Scenes.DesertCave.LesboMilkFilling.MotherGaspsAboutMilk");

            W("Scenes.DesertCave.LesboMilkFilling.NodThatItsGood");

            W(p.Pregnancy.Incubation == 0

                ? "Scenes.DesertCave.LesboMilkFilling.BellySwellsRound"

                : "Scenes.DesertCave.LesboMilkFilling.FountainBackwash");

            W("Scenes.DesertCave.LesboMilkFilling.MotherBabblesSquirt");

            W("Scenes.DesertCave.LesboMilkFilling.QueenGathersSexjuice");

            W("Scenes.DesertCave.LesboMilkFilling.DisentangleWithLick");

            W("Scenes.DesertCave.LesboMilkFilling.SheSuggestsNextTime");

            p.Orgasm("Vaginal");

            Scenes.Scene.AddButton(0, "Common.Button.Back".Localized(), () => FriendlySandMotherMenu(context));
        }

        public static void LoseToTheSandMother()
        {
            if (CoC.Instance.Player.HasCock())
                LoseToSandMotherBadEnd();
            else
                LoseToSandMother();
        }

        public static void LoseToSandMotherBadEnd()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.StumblingBackIntoWall");

            W(p.Health < 1

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.WoundsStealStrength"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.LustWrecksBody");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.YouWord");

            W(p.Health < 1

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.SpitefullyAdmireForm"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.HungrilyTouchYourself");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.DisdainfulSniff");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.ManicuredFootProds");

            W(p.Health < 1

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.RapidlySwellingErection"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.TurgidSlatheredBoner");

            if (p.Cocks.Count > 1)
                W("Scenes.DesertCave.LoseToSandMotherBadEnd.SLetter");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.SorceressAsksThoughts");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.TrySquirmFree");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.MindStillResists");

            W(p.Corruption < 66

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.ArentCorrupt"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.DontServeDemons");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.LookingUpPleadingly");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.GleefullyPumpsYou");

            if (p.Cocks.Count > 1)
                W("Scenes.DesertCave.LoseToSandMotherBadEnd.MovesToSecondCock");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.TorturousStrokesStop");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.CaptorSummonsSisters");

            if (p.HasVagina())
                W("Scenes.DesertCave.LoseToSandMotherBadEnd.IgnoredWomanhood");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.ForeignFemaleMusk");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.AnticipatoryFlexing");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.JismDrenchesBody");

            if (p.CumQ() >= 500)
                W("Scenes.DesertCave.LoseToSandMotherBadEnd.PuddleDeepens");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.RobedFigureEnters");

            W(CoC.Instance.Flags.Get(GameFlagIds.CumWitchDefeated) > 0

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.CumWitchReturns"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.CumWitchIdentified");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.WhisperedExchange");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.ChocolateWomanDisrobes");

            W(p.Balls > 0

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.TestesWord"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.ProstateWord");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.NowhereToGo");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.SlipperyBallsSlide");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.NowItsGay");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.SentencePeriod");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.MagicHumBegins");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.MemoriesFragmentAway");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.MagicianPullsAway");

            W(p.Balls > 0

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.EmptyWord"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.EmptiesWord");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.PearlyFlowFails");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.SilkyVoiceProclaims");

            W(!p.HasVagina()

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.InnardsWord"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.FoldsWord");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.HeavingBallsSwell");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.CockheadSealsDeep");

            if (!p.HasVagina())
                p.ButtChange(12, true, true);
            else
                p.CuntChange(12, true, true, false);

            W(!p.HasVagina()

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.BowelsWord"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.WombWord");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.ForceThickensMiddle");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.ThoughtsRegather");

            W(!p.HasVagina()

                ? "Scenes.DesertCave.LoseToSandMotherBadEnd.ButtToken"

                : "Scenes.DesertCave.LoseToSandMotherBadEnd.UterusWord");

            W("Scenes.DesertCave.LoseToSandMotherBadEnd.ContentedSighWithdrawal");

            p.Orgasm("VaginalAnal");

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), MenLoseToQueenMotherVolI);
        }

        public static void MenLoseToQueenMotherVolI()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.MenLoseToQueenMotherVolI.ChampionBodyChanged");

            W("Scenes.DesertCave.MenLoseToQueenMotherVolI.DesertWitchesWageWar");

            W(CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0

                ? "Scenes.DesertCave.MenLoseToQueenMotherVolI.AlliesReachFortress"

                : "Scenes.DesertCave.MenLoseToQueenMotherVolI.ILetter");

            W("Scenes.DesertCave.MenLoseToQueenMotherVolI.DemonsDefeated");

            W("Scenes.DesertCave.MenLoseToQueenMotherVolI.DesertsBecomeJungle");

            Scenes.Scene.GameOver();
        }

        public static void LoseToSandMother()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.LoseToSandMother.DizzyWorldFades");

            W(p.Health < 1

                ? "Scenes.DesertCave.LoseToSandMother.AgonyEndsFight"

                : "Scenes.DesertCave.LoseToSandMother.LustDistractsFight");

            W("Scenes.DesertCave.LoseToSandMother.FallBackAgainstWall");

            W(p.Health < 1

                ? "Scenes.DesertCave.LoseToSandMother.GritTeethInAnger"

                : "Scenes.DesertCave.LoseToSandMother.BloodRacesPanting");

            W("Scenes.DesertCave.LoseToSandMother.AdmiresHerPhysique");

            W("Scenes.DesertCave.LoseToSandMother.GestureStripsArmor");

            if (!p.HasVagina())
            {
                W("Scenes.DesertCave.LoseToSandMother.GroinFormsLips");

                p.CreateVagina();

                p.SetClitLength(0.25f);
            }

            W("Scenes.DesertCave.LoseToSandMother.VaginaGlancedAt");

            if (p.Wetness() <= 2)
                W("Scenes.DesertCave.LoseToSandMother.GrowsMoist");
            else if (p.Wetness() <= 4)
                W("Scenes.DesertCave.LoseToSandMother.DripsWithArousal");
            else
                W("Scenes.DesertCave.LoseToSandMother.DroolsObscenely");

            W("Scenes.DesertCave.LoseToSandMother.OppositionSilenced");

            W("Scenes.DesertCave.LoseToSandMother.ResistanceFails");

            W(p.Health < 1

                ? "Scenes.DesertCave.LoseToSandMother.AbusedBodyPain"

                : "Scenes.DesertCave.LoseToSandMother.LustFillsSenses");

            W("Scenes.DesertCave.LoseToSandMother.HindersAllStruggles");

            W("Scenes.DesertCave.LoseToSandMother.KneelsProdsPussy");

            if (p.BreastRows.Count == 1)
            {
                W("Scenes.DesertCave.LoseToSandMother.ExtraSetForMilk");

                p.CreateBreastRow();
            }
            else if (p.BreastRows.Count == 2)
                W("Scenes.DesertCave.LoseToSandMother.AlreadyEnoughBreasts");
            else
                W("Scenes.DesertCave.LoseToSandMother.NeverHadFourBreasts");

            W("Scenes.DesertCave.LoseToSandMother.SqueezeEarnsYelp");

            if (p.BiggestBreastSize() < 3)
                W("Scenes.DesertCave.LoseToSandMother.BreastsMustGrow");
            else if (p.BiggestBreastSize() >= 14)
                W("Scenes.DesertCave.LoseToSandMother.ExpectMoreMilk");

            W("Scenes.DesertCave.LoseToSandMother.FingersTeaseLabia");

            W("Scenes.DesertCave.LoseToSandMother.OrdersCumWitchFetched");

            if (p.BiggestBreastSize() < 5)
                W("Scenes.DesertCave.LoseToSandMother.JiggleWord");
            else if (p.BiggestBreastSize() < 10)
                W("Scenes.DesertCave.LoseToSandMother.BounceWord");
            else
                W("Scenes.DesertCave.LoseToSandMother.QuakeWord");

            if (p.LactationQ() >= 50)
                W("Scenes.DesertCave.LoseToSandMother.MilkStartsPouring");

            W("Scenes.DesertCave.LoseToSandMother.EachFingerTwitch");

            W("Scenes.DesertCave.LoseToSandMother.FingersPressDeeper");

            W("Scenes.DesertCave.LoseToSandMother.RobedFigureArrives");

            W(CoC.Instance.Flags.Get(GameFlagIds.CumWitchDefeated) == 1

                ? "Scenes.DesertCave.LoseToSandMother.CumWitchRegainedStrength"

                : "Scenes.DesertCave.LoseToSandMother.SummonedCumWitch");

            W("Scenes.DesertCave.LoseToSandMother.WhisperedDisagreement");

            W(p.BreastRows.Count <= 2

                ? "Scenes.DesertCave.LoseToSandMother.AdmiringExtraBreasts"

                : "Scenes.DesertCave.LoseToSandMother.AdmiringBreastRows");

            W("Scenes.DesertCave.LoseToSandMother.SentencePeriod");

            W("Scenes.DesertCave.LoseToSandMother.ChocolateWomanLowers");

            p.CuntChange(12, true, true, false);

            W("Scenes.DesertCave.LoseToSandMother.ChantingWithThrusts");

            W("Scenes.DesertCave.LoseToSandMother.CrowdedInOwnSkin");

            W("Scenes.DesertCave.LoseToSandMother.PanicCreepsIn");

            W("Scenes.DesertCave.LoseToSandMother.EbonyShaftThrobs");

            W("Scenes.DesertCave.LoseToSandMother.CumPoursAfterOrgasm");

            W("Scenes.DesertCave.LoseToSandMother.SandMotherAsksDone");

            W("Scenes.DesertCave.LoseToSandMother.ChocolateWitchStandsUp");

            p.Orgasm("Vaginal");

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), SandMotherBadEndsLadiesEpilogue);
        }

        public static void SandMotherBadEndsLadiesEpilogue()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.ChampionBecomesWitch");

            if (p.BreastRows.Count > 2)
                W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.ExtraBreastsAdvantage");

            if (p.BiggestBreastSize() >= 22)
                W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.UnrivaledMilkOutput");

            W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.LifeBeganDecadeAgo");

            if (p.HasPerk(Datas.Vanilla.PerkIds.BroodMother))
                W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.RapidPregnancies");

            if (p.HasPerk(Datas.Vanilla.PerkIds.BroodMother) && p.BiggestBreastSize() >= 22)
                W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.ChampionBecomesBroodmare");

            W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.WarTwoYearsLater");

            W(CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0

                ? "Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.AlliesReachFortress"

                : "Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.ILetter");

            W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.DemonsDefeated");

            W("Scenes.DesertCave.SandMotherBadEndsLadiesEpilogue.DesertsBecomeJungle");

            Scenes.Scene.GameOver();
        }

        public static void TDMsLoseToCumWitchScene()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            var x = p.CockThatFits(50);

            if (x < 0) x = p.SmallestCockIndex();
            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.RobesDiscardedNearby");

            if (p.Cocks.Count > 1)
            {
                W("Scenes.DesertCave.TDMsLoseToCumWitchScene.EntrenchedInVagina");

                W(p.Cocks.Count == 2

                    ? "Scenes.DesertCave.TDMsLoseToCumWitchScene.OtherPhallus"

                    : "Scenes.DesertCave.TDMsLoseToCumWitchScene.RemainingCocks");

                W("Scenes.DesertCave.TDMsLoseToCumWitchScene.FutaSmilesMysteriously");
            }

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.AssailantConcentrates");

            if (p.Cocks.Count > 1)
                W("Scenes.DesertCave.TDMsLoseToCumWitchScene.OneOfPrefix");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.YoursWord");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.FearSlithersSpine");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.TeasesCockHead");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.AttentionDrawnDown");

            W(p.Balls > 0

                ? "Scenes.DesertCave.TDMsLoseToCumWitchScene.SackStillFelt"

                : "Scenes.DesertCave.TDMsLoseToCumWitchScene.ProstateBehindBase");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.OddTinglingTightness");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.LeansCloseWhispers");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.GripsLengthJerking");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.BlackBarExperience");

            W(p.Sensitivity < 70

                ? "Scenes.DesertCave.TDMsLoseToCumWitchScene.HersMoreSensitive"

                : "Scenes.DesertCave.TDMsLoseToCumWitchScene.HersLessSensitive");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.DribblingPreTingles");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.SlappingNoiseThrusting");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.WitchCommandsHandsOff");

            if (p.CumQ() < 200)
                W("Scenes.DesertCave.TDMsLoseToCumWitchScene.JizzStreamsFlyUp");
            else if (p.CumQ() < 3000)
                W("Scenes.DesertCave.TDMsLoseToCumWitchScene.CollectiveCockFlow");
            else
                W("Scenes.DesertCave.TDMsLoseToCumWitchScene.EverythingGoesWhite");

            W(Configuration.OptionStore.Instance.Get("silly", 0) == 1

                ? "Scenes.DesertCave.TDMsLoseToCumWitchScene.BodyAndBalls"

                : "Scenes.DesertCave.TDMsLoseToCumWitchScene.BodyAndSpirit");

            W("Scenes.DesertCave.TDMsLoseToCumWitchScene.SpunkCoveredRises");

            p.Orgasm("Dick");

            p.Strength = Math.Max(1, p.Strength - 1);

            p.CumMultiplier += 1;

            if (p.CumQ() >= 200) p.Strength = Math.Max(1, p.Strength - 1);
            if (p.CumQ() >= 3000) p.Strength = Math.Max(1, p.Strength - 1);
        }

        public static void TooBigCumWitchLossFacial()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.RuefullyAcceptsFacial");

            W(p.Corruption < 33

                ? "Scenes.DesertCave.TooBigCumWitchLossFacial.ShuddersAtKnowledge"

                : p.Corruption < 66

                    ? "Scenes.DesertCave.TooBigCumWitchLossFacial.CatchesLickingLips"

                    : "Scenes.DesertCave.TooBigCumWitchLossFacial.GivesLewdLook");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.SecretingMoistureAffects");

            W(p.HasFuckableNipples()

                ? "Scenes.DesertCave.TooBigCumWitchLossFacial.InWord"

                : "Scenes.DesertCave.TooBigCumWitchLossFacial.OnWord");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.NipplesSwellingWithBlood");

            W(p.Balls > 0

                ? "Scenes.DesertCave.TooBigCumWitchLossFacial.MagicFattensBalls"

                : p.HasVagina()

                    ? "Scenes.DesertCave.TooBigCumWitchLossFacial.TinglingFillsVagina"

                    : "Scenes.DesertCave.TooBigCumWitchLossFacial.TaintTingleProstate");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.LosesControlCums");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.WatchesUrethraDilate");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.WitchSmilesHoldingShaft");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.CaptorKeepsHumping");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.JismSplattersWithin");

            W("Scenes.DesertCave.TooBigCumWitchLossFacial.WitchClimbsOff");

            p.CumMultiplier += p.CumQ() < 1000 ? 5 : 2;

            if (p.BallSize < 7) p.BallSize += 1;
            p.Orgasm("Dick");

            p.Libido += 1;

            p.Sensitivity += 1;
        }

        public static void TooBigCumWitchLossNoFacial()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.TooBigCumWitchLossNoFacial.RefusesFacial");

            W("Scenes.DesertCave.TooBigCumWitchLossNoFacial.WitchGrowlsForces");

            W("Scenes.DesertCave.TooBigCumWitchLossNoFacial.SimilarToYours");

            W("Scenes.DesertCave.TooBigCumWitchLossNoFacial.SmilesCruellyTingles");

            W("Scenes.DesertCave.TooBigCumWitchLossNoFacial.FirstSquirtSpray");

            W("Scenes.DesertCave.TooBigCumWitchLossNoFacial.ExhaustionSlumber");

            if (p.BallSize > 5) p.BallSize -= p.BallSize * 0.333f;
            p.CumMultiplier *= 0.75f;

            p.Orgasm("Dick");
        }

        public static void SavinMakesAwesomeFemdom()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W(p.Health < 1

                ? "Scenes.DesertCave.SavinMakesAwesomeFemdom.TopplesIntoSands"

                : "Scenes.DesertCave.SavinMakesAwesomeFemdom.LustSurgesLegsCollapse");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.PinnedStrugglesStripped");

            if (p.BreastRows.Count > 1)
            {
                W("Scenes.DesertCave.SavinMakesAwesomeFemdom.FondlesBothBreasts");

                if (p.LactationQ() >= 200)
                    W("Scenes.DesertCave.SavinMakesAwesomeFemdom.MilkStreamsDownChest");

                W("Scenes.DesertCave.SavinMakesAwesomeFemdom.SentencePeriod");
            }
            else if (p.LactationQ() >= 200)
                W("Scenes.DesertCave.SavinMakesAwesomeFemdom.FingersMilkTrickle");
            else
                W("Scenes.DesertCave.SavinMakesAwesomeFemdom.TweaksNipplesShivers");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.WorksWayDownBelly");

            if (p.HasCock())
            {
                W("Scenes.DesertCave.SavinMakesAwesomeFemdom.HalfErectOpensRobes");

                if (p.LongestCockLength() < 10)
                    W("Scenes.DesertCave.SavinMakesAwesomeFemdom.OvershadowsTinyRod");
                else if (p.LongestCockLength() < 15)
                    W("Scenes.DesertCave.SavinMakesAwesomeFemdom.SizesNearlyEqual");
                else
                    W("Scenes.DesertCave.SavinMakesAwesomeFemdom.SeemsTinyVersusYours");

                W("Scenes.DesertCave.SavinMakesAwesomeFemdom.ChucklesPumpsBoth");
            }

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.LicksEbonyLipsLust");

            W(p.IsGoo() || p.IsDrider() || p.IsTaur() || p.HasTailInsteadOfLegs()

                ? "Scenes.DesertCave.SavinMakesAwesomeFemdom.ShiftsDownBody"

                : "Scenes.DesertCave.SavinMakesAwesomeFemdom.SpreadsLegsOverShoulders");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.GraspsThrobbingCock");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.TurnsToVagina");

            p.CuntChange(12, true, false, true);

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.CoosStrokesHair");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.HooksFingersNeck");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.RollsHipsSlowly");

            W("Scenes.DesertCave.SavinMakesAwesomeFemdom.LostInBliss");

            Scenes.Scene.AddButton(0, "Common.Button.Resist".Localized(), ResistSavinStuff);

            Scenes.Scene.AddButton(1, "Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), DoNotResistSavin);
        }

        public static void ResistSavinStuff()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.ResistSavinStuff.PushesWitchStraddles");

            W("Scenes.DesertCave.ResistSavinStuff.GroansClutchingHips");

            W("Scenes.DesertCave.ResistSavinStuff.SlamsDownScreaming");

            W("Scenes.DesertCave.ResistSavinStuff.GroansFallsBackDunes");

            W("Scenes.DesertCave.ResistSavinStuff.NodsCollapsesAtop");

            p.Orgasm("Vaginal");

            p.Sensitivity += 2;

            KnockUpSandWitch(p);
        }

        public static void DoNotResistSavin()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            W("Scenes.DesertCave.DoNotResistSavin.NeedsToBeBred");

            W("Scenes.DesertCave.DoNotResistSavin.NodsEagerlyAtWords");

            W("Scenes.DesertCave.DoNotResistSavin.GaspsMilkSputters");

            W("Scenes.DesertCave.DoNotResistSavin.EndlessOrgasmFades");

            W("Scenes.DesertCave.DoNotResistSavin.NodsCollapsesAtop");

            p.Orgasm("Vaginal");

            p.Sensitivity += 2;

            KnockUpSandWitch(p);
        }

        public static void KnockUpSandWitch(Creatures.Player p)
        {

            p.KnockUp(Pregnancy.PregnancyIds.SandWitch, 360, 90);
        }

        public static void RepeatLoseToCumWitchForDudes()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            var p = CoC.Instance.Player;

            void W(string key) => Scenes.Scene.OutputText(key.Localized());

            if (p.Health < 1)
            {
                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.CollapseUnderInjuries");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.CaressRemovingArmor");

                W(p.Lust / p.MaxLust < 0.4f

                    ? "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.FlaccidWord"

                    : p.Lust / p.MaxLust < 0.6f

                        ? "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SemiFlaccidWord"

                        : p.Lust / p.MaxLust < 0.7f

                            ? "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SemiHardWord"

                            : "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.TurgidWord");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.PumpingWithDeftStrokes");
            }
            else
            {
                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.RipArmorOntoBack");

                if (p.Balls > 0)
                    W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.BallsQuakingWithNeed");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.EbonyFutanariLaughs");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.KneelingRunsHands");
            }

            if (p.CockThatFits(50) < 0)
            {
                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ClimbsAtopBody");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.OnyxMageFrots");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ChestWord");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SpellcasterComments");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SnapsFingersStones");

                if (p.TotalNipples() > 2)
                    W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.TwoOfPrefix");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.YourNipples");

                if (p.HasFuckableNipples())
                    W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SinksIntoMoistHoles");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.TwoVibratorsRiding");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ContinualTeasingBreasts");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.FrownThenDopeySmile");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.RollingThePrefix");

                if (p.Balls > 0)
                    if (p.BallSize < 2)
                        W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.PetiteWord");
                    else if (p.BallSize >= 5)
                        W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.WeightyWord");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.OrbsAcrossSoles");

                W(p.Balls > 0

                    ? "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.OrbsAcrossSoles"

                    : p.HasVagina()

                        ? "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ToesThroughFolds"

                        : "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SoftlyStrokingTaint");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.GroaningWitchAsks");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.AvoidFacialTradeoff");

                Scenes.Scene.AddButton(0, "Common.Button.Facial".Localized(), TooBigCumWitchLossFacial);

                Scenes.Scene.AddButton(1, "Common.Button.NoFacial".Localized(), TooBigCumWitchLossNoFacial);

                return;
            }

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ClimbsAtopBodyAgain");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SheSuggestsFrotting");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SqueezesCheeksErection");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.HerVoicePurrs");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.FeelsGoodEruptionNear");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ToldYouFocusNipples");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SheIsSoRight");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.BreastsStopSwaying");

            W(p.Balls > 0

                ? "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.HandSecuresYourSack"

                : "Scenes.DesertCave.RepeatLoseToCumWitchForDudes.HandPressesOnTaint");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.LoudFirmDazzledWords");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.VibratingPleasureSizzles");

            if (p.Cocks.Count > 1)
                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.OneInsideHer");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.ThickDripFromNethers");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.MostRelaxingOrgasm");

            if (p.Cocks.Count > 1)
            {
                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.YourExtraErection");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.SSpurtWord");

                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.CreamUpEbonyArch");
            }
            else
                W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.HerBouncingErection");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.HerClenchingClimax");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.LongEndlessCumming");

            W("Scenes.DesertCave.RepeatLoseToCumWitchForDudes.BlackOutFromMagic");

            p.Orgasm("Dick");

            p.Sensitivity += 5;

            if (p.CumQ() < 60000)
                p.CumMultiplier += 2;
        }
    }
}
