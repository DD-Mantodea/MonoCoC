using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<EmberEnemy>("coc:ember")]
    public class EmberEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(3);

            Tallness = 70;

            WeaponName = "claws";

            WeaponVerb = "claw";

            WeaponAttack = 30;

            ArmorName = "scales";

            ArmorDefense = 40;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            if (Lust >= MaxLust * 0.6f)
            {
                SceneInstance<EmberScene>.Get().EmberReactsToLustiness();

                return;
            }

            if (HasStatusEffect(StatusEffectIds.StunCooldown))
            {
                ChangeStatusValue(StatusEffectIds.StunCooldown, 1, -1);

                if (StatusEffectValue(StatusEffectIds.StunCooldown, 1) <= 0)
                    RemoveStatusEffect(StatusEffectIds.StunCooldown);
            }

            var intensity = SceneInstance<EmberScene>.Get().EmberSparIntensity();

            var options = new List<int>();

            if (intensity >= 15 && !HasStatusEffect(StatusEffectIds.StunCooldown))
            {
                options.Add(0);

                options.Add(0);

                options.Add(0);
            }

            if (intensity >= 5)
            {
                options.Add(1);

                options.Add(1);

                options.Add(1);
            }

            options.Add(2);

            options.Add(3);

            options.Add(4);

            options.Add(5);

            switch (options[Random.Shared.Next(options.Count)])
            {
                case 0: DragonForce(context, player); break;
                case 1: DragonBreath(context, player); break;
                case 2:
                case 3: TailSlap(context, player); break;
                case 4:
                case 5: ClawAttack(context, player); break;
            }
        }

        public void ClawAttack(CombatContext context, Player player)
        {
            context.Write("Common.Text.WithAGrowlTheDragonLashes".Localized());

            if (HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(2) == 0)
            {
                context.Write("Common.Text.EmberCompletelyMissesYouWithA".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Scenes.Ember.EmberAttack.ClawsWhistlePast".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack);

            if (damage <= 0)
            {
                context.Write("Common.Text.EmbersClawsScrapeNoisilyButHarmlessly".Localized());

                return;
            }

            context.Write("Scenes.Ember.EmberAttack.ClawsRip".Localized());

            player.HealthChange(-damage);
        }

        public void DragonBreath(CombatContext context, Player player)
        {
            var intensity = SceneInstance<EmberScene>.Get().EmberSparIntensity();

            if (HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(2) == 0)
            {
                context.Write("Common.Text.TheBlindedDragonTracksYouWith".Localized());

                Health = Math.Max(0, Health - 50);

                return;
            }

            context.Write("Common.Text.EmberInhalesDeeplyThenHerJaws".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Common.Text.ItsANarrowThingButYou".Localized());

                return;
            }

            context.Write("Common.Text.ThePainAsTheDeadlyCombination".Localized());

            float damage = 100 + Random.Shared.Next(100) + intensity * 4;

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            player.HealthChange(-damage);
        }

        public void TailSlap(CombatContext context, Player player)
        {
            if (HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Write("Common.Text.EmberCompletelyMissesYouWithA2".Localized());

                return;
            }

            context.Write("Common.Text.EmberSuddenlySpinsOnHerHeel".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed || Random.Shared.Next(2) == 0)
            {
                context.Write("Common.Text.YouDuckUnderTheTailAt".Localized());

                return;
            }

            var damage = Strength + WeaponAttack + 100 + SceneInstance<EmberScene>.Get().EmberSparIntensity() * 2;

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            context.Write("Common.Text.TheTailSlamsIntoYouWith".Localized());

            player.HealthChange(-damage);
        }

        public void DragonForce(CombatContext context, Player player)
        {
            context.Write("Common.Text.EmberBaresHerTeethAndReleases".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Common.Text.YouQuicklyManageToJumpOut".Localized());

                return;
            }

            context.Write("Scenes.Ember.DragonFarce.BlastHits".Localized());

            var intensity = SceneInstance<EmberScene>.Get().EmberSparIntensity();

            if (player.Stun(0, intensity >= 30 ? 33 : 0))
                context.Write("Scenes.Ember.DragonFarce.BalanceLost".Localized());

            var cooldown = 4;

            if (intensity >= 30) cooldown--;
            if (intensity >= 45) cooldown--;
            CreateStatusEffect(StatusEffectIds.StunCooldown, cooldown, 0, 0, 0);

            var damage = 10 + Random.Shared.Next(10) + intensity / 2f;

            player.HealthChange(-damage);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<EmberScene>.Get().BeatEmberSpar();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<EmberScene>.Get().LoseToEmberSpar();

            return true;
        }
    }

    [Register<IsabellaEnemy>("coc:isabella")]
    public class IsabellaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(4);

            Tallness = 84;

            WeaponName = "giant shield";

            WeaponVerb = "bash";

            WeaponAttack = 15;

            ArmorName = "giant shield";

            ArmorDefense = 8;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            if (Health < MaxHealth * 0.7f && Random.Shared.Next(3) == 0)
            {
                MilkHeal(context, player);

                return;
            }

            if (HasSpells(player) && !player.HasStatusEffect(StatusEffectIds.ThroatPunch) && Random.Shared.Next(2) == 0)
            {
                ThroatPunch(context, player);

                return;
            }

            if (!player.HasStatusEffect(StatusEffectIds.Stunned) && Random.Shared.Next(4) == 0)
            {
                ShieldStun(context, player);

                return;
            }

            ShieldCharge(context, player);
        }

        public static bool HasSpells(Player player) =>

            player.HasStatusEffect(StatusEffectIds.KnowsArouse)

            || player.HasStatusEffect(StatusEffectIds.KnowsHeal)

            || player.HasStatusEffect(StatusEffectIds.KnowsMight)

            || player.HasStatusEffect(StatusEffectIds.KnowsCharge)

            || player.HasStatusEffect(StatusEffectIds.KnowsBlind)

            || player.HasStatusEffect(StatusEffectIds.KnowsWhitefire);

        public void MilkHeal(CombatContext context, Player player)
        {
            context.Write("Scenes.Isabella.DrankMalkYaCunt.SucklesMilk".Localized());

            Health = Math.Min(MaxHealth, Health + 100);

            Lust += 5;

            player.TakeLustDamage(10 + player.Libido / 20);
        }

        public void ShieldCharge(CombatContext context, Player player)
        {
            context.Write("Common.Text.IsabellaSnortsAndLowersAShield".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            var damage = CombatCalculator.ReduceDamage(context, player, WeaponAttack + Strength + 20);

            if (damage < 0)
            {
                context.Write("Scenes.Isabella.IsabellaAttack.CatchesCharge".Localized());

                return;
            }

            context.Write("Scenes.Isabella.IsabellaAttack.ShieldHitsHard".Localized());

            player.HealthChange(-damage);
        }

        public void ShieldStun(CombatContext context, Player player)
        {
            context.Write("Common.Text.IsabellaSpinsHerShieldBackAt".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
                return;

            var damage = CombatCalculator.ReduceDamage(context, player, WeaponAttack + Strength);

            if (damage < 0)
            {
                context.Write("Common.Text.YouDeflectHerBlowAwayTaking".Localized());

                return;
            }

            context.Write("Scenes.Isabella.IsabellaStun.BackhandStaggers".Localized());

            player.HealthChange(-damage);

            player.Stun(1, 100);
        }

        public void ThroatPunch(CombatContext context, Player player)
        {
            context.Write("Common.Text.IsabellaPunchesOutFromBehindHer".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            var damage = CombatCalculator.ReduceDamage(context, player, Strength);

            if (damage <= 0)
                context.Write("Common.Text.YouManageToBlockHerWith".Localized());
            else if (player.HasPerk(PerkIds.Resolute) && player.Toughness >= 75)
                context.Write("Common.Text.YouResolutelyIgnoreTheBlowThanks".Localized());
            else
            {
                context.Write("Scenes.Isabella.IsabellaThroatPunch.WindpipeCollapse".Localized());

                player.HealthChange(-damage);

                player.CreateStatusEffect(StatusEffectIds.ThroatPunch, 2, 0, 0, 0);
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<IsabellaScene>.Get().DefeatIsabella();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<IsabellaScene>.Get().IsabellaDefeats();

            return true;
        }
    }

    [Register<HelEnemy>("coc:hel")]
    public class HelEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(true, 1, 1);

            CreateBreastRow(5);

            Tallness = 84;

            WeaponName = "sword";

            WeaponVerb = "slash";

            WeaponAttack = 20;

            ArmorName = "scales";

            ArmorDefense = 14;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            switch (Random.Shared.Next(3))
            {
                case 0:
                    SwordAttack(context, player);

                    break;

                case 1:
                    TailSlap(context, player);

                    break;

                default:
                    Cleavage(context, player);

                    break;
            }
        }

        public void SwordAttack(CombatContext context, Player player)
        {
            var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            if (damage <= 0)
            {
                if (Random.Shared.Next((int)(player.ArmorDefense + player.Toughness)) < player.ArmorDefense)
                    context.Write("You absorb and deflect every " + WeaponVerb + "Common.Text.WithYourArmor".Localized());
                else
                    context.Write("You deflect and block every " + WeaponVerb + "Common.Text.TheSalamanderThrowsAtYou".Localized());

                return;
            }

            context.Write("Scenes.Hel.HelAttack.SalamanderLungeHit".Localized());

            player.HealthChange(-damage);
        }

        public void TailSlap(CombatContext context, Player player)
        {
            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            var damage = CombatCalculator.ReduceDamage(context, player, Strength + SceneInstance<HelScene>.Get().HeliaSparIntensity());

            if (damage <= 0)
            {
                if (Random.Shared.Next((int)(player.ArmorDefense + player.Toughness)) < player.ArmorDefense)
                    context.Write("Scenes.Hel.HelAttack2.TailDeflectsArmor".Localized());
                else
                    context.Write("Scenes.Hel.HelAttack2.TailFailsToMove".Localized());

                return;
            }

            context.Write("Scenes.Hel.HelAttack2.RushPastGuard".Localized());

            player.HealthChange(-damage);
        }

        public void Cleavage(CombatContext context, Player player)
        {
            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Common.Text.ToYourSurpriseTheSalamanderSuddenly".Localized());

                return;
            }

            context.Write("Common.Text.ToYourSurpriseTheSalamanderSuddenly2".Localized());

            var lustDamage = 20 + Random.Shared.Next(10) + player.Sensitivity / 10 + Random.Shared.Next(SceneInstance<HelScene>.Get().HeliaSparIntensity() / 10) + Random.Shared.Next((int)(player.Libido / 20));

            player.TakeLustDamage(lustDamage);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<HelScene>.Get().BeatUpHel();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<HelScene>.Get().LoseToSalamander();

            return true;
        }
    }

    [Register<SophieEnemy>("coc:sophie")]
    public class SophieEnemy : Enemy
    {
        public bool BimboBrawl;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(4);

            Tallness = 70;

            WeaponName = "talons";

            WeaponVerb = "talon";

            WeaponAttack = 20;

            ArmorName = "feathers";

            ArmorDefense = 5;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            var maleAttacks = player.HasCock() && !BimboBrawl;

            if (maleAttacks)
            {
                switch (Random.Shared.Next(3))
                {
                    case 0: HarpyBoats(context, player); break;
                    case 1: Compulsion(context, player); break;
                    default: CombatController.Instance!.Resolver.EnemyBasicAttack(); break;
                }

                return;
            }

            switch (Random.Shared.Next(4))
            {
                case 0: Talons(context, player); break;
                case 1: Batter(context, player); break;
                case 2: Kiss(context, player); break;
                default: CombatController.Instance!.Resolver.EnemyBasicAttack(); break;
            }
        }

        public void Kiss(CombatContext context, Player player)
        {
            context.Write("Common.Text.SophieBobsAndWeavesAsShe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
                return;

            context.Write("Scenes.Sophie.SophieKissAttack.ChastePeck".Localized());

            if (player.HasStatusEffect(StatusEffectIds.Luststick))
            {
                context.Write("Common.Text.BloodRushesToYourCockAs".Localized());

                SceneInstance<SophieScene>.Get().LuststickApplication(2);

                player.TakeLustDamage(12 + player.Libido / 10);
            }
            else
            {
                context.Write("Scenes.Sophie.SophieKissAttack.LipsTingle".Localized());

                SceneInstance<SophieScene>.Get().LuststickApplication(2);

                player.TakeLustDamage(8 + player.Libido / 10);
            }
        }

        public void HarpyBoats(CombatContext context, Player player)
        {
            context.Write("Common.Text.SophieFlapsHerWingsAndLaunches".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
                return;

            context.Write("Scenes.Sophie.SophieHarpyBoatsPC.TalonsMiss".Localized());

            player.TakeLustDamage(13 + player.Sensitivity / 10);
        }

        public void Compulsion(CombatContext context, Player player)
        {
            context.Write("Common.Text.SophieSpreadsHerThickThighsAnd".Localized());

            if (player.Intelligence < 40 || (player.Intelligence < 80 && Random.Shared.Next(40) > player.Intelligence - 40))
            {
                context.Write("Common.Text.YouMoanOutLoudAsYour".Localized());

                player.TakeLustDamage(15 + player.Corruption / 20 + player.Libido / 20);
            }
            else
                context.Write("Scenes.Sophie.SophieCompulsionAttack.MagicCompulsion".Localized());
        }

        public void Talons(CombatContext context, Player player)
        {
            context.Write("Common.Text.SophiePullsHerLegUpCocking".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = false
            });

            if (avoid.AttackFailed)
                return;

            context.Write("Common.Text.HerLegLashesForwardsLightningQuick".Localized());

            var damage = (Strength + WeaponAttack) * CombatCalculator.DamagePercent(context, player) * 0.01025f;

            if (damage < 0) damage = 0;
            damage += 40;

            player.HealthChange(-damage);
        }

        public void Batter(CombatContext context, Player player)
        {
            context.Write("Scenes.Sophie.BatterAttackSophie.WingFlurry".Localized());

            for (var i = 0; i < 4; i++)
            {
                context.Write("Common.Text.HerWingBattersYou".Localized());

                var damage = CombatCalculator.ReduceDamage(context, player, Strength);

                if (damage < 0) damage = 0;
                player.HealthChange(-damage);
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<SophieScene>.Get().SophieLostCombat();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<SophieScene>.Get().SophieWonCombat();

            return true;
        }
    }

    [Register<SheilaEnemy>("coc:sheila")]
    public class SheilaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(4);

            Tallness = 70;

            WeaponName = "foot";

            WeaponVerb = "kick";

            WeaponAttack = 10;

            ArmorName = "clothes";

            ArmorDefense = 4;
        }

        public int _roundCounter;

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 1)
            {
                DemonAI(context, player);

                return;
            }

            if (Random.Shared.Next(3) == 0)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
            else if (Random.Shared.Next(2) == 0)
                FlyingKick(context, player);
            else
                FrogPunch(context, player);
        }

        public void FrogPunch(CombatContext context, Player player)
        {
            Speed -= 30;

            try
            {
                if (player.Tallness < 42 && Random.Shared.Next(2) == 0)
                {
                    context.Write("Common.Text.SheilaBouncesUpToYouAnd".Localized());

                    Health = Math.Max(0, Health - (3 + Random.Shared.Next(10)));

                    return;
                }

                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = this,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false
                });

                if (avoid.AttackFailed || HasStatusEffect(StatusEffectIds.Blind))
                {
                    context.Write("Common.Text.SheilaBouncesUpToYouAnd2".Localized());

                    player.TakeLustDamage(10 + Random.Shared.Next((int)(player.Sensitivity / 10)));

                    return;
                }

                context.Write("Scenes.Sheila.SheilaFrogPunch.FrogPunchTelegraphThree".Localized());

                player.ChangeFatigue(5 + Random.Shared.Next(5));

                if (player.Stun(1, 33))
                    context.Write("Common.Text.BYouAreStunnedB".Localized());

                var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack);

                if (damage < 1) damage = 2;
                player.HealthChange(-damage);
            }
            finally
            {
                Speed += 30;
            }
        }

        public void FlyingKick(CombatContext context, Player player)
        {
            Speed -= 60;

            try
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = this,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false
                });

                if (avoid.AttackFailed)
                {
                    context.Write("Scenes.Sheila.SheilaFlyingKick.KickTelegraphOne".Localized());

                    Health = Math.Max(0, Health - (3 + Random.Shared.Next(10)));

                    return;
                }

                context.Write("Scenes.Sheila.SheilaFlyingKick.KickTelegraphTwo".Localized());

                if (player.Stun(2, 100))
                    context.Write("Common.Text.BYouAreStunnedB".Localized());

                var damage = CombatCalculator.ReduceDamage(context, player, Strength + 50 + WeaponAttack);

                if (damage < 1) damage = 2;
                player.HealthChange(-damage);

                player.ChangeFatigue(10 + Random.Shared.Next(6));
            }
            finally
            {
                Speed += 60;
            }
        }

        public void DemonAI(CombatContext context, Player player)
        {
            _roundCounter++;

            if (_roundCounter >= 5)
            {
                SitAndPout(context, player);

                return;
            }

            if (!player.HasStatusEffect(StatusEffectIds.SheilaOil))
                switch (Random.Shared.Next(3))
                {
                    case 0: SuspiciousGlint(context, player); break;
                    case 1: TittyMonster(context, player); break;
                    default: Splash(context, player); break;
                }
            else
                if (Random.Shared.Next(2) == 0) PressurePoints(context, player);
                else LickEmAndStickEm(context, player);
        }

        public void SitAndPout(CombatContext context, Player player)
        {
            context.Write("Common.Text.SheilaFrownsAtYouThenPlops".Localized());

            Health = 0;

            Lust = 100;

            if (player.Lust < 33)
            {
                context.Write("Common.Text.YoureNotThatInterestedThoughSheila".Localized());

                CombatScene.CleanupAfterCombat();
            }
        }

        public void SuspiciousGlint(CombatContext context, Player player)
        {
            if (HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(2) == 0)
            {
                context.Write("Scenes.Sheila.SuspiciousGlint.BlindEyesGlint".Localized());

                return;
            }

            if (player.Intelligence / 15f + Random.Shared.Next(20) + 1 > 16)
            {
                context.Write("Common.Text.SheilasEyesGlintSuspiciouslyAsShe".Localized());

                return;
            }

            context.Write("Common.Text.SheilasEyesGlintSuspiciouslyAndYou".Localized());

            if (!HasStatusEffect(StatusEffectIds.TwuWuv))
            {
                CreateStatusEffect(StatusEffectIds.TwuWuv, 0, 0, 0, 0);

                var counter = 40 + Random.Shared.Next(5);

                while (counter-- > 0 && player.Intelligence >= 2)

                    player.ChangeStatusValue(StatusEffectIds.TwuWuv, 1, 1);
            }

            player.TakeLustDamage(30 + player.Libido / 10 + player.Corruption / 10);
        }

        public void TittyMonster(CombatContext context, Player player)
        {
            var corruption = SceneInstance<SheilaScene>.Get().SheilaCorruption();

            context.Write("Sheila giggles and strokes her " + SceneInstance<SheilaScene>.Get().SheilaCup() + "Common.Text.BreastsTryingToEnticeYou".Localized());

            if (corruption < 20)
                context.Write("Scenes.Sheila.TittyMonsterAttack.NothingToWork".Localized());
            else if (corruption < 150)
            {
                context.Write("Common.Text.AsHerHandsRunOverThe".Localized());

                player.TakeLustDamage(25 + player.Libido / 10);
            }
            else
                context.Write("Common.Text.SheHasTroubleEvenBudgingTits".Localized());
        }

        public void Splash(CombatContext context, Player player)
        {
            context.Write("Common.Text.SheilaWaitsPatientlyStaringAtYou".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Common.Text.YouEasilyLeanAwayFromThe".Localized());

                return;
            }

            context.Write("Scenes.Sheila.SplashAttackLookOutShellEvolveIntoGyrados.SplashHits".Localized());

            player.CreateStatusEffect(StatusEffectIds.Blind, 1, 0, 0, 0);

            player.CreateStatusEffect(StatusEffectIds.SheilaOil, 0, 0, 0, 0);
        }

        public void PressurePoints(CombatContext context, Player player)
        {
            var corruption = SceneInstance<SheilaScene>.Get().SheilaCorruption();

            context.Write("Common.Text.ForAMomentAllGoesQuiet".Localized());

            if (corruption < 100)
            {
                context.Write("The silence is broken with a giggle as the demon catches you in an embrace, pressing her " + SceneInstance<SheilaScene>.Get().SheilaCup() + "Common.Text.BreastsIntoYouYouShiverAs".Localized());

                player.TakeLustDamage(15 + player.Sensitivity / 20 + player.Libido / 20);
            }
            else if (corruption < 300)
            {
                context.Write("Common.Text.ASighEndsTheSilenceAs".Localized());

                player.TakeLustDamage(25 + player.Sensitivity / 20 + player.Libido / 20);
            }
            else
                context.Write("Common.Text.YoureABitUnnervedButSoon".Localized());
        }

        public void LickEmAndStickEm(CombatContext context, Player player)
        {
            context.Write("Common.Text.SheilasVoiceGetsCloserBecomingDisarmingly".Localized());

            if (player.Intelligence / 15f + Random.Shared.Next(20) + 1 > 16)
            {
                context.Write("Common.Text.BeforeTheFantasyCanAdvanceYou".Localized());

                player.TakeLustDamage(15 + player.Sensitivity / 20 + player.Libido / 20);
            }
            else
            {
                context.Write("Common.Text.YourEgosUrgentDangerWarningsAre".Localized());

                player.TakeLustDamage(25 + player.Sensitivity / 20 + player.Libido / 20);
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 1)
                SceneInstance<SheilaScene>.Get().BeatUpDemonSheila();
            else
                SceneInstance<SheilaScene>.Get().SheilaGotWhomped();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 1)
                SceneInstance<SheilaScene>.Get().LoseToSheila();
            else
                SceneInstance<SheilaScene>.Get().GetBeatUpBySheila();

            return true;
        }
    }
}