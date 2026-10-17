using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        /// <summary>
        /// 原版 ConsumableLib.foodItems 的 C# 对应清单（物品 id 与 Items.hjson 一致）。
        /// 季节判定（感恩节非严格模式）与「包里有食物」类检查用它来数食物。
        /// </summary>
        public static readonly HashSet<Identifier> FoodItems =

        [
            new("coc", "black_cat_beer"),

            new("coc", "chocolate_cupcake"),

            new("coc", "fish_fillet"),

            new("coc", "frothy_beer"),

            new("coc", "gods_mead"),

            new("coc", "hard_biscuits"),

            new("coc", "isabella_milk"),

            new("coc", "marble_milk"),

            new("coc", "peach_whiskey"),

            new("coc", "premium_mead"),

            new("coc", "pure_peach"),

            new("coc", "sheep_milk"),

            new("coc", "spring_water"),

            new("coc", "trail_mix"),

            new("coc", "winter_pudding"),

            new("coc", "bee_honey"),

            new("coc", "black_canine_pepper"),

            new("coc", "boar_truffle"),

            new("coc", "bulbous_canine_pepper"),

            new("coc", "canine_pepper"),

            new("coc", "double_canine_pepper"),

            new("coc", "fox_berry"),

            new("coc", "ferret_fruit"),

            new("coc", "goblin_ale"),

            new("coc", "special_hummanus"),

            new("coc", "imp_food"),

            new("coc", "kanga_fruit"),

            new("coc", "knotty_canine_pepper"),

            new("coc", "mouse_cocoa"),

            new("coc", "pigtail_truffle"),

            new("coc", "purple_fruit"),

            new("coc", "pure_honey"),

            new("coc", "salamander_firewater"),

            new("coc", "satyr_wine"),

            new("coc", "w_fruit"),

            new("coc", "wolf_pepper"),

            new("coc", "large_black_egg"),

            new("coc", "large_blue_egg"),

            new("coc", "large_brown_egg"),

            new("coc", "large_pink_egg"),

            new("coc", "large_purple_egg"),

            new("coc", "large_white_egg"),

            new("coc", "black_egg"),

            new("coc", "blue_egg"),

            new("coc", "brown_egg"),

            new("coc", "pink_egg"),

            new("coc", "purple_egg"),

            new("coc", "white_egg"),

            new("coc", "pumpkin_seed"),
        ];

        // 原版 Inventory.hx countTotalFoodItems()
        public virtual int CountTotalFoodItems()
        {
            var total = 0;

            foreach (var slot in Inventory.Slots)
            {
                if (slot == null || slot.IsEmpty || slot.Quantity <= 0)
                    continue;

                if (slot.Item != null && FoodItems.Contains(slot.Item.Id))
                    total += slot.Quantity;
            }

            return total;
        }
    }
}