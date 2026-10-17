using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ClaraCombatHandlers
    {

        [Register("coc:clara_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        public static void LacticAcid(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Write("Combat.Clara.NotMurbleEnjoysTheLacticAcid.ClaraSuddenlyStarts".Localized());

            enemy.Health = Math.Min(enemy.MaxHealth, enemy.Health + 45);

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);

            player.TakeLustDamage(5 + player.Libido / 5f, true);
        }

        [Register("coc:clara_drug_attack")]
        public static void DrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var red = context.Random.Next(2) == 0;

            context.Append("Combat.Clara.ClaraDrugAttack.ClaraSuddenlySnatches".Localized());

            var evaded = (player.HasPerk(PerkIds.Evade) && context.Random.Next(10) <= 3)

                || context.Random.Next(100) < player.Speed / 5f;

            if (evaded)
            {
                context.Append("Combat.Clara.ClaraDrugAttack.YouNarrowlyAvoid".Localized());

                return;
            }

            if (red)
            {
                context.Append("Combat.Clara.ClaraDrugAttack.TheRedFluids".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                    player.CreateStatusEffect(StatusEffectIds.TemporaryHeat, 0, 1, 0, 0);
            }
            else
            {
                context.Append("Combat.Clara.ClaraDrugAttack.TheBlackFluid".Localized());

                player.ChangeFatigue(10 + context.Random.Next(25));
            }
        }

        [Register("coc:clara_tease_attack")]
        public static void TeaseAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Combat.Clara.ClaraTeaseAttack.ClaraHesitatesThen".Localized());

                    break;

                case 1:
                    context.Append("Combat.Clara.ClaraTeaseAttack.ClaraSeemsTo".Localized());

                    break;

                default:
                    context.Append("Combat.Clara.ClaraTeaseAttack.InsteadOfAttacking".Localized());

                    break;
            }

            player.TakeLustDamage(5 + player.Libido / 20f, true);
        }

        [Register("coc:clara_casts_blind")]
        public static void CastsBlind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Clara.ClaraCastsBlind.ClaraGlaresAt".Localized());

            if (player.Intelligence / 5f + context.Random.Next(20) + 1 < 14)
            {
                context.Append("Combat.Clara.ClaraCastsBlind.ABrightFlash".Localized());

                player.CreateStatusEffect(StatusEffectIds.Blind, 1, 0, 0, 0);
            }
            else
                context.Append("Combat.Clara.ClaraCastsBlind.YouManageTo".Localized());
        }

        public static void GropesBlind(CombatContext context)
        {
            var player = context.Player;

            if (player.HasCock() && (!player.HasVagina() || context.Random.Next(2) == 0))
                context.Append("Combat.Clara.ClaraGropesBlindPCs.SuddenlyClaraWraps".Localized(player.MultiCockDescriptLight()));
            else if (player.HasVagina())
                context.Append("Combat.Clara.ClaraGropesBlindPCs.ASuddenRush".Localized());
            else
                context.Append("Combat.Clara.ClaraGropesBlindPCs.ThanksToClara".Localized());

            player.TakeLustDamage(7 + player.Libido / 15f, true);
        }

        public static void BonusBaseLustDamage(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.Clara.ClaraBonusBaseLustDamage.TheEarlyEffects".Localized());

            player.TakeLustDamage(2 + player.Libido / 20f, true);
        }
    }
}