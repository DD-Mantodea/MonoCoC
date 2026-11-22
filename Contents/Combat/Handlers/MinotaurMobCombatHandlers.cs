using MonoCoC.Attributes;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MinotaurMobCombatHandlers
    {

        [Register("coc:minotaur_mob_minotaur_gang_grope_attack")]
        public static void GangGrope(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.MinotaurMob.Grope.Lead".Localized());

            if (player.Lust >= player.MaxLust * 0.8f)
                context.AppendLog("Combat.MinotaurMob.Grope.Wet".Localized());

            context.AppendLog(".");

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Sensitivity / 10);
        }

        [Register("coc:minotaur_mob_minotaur_gang_gang_grope_attack")]
        public static void GangGangGrope(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.MinotaurMob.GangGrope.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 15 + player.Sensitivity / 10);
        }

        [Register("coc:minotaur_mob_minotaur_gang_waste")]
        public static void Waste(CombatContext context, CombatAbilityDefinition ability)
        {
            CoC.Instance.Flags.Set(GameFlagIds.MinotaurSonsWastedTurn, 1);

            context.AppendLog("Combat.MinotaurMob.Waste.Lead".Localized());
        }

        [Register("coc:minotaur_mob_mino_pheromones")]
        public static void MinoPheromones(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.Minotaur.Pheromones.Lead".Localized());

            if (context.Random.Next(3) == 0)
            {
                context.AppendLog("Combat.Minotaur.Pheromones.Face".Localized());

                if (player.Lust >= player.MaxLust * 0.75f)
                {
                    context.AppendLog("Combat.Minotaur.Pheromones.Swallow".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 10 + player.Libido / 10);
                }
                else
                {
                    context.AppendLog("Combat.Minotaur.Pheromones.Lick".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 20);
                }
            }
            else
                context.AppendLog("Combat.Minotaur.Pheromones.Miss".Localized());

            context.AppendLog("Combat.Minotaur.Pheromones.Scent".Localized());
        }

        [Register("coc:minotaur_mob_precum_tease")]
        public static void PrecumTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var teased = false;

            var oldLust = player.Lust;

            if (context.Random.Next(2) == 0)
            {
                teased = true;

                if (context.Random.Next(5) > 0)
                {
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.TheBiggestLiftsHis".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 7 + player.Libido / 20);
                }
                else
                {

                    context.AppendLog("Combat.MinotaurMob.PrecumTease.TheLargestBullThe".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 13 + player.Libido / 20);
                }
            }

            if (context.Random.Next(2) == 0)
            {
                teased = true;

                if (context.Random.Next(5) > 0)
                {
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.TauntsOneTheBeast".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 3 + player.Libido / 30);
                }
                else
                {
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.AnnouncesWellBuiltMinotaur".Localized());

                    if (player.Speed / 5f + context.Random.Next(20) > 20)
                    {
                        context.AppendLog("Combat.MinotaurMob.PrecumTease.YouAvoidMostThem".Localized());

                        player.Lust = Math.Min(player.MaxLust, player.Lust + 6 + player.Libido / 20);
                    }
                    else
                    {
                        context.AppendLog("Combat.MinotaurMob.PrecumTease.YouTryAvoidThem".Localized());

                        player.Lust = Math.Min(player.MaxLust, player.Lust + 15 + player.Libido / 20);
                    }
                }
            }

            if (!teased || context.Random.Next(3) == 0)
            {
                context.AppendLog("Combat.MinotaurMob.PrecumTease.TheSmallestTheBeastmen".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 9 + player.Libido / 20);
            }

            var gained = player.Lust - oldLust;

            context.AppendLog("Combat.MinotaurMob.PrecumTease.Text".Localized());

            if (gained <= 8)
            {
                context.AppendLog("Combat.MinotaurMob.PrecumTease.ThoughYourBodyTingling".Localized());

                if (player.Lust >= player.MaxLust)
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.StillYou'reTooHorny".Localized());
            }
            else if (gained <= 14)
            {
                context.AppendLog("Combat.MinotaurMob.PrecumTease.ThePowerfulPheromonesAnd".Localized());

                context.AppendLog(player.Lust >= player.MaxLust * 0.7f

                    ? "Combat.MinotaurMob.PrecumTease.ThoughYouDidn'tThink".Localized()

                    : "Combat.MinotaurMob.PrecumTease.FeelingLikeTwoBullets".Localized());

                context.AppendLog("Combat.MinotaurMob.PrecumTease.ButCouldHaveBeen".Localized());

                context.AppendLog(player.Lust >= player.MaxLust

                    ? "Combat.MinotaurMob.PrecumTease.FuckTheySmellGood".Localized()

                    : "Combat.MinotaurMob.PrecumTease.GrowingPartYouWants".Localized());
            }
            else
            {
                context.AppendLog("Combat.MinotaurMob.PrecumTease.AllThatPotentPre".Localized());

                if (player.Wetness() <= 1)
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.Moisten".Localized());
                else if (player.Wetness() <= 2)
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.Drip".Localized());
                else if (player.Wetness() <= 3)
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.Drool".Localized());
                else
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.JuiceItself".Localized());

                context.AppendLog("Combat.MinotaurMob.PrecumTease.Need".Localized());

                if (player.MinotaurNeed())
                {
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.YouNeedFixBad".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
                }
                else
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.YouCanUnderstandFirsthand".Localized());

                if (player.HasCock())
                    context.AppendLog("Combat.MinotaurMob.PrecumTease.Cock".Localized(player.SMultiCockDesc()));

                context.AppendLog("Combat.MinotaurMob.PrecumTease.ShudderingAndMoaningYour".Localized());

                context.AppendLog(player.Lust >= player.MaxLust

                    ? "Combat.MinotaurMob.PrecumTease.YouSighAndLet".Localized()

                    : "Combat.MinotaurMob.PrecumTease.YouPullBackFrom".Localized());
            }
        }
    }
}