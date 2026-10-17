using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 classes/scenes/areas/desert/SandWitch.hx 的 lustMagicAttack
    public static class SandWitchCombatHandlers
    {
        [Register("coc:sand_witch_lust_magic")]
        public static void LustMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.SandWitch.LustMagicAttack.SummoningChant".Localized());

            float lust;

            if (player.HasStatusEffect(StatusEffectIds.StoneLust))
            {
                context.Append("Scenes.SandWitch.LustMagicAttack.OrbHeatRush".Localized());

                lust = 8 + player.Sensitivity / 10f;
            }
            else
            {
                context.Append("Scenes.SandWitch.LustMagicAttack.SandSlipsClothes".Localized());

                context.Append(player.HasVagina()

                    ? "Scenes.SandWitch.LustMagicAttack.SandEntersVagina".Localized()

                    : "Scenes.SandWitch.LustMagicAttack.SandEntersAnus".Localized());

                player.CreateStatusEffect(StatusEffectIds.StoneLust, 0, 0, 0, 0);

                lust = 4 + player.Sensitivity / 10f;
            }

            player.TakeLustDamage(lust, true);
        }
    }
}