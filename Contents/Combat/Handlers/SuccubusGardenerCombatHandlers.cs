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
                context.AppendLog("Combat.SuccubusGardener.VineHeal.TippingHerselfBackward".Localized());

                if (gardener.LustVulnerability <= 0.15f)
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.YouArenT".Localized());
                else if (gardener.LustVulnerability <= 0.45f)
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.SheSDefinitely".Localized());
                else
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.ThereSNo".Localized());

                if (gardener.LustVulnerability <= 0.3f)
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.WhateverIsIn".Localized());
            }
            else
            {
                context.AppendLog("Combat.SuccubusGardener.VineHeal.ActingOnSome".Localized());

                if (gardener.LustVulnerability <= 0.15f)
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.AreSlightlyGlassy".Localized());
                else if (gardener.LustVulnerability <= 0.45f)
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.AreEvenGlassier".Localized());
                else
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.AreDilatedAnd".Localized());

                if (gardener.LustVulnerability <= 0.3f)
                    context.AppendLog("Combat.SuccubusGardener.VineHeal.WhateverIsIn".Localized());
            }

            gardener.Health = gardener.MaxHealth;

            gardener.LustVulnerability += 0.3f;

            gardener.Fatigue += 5;

            if (gardener.Fatigue >= 100)
            {
                context.AppendLog("Combat.SuccubusGardener.VineHeal.ItAppearsThat".Localized());

                gardener.Fatigue = 100;
            }
        }

        [Register("coc:succubus_gardener_tentagrapple")]
        public static void Tentagrapple(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            gardener.CreateStatusEffect(SuccubusGardenerEnemy.TentagrappleCooldownId, 10);

            context.AppendLog("Combat.SuccubusGardener.Tentagrapple.AWebOf".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = gardener, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
                context.AppendLog("Combat.SuccubusGardener.Tentagrapple.YouSlipAside".Localized());
            else
            {
                context.AppendLog("Combat.SuccubusGardener.Tentagrapple.YouTwistTo".Localized());

                player.CreateStatusEffect(SuccubusGardenerEnemy.TentagrappledId);
            }
        }

        [Register("coc:succubus_gardener_sap_speed")]
        public static void SapSpeed(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.SuccubusGardener.SapSpeed.CuppingHerBreasts".Localized());

            context.AppendLog("Combat.SuccubusGardener.SapSpeed.TheLacticAdhesive".Localized());

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

            context.AppendLog("Combat.SuccubusGardener.Sicem.JigglingOhSo".Localized());

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
                context.AppendLog("Combat.SuccubusGardener.Sicem.TheSinuousPlant".Localized());

                player.TakeLustDamage(damage, true);
            }
            else
                context.AppendLog("Combat.SuccubusGardener.Sicem.TheSinuousPlant2".Localized());
        }

        [Register("coc:succubus_gardener_corruptive_shower")]
        public static void CorruptiveShower(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.SuccubusGardener.CorruptiveShower.TheSuccubusLifts".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.AppendLog("Combat.SuccubusGardener.CorruptiveShower.SomehowYouManage".Localized());

                return;
            }

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId))
                player.CreateStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId, 3);
            else
                player.ChangeStatusValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0, 3);

            context.AppendLog("Combat.SuccubusGardener.CorruptiveShower.YouTryYour".Localized());
        }

        [Register("coc:succubus_gardener_lust_aura")]
        public static void LustAuraCast(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            context.AppendLog("Combat.SuccubusGardener.LustAuraCast.TheDemonessBlinks".Localized());

            if (gardener.HasStatusEffect(StatusEffectIds.LustAura))
            {
                context.AppendLog("Combat.SuccubusGardener.LustAuraCast.YourEyesCross".Localized());

                player.TakeLustDamage(8 + (int)(player.Libido / 20f + player.Corruption / 25f), true);
            }
            else
                gardener.CreateStatusEffect(StatusEffectIds.LustAura);
        }

        [Register("coc:succubus_gardener_taunt")]
        public static void Taunt(CombatContext context, CombatAbilityDefinition ability)
        {
            context.AppendLog("Combat.SuccubusGardener.Taunt.TheGreenThumbed".Localized());
        }

        [Register("coc:succubus_gardener_motorboat")]
        public static void Motorboat(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            context.AppendLog("Combat.SuccubusGardener.Motorboat.TheDemonessGrowls".Localized());

            if (player.Tallness <= gardener.Tallness - 6)
                context.AppendLog("Combat.SuccubusGardener.Motorboat.SomehowShortEnough".Localized());
            else if (player.Tallness >= gardener.Tallness + 6)
                context.AppendLog("Combat.SuccubusGardener.Motorboat.SomehowTallEnough".Localized());

            context.AppendLog("Combat.SuccubusGardener.Motorboat.TheyReSo".Localized());

            context.AppendLog("Combat.SuccubusGardener.Motorboat.YourFoeGiggles".Localized());
        }

        [Register("coc:succubus_gardener_taste_the_ecstasy")]
        public static void TasteTheEcstasy(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.SuccubusGardener.TasteTheEcstasy.ThreeTentaclesStab".Localized());

            context.AppendLog("Combat.SuccubusGardener.TasteTheEcstasy.OneOfThose".Localized());

            var roll = (int)(context.Random.NextDouble() * (player.Strength - 30)) + 30;

            if (roll > enemy.Strength)
            {
                context.AppendLog("Combat.SuccubusGardener.TasteTheEcstasy.ItHangsThere".Localized());

                context.AppendLog("Combat.SuccubusGardener.TasteTheEcstasy.YouBarelyEven".Localized());

                player.TakeLustDamage(8 + (int)(player.Libido / 20f + player.Corruption / 25f), true);

                player.ApplyStats(new DynStat(GameStats.Corruption, 5f));
            }
            else
            {
                context.AppendLog("Combat.SuccubusGardener.TasteTheEcstasy.ThatSAll".Localized());

                context.AppendLog("Combat.SuccubusGardener.TasteTheEcstasy.ShePouts".Localized());
            }
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            var gardener = Gardener(context);

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.TentagrappledId))
                return;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            var numRounds = (int)player.StatusEffectValue(SuccubusGardenerEnemy.TentagrappledId, 0);

            if ((int)(context.Random.NextDouble() * player.Strength) > gardener.Strength / (1 + numRounds / 2f))
            {
                context.AppendLog("Combat.SuccubusGardener.Struggle.YouScrabbleDesperately".Localized());

                player.RemoveStatusEffect(SuccubusGardenerEnemy.TentagrappledId);
            }
            else
            {
                context.AppendLog("Combat.SuccubusGardener.Struggle.YouScrabbleDesperately2".Localized());

                context.AppendLog(context.Random.Next(2) == 0

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

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.AppendLog(context.Random.Next(2) == 0

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
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.TheTentaclesSex".Localized());
            else if (player.LustPercent < 60)
            {
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.YouTryTo".Localized());

                context.AppendLog(player.Corruption < 50

                    ? "Combat.SuccubusGardener.ShowerDotEffect.YouReAshamed".Localized()

                    : "Combat.SuccubusGardener.ShowerDotEffect.YouReA".Localized());

                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.ThatItS".Localized());
            }
            else if (player.LustPercent < 70)
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.YouGroanAt".Localized());
            else if (player.LustPercent < 80)
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.YouWhimperAs".Localized());
            else if (player.LustPercent < 90)
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.YouShudderIn".Localized());
            else if (player.LustPercent < 100)
            {
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.OhhhhYouRe".Localized());

                if (player.HasCock())
                    context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.EvenWhileYour"
                        .Localized(player.Cocks.Count > 1 ? "s are" : " is"));
                else if (player.HasVagina())
                    context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.SoakedTwatThreatens"
                        .Localized(player.Wetness() == 4 ? "drown" : "further soak"));
                else
                    context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.Text".Localized());
            }
            else
                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.OhhhFuckThere".Localized());

            if (player.StatusEffectValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0) < 0 &&
                player.LustPercent < 100)
            {
                player.RemoveStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId);

                context.AppendLog("Combat.SuccubusGardener.ShowerDotEffect.TheLustInducing".Localized());
            }
        }
    }
}
