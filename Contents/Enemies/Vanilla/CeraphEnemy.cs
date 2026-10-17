using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 原版 Ceraph.hx:9-166 的战斗 AI 全部实现在本类（BeforeCombatAction + 三个特殊技 + Struggle/React）。
    [Register<CeraphEnemy>("coc:ceraph")]
    public class CeraphEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(10, 2, BodyPartTypes.Demon);

            CreateVagina(false, 5, 5);

            Tallness = 5 * 12 + 6;

            Body.Get<SkinPart>().Tone = "purple";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 20;

            WeaponName = "flaming whip";

            ArmorName = "demon-skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<CeraphScene>.Get().WinRapeChoices();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scene.OutputText("Combat.Ceraph.CameWorms".Localized());

                return false;
            }

            SceneInstance<CeraphScene>.Get().LoseFUCKME();

            return true;
        }

        // ===== 原版 Ceraph.hx:9-166：战斗 AI（4 分支 + 三个特殊技）=====

        public bool _doubleAttackPending;

        /// <summary>原版 Ceraph.hx:9 performCombatAction()</summary>
        public override bool BeforeCombatAction(CombatContext context)
        {
            var player = context.Player;

            // 原版 :11：玩家被捆住时只用鞭技
            if (player.HasStatusEffect(StatusEffectIds.Bound))
            {
                CeraphSpecial2(context);

                return true;
            }

            // 原版 :15：自己在蓄力（Uber）时继续蓄力/释放
            if (HasStatusEffect(StatusEffectIds.Uber))
            {
                CeraphSpecial1(context);

                return true;
            }

            switch (Random.Shared.Next(4))
            {
                case 1:
                    CeraphSpecial1(context);

                    return true;

                case 2:
                    CeraphSpecial2(context);

                    return true;

                case 3:
                    CeraphSpecial3(context);

                    return true;

                default:
                    // 原版 case 0 / default：普通攻击
                    return false;
            }
        }

        /// <summary>原版 Ceraph.hx:12 ceraphSpecial1()：蓄力三回合后放出粉焰（等待可躲）。</summary>
        private void CeraphSpecial1(CombatContext context)
        {
            if (!HasStatusEffect(StatusEffectIds.Uber))
            {
                context.WriteAction(Random.Shared.Next(2) == 0

                    ? "Combat.Ceraph.UberStartA".Localized()

                    : "Combat.Ceraph.UberStartB".Localized());

                context.Append("Combat.Ceraph.BuildingUp".Localized());

                CreateStatusEffect(StatusEffectIds.Uber, 0, 0, 0, 0);

                return;
            }

            // 原版 :24：第二回合继续蓄力
            if ((int)StatusEffectValue(StatusEffectIds.Uber, 1) == 0)
            {
                AddStatusValue(StatusEffectIds.Uber, 1, 1);

                context.WriteAction(Random.Shared.Next(2) == 0

                    ? "Combat.Ceraph.UberChargeA".Localized()

                    : "Combat.Ceraph.UberChargeB".Localized());

                context.Append("Combat.Ceraph.AboutToUnleash".Localized());

                if (context.Player.Intelligence > 50)
                    context.Append("Combat.Ceraph.ShouldWait".Localized());

                return;
            }

            // 原版 :36：第三回合开火
            RemoveStatusEffect(StatusEffectIds.Uber);

            if (context.PlayerWaited)
            {
                context.WriteAction("Combat.Ceraph.UberDodged".Localized());

                return;
            }

            context.WriteAction("Combat.Ceraph.UberHit".Localized());

            // 原版 :44：未躲避直接满淫欲
            context.Player.Lust = context.Player.MaxLust;
        }

        /// <summary>原版 Ceraph.hx:54 ceraphSpecial2()：鞭子束缚 / 被缚时的调情。</summary>
        private void CeraphSpecial2(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.Bound))
            {
                context.WriteAction("Combat.Ceraph.WhipBind".Localized());

                if (SceneInstance<CeraphScene>.Get().HasBondage())
                {
                    context.Append("Combat.Ceraph.BondageTurnsYouOn".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
                }

                player.CreateStatusEffect(StatusEffectIds.Bound, 2 + Random.Shared.Next(5), 0, 0, 0);

                return;
            }

            if (Random.Shared.Next(2) == 0)
            {
                context.WriteAction("Combat.Ceraph.Cuddle".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + (int)(9 + player.Sensitivity / 10f));
            }
            else
            {
                context.WriteAction("Combat.Ceraph.HotKisses".Localized());

                context.Append(player.Lust switch
                {
                    < 33 => "Combat.Ceraph.KissLow".Localized(),

                    < 60 => "Combat.Ceraph.KissMid".Localized(),

                    < 80 => "Combat.Ceraph.KissHigh".Localized(),

                    _ => "Combat.Ceraph.KissMax".Localized(),
                });

                player.Lust = Math.Min(player.MaxLust,

                    player.Lust + (int)(5 + player.Corruption / 10f + player.Libido / 20f));
            }
        }

        /// <summary>原版 Ceraph.hx:136 ceraphSpecial3()：自慰小高潮（高淫欲时）或鞭击双击。</summary>
        private void CeraphSpecial3(CombatContext context)
        {
            // 原版 :138：淫欲 ≥ 75% 时用小高潮代替双击
            if (LustPercent >= 75)
            {
                context.WriteAction("Combat.Ceraph.MiniCum".Localized());

                context.Append("Combat.Ceraph.MiniCumAfter".Localized());

                // 原版 :145：+10 力量/韧性、+1 等级、+20 HP、淫欲归 33、XP +10
                Level += 1;

                Stats.AddModifier(GameStats.Strength, Contents.Stats.ModifierOperation.Addition,

                    () => 10f, new Stats.ModifierSources.TemporarySource(Guid.NewGuid()));

                Stats.AddModifier(GameStats.Toughness, Contents.Stats.ModifierOperation.Addition,

                    () => 10f, new Stats.ModifierSources.TemporarySource(Guid.NewGuid()));

                Stats.AddModifier(GameStats.MaxHealth, Contents.Stats.ModifierOperation.Addition,

                    () => 20f, new Stats.ModifierSources.TemporarySource(Guid.NewGuid()));

                Health = MaxHealth;

                Lust = 33;

                context.Player.Lust = Math.Min(context.Player.MaxLust, context.Player.Lust + 3);

                return;
            }

            context.WriteAction("Combat.Ceraph.WhipDouble".Localized());

            CreateStatusEffect(StatusEffectIds.Attacks, 2, 0, 0, 0);
        }

        /// <summary>原版 Ceraph.hx:98 struggle()：被鞭子捆住时挣脱。</summary>
        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.Bound))
                return;

            context.WriteAction("Combat.Ceraph.StruggleStart".Localized());

            if ((int)StatusEffectValue(StatusEffectIds.Bound, 1) - 1 <= 0)
            {
                context.Append("Combat.Ceraph.StruggleFree".Localized());

                if (SceneInstance<CeraphScene>.Get().HasBondage())
                    context.Append("Combat.Ceraph.StruggleFreeBondage".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Bound);

                return;
            }

            context.Append("Combat.Ceraph.StruggleFail".Localized());

            if (SceneInstance<CeraphScene>.Get().HasBondage())
                context.Append("Combat.Ceraph.StruggleFailBondage".Localized());

            player.AddStatusValue(StatusEffectIds.Bound, 1, -1);

            // 原版 :127：力量高的人挣脱更快
            if (player.Strength > 65 && Random.Shared.Next((int)player.Strength) > 45)
            {
                context.Append("Combat.Ceraph.StruggleLoosen".Localized());

                player.AddStatusValue(StatusEffectIds.Bound, 1, -1);
            }
        }

        /// <summary>原版 Ceraph.hx:118 react(PlayerWaited)：被缚时「等待」也会涨淫欲。</summary>
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.Bound))
                return true;

            context.WriteAction("Combat.Ceraph.WaitWhileBound".Localized());

            if (SceneInstance<CeraphScene>.Get().HasBondage())
            {
                context.Append("Combat.Ceraph.WaitWhileBoundBondage".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
            }

            player.Lust = Math.Min(player.MaxLust,

                player.Lust + (int)(player.Libido / 20f + 5 + Random.Shared.Next(5)));

            return false;
        }

    }
}