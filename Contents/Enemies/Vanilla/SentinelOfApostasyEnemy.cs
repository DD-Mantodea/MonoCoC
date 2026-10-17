using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SentinelOfApostasyEnemy>("coc:sentinel_of_apostasy")]
    public class SentinelOfApostasyEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

                    CreateBreastRow(0, 1);

            Tallness = 6 * 12;

            WeaponName = "spear";

            ArmorName = "cracked stone";
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress,

                CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) | 1);

            Dungeons.WizardTowerRooms.SentinelVictoryPending = true;

            CombatController.Instance?.Context.Append("Combat.Hel.SentinelFalls".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Combat.Hel.SentinelDefeat".Localized());

            Scenes.Scene.GameOver();

            return false;
        }

    }
}