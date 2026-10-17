using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<HermCentaurEnemy>("coc:herm_centaur")]
    public class HermCentaurEnemy : Enemy
    {

        public bool UsedGottaCum;

        public bool UsedHeal;

        public bool ChargingArouse;

        public float LustAtChargeStart = -1;

        public int ArouseCooldown;

        public int HypnoCockUses;

        public static readonly Identifier ArouseSpellCastId = new("coc", "herm_centaur_arouse_spell_cast");

        public static readonly Identifier GottaCumId = new("coc", "herm_centaur_gotta_cum");

        public static readonly Identifier HealUpId = new("coc", "herm_centaur_heal_up");

        public static readonly Identifier HypnoCockId = new("coc", "herm_centaur_hypno_cock");

        public static readonly Identifier FeminineMuskId = new("coc", "herm_centaur_feminine_musk");

        public static readonly Identifier AphrodisiacSquirtId = new("coc", "herm_centaur_aphrodisiac_squirt");

        public static readonly Identifier ArouseSpellChargeId = new("coc", "herm_centaur_arouse_spell_charge");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Race = "Demon";

            Tallness = 108;

            CreateBreastRow(5f);

            CreateCock(36, 5, BodyPartTypes.Horse);

            CreateVagina(false, 4, 5);

            Balls = 2;

            BallSize = 3;

            Body.Hips = 15;

            Body.Butt = 13;

            Strength = 100;

            Toughness = 100;

            Speed = 65;

            Intelligence = 65;

            Libido = 85;

            Sensitivity = 40;

            Corruption = 100;

            WeaponName = "fists";

            WeaponAttack = 1;

            WeaponVerb = "punch";

            ArmorName = "wraps";

            var bonusHealth = 800f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Gems = Random.Shared.Next(50) + 75;

            Level = 22;

            LustVulnerability = 0.6f;

        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            ArouseCooldown--;

            if (ChargingArouse)
                return new CombatAiAction { Ability = ArouseSpellCastId, Cost = 10, Fatigue = FatigueType.Magical };

            if (LustPercent >= 60 && !UsedGottaCum)
                return new CombatAiAction { Ability = GottaCumId, Cost = 10, Fatigue = FatigueType.None };

            if (Health <= MaxHealth * 0.5f && !UsedHeal)
                return new CombatAiAction { Ability = HealUpId, Cost = 0, Fatigue = FatigueType.MagicalHeal };

            if ((int)(context.Random.NextDouble() * Lust) >= 40)
                return new CombatAiAction { Ability = HypnoCockId, Cost = 10, Fatigue = FatigueType.Magical };

            var options = new List<CombatAiAction>
            {
                new() { Ability = FeminineMuskId, Cost = 10, Fatigue = FatigueType.Physical },

                new() { Ability = AphrodisiacSquirtId, Cost = 10, Fatigue = FatigueType.Physical }
            };

            if (!ChargingArouse && ArouseCooldown <= 0)
                options.Add(new CombatAiAction { Ability = ArouseSpellChargeId, Cost = 0, Fatigue = FatigueType.Magical });

            return options[context.Random.Next(options.Count)];
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<HermCentaurScenes>.Get().BeatThePony(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<HermCentaurScenes>.Get().InSovietCoCPonyRidesYou(hpVictory);

            return true;
        }
    }
}