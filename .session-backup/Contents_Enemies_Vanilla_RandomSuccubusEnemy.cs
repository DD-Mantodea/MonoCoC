using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/RandomSuccubus.hx（extends Monster）
    // 随机生成的女魅魔 / 男梦魔 / 双性魔。原版 new(gender, specialP) 带两个参数，
    // 注册表走无参构造，所以这里把两者做成可写字段：默认 "R" / ""（等价 new RandomSuccubus()）。
    // LiddelliumEventDungeon 用到的指定参数场景可走 Create(definition, gender, special)
    // （该地牢在 Contents 全量搜索 0 命中，数据缺失：原版 LiddelliumEventDungeon.hx:265/319）。
    [Register("coc:random_succubus")]
    public class RandomSuccubusEnemy : Enemy
    {
        // 原版构造函数参数（RandomSuccubus.hx:25）
        public string RequestedGender = "R";

        public string Special = "";

        // 原版 StatusEffects.Grappling（RandomSuccubus.hx:265/274/383）：C# 的 VanillaStatusEffects 没有该条目，
        // 按项目 Identifier 约定本地声明。Grappled（玩家被抓住）已有共享常量。
        public static readonly Identifier Grappling = new("coc", "grappling");

        // 原版 CockTypesEnum.TENTACLE（触手屌）：C# 的 VanillaBodyPartTypes 没有这个常量，
        // 但 CockPart.TypeIndex 用的就是 new Identifier("coc", "tentacle")（见 CockPart.cs:21）。
        private static readonly Identifier TentacleType = new("coc", "tentacle");

        private static readonly Identifier GrappleId = new("coc", "random_succubus_grapple");

        private static readonly Identifier SwipeId = new("coc", "random_succubus_swipe");

        private static readonly Identifier LustMagicAttackId = new("coc", "random_succubus_lust_magic_attack");

        private static readonly Identifier TailWhipId = new("coc", "random_succubus_tail_whip");

        private static readonly Identifier HeelKickId = new("coc", "random_succubus_heel_kick");

        private static readonly Identifier HlGrappleId = new("coc", "random_succubus_hl_grapple");

        // 供未来 LiddelliumEventDungeon 之类需要传参的场景使用。
        public static RandomSuccubusEnemy Create(EnemyDefinition definition, string gender, string special)
        {
            var enemy = new RandomSuccubusEnemy { RequestedGender = gender, Special = special };

            enemy.Configure(definition);

            return enemy;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new(gender, specialP) 逐项照抄（RandomSuccubus.hx:25-232）──
            // a = "the " / race = "Demon" 由数据表给；long 原始为空串。

            var gender = RequestedGender;
            var vag = true;

            Tallness = 3 * 12;

            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.DemonicHighHeels;

            // 10% 的概率把 "R" 改成男梦魔或双性魔
            if (gender == "R" && R(10) < 1)
            {
                gender = R(2) switch
                {
                    0 => "M",
                    1 => "H",
                    _ => gender
                };
            }

            if (gender == "M" || gender == "H")
            {
                // createCock(5 + rand(7) + rand(7) - rand(5), rand(501) * 0.01 + 1, DEMON)
                CreateCock(5 + R(7) + R(7) - R(5), R(501) * 0.01f + 1, VanillaBodyPartTypes.Demon);

                Femininity = gender == "M"
                    ? R(10) + R(10) + R(20) + R(60) + R(4) + 1
                    : R(100) + 1;

                vag = false;
            }

            // 双性魔也要有阴道
            if (gender == "H")
                vag = true;

            if (vag)
            {
                // createVagina(false, rand(5) + 1, rand(5) + 1)
                CreateVagina(false, R(5) + 1, R(5) + 1);
            }

            switch (Gender)
            {
                case MonoCoC.Contents.Creatures.Gender.Female:
                    ShortName = "succubus";

                    // 至少 A 罩杯，倾向 DD
                    CreateBreastRow(R(6) + R(5) + 1);

                    Body.Hips = R(6) + R(6) + R(6) + R(6);
                    Body.Butt = R(6) + R(6) + R(6) + R(6);

                    Tallness += R(7) + R(7) + R(13) + R(13) + R(13) + R(13);

                    Body.Get<HairPart>().Length = R(7) + R(7) + R(7) + R(7) + R(7) + R(7) + R(7) + R(7);
                    break;

                case MonoCoC.Contents.Creatures.Gender.Male:
                    ShortName = "incubus";

                    Body.Hips = R(2) + R(2) + R(2) + R(2) + R(2) + R(2) + R(2) + R(2);
                    Body.Butt = R(3) + R(3) + R(3);

                    Balls = R(2) * 2 + 2 - R(2) * 2;
                    BallSize = R(10) + 1;

                    Tallness += R(7) + R(7) + R(7) + R(7) + R(13) + R(13) + R(13) + R(13);

                    Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Human;

                    Body.Get<HairPart>().Length = R(13);
                    break;

                case MonoCoC.Contents.Creatures.Gender.Hermaphrodite:
                    ShortName = "omnibus";

                    CreateBreastRow(R(11));

                    Body.Hips = R(21);
                    Body.Butt = R(21);

                    Balls = R(2) * 2 + 2 - R(2) * 2;
                    BallSize = R(11);

                    Tallness += R(7) + R(7) + R(7) + R(7) + R(13) + R(13) + R(25);

                    Body.Get<HairPart>().Length = R(31);
                    break;
            }

            // 原版：肛门不可能是处，其余随机
            var anal = Body.Get<AnalPart>();
            anal.Looseness = R(5) + 1;
            anal.Wetness = R(6);

            string[] skinTones = ["blue", "purple", "light purple", "dark purple", "dark blue", "indigo", "burgundy", "lavender", "midnight-black"];
            Body.Get<SkinPart>().Tone = skinTones[R(skinTones.Length)];

            string[] hairColors = ["blue", "purple", "light purple", "dark purple", "dark blue", "blond"];
            Body.Get<HairPart>().Color = hairColors[R(hairColors.Length)];

            Identifier[] tails = [VanillaBodyPartTypes.None, VanillaBodyPartTypes.None, VanillaBodyPartTypes.Demonic];
            Body.Get<TailPart>().Id = tails[R(tails.Length)];

            Identifier[] horns =
            [
                VanillaBodyPartTypes.None, VanillaBodyPartTypes.None,
                VanillaBodyPartTypes.Demon, VanillaBodyPartTypes.Ram
            ];
            var hornsPart = Body.Get<HornsPart>();

            hornsPart.Id = horns[R(horns.Length)];

            if (hornsPart.Id != VanillaBodyPartTypes.None)
                hornsPart.Value = R(4) + 1;

            Identifier[] wings =
            [
                VanillaBodyPartTypes.None, VanillaBodyPartTypes.None, VanillaBodyPartTypes.None,
                VanillaBodyPartTypes.BatLikeTiny, VanillaBodyPartTypes.BatLikeTiny,
                VanillaBodyPartTypes.BatLikeLarge, VanillaBodyPartTypes.Imp,
                VanillaBodyPartTypes.ImpLarge, VanillaBodyPartTypes.DraconicSmall,
                VanillaBodyPartTypes.FeatheredLarge
            ];
            var wingsPart = Body.Get<WingsPart>();

            wingsPart.Id = wings[R(wings.Length)];

            if (wingsPart.Id == VanillaBodyPartTypes.FeatheredLarge)
                wingsPart.Color = "black";

            Identifier[] ears =
            [
                VanillaBodyPartTypes.Elfin, VanillaBodyPartTypes.Elfin, VanillaBodyPartTypes.Elfin,
                VanillaBodyPartTypes.Human, VanillaBodyPartTypes.Imp
            ];
            Body.Get<EarsPart>().Id = ears[R(ears.Length)];

            if (HasCock())
            {
                Identifier[] cockTypes =
                [
                    VanillaBodyPartTypes.Demon, VanillaBodyPartTypes.Dog, VanillaBodyPartTypes.Horse,
                    VanillaBodyPartTypes.Human, TentacleType
                ];

                Cocks[0].Id = cockTypes[R(cockTypes.Length)];

                if (Cocks[0].Id == TentacleType)
                {
                    while (R(3) < 1)
                        CreateCock(5 + R(7) + R(7) - R(5), R(501) * 0.01f + 1, TentacleType);
                }
            }

            // ── 战斗属性（RandomSuccubus.hx:163-183）──
            Strength = 50 + R(51);
            Toughness = 40 + R(61);
            Speed = 50 + R(26) + R(26);
            Intelligence = 80 + R(21);

            Libido = 70 + R(31);
            Sensitivity = 60 + R(41);
            Corruption = 100 - R(11);

            Level = 7 + R(5);

            WeaponName = "claws";
            WeaponVerb = "punch";
            WeaponAttack = 10 + R(11);

            ArmorName = "demonic skin";
            ArmorDefense = 5 + R(6);

            // bonusHP = rand(11) * 100（走 MaxHealth 修饰层）
            var bonusHealth = (float)(R(11) * 100);

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            // 原版 bonusLust = rand(11) * 10：C# 的 MaxLust 恒为 100，没有对应修饰。

            Gems = 100 + R(1001) + R(11) * 10;

            // additionalXP = 250 + rand(251)
            AdditionalExperience = 250 + R(251);

            // 原版 drop = WeightedDrop(INCUBID 1, SUCMILK 1, SDELITE 1, SUCWHIP 0.01)：见数据表 loot。
            // 原版 long = ""：正常随机个体没有固定描述。

            // ── special 特化（RandomSuccubus.hx:189-230）──
            if (Special == "LiddelliumCampSucc")
            {
                string[] specialSkin =
                [
                    "blue", "indigo", "purple", "burgundy", "lavender",
                    "blue", "indigo", "purple", "burgundy", "lavender", "midnight-black"
                ];
                Body.Get<SkinPart>().Tone = specialSkin[R(specialSkin.Length)];

                hornsPart.Id = VanillaBodyPartTypes.Demon;
                hornsPart.Value = R(4) + 1;

                string[] specialHair = ["black ", "auburn ", "violet ", "navy-blue ", ""];
                Body.Get<HairPart>().Color = specialHair[R(specialHair.Length)];

                Body.Get<TailPart>().Id = VanillaBodyPartTypes.Demonic;
                wingsPart.Id = VanillaBodyPartTypes.BatLikeLarge;
            }
            else if (Special == "LiddelliumHL")
            {
                Body.Get<SkinPart>().Tone = "deep purple";
                Body.Get<HairPart>().Color = "black";
                Body.Get<TailPart>().Id = VanillaBodyPartTypes.Demonic;
                wingsPart.Id = VanillaBodyPartTypes.BatLikeLarge;

                // 所有随机属性各加一半上限
                Strength += 25;
                Toughness += 30;
                Speed += 30;
                Intelligence += 10;

                Libido += 15;
                Sensitivity += 20;
                Corruption += 10;

                Level += 2;
                WeaponAttack += 5;

                ArmorDefense += 3;

                Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                    () => 500f, new TemporarySource(Guid.NewGuid()));

                Health = MaxHealth;

                // 原版 special 掉落 LETHITE：Enemy.Loot 只读数据表，无法按实例改写
                // （数据缺失：原版 RandomSuccubus.hx:228；LiddelliumEventDungeon 在 Contents 全量搜索 0 命中）。
            }
        }

        // 原版 Utils.rand(n)：0..n-1；n <= 0 时按 0 处理（避免 Random.Next(0) 抛异常）。
        private static int R(int n) => n <= 0 ? 0 : Random.Shared.Next(n);

        private bool IsMale => Gender == MonoCoC.Contents.Creatures.Gender.Male;

        // 原版 performCombatAction（RandomSuccubus.hx:344-378）：
        // 被抓住时只能继续抓/逗，否则走 MonsterAI 权重表。
        public override bool BeforeCombatAction(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(VanillaStatusEffects.Grappled)
                && !HasStatusEffect(Grappling))
            {
                if (Special == "LiddelliumHL")
                    HlGrappleTease(context, (int)player.StatusEffectValue(VanillaStatusEffects.Grappled, 2));
                else
                    Grapple(context);

                return true;
            }

            if (HasStatusEffect(Grappling))
            {
                if (Special == "LiddelliumHL")
                    HlGrappleTease(context, (int)player.StatusEffectValue(VanillaStatusEffects.Grappled, 2));
                else
                    GrappleTease(context);

                return true;
            }

            var options = new List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue)>();

            // grapple：weight 1, cost 0 none，条件 distance == Melee，且 special != LiddelliumHL
            if (Special != "LiddelliumHL" && Distance == CombatDistance.Melee)
                options.Add((GrappleId, 1, 0, FatigueType.None));

            // swipe：weight 1, cost 0 none
            options.Add((SwipeId, 1, 0, FatigueType.None));

            // lustMagicAttack：weight 0.5, cost 10 magical
            if (HasFatigue(10, FatigueType.Magical))
                options.Add((LustMagicAttackId, 0.5f, 10, FatigueType.Magical));

            // tailWhip：weight 0.5, cost 5 physical，条件 tail.type == DEMONIC
            if (Body.Get<TailPart>().Id == VanillaBodyPartTypes.Demonic && HasFatigue(5, FatigueType.Physical))
                options.Add((TailWhipId, 0.5f, 5, FatigueType.Physical));

            // heelKick：weight 0.5, cost 5 physical，条件 lowerBody.type == DEMONIC_HIGH_HEELS
            if (Body.Get<LowerBodyPart>().Id == VanillaBodyPartTypes.DemonicHighHeels
                && HasFatigue(5, FatigueType.Physical))
                options.Add((HeelKickId, 0.5f, 5, FatigueType.Physical));

            // hlGrapple：weight 1, cost 10 physical，条件（special == LiddelliumHL && 玩家未被抓 &&
            // 场上两个敌人都活着且都未被魅惑）
            if (Special == "LiddelliumHL"
                && !player.HasStatusEffect(VanillaStatusEffects.Grappled)
                && context.Enemies.Count >= 2
                && context.Enemies[0].Health > 0 && context.Enemies[1].Health > 0
                && context.Enemies[0].Lust < context.Enemies[0].MaxLust
                && context.Enemies[1].Lust < context.Enemies[1].MaxLust
                && HasFatigue(10, FatigueType.Physical))
            {
                options.Add((HlGrappleId, 1, 10, FatigueType.Physical));
            }

            if (options.Count == 0)
            {
                CombatController.Instance!.Resolver.EnemyBasicAttack();

                return true;
            }

            var total = 0f;

            foreach (var option in options)
                total += option.Weight;

            var roll = context.Random.NextDouble() * total;

            var chosen = options[^1];

            foreach (var option in options)
            {
                roll -= option.Weight;

                if (roll <= 0)
                {
                    chosen = option;
                    break;
                }
            }

            var ability = CombatAbilityRegistry.Get(chosen.Id)
                ?? new CombatAbilityDefinition { Id = chosen.Id, Handler = chosen.Id };

            if (chosen.Cost > 0)
                ChangeFatigue(chosen.Cost, chosen.Fatigue);

            CombatController.Instance!.Resolver.EnemyAction(ability);

            return true;
        }

        // 原版 struggle()（RandomSuccubus.hx:294-319）：被抱住时按双方力量挣脱。
        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(VanillaStatusEffects.Grappled))
                return;

            context.Clear();

            var totalStrength = 0f;

            foreach (var grappler in context.Enemies)
                if (grappler.HasStatusEffect(Grappling))
                    totalStrength += (int)grappler.Strength;

            var grapplers = Math.Max(1, (int)player.StatusEffectValue(VanillaStatusEffects.Grappled, 1));

            var escape = player.StatusEffectValue(VanillaStatusEffects.Grappled, 2) > 6
                || (player.Strength + context.Random.Next(100))
                    > (totalStrength / grapplers + context.Random.Next(100) + 10 * grapplers);

            if (escape)
            {
                context.Append(Special == "LiddelliumHL"
                    ? "Combat.RandomSuccubus.Struggle.YouOvercomeThe".Localized()
                    : "Combat.RandomSuccubus.Struggle.YouManageTo".Localized());

                player.RemoveStatusEffect(VanillaStatusEffects.Grappled);

                foreach (var grappler in context.Enemies)
                    grappler.RemoveStatusEffect(Grappling);
            }
            else
            {
                context.Append(Special == "LiddelliumHL"
                    ? "Combat.RandomSuccubus.Struggle.YouStruggleTo".Localized()
                    : "Combat.RandomSuccubus.Struggle.YouStruggleTo2".Localized());
            }
        }

        // 原版 grapple()（RandomSuccubus.hx:260-281）
        public void Grapple(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(VanillaStatusEffects.Grappled))
            {
                context.Append("Combat.RandomSuccubus.Grapple.The0Joins".Localized(ShortName));

                player.CreateOrFindStatusEffect(VanillaStatusEffects.Grappled).Values[1] += 1;
                CreateStatusEffect(Grappling, 0, 0, 0, 0);

                return;
            }

            context.Append("Combat.RandomSuccubus.Grapple.The0Charges".Localized(ShortName, IsMale ? "his" : "her"));

            if (player.Speed + context.Random.Next(100) < Speed + context.Random.Next(100))
            {
                context.Append("Combat.RandomSuccubus.Grapple.0TacklesYou".Localized(IsMale ? "He" : "She", IsMale ? "his" : "her"));

                if (player.Strength + context.Random.Next(100) < Strength + context.Random.Next(100))
                {
                    context.Append("Combat.RandomSuccubus.Grapple.0HasYou".Localized(IsMale ? "He" : "She", IsMale ? "his" : "her"));

                    player.CreateOrFindStatusEffect(VanillaStatusEffects.Grappled).Values[1] += 1;
                    CreateStatusEffect(Grappling, 0, 0, 0, 0);
                }
                else
                {
                    context.Append("Combat.RandomSuccubus.Grapple.ThankfullyYouManage".Localized(IsMale ? "him" : "her"));
                }
            }
            else
            {
                context.Append("Combat.RandomSuccubus.Grapple.HoweverYourSpeed".Localized(IsMale ? "he" : "she"));
            }
        }

        // 原版 grappleTease()（RandomSuccubus.hx:283-292）
        private void GrappleTease(CombatContext context)
        {
            var player = context.Player;

            if (!IsMale && Body.Get<TailPart>().Id != VanillaBodyPartTypes.None)
                context.Append("Combat.RandomSuccubus.GrappleTease.HerTailWriggles".Localized());

            var lust = (int)((5 + context.Random.Next(6))
                * ((player.Sensitivity + player.Corruption + player.Libido) / 100));

            if (lust > 10)
                context.Append("Combat.RandomSuccubus.GrappleTease.ThisLewdEmbrace".Localized());

            player.TakeLustDamage(lust, true);
        }

        // 原版 hlGrapple()（RandomSuccubus.hx:381-385）
        public void HlGrapple(CombatContext context)
        {
            context.Append("Combat.RandomSuccubus.HlGrapple.TheSuccubusDashes".Localized());

            CreateStatusEffect(Grappling, 0, 0, 0, 0);

            context.Player.CreateStatusEffect(VanillaStatusEffects.Grappled, 0, 0, 1, 0);
        }

        // 原版 hlGrappleTease(turn)（RandomSuccubus.hx:387-417）
        private void HlGrappleTease(CombatContext context, int turn)
        {
            var player = context.Player;

            CreateOrFindStatusEffect(Grappling);
            player.CreateOrFindStatusEffect(VanillaStatusEffects.Grappled).Values[2] += 1;

            var lust = (int)((10 + context.Random.Next(11))
                * ((player.Sensitivity + player.Corruption + player.Libido) / 100));

            switch (turn)
            {
                case 1:
                    return;
                case 2:
                    context.Append("Combat.RandomSuccubus.HlGrappleTease.StillFirmlyGrabbed".Localized());
                    player.TakeLustDamage(lust, true);
                    return;
                case 3:
                    return;
                case 4:
                    context.Append("Combat.RandomSuccubus.HlGrappleTease.TheSuccubusLays".Localized());
                    player.TakeLustDamage(lust + 5, true);
                    return;
                case 5:
                    return;
                case 6:
                    context.Append("Combat.RandomSuccubus.HlGrappleTease.LargeBatLike".Localized());
                    player.TakeLustDamage(lust + 10, true);
                    return;
                default:
                    Struggle(context);
                    return;
            }
        }

        // 原版 defeated()/won() 没有覆写（RandomSuccubus.hx 全文），战斗收尾走通用流程。
        // 原版该类的唯一使用方是 LiddelliumEventDungeon（Contents 全量搜索 LiddelliumEventDungeon = 0 命中，
        // 数据缺失：原版 LiddelliumEventDungeon.hx:265/319）。
    }
}
