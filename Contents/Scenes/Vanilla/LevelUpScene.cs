using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Perks;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    /// <summary>
    /// 原版 PlayerInfo.hx 的升级流程：levelUpGo → 属性点分配 → 专长点购买。
    /// </summary>
    [Register<LevelUpScene>("coc:level_up")]
    public class LevelUpScene : Scene
    {
        private static readonly string[] StatOrder = ["str", "tou", "spe", "int"];

        private int _tempStatPoints;

        private int _perkPage;

        private const int PerkPageSize = 10;

        private readonly Dictionary<string, int> _tempStat = new()
        {
            ["str"] = 0,
            ["tou"] = 0,
            ["spe"] = 0,
            ["int"] = 0,
        };

        public override void Entry() => LevelUpGo();

        // 原版 PlayerInfo.hx:789 levelUpGo()
        public void LevelUpGo()
        {
            ClearOutput();

            ClearButtons();

            if (Player.CanLevelUp())
            {
                var oldPerkPoints = (int)MathF.Floor(Player.PerkPoints);

                var oldStatPoints = (int)MathF.Floor(Player.StatPoints);

                var oldHp = (int)MathF.Floor(Player.MaxHealth);

                Player.RaiseLevel();

                var perkGain = (int)MathF.Floor(Player.PerkPoints) - oldPerkPoints;

                var statGain = (int)MathF.Floor(Player.StatPoints) - oldStatPoints;

                var hpGain = (int)MathF.Floor(Player.MaxHealth) - oldHp;

                OutputText("Scenes.LevelUp.NowLevel".Localized(Utils.Utils.Num2Text(Player.Level)));

                OutputText("Scenes.LevelUp.Gained".Localized(
                    Utils.Utils.Num2Text(hpGain),

                    Utils.Utils.NumberOfThings(statGain, "Common.Text.AttributePoint".Localized()),

                    Utils.Utils.NumberOfThings(perkGain, "Common.Text.PerkPoint".Localized())));

                if (Player.StatsMaxed())
                    OutputText(Player.StatPoints > statGain

                        ? "Scenes.LevelUp.StatsMaxedWithTotal".Localized(Player.StatPoints)

                        : "Scenes.LevelUp.StatsMaxed".Localized());

                if (!Player.CanBuyPerks())
                    OutputText(Player.PerkPoints > perkGain

                        ? "Scenes.LevelUp.NoNewPerksWithTotal".Localized(Player.PerkPoints)

                        : "Scenes.LevelUp.NoNewPerks".Localized());

                if (Player.CanBuyStats())
                {
                    InitAttributeMenu();

                    return;
                }

                if (Player.CanBuyPerks())
                {
                    PerkBuyMenu();

                    return;
                }

                AddNextButton("Common.Button.Next".Localized(), PlayerMenu);

                return;
            }

            if (Player.CanBuyStats())
            {
                InitAttributeMenu();

                return;
            }

            if (Player.CanBuyPerks())
            {
                PerkBuyMenu();

                return;
            }

            OutputText("Scenes.LevelUp.Error".Localized());

            AddNextButton("Common.Button.Next".Localized(), PlayerMenu);
        }

        // 原版 initAttributeMenu()
        private void InitAttributeMenu()
        {
            _tempStatPoints = (int)MathF.Floor(Player.StatPoints);

            foreach (var stat in StatOrder)
                _tempStat[stat] = 0;

            AttributeMenu();
        }

        // 原版 attributeMenu()
        private void AttributeMenu()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.LevelUp.PointsLeft".Localized(_tempStatPoints));

            for (var index = 0; index < StatOrder.Length; index++)
            {
                var stat = StatOrder[index];

                var name = StatName(stat);

                var current = Player.GetStatByString(stat);

                var max = Player.GetMaxStats(stat);

                if (current < max)
                    OutputText("Scenes.LevelUp.StatLine".Localized(name, current, _tempStat[stat], current + _tempStat[stat]));
                else
                    OutputText("Scenes.LevelUp.StatMax".Localized(name, current));

                var add = AddButton(index, "Scenes.LevelUp.Add".Localized(StatAbbr(stat)), () => AddAttribute(stat));

                add?.Hint("Scenes.LevelUp.AddHint".Localized(name));

                if (current + _tempStat[stat] >= max)
                    add?.DisableIf(true, "Scenes.LevelUp.AddMax".Localized(name));

                if (_tempStatPoints <= 0)
                    add?.DisableIf(true, "Scenes.LevelUp.NoPoints".Localized());

                var sub = AddButton(index + 5, "Scenes.LevelUp.Sub".Localized(StatAbbr(stat)), () => SubtractAttribute(stat));

                sub?.Hint("Scenes.LevelUp.SubHint".Localized(name));

                if (_tempStat[stat] <= 0)
                    sub?.DisableIf(true, "Scenes.LevelUp.SubNone".Localized(name));
            }

            AddButton(10, "Scenes.LevelUp.Reset".Localized(), InitAttributeMenu);

            AddButton(14, "Scenes.LevelUp.Done".Localized(), FinishAttributes);
        }

        private static string StatAbbr(string stat) => stat switch
        {
            "str" => "Scenes.LevelUp.StatAbbr.Strength".Localized(),

            "tou" => "Scenes.LevelUp.StatAbbr.Toughness".Localized(),

            "spe" => "Scenes.LevelUp.StatAbbr.Speed".Localized(),

            _ => "Scenes.LevelUp.StatAbbr.Intelligence".Localized(),
        };

        private static string StatName(string stat) => stat switch
        {
            "str" => "Scenes.LevelUp.StatName.Strength".Localized(),

            "tou" => "Scenes.LevelUp.StatName.Toughness".Localized(),

            "spe" => "Scenes.LevelUp.StatName.Speed".Localized(),

            _ => "Scenes.LevelUp.StatName.Intelligence".Localized(),
        };

        // 原版 addAttribute()/increase()（C# 没有 Shift 连加）
        private void AddAttribute(string stat)
        {
            if (_tempStatPoints <= 0)
                return;

            if (Player.GetStatByString(stat) + _tempStat[stat] >= Player.GetMaxStats(stat))
                return;

            _tempStatPoints--;

            _tempStat[stat]++;

            AttributeMenu();
        }

        // 原版 subtractAttribute()
        private void SubtractAttribute(string stat)
        {
            if (_tempStat[stat] <= 0)
                return;

            _tempStat[stat]--;

            _tempStatPoints++;

            AttributeMenu();
        }

        // 原版 finishAttributes()
        private void FinishAttributes()
        {
            ClearOutput();

            ClearButtons();

            foreach (var stat in StatOrder)
            {
                var gained = _tempStat[stat];

                if (gained <= 0)
                    continue;

                OutputText(stat switch
                {
                    "str" => gained < 3

                        ? "Scenes.LevelUp.StrSlightly".Localized()

                        : "Scenes.LevelUp.StrSignificantly".Localized(),

                    "tou" => gained < 3

                        ? "Scenes.LevelUp.TouSlightly".Localized()

                        : "Scenes.LevelUp.Tou".Localized(),

                    "spe" => gained < 3

                        ? "Scenes.LevelUp.SpeSlightly".Localized()

                        : "Scenes.LevelUp.Spe".Localized(),

                    _ => gained < 3

                        ? "Scenes.LevelUp.IntSlightly".Localized()

                        : "Scenes.LevelUp.Int".Localized(),
                });
            }

            if (_tempStatPoints > 0)
                OutputText("Scenes.LevelUp.RemainingPoints".Localized());

            Player.ApplyStats(
                new DynStatsOptions(Scale: false),

                new DynStat(GameStats.Strength, _tempStat["str"]),

                new DynStat(GameStats.Toughness, _tempStat["tou"]),

                new DynStat(GameStats.Speed, _tempStat["spe"]),

                new DynStat(GameStats.Intelligence, _tempStat["int"]));

            Player.StatPoints = _tempStatPoints;

            if (Player.CanBuyPerks())
            {
                PerkBuyMenu();

                return;
            }

            AddNextButton("Common.Button.Next".Localized(), PlayerMenu);
        }

        // 原版 perkBuyMenu()：列出可购买和条件还没满足的专长（带需求），分页显示
        private void PerkBuyMenu()
        {
            ClearOutput();

            ClearButtons();

            var available = PerkPurchaseCatalog.Available(Player);

            var unavailable = PerkPurchaseCatalog.Unavailable(Player);

            var combined = new List<PerkPurchase>(available);

            combined.AddRange(unavailable);

            if (combined.Count == 0)
            {
                OutputText("Scenes.LevelUp.NoNewPerks".Localized());

                AddNextButton("Common.Button.Next".Localized(), PlayerMenu);

                return;
            }

            var pages = (combined.Count + PerkPageSize - 1) / PerkPageSize;

            _perkPage = Math.Clamp(_perkPage, 0, pages - 1);

            OutputText("Scenes.LevelUp.PerkPointsLeft".Localized((int)Player.PerkPoints));

            OutputText("Scenes.LevelUp.PerkPage".Localized(_perkPage + 1, pages));

            var start = _perkPage * PerkPageSize;

            for (var index = 0; index < PerkPageSize && start + index < combined.Count; index++)
            {
                var purchase = combined[start + index];

                var canBuy = purchase.IsAvailable(Player) && Player.PerkPoints >= purchase.Cost;

                var button = AddButton(index, purchase.DisplayName(), () => BuyPerk(purchase));

                var hint = purchase.Description() + "\n" + "Scenes.LevelUp.PerkCost".Localized(purchase.Cost);

                if (purchase.Requirements.Count > 0)
                    hint += "\n" + "Scenes.LevelUp.PerkRequires".Localized(purchase.RequirementText());

                button?.Hint(hint);

                if (!canBuy)
                    button?.DisableIf(true, purchase.IsAvailable(Player)

                        ? "Scenes.LevelUp.PerkNoPoints".Localized()

                        : "Scenes.LevelUp.PerkRequires".Localized(purchase.RequirementText()));
            }

            if (_perkPage > 0)
                AddButton(10, "Common.Button.Previous".Localized(), () =>
                {
                    _perkPage--;

                    PerkBuyMenu();
                });

            if (_perkPage < pages - 1)
                AddButton(11, "Common.Button.Next".Localized(), () =>
                {
                    _perkPage++;

                    PerkBuyMenu();
                });

            AddButton(14, "Common.Button.Back".Localized(), PlayerMenu);
        }

        private void BuyPerk(PerkPurchase purchase)
        {
            if (!purchase.IsAvailable(Player) || Player.PerkPoints < purchase.Cost)
                return;

            Player.PerkPoints -= purchase.Cost;

            Player.CreatePerk(purchase.Perk, 0, 0, 0, 0);

            PerkBuyMenu();
        }
    }
}