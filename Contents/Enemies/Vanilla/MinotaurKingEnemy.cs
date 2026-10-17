using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MinotaurKingEnemy>("coc:minotaur_king")]
    public class MinotaurKingEnemy : Enemy
    {

        public int MilkDrinks;

        public int Orgasms;

        public bool LastRoundStun;

        public int LastSpellCastCount;

        public int ExcelliaHp;

        public bool _dickslapQueued;

        public static readonly Identifier BackhandId = new("coc", "minotaur_king_backhand");

        public static readonly Identifier HeadbuttId = new("coc", "minotaur_king_headbutt");

        public static readonly Identifier DickslapId = new("coc", "minotaur_king_dickslap");

        public static readonly Identifier BattleaxeId = new("coc", "minotaur_king_battleaxe");

        public static readonly Identifier MinoPheromonesId = new("coc", "minotaur_king_mino_pheromones");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            ExcelliaHp = 700 + CoC.Instance.Player.NewGamePlusMod() * 500;

            Race = "Minotaur";

            Tallness = 168;

            CreateCock(24, 5, BodyPartTypes.Horse);

            CreateBreastRow(0);

            Balls = 2;

            BallSize = 4;

            HoursSinceCum = 9999;

            Body.Hips = 2;

            Body.Butt = 2;

            Strength = 100;

            Toughness = 100;

            Speed = 50;

            Intelligence = 60;

            Libido = 66;

            Sensitivity = 10;

            Corruption = 100;

            WeaponName = "axe";

            WeaponAttack = 50;

            WeaponVerb = "swing";

            ArmorName = "rags";

            ArmorDefense = 60;

            var bonusHealth = 850f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Gems = Random.Shared.Next(50) + 75;

            Level = 22;

            LustVulnerability = 0.05f;

            AdditionalExperience = 200;

            LastSpellCastCount = CoC.Instance.Flags.Get(GameFlagIds.SpellsCast);
        }

        public override string Description
        {
            get
            {
                var text = Orgasms == 0

                    ? "Npcs.MinotaurKing.Desc".Localized()

                    : "Npcs.MinotaurKing.Desc.AfterOrgasms".Localized();

                if (Orgasms != 0)
                    if (LustPercent < 40)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaCalm".Localized();
                    else if (LustPercent < 80)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaFallen".Localized();
                    else
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaCreamed".Localized();

                if (MilkDrinks != 0)
                    if (MilkDrinks == 1)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.MilkFirst".Localized();
                    else if (MilkDrinks < 10)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.MilkMore".Localized();
                    else
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.MilkFull".Localized();

                if (ExcelliaHp <= 0)
                    text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaKO".Localized();

                return text;
            }
        }

        public override bool HandleStun(CombatContext context)
        {
            context.WriteAction("Combat.MinotaurKing.HandleStun.ItOnlyTakes".Localized());

            RemoveStatusEffect(StatusEffectIds.Stunned);

            return true;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (!LastRoundStun)
                return false;

            LastRoundStun = false;

            if (context.Player.HasStatusEffect(StatusEffectIds.Stunned))
            {
                _dickslapQueued = true;

                return false;
            }

            return true;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (_dickslapQueued)
            {
                _dickslapQueued = false;

                return new CombatAiAction { Ability = DickslapId };
            }

            var spellsCast = CoC.Instance.Flags.Get(GameFlagIds.SpellsCast);

            if (LastSpellCastCount != spellsCast)
            {
                LastSpellCastCount = spellsCast;

                return new CombatAiAction { Ability = HeadbuttId };
            }

            var options = new[]
            {
                BackhandId, BattleaxeId, MinoPheromonesId
            };

            return new CombatAiAction { Ability = options[context.Random.Next(options.Length)] };
        }

        public void HpRestore(CombatContext context)
        {
            Health = MaxHealth;

            LustVulnerability += 0.15f;

            Lust += 2;

            MilkDrinks += 1;

            context.Append("Combat.MinotaurKing.HpRestore.StaggeringBackThe".Localized());

            context.Append("Combat.MinotaurKing.HpRestore.HeLooksLike".Localized());

            if (MilkDrinks >= 10)
                context.Append("Combat.MinotaurKing.HpRestore.ItLooksLike".Localized());
        }

        public void LustDump(CombatContext context)
        {
            Orgasms += 1;

            context.Append("Combat.MinotaurKing.LustDump.TheIncrediblyAroused".Localized());

            context.Append("Combat.MinotaurKing.LustDump.AMonstrousHand".Localized());

            context.Append("Combat.MinotaurKing.LustDump.TheBruteFucks".Localized());

            context.Append("Combat.MinotaurKing.LustDump.ExcelliaSMaster".Localized());

            if (context.Player.HasPerk(PerkIds.MinotaurCumAddict))
                context.Append("Combat.MinotaurKing.LustDump.YouReJealous".Localized());

            context.Append("Combat.MinotaurKing.LustDump.WhateverSpellThis".Localized());

            Lust = 0;
        }

        public override DefeatResult ResolveDefeated(CombatContext context, bool hpVictory)
        {
            if (!hpVictory && Orgasms == 0)
            {
                LustDump(context);

                TookAction = true;

                return DefeatResult.Continue;
            }

            if (hpVictory && MilkDrinks < 10)
            {
                HpRestore(context);

                TookAction = true;

                return DefeatResult.Continue;
            }

            return DefeatResult.Ended;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<MinotaurKingScenes>.Get().TheKingIsDeadLongLiveTheKing(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<MinotaurKingScenes>.Get().HailToTheKingBaby(hpVictory);

            return true;
        }
    }
}