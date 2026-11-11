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
                _ => null
            };
    }
}
