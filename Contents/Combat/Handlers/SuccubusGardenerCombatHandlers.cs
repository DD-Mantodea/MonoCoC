using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class SuccubusGardenerCombatHandlers
    {
        public static SuccubusGardenerEnemy Gardener(CombatContext context) =>

            (SuccubusGardenerEnemy)context.Enemy;

        [Register("coc:succubus_gardener_vine_heal")]
        public static void VineHeal(CombatContext context, CombatAbilityDefinition ability)
        {
            var gardener = Gardener(context);

            if (!gardener.HasStatusEffect(SuccubusGardenerEnemy.VineHealUsedId))
                gardener.CreateStatusEffect(SuccubusGardenerEnemy.VineHealUsedId);

            if (!gardener.HasStatusEffect(StatusEffectIds.Stunned))
            {
                context.Append("Combat.SuccubusGardener.VineHeal.TippingHerselfBackward".Localized());

                if (gardener.LustVulnerability <= 0.15f)
                    context.Append("Combat.SuccubusGardener.VineHeal.YouArenT".Localized());
                else if (gardener.LustVulnerability <= 0.45f)
                    context.Append("Combat.SuccubusGardener.VineHeal.SheSDefinitely".Localized());
                else
                    context.Append("Combat.SuccubusGardener.VineHeal.ThereSNo".Localized());

                if (gardener.LustVulnerability <= 0.3f)
                    context.Append("Combat.SuccubusGardener.VineHeal.WhateverIsIn".Localized());
            }
            else
            {
                context.Append("Combat.SuccubusGardener.VineHeal.ActingOnSome".Localized());

                if (gardener.LustVulnerability <= 0.15f)
                    context.Append("Combat.SuccubusGardener.VineHeal.AreSlightlyGlassy".Localized());
                else if (gardener.LustVulnerability <= 0.45f)
                    context.Append("Combat.SuccubusGardener.VineHeal.AreEvenGlassier".Localized());
                else
                    context.Append("Combat.SuccubusGardener.VineHeal.AreDilatedAnd".Localized());

                if (gardener.LustVulnerability <= 0.3f)
                    context.Append("Combat.SuccubusGardener.VineHeal.WhateverIsIn".Localized());
            }

            gardener.Health = gardener.MaxHealth;

            gardener.LustVulnerability += 0.3f;

            gardener.Fatigue += 5;

            if (gardener.Fatigue >= 100)
            {
                context.Append("Combat.SuccubusGardener.VineHeal.ItAppearsThat".Localized());

                gardener.Fatigue = 100;
            }
        }

        [Register("coc:succubus_gardener_tentagrapple")]
        public static void Tentagrapple(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            gardener.CreateStatusEffect(SuccubusGardenerEnemy.TentagrappleCooldownId, 10);

            context.Append("Combat.SuccubusGardener.Tentagrapple.AWebOf".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = gardener, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
                context.Append("Combat.SuccubusGardener.Tentagrapple.YouSlipAside".Localized());
            else
            {
                context.Append("Combat.SuccubusGardener.Tentagrapple.YouTwistTo".Localized());

                player.CreateStatusEffect(SuccubusGardenerEnemy.TentagrappledId);
            }
        }

        [Register("coc:succubus_gardener_sap_speed")]
        public static void SapSpeed(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.SuccubusGardener.SapSpeed.CuppingHerBreasts".Localized());

            context.Append("Combat.SuccubusGardener.SapSpeed.TheLacticAdhesive".Localized());

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.GardenerSapSpeedId))
            {
                player.CreateStatusEffect(SuccubusGardenerEnemy.GardenerSapSpeedId);

                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -player.Speed * 0.2f,

                    "GardenerSapSpeed", SuccubusGardenerEnemy.GardenerSapSpeedId);
            }
        }

        [Register("coc:succubus_gardener_sicem")]
        public static void Sicem(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.SuccubusGardener.Sicem.JigglingOhSo".Localized());

            var damage = 0f;

            for (var i = 0; i < 10; i++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy, Defender = player,

                    DoDodge = true, DoParry = false, DoBlock = false
                });

                if (avoid.AttackHit)
                    damage += 2f +

                        (int)(context.Random.NextDouble() * (1 + player.Libido / 20f)) +

                        (int)(context.Random.NextDouble() * (1 + player.Sensitivity / 20f));
            }

            if (damage >= 0)
            {
                context.Append("Combat.SuccubusGardener.Sicem.TheSinuousPlant".Localized());

                player.TakeLustDamage(damage, true);
            }
            else
                context.Append("Combat.SuccubusGardener.Sicem.TheSinuousPlant2".Localized());
        }

        [Register("coc:succubus_gardener_corruptive_shower")]
        public static void CorruptiveShower(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.SuccubusGardener.CorruptiveShower.TheSuccubusLifts".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.SuccubusGardener.CorruptiveShower.SomehowYouManage".Localized());

                return;
            }

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId))
                player.CreateStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId, 3);
            else
                player.ChangeStatusValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0, 3);

            context.Append("Combat.SuccubusGardener.CorruptiveShower.YouTryYour".Localized());
        }

        [Register("coc:succubus_gardener_lust_aura")]
        public static void LustAuraCast(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            context.Append("Combat.SuccubusGardener.LustAuraCast.TheDemonessBlinks".Localized());

            if (gardener.HasStatusEffect(StatusEffectIds.LustAura))
            {
                context.Append("Combat.SuccubusGardener.LustAuraCast.YourEyesCross".Localized());

                player.TakeLustDamage(8 + (int)(player.Libido / 20f + player.Corruption / 25f), true);
            }
            else
                gardener.CreateStatusEffect(StatusEffectIds.LustAura);
        }

        [Register("coc:succubus_gardener_taunt")]
        public static void Taunt(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.SuccubusGardener.Taunt.TheGreenThumbed".Localized());
        }

        [Register("coc:succubus_gardener_motorboat")]
        public static void Motorboat(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            context.Append("Combat.SuccubusGardener.Motorboat.TheDemonessGrowls".Localized());

            if (player.Tallness <= gardener.Tallness - 6)
                context.Append("Combat.SuccubusGardener.Motorboat.SomehowShortEnough".Localized());
            else if (player.Tallness >= gardener.Tallness + 6)
                context.Append("Combat.SuccubusGardener.Motorboat.SomehowTallEnough".Localized());

            context.Append("Combat.SuccubusGardener.Motorboat.TheyReSo".Localized());

            context.Append("Combat.SuccubusGardener.Motorboat.YourFoeGiggles".Localized());
        }

        [Register("coc:succubus_gardener_taste_the_ecstasy")]
        public static void TasteTheEcstasy(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ThreeTentaclesStab".Localized());

            context.Append("Combat.SuccubusGardener.TasteTheEcstasy.OneOfThose".Localized());

            var roll = (int)(context.Random.NextDouble() * (player.Strength - 30)) + 30;

            if (roll > enemy.Strength)
            {
                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ItHangsThere".Localized());

                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.YouBarelyEven".Localized());

                player.TakeLustDamage(8 + (int)(player.Libido / 20f + player.Corruption / 25f), true);

                player.ApplyStats(new DynStat(GameStats.Corruption, 5f));
            }
            else
            {
                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ThatSAll".Localized());

                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ShePouts".Localized());
            }
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.TentagrappledId))
                return;

            context.Clear();

            var numRounds = (int)player.StatusEffectValue(SuccubusGardenerEnemy.TentagrappledId, 0);

            if ((int)(context.Random.NextDouble() * player.Strength) > gardener.Strength / (1 + numRounds / 2f))
            {
                context.Append("Combat.SuccubusGardener.Struggle.YouScrabbleDesperately".Localized());

                player.RemoveStatusEffect(SuccubusGardenerEnemy.TentagrappledId);
            }
            else
            {
                context.Append("Combat.SuccubusGardener.Struggle.YouScrabbleDesperately2".Localized());

                context.Append(context.Random.Next(2) == 0

                    ? "Combat.SuccubusGardener.Struggle.ButTheVines".Localized()

                    : "Combat.SuccubusGardener.Struggle.YouReIntimately".Localized());

                player.ChangeStatusValue(SuccubusGardenerEnemy.TentagrappledId, 0, 1);

                if (!player.HasPerk(PerkIds.Juggernaut) && gardener.ArmorPerk != "Heavy")
                    player.HealthChange(-(75 + context.Random.Next(15)));

                player.TakeLustDamage(3 + context.Random.Next(3), true);

                if (SceneInstance<CeraphScene>.Get().HasBondage())
                    player.TakeLustDamage(7, true);
            }

            gardener.TookAction = true;
        }

        public static void Squeeze(CombatContext context)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            context.Clear();

            context.Append(context.Random.Next(2) == 0

                ? "Combat.SuccubusGardener.Squeeze.TheVinesEncircling".Localized()

                : "Combat.SuccubusGardener.Squeeze.YouReIntimately".Localized());

            player.ChangeStatusValue(SuccubusGardenerEnemy.TentagrappledId, 0, 1);

            if (!player.HasPerk(PerkIds.Juggernaut) && gardener.ArmorPerk != "Heavy")
                player.HealthChange(-(75 + context.Random.Next(15)));

            player.TakeLustDamage(3 + context.Random.Next(3), true);
        }

        public static void ShowerDotEffect(CombatContext context)
        {
            var player = context.Player;

            var lustDamage = 2 + context.Random.Next(2);

            player.TakeLustDamage(lustDamage, false);

            player.ChangeStatusValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0, -1);

            if (player.LustPercent < 50)
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.TheTentaclesSex".Localized());
            else if (player.LustPercent < 60)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouTryTo".Localized());

                context.Append(player.Corruption < 50

                    ? "Combat.SuccubusGardener.ShowerDotEffect.YouReAshamed".Localized()

                    : "Combat.SuccubusGardener.ShowerDotEffect.YouReA".Localized());

                context.Append("Combat.SuccubusGardener.ShowerDotEffect.ThatItS".Localized());
            }
            else if (player.LustPercent < 70)
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouGroanAt".Localized());
            else if (player.LustPercent < 80)
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouWhimperAs".Localized());
            else if (player.LustPercent < 90)
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouShudderIn".Localized());
            else if (player.LustPercent < 100)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.OhhhhYouRe".Localized());

                if (player.HasCock())
                    context.Append("Combat.SuccubusGardener.ShowerDotEffect.EvenWhileYour"
                        .Localized(player.Cocks.Count > 1 ? "s are" : " is"));
                else if (player.HasVagina())
                    context.Append("Combat.SuccubusGardener.ShowerDotEffect.SoakedTwatThreatens"
                        .Localized(player.Wetness() == 4 ? "drown" : "further soak"));
                else
                    context.Append("Combat.SuccubusGardener.ShowerDotEffect.Text".Localized());
            }
            else
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.OhhhFuckThere".Localized());

            if (player.StatusEffectValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0) < 0 &&
                player.LustPercent < 100)
            {
                player.RemoveStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId);

                context.Append("Combat.SuccubusGardener.ShowerDotEffect.TheLustInducing".Localized());
            }
        }
    }
}
