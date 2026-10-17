using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    /// <summary>
    /// 原版 items/undergarments/NobleShirt.hx:38-398 —— 贵妇衬衫（诅咒系，分 6 阶段把人变成美人鱼）。
    /// 阶段、附魔、束缚等状态存在独立存档块 coc:noble_shirt 里；每小时的 TimeChange/TimeChangeLarge
    /// 负责「卡在阶段 5 时补一次变身」与「凌晨 4 点睡着时的夜变」。
    /// </summary>
    [Register<NobleShirtBehavior>("coc:noble_shirt_behavior")]
    public class NobleShirtBehavior : ItemBehavior, ITimeAware
    {
        public static readonly Identifier ItemId = new("coc", "nobles_shirt");

        private static NobleShirtBehavior? _instance;

        public static NobleShirtBehavior Instance => _instance ??= new NobleShirtBehavior();

        public static SaveContent Save => Scene.SaveContent(new Identifier("coc", "noble_shirt"));

        private bool _transformChecked;

        // ── 原版 SaveContent 字段 ─────────────────────────────────────────
        private static int ArmorStage
        {
            get => Save.GetInt("armorStage", 1);

            set => Save.SetInt("armorStage", value);
        }

        public static bool Enchanted
        {
            get => Save.GetBool("enchanted", true);

            set => Save.SetBool("enchanted", value);
        }

        public static bool Bound
        {
            get => Save.GetBool("bound", true);

            set => Save.SetBool("bound", value);
        }

        private static bool UnequipTried
        {
            get => Save.GetBool("unequipTried");

            set => Save.SetBool("unequipTried", value);
        }

        private static int CampDay
        {
            get => Save.GetInt("campDay");

            set => Save.SetInt("campDay", value);
        }

        private static int TransformDay
        {
            get => Save.GetInt("transformDay");

            set => Save.SetInt("transformDay", value);
        }

        private static bool BurnEvent
        {
            get => Save.GetBool("burnEvent");

            set => Save.SetBool("burnEvent", value);
        }

        private static bool RathazulEvent
        {
            get => Save.GetBool("rathazulEvent");

            set => Save.SetBool("rathazulEvent", value);
        }

        private static bool FacePlant
        {
            get => Save.GetBool("facePlant");

            set => Save.SetBool("facePlant", value);
        }

        private static bool GretaBoughtUnbind
        {
            get => Save.GetBool("gretaBoughtUnbind");

            set => Save.SetBool("gretaBoughtUnbind", value);
        }

        private static bool ArianDisenchant
        {
            get => Save.GetBool("arianDisenchant");

            set => Save.SetBool("arianDisenchant", value);
        }

        private static bool SoftResetFlag
        {
            get => Save.GetBool("isSoftReset", true);

            set => Save.SetBool("isSoftReset", value);
        }

        // 原版 reset()/softReset()：软重置保留「是否尝试脱下 / 营地日 / 事件标记 / 询问进度」
        public static void Reset()
        {
            Save.Clear();

            SoftResetFlag = true;
        }

        public static void SoftReset()
        {
            var unequipTried = UnequipTried;

            var campDay = CampDay;

            var burnEvent = BurnEvent;

            var rathazulEvent = RathazulEvent;

            var facePlant = FacePlant;

            var rathazulAsked = Save.GetBool("rathazulAsked");

            var izmaAsked = Save.GetBool("izmaAsked");

            var arianAsked = Save.GetBool("arianAsked");

            Save.Clear();

            UnequipTried = unequipTried;

            CampDay = campDay;

            BurnEvent = burnEvent;

            RathazulEvent = rathazulEvent;

            FacePlant = facePlant;

            Save.SetBool("rathazulAsked", rathazulAsked);

            Save.SetBool("izmaAsked", izmaAsked);

            Save.SetBool("arianAsked", arianAsked);

            SoftResetFlag = true;
        }

        // 原版 :179 wearingEnchanted()
        public static bool WearingEnchanted() =>

            Enchanted && CoC.Instance.Player.UpperGarment.Id == ItemId;

        // 原版 :183 knownBound()
        public static bool KnownBound() => Bound && UnequipTried;

        // 原版 :187 isNormalShirt()
        public static bool IsNormalShirt() => !Bound && !Enchanted;

        // 原版 :195 isGoneCompletely()
        public static bool IsGoneCompletely() =>

            !GretaBoughtUnbind && !ArianDisenchant

            && !CoC.Instance.Player.HasItemAnywhere(ItemRegistry.Create(ItemId));

        // 原版 :90 timeChange()
        public bool TimeChange()
        {
            _transformChecked = false;

            if (!SoftResetFlag && IsGoneCompletely())
                SoftReset();

            return false;
        }

        // 原版 :99 timeChangeLarge()
        public bool TimeChangeLarge()
        {
            var player = CoC.Instance.Player;

            var time = Scene.View.Time;

            if (!WearingEnchanted())
                return false;

            if (!player.Sleeping || time.Hours != 4)
                return false;

            if (ArmorStage == 6)
                return false;

            if (!_transformChecked && TransformDay != time.Days)
            {
                _transformChecked = true;

                var chance = 10 + (!player.HasPerk(PerkIds.LoliliciousBody) ? (time.Days - TransformDay) * 10 : 0);

                if (Random.Shared.Next(100) < chance)
                {
                    NightTransformation();

                    TransformDay = time.Days;

                    return true;
                }
            }

            if (ArmorStage == 5)
            {
                NightTransformation();

                return true;
            }

            return false;
        }

        // 原版 :202 campEventsMinor()
        public static void CampEventsMinor()
        {
            if (Random.Shared.Next(100) < 90)
                return;

            var femOutputs = new[]
            {
                "Scenes.NobleShirt.Minor.Fem1",

                "Scenes.NobleShirt.Minor.Fem2",

                "Scenes.NobleShirt.Minor.Fem3",
            };

            var normalOutputs = new System.Collections.Generic.List<string>();

            if (WearingEnchanted() || BurnEvent)
            {
                normalOutputs.Add("Scenes.NobleShirt.Minor.Normal1");

                normalOutputs.Add("Scenes.NobleShirt.Minor.Normal2");

                normalOutputs.Add("Scenes.NobleShirt.Minor.Normal3");
            }

            if (WearingEnchanted() && Random.Shared.Next(100) < 50)
            {
                Scene.OutputText("[pg]" + femOutputs[Random.Shared.Next(femOutputs.Length)].Localized() + "[pg]");

                FemBoost(1);
            }
            else if (normalOutputs.Count > 0)
                Scene.OutputText("[pg]" + normalOutputs[Random.Shared.Next(normalOutputs.Count)].Localized() + "[pg]");
        }

        // 原版 :240 campEventsMajor()
        public static bool CampEventsMajor()
        {
            var time = Scene.View.Time;

            if (CampDay == time.Days)
                return false;

            var evented = PickMajorEvent();

            if (evented)
                CampDay = time.Days;

            return evented;
        }

        // 原版 :253 pickMajorEvent()
        private static bool PickMajorEvent()
        {
            var player = CoC.Instance.Player;

            // 原版 :254 人鱼鳃的三个独立小事件
            if (player.Body.Get<GillsPart>().Id.Path == "mermaid" && Random.Shared.Next(100) < 20)
            {
                if (!BurnEvent)
                {
                    Scene.OutputText("Scenes.NobleShirt.Major.BurnFirst".Localized());

                    BurnEvent = true;

                    player.ChangeFatigue(10);

                    return true;
                }

                if (!RathazulEvent
                    && SceneInstance<Scenes.Vanilla.Npcs.RathazulScene>.Get().FollowerRathazul())
                {
                    Scene.OutputText("Scenes.NobleShirt.Major.Rathazul".Localized());

                    RathazulEvent = true;

                    player.ChangeFatigue(-5);

                    return true;
                }

                if (!FacePlant && IsFullMermaid(player))
                {
                    Scene.OutputText("Scenes.NobleShirt.Major.FacePlant".Localized());

                    FemBoost(3);

                    FacePlant = true;

                    player.Speed = Math.Max(0, player.Speed - 1);

                    return true;
                }
            }

            // 原版 :278 阶段 3 之后、附魔在身上时的 5% 小变身
            if (ArmorStage >= 3 && WearingEnchanted() && Random.Shared.Next(100) < 5)
            {
                var outputs = new System.Collections.Generic.List<string>
                {
                    "Scenes.NobleShirt.Major.DecreaseHair",
                };

                if (player.IsBiped())
                    outputs.Add("Scenes.NobleShirt.Major.DecreaseBiped");

                Scene.OutputText("[pg]");

                Scene.OutputText(outputs[Random.Shared.Next(outputs.Count)].Localized());

                DecreaseCockAndBalls(0.2f, 0.1f);

                FemBoost(2);

                return true;
            }

            return false;
        }

        // 原版 :291 decreaseCockAndBalls()
        private static void DecreaseCockAndBalls(float cockDecrease, float ballDecrease)
        {
            var player = CoC.Instance.Player;

            if (GameSettings.Hyper)
                return;

            if (player.HasCock())
                foreach (var cock in player.Cocks)

                    if (cock.Length >= 2 + cockDecrease)
                    {
                        cock.Length -= cockDecrease;

                        break;
                    } // 原版 :296 最小保留 2 英寸

            // 原版 :305 蛋蛋最小保留 1
            if (player.HasBalls() && player.BallSize >= 1 + ballDecrease)
                player.BallSize -= ballDecrease;

            Scene.OutputText("[pg]");

            player.LengthChange(cockDecrease * -1, player.Cocks.Count);

            Scene.OutputText("[pg]");
        }

        // 原版 :314 femBoost()
        private static void FemBoost(int amount)
        {
            var player = CoC.Instance.Player;

            var goal = player.Gender switch
            {
                Gender.Genderless or Gender.Hermaphrodite => 85f,

                Gender.Male => 71f,

                _ => 100f,
            };

            if (player.Femininity < goal)
                player.Femininity = Math.Min(goal, player.Femininity + amount);
        }

        // 原版 :319 nightTransformation()
        public static void NightTransformation()
        {
            var player = CoC.Instance.Player;

            var scene = Scene.View;

            Scene.ClearOutput();

            switch (ArmorStage)
            {
                case 1:
                    Scene.OutputText("Scenes.NobleShirt.Night.Stage1First".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage1Second".Localized());

                    FemBoost(4);

                    player.Speed = Math.Max(0, player.Speed - 2);

                    ArmorStage = 2;

                    break;

                case 2:
                    Scene.OutputText("Scenes.NobleShirt.Night.Stage2First".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage2Second".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage2Third".Localized());

                    Mutations.Mutations.Instance.UpdateGills(3);

                    FemBoost(4);

                    player.Speed = Math.Max(0, player.Speed - 2);

                    ArmorStage = 3;

                    break;

                case 3:
                    Scene.OutputText("Scenes.NobleShirt.Night.Stage3First".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage3Second".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage3Third".Localized());

                    FemBoost(4);

                    foreach (var breast in player.BreastRows)
                        if (breast.BreastRating < (player.IsChild() ? 1 : 2))
                        {
                            breast.BreastRating += 1;

                            break;
                        }

                    DecreaseCockAndBalls(2.5f, 1.5f);

                    ArmorStage = 4;

                    break;

                case 4:
                    Scene.OutputText("Scenes.NobleShirt.Night.Stage4First".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage4Second".Localized());

                    ArmorStage = 5;

                    break;

                case 5:
                    Scene.OutputText("Scenes.NobleShirt.Night.Stage5First".Localized());

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage5Second".Localized());

                    // 原版 :358 小鸡鸡太小或没有阴道时：缩掉最小的屌/长阴道
                    if (player.ShortestCockLength() <= 5 || !player.HasVagina())
                    {
                        if (player.ShortestCockLength() <= 5 && !GameSettings.Hyper)
                        {
                            player.RemoveCock(player.SmallestCockIndex(), 1);

                            player.Balls = 0;
                        }

                        if (!player.HasVagina())
                            player.CreateVagina();

                        Scene.OutputText("Scenes.NobleShirt.Night.Stage5Third".Localized());
                    }

                    Scene.OutputText("Scenes.NobleShirt.Night.Stage5Fourth".Localized());

                    var lowerBody = player.Body.Get<LowerBodyPart>();

                    lowerBody.Id = BodyPartTypes.Mermaid;

                    lowerBody.LegCount = 1;

                    // 原版 :374 人鱼下体配尾巴很怪，直接静默丢弃尾巴
                    var tail = player.Body.Get<TailPart>();

                    tail.Id = BodyPartTypes.None;

                    tail.Venom = 0;

                    tail.Recharge = 0;

                    var underBody = player.Body.Get<UnderBodyPart>();

                    underBody.Id = BodyPartTypes.Mermaid;

                    // 原版 :378 已有鳞片/爬虫下体则保留配色，否则从色表随机
                    if (!(player.HasScales() || player.Body.Get<UnderBodyPart>().Id.Path == "reptile"))
                    {
                        underBody.Skin.Tone = ColorLists.MermaidScale[Random.Shared.Next(ColorLists.MermaidScale.Length)];

                        underBody.Skin.Adj = "";
                    }

                    underBody.Skin.Id = BodyPartTypes.FishScales;

                    underBody.Skin.DescKey = "Skin.Desc.Scales";

                    player.Body.Get<ArmsPart>().UpdateClaws(player.Body.Get<ArmsPart>().Claws.Id);

                    player.Speed = Math.Max(0, player.Speed - 20);

                    player.CreatePerk(PerkIds.MermaidChant);

                    ArmorStage = 6;

                    break;
            }

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.Go<CampScene>());
        }

        // 原版 PlayerHelper.hx isFullMermaid()
        private static bool IsFullMermaid(Player player) =>

            player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Mermaid

            && player.Body.Get<GillsPart>().Id.Path == "mermaid"

            && player.Body.Get<SkinPart>().Id.Path == "fish_scales";
    }
}