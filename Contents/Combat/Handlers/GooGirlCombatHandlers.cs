using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GooGirlCombatHandlers
    {

        [Register("coc:goo_girl_goo_play")]
        public static void GooPlay(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.GooGirl.Play.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 3 + context.Random.Next(3) + player.Sensitivity / 10);
        }

        public static void GooThrow(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.GooGirl.GooThrow.Text".Localized());

            player.HealthChange(-1);

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + context.Random.Next(3) + player.Sensitivity / 10);
        }

        [Register("coc:goo_girl_goo_engulph")]
        public static void GooEngulph(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.GooGirl.Engulf.Lead".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.GooBind))
                player.CreateStatusEffect(StatusEffectIds.GooBind);
        }

        [Register("coc:goo_girl_goo_gal_attack")]
        public static void GooGalAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var acid = enemy.HasPerk(PerkIds.Acid);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoFatigue = true
            });

            context.Append((acid

                ? "Combat.GooGirl.GooGalAttack.AcidLead"

                : "Combat.GooGirl.GooGalAttack.Text").Localized());

            var avoidKey = avoid.Outcome switch
            {
                AvoidOutcome.Dodge when avoid.DodgeReason == "Speed" =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidDodgeSpeed" : "Combat.GooGirl.GooGalAttack.DodgeSpeed",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Evade" =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidDodgeEvade" : "Combat.GooGirl.GooGalAttack.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidDodgeMisdirection" : "Combat.GooGirl.GooGalAttack.DodgeMisdirection",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidDodgeFlexibility" : "Combat.GooGirl.GooGalAttack.DodgeFlexibility",

                AvoidOutcome.Dodge =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidDodgeOther" : "Combat.GooGirl.GooGalAttack.Miss",

                AvoidOutcome.Parry =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidParry" : "Combat.GooGirl.GooGalAttack.Parry",

                AvoidOutcome.Block =>

                    acid ? "Combat.GooGirl.GooGalAttack.AcidBlock" : "Combat.GooGirl.GooGalAttack.Blocked",

                _ => null
            };

            if (avoidKey != null)
            {
                context.Append(avoidKey.Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + (acid ? 10f : 0f));

            if (damage <= 0)
            {
                damage = 0;

                var absorbed = context.Random.Next(Math.Max(1, (int)(player.ArmorDefense + player.Toughness))) < player.ArmorDefense;

                context.Append((acid

                    ? absorbed

                        ? "Combat.GooGirl.GooGalAttack.AcidAbsorbedByArmor"

                        : "Combat.GooGirl.GooGalAttack.AcidDeflected"

                    : absorbed

                        ? "Combat.GooGirl.GooGalAttack.AbsorbedByArmor"

                        : "Combat.GooGirl.GooGalAttack.Deflected").Localized());
            }
            else if (acid)
            {
                context.Append("Combat.GooGirl.GooGalAttack.AcidHit".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.AcidSlap))
                    player.CreateStatusEffect(StatusEffectIds.AcidSlap);
            }
            else
                context.Append("Combat.GooGirl.GooGalAttack.Hit".Localized());

            if (damage > 0)
            {

                if (enemy.LustVulnerability > 0 && player.Armor.Id == ArmorIds.BondageStraps)
                {
                    var who = enemy.Article + enemy.ShortName;

                    who = char.ToUpperInvariant(who[0]) + who[1..];

                    context.Append((enemy.Plural

                        ? "Combat.GooGirl.GooGalAttack.BondageLustPlural"

                        : "Combat.GooGirl.GooGalAttack.BondageLust").Localized(who));

                    enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5 * enemy.LustVulnerability);
                }

                player.HealthChange(-damage);
            }
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.GooBind))
                return;

            context.Clear();

            if (context.Random.Next(3) == 0 || context.Random.Next(80) < player.Strength)
            {
                context.Append("Combat.GooGirl.Struggle.Success".Localized());

                player.RemoveStatusEffect(StatusEffectIds.GooBind);

                return;
            }

            context.Append("Combat.GooGirl.Struggle.Fail".Localized());

            player.HealthChange(-Math.Min(0.15f * player.MaxHealth, 100f));
        }

        public static bool OnPlayerWaited(CombatContext context)
        {
            if (!context.Player.HasStatusEffect(StatusEffectIds.GooBind))
                return true;

            context.Clear();

            context.Append("Combat.GooGirl.Struggle.Fail".Localized());

            context.Player.HealthChange(-(0.35f * context.Player.MaxHealth));

            return false;
        }

        public static void OnBurned(CombatContext context)
        {
            context.Append("Combat.GooGirl.Burned".Localized());

            if (!context.Enemy.HasPerk(PerkIds.Acid))
                context.Enemy.CreatePerk(PerkIds.Acid);
        }
    }
}