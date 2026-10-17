using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.Contents.Datas.Vanilla;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BrigidCombatHandlers
    {
        [Register("coc:brigid_poke")]
        [Register("coc:brigid_poke_two")]
        public static void Poke(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.BrigidPoke.Text".Localized());

            var damage = 30 + Random.Shared.Next(15);

            player.HealthChange(-damage);
        }

        [Register("coc:brigid_bop")]
        public static void Bop(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.BrigidBop.Text".Localized());

            player.HealthChange(-5);

            if (Random.Shared.Next(2) == 0)
                player.CreateStatusEffect(StatusEffectIds.Stunned, 1, 0, 0, 0);
        }

        [Register("coc:brigid_ass_grind")]
        [Register("coc:brigid_ass_grind_two")]
        public static void AssGrind(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.BrigidAssGrind.Text".Localized());

            context.Player.TakeLustDamage(30);
        }
    }
}