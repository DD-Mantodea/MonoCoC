using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes/scenes/monsters/Succubus.hx 的三个招式：
    //   kissAttack / seduceAttack 来自 AbstractSuccubus.hx，
    //   succubusBarrage 来自 Succubus.hx。
    public static class SuccubusCombatHandlers
    {
        // 原版 AbstractSuccubus.kissAttack()：不造成伤害，命中挂 KissOfDeath。
        [Register("coc:succubus_kiss_attack")]
        private static void KissAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Succubus.KissAttack.TheSuccubusDances".Localized());

            // 原版三种闪避：
            //   player.spe > spe && rand(4) == 0
            //   || (Evade && rand(4) == 0)
            //   || (Misdirection && rand(4) == 0 && player.armorName == "red, high-society bodysuit")
            var avoided = (player.Speed > enemy.Speed && context.Random.Next(4) == 0)
                || (player.HasPerk(VanillaPerks.Evade) && context.Random.Next(4) == 0)
                || (player.HasPerk(VanillaPerks.Misdirection) && context.Random.Next(4) == 0
                    && player.ArmorName == "red, high-society bodysuit");

            if (avoided)
            {
                context.Append("Combat.Succubus.KissAttack.YouStartTo".Localized());

                if (player.Lust100 >= 70)
                    context.Append("Combat.Succubus.KissAttack.MaybeYouShouldn".Localized());

                return;
            }

            context.Append("Combat.Succubus.KissAttack.YouStartTo2".Localized());

            if (!player.HasStatusEffect(VanillaStatusEffects.KissOfDeath))
                player.CreateStatusEffect(VanillaStatusEffects.KissOfDeath, 0, 0, 0, 0);
        }

        // 原版 AbstractSuccubus.seduceAttack()：按 0/1/2 三分支各掷一次成功率。
        [Register("coc:succubus_seduce_attack")]
        private static void SeduceAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var capitalA = MonoCoC.Utils.Utils.CapitalizeFirstLetter(enemy.Article);

            var lustDamage = 0f;

            switch (context.Random.Next(3))
            {
                // Butt slap!
                case 0:
                {
                    context.Append("Combat.Succubus.SeduceAttack.01Slaps".Localized()
                        capitalA, enemy.ShortName, ButtDescription(enemy));

                    var rating = (int)enemy.Body.Get<ButtPart>().Rating;

                    if (rating >= 10)
                    {
                        context.Append("Combat.Succubus.SeduceAttack.MakingItJiggle".Localized());

                        // 85% success rate for the jiggly girls（原版写 <=95）
                        if (context.Random.Next(100) <= 95)
                        {
                            lustDamage = context.Random.Next(Math.Max(1, rating)) + 10;
                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                        }
                        else
                        {
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                        }
                    }
                    else
                    {
                        context.Append("Combat.Succubus.SeduceAttack.Text".Localized());

                        // 50%ish chance of success for the tight butted.
                        if (context.Random.Next(100) <= 70 + rating * 2)
                        {
                            lustDamage = context.Random.Next(Math.Max(1, rating)) + 9;
                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                        }
                        else
                        {
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                        }
                    }

                    player.TakeLustDamage(lustDamage, true);

                    break;
                }

                // Jiggly-tits（判定用的是**怪物自己**的胸，不是玩家的）
                case 1:
                {
                    if (enemy.BreastRows.Count == 0 || enemy.BreastRows[0].BreastRating < 2)
                        break;

                    var row = enemy.BreastRows[0];

                    // Single breast row
                    if (enemy.BreastRows.Count == 1)
                    {
                        context.Append("Combat.Succubus.SeduceAttack.01Caresses".Localized()
                            capitalA, enemy.ShortName);

                        if (enemy.Lust100 >= 50)
                            context.Append("Combat.Succubus.SeduceAttack.0HardNipples".Localized(enemy.Pronoun2));

                        if (context.Random.Next(100) <= 65 + enemy.BiggestBreastSize())
                        {
                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                            lustDamage = context.Random.Next(Math.Max(1, (int)row.BreastRating))
                                + enemy.BreastRows.Count + 10;
                        }
                        else
                        {
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed2".Localized());
                        }
                    }
                    else
                    {
                        context.Append("Combat.Succubus.SeduceAttack.01Caresses2".Localized()
                            capitalA, enemy.ShortName, enemy.Pronoun2);

                        if (enemy.Lust100 >= 50)
                            context.Append("Combat.Succubus.SeduceAttack.YourPainfullyVisible".Localized());
                        else
                            context.Append("Combat.Succubus.SeduceAttack.Text".Localized());

                        // 原版成功率：54 + (行数-1)*15 + 第一行罩杯
                        if (context.Random.Next(100)
                            <= 54 + (enemy.BreastRows.Count - 1) * 15 + row.BreastRating)
                        {
                            context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                            lustDamage = context.Random.Next(Math.Max(1, (int)row.BreastRating))
                                + enemy.BreastRows.Count * row.Breasts + 5;
                        }
                        else
                        {
                            context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                        }
                    }

                    player.TakeLustDamage(lustDamage, true);

                    break;
                }

                // Genitals flash!
                default:
                {
                    context.Append("Combat.Succubus.SeduceAttack.01Reaches".Localized()
                        capitalA, enemy.ShortName);

                    var rows = player.Vaginas.Count * 2 + player.Cocks.Count * 2;

                    if (context.Random.Next(101) <= 65 + player.Vaginas.Count * 10 + player.Cocks.Count * 10)
                    {
                        context.Append("Combat.Succubus.SeduceAttack.TheDisplayIs".Localized());
                        lustDamage = context.Random.Next(Math.Max(1, rows)) + 13;
                    }
                    else
                    {
                        context.Append("Combat.Succubus.SeduceAttack.YouReUnimpressed".Localized());
                    }

                    player.TakeLustDamage(lustDamage, true);

                    break;
                }
            }
        }

        // 原版 Succubus.succubusBarrage()：只可闪避；命中后按双方力量再分两档。
        [Register("coc:succubus_barrage")]
        private static void SuccubusBarrage(CombatContext context, CombatAbilityDefinition ability)
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

            // 原版：Utils.rand((player.str / 3) + 10) < ((str / 3) + 10)
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

        // 原版 Appearance.buttDescriptionShort(this)：没有屁股时用 "butt"。
        private static string ButtDescription(Creature creature) =>
            creature.Body.Get<ButtPart>().Id.Path == "none" ? "butt" : Appearance.ButtDescript(creature);
    }
}
