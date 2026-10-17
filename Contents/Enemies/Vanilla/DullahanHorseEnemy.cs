using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<DullahanHorseEnemy>("coc:dullahan_horse")]
    public class DullahanHorseEnemy : Enemy
    {
        public bool _charging;

        public int _couchedLanceSpeedBonus;

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

            WeaponName = "rapier";

            WeaponVerb = "lunge";

            WeaponAttack = 14;

            ArmorName = "black and gold armor";

            ArmorDefense = 17;

            Lust = 25 + Random.Shared.Next(15);

            Gems = 30;

            LustVulnerability = 0;

            Distance = CombatDistance.Distant;

            ExtraDistance = 2;

            CreatePerk(PerkIds.Immovable);
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            KnightCharge(context);

            return true;
        }

        public void KnightCharge(CombatContext context)
        {
            if (!_charging)
            {
                context.WriteAction("Combat.DullahanHorse.Charge.Begin".Localized());

                _couchedLanceSpeedBonus += 1;

                _charging = true;

                return;
            }

            switch (ExtraDistance)
            {
                case 2:
                    context.WriteAction("Combat.DullahanHorse.Charge.Step2".Localized());

                    _couchedLanceSpeedBonus += 1;

                    CombatRangeData.CloseDistance(this);

                    return;

                case 1:
                    context.WriteAction("Combat.DullahanHorse.Charge.Step1".Localized());

                    _couchedLanceSpeedBonus += 1;

                    CombatRangeData.CloseDistance(this);

                    if (context.Player.Intelligence > 50)
                        context.WriteAction("Combat.DullahanHorse.Charge.Warn".Localized());

                    return;

                default:
                    ResolveCharge(context);

                    return;
            }
        }

        public void ResolveCharge(CombatContext context)
        {
            var player = context.Player;

            var waited = (CoC.Instance?.Flags.Get(GameFlagIds.InCombatUsePlayerWaitedFlag) ?? 0) != 0;

            if (waited)
            {
                context.WriteAction("Combat.DullahanHorse.Charge.Waited".Localized());

                player.HealthChange(-(10 + context.Random.Next(10) * _couchedLanceSpeedBonus));

                _charging = false;

                MoveDistant(context, 2);

                return;
            }

            context.WriteAction("Combat.DullahanHorse.Charge.Swing".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append(ReactionText(avoid));

                MoveDistant(context, 2);

                return;
            }

            context.Append("Combat.DullahanHorse.Charge.Hit".Localized());

            var damage = Strength * 3 + context.Random.Next(50) + _couchedLanceSpeedBonus * 30;

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            player.HealthChange(-damage);

            MoveDistant(context, 2);
        }

        public static string ReactionText(AvoidResult avoid) => avoid.DodgeReason switch
        {
            "Blind" => "Combat.DullahanHorse.Charge.Blind".Localized(),

            "Speed" => "Combat.DullahanHorse.Charge.Speed".Localized(),

            "Evade" => "Combat.DullahanHorse.Charge.Evade".Localized(),

            "Misdirection" => "Combat.DullahanHorse.Charge.Misdirection".Localized(),

            "Flexibility" => "Combat.DullahanHorse.Charge.Flexibility".Localized(),

            _ => "Combat.DullahanHorse.Charge.Unhandled".Localized()
        };

        public void MoveDistant(CombatContext context, int extra)
        {
            var inRange = ExtraDistance == 0 ||

                context.Player.Weapon.IsRanged() || context.Player.Weapon.IsChanneling(context.Player);

            if (inRange)
                CombatRangeData.MoveMonsterDistant(this);
            else
                CombatRangeData.MoveDistantSafe(this);

            ExtraDistance += extra;
        }

        public override bool HandleStun(CombatContext context)
        {
            RemoveStatusEffect(StatusEffectIds.Uber);

            context.WriteAction("Combat.DullahanHorse.Stun".Localized());

            MoveDistant(context, 2 - ExtraDistance);

            return base.HandleStun(context);
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.DullahanScene>.Get().DullahanPt2();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.DullahanScene>.Get().DullahanVictory();

            return true;
        }
    }
}