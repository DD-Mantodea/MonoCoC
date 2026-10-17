using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SharkGirlEnemy>("coc:shark_girl")]
    public class SharkGirlEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 1);

            if (!HasStatusEffect(StatusEffectIds.BonusVCapacity))
                CreateStatusEffect(StatusEffectIds.BonusVCapacity, 15);

            CreateBreastRow(BreastCups.D);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 40);

            Tallness = 5 * 12 + 5;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "silver";

            hair.Length = 16;

            WeaponName = "shark teeth";

            WeaponVerb = "bite";

            WeaponAttack = 3;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "tough skin";

            ArmorDefense = 5;

            LustVulnerability = 0.9f;

            Gems = Random.Shared.Next(15) + 5;

        }

        public override void OutputAttack(CombatContext context, float damage)
        {
            base.OutputAttack(context, damage);

            if (damage > 0 && context.Random.Next(3) == 0 && context.Player.Bleed(this))
                context.Append("Combat.SharkGirl.OutputAttack.TheSharkGirl".Localized());
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Boat.SharkGirlScene>.Get().SharkWinChoices();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(Flags.GameFlagIds.CameWormsAfterCombat) > 0)
            {
                Scenes.Scene.OutputText("Combat.SharkGirl.Won.YourFoeDoesn".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Places.Boat.SharkGirlScene>.Get().SharkLossRape();

            return true;
        }
    }
}