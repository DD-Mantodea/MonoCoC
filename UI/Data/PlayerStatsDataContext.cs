using System;

namespace MonoCoC.UI.Data
{
    public class PlayerStatsDataContext(PlayerStatsModel model) : IUIDataContext
    {
        public PlayerStatsModel Model { get; } = model;

        public object? Resolve(string path) =>
            path.Trim().ToLowerInvariant() switch
            {
                "name" => Model.Name,
                "strength" => Model.Strength,
                "toughness" => Model.Toughness,
                "speed" => Model.Speed,
                "intelligence" => Model.Intelligence,
                "libido" => Model.Libido,
                "sensitivity" => Model.Sensitivity,
                "corruption" => Model.Corruption,
                "health" or "hp" => Model.Health,
                "maxhealth" or "maxhp" => Model.MaxHealth,
                "lust" => Model.Lust,
                "maxlust" => Model.MaxLust,
                "minlust" => Model.MinLust,
                "fatigue" => Model.Fatigue,
                "maxfatigue" => Model.MaxFatigue,
                "level" => Model.Level,
                "experience" or "xp" => Model.Experience,
                "requiredxp" => Model.RequiredXp,
                "gems" => Model.Gems,
                "day" => Model.DayText,
                "time" => Model.TimeText,
                "attackdamage" => Model.AttackDamage,
                "physicaldamage" => Model.PhysicalDamage,
                "criticalchance" => Model.CriticalChance,
                "weaponcrit" => Model.WeaponCrit,
                "dodgechance" => Model.DodgeChance,
                "parrychance" => Model.ParryChance,
                "armor" => Model.Armor,
                "teasechance" => Model.TeaseChance,
                "teasedamage" => Model.TeaseDamage,
                "lustresistance" => Model.LustResistance,
                "spellmod" => Model.SpellMod,
                "spellcost" => Model.SpellCost,
                _ => null
            };
    }
}
