using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/RhinoBouncer.hx（extends Monster）
    // Demonfist 的犀牛保镖：远距离起手冲锋，之后每回合抓取 + 头槌/断背/倒栽葱。
    [Register("coc:rhino_bouncer")]
    public class RhinoBouncerEnemy : Enemy
    {
        // 原版 public var charging:Bool = false（RhinoBouncer.hx:23）
        public bool Charging;

        // 原版 statusEffects.combat.BackbrokenDebuff（BackbrokenDebuff.hx:7-25）：
        // C# 的 VanillaStatusEffects 没有登记，按原版 key 建；3 回合 Spe-25 + cripple。
        public static readonly Identifier BackbrokenStatus = new("coc", "backbroken");

        // 原版 BackbrokenDebuff.onCombatRound（BackbrokenDebuff.hx:19-22）：
        //   host.cripple(); super.onCombatRound()（countdownTimer：递减、到期出 removeString、否则出 updateString）
        // TickStatuses 是这批新接入的敌人回合开始钩子（Enemy.cs:322 / CombatController.cs:813）。
        public override void TickStatuses(CombatContext context)
        {
            var player = context.Player;
            var status = player.StatusEffectByType(BackbrokenStatus);

            if (status == null)
                return;

            if (status.Values[0] <= 0)
            {
                player.RemoveStatusEffect(BackbrokenStatus);
                player.IsCrippled = false;

                context.Append("Combat.RhinoBouncer.Backbroken.Recovered".Localized());

                return;
            }

            player.IsCrippled = true;

            // 原版 cripple()（BackbrokenDebuff.hx:20）禁用物理技：C# 由 Assets/Data/Effects/Backbroken.hjson
            // 的 flags: ["prevents_physical"] + CombatStatusHooks.BeforePlayerAbilityUse（CombatStatusTicker.cs）
            // 真正拦截；IsCrippled 只是同步保留（Creature.cs:752），供以后读取该字段的代码使用。
            status.Values[0] -= 1;

            if (status.Values[0] <= 0)
            {
                player.RemoveStatusEffect(BackbrokenStatus);
                player.IsCrippled = false;

                context.Append("Combat.RhinoBouncer.Backbroken.Recovered".Localized());
            }
            else
            {
                context.Append("Combat.RhinoBouncer.Backbroken.StillCrippled".Localized());
            }
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new() 逐项照抄（RhinoBouncer.hx:103-140）──
            // createCock(Utils.rand(2) + 15, 2.5, CockTypesEnum.HUMAN)
            CreateCock(Random.Shared.Next(2) + 15, 2.5f, VanillaBodyPartTypes.Human);

            Balls = 2;
            BallSize = 3;

            CreateBreastRow(0);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            // tallness = 7 * 12
            Tallness = 7 * 12;

            // hips.rating = RATING_BOYISH(0) / butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "gray";

            var hair = Body.Get<HairPart>();
            hair.Color = "none";
            hair.Length = 0;

            // horns.type = Horns.RHINO（RhinoBouncer.hx:126）
            Body.Get<HornsPart>().Id = VanillaBodyPartTypes.Rhino;

            // 原版 level = 20 / lust = 5 / 属性在数据表里。
            WeaponName = "fists";
            WeaponVerb = "haymaker";
            WeaponAttack = 0;

            ArmorName = "thick hide";
            ArmorDefense = 35;

            // 原版 gems = Utils.rand(5) + 250
            Gems = Random.Shared.Next(5) + 250;

            // 原版 drop = WeightedDrop(CCUPCAK 2)：见 RhinoBouncer.hjson loot。
        }

        // 原版 defeated() / won() 都是空实现（RhinoBouncer.hx:9-13），不需要接场景。
    }
}
