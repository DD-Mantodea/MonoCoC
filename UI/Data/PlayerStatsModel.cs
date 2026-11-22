using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.UI.Data
{
    public class PlayerStatsModel(Func<Player?> player, Func<Time> time)
    {
        public Player? Player => player();

        public Time Time => time();

        public string Name => Player?.ShortName ?? "";

        public float Strength => Player?.Strength ?? 0;

        public float Toughness => Player?.Toughness ?? 0;

        public float Speed => Player?.Speed ?? 0;

        public float Intelligence => Player?.Intelligence ?? 0;

        public float Libido => Player?.Libido ?? 0;

        public float Sensitivity => Player?.Sensitivity ?? 0;

        public float Corruption => Player?.Corruption ?? 0;

        public float Health => Player?.Health ?? 0;

        public float MaxHealth => Player?.MaxHealth ?? 0;

        public float Lust => Player?.Lust ?? 0;

        public float MaxLust => Player?.MaxLust ?? 0;

        public float MinLust => Player?.MinLust() ?? 0;

        public float Fatigue => Player?.Fatigue ?? 0;

        public float MaxFatigue => Player?.MaxFatigue() ?? 0;

        public float Level => Player?.Level ?? 0;

        public float Experience => Player?.Experience ?? 0;

        public float RequiredXp => Player?.RequiredXP() ?? 0;

        public float Gems => Player?.Gems ?? 0;

        public string DayText => "Views.PlayerStats.Day".Localized(Time.Days);

        public string TimeText => "Views.PlayerStats.Time".Localized(Time.Hours, Time.Minutes);

        public string AttackDamage => Player is null ? "0" : MathF.Round((Player.WeaponAttack + Player.Strength) * Player.PhysMod()).ToString("0");

        public string PhysicalDamage => Player is null ? "0%" : MathF.Round(100 * Player.PhysMod()) + "%";

        public string CriticalChance => Player is null ? "0%" : MathF.Round(Player.GetBaseCritChance()) + "%";

        public string WeaponCrit => Player is null ? "0%" : MathF.Round(Player.GetMeleeCritBonus()) + "%";

        public string DodgeChance => Player is null ? "0%" : MathF.Round(Player.GetEvasionChance()) + "%";

        public string ParryChance => Player is null ? "0%" : MathF.Round(Player.CanParry() ? Player.ParryChance() : 0) + "%";

        public string Armor => Player is null ? "0" : MathF.Round(Player.ArmorDefense).ToString("0");

        public string TeaseChance => Player is null ? "0" : MathF.Round((Player.UpperGarment?.Sexiness + Player.LowerGarment?.Sexiness) ?? 0).ToString("0");

        public string TeaseDamage => Player is null ? "0" : MathF.Round(Player.Stats.Addition(GameStats.TeaseDamage)).ToString("0");

        public string LustResistance => Player is null ? "0%" : MathF.Round(100 - Player.LustPercent) + "%";

        public string SpellMod => Player is null ? "0%" : MathF.Round(100 * Player.SpellMod()) + "%";

        public string SpellCost => Player is null ? "0%" : MathF.Round(Player.SpellCost(100)) + "%";
    }
}
