using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class CorruptedWitchCombatHandlers
    {

        [Register("coc:corrupted_witch_shell_defense")]
        public static void ShellDefense(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.CorruptedWitch.Shell.Lead".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.Shell, 3);
        }

        [Register("coc:corrupted_witch_cum_hunger_attack")]
        public static void CumHungerAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.CorruptedWitch.CumHunger.Lead".Localized());

            if (player.Pregnancy.Incubation > 0)
            {
                context.Append("Combat.CorruptedWitch.CumHunger.Pregnant".Localized());

                return;
            }

            context.Append("Combat.CorruptedWitch.CumHunger.Hit".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 12);
        }

        [Register("coc:corrupted_witch_cum_magic_attack")]
        public static void CumMagicAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.CorruptedWitch.CumMagic.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 12);

            player.HoursSinceCum += 100;
        }

        [Register("coc:corrupted_witch_heals")]
        public static void Heals(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.CorruptedWitch.Heals.Lead".Localized());

            var cum = player.CumQ();

            float healAmount;

            if (cum < 100)
            {
                context.Append("Combat.CorruptedWitch.Heals.Weak".Localized());

                healAmount = 0.1f;
            }
            else if (cum < 500)
            {
                context.Append("Combat.CorruptedWitch.Heals.Normal".Localized());

                healAmount = 0.2f;
            }
            else
            {
                context.Append("Combat.CorruptedWitch.Heals.Huge".Localized());

                healAmount = 0.3f;
            }

            var healed = (int)(enemy.MaxHealth * healAmount);

            enemy.AddHp(healed);

            context.Append("Combat.CorruptedWitch.Heals.Healed".Localized());

            enemy.Fatigue += 10;
        }

        [Register("coc:corrupted_witch_force_queen")]
        public static void ForceQueen(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.CorruptedWitch.Queen.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            var dodgeKey = avoid.Outcome switch
            {
                AvoidOutcome.Dodge when avoid.DodgeReason == "Evade" => "Combat.CorruptedWitch.Queen.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.CorruptedWitch.Queen.DodgeMisdirection",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.CorruptedWitch.Queen.DodgeFlexibility",

                AvoidOutcome.Dodge => "Combat.CorruptedWitch.Queen.DodgeSpeed",

                _ => null
            };

            if (dodgeKey != null)
            {
                context.Append(dodgeKey.Localized());

                return;
            }

            context.Append("Combat.CorruptedWitch.Queen.Hit".Localized());

            context.Append("Combat.CorruptedWitch.Queen.Pinned".Localized());

            if (!player.HasPerk(PerkIds.Juggernaut))
            {
                player.HealthChange(-(2 + context.Random.Next(20)));

                player.CreateStatusEffect(StatusEffectIds.CorrWitchBind);
            }
            else
                context.Append("Combat.CorruptedWitch.Queen.Juggernaut".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 10);

            enemy.Lust = Math.Max(0, enemy.Lust - 5);
        }

        public static void GrabStruggle(CombatContext context)
        {
            if (context.Random.Next(100) >= context.Player.Strength || context.Random.Next(10) == 0)
            {
                GrabFail(context, true);

                return;
            }

            GrabSuccess(context);
        }

        public static void GrabSuccess(CombatContext context)
        {
            var player = context.Player;

            context.Clear();

            context.Append(player.Strength >= 100

                ? "Combat.CorruptedWitch.GrabSuccess.Str100".Localized()

                : "Combat.CorruptedWitch.GrabSuccess.Normal".Localized());

            player.RemoveStatusEffect(StatusEffectIds.CorrWitchBind);
        }

        public static void GrabFail(CombatContext context, bool struggle = true)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Clear();

            if (struggle)
            {
                var key = player.Strength >= 80 ? "Combat.CorruptedWitch.GrabFail.Str80"

                    : player.Strength >= 60 ? "Combat.CorruptedWitch.GrabFail.Str60"

                    : "Combat.CorruptedWitch.GrabFail.StrLow";

                context.Append(key.Localized());
            }
            else
                context.Append("Combat.CorruptedWitch.GrabFail.Relax".Localized());

            enemy.Lust = Math.Max(0, enemy.Lust - 5);

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 10);

            player.HealthChange(-20);

            context.Append("[pg]");
        }

        [Register("coc:corrupted_witch_breed_trance")]
        public static void BreedTrance(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var inte = player.Intelligence;

            context.Append("Scenes.CorruptedWitch.BreedTrance.GesturesSeries".Localized());

            if (player.HasCock())
            {
                if (inte < 100)
                    context.Append("Scenes.CorruptedWitch.BreedTrance.ThrustsWildly".Localized());

                if (inte < 80)
                    context.Append("Scenes.CorruptedWitch.BreedTrance.LetsGroan".Localized());

                if (inte < 60)
                    context.Append("Scenes.CorruptedWitch.BreedTrance.LustRemains".Localized(
                        player.Cocks.Count > 1 ? "[cocks] are" : "[cock] is"));

                if (inte < 40)
                {
                    context.Append("Scenes.CorruptedWitch.BreedTrance.ConcoctionRevives".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.UnsheathesSelf".Localized());
                }

                if (inte < 20)
                {
                    context.Append("Scenes.CorruptedWitch.BreedTrance.MindlessNod".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.ErectAgain".Localized(
                        player.Cocks.Count > 1 ? "[cocks] stand" : "[cock] stands"));

                    context.Append("Scenes.CorruptedWitch.BreedTrance.NoResistance".Localized());

                    context.EndOfRoundOverride = () =>

                        Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.CorruptedWitchScene>.Get().MindControlBadEnd();
                }
                else
                    context.Append("Scenes.CorruptedWitch.BreedTrance.WaitWhatGoingOn".Localized());

                return;
            }

            if (player.HasVagina())
            {
                if (inte < 100)
                {
                    context.Append("Scenes.CorruptedWitch.BreedTrance.WakesInBed".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.MoansIncessantly".Localized());
                }

                if (inte < 80)
                {
                    context.Append("Scenes.CorruptedWitch.BreedTrance.ReflexOrgasm".Localized());

                    if (player.HasTailInsteadOfLegs())
                        context.Append("Scenes.CorruptedWitch.BreedTrance.CoilAround".Localized());

                    if (player.IsGoo())
                        context.Append("Scenes.CorruptedWitch.BreedTrance.EnvelopWitch".Localized());
                    else
                        context.Append("Scenes.CorruptedWitch.BreedTrance.LockWitch".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.ContractsMilks".Localized());
                }

                if (inte < 60)
                    context.Append("Scenes.CorruptedWitch.BreedTrance.RaisedDragged".Localized());

                if (inte < 40)
                {
                    context.Append("Scenes.CorruptedWitch.BreedTrance.CaressesAbdomen".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.OlderWitch".Localized());
                }

                if (inte < 20)
                {
                    context.Append("Scenes.CorruptedWitch.BreedTrance.ManyAsYouCan".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.TinglesAgain".Localized());

                    context.Append("Scenes.CorruptedWitch.BreedTrance.NoResistanceAgain".Localized());

                    context.EndOfRoundOverride = () =>

                        Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.CorruptedWitchScene>.Get().MindControlBadEnd();
                }
                else
                    context.Append("Scenes.CorruptedWitch.BreedTrance.WhatGoingOnAgain".Localized());

                return;
            }

            context.Append("Scenes.CorruptedWitch.BreedTrance.RegainsComposure".Localized());
        }
    }
}