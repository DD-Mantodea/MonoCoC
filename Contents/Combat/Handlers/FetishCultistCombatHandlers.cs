using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class FetishCultistCombatHandlers
    {

        [Register("coc:fetish_cultist_cultist_raise_player_lust")]
        public static void CultistRaisePlayerLust(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (context.Random.Next(2) == 0)
            {
                var key = enemy.ArmorName switch
                {
                    OutfitIds.PervyNunsClothing => "Combat.FetishCultist.Raise.PervyNun",

                    OutfitIds.NoblesClothing => "Combat.FetishCultist.Raise.Noble",

                    OutfitIds.Swimsuit => "Combat.FetishCultist.Raise.Swimsuit",

                    OutfitIds.TeachersOutfit => "Combat.FetishCultist.Raise.Teacher",

                    OutfitIds.NaughtyNursesUniform => "Combat.FetishCultist.Raise.Nurse",

                    OutfitIds.FetishyOutfit => "Combat.FetishCultist.Raise.Fetishy",

                    _ => "Combat.FetishCultist.Raise.Generic"
                };

                context.Append(key.Localized());
            }
            else
                context.Append("Combat.FetishCultist.Raise.Breasts".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + player.Libido / 10 + player.Corruption / 20 + 4);
        }

        [Register("coc:fetish_cultist_cultist_lust_transfer")]
        public static void CultistLustTransfer(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Lust <= 30 || context.Random.Next(2) == 0)
            {
                context.Append("Combat.FetishCultist.Transfer.Build".Localized());

                context.Append(player.Corruption < 40

                    ? "Combat.FetishCultist.Transfer.Afraid".Localized()

                    : "Combat.FetishCultist.Transfer.Wish".Localized());

                context.Append("Combat.FetishCultist.Transfer.BuildTail".Localized());

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 10);

                return;
            }

            context.Append("Combat.FetishCultist.Transfer.Overwhelm".Localized());

            if (player.HasVagina())
            {
                context.Append("Combat.FetishCultist.Transfer.Vagina".Localized(player.VaginaDescript(0)));

                if (player.HasCock())
                    context.Append("Combat.FetishCultist.Transfer.VaginaCock".Localized());

                context.Append(".");
            }
            else if (player.HasCock())
                context.Append("Combat.FetishCultist.Transfer.Cock".Localized());

            if (player.Gender == Gender.Genderless)
                context.Append("Combat.FetishCultist.Transfer.Genderless".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + enemy.Lust / 3 * (1 + player.Corruption / 300));

            context.Append("Combat.FetishCultist.Transfer.Calmed".Localized());

            enemy.Lust -= 50;

            if (enemy.Lust < 0)
                enemy.Lust = 10;
        }

        [Register("coc:fetish_cultist_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }
    }
}