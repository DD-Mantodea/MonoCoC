using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Mutations
{

    /// <summary>
    /// 原版 classes/items/Mutations.hx —— 变形（transformative）系统的共享状态与掷点工具。
    /// 这里先移植不依赖具体身体部件的那部分：changes / changeLimit 计数器、tfChance、initTransformation。
    /// 具体各部位变换（restoreArms / lizardHairChange 等）按需逐步补齐。
    /// </summary>
    public partial class Mutations
    {
        /// <summary>原版 Mutations.hx:34 —— 本次变形已经发生的改动数。</summary>
        public int Changes;

        /// <summary>原版 Mutations.hx:35 —— 单次变形允许的最大改动数。</summary>
        public int ChangeLimit = 1;

        /// <summary>原版 Mutations.hx:39 lizardSkinToneChoices：蜥蜴肤色权重表。</summary>
        public static readonly (string[] Colors, int Weight)[] LizardSkinToneChoices =

        [
            (["purple", "deep pink"], 5),

            (["silver", "light gray"], 5),

            (["red", "orange"], 18),

            (["green", "yellow green"], 18),

            (["white", "light gray"], 18),

            (["blue", "ocean blue"], 18),

            (["black", "dark gray"], 18),
        ];

        private static Mutations? _instance;

        public static Mutations Instance => _instance ??= new Mutations();

        /// <summary>原版 Mutations.hx:55 tfChance()：rand(min(changes+min, max)) == 0。</summary>
        public virtual bool TfChance(int min, int max) =>

            Random.Shared.Next(Math.Min(min + Changes, max)) == 0;

        /// <summary>原版 Mutations.hx:68 initTransformation()</summary>
        public virtual void InitTransformation(int[]? rolls = null, int startChangeLimit = 1, int minChangeLimit = 1)
        {
            var player = CoC.Instance.Player;

            Changes = 0;

            ChangeLimit = startChangeLimit;

            if (player.HasPerk(PerkIds.HistoryAlchemist))
                ChangeLimit += 1;

            // 原版 Player.hx:4682 isTFResistant()
            if (player.HasPerk(PerkIds.TransformationResistance)
                && player.PerkValue(PerkIds.TransformationResistance, 2) == 0)

                ChangeLimit--;

            if (player.HasPerk(PerkIds.LoliliciousBody))
                ChangeLimit--;

            if (rolls is { Length: > 0 } && !player.HasPerk(PerkIds.LoliliciousBody))
                foreach (var roll in rolls)
                    if (Random.Shared.Next(roll) == 0)
                        ChangeLimit += 1;

            if (ChangeLimit < minChangeLimit)
                ChangeLimit = minChangeLimit;

            // 原版：LoliliciousBody 时只有 1/4 概率真的发生改动
            if (player.HasPerk(PerkIds.LoliliciousBody) && Random.Shared.Next(4) > 0)
                ChangeLimit = 0;
        }

        /// <summary>
        /// 原版 Mutations.hx:465 lizardHairChange(tfSource)：蜥蜴类变形对发型的影响。
        /// 返回 -1 = 毛发停止生长，0 = 无变化，1 = 发型已改变。
        /// </summary>
        public virtual int LizardHairChange(string tfSource)
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var hair = player.Body.Get<BodyParts.Vanilla.HairPart>();

            var hairPinId = player.FindKeyItem("Feathery hair-pin");

            switch (tfSource)
            {
                case "reptilum-lizan":
                case "reptilum-dragonewt":
                    // 原版 :470 毛发停止生长（只提示一次）
                    if (flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) == 0)
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourScalpTinglesOddly".Localized());

                        Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourHairHasStopped".Localized());

                        Changes += 1;

                        flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 1);

                        return -1;
                    }

                    return 0;

                case "PlayerEvents-benoitHairPin":
                case "reptilum-basilisk":
                case "reptilum-dracolisk":
                    // 原版 :483 已经是羽冠且未腐化够，保持不动
                    if (hair.Id == BodyPartTypes.BasiliskPlume
                        && player.Corruption < 65)

                        return 0;

                    // 原版 :487 女性/双性 + 低腐化 + 佩戴羽毛发夹 + 蛇怪血统 → 变成羽冠
                    if ((player.Gender == Gender.Female || player.Gender == Gender.Hermaphrodite)
                        && player.Corruption < 15

                        && player.FeatheryHairPinEquipped()

                        && player.IsBasilisk())
                    {
                        if (hair.Id == BodyPartTypes.Goo)
                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourGooeyHairBegins".Localized());

                        // 原版 L495 末尾拼了 Benoit 的性别化措辞（benoitMFText），此处只输出主体文本
                        Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourHeadBeginsTickle".Localized());

                        Scenes.Scene.OutputText(
                            flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) != 0

                                ? "Scenes.Mutations.LizardHairChange.YourHairGrowingAgain".Localized()

                                : "Scenes.Mutations.LizardHairChange.YourHairNowPlume".Localized());

                        flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                        hair.Length = 2;

                        hair.Color = "red";

                        hair.Id = BodyPartTypes.BasiliskPlume;

                        if (hairPinId >= 0)
                            player.CreateKeyItem("Feathery hair-pin", 0, 0, 0, 0);

                        Changes += 1;

                        return 1;
                    }

                    // 原版 :513 高腐化 + 蜥蜴鳞 + 爬虫脸 → 变成蛇怪棘刺
                    if (player.Corruption >= 65
                        && hair.Id != BodyPartTypes.BasiliskSpines

                        && HasLizardScales(player)

                        && HasReptileFace(player))
                    {
                        if (hair.Length > 0
                            && hair.Id != BodyPartTypes.Goo

                            && hair.Id != BodyPartTypes.BasiliskPlume)
                        {
                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourScalpFeelsTight".Localized());

                            if (player.FeatheryHairPinEquipped())
                                Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.ThePinYourHair".Localized());

                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourShockHandfulYour".Localized());
                        }

                        if (hair.Length > 0
                            && hair.Id == BodyPartTypes.BasiliskPlume)
                        {
                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.SuddenSharpPainDrills".Localized());

                            if (player.FeatheryHairPinEquipped())
                                Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.ThePinYourHairHeats".Localized());

                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YouClutchYourHead".Localized());
                        }

                        if (hair.Id == BodyPartTypes.Goo || hair.Length <= 0)
                        {
                            if (hair.Id == BodyPartTypes.Goo)
                                Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourGooeyHairBeginsFall".Localized());

                            if (player.FeatheryHairPinEquipped())
                                Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.ThePinYourHairHeatsThe".Localized());

                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourScalpFeelsTightThough".Localized());
                        }

                        // 原版 :550 收尾：棘刺 + 颜色跟随肤色
                        hair.Color = player.Body.Get<BodyParts.Vanilla.SkinPart>().Tone;

                        hair.Id = BodyPartTypes.BasiliskSpines;

                        hair.Length = 2;

                        flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 1);

                        Changes += 1;

                        Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.WhereYourHairWould".Localized());

                        if (player.FeatheryHairPinEquipped())
                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YouPlaceTheHair".Localized());
                        else if (hairPinId >= 0)
                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.YourThoughtsWanderThe".Localized());

                        if (hairPinId >= 0)
                            player.CreateKeyItem("Feathery hair-pin", 0, 0, 0, 0);

                        if (tfSource == "PlayerEvents-benoitHairPin")
                            Scenes.Scene.OutputText("Scenes.Mutations.LizardHairChange.Line567".Localized());

                        return 1;
                    }

                    return 0;

                default:
                    return 0;
            }
        }

        // 原版 Player.hx hasLizardScales() / hasReptileFace()
        private static bool HasLizardScales(Player player) =>

            player.Body.Get<BodyParts.Vanilla.SkinPart>().Id

                == BodyPartTypes.LizardScales;

        /// <summary>原版 Mutations.hx:102 tfNoFur()：已无毛皮时只有 1/mod 概率允许继续无毛。</summary>
        public virtual bool TfNoFur(int mod = 10)
        {
            var player = CoC.Instance.Player;

            return !(!player.HasFur() && Random.Shared.Next(mod) > 0);
        }

        /// <summary>原版 Mutations.hx:457 newLizardSkinTone()：按权重抽取蜥蜴肤色。</summary>
        public virtual string NewLizardSkinTone() => NewLizardSkinTones()[0];

        /// <summary>原版 newLizardSkinTone() 返回 [主色, 腹面色]；这里补齐成对颜色。</summary>
        public virtual string[] NewLizardSkinTones()
        {
            var total = LizardSkinToneChoices.Sum(choice => choice.Weight);

            var roll = Random.Shared.Next(total);

            foreach (var (colors, weight) in LizardSkinToneChoices)
            {
                if (roll < weight)
                    return [colors[0], colors[1]];

                roll -= weight;
            }

            return [LizardSkinToneChoices[0].Colors[0], LizardSkinToneChoices[0].Colors[1]];
        }

        /// <summary>原版 Mutations.hx:444 removeBlackNipples()。</summary>
        public virtual bool RemoveBlackNipples(string tfSource)
        {
            var player = CoC.Instance.Player;

            if (!player.HasStatusEffect(StatusEffectIds.BlackNipples))
                return false;

            Scenes.Scene.OutputText("Scenes.Mutations.RemoveBlackNipples.NipplesNatural".Localized());

            player.RemoveStatusEffect(StatusEffectIds.BlackNipples);

            Changes += 1;

            return true;
        }

        /// <summary>原版 Mutations.hx:392 removeWings()：移除翅膀，默认同时移除鲨鱼鳍。</summary>
        public virtual bool RemoveWings(string tfSource, bool removeSharkFin = true)
        {
            var player = CoC.Instance.Player;

            var changed = false;

            var rearBody = player.Body.Get<BodyParts.Vanilla.RearBodyPart>();

            if (removeSharkFin && rearBody.Id == BodyPartTypes.SharkFin)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveWings.SharkFinGone".Localized());

                rearBody.Restore();

                changed = true;
            }

            var wings = player.Body.Get<BodyParts.Vanilla.WingsPart>();

            if (wings.Id != BodyPartTypes.None)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveWings.WingsGone".Localized());

                wings.Restore();

                changed = true;
            }

            if (changed)
                Changes += 1;

            return changed;
        }

        /// <summary>原版 Mutations.hx:187 restoreLegs()：把各种兽/蛇/鱼/蛛类下肢恢复成人类双腿。</summary>
        public virtual bool RestoreLegs(string tfSource)
        {
            var player = CoC.Instance.Player;

            var tsParts = tfSource.Split('-');

            var source = tsParts[0];

            var doRestore = false;

            if (player.IsTaur())
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.Taur".Localized());

                doRestore = true;
            }

            if (player.IsGoo())
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.Goo".Localized());

                doRestore = true;
            }

            if (player.IsNaga())
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.Naga".Localized());

                doRestore = true;
            }

            if (player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Mermaid)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.Mermaid".Localized());

                doRestore = true;
            }

            if (source != "sweetGossamer" && player.IsDrider())
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.Drider".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.DriderHuman".Localized());

                doRestore = true;
            }

            if (source == "regularHummus" && player.IsBiped()
                && player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id

                    != BodyPartTypes.Human)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RestoreLegs.NormalHummus".Localized());

                doRestore = true;
            }

            if (!doRestore)
                return false;

            var lowerBody = player.Body.Get<BodyParts.Vanilla.LowerBodyPart>();

            lowerBody.Id = BodyPartTypes.Human;

            lowerBody.LegCount = 2;

            Changes += 1;

            return true;
        }

        /// <summary>原版 Mutations.hx:281 restoreRearBody()：移除鲨鱼鳍 / 龙鬃 / 龙棘 / 树皮。</summary>
        public virtual bool RestoreRearBody(string tfSource)
        {
            var player = CoC.Instance.Player;

            var tsParts = tfSource.Split('-');

            var source = tsParts[0];

            var forceRestore = tsParts.Contains("forceRestoreRearBody");

            var rearBody = player.Body.Get<BodyParts.Vanilla.RearBodyPart>();

            if (rearBody.Id == BodyPartTypes.SharkFin)
            {
                if (source == "sharkTooth" || (!forceRestore && player.SharkScore() >= 3))
                    return false;

                Scenes.Scene.OutputText("Scenes.Mutations.RestoreRearBody.SharkFin".Localized());
            }
            else if (rearBody.Id == BodyPartTypes.DraconicMane
                || rearBody.Id == BodyPartTypes.DraconicSpikes)
            {
                if (source == "EmberTFs" || (!forceRestore && player.DragonScore() >= 11))
                    return false;

                Scenes.Scene.OutputText(
                    rearBody.Id == BodyPartTypes.DraconicMane

                        ? "Scenes.Mutations.RestoreRearBody.DraconicMane".Localized()

                        : "Scenes.Mutations.RestoreRearBody.DraconicSpikes".Localized());
            }
            else if (rearBody.Id == BodyPartTypes.Bark)
            {
                if (source == "alraune" || (!forceRestore && player.DryadScore() >= 8))
                    return false;

                Scenes.Scene.OutputText("Scenes.Mutations.RestoreRearBody.Bark".Localized());
            }
            else
            {
                rearBody.Restore();

                return false;
            }

            if (!forceRestore)
                Changes += 1;

            rearBody.Restore();

            return true;
        }

        /// <summary>
        /// 原版 Mutations.hx:241 restoreNeck()。
        /// 原版文本含 [if (neckPos) …] 条件（颈部位置），C# 侧没有 neckPos 概念，
        /// 这里采用「无颈部位置」分支的文本，差异已注明。
        /// </summary>
        public virtual bool RestoreNeck(string tfSource)
        {
            var player = CoC.Instance.Player;

            var tsParts = tfSource.Split('-');

            if (tsParts.Length > 1 && tsParts[0] != "reptilum")
                tfSource = tsParts[0];

            var forceRestore = tsParts.Contains("forceRestoreNeck");

            var neck = player.Body.Get<BodyParts.Vanilla.NeckPart>();

            if (neck.Id == BodyPartTypes.Draconic)
            {
                if (tfSource == "EmberTFs" || (!forceRestore && player.DragonScore() >= 11))
                    return false;

                Scenes.Scene.OutputText(neck.Pos

                    ? "Scenes.Mutations.RestoreNeck.DraconicPosition".Localized()

                    : "Scenes.Mutations.RestoreNeck.Draconic".Localized());
            }
            else if (neck.Id == BodyPartTypes.Cockatrice)
            {
                if (tfSource == "TonOTrice" || (!forceRestore && player.CockatriceScore() >= 7))
                    return false;

                Scenes.Scene.OutputText("Scenes.Mutations.RestoreNeck.Cockatrice".Localized());
            }
            else
            {
                neck.Restore();

                return false;
            }

            if (!forceRestore)
                Changes += 1;

            neck.Restore();

            return true;
        }

        /// <summary>原版 Mutations.hx:415 removeBassyHair()。</summary>
        public virtual bool RemoveBassyHair()
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var hair = player.Body.Get<BodyParts.Vanilla.HairPart>();

            if (hair.Id != BodyPartTypes.BasiliskPlume
                && hair.Id != BodyPartTypes.BasiliskSpines)

                return false;

            if (hair.Id == BodyPartTypes.BasiliskPlume)
            {
                Scenes.Scene.OutputText(hair.Length >= 5

                    ? "Scenes.Mutations.RemoveBassyHair.PlumeLong".Localized()

                    : "Scenes.Mutations.RemoveBassyHair.PlumeShort".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.RemoveBassyHair.PlumeGone".Localized());
            }
            else
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveBassyHair.Spines".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.RemoveBassyHair.SpinesRegrow".Localized());

                flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                hair.Length = 0;
            }

            hair.Id = BodyPartTypes.Normal;

            Changes += 1;

            return true;
        }

        /// <summary>原版 Mutations.hx:968 growDemonCock()。</summary>
        public virtual void GrowDemonCock(float growCocks)
        {
            var player = CoC.Instance.Player;

            var temp = 0;

            while (growCocks > 0)
            {
                player.CreateCock();

                var cock = player.Cocks[^1];

                cock.Length = Random.Shared.Next(3) + 4;

                cock.Thickness = 0.75f;

                growCocks--;

                temp += 1;
            }

            Scenes.Scene.OutputText("Scenes.Mutations.GrowDemonCock.Base".Localized());

            Scenes.Scene.OutputText(temp == 1

                ? "Scenes.Mutations.GrowDemonCock.Single".Localized()

                : "Scenes.Mutations.GrowDemonCock.Multiple".Localized(temp));

            if (temp > 4)
                Scenes.Scene.OutputText("Scenes.Mutations.GrowDemonCock.Sensitive".Localized());

            player.Orgasm("Dick");
        }

        /// <summary>原版 Mutations.hx:706 gainSnakeTongue()。</summary>
        public virtual bool GainSnakeTongue()
        {
            var player = CoC.Instance.Player;

            var tongue = player.Body.Get<BodyParts.Vanilla.TonguePart>();

            if (tongue.Id == BodyPartTypes.Snake
                || Changes >= ChangeLimit)

                return false;

            if (tongue.Id == BodyPartTypes.Human)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.GainSnakeTongue.Human1".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.GainSnakeTongue.Human2".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.GainSnakeTongue.Human3".Localized());
            }
            else
            {
                Scenes.Scene.OutputText("Scenes.Mutations.GainSnakeTongue.Inhuman1".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.GainSnakeTongue.Inhuman2".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.GainSnakeTongue.Inhuman3".Localized());
            }

            tongue.Id = BodyPartTypes.Snake;

            player.Sensitivity = Math.Max(0f, player.Sensitivity + 5f);

            Changes += 1;

            return true;
        }

        /// <summary>原版 Mutations.hx:726 gainLizardTongue()。</summary>
        public virtual bool GainLizardTongue()
        {
            var player = CoC.Instance.Player;

            var tongue = player.Body.Get<BodyParts.Vanilla.TonguePart>();

            if (tongue.Id == BodyPartTypes.Lizard)
                return false;

            Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.Intro".Localized());

            if (tongue.Id == BodyPartTypes.Snake)
                Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.FromSnake".Localized());
            else if (tongue.Id == BodyPartTypes.Demonic)
                Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.FromDemonic".Localized());
            else if (tongue.Id == BodyPartTypes.Draconic)
                Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.FromDraconic".Localized());
            else if (tongue.Id == BodyPartTypes.Echidna)
                Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.FromEchidna".Localized());
            else
                Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.Default".Localized());

            Scenes.Scene.OutputText("Scenes.Mutations.GainLizardTongue.Done".Localized());

            tongue.Id = BodyPartTypes.Lizard;

            player.Sensitivity = Math.Max(0f, player.Sensitivity + 5f);

            Changes += 1;

            return false;
        }

        /// <summary>原版 Mutations.hx:651 updateGills()：0=无 1=海葵 2=鱼 3=人鱼（与原版 Gills 常量对应）。</summary>
        public virtual int UpdateGills(int newGillType = 0)
        {
            var player = CoC.Instance.Player;

            var gills = player.Body.Get<BodyParts.Vanilla.GillsPart>();

            var oldGillType = GillsIndex(gills.Id);

            if (oldGillType == newGillType)
                return 0;

            // 原版 :659 穿附魔海衬衫时禁止改变鳃（人鱼例外）
            if (newGillType != 3
                && player.UpperGarment.Id == Items.UndergarmentIds.SeaShirt

                && player.UpperGarment.Save.GetBool("enchanted"))

                return 0;

            gills.Id = GillsType(newGillType);

            Changes += 1;

            switch (newGillType)
            {
                case 0:
                    Scenes.Scene.OutputText(oldGillType == 1

                        ? "Scenes.Mutations.UpdateGills.NoneFromAnemone".Localized()

                        : "Scenes.Mutations.UpdateGills.NoneFromGills".Localized());

                    Scenes.Scene.OutputText("Scenes.Mutations.UpdateGills.NoLonger".Localized());

                    return -1;

                case 1:
                    if (oldGillType == 2 || oldGillType == 3)
                        Scenes.Scene.OutputText("Scenes.Mutations.UpdateGills.AnemoneFromGills".Localized());
                    else
                        Scenes.Scene.OutputText(
                            "Scenes.Mutations.UpdateGills.AnemoneFromNone".Localized(player.NippleDescript(0)));

                    Scenes.Scene.OutputText("Scenes.Mutations.UpdateGills.AnemoneDone".Localized());

                    return 1;

                case 2:
                    Scenes.Scene.OutputText(oldGillType == 1

                        ? "Scenes.Mutations.UpdateGills.FishFromAnemone".Localized()

                        : "Scenes.Mutations.UpdateGills.FishFromNone".Localized());

                    Scenes.Scene.OutputText("Scenes.Mutations.UpdateGills.FishDone".Localized());

                    return 1;

                case 3:
                    // 原版 :695 人鱼鳃的文本由 NobleShirt 输出，这里直接返回 1
                    return 1;

                default:
                    gills.Id = GillsType(oldGillType);

                    Changes -= 1;

                    return 0;
            }
        }

        private static int GillsIndex(MonoCoC.Registry.Identifier id)
        {
            if (id == BodyPartTypes.Anemone) return 1;
            if (id == BodyPartTypes.Fish) return 2;
            if (id == BodyPartTypes.Mermaid) return 3;

            return 0;
        }

        private static MonoCoC.Registry.Identifier GillsType(int index) => index switch
        {
            1 => BodyPartTypes.Anemone,

            2 => BodyPartTypes.Fish,

            3 => BodyPartTypes.Mermaid,

            _ => BodyPartTypes.None,
        };

        /// <summary>原版 Mutations.hx:991 removeExtraBreastRow()。</summary>
        public virtual void RemoveExtraBreastRow(string tfSource)
        {
            var player = CoC.Instance.Player;

            if (player.BreastRows.Count <= 1)
                return;

            Scenes.Scene.OutputText("Scenes.Mutations.RemoveExtraBreastRow.Text".Localized());

            if (tfSource != "regularHummus")
                player.Sensitivity = Math.Max(0f, player.Sensitivity - 5f);

            player.RemoveBreastRow(player.BreastRows.Count - 1, 1);

            Changes += 1;
        }

        /// <summary>原版 lists/ColorLists.hx:? COCKATRICE 色表（7 组，每组 3 个颜色）。</summary>
        private static readonly string[][] CockatriceColors =

        [
            ["blue", "turquoise", "blue"],
            ["orange", "red", "orange"],
            ["green", "yellow", "green"],
            ["purple", "pink", "purple"],
            ["black", "white", "black"],
            ["blonde", "brown", "blonde"],
            ["white", "gray", "white"],
        ];

        /// <summary>原版 Mutations.hx:461 newCockatriceColors()：返回一组颜色（对应 ColorLists.COCKATRICE 的 7 组）。</summary>
        public virtual string[] NewCockatriceColors() => CockatriceColors[Random.Shared.Next(CockatriceColors.Length)];

        /// <summary>
        /// 原版 Mutations.hx:584 updateOvipositionPerk(tfSource)。
        /// 返回 1 = 获得产卵特技，-1 = 失去，0 = 无变化。
        /// </summary>
        public virtual int UpdateOvipositionPerk(string tfSource)
        {
            var player = CoC.Instance.Player;

            var tsParts = tfSource.Split('-');

            if (tsParts.Length > 1
                && tsParts[0] != "goldenSeed"

                && tsParts[0] != "catTransformation")

                tfSource = tsParts[0];

            // 原版 :594 蛇怪子宫必有产卵特技（修复历史存档）
            if (player.HasPerk(PerkIds.BasiliskWomb))
            {
                if (player.HasPerk(PerkIds.Oviposition))
                    return 0;

                Scenes.Scene.OutputText("Scenes.Mutations.Oviposition.Gained".Localized());

                player.CreatePerk(PerkIds.Oviposition, 0, 0, 0, 0);

                return 1;
            }

            if (Changes >= ChangeLimit)
                return 0;

            switch (tfSource)
            {
                // 原版 :614 只造成微小变化的 TF：不处理
                case "EmberTFs":
                case "snakeOil":
                case "goldenSeed-HarpyWomb":
                case "winterPudding":
                case "rizzaRootEffect":
                    return 0;

                case "reptilum":
                case "echidnaTFs":
                case "TonOTrice":
                    if (player.HasPerk(PerkIds.Oviposition))
                        return 0;

                    Scenes.Scene.OutputText("Scenes.Mutations.Oviposition.Gained".Localized());

                    player.CreatePerk(PerkIds.Oviposition, 0, 0, 0, 0);

                    Changes += 1;

                    return 1;

                default:
                    if (!player.HasPerk(PerkIds.Oviposition))
                        return 0;

                    // 原版 :640 蜥蜴血统仍高时保留
                    if (player.LizardScore() >= 8)
                        return 0;

                    if (tfSource != "superHummus")
                        Scenes.Scene.OutputText("Scenes.Mutations.Oviposition.Lost".Localized());

                    player.RemovePerk(PerkIds.Oviposition);

                    return -1;
            }
        }

        /// <summary>原版 Mutations.hx:109 restoreArms(tfSource)。返回 1 = 已恢复，0 = 无变化。</summary>
        public virtual int RestoreArms(string tfSource)
        {
            var player = CoC.Instance.Player;

            var arms = player.Body.Get<BodyParts.Vanilla.ArmsPart>();

            var claws = player.Body.Get<BodyParts.Vanilla.ClawsPart>();

            var skin = player.Body.Get<BodyParts.Vanilla.SkinPart>();

            var T = BodyPartTypes.Human;

            var message = "";

            // 原版 :114 刚变成黏液皮肤时的特殊处理
            if (tfSource == "gooGasmic")
            {
                var hasClaws = claws.Id != BodyPartTypes.Normal;

                message = "\n\n";

                if (arms.Id == BodyPartTypes.Harpy)
                {
                    message += "Scenes.Mutations.RestoreArms.GooFeathers".Localized();

                    if (hasClaws)
                        message += "Scenes.Mutations.RestoreArms.GooClawsToo".Localized();
                }
                else if (hasClaws)
                    message += "Scenes.Mutations.RestoreArms.GooClaws".Localized();

                if (hasClaws)
                    message += "Scenes.Mutations.RestoreArms.GooClawsUseless".Localized();

                if (hasClaws || arms.Id == BodyPartTypes.Harpy)
                    Scenes.Scene.OutputText(message + "Scenes.Mutations.RestoreArms.HumanAgain".Localized());

                arms.Restore();

                return 1;
            }

            if (Changes >= ChangeLimit || arms.Id == T)
                return 0;

            if (arms.Id == BodyPartTypes.Harpy
                || arms.Id == BodyPartTypes.Spider

                || arms.Id == BodyPartTypes.Salamander

                || arms.Id == BodyPartTypes.Dragon

                || arms.Id == BodyPartTypes.Lizard)

                message += "Scenes.Mutations.RestoreArms.ScratchBiceps".Localized();

            if (arms.Id == BodyPartTypes.Harpy)
                message += "Scenes.Mutations.RestoreArms.FromHarpy".Localized();
            else if (arms.Id == BodyPartTypes.Spider)
                message += "Scenes.Mutations.RestoreArms.FromSpider".Localized();
            else if (arms.Id == BodyPartTypes.Dragon
                || arms.Id == BodyPartTypes.Lizard

                || arms.Id == BodyPartTypes.Salamander)

                message += "Scenes.Mutations.RestoreArms.FromScaled".Localized();
            else if (arms.Id == BodyPartTypes.Predator)
            {
                if (skin.Id == BodyPartTypes.Goo)
                {
                    if (claws.Id != BodyPartTypes.Normal)
                        message += "Scenes.Mutations.RestoreArms.PredatorGoo".Localized();
                }
                else if (skin.Id == BodyPartTypes.Plain
                    || skin.Id == BodyPartTypes.Fur

                    || skin.Id == BodyPartTypes.LizardScales)

                    message += "Scenes.Mutations.RestoreArms.PredatorClaws".Localized();
            }
            else
                message += "Scenes.Mutations.RestoreArms.Default".Localized();

            Scenes.Scene.OutputText(message + "Scenes.Mutations.RestoreArms.HumanAgain".Localized());

            arms.Restore();

            Changes += 1;

            return 1;
        }
        /// <summary>原版 Mutations.hx:758 gainDraconicHorns(tfSource)。</summary>
        public virtual void GainDraconicHorns(string tfSource)
        {
            var player = CoC.Instance.Player;

            var tsParts = tfSource.Split('-');

            string race;

            if (tsParts[0] == "EmberTFs")
                race = "dragon";
            else if (tsParts[0] == "reptilum" && tsParts.Length > 1)
                race = tsParts[1];
            else
                throw new InvalidOperationException("Unimplemented tfSource: '" + tfSource + "' used in gainDraconicHorns!");

            var horns = player.Body.Get<BodyParts.Vanilla.HornsPart>();

            if (horns.Id != BodyPartTypes.DraconicX2
                && horns.Id != BodyPartTypes.DraconicX412InchLong)
            {
                if (horns.Value > 0)
                {
                    // 原版 :777 大量恶魔角 → 合并成四支 12 寸龙角
                    if (horns.Id == BodyPartTypes.Demonic && horns.Value > 4)
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.Condense".Localized());

                        horns.Value = 12;

                        horns.Id = BodyPartTypes.DraconicX412InchLong;
                    }
                    else
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.Warp".Localized());

                        horns.Id = BodyPartTypes.DraconicX2;

                        if (horns.Value > 13)
                        {
                            Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.Shrunk".Localized());

                            horns.Value = 12;
                        }
                    }

                    Changes += 1;
                }
                else
                {
                    Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.GrowPair".Localized());

                    horns.Value = 4;

                    horns.Id = BodyPartTypes.DraconicX2;

                    Changes += 1;
                }
            }
            else if (horns.Id == BodyPartTypes.DraconicX2)
                if (horns.Value < 12)
                {
                    if (Random.Shared.Next(3) == 0)
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.Headache".Localized());

                        horns.Value += 1;
                    }
                    else
                    {
                        Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.Longer".Localized());

                        horns.Value += 3 + Random.Shared.Next(3);
                    }

                    if (horns.Value >= 12)
                        Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.Settled".Localized());

                    Changes += 1;
                }
                else
                {
                    Scenes.Scene.OutputText("Scenes.Mutations.GainDraconicHorns.SecondRow".Localized(race));

                    horns.Id = BodyPartTypes.DraconicX412InchLong;

                    Changes += 1;
                }
        }
        /// <summary>原版 Mutations.hx:330 removeFeatheryHair()。</summary>
        public virtual bool RemoveFeatheryHair()
        {
            var player = CoC.Instance.Player;

            var hair = player.Body.Get<BodyParts.Vanilla.HairPart>();

            if (Changes >= ChangeLimit
                || hair.Id != BodyPartTypes.Feather

                || Random.Shared.Next(4) != 0)

                return false;

            Scenes.Scene.OutputText(hair.Length >= 6

                ? "Scenes.Mutations.RemoveFeatheryHair.Long".Localized()

                : "Scenes.Mutations.RemoveFeatheryHair.Short".Localized());

            hair.Id = BodyPartTypes.Normal;

            Changes += 1;

            return true;
        }

        /// <summary>原版 Mutations.hx:353 removeAntennae()。</summary>
        public virtual bool RemoveAntennae(bool inLine = false)
        {
            var player = CoC.Instance.Player;

            var antennae = player.Body.Get<BodyParts.Vanilla.AntennaePart>();

            if (antennae.Id == BodyPartTypes.None)
                return false;

            if (inLine)
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveAntennae.InLine".Localized());
            else if (antennae.Id == BodyPartTypes.Cockatrice)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveAntennae.Cockatrice".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.RemoveAntennae.CockatriceLost".Localized());
            }
            else if (antennae.Id == BodyPartTypes.Bee)
            {
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveAntennae.Bee".Localized());

                Scenes.Scene.OutputText("Scenes.Mutations.RemoveAntennae.BeeLost".Localized());
            }
            else
                Scenes.Scene.OutputText("Scenes.Mutations.RemoveAntennae.Default".Localized());

            antennae.Id = BodyPartTypes.None;

            Changes += 1;

            return true;
        }

        /// <summary>原版 Mutations.hx:954 removeFur()。</summary>
        public virtual void RemoveFur()
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.OutputText("Scenes.Mutations.RemoveFur.Text".Localized());

            player.Body.Get<BodyParts.Vanilla.SkinPart>().Id = BodyPartTypes.Plain;

            player.Body.Get<BodyParts.Vanilla.UnderBodyPart>().Restore();

            Changes += 1;
        }

        /// <summary>原版 Mutations.hx:961 removeScales()。</summary>
        public virtual void RemoveScales()
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.OutputText("Scenes.Mutations.RemoveScales.Text".Localized());

            player.Body.Get<BodyParts.Vanilla.SkinPart>().Id = BodyPartTypes.Plain;

            player.Body.Get<BodyParts.Vanilla.UnderBodyPart>().Restore();

            Changes += 1;
        }

        private static bool HasReptileFace(Player player)
        {
            var face = player.Body.Get<BodyParts.Vanilla.FacePart>().Id;

            return face == BodyPartTypes.SnakeFangs

                || face == BodyPartTypes.Lizard

                || face == BodyPartTypes.Dragon;
        }
}
}