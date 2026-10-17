using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    /// <summary>
    /// 原版 Phouka.hx（357 行）的战斗部分完整移植：
    /// 四形态切换（仙灵 / 兔 / 山羊 / 马）+ 物理攻击 + 挑逗攻击 + 泥球沉默。
    /// </summary>
    [Register<PhoukaEnemy>("coc:phouka")]
    public class PhoukaEnemy : Enemy
    {
        // ===== 原版 Phouka.hx:11 phoukaFightAttack()：兔 / 山羊 / 马三形态的物理攻击 =====

        public virtual void PhoukaFightAttack(CombatContext context)
        {
            var player = context.Player;

            // 原版 :13：失明时有 1/3 概率完全打空
            if (HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(3) < 1)
            {
                context.WriteAction("Combat.Phouka.AttackBlindMiss".Localized());

                return;
            }

            float raw;

            string intro, dodged, blocked, hit;

            switch (PhoukaScene.phoukaForm)
            {
                case PhoukaScene.PHOUKA_FORM_BUNNY:
                    raw = 60 + 30 + 10;

                    intro = "Combat.Phouka.BunnyKick".Localized();

                    dodged = "Combat.Phouka.BunnyKickDodged".Localized();

                    blocked = "Combat.Phouka.BunnyKickBlocked".Localized();

                    hit = "Combat.Phouka.BunnyKickHit".Localized();

                    break;

                case PhoukaScene.PHOUKA_FORM_GOAT:
                    raw = 80 + 40 + 10;

                    intro = "Combat.Phouka.GoatCharge".Localized();

                    dodged = "Combat.Phouka.GoatChargeDodged".Localized();

                    blocked = "Combat.Phouka.GoatChargeBlocked".Localized();

                    hit = "Combat.Phouka.GoatChargeHit".Localized();

                    break;

                default: // 马
                    raw = 95 + 55 + 10;

                    intro = "Combat.Phouka.HorseTrample".Localized();

                    dodged = "Combat.Phouka.HorseTrampleDodged".Localized();

                    blocked = "Combat.Phouka.HorseTrampleDodged".Localized();

                    hit = "Combat.Phouka.HorseTrampleHit".Localized();

                    break;
            }

            context.WriteAction(intro);

            var damage = CombatCalculator.ReduceDamage(context, player, raw);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                DoDodge = true,

                DoParry = false,

                DoBlock = false,
            });

            if (avoid.AttackFailed || damage <= 0)
            {
                context.Append(avoid.AttackFailed ? dodged : blocked);

                return;
            }

            context.Append(hit);

            player.HealthChange(-damage);
        }

        // ===== 原版 Phouka.hx:58 phoukaFightLustAttack()：仙灵 / 兔 / 马的挑逗 =====

        public virtual void PhoukaFightLustAttack(CombatContext context)
        {
            var player = context.Player;

            context.WriteAction(PhoukaScene.phoukaForm switch
            {
                PhoukaScene.PHOUKA_FORM_FAERIE => "Combat.Phouka.LustFaerie".Localized(),

                PhoukaScene.PHOUKA_FORM_BUNNY => "Combat.Phouka.LustBunny".Localized(),

                _ => "Combat.Phouka.LustHorse".Localized(),
            });

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                DoDodge = true,

                DoParry = false,

                DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                context.Append(PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_BUNNY

                    ? "Combat.Phouka.LustBunnyDodged".Localized()

                    : "Combat.Phouka.LustDodged".Localized());

                return;
            }

            context.Append(PhoukaScene.phoukaForm switch
            {
                PhoukaScene.PHOUKA_FORM_FAERIE => "Combat.Phouka.LustFaerieHit".Localized(),

                PhoukaScene.PHOUKA_FORM_BUNNY => "Combat.Phouka.LustBunnyHit".Localized(),

                _ => "Combat.Phouka.LustHorseHit".Localized(),
            });

            // 原版 :81：dynStats(Lust(15 + lib/10 + cor/5 + rand(10)))
            player.Lust = Math.Min(player.MaxLust,

                player.Lust + 15 + player.Libido / 10f + player.Corruption / 5f + Random.Shared.Next(10));
        }

        // ===== 原版 Phouka.hx:85 phoukaFightSilence()：泥球沉默（复用蛛网沉默状态）=====

        public virtual void PhoukaFightSilence(CombatContext context)
        {
            var player = context.Player;

            context.WriteAction("Combat.Phouka.SilenceIntro".Localized());

            // 原版 :87：失明时有 2/3 概率打偏
            if (HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(3) < 2)
            {
                context.Append("Combat.Phouka.SilenceBlindMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                DoDodge = true,

                DoParry = false,

                DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                context.Append(avoid.DodgeReason switch
                {
                    "Evade" => "Combat.Phouka.SilenceEvaded".Localized(),

                    "Misdirection" => "Combat.Phouka.SilenceMisdirection".Localized(),

                    "Flexibility" => "Combat.Phouka.SilenceFlexibility".Localized(),

                    _ => "Combat.Phouka.SilenceDodged".Localized(),
                });

                return;
            }

            context.Append("Combat.Phouka.SilenceHit".Localized());

            player.CreateStatusEffect(StatusEffectIds.WebSilence, 0, 0, 0, 0);
        }

        // ===== 原版 Phouka.hx:193-281：四形态切换 =====

        public virtual void TransformToBunny(CombatContext context)
        {
            if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_BUNNY)
                return;

            context.WriteAction(PhoukaScene.phoukaForm switch
            {
                PhoukaScene.PHOUKA_FORM_FAERIE => "Combat.Phouka.ToBunnyFromFaerie".Localized(),

                PhoukaScene.PHOUKA_FORM_GOAT => "Combat.Phouka.ToBunnyFromGoat".Localized(),

                _ => "Combat.Phouka.ToBunnyFromHorse".Localized(),
            });

            ApplyForm(PhoukaScene.PHOUKA_FORM_BUNNY, BodyPartTypes.Human, 10, 1.5f, 60, 60, 90);
        }

        public virtual void TransformToGoat(CombatContext context)
        {
            if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_GOAT)
                return;

            context.WriteAction(PhoukaScene.phoukaForm switch
            {
                PhoukaScene.PHOUKA_FORM_FAERIE => "Combat.Phouka.ToGoatFromFaerie".Localized(),

                PhoukaScene.PHOUKA_FORM_BUNNY => "Combat.Phouka.ToGoatFromBunny".Localized(),

                _ => "Combat.Phouka.ToGoatFromHorse".Localized(),
            });

            ApplyForm(PhoukaScene.PHOUKA_FORM_GOAT, BodyPartTypes.Human, 10, 1.5f, 48, 60, 70);
        }

        public virtual void TransformToHorse(CombatContext context)
        {
            if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_HORSE)
                return;

            context.WriteAction(PhoukaScene.phoukaForm switch
            {
                PhoukaScene.PHOUKA_FORM_FAERIE => "Combat.Phouka.ToHorseFromFaerie".Localized(),

                PhoukaScene.PHOUKA_FORM_BUNNY => "Combat.Phouka.ToHorseFromBunny".Localized(),

                _ => "Combat.Phouka.ToHorseFromGoat".Localized(),
            });

            ApplyForm(PhoukaScene.PHOUKA_FORM_HORSE, BodyPartTypes.Horse, 20, 2.5f, 86, 75, 55);
        }

        public virtual void TransformToFaerie(CombatContext context)
        {
            if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_FAERIE)
                return;

            context.WriteAction(PhoukaScene.phoukaForm switch
            {
                PhoukaScene.PHOUKA_FORM_BUNNY => "Combat.Phouka.ToFaerieFromBunny".Localized(),

                PhoukaScene.PHOUKA_FORM_GOAT => "Combat.Phouka.ToFaerieFromGoat".Localized(),

                _ => "Combat.Phouka.ToFaerieFromHorse".Localized(),
            });

            ApplyForm(PhoukaScene.PHOUKA_FORM_FAERIE, BodyPartTypes.Human, 1, 0.5f, 5, 80, 80);
        }

        /// <summary>原版各 transform 函数共有的数值段：阴茎类型/尺寸、身高、护甲、速度、形态标记。</summary>
        private void ApplyForm(int form, MonoCoC.Registry.Identifier cockType, float length, float thickness,

            float tallness, float armor, float speed)
        {
            if (Cocks.Count > 0)
            {
                Cocks[0].Id = cockType;

                Cocks[0].Length = length;

                Cocks[0].Thickness = thickness;
            }

            Tallness = tallness;

            ArmorDefense = armor;

            Speed = speed;

            PhoukaScene.phoukaForm = form;
        }

        // ===== 原版 Phouka.hx:109 performCombatAction() =====

        public override bool BeforeCombatAction(CombatContext context)
        {
            var player = context.Player;

            var blinded = HasStatusEffect(StatusEffectIds.Blind);

            // 原版 :111：非失明且玩家未被沉默时，1/4 概率变回仙灵并丢泥球
            if (!blinded && !player.HasStatusEffect(StatusEffectIds.WebSilence) && Random.Shared.Next(4) == 0)
            {
                TransformToFaerie(context);

                PhoukaFightSilence(context);

                return true;
            }

            var transformChance = Random.Shared.Next(9); // 原版 :115「2/3 概率保持当前形态」

            if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_FAERIE)
                transformChance = blinded ? Random.Shared.Next(3) : Random.Shared.Next(4);

            switch (transformChance)
            {
                case 0:
                    TransformToBunny(context);

                    break;

                case 1:
                    TransformToGoat(context);

                    break;

                case 2:
                    TransformToHorse(context);

                    break;
            }

            // 原版 :136：按形态决定挑逗或物理攻击
            if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_FAERIE)
                PhoukaFightLustAttack(context);
            else if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_BUNNY && Random.Shared.Next(4) != 0 && !blinded)
                PhoukaFightLustAttack(context);
            else if (PhoukaScene.phoukaForm == PhoukaScene.PHOUKA_FORM_HORSE && Random.Shared.Next(4) == 0 && !blinded)
                PhoukaFightLustAttack(context);
            else
                PhoukaFightAttack(context);

            return true;
        }

        /// <summary>原版 Phouka.hx:151 teased()：被挑逗时的三段反应文本。</summary>
        public override void OnTeased(CombatContext context, float lustDelta)
        {
            if (lustDelta >= 10)
                context.Append("Combat.Phouka.Teased10".Localized());
            else if (lustDelta >= 5)
                context.Append("Combat.Phouka.Teased5".Localized());
            else if (lustDelta > 0)
                context.Append("Combat.Phouka.Teased1".Localized());

            base.OnTeased(context, lustDelta);
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(1, 0.5f, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 1;

            Tallness = 60;

            Body.Get<SkinPart>().Tone = "black";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 1;

            WeaponName = "claws";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<

                PhoukaScene>.Get().PhoukaPlayerWins(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var player = CoC.Instance.Player;

            if (player.HasCock()
                && !player.HasStatusEffect(StatusEffectIds.ParasiteSlug)

                && !player.HasPerk(PerkIds.ParasiteMusk)

                && !player.HasStatusEffect(StatusEffectIds.Infested)

                && Random.Shared.Next(100) < Configuration.OptionStore.Instance.Get("parasites", 0) * 20)

                player.CreateStatusEffect(StatusEffectIds.ParasiteSlug, value1: 72);

            var scene = Scenes.SceneInstance<

                PhoukaScene>.Get();

            if (player.HasVagina())
            {
                if (player.IsTaur() || Random.Shared.Next(2) == 0)
                    scene.PhoukaSexHorse(true, !hpVictory);
                else
                    scene.PhoukaSexBunny(true, !hpVictory);
            }
            else
                scene.PhoukaSexGoat(true, !hpVictory);

            return true;
        }
    }
}