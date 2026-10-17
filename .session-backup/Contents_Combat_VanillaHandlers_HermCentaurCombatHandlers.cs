using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.HermCentaur 的招式（HermCentaur.hx:102-256）。
    // 其中蓄力/自愈/催眠计数等实例状态挂在 HermCentaurEnemy 上（对应原版 _chargingArouse 等字段）。
    public static class HermCentaurCombatHandlers
    {
        // 原版 feminineMusk()（HermCentaur.hx:102-119）：纯挑逗，8 + lib/10 + sens/10 点欲望。
        [Register("coc:herm_centaur_feminine_musk")]
        private static void FeminineMusk(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.HermCentaur.FeminineMusk.TurningAboutThe".Localized());

            if (player.Lust100 <= 50)
            {
                context.Append("Combat.HermCentaur.FeminineMusk.YourGrowsFeverishly".Localized());
            }
            else
            {
                context.Append("Combat.HermCentaur.FeminineMusk.BloodRushesTo".Localized());

                if (player.HasCock())
                    context.Append("Combat.HermCentaur.FeminineMusk.WithTheDesire".Localized());
                else if (player.HasVagina())
                    context.Append("Combat.HermCentaur.FeminineMusk.YourWithThe".Localized());
                else
                    context.Append("Combat.HermCentaur.FeminineMusk.YourAnusUntil".Localized());
            }

            player.TakeLustDamage(8 + player.Libido / 10f + player.Sensitivity / 10f, true);
        }

        // 原版 aphrodisiacSquirt()（HermCentaur.hx:121-164）：
        //   先加 8 + lib/10 + sens/10 点欲望，再按加完之后的 lust100 分档选旁白。
        [Register("coc:herm_centaur_aphrodisiac_squirt")]
        private static void AphrodisiacSquirt(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.HermCentaur.AphrodisiacSquirt.TheCentaurGrabs".Localized());

            player.TakeLustDamage(8 + player.Libido / 10f + player.Sensitivity / 10f, true);

            if (player.Lust100 < 30)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YouCloseYour".Localized());
            }
            else if (player.Lust100 < 40)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YourHeartBeats".Localized());
            }
            else if (player.Lust100 < 50)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YourCheeksColor".Localized());
            }
            else if (player.Lust100 < 60)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YourBlinkBut".Localized());
            }
            else if (player.Lust100 < 70)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YouGroanIn".Localized());

                if (player.HasCock())
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.AwakensFillingItself".Localized());
                else if (player.HasVagina())
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.AWetHeat".Localized());
                else
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.YourTinglesCraving".Localized());
            }
            else if (player.Lust100 < 80)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YouSighAnd".Localized());

                if (player.HasCock())
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.IgnoringTheStiffening".Localized());
                else
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.IgnoringTheIncreasing".Localized());

                context.Append("Combat.HermCentaur.AphrodisiacSquirt.ItSGetting".Localized());
            }
            else if (player.Lust100 < 90)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.PantingFeverishlyYou".Localized());

                if (player.HasCock())
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.PreBeginsTo".Localized());
                else if (player.HasVagina())
                    context.Append("Combat.HermCentaur.AphrodisiacSquirt.AFloodOf".Localized());

                context.Append("Combat.HermCentaur.AphrodisiacSquirt.YouNeedTo".Localized());
            }
            else if (player.Lust100 < 100)
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.MoaningOutLoud".Localized());
            }
            else
            {
                context.Append("Combat.HermCentaur.AphrodisiacSquirt.SinkingToYour".Localized());
            }
        }

        // 原版 arouseSpellCharge()（HermCentaur.hx:166-176）：蓄力一回合，记住当前 lust 供施法时比对。
        [Register("coc:herm_centaur_arouse_spell_charge")]
        private static void ArouseSpellCharge(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;
            var player = context.Player;

            context.Append("Combat.HermCentaur.ArouseSpellCharge.TheCentauressCloses".Localized());

            if (player.Intelligence > 80)
                context.Append("Combat.HermCentaur.ArouseSpellCharge.ATeaseAttack".Localized());

            herm.ChargingArouse = true;
            herm.LustAtChargeStart = herm.Lust;
            herm.ArouseCooldown = 7;
        }

        // 原版 arouseSpellCast()（HermCentaur.hx:178-191）：
        //   注意原版先把 _lustAtChargeStart 置 -1、再拿它做比较，等价于「lust >= 9 就判定被打断」。
        //   这是原版自身的写法（疑似 bug），此处照抄，不做「修正」。
        [Register("coc:herm_centaur_arouse_spell_cast")]
        private static void ArouseSpellCast(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;
            var player = context.Player;

            herm.ChargingArouse = false;
            herm.LustAtChargeStart = -1;

            if (herm.Lust >= herm.LustAtChargeStart + 10f)
            {
                context.Append("Combat.HermCentaur.ArouseSpellCast.ShudderingTheDemoness".Localized());
            }
            else
            {
                context.Append("Combat.HermCentaur.ArouseSpellCast.TheChantingReaches".Localized());

                player.TakeLustDamage(20 + player.Libido / 6f + player.Sensitivity / 6f, true);
            }
        }

        // 原版 hypnoCock()（HermCentaur.hx:193-237）：
        //   每次使用 +1 层 _hypnoCockUses；前 3 段文本只在第一次出现；
        //   抵抗判定 int * (2 / uses) > rand(lib/3 + sens/3 + cor/3)。
        [Register("coc:herm_centaur_hypno_cock")]
        private static void HypnoCock(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;
            var player = context.Player;

            herm.HypnoCockUses += 1;

            if (herm.HypnoCockUses == 1)
            {
                context.Append("Combat.HermCentaur.HypnoCock.ThwackYouStart".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.YouGlanceLower".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.OnlyAfterWatching".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.WellYouCan".Localized());
            }
            else
            {
                context.Append("Combat.HermCentaur.HypnoCock.ThwackSheS".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.SheMusesQuietly".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.YesItS".Localized());
            }

            // 原版 Utils.rand(max:Float) = Std.int(random * max)
            var resistance = player.Intelligence * (2f / herm.HypnoCockUses);

            var threshold = (int)(player.Libido / 3f + player.Sensitivity / 3f + player.Corruption / 3f);

            var roll = (int)(context.Random.NextDouble() * threshold);

            if (resistance > roll)
            {
                var extra = (int)(player.Libido / 20f + player.Sensitivity / 20f);

                player.TakeLustDamage(2 + (int)(context.Random.NextDouble() * extra), true);

                if (player.Lust100 <= 33)
                    context.Append("Combat.HermCentaur.HypnoCock.AWarningThought".Localized());
                else if (player.Lust100 <= 66)
                    context.Append("Combat.HermCentaur.HypnoCock.AQuietVoice".Localized());
                else
                    context.Append("Combat.HermCentaur.HypnoCock.ASimperingVoice".Localized());
            }
            else
            {
                var extra = (int)(player.Libido / 10f + player.Sensitivity / 10f);

                player.TakeLustDamage(20 + 2 * herm.HypnoCockUses + 2 + (int)(context.Random.NextDouble() * extra), true);

                context.Append("Combat.HermCentaur.HypnoCock.DownItBobs".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.FuckSheS".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.TheVoiceAsks".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.YouNod".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.Text".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.AgainYouNod".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.TheVoiceContinues".Localized());
                context.Append("Combat.HermCentaur.HypnoCock.GodsYouWant".Localized());

                if (player.Lust >= player.MaxLust)
                {
                    context.Append("Combat.HermCentaur.HypnoCock.YouWhimperToo".Localized());
                }
                else
                {
                    context.Append("Combat.HermCentaur.HypnoCock.YouStumbleForward".Localized());
                    context.Append("Combat.HermCentaur.HypnoCock.TheEquineDemon".Localized());
                }
            }
        }

        // 原版 gottaCum()（HermCentaur.hx:239-248）：一生一次，把自己的欲望清零，玩家 +15 欲望。
        [Register("coc:herm_centaur_gotta_cum")]
        private static void GottaCum(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;
            var player = context.Player;

            herm.UsedGottaCum = true;

            context.Append("Combat.HermCentaur.GottaCum.SighingTheDemoness".Localized());
            context.Append("Combat.HermCentaur.GottaCum.TheCorruptionFouled".Localized());
            context.Append("Combat.HermCentaur.GottaCum.HundredsOfPounds".Localized());
            context.Append("Combat.HermCentaur.GottaCum.FlushingTheDemoness".Localized());

            herm.Lust = 0;

            player.TakeLustDamage(15, true);
        }

        // 原版 healUp()（HermCentaur.hx:250-256）：一生一次，回满血。
        [Register("coc:herm_centaur_heal_up")]
        private static void HealUp(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;

            herm.UsedHeal = true;

            context.Append("Combat.HermCentaur.HealUp.WipingADrop".Localized());

            herm.Health = herm.MaxHealth;
        }
    }
}
