using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Plains;
using MonoCoC.Contents.Scenes.Vanilla.Places;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class PlainsEncounters
    {

        public static void WalkingPlains(EncounterContext context)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Scenes.Plains.WalkingPlainsStatBoost.UneventfulPlainsRun".Localized());

            if (context.Random.Next(2) == 0)
                if (context.Random.Next(2) == 0 && context.Player.Toughness < 50)
                {
                    context.Write("Scenes.Plains.WalkingPlainsStatBoost.PlainsToughnessGain".Localized());

                    context.Player.Toughness += 0.5f;

                    context.Player.Libido -= 1;
                }
                else if (context.Player.Speed < 50)
                {
                    context.Write("Scenes.Plains.WalkingPlainsStatBoost.PlainsSpeedGain".Localized());

                    context.Player.Speed += 0.5f;

                    context.Player.Libido -= 1;
                }

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
        }

        public static readonly IEncounter Table = IEncounter.Group(
            "plains",

            CommonEncounters.Table,

            IEncounter.Create("sheila_xp3", _ => 1f, _ => true, _ => SceneInstance<SheilaScene>.Get().SheilaEncounterRouter()),

            ItemEncounters.Find(
                "ovielix",

                "Scenes.Plains.FindOviElix.DiscardedOviBottle",

                () => ItemRegistry.Create(ConsumableIds.OviElixir),

                0.5f),

            ItemEncounters.Find(
                "kangaft",

                "Scenes.Plains.FindKangaFruit.KangaFruitPlant",

                () => ItemRegistry.Create(ConsumableIds.KangaFruit),

                0.5f),

            IEncounter.Create("candy_cane", _ => 1f, _ => Holidays.IsSaturnalia() && DateTime.Now.Year > CoC.Instance.Flags.Get(GameFlagIds.CandyCaneYearMet), _ => Scene.Go<XmasMiscScene>()),

            IEncounter.Create("polar_pete", _ => 1f, _ => Holidays.IsSaturnalia() && DateTime.Now.Year > CoC.Instance.Flags.Get(GameFlagIds.PolarPeteYearMet), _ => SceneInstance<XmasMiscScene>.Get().PolarPete()),

            IEncounter.Create("niamh", _ => 1f, _ => true, _ => SceneInstance<NiamhScene>.Get().TelAdreNiamh()),

            IEncounter.Create("owca", _ => 0.3f, _ => true, _ => Scene.Go<OwcaScene>()),

            IEncounter.Create("bazaar", _ => 1f, _ => true, _ => SceneInstance<BazaarScene>.Get().FindBazaar()),
            // 原版 Plains.hx:86 helXizzy：Hel 与 Isabella 的三明治遭遇
            IEncounter.Create("helXizzy", _ => 0.2f, _ =>
            {
                var flags = CoC.Instance.Flags;

                var player = CoC.Instance.Player;

                return flags.Get(GameFlagIds.IsabellaCampApproached) != 0

                    && flags.Get(GameFlagIds.IsabellaMet) != 0

                    && flags.Get(GameFlagIds.HelFuckbuddy) == 1

                    && flags.Get(GameFlagIds.IsabellaAngryAtPcCounter) == 0

                    && !SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower()

                    && (player.Tallness <= 78 || flags.Get(GameFlagIds.IsabellaOkayWithTallFolks) > 0);
            }, _ =>
            {
                var hel = SceneInstance<HelScene>.Get();

                var state = CoC.Instance.Flags.Get(GameFlagIds.HelIsabellaThreesomeEnabled);

                if (state == 0)
                    hel.SalamanderXIsabellaPlainsIntro();
                else if (state == 1)
                    hel.IsabellaXHelThreeSomePlainsStart();
            }),

            IEncounter.Create("coc:gnoll", _ => 0.5f, _ => true, _ => Scene.Go<GnollSpearThrowerScene>()),

            IEncounter.Create("gnoll2", _ => 0.5f, _ => true, _ => Scene.Go<GnollScene>()),

            IEncounter.Create("bunny", _ => 1f, _ => true, _ => Scene.Go<BunnyGirlScene>()),

            IEncounter.Create("isabella", _ => 1f, _ => true, _ => SceneInstance<IsabellaScene>.Get().IsabellaGreeting()),

            IEncounter.Create("helia", _ => 1f, _ => !SceneInstance<HelScene>.Get().FollowerHel(), _ => SceneInstance<HelScene>.Get().EncounterAJerkInThePlains()),

            IEncounter.Create("satyr", _ => 1f, _ => true, _ => Scene.Go<SatyrScene>()),

            IEncounter.Create("sheila", _ => 1f, _ => true, _ => SceneInstance<SheilaScene>.Get().SheilaEncounterRouter()),
            // 原版 Plains.hx:141 whitney：农场开启、未腐化、尚未遇到侏儒时触发 whitneyFightsGnoll
            IEncounter.Create("whitney", _ => 0.1f, _ =>
            {
                var farm = SceneInstance<FarmScene>.Get();

                return farm.FarmEnabled() && !farm.FarmCorrupt() && !FarmScene.Save.GetBool("coc:gnoll");
            }, _ => SceneInstance<FarmScene>.Get().WhitneyFightsGnoll()),

            IEncounter.Create("walkingPlains", _ => 1f, _ => true, WalkingPlains));
    }
}