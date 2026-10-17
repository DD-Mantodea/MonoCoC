using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class AkbalUnsealedCombatHandlers
    {
        public static AkbalUnsealedEnemy Me(CombatContext context) => context.EnemyAs<AkbalUnsealedEnemy>()!;

        public static bool TerraFireReady(CombatContext context) =>

            Me(context).TerraFireCooldown <= 0 &&

            !(context.Enemy.HasStatusEffect(StatusEffectIds.AkbalFireUsed) && context.Player.CanFly());

        [Register("coc:akbal_unsealed_punch")]
        internal static void Punch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var me = Me(context);

            context.WriteAction("Combat.AkbalUnsealed.Punch.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                me.Dodges++;

                return;
            }

            context.WriteAction("Combat.AkbalUnsealed.Punch.Hit".Localized());

            me.AttemptStunned += 0.2f;

            if (player.Stun(1, 20))
                me.GotStunned++;

            var damage = (context.Enemy.Strength / 2f + context.Enemy.Intelligence / 2f + context.Enemy.WeaponAttack) * me.RageMod;

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));
        }

        [Register("coc:akbal_unsealed_claw")]
        internal static void Claw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var me = Me(context);

            context.WriteAction("Combat.AkbalUnsealed.Claw.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                me.Dodges++;

                return;
            }

            context.WriteAction("Combat.AkbalUnsealed.Claw.Hit".Localized());

            var damage = (context.Enemy.Strength + context.Enemy.WeaponAttack) * me.RageMod;

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));
        }

        [Register("coc:akbal_unsealed_terra_fire")]
        internal static void TerraFire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var me = Me(context);

            var doStun = true;

            me.TerraFireCooldown = 2;

            var damage = (context.Enemy.Intelligence + 50f) * me.RageMod;

            if (!context.Enemy.HasStatusEffect(StatusEffectIds.AkbalFireUsed))
            {
                context.Enemy.CreateStatusEffect(StatusEffectIds.AkbalFireUsed);

                damage *= 1.5f;

                context.WriteAction("Combat.AkbalUnsealed.TerraFire.First".Localized());

                if (player.CanFly())
                {
                    damage *= 0.25f;

                    doStun = false;

                    context.WriteAction("Combat.AkbalUnsealed.TerraFire.FirstFly".Localized());
                }
                else
                    context.WriteAction("Combat.AkbalUnsealed.TerraFire.FirstHit".Localized());
            }
            else
            {
                context.WriteAction("Combat.AkbalUnsealed.TerraFire.Again".Localized());

                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = context.Enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false
                });

                if (avoid.Outcome != AvoidOutcome.Hit)
                {
                    damage *= 0.25f;

                    doStun = false;

                    context.WriteAction("Combat.AkbalUnsealed.TerraFire.AgainDodge".Localized());

                    me.Dodges++;
                }
                else
                    context.WriteAction("Combat.AkbalUnsealed.TerraFire.AgainHit".Localized());
            }

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));

            if (!doStun)
                return;

            me.AttemptStunned += 1;

            if (player.Stun(1, 100))
                me.GotStunned++;
        }

        [Register("coc:akbal_unsealed_fire_breath")]
        internal static void FireBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var me = Me(context);

            var damage = (context.Enemy.Intelligence + context.Random.Next((int)context.Enemy.Intelligence / 2)) * me.RageMod;

            context.WriteAction("Combat.AkbalUnsealed.FireBreath.Roar".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction(DodgeText(context));

                me.Dodges++;
            }
            else
            {
                context.WriteAction("Combat.AkbalUnsealed.FireBreath.Hit".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));
            }

            context.WriteAction("Combat.AkbalUnsealed.FireBreath.Ground".Localized());

            if (player.HasStatusEffect(StatusEffectIds.AkbalFlameDebuff))
                player.ChangeStatusValue(StatusEffectIds.AkbalFlameDebuff, 1, 1);
            else
                player.CreateStatusEffect(StatusEffectIds.AkbalFlameDebuff, 1);

            me.Dodges = 0;
        }

        public static string DodgeText(CombatContext context)
        {
            var keys = new[]
            {
                "Combat.AkbalUnsealed.FireBreath.Dodge1",

                "Combat.AkbalUnsealed.FireBreath.Dodge2",

                "Combat.AkbalUnsealed.FireBreath.Dodge3",

                "Combat.AkbalUnsealed.FireBreath.DodgeGeneric"
            };

            return keys[context.Random.Next(keys.Length)].Localized();
        }

        [Register("coc:akbal_unsealed_psi_assault")]
        internal static void PsiAssault(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var me = Me(context);

            var baseDamage = context.Enemy.Intelligence / 10f +

                context.Random.Next((int)context.Enemy.Intelligence / 10) -

                player.Intelligence / 10f;

            var damage = baseDamage * (1 + player.LibidoPercent / 200f);

            damage *= (me.RageMod + 1) / 2f;

            me.PsiAssaultCooldown = 3;

            context.WriteAction("Combat.AkbalUnsealed.PsiAssault.One".Localized());

            context.WriteAction("Combat.AkbalUnsealed.PsiAssault.Two".Localized());

            context.WriteAction("Combat.AkbalUnsealed.PsiAssault.Three".Localized());

            if (player.HasStatusEffect(StatusEffectIds.TrueWhispered))
                player.CreateStatusEffect(StatusEffectIds.Whispered);
            else
                player.CreateStatusEffect(StatusEffectIds.TrueWhispered, 0, 1);

            player.Teased(Math.Max(0f, damage));
        }

        [Register("coc:akbal_unsealed_super_heal")]
        internal static void SuperHeal(CombatContext context, CombatAbilityDefinition ability)
        {
            var me = Me(context);

            context.WriteAction("Combat.AkbalUnsealed.SuperHeal.One".Localized());

            context.WriteAction("Combat.AkbalUnsealed.SuperHeal.Two".Localized());

            context.WriteAction("Combat.AkbalUnsealed.SuperHeal.Three".Localized());

            context.Enemy.Health = context.Enemy.MaxHealth;

            context.Enemy.Lust = 0;

            context.Enemy.Fatigue = 0;

            me.RageBonus += 0.3f;

            context.Enemy.CreateStatusEffect(StatusEffectIds.AkbalHealUsed);
        }

        [Register("coc:akbal_unsealed_fly_up")]
        internal static void FlyUp(CombatContext context, CombatAbilityDefinition ability)
        {
            var me = Me(context);

            context.WriteAction("Combat.AkbalUnsealed.FlyUp.One".Localized());

            context.WriteAction("Combat.AkbalUnsealed.FlyUp.Two".Localized());

            me.RageBonus += 0.3f;

            context.Enemy.IsFlying = true;
        }

        [Register("coc:akbal_unsealed_swoop")]
        internal static void Swoop(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var me = Me(context);

            me.SwoopCooldown = 2;

            context.WriteAction("Combat.AkbalUnsealed.Swoop.One".Localized());

            if (player.HasStatusEffect(StatusEffectIds.TrueWhispered))
            {
                context.WriteAction("Combat.AkbalUnsealed.Swoop.Whispered".Localized());

                if (context.Random.Next(2) == 0)
                    PlayerSilly(context);
            }
            else
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = context.Enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false
                });

                if (avoid.Outcome != AvoidOutcome.Hit)
                {
                    context.WriteAction("Combat.AkbalUnsealed.Swoop.Dodge".Localized());

                    me.Dodges++;

                    return;
                }

                context.WriteAction("Combat.AkbalUnsealed.Swoop.Hit".Localized());
            }

            me.AttemptStunned += 0.5f;

            if (player.Stun(1, 50))
                me.GotStunned++;

            var damage = (context.Enemy.Strength + context.Enemy.Speed + context.Enemy.WeaponAttack) * me.RageMod;

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));
        }

        public static void PlayerSilly(CombatContext context)
        {
            if (Configuration.OptionStore.Instance.Get("silly", 0) > 0)
                context.WriteAction("Combat.AkbalUnsealed.Swoop.Silly".Localized());
        }

            [Register("coc:akbal_unsealed_can_heal")]
        public static bool AkbalUnsealedCanHeal(CombatContext ctx, CombatState _) => !ctx.Enemy.HasStatusEffect(StatusEffectIds.AkbalHealUsed);

        [Register("coc:akbal_unsealed_flying")]
        public static bool AkbalUnsealedFlying(CombatContext ctx, CombatState _) => ctx.Enemy.IsFlying;

        [Register("coc:akbal_unsealed_terra_fire_ready")]
        public static bool AkbalUnsealedTerraFireReady(CombatContext ctx, CombatState _) => TerraFireReady(ctx);

        [Register("coc:akbal_unsealed_psi_ready")]
        public static bool AkbalUnsealedPsiReady(CombatContext ctx, CombatState _) => ((AkbalUnsealedEnemy)ctx.Enemy).PsiAssaultCooldown <= 0;

        [Register("coc:akbal_unsealed_should_heal")]
        public static bool AkbalUnsealedShouldHeal(CombatContext ctx, CombatState _) => ((AkbalUnsealedEnemy)ctx.Enemy).ShouldHeal;

        [Register("coc:akbal_unsealed_should_fly")]
        public static bool AkbalUnsealedShouldFly(CombatContext ctx, CombatState _) => ((AkbalUnsealedEnemy)ctx.Enemy).ShouldFlyUp;

}
}