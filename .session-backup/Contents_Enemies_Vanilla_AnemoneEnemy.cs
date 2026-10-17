using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Linq;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Anemone extends Monster（src/classes/scenes/npcs/Anemone.hx）
    // 招式见 Contents/Combat/VanillaHandlers/AnemoneCombatHandlers.cs。
    //
    // won/defeated 不覆写：原版 Anemone.hx:33 的 defeated 调用 game.anemoneScene.defeatAnemone()、
    // 原版 Anemone.hx:37 的 won 调用 game.anemoneScene.loseToAnemone()（pcCameWorms 走通用力竭战败）。
    // C# 侧缺 AnemoneScene（全量搜索 `class \w*Anemone` 0 命中；`AnemoneScene` 只在
    // IzmaScene.cs:4041 的注释里出现），胜负分支无处可接，等场景落地后回填。
    [Register("coc:anemone")]
    public class AnemoneEnemy : Enemy
    {
        // 原版 AnemoneVenomDebuff（classes/statusEffects/combat/AnemoneVenomDebuff.hx）：
        // applyEffect(str) → host.takeLustDamage(2 * str) + buffHost(Str(-str), Spe(-str))，可叠加。
        // C# 没有这个状态定义，用「一个来源 + 累计值」的修饰层实现，并登记到脱战清理表。
        public const string VenomSource = "AnemoneVenom";

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            CreateCock(7, 1, VanillaBodyPartTypes.Anemone);

            CreateVagina(false, 3, 3);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 5);

            CreateBreastRow(VanillaBreastCups.B);

            // ass.analLooseness = Ass.LOOSENESS_NORMAL(2) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 2;
            anal.Wetness = 0;

            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 10);

            // tallness = 5 * 12 + 5
            Tallness = 65;

            // hips.rating = Hips.RATING_CURVY(10) / butt.rating = Butt.RATING_NOTICEABLE(6)
            Body.Hips = 10;
            Body.Butt = 6;

            Body.Get<SkinPart>().Tone = "purple";

            var hair = Body.Get<HairPart>();
            hair.Color = "purplish-black";
            hair.Length = 20;
            // hair.type = Hair.ANEMONE
            hair.Id = VanillaBodyPartTypes.Anemone;

            // initStrTouSpeInte(40, 20, 40, 50)
            Strength = 40;
            Toughness = 20;
            Speed = 40;
            Intelligence = 50;

            // initLibSensCor(55, 35, 50)
            Libido = 55;
            Sensitivity = 35;
            Corruption = 50;

            WeaponName = "tendrils";
            WeaponVerb = "tentacle";
            WeaponAttack = 5;

            // 原版 Anemone.hx:163-164：普通攻击不可招架 / 不可被反击
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            ArmorName = "clammy skin";

            // bonusHP = 120：走数据表 bonusHealth（base.Configure 已经加进最大生命）

            Lust = 30;
            LustVulnerability = 0.9f;

            // 原版 Anemone.hx:169 temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：死字段，
            // AI 权重由 Assets/Data/Combat/Ai/Anemone.hjson 表达。

            Level = 4;
            Gems = Random.Shared.Next(5) + 1;

            // 原版 Anemone.hx:172 drop = new WeightedDrop(consumables.DRYTENT, 1)
            // → Assets/Data/Enemies/Anemone.hjson 的 loot（coc:dried_tentacle）
        }

        // 原版 Anemone.hx:46 react(WhenAttacked)：30% 概率装可怜（cor < 75 时直接打断玩家这次攻击）。
        // 原版 Anemone.hx:72 react(AfterAttacked)：近战命中她时被触手反蜇（派发点 CombatResolver.cs:66-68）。
        // 原版两条都要求 !game.combat.isWieldingRangedWeapon()：远程武器吃不到这套，
        // 这里用 Player.IsWieldingRangedWeapon()（原版 Combat.hx:1144-1150）判定。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.AfterAttacked)
            {
                if (context.Player.IsWieldingRangedWeapon())
                    return true;

                // 原版条件还带 game.combat.damage > 0：AfterAttacked 只在命中路径派发，
                // 而 CombatCalculator.PhysicalDamage 保底 1 点，所以这里等价于恒真。
                context.Append("Combat.Anemone.React.ThoughYouManaged".Localized());

                ApplyVenom(context, 1 + context.Random.Next(2));

                return true;
            }

            if (reaction != CombatReactionContext.WhenAttacked)
                return true;

            if (context.Player.IsWieldingRangedWeapon())
                return true;

            if (context.Player.StatusEffectValue(VanillaStatusEffects.CounterAB, 0) == 1)
                return true;

            if (context.Random.Next(10) > 2)
                return true;

            context.Append("Combat.Anemone.React.SeeingYourRaised".Localized());

            if (context.Player.Corruption < 75)
            {
                context.Append("Combat.Anemone.React.YouStareInto".Localized());

                // 原版这里把 game.combat.damage 置 0 并返回 false，等于取消这次攻击。
                // 原版还会在 player 有 FirstAttack 时重打一次 attack()；该分支依赖的战斗流程
                // 在 C# 里没有对应入口，先不接。
                return false;
            }

            context.Append("Combat.Anemone.React.ThoughYouLose".Localized());

            return true;
        }

        // 原版 Anemone.hx:84 runCheck()：player.lust100 < 60 时无视逃率直接 runSuccess()。
        // C# 侧由 Enemy.ModifyRunOutcome 承担（CombatController.Run 先掷 EscapeChance 再问敌人是否改写）；
        // 原版 Anemone.hx:88 的 super.runCheck() 其余分支仍由共享 Run() 承担。
        public override bool ModifyRunOutcome(CombatContext context, bool escaped) =>
            escaped || context.Player.Lust < 60;

        // 原版 Anemone.hx:92 runFail()：逃脱失败 → 专属台词 + applyVenom(4 + sens / 20)。
        // C# 的派发点在 CombatController.Run()（对应原版 Monster.hx:1350）；
        // 原版末尾的 startMonsterTurn 由派发层统一换成 EnemyTurn。
        public override void RunFail(CombatContext context)
        {
            // 原版文案含 [if (isSwimming) {flee|run}] 与 [if (!singleleg) {r knees}]：
            // C# 没有游泳 / 单腿判定，这里固定用「陆上 + 双腿」分支（Combat.Anemone.RunFail.Land）。
            context.Append("Combat.Anemone.RunFail.Land".Localized());

            ApplyVenom(context, 4 + context.Player.Sensitivity / 20f);
        }

        // 原版 Anemone.hx:99 runSuccess()：专属逃脱成功台词 + doRunAway。
        // C# 的派发点在 CombatController.Run()（对应原版 Monster.hx:1412），Finish 由派发层负责。
        public override void RunSuccess(CombatContext context)
        {
            // 原版文案含 [if (isSwimming) {swim toward|march up}]：C# 无游泳判定，固定陆上分支。
            context.Append("Combat.Anemone.RunSuccess.Land".Localized());
        }

        // 原版 applyVenom(str = 1)（Anemone.hx:27）：AnemoneVenomDebuff.applyEffect(str)。
        public static void ApplyVenom(CombatContext context, float amount)
        {
            var player = context.Player;

            // host.takeLustDamage(2 * str, true)
            player.TakeLustDamage(2 * amount, true);

            var strengthLoss = CurrentLoss(player, VanillaStats.Strength) + amount;
            var speedLoss = CurrentLoss(player, VanillaStats.Speed) + amount;

            var source = VenomStatusSource();

            player.Stats.RemoveFrom(source);

            player.Stats.AddModifier(VanillaStats.Strength, ModifierOperation.Addition, () => -strengthLoss, source);
            player.Stats.AddModifier(VanillaStats.Speed, ModifierOperation.Addition, () => -speedLoss, source);

            // 登记脱战清理（与 AmilyCombatHandlers 同一手法：0 值不会改变数值）
            CombatCalculator.AddCombatBuff(player, VanillaStats.Strength, 0, VenomSource, VenomStatus());
        }

        private static float CurrentLoss(Creature creature, Identifier stat)
        {
            var source = VenomStatusSource();

            return -creature.Stats.From(source).Where(modifier => modifier.Stat.Id == stat).Sum(modifier => modifier.Resolve());
        }

        // 与 CombatCalculator.AddCombatBuff 内部生成的 id 保持完全一致
        private static StatusSource VenomStatusSource() =>
            new(new Identifier("coc", "combat_buff_" + VenomSource.ToLowerInvariant().Replace(' ', '_')), VenomSource);

        // 原版状态 id（Anemone Venom）；C# 的 VanillaStatusEffects 是共享文件，这里只做联动键用
        private static Identifier VenomStatus() => new("coc", "anemone_venom");
    }
}
