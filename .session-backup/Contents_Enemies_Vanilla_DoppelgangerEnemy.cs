using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/dungeons/lethicesKeep/Doppelganger.hx（338 行）。
    // 机制：全身复制玩家的身体 / 属性 / 装备；不普攻，每个回合只「说骚话」并把回合数推进；
    // 玩家攻击会被镜面反击（WhenAttacked）；对 whitefire 免疫并反射（handleSpellResistance）；
    // 玩家等待时它跟着休息（handlePlayerWait）。
    [Register("coc:doppelganger")]
    public class DoppelgangerEnemy : Enemy
    {
        // 原版 Doppelganger.hx:13 `_roundCount`：addTalkShit 每轮 +1，决定说哪段台词。
        private int _roundCount;

        private static readonly Identifier StunnedId = VanillaStatusEffects.Stunned;

        // 原版 new()（Doppelganger.hx:172-270）：把玩家的体型 / 性别 / 装备全部复制一份。
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var player = CoC.Instance.Player;

            Race = "Demon";
            Tallness = player.Tallness;

            Balls = player.Balls;
            BallSize = player.BallSize;
            HoursSinceCum = player.HoursSinceCum;

            // 原版 hips.rating / butt.rating 复制后下限 1（Doppelganger.hx:194-201）
            Body.Get<HipsPart>().Rating = Math.Max(1f, player.Body.Get<HipsPart>().Rating);
            Body.Get<ButtPart>().Rating = Math.Max(1f, player.Body.Get<ButtPart>().Rating);

            // lowerBody.type / skin.type / skin.desc / face.type（Doppelganger.hx:202-211）
            Body.Get<LowerBodyPart>().Id = player.Body.Get<LowerBodyPart>().Id;

            var skin = Body.Get<SkinPart>();
            var playerSkin = player.Body.Get<SkinPart>();
            skin.Id = playerSkin.Id;
            skin.Tone = playerSkin.Tone;
            skin.DescKey = playerSkin.DescKey;

            Body.Get<FacePart>().Id = player.Body.Get<FacePart>().Id;

            // initStrTouSpeInte(player...) / initLibSensCor(player...)，cor 下限 50（Doppelganger.hx:204-208）
            Strength = player.Strength;
            Toughness = player.Toughness;
            Speed = player.Speed;
            Intelligence = player.Intelligence;
            Libido = player.Libido;
            Sensitivity = player.Sensitivity;
            Corruption = Math.Max(player.Corruption, 50f);

            // 装备复制（Doppelganger.hx:215-222）
            WeaponName = player.WeaponName;
            WeaponAttack = player.WeaponAttack;
            WeaponVerb = player.WeaponVerb;

            // normalAttackAvoid.doParry = false; doCounter = false（Doppelganger.hx:218-219）
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            ArmorName = player.ArmorName;
            ArmorDefense = player.ArmorDefense;

            Level = player.Level;

            // ass.analLooseness / analWetness（Doppelganger.hx:226-227）
            var anal = Body.Get<AnalPart>();
            var playerAnal = player.Body.Get<AnalPart>();
            anal.Looseness = playerAnal.Looseness;
            anal.Wetness = playerAnal.Wetness;

            // 阴茎逐个复制（Doppelganger.hx:229-234）
            foreach (var cock in player.Cocks)
                CreateCock(cock.Length, cock.Thickness, cock.Id);

            if (player.Vaginas.Count > 0)
            {
                // 只复制第一个阴部（Doppelganger.hx:236-241）
                var vagina = player.Vaginas[0];

                CreateVagina(vagina.Virgin, vagina.Wetness, vagina.Looseness);
            }
            else if (player.Cocks.Count == 0)
            {
                // 无性别玩家被强制长出一个阴部（Doppelganger.hx:242-248）
                CreateVagina(false, 6, 2);
            }

            // 乳房：先清空再逐行复制（Doppelganger.hx:249-263）
            BreastRows.Clear();

            foreach (var row in player.BreastRows)
            {
                CreateBreastRow(row.BreastRating, row.NipplesPerBreast);

                var created = BreastRows[^1];
                created.Breasts = row.Breasts;
                created.Fuckable = row.Fuckable;
                created.Lactation = row.Lactation;
                created.MilkFullness = row.MilkFullness;
            }

            // bonusHP = 250 由 Assets/Data/Enemies/Doppelganger.hjson 的 bonusHealth 给；
            // 复制完属性后按新的 MaxHealth 补满（原版构造函数末尾 checkMonster() 之前就是满血）。
            Health = MaxHealth;
        }

        // 原版 get_long()（Doppelganger.hx:272-336）：整段描述都照着玩家当前外貌现拼。
        // 这里的 [race] / [bodytype] / [facelong] / [hair] / [ears] / [armor] / [weapon] 等标签
        // 解析的都是玩家（镜像），与原文一致。
        public override string Description
        {
            get
            {
                var player = CoC.Instance.Player;

                var he = Capitalize(player.Mf("he", "she"));
                var his = Capitalize(player.Mf("his", "her"));

                var text = "You are fighting the doppelganger. " + he + " is a "
                    + (int)Math.Floor(player.Tallness / 12) + " foot " + (int)(player.Tallness % 12) + " inch tall "
                    + "[race], with [bodytype]. ";

                text += his + " face has [facelong].";

                text += " " + his + " [hair] is parted by [ears].";

                text += " " + he + " keeps exploring the area around [his] mouth with [his] tongue with a horribly acquisitive, sensual interest.";
                text += " " + he + " moves around on [his] [legs] with a twitchy jerkiness, [his] [hips] swinging and tightening.";

                if (player.Body.Get<TailPart>().Id.Path != "none")
                    text += " " + his + " tail flicks this way and that.";

                text += " " + he + " wields the exact same [weapon] you do, and is dressed in the mirror image of your [armor].";

                if (player.BiggestBreastSize() >= 2)
                {
                    text += " It's difficult not to notice the way the mirror image of your "
                        + player.BreastDescript(player.BiggestTitRow()) + " ebbs and heaves within it.";
                }

                return text;
            }
        }

        private static string Capitalize(string text) =>
            string.IsNullOrEmpty(text) ? text : char.ToUpperInvariant(text[0]) + text[1..];

        // 原版 defeated()（Doppelganger.hx:110-112）→ doppelganger.punchYourselfInTheBalls()
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DoppelgangerScenes>.Get().PunchYourselfInTheBalls();

            return true;
        }

        // 原版 won()（Doppelganger.hx:114-116）→ doppelganger.inSovietCoCSelfFucksYou()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<DoppelgangerScenes>.Get().InSovietCoCSelfFucksYou();

            return true;
        }

        // 原版 handleSpellResistance（Doppelganger.hx:118-131）。
        // 派发点在 CombatAbilities.hx:378（spellWhitefire）/ :2982（spellFireball）。
        public override bool HandleSpellResistance(CombatContext context, Identifier spell)
        {
            context.WriteAction("Scenes.Doppelganger.HandleSpellResistance.BarelyFlinches".Localized());
            context.WriteAction("Scenes.Doppelganger.HandleSpellResistance.YawnsFlame".Localized());

            var player = context.Player;

            switch (spell.Path)
            {
                case "spell_fireball":
                    // 原版 Doppelganger.hx:122-123：player.level * 10 + 45 + rand(10)
                    context.Player.HealthChange(-(player.Level * 10 + 45 + context.Random.Next(10)));
                    break;
                case "spell_whitefire":
                    // 原版 Doppelganger.hx:124-125：10 + int/3 + rand(int/2)
                    context.Player.HealthChange(-(10 + player.Intelligence / 3f
                        + context.Random.Next(Math.Max(1, (int)(player.Intelligence / 2)))));
                    break;
                case "spell_blackfire":
                    // 原版 Doppelganger.hx:126-127：30 + int/3 + rand(int/2)
                    context.Player.HealthChange(-(30 + player.Intelligence / 3f
                        + context.Random.Next(Math.Max(1, (int)(player.Intelligence / 2)))));
                    break;
            }

            AddTalkShit(context);

            // 原版 addTalkShit 末尾置 tookAction = true（Doppelganger.hx:107）：
            // 法术反制之后本回合不再走 doAI，由 CombatController.EnemyTurn 的 TookAction 判定跳过。
            TookAction = true;

            return true;
        }

        // 原版 handlePlayerWait（Doppelganger.hx:133-136）。
        // 派发点在 Combat.hx:764（reactWrapper(PlayerWaited)）→ CombatController.Wait。
        public override bool HandlePlayerWait(CombatContext context)
        {
            // 原版 react(PlayerWaited) 自己先写这句再调 handlePlayerWait（Doppelganger.hx:162-166）。
            context.WriteAction("Scenes.Doppelganger.React.NoAction".Localized());
            context.WriteAction("Scenes.Doppelganger.HandlePlayerWait.WaitsToo".Localized());

            AddTalkShit(context);

            TookAction = true;

            return true;
        }

        // 原版 doAI（Doppelganger.hx:138-147）：眩晕时清掉并白送玩家一轮；否则只说骚话、不出手。
        // 眩晕判定由 CombatController.IsEnemyIncapacitated 先做，所以把原文的眩晕分支放进 HandleStun。
        public override bool HandleStun(CombatContext context)
        {
            context.WriteAction("Scenes.Doppelganger.DoAI.DuplicateStunned".Localized());

            RemoveStatusEffect(StunnedId);

            // 返回 false = 本回合不出手（IsEnemyIncapacitated → 跳过 AI）。
            return false;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            context.WriteAction("Scenes.Doppelganger.DoAI.DuplicateChuckles".Localized());

            AddTalkShit(context);

            return true;
        }

        // 原版 react()（Doppelganger.hx:149-170）的 WhenAttacked 分支：
        // 没被眩晕时，玩家的近战命中会被「镜像」——按玩家的伤害反打回去。
        // 原版是在伤害计算完成后（Combat.hx:985-1002）调用 reactWrapper(WhenAttacked)，
        // C# 的派发点同样在 Avoid 判定之后、DamageEnemy 之前，所以这里按同一公式重算伤害，
        // 再用返回 false 吃掉正常结算（与 DullahanEnemy 的 WhenAttacked 反击同一写法）。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.WhenAttacked)
                return true;

            if (HasStatusEffect(StunnedId))
                return true;

            var damage = CombatCalculator.PhysicalDamage(context, context.Player, this);

            MirrorAttack(context, damage);

            return false;
        }

        // 原版 mirrorAttack（Doppelganger.hx:15-37）。
        private void MirrorAttack(CombatContext context, float damage)
        {
            // 原版这里 createStatusEffect(MirroredAttack)：该状态在 Haxe 里没有任何读取点（纯标记），
            // C# 未定义对应 Identifier，故不产出（见 Doppelganger.hx:16）。

            context.WriteAction("Scenes.Doppelganger.MirrorAttack.SwingWeapon".Localized());

            // 原版判定：damage > 0 && player.inte > rand(140)
            if (damage > 0 && context.Player.Intelligence > context.Random.Next(140))
            {
                context.Append("Scenes.Doppelganger.MirrorAttack.TwistsDownwards".Localized(
                    "#ff0000", (int)Math.Round(damage)));

                HealthChange(-damage);
            }
            else
            {
                context.Append("Scenes.Doppelganger.MirrorAttack.YourWord".Localized());
                context.Append(context.Player.IsUnarmed()
                    ? "Scenes.Doppelganger.MirrorAttack.WeaponWord".Localized()
                    : "Scenes.Doppelganger.MirrorAttack.WeaponsPlural".Localized());
                context.Append("Scenes.Doppelganger.MirrorAttack.BoneJarringImpact".Localized());
                context.Append("Scenes.Doppelganger.MirrorAttack.TryAgainTaunt".Localized());
            }

            AddTalkShit(context);
        }

        // 原版 mirrorTease（Doppelganger.hx:39-55），在 CombatTeases.hx:1765 由命中分支调用。
        // C# 的挑逗没有命中判定（CombatResolver.PlayerTease 必定结算），所以等价于 successful = true 的分支；
        // 基础欲望由基类 Creature.ApplyTease 先加（对应原文的 this.applyTease(damage)），这里只补叙事。
        public override void OnTeased(CombatContext context, float lustDelta)
        {
            context.WriteAction("Scenes.Doppelganger.MirrorTease.HandsSeductive".Localized());
            context.WriteAction("Scenes.Doppelganger.MirrorTease.DisplaysCarnal".Localized());
            context.Append("Scenes.Doppelganger.MirrorTease.ItsBeenSoLong".Localized());

            AddTalkShit(context);
        }

        // 原版 addTalkShit（Doppelganger.hx:57-108）：按 _roundCount 播报，最后 tookAction = true。
        private void AddTalkShit(CombatContext context)
        {
            switch (_roundCount)
            {
                case 0:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.YouFeelIt".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.DullCroon".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.FlutteringArm".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.FinishFastBanner".Localized());
                    break;

                case 1:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.CantRemember".Localized());
                    break;

                case 2:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.ClosesEyes".Localized());

                    if (context.Player.HasCock())
                        context.Append("Scenes.Doppelganger.AddTalkShit.StrokesCock".Localized());
                    else if (context.Player.HasVagina())
                        context.Append("Scenes.Doppelganger.AddTalkShit.FingersVagina".Localized());
                    else
                        context.Append("Scenes.Doppelganger.AddTalkShit.SlipsHand".Localized());

                    context.Append("Scenes.Doppelganger.AddTalkShit.BizarrenessSeen".Localized(ArmorName));
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.HarderToIgnore".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.SomethingRealize".Localized());
                    break;

                case 3:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.MemoriesFlow".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.NotBadTaunt".Localized());
                    break;

                case 4:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.DidYouThink".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.DareNotOpen".Localized());
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.LastGaspBanner".Localized());
                    break;

                case 5:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.ShimmerIntensifies".Localized());

                    // 原版 Doppelganger.hx:98 `game.dynStats(Lust(1000))`
                    context.Player.TakeLustDamage(1000, true);
                    break;

                default:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.HowSurvive".Localized());
                    break;
            }

            _roundCount += 1;

            TookAction = true;
        }
    }
}
