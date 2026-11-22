using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class HermCentaurCombatHandlers
    {

        [Register("coc:herm_centaur_feminine_musk")]
        public static void FeminineMusk(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.HermCentaur.FeminineMusk.TurningAboutThe".Localized());

            if (player.LustPercent <= 50)
                context.AppendLog("Combat.HermCentaur.FeminineMusk.YourGrowsFeverishly".Localized());
            else
            {
                context.AppendLog("Combat.HermCentaur.FeminineMusk.BloodRushesTo".Localized());

                if (player.HasCock())
                    context.AppendLog("Combat.HermCentaur.FeminineMusk.WithTheDesire".Localized());
                else if (player.HasVagina())
                    context.AppendLog("Combat.HermCentaur.FeminineMusk.YourWithThe".Localized());
                else
                    context.AppendLog("Combat.HermCentaur.FeminineMusk.YourAnusUntil".Localized());
            }

            player.TakeLustDamage(8 + player.Libido / 10f + player.Sensitivity / 10f, true);
        }

        [Register("coc:herm_centaur_aphrodisiac_squirt")]
        public static void AphrodisiacSquirt(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.TheCentaurGrabs".Localized());

            player.TakeLustDamage(8 + player.Libido / 10f + player.Sensitivity / 10f, true);

            if (player.LustPercent < 30)
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YouCloseYour".Localized());
            else if (player.LustPercent < 40)
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YourHeartBeats".Localized());
            else if (player.LustPercent < 50)
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YourCheeksColor".Localized());
            else if (player.LustPercent < 60)
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YourBlinkBut".Localized());
            else if (player.LustPercent < 70)
            {
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YouGroanIn".Localized());

                if (player.HasCock())
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.AwakensFillingItself".Localized());
                else if (player.HasVagina())
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.AWetHeat".Localized());
                else
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YourTinglesCraving".Localized());
            }
            else if (player.LustPercent < 80)
            {
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YouSighAnd".Localized());

                if (player.HasCock())
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.IgnoringTheStiffening".Localized());
                else
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.IgnoringTheIncreasing".Localized());

                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.ItSGetting".Localized());
            }
            else if (player.LustPercent < 90)
            {
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.PantingFeverishlyYou".Localized());

                if (player.HasCock())
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.PreBeginsTo".Localized());
                else if (player.HasVagina())
                    context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.AFloodOf".Localized());

                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.YouNeedTo".Localized());
            }
            else if (player.LustPercent < 100)
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.MoaningOutLoud".Localized());
            else
                context.AppendLog("Combat.HermCentaur.AphrodisiacSquirt.SinkingToYour".Localized());
        }

        [Register("coc:herm_centaur_arouse_spell_charge")]
        public static void ArouseSpellCharge(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;

            var player = context.Player;

            context.AppendLog("Combat.HermCentaur.ArouseSpellCharge.TheCentauressCloses".Localized());

            if (player.Intelligence > 80)
                context.AppendLog("Combat.HermCentaur.ArouseSpellCharge.ATeaseAttack".Localized());

            herm.ChargingArouse = true;

            herm.LustAtChargeStart = herm.Lust;

            herm.ArouseCooldown = 7;
        }

        [Register("coc:herm_centaur_arouse_spell_cast")]
        public static void ArouseSpellCast(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;

            var player = context.Player;

            herm.ChargingArouse = false;

            herm.LustAtChargeStart = -1;

            if (herm.Lust >= herm.LustAtChargeStart + 10f)
                context.AppendLog("Combat.HermCentaur.ArouseSpellCast.ShudderingTheDemoness".Localized());
            else
            {
                context.AppendLog("Combat.HermCentaur.ArouseSpellCast.TheChantingReaches".Localized());

                player.TakeLustDamage(20 + player.Libido / 6f + player.Sensitivity / 6f, true);
            }
        }

        [Register("coc:herm_centaur_hypno_cock")]
        public static void HypnoCock(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;

            var player = context.Player;

            herm.HypnoCockUses += 1;

            if (herm.HypnoCockUses == 1)
            {
                context.AppendLog("Combat.HermCentaur.HypnoCock.ThwackYouStart".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.YouGlanceLower".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.OnlyAfterWatching".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.WellYouCan".Localized());
            }
            else
            {
                context.AppendLog("Combat.HermCentaur.HypnoCock.ThwackSheS".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.SheMusesQuietly".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.YesItS".Localized());
            }

            var resistance = player.Intelligence * (2f / herm.HypnoCockUses);

            var threshold = (int)(player.Libido / 3f + player.Sensitivity / 3f + player.Corruption / 3f);

            var roll = (int)(context.Random.NextDouble() * threshold);

            if (resistance > roll)
            {
                var extra = (int)(player.Libido / 20f + player.Sensitivity / 20f);

                player.TakeLustDamage(2 + (int)(context.Random.NextDouble() * extra), true);

                if (player.LustPercent <= 33)
                    context.AppendLog("Combat.HermCentaur.HypnoCock.AWarningThought".Localized());
                else if (player.LustPercent <= 66)
                    context.AppendLog("Combat.HermCentaur.HypnoCock.AQuietVoice".Localized());
                else
                    context.AppendLog("Combat.HermCentaur.HypnoCock.ASimperingVoice".Localized());
            }
            else
            {
                var extra = (int)(player.Libido / 10f + player.Sensitivity / 10f);

                player.TakeLustDamage(20 + 2 * herm.HypnoCockUses + 2 + (int)(context.Random.NextDouble() * extra), true);

                context.AppendLog("Combat.HermCentaur.HypnoCock.DownItBobs".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.FuckSheS".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.TheVoiceAsks".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.YouNod".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.Text".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.AgainYouNod".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.TheVoiceContinues".Localized());

                context.AppendLog("Combat.HermCentaur.HypnoCock.GodsYouWant".Localized());

                if (player.Lust >= player.MaxLust)
                    context.AppendLog("Combat.HermCentaur.HypnoCock.YouWhimperToo".Localized());
                else
                {
                    context.AppendLog("Combat.HermCentaur.HypnoCock.YouStumbleForward".Localized());

                    context.AppendLog("Combat.HermCentaur.HypnoCock.TheEquineDemon".Localized());
                }
            }
        }

        [Register("coc:herm_centaur_gotta_cum")]
        public static void GottaCum(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;

            var player = context.Player;

            herm.UsedGottaCum = true;

            context.AppendLog("Combat.HermCentaur.GottaCum.SighingTheDemoness".Localized());

            context.AppendLog("Combat.HermCentaur.GottaCum.TheCorruptionFouled".Localized());

            context.AppendLog("Combat.HermCentaur.GottaCum.HundredsOfPounds".Localized());

            context.AppendLog("Combat.HermCentaur.GottaCum.FlushingTheDemoness".Localized());

            herm.Lust = 0;

            player.TakeLustDamage(15, true);
        }

        [Register("coc:herm_centaur_heal_up")]
        public static void HealUp(CombatContext context, CombatAbilityDefinition ability)
        {
            var herm = (HermCentaurEnemy)context.Enemy;

            herm.UsedHeal = true;

            context.AppendLog("Combat.HermCentaur.HealUp.WipingADrop".Localized());

            herm.Health = herm.MaxHealth;
        }
    }
}