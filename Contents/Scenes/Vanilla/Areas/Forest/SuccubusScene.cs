using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<SuccubusScene>("coc:succubus_scene")]
    public class SuccubusScene : Scene
    {

        public override void Entry()
        {

        }

        public void EncounterSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Succubusscene.EncounterSuccubus.IvorySuccubusSpotted".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySuccubus);

            if (Player.HasCock())
            {
                OutputText("Scenes.Succubusscene.EncounterSuccubus.CockOffer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightAgainstSuccubus);

                AddButton(1, "Common.Button.Accept".Localized(), () => LoseToSuccubus(false));

                AddButton(4, "Common.Button.Run".Localized(), TryToFlee);
            }
            else
            {
                OutputText("Scenes.Succubusscene.EncounterSuccubus.ChargesIntoFight".Localized());

                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "succubus")), () => Go<CampScene>());
            }
        }

        public void FightAgainstSuccubus()
        {

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "succubus")), () => Go<CampScene>(), canFlee: false);
        }

        public void WinAgainstSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Succubusscene.WinAgainstSuccubus.FallsToKnees".Localized(
                (CombatController.Instance?.Context.Enemy.Health ?? 0) <= 0 ? "beaten" : "aroused"));

            if (Player.HasCock() && Player.Lust >= 33)
            {
                OutputText("Scenes.Succubusscene.WinAgainstSuccubus.RapePrompt".Localized());

                AddButton(0, "Common.Button.RapeHer".Localized(), WinAgainstSuccubusRape);

                AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
            }
            else
                CombatScene.CleanupAfterCombat();
        }

        public void WinAgainstSuccubusRape()
        {
            ClearOutput();

            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.MovesOverStripping".Localized(
                Player.ClothedOrNakedLower(", stripping out of your " + Player.ArmorDescript() + ", exposing your " + Player.CockDescript() + "", " while your " + Player.CockDescript() + " swings freely")
            ));

            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.GrindPelvis".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.AllCocksThrust".Localized());
            else
                OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.SingleCockThrust".Localized());

            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.HugeLoad".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.SecondCockCram".Localized());

            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.SlowRapeGloat".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 3f));

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToSuccubus(bool fromBattle = true)
        {
            ClearOutput();

            if (fromBattle)
                OutputText("Scenes.Succubusscene.LoseToSuccubus.GivesIn".Localized(
                    Player.Health <= 0 ? "beaten" : "aroused"));

            if (Player.HasCock())
            {
                OutputText("Scenes.Succubusscene.LoseToSuccubus.KissDeepThroat".Localized());

                OutputText("Scenes.Succubusscene.LoseToSuccubus.RidesYou".Localized());

                OutputText("Scenes.Succubusscene.LoseToSuccubus.AwakensExhausted".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 3f));

                if (fromBattle)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(2));
            }
            else
            {
                OutputText("Scenes.Succubusscene.LoseToSuccubus.CaressesFace".Localized(
                    Player.Libido + Player.Corruption < 80 ? "reluctantly" : "eagerly"));

                OutputText("Scenes.Succubusscene.LoseToSuccubus.LeavesBottle".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 3f));

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.IncubiDraft), 1);

CombatScene.CleanupAfterCombat();
            }
        }

        public void TryToFlee()
        {
            ClearOutput();

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "succubus")), () => Go<CampScene>());

            CombatController.Instance!.Run();
        }

    }
}