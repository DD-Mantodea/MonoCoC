using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<BoneCourtierEnemy>("coc:bone_courtier")]
    public class BoneCourtierEnemy : Enemy
    {
        public static readonly Identifier TemptingGobletId = new("coc", "bone_courtier_tempting_goblet");

        public static readonly Identifier HealId = new("coc", "bone_courtier_heal");

        public static readonly Identifier MarkId = new("coc", "bone_courtier_mark");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            Tallness = 65;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "purple";

            hair.Length = 4;

            WeaponName = "fists";

            WeaponVerb = "tiny punch";

            ArmorName = "leather straps";

            Gems = Random.Shared.Next(5) + 5;

            LustVulnerability = 0;

            CreatePerk(PerkIds.BleedImmune);

            CreatePerk(PerkIds.PoisonImmune);
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var canMark = !context.Player.HasStatusEffect(StatusEffectIds.Marked);

            var total = canMark ? 5 : 4;

            var roll = context.Random.Next(total);

            if (roll < 3)
                return new CombatAiAction { Ability = TemptingGobletId };

            if (roll == 3)
                return new CombatAiAction { Ability = HealId, Cost = 15, Fatigue = FatigueType.MagicalHeal };

            return new CombatAiAction { Ability = MarkId, Cost = 10, Fatigue = FatigueType.Magical };
        }

        public override void TickStatuses(CombatContext context) =>

            BoneGeneralEnemy.TickGuardAb(context, this);

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.BoneCourtier.HandleFear.TheSkeletonSeems".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
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

            var damageLow = damage < 15 || damage < maxHealth * 0.05f;

            var damageMed = damage < 50 || damage < maxHealth * 0.20f;

            var damageHigh = damage < 100 || damage < maxHealth * 0.33f;

            if (damage <= 0)
                context.WriteAction("Combat.BoneCourtier.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.BoneCourtier.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.BoneCourtier.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.BoneCourtier.ReplacesDescribeAttacked.ZeroCracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.BoneCourtier.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            context.Append("Combat.BoneCourtier.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DefeatCourtier();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToCourtier();

            return true;
        }
    }
}