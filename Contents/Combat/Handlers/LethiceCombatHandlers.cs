using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class LethiceCombatHandlers
    {
        public static LethiceEnemy Lethice(CombatContext context) => (LethiceEnemy)context.Enemy;

        public static float Ngp(Player player) => 1f + player.NewGamePlusMod() * 0.3f;

        public static int Rand(CombatContext context, float max) => (int)(context.Random.NextDouble() * max);

        public static float ReduceDamageWithIgnore(CombatContext context, Creature defender, float damage, float armorIgnore)
        {
            var percent = CombatCalculator.DamagePercent(context, defender);

            var reduction = (100f - percent) * (1f - armorIgnore * 0.01f) * 0.01f;

            return damage * (1f - reduction) * CombatStatusModifiers.DamageTakenMultiplier(defender);
        }

        public static float LustSpellDamage(Player player, float baseValue = 10f)
        {
            var value = player.Libido / 10f + player.Corruption / 10f + baseValue;

            if (player.HasStatusEffect(LethiceEnemy.MinotaurKingsTouchId))
                value *= 1.25f;

            return value;
        }

        [Register("coc:lethice_demonic_arouse")]
        public static void DemonicArouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.Lethice.DemonicArouse.LethiceSHands".Localized());

            player.TakeLustDamage(LustSpellDamage(player, 25f), true);

            if (player.LustPercent <= 30)
                context.AppendLog("Combat.Lethice.DemonicArouse.YouFeelStrangely".Localized());
            else if (player.LustPercent <= 60)
                context.AppendLog("Combat.Lethice.DemonicArouse.BloodRushesTo".Localized());
            else if (player.LustPercent <= 75)
                context.AppendLog("Combat.Lethice.DemonicArouse.DespiteTheDire".Localized());
            else if (player.LustPercent <= 90)
                context.AppendLog("Combat.Lethice.DemonicArouse.TheDarkPower".Localized());
            else if (player.LustPercent < 100)
                context.AppendLog("Combat.Lethice.DemonicArouse.YourMindIs".Localized());
            else
                context.AppendLog("Combat.Lethice.DemonicArouse.YouGiveYourself".Localized());
        }

        [Register("coc:lethice_rapetacles")]
        public static void Rapetacles(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Rapetacles.LethiceGesturesAt".Localized());

            if (player.CanFly())
            {
                context.AppendLog("Combat.Lethice.Rapetacles.YouLaughAs".Localized());

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
                context.AppendLog("Combat.Lethice.Rapetacles.YouManageTo".Localized());

                player.CreateStatusEffect(LethiceEnemy.LethicesRapeTentaclesId, 4 + context.Random.Next(2), 0, 0, 0);
            }
            else
            {
                context.AppendLog("Combat.Lethice.Rapetacles.YouArenT".Localized());

                player.CreateStatusEffect(LethiceEnemy.LethicesRapeTentaclesId, 4 + context.Random.Next(2), 0, 1, 0);
            }
        }

        [Register("coc:lethice_demonfire")]
        public static void Demonfire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Demonfire.LethiceNarrowsHer".Localized());

            context.MonsterDamageType = new Identifier("coc", "fire");

            var damage = enemy.Intelligence + context.Random.Next(25) * Ngp(player);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:lethice_wingbuffet")]
        public static void Wingbuffet(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Wingbuffet.LethiceFluttersToward".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.AppendLog("Combat.Lethice.Wingbuffet.YouManageTo".Localized());

                return;
            }

            context.AppendLog("Combat.Lethice.Wingbuffet.TheCloyingSmoke".Localized());

            player.CreateStatusEffect(StatusEffectIds.Blind, 2, 0, 0, 0);

            context.DealDamageToPlayer(1);
        }

        [Register("coc:lethice_claw")]
        public static void Claw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Claw.SwoopingLowThe".Localized());

            var damage = ReduceDamageWithIgnore(context, player, enemy.Strength + enemy.WeaponAttack, 30f);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = true, DoBlock = true
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered" or "Blind")
                context.AppendLog("Combat.Lethice.Claw.YouManageTo".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.Lethice.Claw.MisdirectingHerWith".Localized());
            else if (reason == "Flexibility")
                context.AppendLog("Combat.Lethice.Claw.WithYourFeline".Localized());
            else if (reason == "Evade")
                context.AppendLog("Combat.Lethice.Claw.YouEvadeHer".Localized());
            else if (avoid.Outcome != AvoidOutcome.Hit)
                context.AppendLog("Combat.Lethice.Claw.YouManageTo2".Localized());
            else if (damage <= 5)
            {
                context.AppendLog("Combat.Lethice.Claw.SheBarelyScratches".Localized());

                context.DealDamageToPlayer(damage);
            }
            else
            {
                context.AppendLog("Combat.Lethice.Claw.DamnThatHurts".Localized());

                context.DealDamageToPlayer(damage);

                if (player.Bleed(enemy))
                    context.AppendLog("Combat.Lethice.Claw.HerSharpClaws".Localized());
            }
        }

        [Register("coc:lethice_demon_lust_magic")]
        public static void DemonLustMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.Lethice.DemonLustMagic.ThoughTheFront".Localized());

            if (context.Random.Next(100) >= player.Libido / 2f)
            {
                context.AppendLog("Combat.Lethice.DemonLustMagic.YouCloseYour".Localized());

                return;
            }

            if (player.LustPercent <= 33)
                context.AppendLog("Combat.Lethice.DemonLustMagic.YouTryYour".Localized());
            else if (player.LustPercent <= 66)
                context.AppendLog("Combat.Lethice.DemonLustMagic.AtFirstYou".Localized());
            else
                context.AppendLog("Combat.Lethice.DemonLustMagic.YouDonT".Localized());

            player.TakeLustDamage(LustSpellDamage(player), true);
        }

        [Register("coc:lethice_dirty_dancing")]
        public static void DirtyDancing(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.Lethice.DirtyDancing.TheDemonsClosest".Localized());

            if (context.Random.Next(100) >= player.Libido / 2f)
            {
                context.AppendLog("Combat.Lethice.DirtyDancing.YouPushBack".Localized());

                return;
            }

            if (player.LustPercent <= 33)
                context.AppendLog("Combat.Lethice.DirtyDancing.YouTryAnd".Localized());
            else if (player.LustPercent <= 66)
                context.AppendLog("Combat.Lethice.DirtyDancing.TryAsYou".Localized());
            else
                context.AppendLog("Combat.Lethice.DirtyDancing.OhGodsThe".Localized());

            player.TakeLustDamage(LustSpellDamage(player), true);
        }

        [Register("coc:lethice_horny_poke")]
        public static void HornyPoke(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.HornyPoke.SeveralOfThe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered" or "Blind")
                context.AppendLog("Combat.Lethice.HornyPoke.YouDeftlyDodge".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.Lethice.HornyPoke.AtLeastThey".Localized());
            else if (reason == "Flexibility")
                context.AppendLog("Combat.Lethice.HornyPoke.YouContortAnd".Localized());
            else if (reason == "Evade")
                context.AppendLog("Combat.Lethice.HornyPoke.YouManageTo".Localized());
            else
            {
                var damage = (70f - Rand(context, player.Toughness)) * Ngp(player);

                context.AppendLog("Combat.Lethice.HornyPoke.SeveralOfThe2".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        [Register("coc:lethice_crushing_bodies")]
        public static void CrushingBodies(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.CrushingBodies.TheSheerWeight".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.DodgeReason == "Evade")
            {
                context.AppendLog("Combat.Lethice.CrushingBodies.YouDropTo".Localized());

                return;
            }

            var damage = (70f - Rand(context, player.Toughness)) * Ngp(player);

            if (damage < 5)
                damage = 10;

            context.AppendLog("Combat.Lethice.CrushingBodies.YouSquirmOut".Localized());

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:lethice_bukkake_time")]
        public static void BukkakeTime(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.BukkakeTime.ConsideringHowHalf".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered")
                context.AppendLog("Combat.Lethice.BukkakeTime.YouGrabThe".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.Lethice.BukkakeTime.YouDuckUnder".Localized());
            else if (reason == "Flexibility")
                context.AppendLog("Combat.Lethice.BukkakeTime.YouDoA".Localized());
            else if (reason == "Evade")
            {
                context.AppendLog("Combat.Lethice.BukkakeTime.YouAtLeast".Localized());

                player.TakeLustDamage(5, true);
            }
            else
            {
                context.AppendLog("Combat.Lethice.BukkakeTime.YouTakeA".Localized());

                player.TakeLustDamage(5, true);

                player.CreateStatusEffect(StatusEffectIds.Blind, 2 + context.Random.Next(2), 0, 0, 0);
            }
        }

        [Register("coc:lethice_parasite_throwing_stars")]
        public static void ParasiteThrowingStars(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            context.AppendLog("Combat.Lethice.ParasiteThrowingStars.LethiceRetrievesThree".Localized());

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered" or "Blind")
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.TheLivingThrowing".Localized());

                return;
            }

            if (reason == "Evade")
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.YouBarelyAvoid".Localized());

                return;
            }

            if (reason == "Misdirection")
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.YourMisdirectingMovements".Localized());

                return;
            }

            if (reason == "Flexibility")
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.YouBendOver".Localized());

                return;
            }

            player.TakeLustDamage(LustSpellDamage(player), true);

            var damage = enemy.Strength + enemy.WeaponAttack - Rand(context, player.Toughness);

            context.AppendLog("Combat.Lethice.ParasiteThrowingStars.YouCanT".Localized());

            if (player.HasCock() && Rand(context, player.Toughness + 50) < 25)
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.ThereSSuddenly".Localized());

                player.IncreaseEachCock(1);
            }
            else if (player.BiggestBreastSize() <= 0 && Rand(context, player.Toughness + 50) < 25)
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.TheInsideOf".Localized());

                player.GrowTits(3, player.BreastRows.Count, false, 2);
            }
            else if (Rand(context, player.Toughness + 50) < 25)
            {
                context.AppendLog("Combat.Lethice.ParasiteThrowingStars.ThereSSuddenly2".Localized());

                player.GrowTits(2, player.BreastRows.Count, false, 2);
            }

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:lethice_whiptrip")]
        public static void Whiptrip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.WhipTrip.LethiceSlashesHer".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var minDamage = false;

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered")
            {
                context.AppendLog("Combat.Lethice.WhipTrip.YouJumpOver".Localized());

                minDamage = true;
            }
            else if (reason is "Evade" or "Blind")
            {
                context.AppendLog("Combat.Lethice.WhipTrip.YouEvadeHer".Localized());

                minDamage = true;
            }
            else
            {
                context.AppendLog("Combat.Lethice.WhipTrip.YouTryTo".Localized());

                if (player.Stun(1, 25, (int)player.Toughness))
                    context.AppendLog("Combat.Lethice.WhipTrip.TheGroundRushes".Localized());
                else
                    context.AppendLog("Combat.Lethice.WhipTrip.LethiceIsGoing".Localized());
            }

            var damage = minDamage

                ? (25f + enemy.WeaponAttack - Rand(context, player.Toughness)) * Ngp(player)

                : (100f + enemy.WeaponAttack + enemy.Strength - Rand(context, player.Toughness)) * Ngp(player);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:lethice_triplestroke")]
        public static void Triplestroke(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Triplestroke.Intro".Localized());

            for (var i = 0; i < 3; i++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy, Defender = player,

                    DoDodge = true, DoParry = false, DoBlock = false
                });

                if (avoid.AttackFailed)
                {
                    context.AppendLog("Combat.Lethice.Triplestroke.Dodge".Localized());

                    continue;
                }

                var damage = ReduceDamageWithIgnore(context, player, enemy.Strength + enemy.WeaponAttack, 0f);

                context.Player.HealthChange(-damage);

                context.Log.Add("Combat.Resolve.EnemyHit".Localized(enemy.ShortName, damage));
            }
        }

        [Register("coc:lethice_sonicwhip")]
        public static void Sonicwhip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Sonicwhip.LethiceRaisesHer".Localized());

            var damage = (75f + enemy.WeaponAttack + enemy.Strength) * Ngp(player);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:lethice_whipchoke")]
        public static void Whipchoke(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Lethice.Whipchoke.LethiceStrikesOut".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Unhindered")
                context.AppendLog("Combat.Lethice.Whipchoke.YouBarelyAvoid".Localized());
            else if (reason is "Evade" or "Blind")
                context.AppendLog("Combat.Lethice.Whipchoke.YouEvadeThe".Localized());
            else if (reason == "Flexibility")
                context.AppendLog("Combat.Lethice.Whipchoke.YouTwistAside".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.Lethice.Whipchoke.RaphaelTaughtYou".Localized());
            else
            {
                context.AppendLog("Combat.Lethice.Whipchoke.YouGaspWhen".Localized());

                context.AppendLog("Combat.Lethice.Whipchoke.YouAreEffectively".Localized());

                var damage = enemy.WeaponAttack + 25 - Rand(context, player.Toughness);

                context.DealDamageToPlayer(damage);

                player.CreateStatusEffect(LethiceEnemy.WhipSilenceId, 3);
            }
        }

        [Register("coc:lethice_gropehands")]
        public static void Gropehands(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.Lethice.Gropehands.LethiceMurmursCupping".Localized());

            context.AppendLog("Combat.Lethice.Gropehands.YouGlareDaggers".Localized());

            player.TakeLustDamage(5);

            player.CreateStatusEffect(LethiceEnemy.PigbysHandsId);
        }

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
                    context.AppendLog("Combat.Lethice.RapetacleTurn.TakingAdvantageOf".Localized());

                    lethice.Lust += 5;
                }
                else if (tentaround == 2)
                {
                    context.AppendLog("Combat.Lethice.RapetacleTurn.NowThatThey".Localized());

                    if (player.HasCock())
                        context.AppendLog("Combat.Lethice.RapetacleTurn.TheyTwirlAnd".Localized());

                    if (player.HasVagina())
                        context.AppendLog("Combat.Lethice.RapetacleTurn.OneFlossesYour".Localized());

                    context.AppendLog("Combat.Lethice.RapetacleTurn.ZeroTendrilsEncircle"
                        .Localized(NumberUtils.Num2Text(player.TotalNipples())));

                    if (player.HasFuckableNipples())
                        context.AppendLog("Combat.Lethice.RapetacleTurn.ThreateningToSlide".Localized());
                    else
                    {
                        context.AppendLog("Combat.Lethice.RapetacleTurn.PinchingAndTugging".Localized());

                        if (player.IsLactating())
                            context.AppendLog("Combat.Lethice.RapetacleTurn.SqueezingOutSmall".Localized());
                    }

                    context.AppendLog("Combat.Lethice.RapetacleTurn.WorstOfAll".Localized());

                    lethice.Lust += 5;
                }
                else if (tentaround == 3)
                {
                    context.AppendLog("Combat.Lethice.RapetacleTurn.AnotherInkyLength".Localized());

                    lethice.Lust += 5;
                }
                else if (tentaround == 4)
                {
                    context.AppendLog("Combat.Lethice.RapetacleTurn.IfYouThought".Localized());

                    if (player.HasVagina())
                    {
                        context.AppendLog("Combat.Lethice.RapetacleTurn.WhileItsBrother".Localized());

                        if (player.Vaginas.Count > 0 && player.Vaginas[0].Virgin)
                            context.AppendLog("Combat.Lethice.RapetacleTurn.Text".Localized());

                        player.CuntChange(10, false);
                    }

                    context.AppendLog("Combat.Lethice.RapetacleTurn.Text".Localized());

                    if (player.HasFuckableNipples())
                        context.AppendLog("Combat.Lethice.RapetacleTurn.YourAreSimilarly".Localized());

                    if (player.HasCock())
                        context.AppendLog("Combat.Lethice.RapetacleTurn.AndIsSuddenly".Localized());

                    context.AppendLog("Combat.Lethice.RapetacleTurn.ThereSToo".Localized());

                    lethice.Lust += 10;
                }
                else
                {
                    context.AppendLog("Combat.Lethice.RapetacleTurn.YouVeReally".Localized());

                    context.AppendLog(player.Corruption >= 50

                        ? "Combat.Lethice.RapetacleTurn.HowMuchOf".Localized()

                        : "Combat.Lethice.RapetacleTurn.HowDirtyIt".Localized());

                    lethice.Lust += 10;

                    lethice.Corruption += 1;
                }
            }
            else
            {
                context.AppendLog("Combat.Lethice.RapetacleTurn.TheTentaclesGrab".Localized());

                if (player.CanFly())
                    context.AppendLog("Combat.Lethice.RapetacleTurn.NoMatterHow".Localized());
                else
                {
                    var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                    {
                        Attacker = lethice, Defender = player,

                        DoDodge = true, DoParry = false, DoBlock = false
                    });

                    if (avoid.AttackFailed)
                        context.AppendLog("Combat.Lethice.RapetacleTurn.YouTwistOut".Localized());
                    else
                    {
                        context.AppendLog("Combat.Lethice.RapetacleTurn.DamnTheyGot".Localized());

                        player.ChangeStatusValue(LethiceEnemy.LethicesRapeTentaclesId, 2, 1);

                        lethice.Lust += 5;
                    }
                }
            }

            if (player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 0) <= 0)
            {
                context.AppendLog(player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 2) != 0

                    ? "Combat.Lethice.RapetacleTurn.TheTentaclesIn".Localized()

                    : "Combat.Lethice.RapetacleTurn.TheTentaclesHolding".Localized());

                player.RemoveStatusEffect(LethiceEnemy.LethicesRapeTentaclesId);
            }

            context.AppendLog("Combat.Lethice.RapetacleTurn.ParagraphBreak".Localized());
        }

        public static void Struggle(CombatContext context)
        {
            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.AppendLog("Combat.Lethice.Struggle.YouPullWith".Localized());
        }

        public static void GrappleWait(CombatContext context)
        {
            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.AppendLog("Combat.Lethice.GrappleWait.YouCanT".Localized());
        }

        public static void DispellRapetacles(CombatContext context)
        {
            var player = context.Player;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            if (player.StatusEffectValue(LethiceEnemy.LethicesRapeTentaclesId, 2) == 0)
                context.AppendLog("Combat.Lethice.DispellRapetacles.YouRaiseYour".Localized());
            else
                context.AppendLog("Combat.Lethice.DispellRapetacles.YouStruggleAnd".Localized());

            context.AppendLog("Combat.Lethice.DispellRapetacles.SprayForthA".Localized());

            player.RemoveStatusEffect(LethiceEnemy.LethicesRapeTentaclesId);

            player.ChangeFatigue(30);

            CoC.Instance.Flags.Set(GameFlagIds.SpellsCast, CoC.Instance.Flags.Get(GameFlagIds.SpellsCast) + 1);

            player.MasteryXP(new Identifier("coc", "Casting"), 2 + context.Random.Next(7));
        }

        public static void UpdatePhase3Stats(CombatContext context)
        {
            var lethice = Lethice(context);

            var hpLost = lethice.HpLastTurn - (int)lethice.HpPercent;

            var lustGained = (int)lethice.LustPercent - lethice.LustLastTurn;

            lethice.DefMode = lustGained > hpLost ? 2 : 1;

            lethice.HpLastTurn = (int)lethice.HpPercent;

            lethice.LustLastTurn = (int)lethice.LustPercent;

            if (lethice.DefMode == 1)
            {
                context.AppendLog("Combat.Lethice.Phase3.LethiceDabsAt".Localized());

                lethice.Speed = 400;

                lethice.ArmorDefense = 400;

                if (lethice.ArmorName == "lethicite armor")
                    lethice.ArmorDefense += 30;

                lethice.LustVulnerability = 1.125f;
            }
            else
            {
                context.AppendLog("Combat.Lethice.Phase3.ShudderingTheDemon".Localized());

                lethice.Speed = 80;

                lethice.ArmorDefense = 14;

                if (lethice.ArmorName == "lethicite armor")
                    lethice.ArmorDefense += 30;

                lethice.LustVulnerability = 0;
            }
        }

        public static void Phase1Ends(CombatContext context, bool hpVictory)
        {
            var player = context.Player;

            var lethice = Lethice(context);

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            if (hpVictory)
            {
                if (player.CanFly())
                {
                    context.Log.Add("Combat.Lethice.Phase1Ends.EvenHeldAloft".Localized());

                    context.AppendLog("Combat.Lethice.Phase1Ends.TakingWingYourself".Localized());
                }
                else
                {
                    context.Log.Add("Combat.Lethice.Phase1Ends.EvenHeldAloft2".Localized());

                    var magicAbility = CombatController.Instance?.CurrAbilityUsed is { } used &&

                        CombatAbilityRegistry.Get(used)?.Type == CombatAbilityType.Magical;

                    var magic = magicAbility || player.Weapon.IsMagicStaff();

                    var bow = player.Weapon.IsBow() || player.Weapon.IsCrossbow();

                    var firearm = player.Weapon.IsFirearm();

                    if (!magic && !bow && !firearm && !player.Weapon.IsRanged())
                        context.AppendLog("Combat.Lethice.Phase1Ends.WatchAsHer".Localized());
                    else
                    {
                        if (magic)

                            context.AppendLog("Combat.Lethice.Phase1Ends.ChargeYourWith".Localized());
                        else
                        {
                            if (bow)
                                context.AppendLog("Combat.Lethice.Phase1Ends.DrawYourBowstring".Localized());
                            else if (firearm)
                                context.AppendLog("Combat.Lethice.Phase1Ends.LineUpYour".Localized());
                            else
                                context.AppendLog("Combat.Lethice.Phase1Ends.UnleashAFinal".Localized());

                            context.AppendLog("Combat.Lethice.Phase1Ends.SendingTheMissile".Localized());
                        }

                        context.AppendLog("Combat.Lethice.Phase1Ends.AndRightInto".Localized());
                    }

                    context.AppendLog("Combat.Lethice.Phase1Ends.SheSDown".Localized());
                }
            }
            else
                context.Log.Add("Combat.Lethice.Phase1Ends.UnableToResist".Localized());

            context.Log.Add("Combat.Lethice.Phase1Ends.GameSheShouts".Localized());

            context.Log.Add("Combat.Lethice.Phase1Ends.YouLookUp".Localized());

            lethice.Health = lethice.MaxHealth;

            lethice.Lust = 10;

            lethice.FightPhase = 2;

            lethice.ArticleOverride = "the ";

            lethice.ShortName = "demons";

            if (lethice.HasStatusEffect(StatusEffectIds.PhysicalDisabled))
                lethice.RemoveStatusEffect(StatusEffectIds.PhysicalDisabled);

            if (lethice.HasStatusEffect(StatusEffectIds.AttackDisabled))
                lethice.RemoveStatusEffect(StatusEffectIds.AttackDisabled);

            lethice.RemoveStatusEffect(LethiceEnemy.PermaFlyStatusId);

            lethice.TookAction = true;
        }

        public static void Phase2Ends(CombatContext context, bool hpVictory)
        {
            var lethice = Lethice(context);

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.Log.Add(hpVictory

                ? "Combat.Lethice.Phase2Ends.TheLastOf".Localized()

                : "Combat.Lethice.Phase2Ends.LethiceSMinions".Localized());

            context.Log.Add("Combat.Lethice.Phase2Ends.WhileTheDemons".Localized());

            lethice.P2VictoryWasHp = hpVictory;

            lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.Top;

            lethice.TookAction = true;
        }

        public static void P2DemonFuck(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            var lethice = Lethice(context);

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            Scene.ShowImage("monster-demonmob");

            context.Log.Add("Combat.Lethice.P2DemonFuck.RatherThanAdvance".Localized());

            context.AppendLog(lethice.P2VictoryWasHp

                ? "Combat.Lethice.P2DemonFuck.LyingStunnedAnd".Localized()

                : "Combat.Lethice.P2DemonFuck.WhoVeFallen".Localized());

            context.AppendLog("Combat.Lethice.P2DemonFuck.OnTheFloor".Localized());

            context.AppendLog(player.HasCock()

                ? "Combat.Lethice.P2DemonFuck.Cock".Localized()

                : "Combat.Lethice.P2DemonFuck.Cunt".Localized());

            context.Log.Add("Combat.Lethice.P2DemonFuck.ShePurrsBlack".Localized());

            context.Log.Add("Combat.Lethice.P2DemonFuck.YouGrowlFor".Localized());

            context.AppendLog(player.HasCock()

                ? "Combat.Lethice.P2DemonFuck.WrapAroundYour".Localized()

                : "Combat.Lethice.P2DemonFuck.LapAcrossThe".Localized());

            context.AppendLog("Combat.Lethice.P2DemonFuck.ShuddersOfPleasure".Localized());

            context.AppendLog(player.HasCock()

                ? "Combat.Lethice.P2DemonFuck.SuckingCock".Localized()

                : "Combat.Lethice.P2DemonFuck.MunchingRug".Localized());

            context.Log.Add("Combat.Lethice.P2DemonFuck.AroundYouSpurred".Localized());

            lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.DemonFuckSub;

            controller.ShowMenu();
        }

        public static void OralFinish(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            if (player.HasCock())
            {

                Scene.ShowImage("lethice-oral-male");

                context.Log.Add("Combat.Lethice.OralFinish.YouIgnoreThe".Localized());
            }
            else
            {

                Scene.ShowImage("lethice-oral-female");

                context.Log.Add("Combat.Lethice.OralFinish.YouDisregardThe".Localized());
            }

            context.Log.Add("Combat.Lethice.OralFinish.SatedYouGive".Localized());

            player.Orgasm("Generic");

            BeginPhase3(context, controller, true);
        }

        public static void FuckDemon(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            Scene.ShowImage("lethice-plow-her-pussy");

            context.Log.Add("Combat.Lethice.FuckDemon.TheDemonsUrging".Localized());

            context.Log.Add("Combat.Lethice.FuckDemon.YouLungeOn".Localized());

            context.AppendLog(player.HasFuckableNipples()

                ? "Combat.Lethice.FuckDemon.UntilYouGive".Localized()

                : "Combat.Lethice.FuckDemon.AndSmearingYour".Localized());

            context.AppendLog("Combat.Lethice.FuckDemon.YouGrabHer".Localized());

            context.Log.Add("Combat.Lethice.FuckDemon.SuddenlyYourEntire".Localized());

            context.AppendLog(player.Balls > 0

                ? "Combat.Lethice.FuckDemon.AndAroundYour".Localized()

                : "Combat.Lethice.FuckDemon.DownToThe".Localized());

            context.AppendLog("Combat.Lethice.FuckDemon.AnotherDemonessSnuggles".Localized());

            context.Log.Add("Combat.Lethice.FuckDemon.WithMoreAnd".Localized());

            context.AppendLog(player.HasFuckableNipples()

                ? "Combat.Lethice.FuckDemon.RightIntoYour".Localized()

                : "Combat.Lethice.FuckDemon.ThatSplattersAll".Localized());

            context.AppendLog("Combat.Lethice.FuckDemon.YouGiveHer".Localized());

            context.Log.Add("Combat.Lethice.FuckDemon.SheGigglesRunning".Localized());

            context.Log.Add("Combat.Lethice.FuckDemon.ShudderingAtThe".Localized());

            player.Orgasm("Generic");

            BeginPhase3(context, controller, true);
        }

        public static void RideCock(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.Log.Add("Combat.Lethice.RideCock.ConsideringTheDemons".Localized());

            context.Log.Add("Combat.Lethice.RideCock.YouGiveThe".Localized());

            context.Log.Add("Combat.Lethice.RideCock.DamnRightYou".Localized());

            if (player.HasVagina())
                Scene.ShowImage("lethice-phase-female-ride");
            else
                Scene.ShowImage("lethice-phase-male-ride");

            context.Log.Add("Combat.Lethice.RideCock.ItTakesYou".Localized());

            context.Log.Add("Combat.Lethice.RideCock.SuddenlyYourEntire".Localized());

            context.AppendLog(player.HasVagina()

                ? "Combat.Lethice.RideCock.LickingAtYour".Localized()

                : "Combat.Lethice.RideCock.ReachingAroundTo".Localized());

            context.Log.Add("Combat.Lethice.RideCock.WithMoreAnd".Localized());

            context.Log.Add("Combat.Lethice.RideCock.TheDemonessIsn".Localized());

            context.AppendLog(player.HasVagina()

                ? "Combat.Lethice.RideCock.Womb".Localized()

                : "Combat.Lethice.RideCock.Bowels".Localized());

            context.AppendLog("Combat.Lethice.RideCock.YouOnlyBarely".Localized());

            context.Log.Add("Combat.Lethice.RideCock.TheDemonPurrs".Localized());

            context.Log.Add("Combat.Lethice.RideCock.YouShoveThe".Localized());

            player.Orgasm("VaginalAnal");

            BeginPhase3(context, controller, true);
        }

        public static void BeginPhase3(CombatContext context, CombatController controller, bool doLethNext)
        {
            var lethice = Lethice(context);

            lethice.FightPhase = 3;

            lethice.Health = lethice.MaxHealth;

            lethice.Lust = 10;

            lethice.DefMode = 1;

            lethice.HpLastTurn = (int)lethice.HpPercent;

            lethice.LustLastTurn = (int)lethice.LustPercent;

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

                lethice.Phase2Menu = LethiceEnemy.Phase2MenuState.None;

                lethice.TookAction = false;

                controller.ShowMenu();
            }
        }

        public static void P2Next(CombatContext context, CombatController controller)
        {
            var lethice = Lethice(context);

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.Log.Add("Combat.Lethice.P2Next.LethiceGrowlsRising".Localized());

            if (lethice.WingsDestroyed)
                context.AppendLog("Combat.Lethice.P2Next.Tattered".Localized());

            context.AppendLog("Combat.Lethice.P2Next.DraconicWingsBehind".Localized());

            context.Log.Add("Combat.Lethice.P2Next.SheSnarlsThrowing".Localized());

            BeginPhase3(context, controller, false);
        }

        public static void P2Heal(CombatContext context, CombatController controller)
        {
            var player = context.Player;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.Log.Add("Combat.Lethice.P2Heal.DrawingOnYour".Localized());

            var heal = (int)((player.Level + player.Intelligence / 1.5f + context.Random.Next((int)player.Intelligence)) *

                player.SpellMod()) * 2;

            if (player.ArmorName == "skimpy nurse's outfit")
                heal = (int)(heal * 1.2f);

            player.HealthChange(heal);

            BeginPhase3(context, controller, true);
        }
    }
}
