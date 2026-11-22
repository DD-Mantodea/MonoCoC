using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class TamanisDaughtersCombatHandlers
    {
        [Register("coc:tamanis_daughters_goblin_drug_attack")]
        internal static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var color = context.Random.Next(5) switch
            {
                0 => "red",

                1 => "green",

                2 => "blue",

                3 => "white",

                _ => "black"
            };

            var multiplier = 1f + player.NewGamePlusMod() * 0.5f;

            if (color == "blue")
            {

                context.Log.Add("Combat.TamanisDaughters.BlueDouse".Localized());

                if (context.Enemy.Health < context.Enemy.MaxHealth)
                {
                    context.Log.Add("Combat.TamanisDaughters.BlueHeal".Localized());

                    context.Enemy.HealthChange(80 * multiplier);
                }
                else
                    context.Log.Add("Combat.TamanisDaughters.BlueNoEffect".Localized());

                return;
            }

            context.Log.Add("Combat.TamanisDaughters.DrugThrow".Localized(color));

            if ((player.HasPerk(PerkIds.Evade) && context.Random.Next(10) <= 3) ||
                context.Random.Next(100) < player.Speed / 5)
            {
                context.Log.Add("Combat.Goblin.Dodge".Localized());

                return;
            }

            switch (color)
            {
                case "red":
                    context.Log.Add("Combat.Goblin.Red".Localized());

                    if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                        StatusEffects.EffectApplication.Apply(
                            player, StatusEffectIds.TemporaryHeat, value2: 1);

                    break;

                case "green":
                    context.Log.Add("Combat.Goblin.Green".Localized());

                    if (!player.HasStatusEffect(StatusEffectIds.Poison))
                        StatusEffects.EffectApplication.Apply(
                            player, StatusEffectIds.Poison, value2: 1);

                    break;

                case "white":
                    context.Log.Add("Combat.Goblin.White".Localized());

                    if (!player.HasStatusEffect(StatusEffectIds.NoFlee))
                        player.CreateStatusEffect(StatusEffectIds.NoFlee, 0, 0, 0, 0, 10);

                    break;

                case "black":
                    context.Log.Add("Combat.Goblin.Black".Localized());

                    player.ChangeFatigue(10 + context.Random.Next(25) * multiplier);

                    break;
            }
        }
    }
}