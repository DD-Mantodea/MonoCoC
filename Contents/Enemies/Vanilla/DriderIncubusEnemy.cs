using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<DriderIncubusEnemy>("coc:drider_incubus")]
    public class DriderIncubusEnemy : Enemy
    {

        public bool GoblinFree;

        public int CombatRound;

        public int HpGains;

        public bool SeenResolute;

        public bool GoblinWebChain;

        public bool GoblinOiled;

        public static readonly Identifier DriderIncubusVenomId = new("coc", "drider_incubus_venom");

        public static readonly Identifier TaintedMindId = new("coc", "tainted_mind");

        public static readonly Identifier PurpleHazeId = new("coc", "purple_haze");

        public static readonly Identifier RoundId = new("coc", "drider_incubus_round");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Race = "Demon";

            Tallness = 108;

            CreateCock(24, 5, BodyPartTypes.Demon);

            CreateBreastRow(0);

            Balls = 2;

            BallSize = 4;

            HoursSinceCum = 9999;

            Body.Hips = 2;

            Body.Butt = 2;

            Strength = 65;

            Toughness = 80;

            Speed = 90;

            Intelligence = 70;

            Libido = 66;

            Sensitivity = 40;

            Corruption = 100;

            WeaponName = "spear";

            WeaponAttack = 19;

            WeaponVerb = "lunge";

            ArmorName = "chitin";

            ArmorDefense = 40;

            var bonusHealth = 500f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Gems = Random.Shared.Next(50) + 75;

            Level = 22;

            AdditionalExperience = 200;

            LustVulnerability = 0.45f;

        }

        public override string Description
        {
            get
            {
                var text = "Npcs.DriderIncubus.Desc".Localized();

                text += GoblinFree

                    ? "Npcs.DriderIncubus.Desc.Freed".Localized()

                    : "Npcs.DriderIncubus.Desc.Harness".Localized();

                return text;
            }
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>

            new CombatAiAction { Ability = RoundId };

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.BeforeAttacked)
                return true;

            if (!context.Player.HasStatusEffect(TaintedMindId))
                return true;

            DriderIncubusCombatHandlers.TaintedMindAttackAttempt(context);

            return false;
        }

        public override void CombatRoundUpdate(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(PurpleHazeId))
            {
                player.ChangeStatusValue(PurpleHazeId, 0, -1);

                if (player.StatusEffectValue(PurpleHazeId, 0) <= 0)
                {
                    player.RemoveStatusEffect(PurpleHazeId);

                    player.RemoveStatusEffect(StatusEffectIds.Blind);

                    context.Append("Combat.DriderIncubus.CombatRound.PurpleHazeEnds".Localized());
                }
                else
                {
                    context.Append("Combat.DriderIncubus.CombatRound.PurpleHazeTick".Localized());

                    player.TakeLustDamage(3, true);
                }
            }

            if (player.HasStatusEffect(TaintedMindId))
            {
                player.ChangeStatusValue(TaintedMindId, 0, -1);

                if (player.StatusEffectValue(TaintedMindId, 0) <= 0)
                {
                    player.RemoveStatusEffect(TaintedMindId);

                    context.Append("Combat.DriderIncubus.CombatRound.TaintedMindEnds".Localized());
                }
            }

            if (player.HasStatusEffect(DriderIncubusVenomId) &&
                player.HasPerk(PerkIds.Medicine) &&

                context.Random.Next(100) <= 14)
            {
                player.RemoveStatusEffect(DriderIncubusVenomId);

                context.Append("Combat.DriderIncubus.CombatRound.VenomCleansed".Localized());
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DriderIncubusScenes>.Get().BeatTheSpooderbutt(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<DriderIncubusScenes>.Get().SpooderbuttGetsANewCockSleeve(hpVictory);

            return true;
        }
    }
}