using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 SharkMan extends Monster（src/classes/scenes/places/swim/SharkMan.hx）
    // 只移植类本体；defeated/won 依赖 SharkManScene（未移植，原版 SharkMan.hx:13-21）。
    [Register("coc:shark_man")]
    public class SharkManEnemy : Enemy
    {
        // 原版 isCharging（SharkMan.hx:11）：chargePt1 置真，chargePt2 解除，期间 AI 只能冲锋。
        private bool _isCharging;

        private static readonly Identifier AttackId = new("coc", "shark_man_attack");
        private static readonly Identifier ThrustGrabId = new("coc", "shark_man_thrust_grab");
        private static readonly Identifier ThrustFuckId = new("coc", "shark_man_thrust_fuck");
        private static readonly Identifier ChargePt1Id = new("coc", "shark_man_charge_pt1");
        private static readonly Identifier ChargePt2Id = new("coc", "shark_man_charge_pt2");

        public bool IsCharging
        {
            get => _isCharging;
            set => _isCharging = value;
        }

        // 原版 performCombatAction（SharkMan.hx:75-92）：
        //   蓄力中只有 chargePt2；
        //   否则 eAttack 2，玩家被缠住时 thrustFuck 5，否则 thrustGrab 2 + chargePt1 1。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (_isCharging)
                return new CombatAiAction { Ability = ChargePt2Id };

            var grappled = context.Player.HasStatusEffect(VanillaStatusEffects.Grappled);

            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 2f) };

            if (grappled)
            {
                choices.Add((ThrustFuckId, 5f));
            }
            else
            {
                choices.Add((ThrustGrabId, 2f));
                choices.Add((ChargePt1Id, 1f));
            }

            var total = 0f;

            foreach (var (_, weight) in choices)
                total += weight;

            var roll = (float)context.Random.NextDouble() * total;

            foreach (var (id, weight) in choices)
            {
                roll -= weight;

                if (roll < 0)
                    return new CombatAiAction { Ability = id };
            }

            return new CombatAiAction { Ability = choices[^1].Id };
        }

        // 原版 struggle（SharkMan.hx:24-36）：1/3 或 str×1.25% 成功脱身并反伤鲨鱼人。
        public override void Struggle(CombatContext context) => SharkManCombatHandlers.Struggle(context);

        // 原版 outputAttack（SharkMan.hx:94-104）：整段替换成专属战报（链子 / 拳头）。
        public override void OutputAttack(CombatContext context, float damage)
        {
            context.Append((IsUnarmed()
                ? "Combat.SharkMan.OutputAttack.TheSharkMan2"
                : "Combat.SharkMan.OutputAttack.TheSharkMan").Localized());

            if (CoC.Instance.Flags.Get(MonoCoC.Contents.Flags.VanillaFlags.EnemyCritical) > 0)
                context.Append("Combat.SharkMan.OutputAttack.Text".Localized());

            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createCock(13, 2) ×2
            CreateCock(13, 2);
            CreateCock(13, 2);

            // 原版 ass.analLooseness = LOOSENESS_TIGHT(1); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;
            anal.Wetness = 0;

            // 原版 tallness = 72
            Tallness = 72;

            // 原版 createBreastRow(Appearance.breastCupInverse("flat")) → 0
            CreateBreastRow(0);

            // 原版 hips.rating = RATING_BOYISH(0); butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            // 原版 skin.tone = "gray"; hair.color = "silver"; hair.length = 16
            Body.Get<SkinPart>().Tone = "gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "silver";
            hair.Length = 16;

            // 原版 weaponName = "broken chain"; weaponVerb = "slash"; weaponAttack = 10
            WeaponName = "broken chain";
            WeaponVerb = "slash";
            WeaponAttack = 10;

            // 原版 armorName = "tough skin"; armorDef = 5
            ArmorName = "tough skin";
            ArmorDefense = 5;

            // 原版 lustVuln = .30
            LustVulnerability = 0.3f;

            // 原版 gems = Utils.rand(15) + 5：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(15) + 5;

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = NO_DROP（SharkMan.hx:143），所以数据表 loot 留空。
            // 原版 onPcRunSuccess（SharkMan.hx:106-108）设置场景 saveContent.hasLost：已接在下方 RunSuccess()。
            //
            // 原版 defeated()：SharkMan.hx:13-15（swim.sharkManScene.sharkManLostCombat）
            // 原版 won()：SharkMan.hx:19-21（swim.sharkManScene.sharkManWonCombat）
        }

        // 原版 defeated()：SharkMan.hx:13-15
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.SharkManScene>.Get().SharkManLostCombat(hpVictory);

            return true;
        }

        // 原版 won()：SharkMan.hx:19-21
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.SharkManScene>.Get().SharkManWonCombat(hpVictory);

            return true;
        }

        // 原版 onPcRunSuccess（SharkMan.hx:106-108）：逃跑成功会把 SharkManScene.saveContent.hasLost 置位。
        public override void RunSuccess(CombatContext context) =>
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.SharkManScene>.Get().MarkPlayerEscaped();
    }
}
