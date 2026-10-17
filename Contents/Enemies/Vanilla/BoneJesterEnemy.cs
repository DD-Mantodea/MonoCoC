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

    [Register<BoneJesterEnemy>("coc:bone_jester")]
    public class BoneJesterEnemy : Enemy
    {

        public bool Hidden;

        public float PrevHp;

        public static readonly Identifier HideInShadowsId = new("coc", "bone_jester_hide_in_shadows");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            Tallness = 80;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "purple";

            hair.Length = 4;

            WeaponAttack = 60;

            ArmorDefense = 0;

            WeaponName = "jagged daggers";

            WeaponVerb = "slash";

            ArmorName = "Jester's Garb";

            LustVulnerability = 0;

            Gems = Random.Shared.Next(5) + 100;

            CreatePerk(PerkIds.BleedImmune);

            CreatePerk(PerkIds.PoisonImmune);
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (PrevHp > Health)
                Hidden = false;

            PrevHp = Health;

            if (Hidden)
                return new CombatAiAction { Ability = HideInShadowsId };

            return interpreter.Choose();
        }

        public override void TickStatuses(CombatContext context) =>

            BoneGeneralEnemy.TickGuardAb(context, this);

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.BoneJester.HandleFear.TheSkeletonSeems".Localized());

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
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.ZeroCracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            context.Append("Combat.BoneJester.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DefeatJester();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToJester();

            return true;
        }
    }
}