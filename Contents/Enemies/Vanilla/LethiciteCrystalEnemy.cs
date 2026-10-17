using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<LethiciteCrystalEnemy>("coc:lethicite_crystal")]
    public class LethiciteCrystalEnemy : Enemy
    {
        public static readonly Identifier PulseId = new("coc", "lethicite_crystal_pulse");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.Flat);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            Tallness = 65;

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "purple";

            hair.Length = 4;

            WeaponName = "fists";

            WeaponVerb = "tiny punch";

            ArmorName = "leather straps";

            ArmorDefense = 100;

            Gems = Random.Shared.Next(5) + 5;

            LustVulnerability = 0.7f;

            Stats.AddModifier(GameStats.MaxLust, ModifierOperation.Addition, () => 200f,

                new TemporarySource(Guid.NewGuid()));

            CreatePerk(PerkIds.Immovable);
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>

            new() { Ability = PulseId };

        public override bool HandleFear(CombatContext context)
        {
            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool HandleStun(CombatContext context)
        {
            RemoveStatusEffect(StatusEffectIds.Stunned);

            return true;
        }

        public override void HandleConstricted(CombatContext context)
        {
            RemoveStatusEffect(StatusEffectIds.Constricted);
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var capitalA = Utils.Utils.CapitalizeFirstLetter(Article);

            context.WriteAction((lustDelta == 0

                ? "Combat.LethiciteCrystal.OutputDefaultTeaseReaction.OneDoesn"

                : "Combat.LethiciteCrystal.OutputDefaultTeaseReaction.OneHums").Localized(capitalA, ShortName));
        }

        public override bool DescribeAttacked(CombatContext context, float damage)
        {
            var weapon = context.Player.Weapon;

            if (weapon.IsChanneling(context.Player))
                return false;

            var attackNoun = weapon.Name;

            var attackVerb = weapon.Verb;

            var theMonster = Article + ShortName;

            var theMonsterCap = Utils.Utils.CapitalizeFirstLetter(Article) + ShortName;

            var maxHealth = MaxHealth;

            var damageLow = damage < 15 || damage < maxHealth * 0.02f;

            var damageMed = damage < 50 || damage < maxHealth * 0.05f;

            var damageHigh = damage < 100 || damage < maxHealth * 0.15f;

            if (damage <= 0)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.ZeroCracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            context.Append("Combat.LethiciteCrystal.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DestroyCrystal();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToCrystal();

            return true;
        }
    }
}