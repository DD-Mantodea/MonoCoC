using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag
{
    [Register<VolcanicGolemScene>("coc:volcanic_golem")]
    public class VolcanicGolemScene : Scene
    {

        public override void Entry()
        {
            VolcanicGolemIntro();
        }

        public void VolcanicGolemIntro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.Metvolcanicgolem) <= 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.Volcanicgolemhp, 10000);

                OutputText("Scenes.Volcanicgolemscene.VolcanicGolemIntro.StatueInvestigation".Localized());

                OutputText("Scenes.Volcanicgolemscene.VolcanicGolemIntro.GolemThreat".Localized());

                OutputText("Scenes.Volcanicgolemscene.VolcanicGolemIntro.FightAnnouncement".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.Metvolcanicgolem, 1);

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.VolcanicGolem), () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Volcanicgolemscene.VolcanicGolemIntro.GolemApproaches".Localized());

                VolcanicGolemMenu();
            }
        }

        public void VolcanicGolemMenu()
        {
            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), StartFight);
        }

        public void StartFight()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.Volcanicgolemhp, CoC.Instance.Flags.Get(GameFlagIds.Volcanicgolemhp) + 1000);

            if (CoC.Instance.Flags.Get(GameFlagIds.Volcanicgolemhp) > 10000)
                CoC.Instance.Flags.Set(GameFlagIds.Volcanicgolemhp, 10000);

            OutputText("Scenes.Volcanicgolemscene.StartFight.ChargeIntoFight".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.VolcanicGolem), () => Go<CampScene>());
        }

        public void WinAgainstGolem()
        {
            ClearOutput();

            OutputText("Scenes.Volcanicgolemscene.WinAgainstGolem.GolemFalls".Localized());

            OutputText("Scenes.Volcanicgolemscene.WinAgainstGolem.SelfDestructWarning".Localized());

            OutputText("Scenes.Volcanicgolemscene.WinAgainstGolem.RubyAftermath".Localized());

            Player.CreateKeyItem("Golem's Heart", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.Destroyedvolcanicgolem, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToGolem()
        {
            ClearOutput();

            OutputText("Scenes.Volcanicgolemscene.LoseToGolem.DefeatBeforeGolem".Localized());

            GameOver();
        }

    }
}