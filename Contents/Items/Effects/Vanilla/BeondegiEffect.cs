using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/Beondegi.hx:14-71 —— 蚕蛹（Willows 树精的转化食物）。
    /// 蝙蝠耳/恶魔翼/蝙蝠腿/蝙蝠脸；战斗中对豆荚使用可直接结束战斗。
    /// </summary>
    public class BeondegiEffect : ConsumableEffect
    {
        // 原版 Beondegi.hx:20
        private const string TfSource = "Beondegi";

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var mutations = Mutations.Mutations.Instance;

            // 原版 :21 dynStats(Cor(3))
            player.Corruption = Math.Clamp(player.Corruption + 3, 0, 100);

            context.Write("Items.Beondegi.Use".Localized());

            // 原版 :24 game.inCombat && monster.short == "pod"
            // C# 侧没有 monster.short，用敌人注册 id 判定同一个豆荚敌人
            if (CombatController.InCombat
                && CombatController.Instance?.Context.Enemy is { } enemy

                && enemy.Id == EnemyIds.EncapsulationPod)
            {
                context.Write("Items.Beondegi.Pod".Localized());

                enemy.IsCrippled = true;

                return;
            }

            // 原版 :30 mutations.initTransformation([1, 2, 3])
            mutations.InitTransformation([1, 2, 3]);

            // 原版 :33 掉毛
            if (mutations.Changes < mutations.ChangeLimit && player.HasFur() && random.Next(5) == 0)
                mutations.RemoveFur();

            // 原版 :35 体脂
            if (random.Next(3) == 0)
                context.Write(player.ModThickness(0, 5));

            // 原版 :39 蝙蝠耳
            var ears = player.Body.Get<EarsPart>();

            if (mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0 && ears.Id != BodyPartTypes.Bat)
            {
                context.Write("Items.Beondegi.Ears".Localized());

                ears.Id = BodyPartTypes.Bat;

                mutations.Changes += 1;
            }

            // 原版 :45 大蝙蝠魔翼
            var wings = player.Body.Get<WingsPart>();

            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(3) == 0

                && wings.Id != BodyPartTypes.BatLikeLarge)
            {
                context.Write("Items.Beondegi.Wings".Localized());

                wings.Id = BodyPartTypes.BatLikeLarge;

                mutations.Changes += 1;
            }

            // 原版 :51 腿数还原
            var lowerBody = player.Body.Get<LowerBodyPart>();

            if (mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0 && lowerBody.LegCount != 2)
                mutations.RestoreLegs(TfSource);

            // 原版 :55 蝙蝠腿
            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(3) == 0

                && lowerBody.LegCount == 2

                && lowerBody.Id != BodyPartTypes.Bat)
            {
                lowerBody.Id = BodyPartTypes.Bat;

                context.Write("Items.Beondegi.Legs".Localized());

                mutations.Changes += 1;
            }

            // 原版 :61 蝙蝠脸
            var face = player.Body.Get<FacePart>();

            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(3) == 0

                && mutations.TfNoFur()

                && face.Id != BodyPartTypes.Bat)
            {
                face.Id = BodyPartTypes.Bat;

                context.Write("Items.Beondegi.Face".Localized());

                mutations.Changes += 1;
            }

            // 原版 :67
            player.RefillHunger(20);

            // 原版 :69 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}