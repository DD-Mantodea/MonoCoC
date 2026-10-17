using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    /// <summary>
    /// 原版 items/weapons/unarmed/UnarmedClaws.hx:7-98 —— 徒手爪武器。
    /// 按玩家爪子类型查表决定攻击力；种族匹配 + 自然武器 perk 各加 50%；
    /// 自然武器 perk 提供护甲穿透与流血效果；爪精通提升流血。
    /// </summary>
    public class UnarmedClawsBehavior : ItemBehavior, IWeaponAttackBehavior
    {
        public sealed record ClawData(int Attack, string Name, int BleedChance, float BleedDamage, float Penetration, string RacePattern);

        private static readonly Dictionary<string, ClawData> Table = new()
        {
            ["lizard"] = new(3, "lizard", 25, 1 / 4f, 0.9f, "(?:dracolisk|dragonewt|basilisk|lizan)"),
            ["dragon"] = new(10, "dragon", 30, 1 / 3f, 0.8f, "dragon-"),
            ["salamander"] = new(5, "salamander", 25, 1 / 4f, 0.9f, "salamander-"),
            ["cat"] = new(4, "cat", 25, 1 / 4f, 1f, "(?:kitten|cat|sphinx)-"),
            ["dog"] = new(3, "dog", 0, 1 / 6f, 1f, "(?:dog|puppy)-"),
            ["fox"] = new(4, "fox", 25, 1 / 4f, 1f, "(?:fox-|kitsune)"),
            ["imp"] = new(5, "imp", 25, 1 / 4f, 0.9f, "(?:imp|demon-)"),
            ["cockatrice"] = new(5, "cockatrice", 25, 1 / 4f, 0.9f, "cockatrice"),
            ["red_panda"] = new(2, "red panda", 0, 1 / 6f, 1f, "red-panda"),
            ["ferret"] = new(2, "ferret", 0, 1 / 6f, 1f, "ferret-"),
            ["wolf"] = new(5, "wolf", 10, 1 / 5f, 0.9f, "wolf-"),
        };

        public static ClawData DataFor(Player player) =>

            Table.TryGetValue(player.Body.Get<ArmsPart>().Claws.Id.Path, out var data) ? data : Table["lizard"];

        public static bool RaceMatch(Player player) =>

            Regex.IsMatch(player.Race, DataFor(player).RacePattern);

        public static bool NaturalWeapons(Player player) => player.HasPerk(PerkIds.NaturalWeapons);

        public static int MasteryLevel(Player player) => player.MasteryLevel(new Identifier("coc", "Claw"));

        // 原版 :54 get_attack()
        public float GetAttack(Weapon weapon, Player player)
        {
            var data = DataFor(player);

            var multi = 1f;

            if (RaceMatch(player))
                multi += 0.5f;

            if (NaturalWeapons(player))
                multi += 0.5f;

            return MathF.Round(data.Attack * multi);
        }

        // 原版 :63 get_armorMod()
        public static float ArmorMod(Player player) => NaturalWeapons(player) ? DataFor(player).Penetration : 1f;

        // 原版 :69 get_effects() 的流血部分（爪姿态 ClawStance 在 C# 不存在，未计入）
        public static (int Chance, float Intensity) Bleed(Player player)
        {
            if (!NaturalWeapons(player))
                return (0, 0f);

            var data = DataFor(player);

            var chance = data.BleedChance;

            var intensity = data.BleedDamage;

            if (RaceMatch(player))
            {
                var mastery = MasteryLevel(player);

                chance += 5 * mastery;

                intensity *= 1 + 0.2f * mastery;
            }

            // 原版 :80 爪姿态：低概率时大幅提升（逼近 60 上限，收益递减）
            if (player.HasStatusEffect(StatusEffectIds.ClawStance) && chance < 60)
                chance += (int)MathF.Round((60 - chance) * 0.7f);

            return (chance, intensity);
        }

        public void OnAttack(Player player)
        {
        }
    }
}