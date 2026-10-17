using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 VolcanicGolem.hx:200 performCombatAction / :251 nuke / :319 earthshatter / :240 reinforce
    [Register<VolcanicGolemEnemy>("coc:volcanic_golem")]
    public class VolcanicGolemEnemy : Enemy
    {
        // 原版 VolcanicGolem.hx:22-32：被近战攻击时熔岩反伤
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.AfterAttacked)
            {
                if (!context.Player.Weapon.IsRanged())
                {
                    var thorns = CombatCalculator.ReduceDamage(context, context.Player,

                        15 + context.Random.Next(30));

                    context.Player.HealthChange(-thorns);

                    context.Append("Combat.VolcanicGolem.MagmaSpills".Localized((int)thorns));
                }

                return true;
            }

            return base.React(context, reaction);
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Tallness = 16 * 12 + 7;

            Body.Get<SkinPart>().Tone = "black";

            WeaponName = "Stone Fists";

            ArmorName = "Rock Plates";

            var persistent = CoC.Instance.Flags.Get(GameFlagIds.Volcanicgolemhp);

            if (persistent > 0)
                Health = persistent;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            VolcanicStatus(context);

            if (Distance != CombatDistance.Melee)
            {
                DistanceAnger(context);

                return;
            }

            if (HasStatusEffect(StatusEffectIds.Stunned))
                return;

            if (!HasStatusEffect(StatusEffectIds.Uber) && !HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
            {
                var max = CoC.Instance.Flags.Get(GameFlagIds.Volcanicgolemhp);

                var ratio = max / MathF.Max(1f, Health);

                if (ratio >= 3 && Random.Shared.Next(4) == 0)
                {
                    Heal(context);

                    return;
                }

                if (ArmorDefense < 50 && Random.Shared.Next(3) == 0)
                {
                    Reinforce(context);

                    return;
                }

                if ((Random.Shared.Next(3) == 0 && !player.HasStatusEffect(StatusEffectIds.Stunned) && !player.HasPerk(PerkIds.Resolute))
                    || (Random.Shared.Next(5) == 0 && player.HasPerk(PerkIds.Resolute)))
                {
                    Earthshatter(context);

                    return;
                }

                if ((Random.Shared.Next(10) == 0 && ratio >= 2) || Random.Shared.Next(20) == 0)
                {
                    Nuke(context);

                    return;
                }

                CombatController.Instance!.Resolver.EnemyBasicAttack();

                return;
            }

            if (!HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
                Nuke(context);
            else
                Heal(context);
        }

        private void VolcanicStatus(CombatContext context)
        {
            if (HasStatusEffect(StatusEffectIds.VolcanicWeapRed))
            {
                AddStatusValue(StatusEffectIds.VolcanicWeapRed, 0, -1);

                if (StatusEffectValue(StatusEffectIds.VolcanicWeapRed, 0) < 0)
                {
                    context.Write("Combat.VolcanicGolem.WeapRegrown".Localized());

                    WeaponAttack += 40;

                    if (WeaponAttack > 120 && !HasStatusEffect(StatusEffectIds.VolcanicFrenzy))
                        WeaponAttack = 120;
                    else if (WeaponAttack > 400)
                        WeaponAttack = 400;
                }
            }

            if (HasStatusEffect(StatusEffectIds.VolcanicArmorRed))
            {
                AddStatusValue(StatusEffectIds.VolcanicArmorRed, 0, -1);

                if (StatusEffectValue(StatusEffectIds.VolcanicArmorRed, 0) % 3 == 0)
                {
                    AddStatusValue(StatusEffectIds.VolcanicArmorRed, 1, -1);

                    ArmorDefense += 150;

                    context.Write("Combat.VolcanicGolem.ArmorReturnSome".Localized());
                }

                if (StatusEffectValue(StatusEffectIds.VolcanicArmorRed, 0) < 0)
                {
                    context.Write("Combat.VolcanicGolem.ArmorReturnAll".Localized());

                    RemoveStatusEffect(StatusEffectIds.VolcanicArmorRed);
                }
            }

            if (HasStatusEffect(StatusEffectIds.VolcanicFrenzy))
            {
                AddStatusValue(StatusEffectIds.VolcanicFrenzy, 0, -1);

                if (StatusEffectValue(StatusEffectIds.VolcanicFrenzy, 0) < 0)
                {
                    RemoveStatusEffect(StatusEffectIds.VolcanicFrenzy);

                    context.Write("Combat.VolcanicGolem.FrenzyCalm".Localized());

                    WeaponAttack = 120;
                }
            }

            if (HasStatusEffect(StatusEffectIds.VolcanicFistProblem))
            {
                if (HasPerk(PerkIds.StunImmune))
                    RemovePerk(PerkIds.StunImmune);

                AddStatusValue(StatusEffectIds.VolcanicFistProblem, 0, -1);
            }
            else
            {
                RemoveStatusEffect(StatusEffectIds.VolcanicFistProblem);

                if (!HasPerk(PerkIds.StunImmune))
                    CreatePerk(PerkIds.StunImmune, 0, 0, 0, 0);
            }

            if (HasStatusEffect(StatusEffectIds.Stunned))
            {
                context.Write("Combat.VolcanicGolem.StunnedWait".Localized());

                ArmorDefense = 0;

                if (StatusEffectValue(StatusEffectIds.Stunned, 0) < 0)
                {
                    RemoveStatusEffect(StatusEffectIds.Stunned);

                    context.Write("Combat.VolcanicGolem.StunnedEnd".Localized());

                    CreateStatusEffect(StatusEffectIds.VolcanicFrenzy, 3, 0, 0, 0);

                    WeaponAttack = 400;

                    ArmorDefense = 300;
                }
                else
                    AddStatusValue(StatusEffectIds.Stunned, 0, -1);

                return;
            }

            if (ArmorDefense < 0)
                ArmorDefense = 0;
        }

        public void Reinforce(CombatContext context)
        {
            context.Write("Combat.VolcanicGolem.Reinforce".Localized());

            ArmorDefense = 300;
        }

        public void Nuke(CombatContext context)
        {
            var player = context.Player;

            if (!HasStatusEffect(StatusEffectIds.Uber))
            {
                context.Write("Combat.VolcanicGolem.UberCharge".Localized());

                CreateStatusEffect(StatusEffectIds.Uber, 0, 0, 0, 0);

                return;
            }

            var stage = (int)StatusEffectValue(StatusEffectIds.Uber, 0);

            if (stage == 0)
            {
                AddStatusValue(StatusEffectIds.Uber, 0, 1);

                context.Write("Combat.VolcanicGolem.Uber1".Localized());

                if (player.Intelligence > 50)
                    context.Write("Combat.VolcanicGolem.UberHint1".Localized());

                return;
            }

            if (stage == 1)
            {
                AddStatusValue(StatusEffectIds.Uber, 0, 1);

                context.Write("Combat.VolcanicGolem.Uber2".Localized());

                if (player.Intelligence > 50)
                    context.Write("Combat.VolcanicGolem.UberHint2".Localized());

                return;
            }

            context.Write("Combat.VolcanicGolem.Nuke1".Localized());

            context.Write("Combat.VolcanicGolem.Nuke2".Localized());

            context.Write("Combat.VolcanicGolem.Nuke3".Localized());

            player.HealthChange(-9999);
        }

        public void Heal(CombatContext context)
        {
            if (!HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
            {
                context.Write("Combat.VolcanicGolem.HealStart".Localized());

                CreateStatusEffect(StatusEffectIds.VolcanicUberHeal, 2, 0, 0, 0);

                CoC.Instance.Flags.Set(GameFlagIds.Volcanicgolemshieldhp, 100);

                return;
            }

            if (StatusEffectValue(StatusEffectIds.VolcanicUberHeal, 0) != 0)
            {
                AddStatusValue(StatusEffectIds.VolcanicUberHeal, 0, -1);

                context.Write("Combat.VolcanicGolem.HealContinue".Localized());

                if (context.Player.Intelligence > 70)
                    context.Write("Combat.VolcanicGolem.HealHint".Localized());

                return;
            }

            RemoveStatusEffect(StatusEffectIds.VolcanicUberHeal);

            context.Write("Combat.VolcanicGolem.HealDone".Localized());

            HealthChange(2000);

            var max = CoC.Instance.Flags.Get(GameFlagIds.Volcanicgolemhp);

            if (Health > max)
                Health = max;
        }

        public void Earthshatter(CombatContext context)
        {
            var player = context.Player;

            context.Write("Combat.VolcanicGolem.ShatterIntro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                DoDodge = false,

                DoParry = false,

                DoBlock = true,

                Attacker = this,

                Defender = player,
            });

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Write("Combat.VolcanicGolem.ShatterBlock".Localized());

                return;
            }

            if (player.Stun(1, 50))
            {
                context.Write("Combat.VolcanicGolem.ShatterStun".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, (int)(Strength / 2) + Random.Shared.Next(10));

            if (damage < 10)
                damage = 10;

            context.Write("Combat.VolcanicGolem.ShatterHit".Localized());

            player.HealthChange(-damage);
        }

        public override float ModifyIncomingDamage(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (HasStatusEffect(StatusEffectIds.Uber) && damage >= 150)
            {
                context.Write("Combat.VolcanicGolem.ChargeBreak".Localized());

                ArmorDefense = 0;

                CreateStatusEffect(StatusEffectIds.Stunned, 1, 0, 0, 0);

                RemoveStatusEffect(StatusEffectIds.Uber);
            }

            if (HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
            {
                if (damage < 100)
                {
                    context.Write("Combat.VolcanicGolem.ShieldAbsorb".Localized());

                    var shield = CoC.Instance.Flags.Get(GameFlagIds.Volcanicgolemshieldhp) - damage * 0.3f;

                    CoC.Instance.Flags.Set(GameFlagIds.Volcanicgolemshieldhp, shield);

                    if (shield <= 0)
                    {
                        context.Write("Combat.VolcanicGolem.ShieldBreak".Localized());

                        ArmorDefense = 0;

                        RemoveStatusEffect(StatusEffectIds.VolcanicUberHeal);
                    }

                    return 1;
                }

                context.Write("Combat.VolcanicGolem.ShieldShatter".Localized());

                ArmorDefense = 0;

                RemoveStatusEffect(StatusEffectIds.VolcanicUberHeal);

                CreateStatusEffect(StatusEffectIds.Stunned, 2, 0, 0, 0);

                return 1;
            }

            return base.ModifyIncomingDamage(context, damage, kind);
        }

        private void DistanceAnger(CombatContext context)
        {
            CombatRangeData.CloseDistance(this);

            var player = context.Player;

            if (!HasStatusEffect(StatusEffectIds.Uber) && !HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
            {
                if ((player.Speed < 70 && Random.Shared.Next(4) == 0) || (player.Speed >= 70 && Random.Shared.Next(2) == 0))
                {
                    context.Write("Combat.VolcanicGolem.FistStuck".Localized());

                    context.Write("Combat.VolcanicGolem.FistStuckHint".Localized());

                    CreateStatusEffect(StatusEffectIds.VolcanicFistProblem, 2, 0, 0, 0);
                }
                else
                    ThrowRock(context);
            }
            else
            {
                context.Write("Combat.VolcanicGolem.ChargingRetreat".Localized());

                if (!HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
                    Nuke(context);
                else
                    Heal(context);
            }
        }

        private void ThrowRock(CombatContext context)
        {
            var player = context.Player;

            context.Write("Combat.VolcanicGolem.ThrowRock".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
                context.Write("Combat.VolcanicGolem.ThrowDodge".Localized());
            else
            {
                context.Write("Combat.VolcanicGolem.ThrowHit".Localized());

                var damage = CombatCalculator.ReduceDamage(context, player, Strength + 200);

                player.HealthChange(-damage);
            }

            context.Write("Combat.VolcanicGolem.ArmorDecreased".Localized());

            if (HasStatusEffect(StatusEffectIds.VolcanicArmorRed))
            {
                AddStatusValue(StatusEffectIds.VolcanicArmorRed, 0, 3);

                AddStatusValue(StatusEffectIds.VolcanicArmorRed, 1, 1);
            }
            else
                CreateStatusEffect(StatusEffectIds.VolcanicArmorRed, 3, 1, 0, 0);

            ArmorDefense = Math.Max(0, ArmorDefense - 150);
        }

        public override string? RunCheck(CombatContext context)
        {
            if (HasStatusEffect(StatusEffectIds.Uber) || HasStatusEffect(StatusEffectIds.VolcanicUberHeal))
                return "Combat.VolcanicGolem.RunAway".Localized();

            return base.RunCheck(context);
        }

        public override bool GuaranteesEscape(CombatContext context) =>

            HasStatusEffect(StatusEffectIds.Uber) || HasStatusEffect(StatusEffectIds.VolcanicUberHeal);

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.VolcanicGolemScene>.Get().WinAgainstGolem();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.Volcanicgolemhp, (int)(Health - 50));

            var scene = Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.VolcanicGolemScene>.Get();

            if (HasStatusEffect(StatusEffectIds.Infested))
                Scenes.Scene.OutputText("Combat.Monster.CameWormsIndifferent".Localized());

            scene.LoseToGolem();

            return true;
        }
    }
}