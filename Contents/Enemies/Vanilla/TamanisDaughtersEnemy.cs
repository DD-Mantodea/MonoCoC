using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;
using System.Text;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<TamanisDaughtersEnemy>("coc:tamanis_daughters")]
    public class TamanisDaughtersEnemy : GoblinEnemy
    {

        public const string TamaniLurkingDescription =

            " <b>Tamani lurks in the back of the crowd, curvier than her brood and watching with a mixture of " +

            "amusement and irritation. She runs a hand through her pink and black hair, waiting for an opportunity " +

            "to get involved...</b>";

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var flags = CoC.Instance.Flags;

            var daughters = flags.Get(GameFlagIds.TamaniNumberOfDaughters);

            Level = Math.Min(30, 8 + daughters / 20);

            var bonusHealth = Math.Min(3350, 50 + daughters / 2 * 15);

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            LustVulnerability = .65f;

            Gems = Random.Shared.Next(15) + 5;
        }

        protected override void ConfigureBody()
        {

            CreateVagina(false, 4, 0);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateBreastRow(BreastCups.D);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 25);

            Tallness = 40;

            Body.SetHipsRating("ample", 1);

            Body.SetButtRating("noticeable", 1);

            Body.Get<SkinPart>().Tone = "greenish gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "pink";

            hair.Length = 16;

            WeaponName = "fists";

            WeaponVerb = "tiny punches";

            ArmorName = "leather straps";
        }

        public override string Description =>

            TamanisDaughtersScene.TamaniPresent ? base.Description + TamaniLurkingDescription : base.Description;

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<TamanisDaughtersScene>.Get().CombatWinAgainstDaughters();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scene.OutputText("Scenes.TamanisDaughters.Won.FoesDisgustedLeave".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<TamanisDaughtersScene>.Get().LoseToDaughters();

            return true;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            MidRoundMadness(context);

            TamaniShowsUp(context);

            ApplyAttacks();

            return false;
        }

        public static void MidRoundMadness(CombatContext context)
        {
            var player = context.Player;

            var text = new StringBuilder();

            switch (context.Random.Next(4))
            {
                case 0:
                    text.Append("Scenes.TamanisDaughters.MidRoundMadness.SlenderHandReaches".Localized());

                    if (player.Balls > 0)
                    {
                        if (context.Random.Next(2) == 0)
                            text.Append(player.MultiCockDescriptLight());
                        else
                            text.Append(player.BallsDescriptLight());
                    }
                    else
                        text.Append(player.MultiCockDescriptLight());

                    text.Append("Scenes.TamanisDaughters.MidRoundMadness.GentleSqueeze".Localized());

                    break;

                case 1:
                    text.Append("Scenes.TamanisDaughters.MidRoundMadness.GirlLatchesLegs".Localized());

                    break;

                case 2:
                    text.Append("Scenes.TamanisDaughters.MidRoundMadness.DaughterLaunchesBack".Localized());

                    text.Append("Scenes.TamanisDaughters.MidRoundMadness.ReachBackThrow".Localized());

                    break;

                default:
                    text.Append("Scenes.TamanisDaughters.MidRoundMadness.DaughterJills".Localized());

                    break;
            }

            var lustDamage = (int)(1 + player.Libido / 15 + context.Random.Next((int)(player.Corruption / 30)));

            player.TakeLustDamage(lustDamage, true, false);

            text.Append("Scenes.TamanisDaughters.MidRoundMadness.ParagraphBreak".Localized());

            context.WriteAction(text.ToString());
        }

        public static void TamaniShowsUp(CombatContext context)
        {
            if (TamanisDaughtersScene.TamaniPresent)
            {

                if (context.Random.Next(4) == 0)
                    TamanisDaughtersCombatHandlers.GoblinDrugAttack(context, null!);

                return;
            }

            if (context.Random.Next(6) != 0)
                return;

            TamanisDaughtersScene.TamaniPresent = true;

            context.WriteAction("Scenes.TamanisDaughters.TamaniShowsUp.FamiliarVoiceCalls".Localized());

            context.WriteAction("Scenes.TamanisDaughters.TamaniShowsUp.TamaniLurking".Localized());

            var enemy = context.Enemy;

            enemy.Strength += 5;

            enemy.Toughness += 5;

            enemy.Health += 10;

            enemy.Lust -= 20;
        }

        public void ApplyAttacks()
        {
            var daughters = CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters);

            var stacks = Math.Min(20, daughters / 20);

            var effect = CreateOrFindStatusEffect(StatusEffectIds.Attacks, stacks);

            effect.Values[0] = stacks;
        }
    }
}