using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class YamataCombatHandlers
    {

        [Register("coc:yamata_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoCounter = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {

                context.WriteAction("Combat.Yamata.Miss".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player,

                context.Enemy.Strength + context.Random.Next(20));

            player.HealthChange(-damage);
        }

        [Register("coc:yamata_basic")]
        public static void Basic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var roll = context.Random.Next(4);

            float damage;

            switch (roll)
            {
                case 0:
                    context.WriteAction("Combat.Yamata.Basic.One".Localized());

                    damage = enemy.Strength / 2f + context.Random.Next(15);

                    break;

                case 1:
                    context.WriteAction("Combat.Yamata.Basic.Two".Localized());

                    damage = enemy.Strength / 2f + context.Random.Next(35);

                    break;

                case 2:
                    context.WriteAction("Combat.Yamata.Basic.Three".Localized());

                    damage = enemy.Strength - context.Random.Next(25);

                    player.Teased(7);

                    BumpMasochist();

                    break;

                default:
                    context.WriteAction("Combat.Yamata.Basic.Four".Localized());

                    damage = enemy.Strength / 2f + context.Random.Next(50);

                    player.Teased(12);

                    BumpMasochist();

                    break;
            }

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            player.HealthChange(-damage);

            SadomasochistApply(context, damage);

            player.Bleed(context.Enemy);
        }

        [Register("coc:yamata_dark_foxfire")]
        public static void DarkFoxfire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Yamata.DarkFoxfire".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player,

                context.Enemy.Strength / 2f + context.Random.Next(15));

            player.HealthChange(-damage);

            player.Teased(player.HasPerk(PerkIds.Masochist)

                ? 15f + player.Sensitivity / 10f

                : (10f + player.Sensitivity / 10f) * 2f);

            BumpMasochist();
        }

        [Register("coc:yamata_nightmare")]
        public static void Nightmare(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Yamata.Nightmare.One".Localized());

            var resist = player.Intelligence < 80

                ? (int)Math.Round(player.Intelligence / 70f * 25f)

                : 25;

            if (player.HasPerk(PerkIds.Whispered))
                resist += 30;
            else
                context.WriteAction("Combat.Yamata.Nightmare.Two".Localized());

            if (player.IsReligious() && player.IsPureEnough(20))
                resist += (int)(15 - player.CorAdjustedDown());

            if (context.Random.Next(100) < resist)
            {
                context.WriteAction("Combat.Yamata.Nightmare.Resisted".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Fear);

                return;
            }

            context.WriteAction("Combat.Yamata.Nightmare.Failed".Localized());

            if (player.HasStatusEffect(StatusEffectIds.Fear))
                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -12, "Yamata");
            else
            {
                player.CreateStatusEffect(StatusEffectIds.Fear);

                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -18, "Yamata");
            }
        }

        [Register("coc:yamata_illusion_lust")]
        public static void IllusionLust(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var lustDamage = 10f + player.Sensitivity / 5f;

            context.WriteAction("Combat.Yamata.IllusionLust.One".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player,

                context.Enemy.Strength / 2f + context.Random.Next(15));

            player.HealthChange(-damage);

            if (player.HasStatusEffect(StatusEffectIds.Fear))
                switch (context.Random.Next(3))
                {
                    case 0:
                        context.WriteAction("Combat.Yamata.IllusionLust.Fear.Zero".Localized());

                        lustDamage += 4;

                        player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                        BumpMasochist();

                        break;

                    case 1:
                        context.WriteAction("Combat.Yamata.IllusionLust.Fear.One".Localized());

                        player.Teased(lustDamage);

                        break;

                    default:
                        context.WriteAction("Combat.Yamata.IllusionLust.Fear.Twoa".Localized());

                        context.WriteAction("Combat.Yamata.IllusionLust.Fear.Twob".Localized());

                        player.Teased(lustDamage * 2f);

                        break;
                }

            context.WriteAction("Combat.Yamata.IllusionLust.Taunt.One".Localized());

            context.WriteAction("Combat.Yamata.IllusionLust.Taunt.Two".Localized());

            var masochist = CoC.Instance.Flags.Get(GameFlagIds.YamataMasochist);

            if (masochist > 60)
            {
                context.WriteAction("Combat.Yamata.IllusionLust.Masochism.AtLustSixty".Localized());

                lustDamage += 5;
            }

            if (masochist > 80)
            {
                context.WriteAction("Combat.Yamata.IllusionLust.Masochism.AtLustEighty".Localized());

                lustDamage += 3;

                BumpMasochist();
            }

            player.Teased(lustDamage);
        }

        [Register("coc:yamata_lust_attack")]
        public static void LustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Yamata.LustAttack.One".Localized());

            var key = player.HasCock()

                ? player.HasVagina()

                    ? "Combat.Yamata.LustAttack.TwoHerm"

                    : "Combat.Yamata.LustAttack.TwoMale"

                : "Combat.Yamata.LustAttack.TwoFemale";

            context.WriteAction(key.Localized());

            context.WriteAction("Combat.Yamata.LustAttack.Three".Localized());

            var lustDamage = 15f + player.Sensitivity / 3f + player.Libido / 5f;

            var damage = CombatCalculator.ReduceDamage(context, player, 15 + context.Random.Next(26));

            player.HealthChange(-damage);

            player.Teased(lustDamage);

            if (player.HasCock())
                player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            if (player.HasVagina())
                player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            SadomasochistApply(context, lustDamage + 5);

            BumpMasochist();
        }

        [Register("coc:yamata_entwine")]
        public static void Entwine(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Yamata.Entwine.One".Localized());

            context.WriteAction("Combat.Yamata.Entwine.Two".Localized());

            context.WriteAction("Combat.Yamata.Entwine.Three".Localized());

            player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, -10, "Yamata");

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -10, "Yamata");

            var damage = 40f + 25f / (context.Random.Next(3) + 1);

            player.Teased(context.Random.Next(6) + 10);

            player.HealthChange(-damage);

            SadomasochistApply(context, damage);

            player.CreateStatusEffect(StatusEffectIds.YamataEntwine);
        }

        [Register("coc:yamata_foxfire_cannon_one")]
        public static void CannonCharge(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Yamata.Cannon.One".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.YamataCannon, 1);
        }

        [Register("coc:yamata_foxfire_cannon_two")]
        public static void CannonFire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Enemy.RemoveStatusEffect(StatusEffectIds.YamataCannon);

            if (player.HasWaited())
            {
                context.WriteAction("Combat.Yamata.Cannon.TwoPrepared".Localized());

                return;
            }

            var hits = context.Random.Next(9);

            context.WriteAction("Combat.Yamata.Cannon.TwoIntro".Localized());

            if (hits == 0)
            {
                context.WriteAction("Combat.Yamata.Cannon.TwoMissed".Localized());

                player.ChangeFatigue(10);

                return;
            }

            context.WriteAction((hits > 1

                ? "Combat.Yamata.Cannon.TwoHitMany"

                : "Combat.Yamata.Cannon.TwoHitOne").Localized(hits));

            var total = 0f;

            for (var i = 0; i < hits; i++)
            {
                var x = context.Random.Next(10) + 5;

                player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                var damage = CombatCalculator.ReduceDamage(context, player, x * 2);

                player.HealthChange(-damage);

                player.Teased(x / 2f);

                total += damage;
            }

            if (total > 0)
                SadomasochistApply(context, total);
        }

        [Register("coc:yamata_seal")]
        public static void Seal(CombatContext context, CombatAbilityDefinition ability) =>

            KitsuneSeals.Seal(context);

        public static void SadomasochistApply(CombatContext context, float damage)
        {
            var enemy = context.Enemy;

            context.WriteAction("Combat.Yamata.Sadomasochist".Localized());

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + (int)(damage / 2f));

            enemy.ApplyStats(
                new DynStat(GameStats.Strength, damage / 25f),

                new DynStat(GameStats.Speed, -damage / 30f));
        }

        public static void BumpMasochist() =>

            CoC.Instance.Flags.Set(GameFlagIds.YamataMasochist, CoC.Instance.Flags.Get(GameFlagIds.YamataMasochist) + 1);

            [Register("coc:yamata_cannon_primed")]
        public static bool YamataCannonPrimed(CombatContext ctx, CombatState _) => ctx.Enemy.HasStatusEffect(StatusEffectIds.YamataCannon);

        [Register("coc:yamata_can_entwine")]
        public static bool YamataCanEntwine(CombatContext ctx, CombatState _) => !ctx.Player.HasStatusEffect(StatusEffectIds.YamataEntwine);

}
}