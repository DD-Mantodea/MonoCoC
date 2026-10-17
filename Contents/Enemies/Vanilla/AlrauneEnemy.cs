using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AlrauneEnemy>("coc:alraune")]
    public class AlrauneEnemy : Enemy
    {

        public bool Rooted { get; set; } = true;

        public bool DidTrip { get; set; }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2); 

            CreateBreastRow(2f);       

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1; 

            anal.Wetness = 0;   

            Tallness = 5 * 12 + 4;

            Body.SetHipsRating("average");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Color = "deep purple";

            hair.Length = 4;

            WeaponName = "vines";

            WeaponVerb = "whip";

            WeaponAttack = 25;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "skin";

            ArmorDefense = 15;

            ShieldName = "vines";

            ShieldBlock = 20;

            LustVulnerability = 0.4f;

            Gems = 25 + Random.Shared.Next(15);

            CreatePerk(PerkIds.PoisonImmune);

            CreatePerk(PerkIds.BleedImmune);

            FireResistance = 1.5f;

            IsImmobilized = true;
        }

        public override string Description =>

            "Scenes.Alraune.Description".Localized(Rooted

                ? "Scenes.Alraune.Description.Rooted".Localized()

                : "Scenes.Alraune.Description.Uprooted".Localized());

        public void Root(bool silent = false)
        {
            if (Rooted)
                return;

            Rooted = true;

            IsImmobilized = true;

            ApplyStats(new DynStat(GameStats.Speed, -40f));

            WeaponName = "vines";

            WeaponVerb = "whip";

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorDefense += 10;

            ShieldBlock += 15;

            LustVulnerability -= 0.4f;

            RemovePerk(PerkIds.ExtraDodge);

            AddHp(MaxHealth * 0.05f);
        }

        public void Uproot(bool silent = false)
        {
            if (!Rooted)
                return;

            Rooted = false;

            IsImmobilized = false;

            ApplyStats(new DynStat(GameStats.Speed, 40f));

            WeaponName = "claws";

            WeaponVerb = "swipe";

            NormalAttackAvoid.DoParry = true;

            NormalAttackAvoid.DoCounter = true;

            ArmorDefense = Math.Max(0, ArmorDefense - 10);

            ShieldBlock = Math.Max(0, ShieldBlock - 15);

            LustVulnerability += 0.4f;

            CreatePerk(PerkIds.ExtraDodge, 20);
        }

        public void RootPassives(Creature target)
        {
            if (target.HasStatusEffect(StatusEffectIds.StimulatingAura))
            {
                var aura = target.StatusEffectByType(StatusEffectIds.StimulatingAura);

                aura.Values[1] += 2;
            }
            else
                target.CreateStatusEffect(StatusEffectIds.StimulatingAura, 0, 2);

            target.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            ChangeFatigue(-5);
        }

        public override void CombatRoundUpdate(CombatContext context)
        {
            DidTrip = false;

            if (Rooted)
                RootPassives(context.Player);
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction is CombatReactionContext.Distanced or CombatReactionContext.Approached)
                return VineTrip(context);

            return true;
        }

        public bool VineTrip(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            var dodged = avoid.Outcome != AvoidOutcome.Hit;

            context.WriteAction("Scenes.Alraune.VineTrip.HiddenVinesTrip".Localized(dodged

                ? "Scenes.Alraune.VineTrip.NearlySnaring".Localized()

                : "Scenes.Alraune.VineTrip.SnaringFall".Localized()));

            DidTrip = !dodged;

            return dodged;
        }

        public override float GetBaseCritChance() => Rooted ? 5f : 15f;

        public override float GetEvasionChance() => Rooted ? 0f : base.GetEvasionChance();

        public override bool HandleAwardItemText(CombatContext context, Identifier item)
        {
            var key = item.Path switch
            {
                "coal" => "Scenes.Alraune.HandleAwardItemText.CoalEyeshadow",

                "peach_whiskey" => "Scenes.Alraune.HandleAwardItemText.WhiskeyBottle",

                "pumpkin_seed" => "Scenes.Alraune.HandleAwardItemText.PumpkinSeedFound",

                _ => null
            };

            if (key == null)
                return false;

            context.Write(key.Localized());

            return true;
        }
    }
}