using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class BogEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:bog",

            CommonEncounters.Table,

            IEncounter.Create("phoukahalloween", _ => 1f, _ =>
            {
                var now = DateTime.Now;

                return CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) > 0 &&

                       now.Year > CoC.Instance.Flags.Get(GameFlagIds.TreacleMineYearDone) &&

                       ((now.Month == 10 && now.Day >= 24) || (now.Month == 11 && now.Day <= 7));
            }, _ => SceneInstance<PhoukaScene>.Get().PhoukaHalloween()),

            IEncounter.Create("chest", _ => 0.1f, _ => !CoC.Instance.Player.HasKeyItem("Camp - Murky Chest"), _ => FindMurkyChest()),

            IEncounter.Create("frogGirlScene", _ => 1f, _ => true, _ => Scene.Go<FrogGirlScene>()),

            IEncounter.Create("phouka", _ => 1f, _ => true, _ => Scene.Go<PhoukaScene>()),

            IEncounter.Create("chameleon", _ => 1f, _ => true, _ => Scene.Go<ChameleonGirlScene>()),

            IEncounter.Create("anneMarie", _ => 1f, _ => true, _ => Scene.Go<AnneMarieScene>()),

            IEncounter.Create("parasite", _ => 1f, _ => true, _ => Scene.Go<ParasiteScene>()),

            IEncounter.Create("inf_chameleon",

                _ => Configuration.OptionStore.Instance.Get("parasites", 0) / 2f,

                _ => Configuration.OptionStore.Instance.Get("parasites", 0) != 0

                     && !CoC.Instance.Player.HasStatusEffect(
                         Datas.Vanilla.StatusEffectIds.ParasiteEel),

                _ => Scene.Go<InfestedChameleonGirlScene>()),

            IEncounter.Create("sylvia", _ => 1f, _ => true, _ => Scene.Go<SylviaScene>()),

            IEncounter.Create("bogtemple", _ => 1f, _ => true, _ => Scene.Go<BogTempleScene>()),

            IEncounter.Create("walk", _ => 0.2f, _ => true, _ => Walk()),

            IEncounter.Create("BogExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.Bog.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void Walk()
    {
        Scene.ClearOutput();

        Scene.OutputText("Scenes.Bog.Walk.UneventfulMuckWalk".Localized());

        Scene.Go<CampScene>();
    }

    public static void FindMurkyChest()
    {
        var gems = 200 + Random.Shared.Next(300);

        Scene.ClearOutput();

        Scene.OutputText("While you're minding your own business, you spot a waterlogged chest. You wade in the murky waters until you finally reach the chest. As you open the chest, you find " + gems + "Common.Text.GemsInsideTheChestYouPocket".Localized());

        CoC.Instance.Player.CreateKeyItem("Camp - Murky Chest");

        CoC.Instance.Player.Gems += gems;

        Scene.OutputText("Common.Text.PgBYouNowHaveMore".Localized());

        Scene.Go<CampScene>();
    }
}
}