using Hjson;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Tags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class EnemyManager : ContentManager<EnemyDefinition>
    {
        public override string Folder => "Data/Enemies";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, EnemyDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (!root.TryGetValue("enemies", out var value) || value is not JsonArray enemies)
                throw new FormatException($"Enemy file '{path}' has no 'enemies' array");

            foreach (var entry in enemies)
            {
                if (entry is not JsonObject json)
                    continue;

                var id = json.Str("id");

                if (!Identifier.TryParse(id, out var identifier))
                    throw new FormatException($"Invalid enemy id '{id}' in '{path}'");

                var definition = new EnemyDefinition
                {
                    Id = identifier,

                    Type = json.Str("type"),

                    NameKey = json.Str("nameKey"),

                    DescriptionKey = json.Str("descriptionKey"),

                    Article = json.Str("article", "the "),

                    Level = json.Float("level"),

                    Health = json.Float("health"),

                    Lust = json.Float("lust"),

                    Strength = json.Float("strength"),

                    Toughness = json.Float("toughness"),

                    Speed = json.Float("speed"),

                    Intelligence = json.Float("intelligence"),

                    Libido = json.Float("libido"),

                    Sensitivity = json.Float("sensitivity"),

                    Corruption = json.Float("corruption"),

                    Gems = json.Int("gems"),

                    Experience = json.Float("experience"),

                    BonusHealth = json.Float("bonusHealth")
                };

                if (definition.Type == "")
                    throw new FormatException($"Enemy '{identifier}' has no type in '{path}'");

                if (json.TryId("ai", out var ai))
                    definition.AiId = ai;

                if (json.TryId("defeatScene", out var defeatScene))
                    definition.DefeatScene = defeatScene;

                if (json.TryId("victoryScene", out var victoryScene))
                    definition.VictoryScene = victoryScene;

                if (json.TryId("fleeScene", out var fleeScene))
                    definition.FleeScene = fleeScene;

                definition.Tags = TagSet.NormalizeAll(json.Strings("tags"));

                definition.Plural = json.Bool("plural");

                definition.Pronoun1 = json.Str("pronoun1");

                definition.Pronoun2 = json.Str("pronoun2");

                definition.Pronoun3 = json.Str("pronoun3");

                definition.ChainLoot = json.Bool("chainLoot");

                definition.WeightedLoot = json.Bool("weightedLoot");

                if (json.Object("attackAvoid") is { } avoid)
                    definition.AttackAvoid = new EnemyAvoidProfile
                    {
                        Dodge = avoid.Bool("dodge", true),

                        Parry = avoid.Bool("parry", true),

                        Block = avoid.Bool("block", true),

                        Counter = avoid.Bool("counter", true),

                        Fatigue = avoid.Bool("fatigue", true)
                    };

                // 初始装备候选（按权重随机挑一个）
                if (json.Array("weapons") is { } weapons)
                    definition.Weapons = ParseEquipment(weapons);

                if (json.Array("armors") is { } armors)
                    definition.Armors = ParseEquipment(armors);

                if (json.Array("shields") is { } shields)
                    definition.Shields = ParseEquipment(shields);

                if (json.Float("weaponAttack", -1f) >= 0)
                    definition.WeaponAttack = json.Float("weaponAttack");

                if (json.Float("armorDefense", -1f) >= 0)
                    definition.ArmorDefense = json.Float("armorDefense");

                if (json.Float("shieldBlock", -1f) >= 0)
                    definition.ShieldBlock = json.Float("shieldBlock");

                if (json.Float("fireResistance", -1f) >= 0)
                    definition.FireResistance = json.Float("fireResistance");

                if (json.Array("loot") is { } loot)
                    foreach (var element in loot)
                    {
                        if (element is not JsonObject entryJson)
                            continue;

                        // 加权掉落表里允许「什么都不掉」的条目：没有 item 就是空手。
                        entryJson.TryId("item", out var item);

                        if (string.IsNullOrEmpty(item.Path) && !entryJson.Bool("nothing"))
                            continue;

                        definition.Loot.Add(new EnemyLoot
                        {
                            Item = item,

                            Chance = entryJson.Float("chance", 1f),

                            Min = entryJson.Int("min", 1),

                            Max = entryJson.Int("max", 1),

                            Weight = entryJson.Float("weight", 1f)
                        });
                    }

                entries[identifier.ToString()] = definition;
            }
        }

        // 解析 [{ item: "coc:x", weight: 2 }, ...] 这类候选列表
        private static List<EnemyEquipment> ParseEquipment(JsonArray array)
        {
            var list = new List<EnemyEquipment>();

            foreach (var element in array)
            {
                if (element is not JsonObject entryJson)
                    continue;

                if (!entryJson.TryId("item", out var item) || string.IsNullOrEmpty(item.Path))
                    continue;

                list.Add(new EnemyEquipment
                {
                    Item = item,

                    Weight = entryJson.Float("weight", 1f)
                });
            }

            return list;
        }
    }
}