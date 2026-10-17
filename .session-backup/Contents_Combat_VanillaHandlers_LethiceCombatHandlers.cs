using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.Utils;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.Lethice 的三阶段招式（Lethice.hx:151-855）。
    // 选招在 LethiceEnemy.ChooseCombatAction，每回合前置在 LethiceEnemy.BeforeCombatAction。
    // 阶段切换：phase1Ends 由 LethiceEnemy.ResolveDefeated 返回 Continue 续战；
    // phase2Ends 停在 LethiceEnemy.CustomCombatMenu 的三选一菜单（Lethice.hx:528-535），
    // 菜单里的插曲 → beginPhase3 → Next 的流程都在本文件下半部分。
    public static class LethiceCombatHandlers
    {
        private static LethiceEnemy Lethice(CombatContext context) => (LethiceEnemy)context.Enemy;

        // 原版伤害公式里都乘 (1 + player.newGamePlusMod() * 0.3)
        private static float Ngp(Player player) => 1f + player.NewGamePlusMod() * 0.3f;

        // 原版 Utils.rand(max:Float) = Std.int(random * max)
        private static int Rand(CombatContext context, float max) => (int)(context.Random.NextDouble() * max);

        // 原版 player.reduceDamage(damage, this, armorIgnore)，C# 的 ReduceDamage 没有破甲参数，
        // 这里按原版公式手算（与 MinotaurKing 的 battleaxe 用法一致）。
        private static float ReduceDamageWithIgnore(CombatContext context, Creature defender, float damage, float armorIgnore)
        {
            var percent = CombatCalculator.DamagePercent(context, defender);

            var reduction = (100f - percent) * (1f - armorIgnore * 0.01f) * 0.01f;

            return damage * (1f - reduction) * CombatStatusModifiers.DamageTakenMultiplier(defender);
        }

        // 原版三处重复的「欲望魔法伤害」：lib/10 + cor/10 + base，中 MinotaurKingsTouch 时 ×1.25
        private static float LustSpellDamage(Player player, float baseValue = 10f)
        {
            var value = player.Libido / 10f + player.Corruption / 10f + baseValue;

            if (player.HasStatusEffect(LethiceEnemy.MinotaurKingsTouchId))
                value *= 1.25f;

            return value;
        }

        // 原版 demonicArouse()（Lethice.hx:161-181）：lib/10 + cor/10 + 25 点欲望，按加完后的 lust100 分档旁白。
        [Register("coc:lethice_demonic_arouse")]
        private static void DemonicArouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Lethice.DemonicArouse.LethiceSHands".Localized());

            player.TakeLustDamage(LustSpellDamage(player, 25f), true);

            if (player.Lust100 <= 30)
                context.Append("Combat.Lethice.DemonicArouse.YouFeelStrangely".Localized());
            else if (player.Lust100 <= 60)
                context.Append("Combat.Lethice.DemonicArouse.BloodRushesTo".Localized());
            else if (player.Lust100 <= 75)
                context.Append("Combat.Lethice.DemonicArouse.DespiteTheDire".Localized());
            else if (player.Lust100 <= 90)
                context.Append("Combat.Lethice.DemonicArouse.TheDarkPower".Localized());
            else if (player.Lust100 < 100)
                context.Append("Combat.Lethice.DemonicArouse.YourMindIs".Localized());
            else
                context.Append("Combat.Lethice.DemonicArouse.YouGiveYourself".Localized());
        }

        // 原版 rapetacles()（Lethice.hx:183-195）：召唤触手，成功/飞行都只挂「未被抓」的版本；
        // 被抓版本把 v3 置 1（表示玩家被束缚，可挣扎/用火烧掉）。
        [Register("coc:lethice_rapetacles")]
        private static void Rapetacles(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Rapetacles.LethiceGesturesAt".Localized());

            if (player.CanFly())
            {
                context.Append("Combat.Lethice.Rapetacles.YouLaughAs".Localized());

                player.CreateStatusEffect(LethiceEnemy.LethicesRapeTentaclesId, 4 + context.Random.Next(2), 0, 0, 0);

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Lethice.Rapetacles.YouManageTo".Localized());

                player.CreateStatusEffect(LethiceEnemy.LethicesRapeTentaclesId, 4 + context.Random.Next(2), 0, 0, 0);
            }
            else
            {
                context.Append("Combat.Lethice.Rapetacles.YouArenT".Localized());

                player.CreateStatusEffect(LethiceEnemy.LethicesRapeTentaclesId, 4 + context.Random.Next(2), 0, 1, 0);
            }
        }

        // 原版 demonfire()（Lethice.hx:280-285）：火焰伤害 = inte + rand(25) × NG+ 系数。
        [Register("coc:lethice_demonfire")]
        private static void Demonfire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Demonfire.LethiceNarrowsHer".Localized());

            // 原版 game.combat.monsterDamageType = DAMAGE_FIRE
            context.MonsterDamageType = new Identifier("coc", "fire");

            var damage = enemy.Intelligence + context.Random.Next(25) * Ngp(player);

            context.DealDamageToPlayer(damage);
        }

        // 原版 wingbuffet()（Lethice.hx:287-296）：1 点伤害 + 失明 2 回合。
        [Register("coc:lethice_wingbuffet")]
        private static void Wingbuffet(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Wingbuffet.LethiceFluttersToward".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Lethice.Wingbuffet.YouManageTo".Localized());

                return;
            }

            context.Append("Combat.Lethice.Wingbuffet.TheCloyingSmoke".Localized());

            player.CreateStatusEffect(VanillaStatusEffects.Blind, 2, 0, 0, 0);

            context.DealDamageToPlayer(1);
        }

        // 原版 claw()（Lethice.hx:298-324）：reduceDamage(eBaseDamage, this, 30)，可闪/可招架/可格挡；
        // 命中超过 5 点伤害时还会尝试割裂（player.bleed）。
        [Register("coc:lethice_claw")]
        private static void Claw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Claw.SwoopingLowThe".Localized());

            var damage = ReduceDamageWithIgnore(context, player, enemy.Strength + enemy.WeaponAttack, 30f);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = true, DoBlock = true
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered" or "Blind")
            {
                context.Append("Combat.Lethice.Claw.YouManageTo".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.Lethice.Claw.MisdirectingHerWith".Localized());
            }
            else if (reason == "Flexibility")
            {
                context.Append("Combat.Lethice.Claw.WithYourFeline".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.Lethice.Claw.YouEvadeHer".Localized());
            }
            else if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Lethice.Claw.YouManageTo2".Localized());
            }
            else if (damage <= 5)
            {
                context.Append("Combat.Lethice.Claw.SheBarelyScratches".Localized());

                context.DealDamageToPlayer(damage);
            }
            else
            {
                context.Append("Combat.Lethice.Claw.DamnThatHurts".Localized());

                context.DealDamageToPlayer(damage);

                if (player.Bleed(enemy))
                    context.Append("Combat.Lethice.Claw.HerSharpClaws".Localized());
            }
        }

        // 原版 demonLustMagic()（Lethice.hx:416-443）：先 rand(100) >= lib/2 抵抗，否则按 lust100 三档吃欲望伤害。
        [Register("coc:lethice_demon_lust_magic")]
        private static void DemonLustMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Lethice.DemonLustMagic.ThoughTheFront".Localized());

            if (context.Random.Next(100) >= player.Libido / 2f)
            {
                context.Append("Combat.Lethice.DemonLustMagic.YouCloseYour".Localized());

                return;
            }

            if (player.Lust100 <= 33)
                context.Append("Combat.Lethice.DemonLustMagic.YouTryYour".Localized());
            else if (player.Lust100 <= 66)
                context.Append("Combat.Lethice.DemonLustMagic.AtFirstYou".Localized());
            else
                context.Append("Combat.Lethice.DemonLustMagic.YouDonT".Localized());

            player.TakeLustDamage(LustSpellDamage(player), true);
        }

        // 原版 dirtyDancing()（Lethice.hx:445-464）：抵抗判定同上，成功则只出旁白，不涨欲望。
        [Register("coc:lethice_dirty_dancing")]
        private static void DirtyDancing(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Lethice.DirtyDancing.TheDemonsClosest".Localized());

            if (context.Random.Next(100) >= player.Libido / 2f)
            {
                context.Append("Combat.Lethice.DirtyDancing.YouPushBack".Localized());

                return;
            }

            if (player.Lust100 <= 33)
                context.Append("Combat.Lethice.DirtyDancing.YouTryAnd".Localized());
            else if (player.Lust100 <= 66)
                context.Append("Combat.Lethice.DirtyDancing.TryAsYou".Localized());
            else
                context.Append("Combat.Lethice.DirtyDancing.OhGodsThe".Localized());

            player.TakeLustDamage(LustSpellDamage(player), true);
        }

        // 原版 hornyPoke()（Lethice.hx:466-483）：伤害 = (70 - rand(tou)) × NG+ 系数。
        [Register("coc:lethice_horny_poke")]
        private static void HornyPoke(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.HornyPoke.SeveralOfThe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered" or "Blind")
            {
                context.Append("Combat.Lethice.HornyPoke.YouDeftlyDodge".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.Lethice.HornyPoke.AtLeastThey".Localized());
            }
            else if (reason == "Flexibility")
            {
                context.Append("Combat.Lethice.HornyPoke.YouContortAnd".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.Lethice.HornyPoke.YouManageTo".Localized());
            }
            else
            {
                var damage = (70f - Rand(context, player.Toughness)) * Ngp(player);

                context.Append("Combat.Lethice.HornyPoke.SeveralOfThe2".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        // 原版 crushingBodies()（Lethice.hx:485-499）：只有 EVASION_EVADE 能完全躲开；
        // 其余情况伤害最低 10，且即使闪避原因不是 Evade 也会结算伤害（原版就是这样写的）。
        [Register("coc:lethice_crushing_bodies")]
        private static void CrushingBodies(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.CrushingBodies.TheSheerWeight".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.DodgeReason == "Evade")
            {
                context.Append("Combat.Lethice.CrushingBodies.YouDropTo".Localized());

                return;
            }

            var damage = (70f - Rand(context, player.Toughness)) * Ngp(player);

            if (damage < 5)
                damage = 10;

            context.Append("Combat.Lethice.CrushingBodies.YouSquirmOut".Localized());

            context.DealDamageToPlayer(damage);
        }

        // 原版 bukkakeTime()（Lethice.hx:501-518）：按闪避原因分五种文案；Evade/未闪开吃 5 欲望，
        // 未闪开还会被糊一脸（失明 2~3 回合）。
        [Register("coc:lethice_bukkake_time")]
        private static void BukkakeTime(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.BukkakeTime.ConsideringHowHalf".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered")
            {
                context.Append("Combat.Lethice.BukkakeTime.YouGrabThe".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.Lethice.BukkakeTime.YouDuckUnder".Localized());
            }
            else if (reason == "Flexibility")
            {
                context.Append("Combat.Lethice.BukkakeTime.YouDoA".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.Lethice.BukkakeTime.YouAtLeast".Localized());

                player.TakeLustDamage(5, true);
            }
            else
            {
                context.Append("Combat.Lethice.BukkakeTime.YouTakeA".Localized());

                player.TakeLustDamage(5, true);

                player.CreateStatusEffect(VanillaStatusEffects.Blind, 2 + context.Random.Next(2), 0, 0, 0);
            }
        }

        // 原版 parasiteThrowingStars()（Lethice.hx:744-777）：
        //   未闪开 → 欲望伤害 + 物理伤害，并按 25% 概率随机让玩家长屌/长胸。
        [Register("coc:lethice_parasite_throwing_stars")]
        private static void ParasiteThrowingStars(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            context.Append("Combat.Lethice.ParasiteThrowingStars.LethiceRetrievesThree".Localized());

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered" or "Blind")
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.TheLivingThrowing".Localized());

                return;
            }

            if (reason == "Evade")
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.YouBarelyAvoid".Localized());

                return;
            }

            if (reason == "Misdirection")
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.YourMisdirectingMovements".Localized());

                return;
            }

            if (reason == "Flexibility")
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.YouBendOver".Localized());

                return;
            }

            player.TakeLustDamage(LustSpellDamage(player), true);

            var damage = enemy.Strength + enemy.WeaponAttack - Rand(context, player.Toughness);

            context.Append("Combat.Lethice.ParasiteThrowingStars.YouCanT".Localized());

            if (player.HasCock() && Rand(context, player.Toughness + 50) < 25)
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.ThereSSuddenly".Localized());

                player.IncreaseEachCock(1);
            }
            else if (player.BiggestBreastSize() <= 0 && Rand(context, player.Toughness + 50) < 25)
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.TheInsideOf".Localized());

                player.GrowTits(3, player.BreastRows.Count, false, 2);
            }
            else if (Rand(context, player.Toughness + 50) < 25)
            {
                context.Append("Combat.Lethice.ParasiteThrowingStars.ThereSSuddenly2".Localized());

                player.GrowTits(2, player.BreastRows.Count, false, 2);
            }

            context.DealDamageToPlayer(damage);
        }

        // 原版 whiptrip()（Lethice.hx:779-804）：
        //   闪开（Speed/Unhindered/Evade/Blind）只吃「小火苗」伤害；没闪开则可能被绊倒（stun），吃满伤害。
        //   注意原版无论是否闪开都会走到最后的 takeDamage。
        [Register("coc:lethice_whiptrip")]
        private static void Whiptrip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Whiptrip.LethiceSlashesHer".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            var minDamage = false;
            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered")
            {
                context.Append("Combat.Lethice.Whiptrip.YouJumpOver".Localized());

                minDamage = true;
            }
            else if (reason is "Evade" or "Blind")
            {
                context.Append("Combat.Lethice.Whiptrip.YouEvadeHer".Localized());

                minDamage = true;
            }
            else
            {
                context.Append("Combat.Lethice.Whiptrip.YouTryTo".Localized());

                // 原版 player.stun(1, 25, Std.int(player.tou))
                if (player.Stun(1, 25, (int)player.Toughness))
                    context.Append("Combat.Lethice.Whiptrip.TheGroundRushes".Localized());
                else
                    context.Append("Combat.Lethice.Whiptrip.LethiceIsGoing".Localized());
            }

            var damage = minDamage
                ? (25f + enemy.WeaponAttack - Rand(context, player.Toughness)) * Ngp(player)
                : (100f + enemy.WeaponAttack + enemy.Strength - Rand(context, player.Toughness)) * Ngp(player);

            context.DealDamageToPlayer(damage);
        }

        // 原版 sonicwhip()（Lethice.hx:806-810）：无法闪避，固定 (75 + weaponAttack + str) × NG+ 系数。
        [Register("coc:lethice_sonicwhip")]
        private static void Sonicwhip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Sonicwhip.LethiceRaisesHer".Localized());

            var damage = (75f + enemy.WeaponAttack + enemy.Strength) * Ngp(player);

            context.DealDamageToPlayer(damage);
        }

        // 原版 whipchoke()（Lethice.hx:812-831）：命中则玩家被鞭子沉默 3 回合。
        [Register("coc:lethice_whipchoke")]
        private static void Whipchoke(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Lethice.Whipchoke.LethiceStrikesOut".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered")
            {
                context.Append("Combat.Lethice.Whipchoke.YouBarelyAvoid".Localized());
            }
            else if (reason is "Evade" or "Blind")
            {
                context.Append("Combat.Lethice.Whipchoke.YouEvadeThe".Localized());
            }
            else if (reason == "Flexibility")
            {
                context.Append("Combat.Lethice.Whipchoke.YouTwistAside".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.Lethice.Whipchoke.RaphaelTaughtYou".Localized());
            }
            else
            {
                context.Append("Combat.Lethice.Whipchoke.YouGaspWhen".Localized());
                context.Append("Combat.Lethice.Whipchoke.YouAreEffectively".Localized());

                var damage = enemy.WeaponAttack + 25 - Rand(context, player.Toughness);

                context.DealDamageToPlayer(damage);

                player.CreateStatusEffect(LethiceEnemy.WhipSilenceId, 3);
            }
        }

        // 原版 gropehands()（Lethice.hx:849-855）：第 5 回合固定放，玩家 +5 欲望并挂 PigbysHands。
        [Register("coc:lethice_gropehands")]
        private static void Gropehands(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Lethice.Gropehands.LethiceMurmursCupping".Localized());
            context.Append("Combat.Lethice.Gropehands.YouGlareDaggers".Localized());

            // 原版 player.takeLustDamage(5) 没传 ignoreResist
            player.TakeLustDamage(5);

            player.CreateStatusEffect(LethiceEnemy.PigbysHandsId);
        }

        // 原版 rapetacleTurn()（Lethice.hx:212-278）：玩家身上有 LethicesRapeTentacles 时每回合结算。
        // 注意原版这里的 dynStats(Lust(...)) 写在 Monster 子类里，按 Haxe 的解析（Creature.dynStats）
        // 是加在**她自己**身上，不是玩家；照抄。
        public static void RapetacleTurn(CombatContext context)
        {
            var player = context.Player;
            var lethice = Lethice(context);

            player.ChangeStatusValue(LethiceEnemy.LethicesRapeTentaclesId, 0, -1);

            if (player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 2) != 0)
            {
                player.ChangeStatusValue(LethiceEnemy.LethicesRapeTentaclesId, 1, 1);

                var tentaround = player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 1);

                if (tentaround == 1)
                {
                    context.Append("Combat.Lethice.RapetacleTurn.TakingAdvantageOf".Localized());

                    lethice.Lust += 5;
                }
                else if (tentaround == 2)
                {
                    context.Append("Combat.Lethice.RapetacleTurn.NowThatThey".Localized());

                    if (player.HasCock())
                        context.Append("Combat.Lethice.RapetacleTurn.TheyTwirlAnd".Localized());

                    if (player.HasVagina())
                        context.Append("Combat.Lethice.RapetacleTurn.OneFlossesYour".Localized());

                    context.Append("Combat.Lethice.RapetacleTurn.0TendrilsEncircle"
                        .Localized(NumberUtils.Num2Text(player.TotalNipples())));

                    if (player.HasFuckableNipples())
                    {
                        context.Append("Combat.Lethice.RapetacleTurn.ThreateningToSlide".Localized());
                    }
                    else
                    {
                        context.Append("Combat.Lethice.RapetacleTurn.PinchingAndTugging".Localized());

                        if (player.IsLactating())
                            context.Append("Combat.Lethice.RapetacleTurn.SqueezingOutSmall".Localized());
                    }

                    context.Append("Combat.Lethice.RapetacleTurn.WorstOfAll".Localized());

                    lethice.Lust += 5;
                }
                else if (tentaround == 3)
                {
                    context.Append("Combat.Lethice.RapetacleTurn.AnotherInkyLength".Localized());

                    lethice.Lust += 5;
                }
                else if (tentaround == 4)
                {
                    context.Append("Combat.Lethice.RapetacleTurn.IfYouThought".Localized());

                    if (player.HasVagina())
                    {
                        context.Append("Combat.Lethice.RapetacleTurn.WhileItsBrother".Localized());

                        if (player.Vaginas.Count > 0 && player.Vaginas[0].Virgin)
                            context.Append("Combat.Lethice.RapetacleTurn.Text".Localized());

                        player.CuntChange(10, false);
                    }

                    context.Append("Combat.Lethice.RapetacleTurn.Text2".Localized());

                    if (player.HasFuckableNipples())
                        context.Append("Combat.Lethice.RapetacleTurn.YourAreSimilarly".Localized());

                    if (player.HasCock())
                        context.Append("Combat.Lethice.RapetacleTurn.AndIsSuddenly".Localized());

                    context.Append("Combat.Lethice.RapetacleTurn.ThereSToo".Localized());

                    lethice.Lust += 10;
                }
                else
                {
                    context.Append("Combat.Lethice.RapetacleTurn.YouVeReally".Localized());

                    context.Append(player.Corruption >= 50
                        ? "Combat.Lethice.RapetacleTurn.HowMuchOf".Localized()
                        : "Combat.Lethice.RapetacleTurn.HowDirtyIt".Localized());

                    lethice.Lust += 10;
                    lethice.Corruption += 1;
                }
            }
            else
            {
                context.Append("Combat.Lethice.RapetacleTurn.TheTentaclesGrab".Localized());

                if (player.CanFly())
                {
                    context.Append("Combat.Lethice.RapetacleTurn.NoMatterHow".Localized());
                }
                else
                {
                    var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                    {
                        Attacker = lethice, Defender = player,
                        DoDodge = true, DoParry = false, DoBlock = false
                    });

                    if (avoid.AttackFailed)
                    {
                        context.Append("Combat.Lethice.RapetacleTurn.YouTwistOut".Localized());
                    }
                    else
                    {
                        context.Append("Combat.Lethice.RapetacleTurn.DamnTheyGot".Localized());

                        player.ChangeStatusValue(LethiceEnemy.LethicesRapeTentaclesId, 2, 1);

                        lethice.Lust += 5;
                    }
                }
            }

            if (player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 0) <= 0)
            {
                context.Append(player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 2) != 0
                    ? "Combat.Lethice.RapetacleTurn.TheTentaclesIn".Localized()
                    : "Combat.Lethice.RapetacleTurn.TheTentaclesHolding".Localized());

                player.RemoveStatusEffect(LethiceEnemy.LethicesRapeTentaclesId);
            }

            context.Append("Combat.Lethice.RapetacleTurn.Text3".Localized());
        }

        // 原版 struggle()（Lethice.hx:326-330）：被触手抓住时挣扎必定失败，只出一句；
        // 原版末尾 game.combat.startMonsterTurn() 在 C# 里由 CombatController.Struggle() 之后的
        // AfterPlayerAction() 自然推进，不需要额外调用。
        public static void Struggle(CombatContext context)
        {
            context.Clear();

            context.Append("Combat.Lethice.Struggle.YouPullWith".Localized());
        }

        // 原版 grappleWait()（Lethice.hx:332-336）：被抓住时选「等待」。
        // C# 的战斗菜单还没有 Lethice 专属的 Wait 按钮（原版 Combat.hx:579-584 那条），所以这个方法
        // 目前没有调用点；文本与状态逻辑先备好。
        public static void GrappleWait(CombatContext context)
        {
            context.Clear();

            context.Append("Combat.Lethice.GrappleWait.YouCanT".Localized());
        }

        // 原版 dispellRapetacles()（Lethice.hx:197-210）：玩家用白焰烧掉触手。
        // 原版由 Combat 菜单的 Dispel 按钮调用（Combat.hx:530-533 / 579-584），C# 还没有这个按钮；
        // 方法本身完整移植（含疲劳、法术计数、精通经验），只少了末尾的 startMonsterTurn。
        public static void DispellRapetacles(CombatContext context)
        {
            var player = context.Player;

            context.Clear();

            if (player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 2) == 0)
                context.Append("Combat.Lethice.DispellRapetacles.YouRaiseYour".Localized());
            else
                context.Append("Combat.Lethice.DispellRapetacles.YouStruggleAnd".Localized());

            context.Append("Combat.Lethice.DispellRapetacles.SprayForthA".Localized());

            player.RemoveStatusEffect(LethiceEnemy.LethicesRapeTentaclesId);

            // 原版 player.changeFatigue(30, 1)：第二个参数 1 = 魔法疲劳
            player.ChangeFatigue(30);

            CoC.Instance.Flags.Set(VanillaFlags.SpellsCast, CoC.Instance.Flags.Get(VanillaFlags.SpellsCast) + 1);

            player.MasteryXP(new Identifier("coc", "Casting"), 2 + context.Random.Next(7));
        }

        // 原版 phase3() 的前半段（Lethice.hx:700-731）：
        //   按上回合「掉血 vs 涨欲望」决定防御姿态：涨欲更快 → 姿态 2（不防物理、免疫挑逗），
        //   否则姿态 1（速度/护甲 400、欲望易感 1.125）。
        public static void UpdatePhase3Stats(CombatContext context)
        {
            var lethice = Lethice(context);

            var hpLost = lethice.HpLastTurn - (int)lethice.Hp100;
            var lustGained = (int)lethice.Lust100 - lethice.LustLastTurn;

            lethice.DefMode = lustGained > hpLost ? 2 : 1;

            lethice.HpLastTurn = (int)lethice.Hp100;
            lethice.LustLastTurn = (int)lethice.Lust100;

            if (lethice.DefMode == 1)
            {
                context.Append("Combat.Lethice.Phase3.LethiceDabsAt".Localized());

                lethice.Speed = 400;
                lethice.ArmorDefense = 400;

                if (lethice.ArmorName == "lethicite armor")
                    lethice.ArmorDefense += 30;

                lethice.LustVulnerability = 1.125f;
            }
            else
            {
                context.Append("Combat.Lethice.Phase3.ShudderingTheDemon".Localized());

                lethice.Speed = 80;
                lethice.ArmorDefense = 14;

                if (lethice.ArmorName == "lethicite armor")
                    lethice.ArmorDefense += 30;

                lethice.LustVulnerability = 0;
            }
        }

        // 原版 phase1Ends()（Lethice.hx:338-404）：第一阶段战败演出。
        // 由 LethiceEnemy.ResolveDefeated 在「阶段 1 战败」时调用，随后派发层走 Continue（不发奖励、重置战斗、进敌人回合）。
        // 原版末尾 startMonsterTurn() 的作用由派发层的 ContinueCombat 承担；这里只置 TookAction=true 消费掉紧接的那次敌人行动（Lethice.hx:401）。
        // 注意：复数/代词切换（Lethice.hx:390-393）在 C# 做不到（Plural/Pronoun1-3 来自数据表）。
        public static void Phase1Ends(CombatContext context, bool hpVictory)
        {
            var player = context.Player;
            var lethice = Lethice(context);

            context.Clear();

            if (hpVictory)
            {
                if (player.CanFly())
                {
                    context.WriteAction("Combat.Lethice.Phase1Ends.EvenHeldAloft".Localized());
                    context.Append("Combat.Lethice.Phase1Ends.TakingWingYourself".Localized());
                }
                else
                {
                    context.WriteAction("Combat.Lethice.Phase1Ends.EvenHeldAloft2".Localized());

                    // 原版先看 game.combat.lastAbilityUsed.isMagic() / isBow，再看武器类型。
                    // C# 用 CurrAbilityUsed（玩家最近一次技能）+ 武器判断替代。
                    var magicAbility = CombatController.Instance?.CurrAbilityUsed is { } used &&
                        CombatAbilityRegistry.Get(used)?.Type == CombatAbilityType.Magical;

                    var magic = magicAbility || player.Weapon.IsMagicStaff();
                    var bow = player.Weapon.IsBow() || player.Weapon.IsCrossbow();
                    var firearm = player.Weapon.IsFirearm();

                    if (!magic && !bow && !firearm && !player.Weapon.IsRanged())
                    {
                        context.Append("Combat.Lethice.Phase1Ends.WatchAsHer".Localized());
                    }
                    else
                    {
                        if (magic)
                        {
                            // 原版还分 usingMagicTF()（绿焰）；C# 没有这个判定，统一走普通魔法分支
                            context.Append("Combat.Lethice.Phase1Ends.ChargeYourWith".Localized());
                        }
                        else
                        {
                            if (bow)
                                context.Append("Combat.Lethice.Phase1Ends.DrawYourBowstring".Localized());
                            else if (firearm)
                                context.Append("Combat.Lethice.Phase1Ends.LineUpYour".Localized());
                            else
                                context.Append("Combat.Lethice.Phase1Ends.UnleashAFinal".Localized());

                            context.Append("Combat.Lethice.Phase1Ends.SendingTheMissile".Localized());
                        }

                        context.Append("Combat.Lethice.Phase1Ends.AndRightInto".Localized());
                    }

                    context.Append("Combat.Lethice.Phase1Ends.SheSDown".Localized());
                }
            }
            else
            {
                context.WriteAction("Combat.Lethice.Phase1Ends.UnableToResist".Localized());
            }

            context.WriteAction("Combat.Lethice.Phase1Ends.GameSheShouts".Localized());
            context.WriteAction("Combat.Lethice.Phase1Ends.YouLookUp".Localized());

            // 原版这里显式 HP = maxHP() / lust = 10（Lethice.hx:385-386），是阶段切换的一部分；
            // 派发层的 ContinueCombat 只做兜底（Health<=0 补满、Lust>=max 清零），不覆盖这两条显式赋值。
            lethice.Health = lethice.MaxHealth;
            lethice.Lust = 10;
            lethice.FightPhase = 2;
            lethice.ArticleOverride = "the ";
            lethice.ShortName = "demons";

            if (lethice.HasStatusEffect(VanillaStatusEffects.PhysicalDisabled))
                lethice.RemoveStatusEffect(VanillaStatusEffects.PhysicalDisabled);

            if (lethice.HasStatusEffect(VanillaStatusEffects.AttackDisabled))
                lethice.RemoveStatusEffect(VanillaStatusEffects.AttackDisabled);

            lethice.RemoveStatusEffect(LethiceEnemy.PermaFlyStatusId);

            lethice.TookAction = true;
        }

        // 原版 phase2Ends()（Lethice.hx:520-536）：第二阶段战败演出 + 三选一菜单
        // （DemonFuck / Heal / Next，Lethice.hx:528-535）。
        // 菜单由 LethiceEnemy.CustomCombatMenu 在 CombatController.Menu() 里接管；这里只置状态位，
        // 并消费掉 ContinueCombat 紧接着跑的那次敌人行动（原版 defeated() 之后停在菜单，不会让敌人再出手）。
        public static void Phase2Ends(CombatContext context, bool hpVictory)
        {
            var lethice = Lethice(context);

            context.Clear();

            context.WriteAction(hpVictory
                ? "Combat.Lethice.Phase2Ends.TheLastOf".Localized()
                : "Combat.Lethice.Phase2Ends.LethiceSMinions".Localized());

            context.WriteAction("Combat.Lethice.Phase2Ends.WhileTheDemons".Localized());

            lethice.P2VictoryWasHp = hpVictory;
            lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.Top;

            // EnemyTurn 末尾会自动复位 TookAction，所以这里置 true 只影响紧接着的那一次行动。
            lethice.TookAction = true;
        }

        // 原版 p2DemonFuck()（Lethice.hx:538-573）：插曲第一段，然后出子菜单
        // （OralFinish / FuckDemon / RideCock，Lethice.hx:566-573）。
        public static void P2DemonFuck(CombatContext context, CombatController controller)
        {
            var player = context.Player;
            var lethice = Lethice(context);

            context.Clear();

            // 原版 images.showImage("monster-demonmob")（Lethice.hx:540）：
            // C# 没有这张贴图（TextureManager 无对应 id），Scene.ShowImage 自带 null 保护 → no-op（贴图数据缺失）。
            Scene.ShowImage("monster-demonmob");

            context.WriteAction("Combat.Lethice.P2DemonFuck.RatherThanAdvance".Localized());

            context.Append(lethice.P2VictoryWasHp
                ? "Combat.Lethice.P2DemonFuck.LyingStunnedAnd".Localized()
                : "Combat.Lethice.P2DemonFuck.WhoVeFallen".Localized());

            context.Append("Combat.Lethice.P2DemonFuck.OnTheFloor".Localized());

            context.Append(player.HasCock()
                ? "Combat.Lethice.P2DemonFuck.Cock".Localized()
                : "Combat.Lethice.P2DemonFuck.Cunt".Localized());

            context.WriteAction("Combat.Lethice.P2DemonFuck.ShePurrsBlack".Localized());
            context.WriteAction("Combat.Lethice.P2DemonFuck.YouGrowlFor".Localized());

            context.Append(player.HasCock()
                ? "Combat.Lethice.P2DemonFuck.WrapAroundYour".Localized()
                : "Combat.Lethice.P2DemonFuck.LapAcrossThe".Localized());

            context.Append("Combat.Lethice.P2DemonFuck.ShuddersOfPleasure".Localized());

            context.Append(player.HasCock()
                ? "Combat.Lethice.P2DemonFuck.SuckingCock".Localized()
                : "Combat.Lethice.P2DemonFuck.MunchingRug".Localized());

            context.WriteAction("Combat.Lethice.P2DemonFuck.AroundYouSpurred".Localized());

            // 原版 game.output.menu() + 三个按钮（Lethice.hx:567-572）
            lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.DemonFuckSub;
            controller.ShowMenu();
        }

        // 原版 oralFinish()（Lethice.hx:575-588）：口交插曲 → orgasm('Generic') → beginPhase3(true)。
        public static void OralFinish(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            context.Clear();

            if (player.HasCock())
            {
                // 原版 images.showImage("lethice-oral-male")（Lethice.hx:578，贴图数据缺失 → no-op）
                Scene.ShowImage("lethice-oral-male");

                context.WriteAction("Combat.Lethice.OralFinish.YouIgnoreThe".Localized());
            }
            else
            {
                // 原版 images.showImage("lethice-oral-female")（Lethice.hx:582，贴图数据缺失 → no-op）
                Scene.ShowImage("lethice-oral-female");

                context.WriteAction("Combat.Lethice.OralFinish.YouDisregardThe".Localized());
            }

            context.WriteAction("Combat.Lethice.OralFinish.SatedYouGive".Localized());

            player.Orgasm("Generic");

            BeginPhase3(context, controller, true);
        }

        // 原版 fuckDemon()（Lethice.hx:590-619）：插入插曲 → orgasm('Generic') → beginPhase3(true)。
        public static void FuckDemon(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            context.Clear();

            // 原版 images.showImage("lethice-plow-her-pussy")（Lethice.hx:592，贴图数据缺失 → no-op）
            Scene.ShowImage("lethice-plow-her-pussy");

            context.WriteAction("Combat.Lethice.FuckDemon.TheDemonsUrging".Localized());
            context.WriteAction("Combat.Lethice.FuckDemon.YouLungeOn".Localized());

            context.Append(player.HasFuckableNipples()
                ? "Combat.Lethice.FuckDemon.UntilYouGive".Localized()
                : "Combat.Lethice.FuckDemon.AndSmearingYour".Localized());

            context.Append("Combat.Lethice.FuckDemon.YouGrabHer".Localized());

            context.WriteAction("Combat.Lethice.FuckDemon.SuddenlyYourEntire".Localized());

            context.Append(player.Balls > 0
                ? "Combat.Lethice.FuckDemon.AndAroundYour".Localized()
                : "Combat.Lethice.FuckDemon.DownToThe".Localized());

            context.Append("Combat.Lethice.FuckDemon.AnotherDemonessSnuggles".Localized());

            context.WriteAction("Combat.Lethice.FuckDemon.WithMoreAnd".Localized());

            context.Append(player.HasFuckableNipples()
                ? "Combat.Lethice.FuckDemon.RightIntoYour".Localized()
                : "Combat.Lethice.FuckDemon.ThatSplattersAll".Localized());

            context.Append("Combat.Lethice.FuckDemon.YouGiveHer".Localized());

            context.WriteAction("Combat.Lethice.FuckDemon.SheGigglesRunning".Localized());
            context.WriteAction("Combat.Lethice.FuckDemon.ShudderingAtThe".Localized());

            player.Orgasm("Generic");

            BeginPhase3(context, controller, true);
        }

        // 原版 rideCock()（Lethice.hx:621-650）：骑乘插曲 → orgasm('VaginalAnal') → beginPhase3(true)。
        public static void RideCock(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            context.Clear();

            context.WriteAction("Combat.Lethice.RideCock.ConsideringTheDemons".Localized());
            context.WriteAction("Combat.Lethice.RideCock.YouGiveThe".Localized());
            context.WriteAction("Combat.Lethice.RideCock.DamnRightYou".Localized());

            // 原版按有没有阴道选两张贴图之一（Lethice.hx:626-630，贴图数据缺失 → no-op）
            if (player.HasVagina())
                Scene.ShowImage("lethice-phase-female-ride");
            else
                Scene.ShowImage("lethice-phase-male-ride");

            context.WriteAction("Combat.Lethice.RideCock.ItTakesYou".Localized());
            context.WriteAction("Combat.Lethice.RideCock.SuddenlyYourEntire".Localized());

            context.Append(player.HasVagina()
                ? "Combat.Lethice.RideCock.LickingAtYour".Localized()
                : "Combat.Lethice.RideCock.ReachingAroundTo".Localized());

            context.WriteAction("Combat.Lethice.RideCock.WithMoreAnd".Localized());
            context.WriteAction("Combat.Lethice.RideCock.TheDemonessIsn".Localized());

            context.Append(player.HasVagina()
                ? "Combat.Lethice.RideCock.Womb".Localized()
                : "Combat.Lethice.RideCock.Bowels".Localized());

            context.Append("Combat.Lethice.RideCock.YouOnlyBarely".Localized());

            context.WriteAction("Combat.Lethice.RideCock.TheDemonPurrs".Localized());
            context.WriteAction("Combat.Lethice.RideCock.YouShoveThe".Localized());

            player.Orgasm("VaginalAnal");

            BeginPhase3(context, controller, true);
        }

        // 原版 beginPhase3()（Lethice.hx:674-698）：切到第三阶段并回满血/欲望 10，按 NG+ 换装。
        // 这里的 HP / Lust 是原版显式赋值（Lethice.hx:676-677），不是重复派发层的兜底复原。
        // doLethNext = true → 原版 game.output.menu() + Next（Lethice.hx:691-693），交给 CustomCombatMenu 渲染；
        // doLethNext = false → 原版 tookAction=true + startMonsterTurn()（Lethice.hx:695-696）。
        public static void BeginPhase3(CombatContext context, CombatController controller, bool doLethNext)
        {
            var lethice = Lethice(context);

            lethice.FightPhase = 3;
            lethice.Health = lethice.MaxHealth;
            lethice.Lust = 10;
            lethice.DefMode = 1;
            lethice.HpLastTurn = (int)lethice.Hp100;
            lethice.LustLastTurn = (int)lethice.Lust100;
            lethice.ArticleOverride = "";
            lethice.ShortName = "Lethice";

            if (CoC.Instance.Player.NewGamePlusMod() >= 1 && lethice.ArmorName != "lethicite armor")
            {
                lethice.ArmorName = "lethicite armor";
                lethice.ArmorDefense += 30;
            }

            if (doLethNext)
            {
                lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.Phase3Next;
                controller.ShowMenu();
            }
            else
            {
                // 原版这里是「空敌人回合（tookAction）→ combatRoundOver(newRound) → 玩家菜单」。
                // 派发层的 ContinueCombat 已经把空敌人回合跑掉了，所以这里直接回到玩家菜单。
                lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.None;
                lethice.TookAction = false;
                controller.ShowMenu();
            }
        }

        // 原版 p2Next()（Lethice.hx:663-672）：第二阶段后的「继续」——Lethice 起身、亮鞭，进入第三阶段。
        public static void P2Next(CombatContext context, CombatController controller)
        {
            var lethice = Lethice(context);

            context.Clear();

            context.WriteAction("Combat.Lethice.P2Next.LethiceGrowlsRising".Localized());

            if (lethice.WingsDestroyed)
                context.Append("Combat.Lethice.P2Next.Tattered".Localized());

            context.Append("Combat.Lethice.P2Next.DraconicWingsBehind".Localized());
            context.WriteAction("Combat.Lethice.P2Next.SheSnarlsThrowing".Localized());

            BeginPhase3(context, controller, false);
        }

        // 原版 p2Heal()（Lethice.hx:652-661）：第二阶段间隙的自我治疗，然后进 beginPhase3(true) 的 Next 画面。
        public static void P2Heal(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            context.Clear();

            context.WriteAction("Combat.Lethice.P2Heal.DrawingOnYour".Localized());

            var heal = (int)((player.Level + player.Intelligence / 1.5f + context.Random.Next((int)player.Intelligence)) *
                player.SpellMod()) * 2;

            if (player.ArmorName == "skimpy nurse's outfit")
                heal = (int)(heal * 1.2f);

            player.HealthChange(heal);

            BeginPhase3(context, controller, true);
        }
    }
}
