using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AspectOfLaurentiusEnemy>("coc:aspect_of_laurentius")]
    public class AspectOfLaurentiusEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

                    CreateBreastRow(0, 1);

            Tallness = 6 * 12;

            WeaponName = "nothing";

            ArmorName = "ethereal robes";
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress,

                CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) | 2048);

            return false;
        }

        public override bool Won(bool hpVictory) => false;

        public static readonly MonoCoC.Registry.Identifier ApotheosisId = new("coc", "aspect_of_laurentius_apotheosis");

        public static readonly MonoCoC.Registry.Identifier WhitefireId = new("coc", "aspect_of_laurentius_whitefire");

        public static readonly MonoCoC.Registry.Identifier SummonId = new("coc", "aspect_of_laurentius_summon_fodder");

        public bool Invincible;

        public int SummonedTurn;

        public override float ModifyIncomingDamage(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (Invincible)
                return 0;

            return base.ModifyIncomingDamage(context, damage, kind);
        }

        public override void HandleDamaged(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (Health <= 0 || Invincible)
                return;

            context.Append("Combat.Hel.LaurentiusReality".Localized());

            var healed = damage * 0.6f;

            Health = Math.Min(MaxHealth, Health + healed);

            context.Append("Combat.Hel.LaurentiusHealed".Localized((int)healed));
        }

        public override bool HandleStun(CombatContext context)
        {
            var round = CombatController.Instance!.State.Round;

            if ((round % 4 == 0 && round != 0) || (StatusEffectValue(StatusEffectIds.Apotheosis, 0) >= 1 && SummonedTurn == 0))
                return true;

            return base.HandleStun(context);
        }

        public override bool HandleFear(CombatContext context)
        {
            context.Append("Combat.Hel.LaurentiusFear".Localized());

            return true;
        }

        public static readonly MonoCoC.Registry.Identifier WatchId = new("coc", "aspect_of_laurentius_watch");

        public static readonly MonoCoC.Registry.Identifier UnsummonId = new("coc", "aspect_of_laurentius_unsummon");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var round = CombatController.Instance!.State.Round;

            if (Invincible)
            {
                if (SummonedTurn + 3 == round)
                    return new CombatAiAction { Ability = UnsummonId };

                return new CombatAiAction { Ability = WatchId };
            }

            if (round % 4 == 0 && round != 0)
                return new CombatAiAction { Ability = ApotheosisId };

            if (HasStatusEffect(StatusEffectIds.Apotheosis) && (round + 3) % 8 == 0 && round > 0)
                return new CombatAiAction { Ability = SummonId };

            return new CombatAiAction { Ability = WhitefireId };
        }

    }
}