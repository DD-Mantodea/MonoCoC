using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AnemoneEnemy>("coc:anemone")]
    public class AnemoneEnemy : Enemy
    {

        public const string VenomSource = "AnemoneVenom";

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(7, 1, BodyPartTypes.Anemone);

            CreateVagina(false, 3, 3);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 5);

            CreateBreastRow(BreastCups.B);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 10);

            Tallness = 65;

            Body.Hips = 10;

            Body.Butt = 6;

            Body.Get<SkinPart>().Tone = "purple";

            var hair = Body.Get<HairPart>();

            hair.Color = "purplish-black";

            hair.Length = 20;

            hair.Id = BodyPartTypes.Anemone;

            Strength = 40;

            Toughness = 20;

            Speed = 40;

            Intelligence = 50;

            Libido = 55;

            Sensitivity = 35;

            Corruption = 50;

            WeaponName = "tendrils";

            WeaponVerb = "tentacle";

            WeaponAttack = 5;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "clammy skin";

            Lust = 30;

            LustVulnerability = 0.9f;

            Level = 4;

            Gems = Random.Shared.Next(5) + 1;

        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.AfterAttacked)
            {
                if (context.Player.Weapon.IsRanged())
                    return true;

                context.Append("Combat.Anemone.React.ThoughYouManaged".Localized());

                ApplyVenom(context, 1 + context.Random.Next(2));

                return true;
            }

            if (reaction != CombatReactionContext.WhenAttacked)
                return true;

            if (context.Player.Weapon.IsRanged())
                return true;

            if (context.Player.StatusEffectValue(StatusEffectIds.CounterAB, 0) == 1)
                return true;

            if (context.Random.Next(10) > 2)
                return true;

            context.Append("Combat.Anemone.React.SeeingYourRaised".Localized());

            if (context.Player.Corruption < 75)
            {
                context.Append("Combat.Anemone.React.YouStareInto".Localized());

                return false;
            }

            context.Append("Combat.Anemone.React.ThoughYouLose".Localized());

            return true;
        }

        public override bool ModifyRunOutcome(CombatContext context, bool escaped) =>

            escaped || context.Player.Lust < 60;

        public override void RunFail(CombatContext context)
        {

            context.Append("Combat.Anemone.RunFail.Land".Localized());

            ApplyVenom(context, 4 + context.Player.Sensitivity / 20f);
        }

        public override void RunSuccess(CombatContext context)
        {

            context.Append("Combat.Anemone.RunSuccess.Land".Localized());
        }

        public static void ApplyVenom(CombatContext context, float amount)
        {
            var player = context.Player;

            player.TakeLustDamage(2 * amount, true);

            var strengthLoss = CurrentLoss(player, GameStats.Strength) + amount;

            var speedLoss = CurrentLoss(player, GameStats.Speed) + amount;

            var source = VenomStatusSource();

            player.Stats.RemoveFrom(source);

            player.Stats.AddModifier(GameStats.Strength, ModifierOperation.Addition, () => -strengthLoss, source);

            player.Stats.AddModifier(GameStats.Speed, ModifierOperation.Addition, () => -speedLoss, source);

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, 0, VenomSource, VenomStatus());
        }

        public static float CurrentLoss(Creature creature, Identifier stat)
        {
            var source = VenomStatusSource();

            return -creature.Stats.From(source).Where(modifier => modifier.Stat.Id == stat).Sum(modifier => modifier.Resolve());
        }

        public static StatusSource VenomStatusSource() =>

            new(new Identifier("coc", "combat_buff_" + VenomSource.ToLowerInvariant().Replace(' ', '_')), VenomSource);

        public static Identifier VenomStatus() => new("coc", "anemone_venom");

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.AnemoneScene>.Get().DefeatAnemone();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Anemone.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Npcs.AnemoneScene>.Get().LoseToAnemone();

            return true;
        }
    }
}