using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<DullahanEnemy>("coc:dullahan")]
    public class DullahanEnemy : Enemy
    {

        public int Determined;

        public bool IsRude => (CoC.Instance?.Flags.Get(GameFlagIds.DullahanRude) ?? 0) == 2;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 1);

            CreateBreastRow(6f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 2;

            Tallness = 60;

            Body.SetHipsRating("slender");

            Body.SetButtRating("average");

            Body.Get<SkinPart>().Tone = "pale blue";

            var hair = Body.Get<HairPart>();

            hair.Color = "white";

            hair.Length = 20;

            Fatigue = 0;

            ArmorName = "black and gold armor";

            ArmorDefense = 30;

            Lust = 5 + Random.Shared.Next(15);

            Gems = 30;

            WeaponName = "saber";

            WeaponVerb = "slash";

            if (!IsRude)
            {
                WeaponAttack = 20;

                LustVulnerability = 0.46f;

                return;
            }

            Level = 25;

            WeaponAttack = 50;

            LustVulnerability = 0;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => 1120f, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (!IsRude)
                return false;

            if (HealthRatio() <= 0.5f && Determined == 0)
            {
                Determination(context);

                return true;
            }

            if (HealthRatio() <= 0.25f && Determined == 1)
            {
                Determination(context);

                return true;
            }

            return false;
        }

        public void Determination(CombatContext context)
        {
            context.WriteAction(Determined == 0

                ? "Combat.Dullahan.Determination.First".Localized()

                : "Combat.Dullahan.Determination.Second".Localized());

            context.WriteAction("Combat.Dullahan.Determination.Line".Localized());

            Strength += 10;

            Speed += 30;

            WeaponAttack += 10;

            Determined += 1;

            var healed = MaxHealth * 0.25f;

            HealthChange(healed);

            context.WriteAction("Combat.Dullahan.Determination.Heal".Localized(healed));
        }

        public override bool HandleStun(CombatContext context)
        {
            if (IsRude && Random.Shared.Next(2) == 0)
            {
                context.WriteAction("Combat.Dullahan.Stun.Determined".Localized());

                RemoveStatusEffect(StatusEffectIds.Stunned);

                return true;
            }

            context.WriteAction("Combat.Dullahan.Stun.BodyKeepsGoing".Localized());

            RemoveStatusEffect(StatusEffectIds.Stunned);

            CreateStatusEffect(StatusEffectIds.Blind, 3, 0, 0, 0, 3);

            return true;
        }

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction(IsRude

                ? "Combat.Dullahan.Fear.Rude".Localized()

                : "Combat.Dullahan.Fear.Normal".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override string? RunCheck(CombatContext context) => "Combat.Dullahan.Run".Localized();

        public override int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost)
        {
            if (HasStatusEffect(StatusEffectIds.Spar))
            {
                if (context.Player.Health <= 0)
                    context.Player.Health = 1;

                return 1;
            }

            return base.HandleCombatLossText(context, false, 10);
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            switch (reaction)
            {
                case CombatReactionContext.WhenAttacked:
                    {
                        var chance = 90f;

                        if (HasStatusEffect(StatusEffectIds.Blind))
                            chance -= 30;

                        if (IsRude)
                            chance += 25;

                        chance -= context.Player.Speed / 3f;

                        chance -= Lust / 2f;

                        if (context.Random.Next(100) >= chance)
                            return true;

                        var thorns = CombatCalculator.PhysicalDamage(context, this, context.Player) * 0.75f +

                            context.Random.Next(15);

                        context.WriteAction("Combat.Dullahan.Counter".Localized(thorns));

                        context.Player.HealthChange(-thorns);

                        return false;
                    }

                case CombatReactionContext.PlayerWaited:
                    {
                        if (!context.Player.IsDisarmed())
                            return true;

                        context.WriteAction("Combat.Dullahan.PickUpWeapon".Localized());

                        WeaponName = "saber";

                        WeaponVerb = "slash";

                        WeaponAttack = 50;

                        Speed -= 20;

                        context.Player.Rearm();

                        return false;
                    }

                default:
                    return true;
            }
        }
    }
}