using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Monsters
{
    [Register<BazaarMobScene>("coc:mob")]
    public class BazaarMobScene : Scene
    {
        public float unitAmount = 0;

        public override void Entry()
        {
            MaxHP();
        }

        public float MaxHP()
        {

            return unitAmount * 100;
        }

        public void PerformCombatAction()
        {

            unitAmount = (float)Math.Ceiling(CurrentMonster.Health / 100);

            OutputText("Scenes.BazaarMob.PerformCombatAction.BlankLine".Localized());

            float i = 0;

            while (i < unitAmount)
            {
                RecombobulateWeapon();

                CombatController.Instance!.Resolver.EnemyBasicAttack();

                i += 1;
            }
        }

        public void RecombobulateWeapon()
        {

            float chooser = Random.Shared.Next(4);

            switch (chooser)
            {
                case 0:
                {
                CurrentMonster.WeaponVerb = "stab";

                CurrentMonster.WeaponName = "spear";

                CurrentMonster.WeaponAttack = 25;
                }

                break;

                case 1:
                {
                CurrentMonster.WeaponVerb = "slash";

                CurrentMonster.WeaponName = "scimitar";

                CurrentMonster.WeaponAttack = 25;
                }

                break;

                case 2:
                {
                CurrentMonster.WeaponVerb = "crush";

                CurrentMonster.WeaponName = "mace";

                CurrentMonster.WeaponAttack = 15;
                }

                break;

                case 3:
                {
                CurrentMonster.WeaponVerb = "bash";

                CurrentMonster.WeaponName = "club";

                CurrentMonster.WeaponAttack = 30;
                }

                break;
            }

            if (true )
            {
                CurrentMonster.WeaponVerb = "pick";

                CurrentMonster.WeaponName = "pickaxe";

                CurrentMonster.WeaponAttack = 20;
            }
        }

        public void Won(bool hpVictory, bool pcCameWorms = false)
        {

        }

        public void Defeated(bool hpVictory)
        {

        }

    }
}