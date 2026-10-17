using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    /// <summary>
    /// 原版 SandTrap.hx（218 行）的完整移植：沙坑层数状态机 + 三个专属动作 + 三个反应钩子。
    /// </summary>
    [Register<SandTrapEnemy>("coc:sand_trap")]
    public class SandTrapEnemy : Enemy
    {
        /// <summary>原版 SandTrap.hx:10 playerClimbed —— 本回合玩家是否成功攀爬。</summary>
        public bool PlayerClimbed;

        /// <summary>原版 SandTrap.hx:34 trapLevel()：读取/调整沙坑层数（1-4，初值 4）。</summary>
        public virtual float TrapLevel(float adjustment = 0f)
        {
            if (!HasStatusEffect(StatusEffectIds.Level))
                CreateStatusEffect(StatusEffectIds.Level, 4);

            if (adjustment != 0f)
            {
                AddStatusValue(StatusEffectIds.Level, 0, adjustment);

                var value = StatusEffectValue(StatusEffectIds.Level, 0);

                // 原版 :41-47「Keep in bounds ya lummox」
                if (value < 1f)
                    ChangeStatusValue(StatusEffectIds.Level, 0, 1f - value);
                else if (value > 4f)
                    ChangeStatusValue(StatusEffectIds.Level, 0, 4f - value);
            }

            return StatusEffectValue(StatusEffectIds.Level, 0);
        }

        /// <summary>原版 SandTrap.hx:12 sandTrapWait()：玩家「等待」时尝试爬出沙坑。</summary>
        public virtual void SandTrapWait(CombatContext context)
        {
            var player = context.Player;

            PlayerClimbed = true;

            context.WriteAction("Combat.SandTrap.WaitIntro".Localized());

            if (TrapLevel() == 4)
            {
                context.Append("Combat.SandTrap.WaitTop".Localized());

                return;
            }

            // 原版 :23：力量检定成功则上升两层
            if (player.Strength / 10f + Random.Shared.Next(20) > 10)
            {
                context.Append("Combat.SandTrap.ClimbSuccess".Localized());

                TrapLevel(2);
            }
            else
            {
                context.Append("Combat.SandTrap.ClimbFail".Localized());

                TrapLevel(1);
            }
        }

        /// <summary>原版 SandTrap.hx:52 sandTrapPheromones()</summary>
        public virtual void SandTrapPheromones(CombatContext context)
        {
            var player = context.Player;

            context.WriteAction("Combat.SandTrap.PheromonesIntro".Localized());

            // 原版 :55：速度检定 或 闪避成功
            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                DoDodge = true,

                DoParry = false,

                DoBlock = false,
            });

            if (player.Speed / 10f + Random.Shared.Next(20) > 10 || avoid.AttackFailed)
            {
                context.Append("Combat.SandTrap.PheromonesAvoid".Localized());

                return;
            }

            context.Append("Combat.SandTrap.PheromonesHit".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + (int)(10 + player.Libido / 10f));
        }

        /// <summary>原版 SandTrap.hx:69 nestleQuikSandAttack()</summary>
        public virtual void NestleQuikSandAttack(CombatContext context)
        {
            var player = context.Player;

            context.WriteAction("Combat.SandTrap.QuiksandIntro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                DoDodge = true,

                DoParry = false,

                DoBlock = false,
            });

            if (player.Speed / 10f + Random.Shared.Next(20) > 10 || avoid.AttackFailed)
            {
                context.Append("Combat.SandTrap.QuiksandAvoid".Localized());

                return;
            }

            context.Append("Combat.SandTrap.QuiksandHit".Localized());

            if (player.CanFly())
                context.Append("Combat.SandTrap.QuiksandWings".Localized());

            TrapLevel(-1);

            PlayerClimbed = true;
        }

        /// <summary>原版 SandTrap.hx:91 react()：三个战斗反应钩子。</summary>
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            switch (reaction)
            {
                // 原版 :99：陷在沙坑里时受到的伤害 ×1.75
                case CombatReactionContext.WhenAttacked:
                    if (HasStatusEffect(StatusEffectIds.Level))
                        context.EnemyDamageTakenMultiplier *= 1.75f;

                    break;

                // 原版 :102：未使用远程武器且非首击时，玩家会冲下斜坡猛击（直接打到底层）
                case CombatReactionContext.BeforeAttacked:
                    if (HasStatusEffect(StatusEffectIds.Level)
                        && !player.HasStatusEffect(StatusEffectIds.FirstAttack)

                        && !context.Player.Weapon.IsRanged())                    {
                        context.WriteAction("Combat.SandTrap.ChargeDown".Localized());

                        TrapLevel(-4);
                    }

                    break;

                // 原版 :112：等待时由 sandTrapWait 接管
                case CombatReactionContext.PlayerWaited:
                    if (HasStatusEffect(StatusEffectIds.Level))
                    {
                        SandTrapWait(context);

                        return false;
                    }

                    break;
            }

            return true;
        }

        /// <summary>原版 SandTrap.hx:118 performCombatAction()</summary>
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (!HasStatusEffect(StatusEffectIds.Level))
                return false;

            if (TrapLevel() == 4 && !PlayerClimbed)
                NestleQuikSandAttack(context);
            else
                SandTrapPheromones(context);

            // 原版 :126：本回合没成功攀爬就继续下沉一层
            if (!PlayerClimbed)
            {
                context.Append("Combat.SandTrap.Sinking".Localized());

                TrapLevel(-1);
            }

            PlayerClimbed = false;

            return true;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(10, 2, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 4;

            CreateBreastRow(0, 0);

            Tallness = Random.Shared.Next(8) + 150;

            Body.Get<SkinPart>().Tone = "fair";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "claws";

            ArmorName = "chitin";

            // 原版 SandTrap.hx:174：出生即带 Fertilized 状态
            CreateStatusEffect(StatusEffectIds.Fertilized, 0, 0, 0, 0);
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.SandTrapScene>.Get().PcBeatsATrap();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.SandTrap.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.SandTrapScene>.Get().SandtrapmentLoss(true);

            return true;
        }
    }
}