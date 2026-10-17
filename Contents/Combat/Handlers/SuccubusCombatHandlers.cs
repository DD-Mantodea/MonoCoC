using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class SuccubusCombatHandlers
    {

        [Register("coc:succubus_kiss_attack")]
        public static void KissAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Succubus.KissAttack.TheSuccubusDances".Localized());

            var avoided = (player.Speed > enemy.Speed && context.Random.Next(4) == 0)

                || (player.HasPerk(PerkIds.Evade) && context.Random.Next(4) == 0)

                || (player.HasPerk(PerkIds.Misdirection) && context.Random.Next(4) == 0

                    && player.ArmorName == "red, high-society bodysuit");

            if (avoided)
            {
                context.Append("Combat.Succubus.KissAttack.YouStartTo".Localized());

                if (player.LustPercent >= 70)
                    context.Append("Combat.Succubus.KissAttack.MaybeYouShouldn".Localized());

                return;
            }

            context.Append("Combat.Succubus.KissAttack.YouStartTo2".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.KissOfDeath))
                player.CreateStatusEffect(StatusEffectIds.KissOfDeath, 0, 0, 0, 0);
        }

        [Register("coc:succubus_seduce_attack")]
        public static void SeduceAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var capitalA = Utils.Utils.CapitalizeFirstLetter(enemy.Article);

            var lustDamage = 0f;

            switch (context.Random.Next(3))
            {

                case 0:
                {
                    context.Append("Combat.Succubus.SeduceAttack.OneSlaps".Localized(
                        capitalA, enemy.ShortName, ButtDescription(enemy)));

                    var rating = (int)enemy.Body.Get<ButtPart>().Rating;

                    if (rating >= 10)
                    {
                        context.Append("Combat.Succubus.SeduceAttack.MakingItJiggle".Localized());

                        if (context.Random.Next(100) <= 95)
                        {
                            lustDamage = context.Random.Next(Math.Max(1, rating)) + 10;

                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                        }
                        else
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                    }
                    else
                    {
                        context.Append("Combat.Succubus.SeduceAttack.Text".Localized());

                        if (context.Random.Next(100) <= 70 + rating * 2)
                        {
                            lustDamage = context.Random.Next(Math.Max(1, rating)) + 9;

                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                        }
                        else
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                    }

                    player.TakeLustDamage(lustDamage, true);

                    break;
                }

                case 1:
                {
                    if (enemy.BreastRows.Count == 0 || enemy.BreastRows[0].BreastRating < 2)
                        break;

                    var row = enemy.BreastRows[0];

                    if (enemy.BreastRows.Count == 1)
                    {
                        context.Append("Combat.Succubus.SeduceAttack.OneCaresses".Localized(
                            capitalA, enemy.ShortName));

                        if (enemy.LustPercent >= 50)
                            context.Append("Combat.Succubus.SeduceAttack.ZeroHardNipples".Localized(enemy.Pronoun2));

                        if (context.Random.Next(100) <= 65 + enemy.BiggestBreastSize())
                        {
                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());

                            lustDamage = context.Random.Next(Math.Max(1, (int)row.BreastRating))

                                + enemy.BreastRows.Count + 10;
                        }
                        else
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed2".Localized());
                    }
                    else
                    {
                        context.Append("Combat.Succubus.SeduceAttack.OneCaresses2".Localized(
                            capitalA, enemy.ShortName, enemy.Pronoun2));

                        if (enemy.LustPercent >= 50)
                            context.Append("Combat.Succubus.SeduceAttack.YourPainfullyVisible".Localized());
                        else
                            context.Append("Combat.Succubus.SeduceAttack.Text".Localized());

                        if (context.Random.Next(100)
                            <= 54 + (enemy.BreastRows.Count - 1) * 15 + row.BreastRating)
                        {
                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());

                            lustDamage = context.Random.Next(Math.Max(1, (int)row.BreastRating))

                                + enemy.BreastRows.Count * row.Breasts + 5;
                        }
                        else
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                    }

                    player.TakeLustDamage(lustDamage, true);

                    break;
                }

                default:
                {
                    context.Append("Combat.Succubus.SeduceAttack.OneReaches".Localized(
                        capitalA, enemy.ShortName));

                    var rows = player.Vaginas.Count * 2 + player.Cocks.Count * 2;

                    if (context.Random.Next(101) <= 65 + player.Vaginas.Count * 10 + player.Cocks.Count * 10)
                    {
                        context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());

                        lustDamage = context.Random.Next(Math.Max(1, rows)) + 13;
                    }
                    else
                        context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());

                    player.TakeLustDamage(lustDamage, true);

                    break;
                }
            }
        }

        [Register("coc:succubus_barrage")]
        public static void SuccubusBarrage(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Succubus.SuccubusBarrage.TheSuccubusLooks".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Succubus.SuccubusBarrage.WithYourEfforts".Localized());

                return;
            }

            context.Append("Combat.Succubus.SuccubusBarrage.DespiteYourSprint".Localized());

            var damage = 0f;

            if (context.Random.Next(Math.Max(1, (int)(player.Strength / 3) + 10))
                < (int)(enemy.Strength / 3) + 10)
            {
                context.Append("Combat.Succubus.SuccubusBarrage.YouTryTo".Localized());

                damage = 10 + enemy.Strength;
            }
            else
            {
                context.Append("Combat.Succubus.SuccubusBarrage.YouBringYour".Localized());

                damage = MathF.Floor((5 + enemy.Strength) / 2f);
            }

            player.TakeDamage(damage);
        }

        public static string ButtDescription(Creature creature) =>

            creature.Body.Get<ButtPart>().Id.Path == "none" ? "butt" : Appearance.ButtDescript(creature);
    }
}