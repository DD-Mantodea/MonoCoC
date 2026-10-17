using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<NecromancerEnemy>("coc:necromancer")]
    public class NecromancerEnemy : Enemy
    {

        public bool HasSummonedElite;

        public int SkellyHordeLocation = 1;

        public static readonly Identifier RaiseDeadId = new("coc", "necromancer_raise_dead");

        public static readonly Identifier RaiseEliteId = new("coc", "necromancer_raise_elite");

        public static readonly Identifier AttackId = new("coc", "necromancer_attack");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 6 * 12 + 10;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "gray";

            hair.Length = 4;

            WeaponName = "clawed fingers";

            WeaponVerb = "claw";

            WeaponAttack = 40;

            ArmorName = "crimson cloak";

            Gems = Random.Shared.Next(15) + 250;

            LustVulnerability = 0;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var horde = FindSkellyHorde(context, out var location);

            SkellyHordeLocation = location;

            if (!HasSummonedElite && HealthRatio() < 0.5f)
                return new CombatAiAction { Ability = RaiseEliteId };

            if (horde != null && CanRaiseDead(horde) && context.Random.Next(3) < 2)
                return new CombatAiAction { Ability = RaiseDeadId };

            return new CombatAiAction { Ability = AttackId };
        }

        public static bool CanRaiseDead(SkeletonHordeEnemy horde)
        {
            var flag512 = (CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 512) != 0;

            return (horde.UnitAmount < 10 && !flag512) || (horde.UnitAmount < 5 && flag512);
        }

        public static SkeletonHordeEnemy? FindSkellyHorde(CombatContext context, out int location)
        {
            location = 1;

            for (var i = 0; i < context.Enemies.Count; i++)
            {
                if (context.Enemies[i] is not SkeletonHordeEnemy horde)
                    continue;

                location = i;

                return horde;
            }

            return null;
        }

        public override void TickStatuses(CombatContext context) =>

            BoneGeneralEnemy.TickGuardAb(context, this);

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.Necromancer.HandleFear.TheNecromancerIsn".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DefeatNecro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToNecro();

            return true;
        }
    }
}