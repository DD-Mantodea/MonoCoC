using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<BoneGeneralEnemy>("coc:bone_general")]
    public class BoneGeneralEnemy : Enemy
    {

        public static void TickGuardAb(CombatContext context, Enemy self)
        {
            var guard = self.StatusEffectByType(StatusEffectIds.GuardAB);

            if (guard == null)
                return;

            guard.Values[0] -= 1;

            if (guard.Values[0] <= 0)
            {
                context.Write(Utils.Utils.CapitalizeFirstLetter(self.Article) + self.ShortName + "Common.Text.IsNoLongerGuarded".Localized());

                self.RemoveStatusEffect(StatusEffectIds.GuardAB);

                return;
            }

            context.Write(Utils.Utils.CapitalizeFirstLetter(self.Article) + self.ShortName

                + "Common.Text.IsCurrentlyBeingGuardedAndIs".Localized());
        }

        public static readonly Identifier CrushingBlowId = new("coc", "bone_general_crushing_blow");

        public static readonly Identifier GuardAllyId = new("coc", "bone_general_guard_ally");

        public static readonly Identifier AttackId = new("coc", "bone_general_attack");

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

            ArmorDefense = 60;

            ArmorPerk = "Heavy";

            WeaponName = "spiked mace";

            WeaponVerb = "crush";

            ArmorName = "rusted plate armor";

            Gems = Random.Shared.Next(5) + 100;

            LustVulnerability = 0;

            CreatePerk(PerkIds.BleedImmune);

            CreatePerk(PerkIds.PoisonImmune);

            CreatePerk(PerkIds.Juggernaut);

            CreatePerk(PerkIds.ChargingSwings);
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var canCrush = !context.Player.HasStatusEffect(StatusEffectIds.Stunned);

            var canGuard = CanGuard(context);

            var total = 2 + (canCrush ? 1 : 0) + (canGuard ? 1 : 0);

            var roll = context.Random.Next(total);

            if (roll < 2)
                return new CombatAiAction { Ability = AttackId };

            roll -= 2;

            if (canCrush && roll == 0)
                return new CombatAiAction { Ability = CrushingBlowId, Cost = 10, Fatigue = FatigueType.Physical };

            return new CombatAiAction { Ability = GuardAllyId };
        }

        public bool CanGuard(CombatContext context)
        {
            if (context.Enemies.Count <= 1)
                return false;

            if (context.Enemies.Any(e => e.HasStatusEffect(StatusEffectIds.GuardAB)))
                return false;

            return context.Enemies.Any(e => e.Health > 0 && e is not BoneGeneralEnemy);
        }

        public override bool HandleStun(CombatContext context)
        {
            var result = base.HandleStun(context);

            foreach (var enemy in context.Enemies)
                enemy.RemoveStatusEffect(StatusEffectIds.GuardAB);

            return result;
        }

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.BoneGeneral.HandleFear.TheSkeletonSeems".Localized());

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
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.ZeroCracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            context.Append("Combat.BoneGeneral.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {

            if (OnDefeated != null || OnWon != null)
                return false;

            SceneInstance<ManorScene>.Get().DefeatGeneral();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (OnDefeated != null || OnWon != null)
                return false;

            SceneInstance<ManorScene>.Get().LoseToGeneral();

            return true;
        }
    }
}