using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class GoblinCombatHandlers
    {

        [Register("coc:goblin_drug")]
        [Register("coc:goblin_drug_attack")]
        public static void Drug(CombatContext context, CombatAbilityDefinition ability)
        {
            var roll = context.Random.Next(5);

            var color = roll switch
            {
                0 => "red",

                1 => "green",

                2 => "blue",

                3 => "white",

                _ => "black"
            };

            if (color == "blue")
            {
                context.WriteAction("Combat.Goblin.Blue".Localized());

                if (context.Enemy.Health < context.Enemy.MaxHealth)
                {
                    context.WriteAction("Combat.Goblin.BlueHeal".Localized());

                    context.Enemy.HealthChange(context.Enemy.MaxHealth / 4);
                }
                else
                    context.WriteAction("Combat.Goblin.BlueNoEffect".Localized());

                return;
            }

            context.WriteAction("Combat.Goblin.Throw".Localized(color));

            if ((context.Player.HasPerk(PerkIds.Evade) && context.Random.Next(10) <= 3) ||
                context.Random.Next(100) < context.Player.Speed / 5)
            {
                context.WriteAction("Combat.Goblin.Dodge".Localized());

                return;
            }

            switch (color)
            {
                case "red":
                    context.WriteAction("Combat.Goblin.Red".Localized());

                    if (!context.Player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                        StatusEffects.EffectApplication.Apply(
                            context.Player, StatusEffectIds.TemporaryHeat, value2: 1);

                    break;

                case "green":
                    context.WriteAction("Combat.Goblin.Green".Localized());

                    if (!context.Player.HasStatusEffect(StatusEffectIds.Poison))
                        StatusEffects.EffectApplication.Apply(
                            context.Player, StatusEffectIds.Poison, value2: 1);

                    break;

                case "white":
                    context.WriteAction("Combat.Goblin.White".Localized());

                    if (!context.Player.HasStatusEffect(StatusEffectIds.NoFlee))
                        context.Player.CreateStatusEffect(StatusEffectIds.NoFlee, 0, 0, 0, 0, 10);

                    break;

                case "black":
                    context.WriteAction("Combat.Goblin.Black".Localized());

                    context.Player.ChangeFatigue(10 + context.Random.Next(25));

                    break;
            }
        }
    }
}