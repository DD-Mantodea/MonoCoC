using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using System.Linq;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5761-5780 / :5909-5946 Terra 形态 Plate / Inflame
    public static class PlayerTerraFormHandlers
    {
        private static readonly Identifier TerraMastery = new("coc", "TerrestrialFire");

        [Register("coc:has_terrestrial_fire")]
        public static bool HasTerrestrialFire(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.TerrestrialFire);

        [Register("coc:can_tf_plate")]
        public static bool CanPlate(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.TerrestrialFire)

            && !TerraFormCombo.Combod(context.Player, "Plate");

        [Register("coc:can_tf_inflame")]
        public static bool CanInflame(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.TerrestrialFire)

            && !TerraFormCombo.Combod(context.Player, "Inflame");

        [Register("coc:tf_plate_active")]
        public static bool PlateActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.TFPlate);

        [Register("coc:tf_inflame_active")]
        public static bool InflameActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.TFInflame);

        private static float TfSpellMod(Player player)
        {
            var mastery = player.MasteryLevel(TerraMastery);

            return 1 + (player.SpellMod() - 1) * mastery / 10f;
        }

        [Register("coc:can_tf_molten_plate")]
        public static bool CanMoltenPlate(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 1

            && TerraFormCombo.ComboReady(context.Player, "Molten Plate")

            && !TerraFormCombo.Combod(context.Player, "Molten Plate");

        [Register("coc:tf_molten_plate_active")]
        public static bool MoltenPlateActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.TFMoltenPlate);

        public static float MoltenPlateLava(Player player, bool self)
        {
            var mastery = player.MasteryLevel(TerraMastery);

            var damage = (10 + player.Strength * mastery / 50f + player.Intelligence * mastery / 50f) * TfSpellMod(player);

            var selfDamage = damage / 2f * (0.4f + 60f / (100f + player.Toughness));

            var mod = 0.5f + Random.Shared.NextSingle() * 0.5f;

            return MathF.Round(self ? mod * selfDamage : mod * damage);
        }

        [Register("coc:tf_molten_plate")]
        public static void MoltenPlate(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var hadPlate = player.HasStatusEffect(StatusEffectIds.TFPlate);

            var armor = (int)MathF.Round(player.Strength / (25f - 2 * player.MasteryLevel(TerraMastery)) * TfSpellMod(player));

            var mastery = player.MasteryLevel(TerraMastery);

            var damage = (12 + player.Intelligence * mastery / 50f) * TfSpellMod(player);

            var spellblade = damage * (player.SpellbladeBonus() - 1);

            var selfDamage = Math.Max(player.MaxHealth * player.Intelligence / 5000f, damage / 2f) * (0.6f + 40f / (100f + player.Toughness));

            if (!player.HasStatusEffect(StatusEffectIds.TFPlate))
            {
                player.CreateStatusEffect(StatusEffectIds.TFPlate, armor, 0, 0, 0, 99);

                CombatCalculator.AddArmorBonus(player, armor);
            }
            else
                player.ChangeStatusValue(StatusEffectIds.TFPlate, 0, armor);

            if (!player.HasStatusEffect(StatusEffectIds.TFInflame))
                player.CreateStatusEffect(StatusEffectIds.TFInflame, damage, spellblade, selfDamage, 0, 99);
            else
            {
                player.ChangeStatusValue(StatusEffectIds.TFInflame, 0, damage);

                player.ChangeStatusValue(StatusEffectIds.TFInflame, 1, spellblade);

                player.ChangeStatusValue(StatusEffectIds.TFInflame, 2, selfDamage);
            }

            player.CreateStatusEffect(StatusEffectIds.TFMoltenPlate, 0, 0, 0, 0, 99);

            context.Append((hadPlate

                ? "Combat.Terra.MoltenPlate.Text1"

                : "Combat.Terra.MoltenPlate.Text2").Localized());

            player.MasteryXP(TerraMastery, 20);
        }

        [Register("coc:can_tf_shell")]
        public static bool CanShell(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 2

            && TerraFormCombo.ComboReady(context.Player, "Shell");

        [Register("coc:tf_shell_active")]
        public static bool ShellActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.TFShell);

        [Register("coc:tf_shell")]
        public static void Shell(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            player.CreateStatusEffect(StatusEffectIds.TFShell, 0, 0, 0, 0, 1);

            context.Append("Combat.Terra.Shell.Text".Localized());

            player.MasteryXP(TerraMastery, 30);
        }

        [Register("coc:can_tf_quake")]
        public static bool CanQuake(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 1;

        private static float QuakeDamage(CombatContext context)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(TerraMastery);

            var low = player.Strength / 10f;

            var high = Math.Max(low, CombatCalculator.GlobalMod(player, 5 * mastery) * TfSpellMod(player));

            var final = low + (high > low ? context.Random.NextSingle() * (high - low) : 0);

            return CombatCalculator.ReduceDamage(context, context.Enemy, final);
        }

        private static bool QuakeOnEnemy(CombatContext context, Enemy enemy, bool mainTarget, bool aftershocks, System.Text.StringBuilder text)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(TerraMastery);

            var damage = QuakeDamage(context);

            var stunChance = (5 + mastery) * (aftershocks ? 1 : 5);

            if (mainTarget)
            {
                damage *= 1.5f;

                if (aftershocks)
                    stunChance *= 2;
                else
                    stunChance = 100;
            }

            var stunned = enemy.Stun(0, stunChance);

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            text.Append(stunned

                ? "Combat.Terra.Quake.Stun".Localized()

                : "Combat.Terra.Quake.Retain".Localized());

            text.Append("Combat.Terra.Quake.Damage".Localized((int)damage));

            return stunned;
        }

        [Register("coc:tf_quake")]
        public static void Quake(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var mainTarget = context.Enemy;

            player.CreateStatusEffect(StatusEffectIds.TFQuake, 0, 0, 0, 0, 3);

            var text = new System.Text.StringBuilder();

            var didStun = false;

            CombatCalculator.ForEachEnemy(context, enemy =>

                didStun |= QuakeOnEnemy(context, enemy, enemy == mainTarget, false, text));

            context.Append("Combat.Terra.Quake.Intro".Localized());

            context.Append(didStun

                ? "Combat.Terra.Quake.TopplePlural".Localized()

                : "Combat.Terra.Quake.Topple".Localized());

            context.Append("[pg]");

            context.Append(text.ToString());

            player.MasteryXP(TerraMastery, 20);
        }

        [Register("coc:effect_tfquake_round")]
        public static void QuakeRound(CombatContext context, Creature host, StatusEffect status)
        {
            // 原版 TFQuakeBuff：施放当回合不触发，之后每回合结束余震
            if (status.Duration >= 3)
                return;

            context.Append("Combat.Terra.Quake.Aftershocks".Localized());

            var text = new System.Text.StringBuilder();

            CombatCalculator.ForEachEnemy(context, enemy =>

                QuakeOnEnemy(context, enemy, false, true, text));

            context.Append(text.ToString());
        }

        [Register("coc:can_tf_spout")]
        public static bool CanSpout(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 1

            && !TerraFormCombo.Combod(context.Player, "Spout");

        [Register("coc:can_tf_eruption")]
        public static bool CanEruption(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 2

            && TerraFormCombo.ComboReady(context.Player, "Eruption");

        private static float FireAreaDamage(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var mastery = player.MasteryLevel(TerraMastery);

            var low = InfernoModifiers.CalcInfernoMod(context,

                CombatCalculator.GlobalMod(player, 5 * mastery + player.Intelligence / 2f)) * TfSpellMod(player);

            var high = low + InfernoModifiers.CalcInfernoMod(context,

                CombatCalculator.GlobalMod(player, player.Intelligence * mastery / 10f)) * TfSpellMod(player);

            var damage = low + (high > low ? context.Random.NextSingle() * (high - low) : 0);

            damage = CombatCalculator.ReduceDamage(context, enemy, damage);

            return damage * enemy.FireResistance;
        }

        [Register("coc:tf_spout")]
        public static void Spout(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = FireAreaDamage(context);

            context.Append("Combat.Terra.Spout.Text".Localized());

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            player.MasteryXP(TerraMastery, 20);
        }

        [Register("coc:tf_eruption")]
        public static void Eruption(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Terra.Eruption.Text".Localized());

            InfernoModifiers.InfernoDisplay(context);

            CombatCalculator.ForEachEnemy(context, target =>
            {
                var damage = FireAreaDamage(context);

                context.Append("Combat.Terra.Eruption.Engulf".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
            });

            InfernoModifiers.BoostInferno(context);

            player.MasteryXP(TerraMastery, 30);
        }

        [Register("coc:can_tf_stone_knives")]
        public static bool CanStoneKnives(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 2

            && !TerraFormCombo.Combod(context.Player, "Stone Knives");

        [Register("coc:tf_stone_knives")]
        public static void StoneKnives(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var mastery = player.MasteryLevel(TerraMastery);

            var stones = 5 + 4 * mastery + context.Random.Next(Math.Max(1, 25 - (5 + 4 * mastery)));

            var hits = 0;

            var total = 0f;

            var accuracy = player.StandardDodgeFunc(enemy, -35 + 5 * mastery);

            for (var i = 0; i < stones; i++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = player, Defender = enemy,

                    DoDodge = true, DoParry = true, DoBlock = true,

                    ToHitChance = accuracy,
                });

                if (!avoid.AttackHit)
                    continue;

                var low = MathF.Round(CombatCalculator.GlobalMod(player, 1 + player.Strength / (30f - 2 * mastery)) * TfSpellMod(player));

                var high = (int)(low + MathF.Round(CombatCalculator.GlobalMod(player, player.Strength / (30f - 2 * mastery)) * TfSpellMod(player)));

                var damage = low + (high > low ? context.Random.NextSingle() * (high - low) : 0);

                total += damage;

                hits += 1;
            }

            var reduced = CombatCalculator.ReduceDamage(context, enemy, total);

            context.Append("Combat.Terra.StoneKnives.Intro".Localized());

            if (hits == 0)
                context.Append("Combat.Terra.StoneKnives.None".Localized());
            else if (hits * 2 < stones)
                context.Append("Combat.Terra.StoneKnives.Most".Localized(hits));
            else
                context.Append((reduced > 5

                    ? "Combat.Terra.StoneKnives.Hit"

                    : "Combat.Terra.StoneKnives.Unharmed").Localized(hits));

            CombatCalculator.DamageEnemy(context, reduced, CombatDamageKind.Magical);

            player.MasteryXP(TerraMastery, 30);
        }

        [Register("coc:can_tf_scorch")]
        public static bool CanScorch(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 2;

        [Register("coc:tf_scorch_active")]
        public static bool ScorchActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.TFScorch);

        public static float ScorchDamage(CombatContext context)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(TerraMastery);

            var low = InfernoModifiers.CalcInfernoMod(context,

                CombatCalculator.GlobalMod(player, 5 + 2 * mastery + player.Intelligence / 4f)) * TfSpellMod(player);

            var high = low + InfernoModifiers.CalcInfernoMod(context,

                CombatCalculator.GlobalMod(player, player.Intelligence * mastery / 15f)) * TfSpellMod(player);

            var damage = low + (high > low ? context.Random.NextSingle() * (high - low) : 0);

            damage = CombatCalculator.ReduceDamage(context, context.Enemy, damage);

            return damage * context.Enemy.FireResistance;
        }

        [Register("coc:tf_scorch")]
        public static void Scorch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Terra.Scorch.Text".Localized());

            player.CreateStatusEffect(StatusEffectIds.TFScorch, 0, 0, 0, 0, 99);

            InfernoModifiers.BoostInferno(context);

            player.MasteryXP(TerraMastery, 30);
        }

        [Register("coc:can_tf_meteor_shower")]
        public static bool CanMeteorShower(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 3

            && TerraFormCombo.ComboReady(context.Player, "Meteor Shower");

        [Register("coc:tf_meteor_shower")]
        public static void MeteorShower(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(TerraMastery);

            var stones = 10 * mastery + context.Random.Next(Math.Max(1, 50 - 10 * mastery));

            var enemyCount = Math.Max(1, context.Enemies.Count(e => e.Health > 0));

            var perEnemy = Math.Max(1, stones / enemyCount);

            context.Append("Combat.Terra.MeteorShower.Intro".Localized());

            CombatCalculator.ForEachEnemy(context, target => MeteorShowerOnTarget(context, player, target, perEnemy, mastery));

            player.MasteryXP(TerraMastery, 40);
        }

        private static void MeteorShowerOnTarget(CombatContext context, Player player, Enemy enemy, int stones, int mastery)
        {
            var accuracy = player.StandardDodgeFunc(enemy, -40 + 5 * mastery);

            var total = 0f;

            var hits = 0;

            var grounded = false;

            for (var i = 0; i < stones; i++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = player, Defender = enemy,

                    DoDodge = true, DoParry = true, DoBlock = true,

                    ToHitChance = accuracy,
                });

                if (!avoid.AttackHit)
                    continue;

                var low = MathF.Round(CombatCalculator.GlobalMod(player, 1 + player.Strength / (30f - 2 * mastery)) * TfSpellMod(player));

                var high = (int)(low + MathF.Round(CombatCalculator.GlobalMod(player, player.Strength / (30f - 2 * mastery)) * TfSpellMod(player)));

                var damage = low + (high > low ? context.Random.NextSingle() * (high - low) : 0);

                var bonus = 0;

                if (enemy.IsFlying)
                {
                    bonus = 1 + context.Random.Next(3);

                    if (hits + bonus > 4)
                        grounded = true;
                }

                hits += 1 + bonus;

                total += damage * (1 + bonus);
            }

            var reduced = CombatCalculator.ReduceDamage(context, enemy, total);

            if (grounded && enemy.Stun(1, 100))
                context.Append("Combat.Terra.MeteorShower.Grounded".Localized(hits));
            else if (hits == 0)
                context.Append("Combat.Terra.MeteorShower.None".Localized());
            else
                context.Append((reduced > 5

                    ? "Combat.Terra.MeteorShower.Hit"

                    : "Combat.Terra.MeteorShower.Unharmed").Localized(hits));

            CombatCalculator.DamageEnemy(context, reduced, CombatDamageKind.Magical);

            player.MasteryXP(TerraMastery, 40);
        }

        private static readonly Identifier GeodeKnuckleId = new("coc", "geode_knuckle");

        [Register("coc:can_tf_geode_knuckle")]
        public static bool CanGeodeKnuckle(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 3;

        [Register("coc:not_enough_gems")]
        public static bool NotEnoughGems(CombatContext context, CombatState state) =>

            context.Player.Gems < 10;

        [Register("coc:has_geode_knuckle")]
        public static bool HasGeodeKnuckle(CombatContext context, CombatState state) =>

            context.Player.Weapon.Id == GeodeKnuckleId;

        [Register("coc:tf_geode_knuckle")]
        public static void GeodeKnuckle(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            player.Gems -= 10;

            var mastery = player.MasteryLevel(TerraMastery);

            var mod = 1.3f + 0.35f * (mastery - 3);

            context.Append("Combat.Terra.GeodeKnuckle.Text".Localized());

            player.CreateStatusEffect(StatusEffectIds.TFGeodeKnuckle, 0, 0, 0, 0, 99);

            var previous = player.Weapon;

            var wasUnarmed = player.Weapon.IsUnarmed();

            var knuckle = Identifier.Parse("coc:geode_knuckle");

            var weapon = Items.ItemRegistry.Create(knuckle) as Items.Weapon;

            if (weapon != null)
                player.SetWeapon(weapon);

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * mod;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            if (!wasUnarmed)
                player.SetWeapon(previous);

            player.MasteryXP(TerraMastery, 40);
        }

        [Register("coc:tf_geode_dispel")]
        public static void GeodeDispel(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Terra.GeodeDispel.Text".Localized());

            context.Player.SetUnarmed();
        }

        private static float CarnalBurnSelf(Player player) =>

            player.Lust / 2f * player.MaxHealth / 100f;

        [Register("coc:can_tf_carnal_burn")]
        public static bool CanCarnalBurn(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 3;

        [Register("coc:carnal_burn_lethal")]
        public static bool CarnalBurnLethal(CombatContext context, CombatState state) =>

            CarnalBurnSelf(context.Player) >= context.Player.Health;

        [Register("coc:tf_carnal_burn")]
        public static void CarnalBurn(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var aoe = player.Lust / player.MaxLust * 100f > 70f;

            context.Append("Combat.Terra.CarnalBurn.Intro".Localized());

            context.Append(aoe

                ? "Combat.Terra.CarnalBurn.All".Localized()

                : "Combat.Terra.CarnalBurn.Single".Localized());

            InfernoModifiers.InfernoDisplay(context);

            var damage = CarnalBurnSelf(player);

            damage = InfernoModifiers.CalcInfernoMod(context, CombatCalculator.GlobalMod(player, damage)) * TfSpellMod(player);

            damage *= enemy.FireResistance;

            if (aoe)
                CombatCalculator.ForEachEnemy(context, target =>
                {
                    context.Append("Combat.Terra.CarnalBurn.Burnt".Localized());

                    CombatCalculator.DamageEnemy(context, damage * target.FireResistance, CombatDamageKind.Magical);
                });
            else
                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            player.HealthChange(-CarnalBurnSelf(player));

            InfernoModifiers.BoostInferno(context);

            player.MasteryXP(TerraMastery, 40);
        }

        private static int TerraCoreAccuracy(Player player, Enemy enemy)
        {
            var mastery = player.MasteryLevel(TerraMastery);

            return (int)player.StandardDodgeFunc(enemy, mastery >= 5 ? 0 : -10);
        }

        private static float TerraCoreDamage(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var mastery = player.MasteryLevel(TerraMastery);

            var level = Math.Min(player.Level, 30);

            var low = CombatCalculator.GlobalMod(player,

                25 + player.Intelligence / 2f + player.Strength / 2f + (mastery >= 5 ? level * 3 : 0)) * TfSpellMod(player);

            var high = low + CombatCalculator.GlobalMod(player, player.Intelligence / 3f + player.Strength / 3f) * TfSpellMod(player);

            var damage = low + (high > low ? context.Random.NextSingle() * (high - low) : 0);

            var adjustedResist = 1 + (enemy.FireResistance - 1) / 4f;

            return damage * adjustedResist;
        }

        [Register("coc:can_tf_terra_core")]
        public static bool CanTerraCore(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 4

            && !TerraFormCombo.Combod(context.Player, "Terra Core");

        [Register("coc:tf_terra_core")]
        public static void TerraCore(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = TerraCoreDamage(context);

            context.Append("Combat.Terra.TerraCore.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = TerraCoreAccuracy(player, enemy),
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Terra.TerraCore.Miss".Localized());

                player.CreateStatusEffect(StatusEffectIds.TFTerraCore, 0, 0, 0, 0, 2);
            }
            else
            {
                context.Append("Combat.Terra.TerraCore.Hit".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
            }

            player.MasteryXP(TerraMastery, 60);
        }

        [Register("coc:effect_terra_core_round")]
        public static void TerraCoreRound(CombatContext context, Creature host, StatusEffect status)
        {
            if (status.Duration >= 2)
                return;

            host.RemoveStatusEffect(StatusEffectIds.TFTerraCore);

            var player = context.Player;

            var enemy = context.Enemy;

            var damage = TerraCoreDamage(context);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = TerraCoreAccuracy(player, enemy),
            });

            if (avoid.AttackFailed)
                context.Append("Combat.Terra.TerraCore.RetryMiss".Localized());
            else if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.Terra.TerraCore.RetryWeak".Localized());

                CombatCalculator.DamageEnemy(context, damage * 0.5f, CombatDamageKind.Magical);
            }
            else
            {
                context.Append("Combat.Terra.TerraCore.RetryHit".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
            }
        }

        [Register("coc:can_tf_terra_flames")]
        public static bool CanTerraFlames(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 4

            && !TerraFormCombo.Combod(context.Player, "Terra Flames");

        [Register("coc:tf_terra_flames")]
        public static void TerraFlames(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var mastery = player.MasteryLevel(TerraMastery);

            var level = Math.Min(player.Level, 30);

            context.Append("Combat.Terra.TerraFlames.Text".Localized());

            InfernoModifiers.InfernoDisplay(context);

            var baseDamage = CombatCalculator.GlobalMod(player,

                25 + player.Intelligence / 2f + (mastery >= 5 ? level * 3 : 0)) * TfSpellMod(player);

            CombatCalculator.ForEachEnemy(context, target =>
            {
                var damage = CombatCalculator.ReduceDamage(context, target, baseDamage) * target.FireResistance;

                context.Append("Combat.Terra.TerraFlames.Wreathed".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
            });

            InfernoModifiers.BoostInferno(context);

            player.MasteryXP(TerraMastery, 60);
        }

        [Register("coc:can_tf_terra_star")]
        public static bool CanTerraStar(CombatContext context, CombatState state) =>

            context.Player.MasteryLevel(TerraMastery) >= 5

            && TerraFormCombo.ComboReady(context.Player, "Terra Star")

            && !context.Player.HasStatusEffect(StatusEffectIds.TFTerraStar);

        private static float GameTimeHours =>

            Scenes.Scene.View.Time.TotalTime;

        [Register("coc:tf_terra_star_disabled")]
        public static bool TerraStarDisabled(CombatContext context, CombatState state) =>

            context.Player.Fatigue / context.Player.MaxFatigue() > 0.4f

            || context.Player.HasStatusEffect(StatusEffectIds.TFTerraStarCooldown)

            || context.Player.TerraStarCooldownUntil > GameTimeHours;

        public static float TerraStarDamage(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var mastery = player.MasteryLevel(TerraMastery);

            var level = Math.Min(player.Level, 20);

            var baseDamage = 4f / (1f / (level * 5f) + 1f / player.Strength + 1f / player.Intelligence

                + 1f / (TfSpellMod(player) * 50f)) * mastery;

            var damage = MathF.Pow(baseDamage, 3) * MathF.Pow(600, 1 - 3);

            damage *= 0.8f + context.Random.NextSingle() * 0.4f;

            var firePortion = player.Intelligence / (player.Strength + player.Intelligence);

            var adjustedResist = 1 + (enemy.FireResistance - 1) * firePortion;

            return damage * adjustedResist;
        }

        [Register("coc:tf_terra_star")]
        public static void TerraStar(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Terra.TerraStar.Text1".Localized());

            context.Append("Combat.Terra.TerraStar.Text2".Localized());

            player.Fatigue = player.MaxFatigue();

            player.HealthChange(-player.Health / 2f);

            player.CreateStatusEffect(StatusEffectIds.TFTerraStar, 0, 0, 0, 0, 99);

            var hoursLeft = 24 - Scenes.Scene.View.Time.Hours;

            player.CreateStatusEffect(StatusEffectIds.TFTerraStarCooldown, 0, 0, 0, 0, Math.Max(1, hoursLeft));

            player.TerraStarCooldownUntil = GameTimeHours + 120;

            player.RemoveStatusEffect(StatusEffectIds.TFQuake);

            player.RemoveStatusEffect(StatusEffectIds.TFScorch);

            player.MasteryXP(TerraMastery, 70);
        }

        [Register("coc:effect_terra_star_round")]
        public static void TerraStarRound(CombatContext context, Creature host, StatusEffect status)
        {
            if (host.HasStatusEffect(StatusEffectIds.TFSupercharging))
                return;

            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Health <= 0)
                return;

            var damage = TerraStarDamage(context);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = true, DoBlock = true,

                ToHitChance = player.StandardDodgeFunc(enemy, -40),
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Dodge:
                    context.Append("Combat.Terra.TerraStar.AttackDodge".Localized());

                    return;

                case AvoidOutcome.Parry:
                    context.Append("Combat.Terra.TerraStar.AttackParry".Localized());

                    damage *= 0.4f;

                    break;

                case AvoidOutcome.Block:
                    context.Append("Combat.Terra.TerraStar.AttackBlock".Localized());

                    damage *= 0.6f;

                    break;

                default:
                    context.Append("Combat.Terra.TerraStar.AttackHit".Localized());

                    break;
            }

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
        }

        private static void GainXp(Player player) =>

            player.MasteryXP(TerraMastery, 10);

        [Register("coc:tf_plate")]
        public static void Plate(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(TerraMastery);

            var armor = (int)MathF.Round(player.Strength / (25f - 2 * mastery) * TfSpellMod(player));

            player.CreateStatusEffect(StatusEffectIds.TFPlate, armor, 0, 0, 0, 99);

            CombatCalculator.AddArmorBonus(player, armor);

            context.Append("Combat.Terra.Plate.Text".Localized());

            GainXp(player);
        }

        [Register("coc:effect_tf_plate_expire")]
        public static void PlateExpire(CombatContext context, Creature host, StatusEffect status)
        {
            host.ArmorDefense = Math.Max(0, host.ArmorDefense - (int)status.Values[0]);

            context.Append("Combat.Effect.TFPlate.Expire".Localized());
        }

        [Register("coc:tf_inflame")]
        public static void Inflame(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(TerraMastery);

            var damage = (12 + player.Intelligence * mastery / 50f) * TfSpellMod(player);

            var spellblade = damage * (player.SpellbladeBonus() - 1);

            var selfDamage = Math.Max(player.MaxHealth * player.Intelligence / 5000f, damage / 2f);

            selfDamage *= 0.6f + 40f / (100f + player.Toughness);

            player.CreateStatusEffect(StatusEffectIds.TFInflame, damage, spellblade, selfDamage, 0, 99);

            context.Append("Combat.Terra.Inflame.Text".Localized());

            GainXp(player);
        }

        [Register("coc:effect_tf_inflame_round")]
        public static void InflameRound(CombatContext context, Creature host, StatusEffect status)
        {
            context.Append("Combat.Effect.TFInflame.Burning".Localized());

            host.HealthChange(-status.Values[2]);
        }
    }
}