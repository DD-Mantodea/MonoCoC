using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Mutations
{
    /// <summary>
    /// 原版 Mutations.hx:828 demonChanges(tfSource) —— 恶魔化过程中的身体部件变化。
    /// 拆到独立文件只为可读性；状态（Changes/ChangeLimit）与主文件共享（partial）。
    /// </summary>
    public partial class Mutations
    {
        /// <summary>原版 Mutations.hx:828 demonChanges()。</summary>
        public virtual void DemonChanges(string tfSource)
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var tail = player.Body.Get<BodyParts.Vanilla.TailPart>();

            var horns = player.Body.Get<BodyParts.Vanilla.HornsPart>();

            var face = player.Body.Get<BodyParts.Vanilla.FacePart>();

            var tongue = player.Body.Get<BodyParts.Vanilla.TonguePart>();

            var lowerBody = player.Body.Get<BodyParts.Vanilla.LowerBodyPart>();

            var wings = player.Body.Get<BodyParts.Vanilla.WingsPart>();

            var rearBody = player.Body.Get<BodyParts.Vanilla.RearBodyPart>();

            // 原版 :832 已有角则把尾巴换成恶魔尾
            if (tail.Id != BodyPartTypes.Demonic && horns.Value > 0)
            {
                if (tail.Id != BodyPartTypes.None)
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText(
                        tail.Id == BodyPartTypes.SpiderAbdomen || tail.Id == BodyPartTypes.BeeAbdomen

                            ? "Scenes.Mutations.DemonChanges.TailFromInsect".Localized()

                            : "Scenes.Mutations.DemonChanges.TailFromOther".Localized());

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.TailDemon".Localized());
                }
                else
                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.TailGrow".Localized());

                player.Corruption += 4f;

                tail.Id = BodyPartTypes.Demonic;

                Changes += 1;
            }

            // 原版 :849 长角
            if (horns.Value == 0 || Random.Shared.Next((int)horns.Value + 3) == 0)
            {
                if (horns.Value < 12
                    && (horns.Id == BodyPartTypes.None || horns.Id == BodyPartTypes.Demonic))
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText(horns.Value == 0

                        ? "Scenes.Mutations.DemonChanges.HornsFirst".Localized()

                        : "Scenes.Mutations.DemonChanges.HornsSecond".Localized());

                    if (horns.Id == BodyPartTypes.None)
                        horns.Id = BodyPartTypes.Demonic;

                    horns.Value += 1;

                    horns.Value += 1;

                    player.Corruption += 3f;
                }
                else if (horns.Id != BodyPartTypes.Demonic && horns.Id != BodyPartTypes.None)
                {
                    // 原版 :865 其他角型 → 缩成一对小恶魔角
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.HornsShift".Localized());

                    horns.Value = 2;

                    horns.Id = BodyPartTypes.Demonic;

                    player.Corruption += 3f;
                }

                Changes += 1;
            }

            // 原版 :875 黑乳头恢复
            if (player.HasStatusEffect(StatusEffectIds.BlackNipples) && Random.Shared.Next(3) == 0)
                RemoveBlackNipples(tfSource);

            // 原版 :879 先恢复脸，再去毛/鳞
            if ((face.Id != BodyPartTypes.Human || !player.HasPlainSkin()) && Random.Shared.Next(3) == 0)
            {
                if (face.Id != BodyPartTypes.Human)
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.FaceHuman".Localized());

                    face.Id = BodyPartTypes.Human;
                }
                else if (!player.HasPlainSkin())
                {
                    if (player.HasFur()) RemoveFur();
                    if (player.HasScales()) RemoveScales();
                }

                Changes += 1;
            }

            // 原版 :894 蛇舌 → 恶魔舌
            if (tongue.Id == BodyPartTypes.Snake && Random.Shared.Next(3) == 0)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.DemonTongue".Localized());

                tongue.Id = BodyPartTypes.Demonic;

                Changes += 1;
            }

            // 原版 :900 无毛皮肤时改造脚（男性/无性别→爪，女性/扶她→高跟）
            if (player.HasPlainSkin() && Random.Shared.Next(4) == 0)
            {
                if (player.Gender == Creatures.Gender.Male || player.Gender == Creatures.Gender.Genderless
                    || (player.Gender == Creatures.Gender.Hermaphrodite && player.Mf("m", "f") == "m"))
                {
                    if (lowerBody.Id != BodyPartTypes.DemonicClaws)
                    {
                        Scenes.Scene.OutputText("[pg]");

                        Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.FeetClaws".Localized());

                        lowerBody.Id = BodyPartTypes.DemonicClaws;

                        lowerBody.LegCount = 2;
                    }
                }
                else if (lowerBody.Id != BodyPartTypes.DemonicHighHeels)
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.FeetHeels".Localized());

                    lowerBody.Id = BodyPartTypes.DemonicHighHeels;

                    lowerBody.LegCount = 2;
                }

                Changes += 1;
            }

            // 原版 :920 长恶魔翼
            if ((wings.Id != BodyPartTypes.BatLikeLarge || rearBody.Id == BodyPartTypes.SharkFin)
                && Random.Shared.Next(8) == 0

                && player.IsCorruptEnough(50))
            {
                if (wings.Id == BodyPartTypes.BatLikeTiny
                    && player.Corruption >= 75 - player.CorruptionTolerance())
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsGrowLarge".Localized());

                    wings.Id = BodyPartTypes.BatLikeLarge;
                }
                else if (rearBody.Id == BodyPartTypes.SharkFin)
                {
                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsFromFin".Localized());

                    rearBody.Restore();

                    wings.Id = BodyPartTypes.BatLikeTiny;
                }
                else if (wings.Id == BodyPartTypes.None)
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsGrowTiny".Localized());

                    wings.Id = BodyPartTypes.BatLikeTiny;
                }
                else
                {
                    Scenes.Scene.OutputText("[pg]");

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsChange".Localized());

                    if (wings.Id == BodyPartTypes.BeeLikeSmall
                        || wings.Id == BodyPartTypes.Harpy

                        || wings.Id == BodyPartTypes.Imp

                        || wings.Id == BodyPartTypes.DraconicSmall

                        || wings.Id == BodyPartTypes.Wooden)
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsSmall".Localized());

                        wings.Id = BodyPartTypes.BatLikeTiny;
                    }
                    else
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsLarge".Localized());

                        wings.Id = BodyPartTypes.BatLikeLarge;
                    }

                    Scenes.Scene.OutputText("Scenes.Mutations.DemonChanges.WingsBatlike".Localized());
                }

                Changes += 1;
            }
        }
    }
}