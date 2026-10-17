using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/Equinum.hx:13-606 —— Equinum（马用转化药水）。
    /// 逐行对照迁移：20% 坏结局、属性增减、马屌/蛋蛋、阴道、缩胸、眼/脸/毛/蹄/耳/尾。
    /// </summary>
    public class EquinumEffect : ConsumableEffect
    {
        // 原版 Equinum.hx:19 —— 转化来源标识，传给 Mutations 的各个 restore* 方法
        private const string TfSource = "equinum";

        // 原版 Vagina.hx:23-25 —— 阴道松紧/湿润常量
        private const int LoosenessGaping = 3;

        private const int WetnessNormal = 1;

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;
            // 命名空间同名的类：必须写全路径，否则 Mutations.Instance 解析到命名空间
            var mutations = Mutations.Mutations.Instance;

            player.SlimeFeed();

            // 原版 :28 mutations.initTransformation([2, 3])
            mutations.InitTransformation([2, 3]);

            context.Write("Items.Equinum.Use".Localized());

            var face = player.Body.Get<FacePart>();

            var tail = player.Body.Get<TailPart>();

            var lowerBody = player.Body.Get<LowerBodyPart>();

            var ears = player.Body.Get<EarsPart>();

            var eyes = player.Body.Get<EyesPart>();

            var neck = player.Body.Get<NeckPart>();

            var skin = player.Body.Get<SkinPart>();

            var rearBody = player.Body.Get<RearBodyPart>();

            // ── 原版 :33-123 坏结局：已经完全马化时 20% 触发 ─────────────────
            if (player.HasFur()
                && face.Id == BodyPartTypes.Horse

                && tail.Id == BodyPartTypes.Horse

                && lowerBody.Id == BodyPartTypes.Hoofed)
            {
                // 原版 :36-44 重复警告
                if (player.HasStatusEffect(StatusEffectIds.HorseWarning) && Rand(random, 3) == 0)
                {
                    if (player.StatusEffectValue(StatusEffectIds.HorseWarning, 0) == 0)
                        context.Write("Items.Equinum.Warn.Chill".Localized());

                    if (player.StatusEffectValue(StatusEffectIds.HorseWarning, 0) > 0)
                        context.Write("Items.Equinum.Warn.HowMany".Localized());

                    player.AddStatusValue(StatusEffectIds.HorseWarning, 0, 1);
                }

                // 原版 :46-49 首次警告
                if (!player.HasStatusEffect(StatusEffectIds.HorseWarning))
                {
                    context.Write("Items.Equinum.Warn.First".Localized());

                    player.CreateStatusEffect(StatusEffectIds.HorseWarning);
                }

                // 原版 :51 坏结局判定
                if ((Rand(random, 4) == 0
                    && player.HasStatusEffect(StatusEffectIds.HorseWarning)

                    && !IsTfResistant(player)) && (player.StatusEffectValue(StatusEffectIds.HorseWarning, 0) > 0))

                        if (player.CockTotal() > 0)
                        {
                            // 原版 :57-88 有马屌 → 坏结局
                            if (player.CountCocksOfType("Horse") > 0)
                            {
                                WriteBadEnd(context, player, withCocks: true);

                                Scene.GameOver();

                                return;
                            }
                        }
                        else
                        {
                            // 原版 :90-120 没有屌 → 坏结局
                            WriteBadEnd(context, player, withCocks: false);

                            Scene.GameOver();

                            return;
                        } // 原版 :53 必须先被警告过（v1 > 0）
            }

            // ── 原版 :126-179 属性变化 ─────────────────────────────────────
            // 力量
            if (Rand(random, 2) == 0)
                if (player.Strength >= 60)
                    context.Write("Items.Equinum.StrMax".Localized());
                else
                {
                    player.Strength = Math.Min(100, player.Strength + 1);

                    context.Write("Items.Equinum.StrUp".Localized());
                }

            // 韧性
            if (Rand(random, 2) == 0)
                if (player.Toughness >= 75)
                    context.Write(player.Gender is Gender.Male or Gender.Hermaphrodite

                        ? "Items.Equinum.TouMaxStallion".Localized()

                        : "Items.Equinum.TouMaxMare".Localized());
                else
                {
                    player.Toughness = Math.Min(100, player.Toughness + 1.25f);

                    context.Write("Items.Equinum.TouUp".Localized());
                }

            // 智力（原版 :155-179，注意是并列 if，不是 else-if）
            if (Rand(random, 3) == 0)
            {
                if (player.Intelligence <= 5)
                    context.Write("Items.Equinum.InteMute".Localized());

                if (player.Intelligence < 10 && player.Intelligence > 5)
                {
                    player.Intelligence = Math.Max(0, player.Intelligence - 1);

                    context.Write("Items.Equinum.InteDumb".Localized());
                }

                if (player.Intelligence <= 20 && player.Intelligence >= 10)
                {
                    player.Intelligence = Math.Max(0, player.Intelligence - 2);

                    context.Write("Items.Equinum.InteBlank".Localized());
                }

                if (player.Intelligence <= 30 && player.Intelligence > 20)
                {
                    player.Intelligence = Math.Max(0, player.Intelligence - 3);

                    context.Write("Items.Equinum.InteBimbo".Localized());
                }

                if (player.Intelligence <= 50 && player.Intelligence > 30)
                {
                    player.Intelligence = Math.Max(0, player.Intelligence - 4);

                    context.Write("Items.Equinum.InteSlow".Localized());
                }

                if (player.Intelligence > 50)
                {
                    player.Intelligence = Math.Max(0, player.Intelligence - 5);

                    context.Write("Items.Equinum.InteSluggish".Localized());
                }
            }

            // 原版 :181 颈部还原
            if (neck.Id != BodyPartTypes.Normal && mutations.Changes < mutations.ChangeLimit && Rand(random, 4) == 0)
                mutations.RestoreNeck(TfSource);

            // 原版 :185 后体还原（非鲨鱼鳍）
            if (rearBody.Id != BodyPartTypes.None
                && rearBody.Id != BodyPartTypes.SharkFin

                && mutations.Changes < mutations.ChangeLimit

                && Rand(random, 5) == 0)

                mutations.RestoreRearBody(TfSource);

            // 原版 :189 产卵 perk 流失
            if (Rand(random, 5) == 0)
                mutations.UpdateOvipositionPerk(TfSource);

            // 原版 :193 手臂还原
            if (Rand(random, 4) == 0)
                mutations.RestoreArms(TfSource);

            // 原版 :197 去掉羽毛头发
            mutations.RemoveFeatheryHair();

            // ── 原版 :202-358 男性特征 ─────────────────────────────────────
            if (player.Gender is Gender.Male or Gender.Hermaphrodite
                && Rand(random, 1.5) == 0

                && mutations.Changes < mutations.ChangeLimit)

                if (player.CountCocksOfType("Horse") + player.CountCocksOfType("Demon") < player.Cocks.Count)
                {
                    var temp = 0;

                    // 原版 :207-242 单屌
                    if (player.Cocks.Count == 1)
                    {
                        // 原版 :210 用 temp3 记录是否发生转化
                        var changed = false;

                        var cock = player.Cocks[0];

                        if (cock.Id == BodyPartTypes.Human)
                        {
                            context.Write("Items.Equinum.CockHumanToHorse".Localized());

                            temp = AddHorseCock(player);

                            player.IncreaseCock(temp, Rand(random, 4) + 4);

                            changed = true;

                            AddStats(player, lib: 5, sens: 4, lust: 35);
                        }

                        if (cock.Id == BodyPartTypes.Dog)
                        {
                            // 原版 :219 先 addHorseCock，文本里用枚举常量取名，顺序无影响
                            temp = AddHorseCock(player);

                            context.Write("Items.Equinum.CockDogToHorse".Localized(
                                Appearance.CockNoun(BodyPartTypes.Dog),

                                Appearance.CockNoun(BodyPartTypes.Dog),

                                Appearance.CockNoun(BodyPartTypes.Horse)));

                            player.IncreaseCock(temp, Rand(random, 4) + 4);

                            changed = true;

                            AddStats(player, lib: 5, sens: 4, lust: 35);
                        }

                        if (cock.Id == BodyPartTypes.Tentacle)
                        {
                            // 修复原版 :226-227 的顺序缺陷：先取旧形态名字再改类型，
                            // 否则文本会把已经变成马屌的它当成触手屌描述
                            var oldDescript = player.CockDescript(0);

                            temp = AddHorseCock(player);

                            context.Write("Items.Equinum.CockOtherToHorse".Localized(
                                oldDescript,

                                Appearance.CockNoun(BodyPartTypes.Horse)));

                            player.IncreaseCock(temp, Rand(random, 4) + 4);

                            changed = true;

                            AddStats(player, lib: 5, sens: 4, lust: 35);
                        }

                        // 原版 :232 cockType.Index > 4（Cat 及之后的类型）
                        if (cock.TypeIndex > 4)
                        {
                            // 原版 :233 先输出旧形态描述，再改类型
                            context.Write("Items.Equinum.CockOtherToHorse".Localized(
                                player.CockDescript(0),

                                Appearance.CockNoun(BodyPartTypes.Horse)));

                            temp = AddHorseCock(player);

                            player.IncreaseCock(temp, Rand(random, 4) + 4);

                            changed = true;

                            AddStats(player, lib: 5, sens: 4, lust: 35);
                        }

                        if (changed)
                            context.Write("Items.Equinum.CockTransformed".Localized());
                    }
                    // 原版 :244-262 多屌
                    else
                    {
                        AddStats(player, lib: 5, sens: 4, lust: 35);

                        temp = AddHorseCock(player);

                        if (temp < 0)

                            context.Write("Items.Equinum.MultiCockError".Localized()); // 原版 :248-252 调试错误分支
                        else
                        {
                            context.Write("Items.Equinum.MultiCockStart".Localized(player.CockDescript(temp)));

                            // 原版 :254 已有鞘
                            if (player.CountCocksOfType("Horse") > 1 || player.CountCocksOfType("Dog") > 0)
                                context.Write("Items.Equinum.MultiCockSheath".Localized());
                            else
                                context.Write("Items.Equinum.MultiCockNewSheath".Localized(
                                    player.CockDescript(temp),

                                    player.CockDescript(temp)));

                            player.IncreaseCock(temp, Rand(random, 4) + 4);

                            context.Write("Items.Equinum.MultiCockGrow".Localized());

                            context.Write("Items.Equinum.MultiCockDone".Localized());
                        }
                    }

                    // 原版 :264 不够粗就加粗
                    if (temp >= 0 && temp < player.Cocks.Count && player.Cocks[temp].Thickness <= 2)
                        player.Cocks[temp].ThickenCock(1);

                    mutations.Changes += 1;
                }
                // 原版 :270-308 全是马屌/恶魔屌 → 加长
                else
                {
                    float growth;

                    int grown;

                    // 单屌（原版 :271 单根时 temp = 0）
                    if (player.Cocks.Count == 1)
                    {
                        grown = 0;

                        growth = player.IncreaseCock(0, Rand(random, 3) + 1);

                        AddStats(player, sens: 1, lust: 10);
                    }
                    // 多屌：找最短的那根（原版 :282-295）
                    else
                    {
                        var counter = player.Cocks.Count;

                        var smallest = 0;

                        while (counter > 0)
                        {
                            counter--;

                            if (player.Cocks[smallest].Length > player.Cocks[counter].Length)
                                smallest = counter;
                        }

                        grown = smallest;

                        growth = player.IncreaseCock(smallest, Rand(random, 4) + 1);

                        AddStats(player, sens: 1, lust: 10);
                    }

                    context.Write("[pg]");

                    if (growth > 2)
                        context.Write("Items.Equinum.CockGrowBig".Localized(player.CockDescript(grown)));

                    if (growth > 1 && growth <= 2)
                        context.Write("Items.Equinum.CockGrowMid".Localized(player.CockDescript(grown)));

                    if (growth <= 1)
                        context.Write("Items.Equinum.CockGrowSmall".Localized(player.CockDescript(grown)));

                    mutations.Changes += 1;
                } // 原版 :204 并非全部是马屌/恶魔屌 → 转化一根

            // 原版 :310-338 变粗 + 白日梦
            if (Rand(random, 2) == 0
                && mutations.Changes < mutations.ChangeLimit

                && player.CountCocksOfType("Horse") > 0)
            {
                player.Cocks[player.ThinnestCock()].ThickenCock(0.5f);

                context.Write("Items.Equinum.CockThickenLead".Localized(
                    Appearance.CockNoun(BodyPartTypes.Horse)));

                var corLib = player.Corruption + player.Libido;

                if (corLib < 50)
                    context.Write("Items.Equinum.CockThicken.Str0".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse)));

                if (corLib >= 50 && corLib < 75)
                    context.Write("Items.Equinum.CockThicken.Str1".Localized());

                if (corLib >= 75 && corLib <= 125)
                    context.Write("Items.Equinum.CockThicken.Str2".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse)));

                if (corLib > 125)
                    context.Write("Items.Equinum.CockThicken.Str3".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse)));

                if (player.Corruption < 30)
                    context.Write("Items.Equinum.CockThicken.Cor0".Localized());

                if (player.Corruption >= 30 && player.Corruption < 60)
                    context.Write("Items.Equinum.CockThicken.Cor1".Localized());

                if (player.Corruption >= 60 && player.Corruption < 90)
                    context.Write("Items.Equinum.CockThicken.Cor2".Localized());

                if (player.Corruption >= 90)
                    context.Write("Items.Equinum.CockThicken.Cor3".Localized());

                AddStats(player, lib: 0.5f, lust: 10);
            }

            // 原版 :340-357 蛋蛋
            if (Rand(random, 2) == 0
                && mutations.Changes < mutations.ChangeLimit

                && player.BallSize <= 3

                && player.CountCocksOfType("Horse") > 0)
            {
                if (player.Balls == 0)
                {
                    player.Balls = 2;

                    player.BallSize = 1;

                    context.Write("Items.Equinum.BallsNew".Localized());

                    AddStats(player, lib: 2, lust: 5);
                }
                else
                {
                    player.BallSize += 1;

                    if (player.BallSize <= 2)
                        context.Write("Items.Equinum.BallsGrow".Localized(player.SimpleBallsDescript()));

                    if (player.BallSize > 2)
                        context.Write("Items.Equinum.BallsGrow2".Localized(player.SimpleBallsDescript()));

                    AddStats(player, lib: 1, lust: 3);
                }

                mutations.Changes += 1;
            }

            // ── 原版 :360-483 女性特征 ─────────────────────────────────────
            if (player.Gender is Gender.Female or Gender.Hermaphrodite)
            {
                // 原版 :362-373 单穴
                if (player.Vaginas.Count == 1)
                {
                    if (player.Vaginas[0].Looseness <= LoosenessGaping
                        && mutations.Changes < mutations.ChangeLimit

                        && Rand(random, 2) == 0)
                    {
                        context.Write("Items.Equinum.VaginaLoosen".Localized(player.VaginaDescript(0)));

                        player.Vaginas[0].Looseness += 1;

                        mutations.Changes += 1;
                    }

                    if (player.Vaginas[0].Wetness <= WetnessNormal
                        && mutations.Changes < mutations.ChangeLimit

                        && Rand(random, 2) == 0)
                    {
                        context.Write("Items.Equinum.VaginaWet".Localized(player.VaginaDescript(0)));

                        player.Vaginas[0].Wetness += 1;

                        mutations.Changes += 1;
                    }
                }
                // 原版 :374-414 多穴
                else
                {
                    // 原版 :376-394 找最干的那个
                    var temp = 0;

                    var temp2 = player.Vaginas[0].Wetness;

                    var counter = player.Vaginas.Count;

                    while (counter > 0)
                    {
                        counter--;

                        if (temp2 > player.Vaginas[counter].Wetness)
                        {
                            temp = counter;

                            temp2 = player.Vaginas[temp].Wetness;
                        }
                    }

                    if (player.Vaginas[temp].Wetness <= WetnessNormal
                        && mutations.Changes < mutations.ChangeLimit

                        && Rand(random, 2) == 0)
                    {
                        context.Write("Items.Equinum.VaginaMultiWet".Localized(player.VaginaDescript(temp)));

                        player.Vaginas[temp].Wetness += 1;

                        mutations.Changes += 1;
                    }

                    // 原版 :395-408 找最紧的那个
                    temp = 0;

                    temp2 = player.Vaginas[0].Looseness;

                    counter = player.Vaginas.Count;

                    while (counter > 0)
                    {
                        counter--;

                        if (temp2 > player.Vaginas[counter].Looseness)
                        {
                            temp = counter;

                            temp2 = player.Vaginas[temp].Looseness;
                        }
                    }

                    // 修复原版 :409 的索引不一致：原版条件读 Vaginas[0]，正文与 +1 却用 temp
                    if (player.Vaginas[temp].Looseness <= LoosenessGaping
                        && mutations.Changes < mutations.ChangeLimit

                        && Rand(random, 2) == 0)
                    {
                        context.Write("Items.Equinum.VaginaMultiLoosen".Localized(player.VaginaDescript(temp)));

                        player.Vaginas[temp].Looseness += 1;

                        mutations.Changes += 1;
                    }
                }

                // 原版 :415 发情
                if ((player.StatusEffectValue(StatusEffectIds.Heat, 1) < 30
                    && Rand(random, 2) == 0

                    && mutations.Changes < mutations.ChangeLimit) && (player.GoIntoHeat(true)))

                        mutations.Changes += 1;

                // 原版 :421 缩胸（hyper happy 模式不缩）
                if ((!GameSettings.Hyper) && (Rand(random, 2) == 0 && mutations.Changes < mutations.ChangeLimit))

                        if (player.BreastRows.Count == 1)
                        {
                            if (player.BreastRows[0].BreastRating > 3)
                            {
                                var shrunk = 1;

                                player.BreastRows[0].BreastRating--;

                                if (player.BreastRows[0].BreastRating > 8)
                                {
                                    shrunk += 1;

                                    player.BreastRows[0].BreastRating--;
                                }

                                if (shrunk == 1)
                                    context.Write("Items.Equinum.BreastShrink1".Localized());

                                if (shrunk == 2)
                                    context.Write("Items.Equinum.BreastShrink2".Localized());

                                mutations.Changes += 1;
                            }
                        }
                        // 多排
                        else
                        {
                            var changedRows = 0;

                            var counter = player.BreastRows.Count;

                            if (player.BiggestBreastSize() > 3)
                                context.Write("\n");

                            while (counter > 0)
                            {
                                counter--;

                                if (player.BreastRows[counter].BreastRating > 3)
                                {
                                    player.BreastRows[counter].BreastRating--;

                                    changedRows += 1;

                                    context.Write(counter < player.BreastRows.Count - 1

                                        ? "Items.Equinum.BreastShrinkRowAnd".Localized(player.BreastDescript(counter), player.BreastCup(counter))

                                        : "Items.Equinum.BreastShrinkRowY".Localized(player.BreastDescript(counter), player.BreastCup(counter)));
                                }
                            }

                            if (changedRows == 2)
                                context.Write("Items.Equinum.BreastLighter".Localized());

                            if (changedRows == 3)
                                context.Write("Items.Equinum.BreastLimber".Localized());

                            if (changedRows >= 4)
                                context.Write("Items.Equinum.BreastWeightless".Localized());

                            if (changedRows > 0)
                                mutations.Changes += 1;
                        } // 单排
            }

            // ── 原版 :487-591 性别无关变化 ─────────────────────────────────
            // 眼球
            if (mutations.Changes < mutations.ChangeLimit && Rand(random, 5) == 0 && eyes.Id != BodyPartTypes.Human)
            {
                if (eyes.Id == BodyPartTypes.BlackEyesSandTrap)
                    context.Write("Items.Equinum.EyesSandTrap".Localized());
                else
                {
                    context.Write("Items.Equinum.EyesChanged".Localized());

                    if (eyes.Id == BodyPartTypes.FourSpiderEyes || eyes.Id == BodyPartTypes.Spider)
                        context.Write("Items.Equinum.EyesArachnid".Localized());

                    context.Write("Items.Equinum.EyesHuman".Localized());
                }

                eyes.Id = BodyPartTypes.Human;

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            // 原版 :502 马脸（需要毛与马耳）
            if (mutations.TfNoFur()
                && face.Id != BodyPartTypes.Horse

                && player.HasFur()

                && mutations.Changes < mutations.ChangeLimit

                && Rand(random, 5) == 0

                && ears.Id == BodyPartTypes.Horse)
            {
                context.Write(face.Id == BodyPartTypes.Dog

                    ? "Items.Equinum.FaceFromDog".Localized()

                    : "Items.Equinum.FaceEquine".Localized());

                mutations.Changes += 1;

                face.Id = BodyPartTypes.Horse;
            }

            // 原版 :512 长出马毛（需要马尾）
            if (mutations.TfNoFur()
                && !player.HasFur()

                && mutations.Changes < mutations.ChangeLimit

                && Rand(random, 4) == 0

                && tail.Id == BodyPartTypes.Horse)
            {
                skin.FurColor = ColorLists.HorseFur[random.Next(ColorLists.HorseFur.Length)];

                if (player.HasPlainSkin())
                    context.Write("Items.Equinum.FurGrow".Localized());

                if (player.HasScales())
                    context.Write("Items.Equinum.FurFromScales".Localized(skin.FurColor));

                mutations.Changes += 1;

                skin.Id = BodyPartTypes.Fur;

                skin.DescKey = "Skin.Desc.fur";

                player.Body.Get<UnderBodyPart>().Restore();
            }

            // 原版 :527 蹄子（需要马尾）
            if (lowerBody.Id != BodyPartTypes.Hoofed && tail.Id == BodyPartTypes.Horse
                && mutations.Changes < mutations.ChangeLimit && Rand(random, 5) == 0)
            {
                // 原版 :528
                mutations.Changes += 1;

                if (lowerBody.Id == BodyPartTypes.Human)
                    context.Write("Items.Equinum.HoovesFeet".Localized());
                else if (lowerBody.Id == BodyPartTypes.Dog)
                    context.Write("Items.Equinum.HoovesPaws".Localized());
                else if (player.HasTailInsteadOfLegs())
                    context.Write("Items.Equinum.HoovesTail".Localized());
                else if (LowerBodyIndex(lowerBody.Id) > 3)
                    context.Write("Items.Equinum.HoovesOther".Localized());
                else if (!player.HasFur())
                    context.Write("Items.Equinum.HoovesFur".Localized());

                context.Write("Items.Equinum.HoovesDone".Localized());

                lowerBody.Id = BodyPartTypes.Hoofed;

                lowerBody.LegCount = 2;

                player.Speed = Math.Min(100, player.Speed + 1);

                // 原版 :545 第二次累加
                mutations.Changes += 1;
            }

            // 原版 :548 马耳（需要马尾）
            if (ears.Id != BodyPartTypes.Horse && tail.Id == BodyPartTypes.Horse
                && mutations.Changes < mutations.ChangeLimit && Rand(random, 3) == 0)
            {
                if (ears.Id == BodyPartTypes.None)

                    context.Write("Items.Equinum.EarsNone".Localized()); // 原版 :549 的 ears.type == -1 是遗留值，C# 里没有耳朵对应 none

                if (ears.Id == BodyPartTypes.Human)
                    context.Write("Items.Equinum.EarsHuman".Localized());

                if (ears.Id == BodyPartTypes.Dog)
                    context.Write("Items.Equinum.EarsDog".Localized());

                if (EarsIndex(ears.Id) > 2)
                    context.Write("Items.Equinum.EarsOther".Localized());

                ears.Id = BodyPartTypes.Horse;

                context.Write("Items.Equinum.EarsDone".Localized());

                mutations.Changes += 1;
            }

            // 原版 :566 马尾（无前置）
            if (tail.Id != BodyPartTypes.Horse && Rand(random, 2) == 0
                && mutations.Changes < mutations.ChangeLimit)
            {
                var tailIndex = TailIndex(tail.Id);

                // 原版 :568 没有尾巴
                if (tail.Id == BodyPartTypes.None)
                    context.Write("Items.Equinum.TailNew".Localized());

                // 原版 :572 其他兽尾（Dog..Cow）
                if (tailIndex > 1 && tailIndex <= 4)
                    context.Write("Items.Equinum.TailMorph".Localized());

                // 原版 :576 虫类腹部（Spider..Bee）
                if (tailIndex > 4 && tailIndex < 7)
                    context.Write("Items.Equinum.TailInsect".Localized());

                // 原版 :579 鲨鱼及之后的尾巴
                if (tailIndex >= 7)
                    context.Write("Items.Equinum.TailShark".Localized());

                context.Write("Items.Equinum.TailDone".Localized());

                tail.Id = BodyPartTypes.Horse;

                tail.Venom = 0;

                tail.Recharge = 0;

                mutations.Changes += 1;
            }

            // 原版 :589 去掉鳃
            if (Rand(random, 4) == 0 && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            // 原版 :593 体脂
            if (Rand(random, 3) == 0)
                context.Write(player.ModTone(60, 1));

            // 原版 :597 保底变化
            if (mutations.Changes == 0)
            {
                context.Write("Items.Equinum.Failsafe".Localized());

                player.HealthChange(20);

                AddStats(player, lust: 3);
            }

            player.RefillHunger(15);

            // 原版 :603 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        // ── 坏结局文本（原版 :58-119）────────────────────────────────────
        private static void WriteBadEnd(ItemUseContext context, Player player, bool withCocks)
        {
            context.Write(withCocks

                ? "Items.Equinum.BadEnd.StartCock".Localized()

                : "Items.Equinum.BadEnd.StartNoCock".Localized());

            // 原版 :59 / :92 两处文本完全相同，共用一个键
            context.Write("Items.Equinum.BadEnd.Pond".Localized());

            // 原版 :60-68 / :93-101 性别词
            if (player.Gender is Gender.Genderless or Gender.Hermaphrodite)
                context.Write("Items.Equinum.BadEnd.GenderHorse".Localized());

            if (player.Gender == Gender.Male)
                context.Write("Items.Equinum.BadEnd.GenderStallion".Localized());

            if (player.Gender == Gender.Female)
                context.Write("Items.Equinum.BadEnd.GenderMare".Localized());

            context.Write(withCocks

                ? "Items.Equinum.BadEnd.LookCock".Localized()

                : "Items.Equinum.BadEnd.LookNoCock".Localized());

            context.Write(withCocks

                ? "Items.Equinum.BadEnd.YearCock".Localized()

                : "Items.Equinum.BadEnd.YearNoCock".Localized());

            context.Write(withCocks

                ? "Items.Equinum.BadEnd.GrazeCock".Localized()

                : "Items.Equinum.BadEnd.GrazeNoCock".Localized());

            if (player.Gender is Gender.Genderless or Gender.Male)
                context.Write("Items.Equinum.BadEnd.Male".Localized());

            if (player.Gender == Gender.Female)
                context.Write("Items.Equinum.BadEnd.Female".Localized());

            if (player.Gender == Gender.Hermaphrodite)
                context.Write("Items.Equinum.BadEnd.Herm".Localized());

            context.Write("Items.Equinum.BadEnd.Nostalgia".Localized());

            context.Write(withCocks

                ? "Items.Equinum.BadEnd.DialogCock".Localized()

                : "Items.Equinum.BadEnd.DialogNoCock".Localized());

            context.Write(withCocks

                ? "Items.Equinum.BadEnd.WhinnyCock".Localized()

                : "Items.Equinum.BadEnd.WhinnyNoCock".Localized());

            context.Write(withCocks

                ? "Items.Equinum.BadEnd.FailCock".Localized()

                : "Items.Equinum.BadEnd.FailNoCock".Localized());
        }

        // 原版 Creature.hx:4255 addHorseCock()：把第一根非马屌改成马屌并返回索引，全马则 -1
        private static int AddHorseCock(Creature creature)
        {
            var counter = creature.Cocks.Count;

            while (counter > 0)
            {
                counter--;

                if (creature.Cocks[counter].Id != BodyPartTypes.Horse)
                {
                    creature.ChangeCockType(counter, "Horse");

                    return counter;
                }
            }

            return -1;
        }

        // 原版 Player.hx:4682 isTFResistant()
        private static bool IsTfResistant(Player player) =>

            (player.HasPerk(PerkIds.TransformationResistance)

                && player.PerkValue(PerkIds.TransformationResistance, 2) == 0)

            || player.HasPerk(PerkIds.LoliliciousBody);

        // 原版 Utils.hx rand(max:Float) = Std.int(Math.random() * max)
        private static int Rand(Random random, double max) => (int)(random.NextDouble() * max);

        // dynStats(Lib/Sens/Lust)：加法并夹在 0..100
        private static void AddStats(Player player, float lib = 0, float sens = 0, float lust = 0)
        {
            if (lib != 0)
                player.Libido = Math.Clamp(player.Libido + lib, 0, 100);

            if (sens != 0)
                player.Sensitivity = Math.Clamp(player.Sensitivity + sens, 0, 100);

            if (lust != 0)
                player.Lust = Math.Clamp(player.Lust + lust, 0, 100);
        }

        // 原版 Tail.hx:2-33 的类型顺序（只关心 Equinum 用到的分界）
        private static int TailIndex(Identifier id) => id switch
        {
            _ when id == BodyPartTypes.None => 0,

            _ when id == BodyPartTypes.Horse => 1,

            _ when id == BodyPartTypes.Dog => 2,

            _ when id == BodyPartTypes.Demonic => 3,

            _ when id == BodyPartTypes.Cow => 4,

            _ when id == BodyPartTypes.SpiderAbdomen => 5,

            _ when id == BodyPartTypes.BeeAbdomen => 6,
            // SHARK=7 及其后的所有尾巴都走「鲨鱼及之后」分支
            _ => 7,
        };

        // 原版 Ears.hx:2-26 的类型顺序
        private static int EarsIndex(Identifier id) => id switch
        {
            _ when id == BodyPartTypes.Human => 0,

            _ when id == BodyPartTypes.Horse => 1,

            _ when id == BodyPartTypes.Dog => 2,

            _ => 3,
        };

        // 原版 LowerBody.hx:2-33 的类型顺序
        private static int LowerBodyIndex(Identifier id) => id switch
        {
            _ when id == BodyPartTypes.Human => 0,

            _ when id == BodyPartTypes.Hoofed => 1,

            _ when id == BodyPartTypes.Dog => 2,

            _ when id == BodyPartTypes.Naga => 3,
            // 4 是废弃的 CENTAUR；DEMONIC_HIGH_HEELS=5 及其后都 > NAGA
            _ => 5,
        };
    }
}