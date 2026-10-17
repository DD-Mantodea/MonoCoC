using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.npcs.Clara 的招式（Clara.hx:7 / :17 / :51 / :64 / :75 / :90）。
    public static class ClaraCombatHandlers
    {
        // 原版 eAttack()：普攻
        [Register("coc:clara_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();

        // 原版 notMurbleEnjoysTheLacticAcid()（Clara.hx:7）：喝自己的奶回 45 血、+5 欲望
        public static void LacticAcid(CombatContext context)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Write("Combat.Clara.NotMurbleEnjoysTheLacticAcid.ClaraSuddenlyStarts".Localized());

            enemy.Health = Math.Min(enemy.MaxHealth, enemy.Health + 45);

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);

            player.TakeLustDamage(5 + player.Libido / 5f, true);
        }

        // 原版 claraDrugAttack()（Clara.hx:17）：红药 = 临时发情，黑药 = 疲劳
        [Register("coc:clara_drug_attack")]
        private static void DrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var red = context.Random.Next(2) == 0;

            context.Append("Combat.Clara.ClaraDrugAttack.ClaraSuddenlySnatches".Localized());

            var evaded = (player.HasPerk(VanillaPerks.Evade) && context.Random.Next(10) <= 3)
                || context.Random.Next(100) < player.Speed / 5f;

            if (evaded)
            {
                context.Append("Combat.Clara.ClaraDrugAttack.YouNarrowlyAvoid".Localized());

                return;
            }

            if (red)
            {
                context.Append("Combat.Clara.ClaraDrugAttack.TheRedFluids".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.TemporaryHeat))
                    player.CreateStatusEffect(VanillaStatusEffects.TemporaryHeat, 0, 1, 0, 0);
            }
            else
            {
                context.Append("Combat.Clara.ClaraDrugAttack.TheBlackFluid".Localized());

                player.ChangeFatigue(10 + context.Random.Next(25));
            }
        }

        // 原版 claraTeaseAttack()（Clara.hx:51）：三套台词，5 + libido / 20 点欲望
        [Register("coc:clara_tease_attack")]
        private static void TeaseAttack(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 claraCastsBlind()（Clara.hx:64）：inte / 5 + rand(20) + 1 < 14 则致盲
        [Register("coc:clara_casts_blind")]
        private static void CastsBlind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Clara.ClaraCastsBlind.ClaraGlaresAt".Localized());

            if (player.Intelligence / 5f + context.Random.Next(20) + 1 < 14)
            {
                context.Append("Combat.Clara.ClaraCastsBlind.ABrightFlash".Localized());

                player.CreateStatusEffect(VanillaStatusEffects.Blind, 1, 0, 0, 0);
            }
            else
            {
                context.Append("Combat.Clara.ClaraCastsBlind.YouManageTo".Localized());
            }
        }

        // 原版 claraGropesBlindPCs()（Clara.hx:75）：按玩家身体部位三选一，7 + libido / 15 点欲望
        public static void GropesBlind(CombatContext context)
        {
            var player = context.Player;

            if (player.HasCock() && (!player.HasVagina() || context.Random.Next(2) == 0))
            {
                context.Append("Combat.Clara.ClaraGropesBlindPCs.SuddenlyClaraWraps".Localized(player.MultiCockDescriptLight()));
            }
            else if (player.HasVagina())
            {
                context.Append("Combat.Clara.ClaraGropesBlindPCs.ASuddenRush".Localized());
            }
            else
            {
                context.Append("Combat.Clara.ClaraGropesBlindPCs.ThanksToClara".Localized());
            }

            player.TakeLustDamage(7 + player.Libido / 15f, true);
        }

        // 原版 claraBonusBaseLustDamage()（Clara.hx:90）：非营地战每回合 2 + libido / 20 点欲望
        public static void BonusBaseLustDamage(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.Clara.ClaraBonusBaseLustDamage.TheEarlyEffects".Localized());

            player.TakeLustDamage(2 + player.Libido / 20f, true);
        }
    }
}
