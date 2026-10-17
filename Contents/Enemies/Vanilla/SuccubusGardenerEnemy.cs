using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SuccubusGardenerEnemy>("coc:succubus_gardener")]
    public class SuccubusGardenerEnemy : Enemy
    {

        public static readonly Identifier TentagrappleCooldownId = new("coc", "tentagrapple_cooldown");

        public static readonly Identifier TentagrappledId = new("coc", "tentagrappled");

        public static readonly Identifier ShowerDotEffectId = new("coc", "shower_dot_effect");

        public static readonly Identifier GardenerSapSpeedId = new("coc", "gardener_sap_speed");

        public static readonly Identifier VineHealUsedId = new("coc", "vine_heal_used");

        public static readonly Identifier VineHealId = new("coc", "succubus_gardener_vine_heal");

        public static readonly Identifier TentagrappleId = new("coc", "succubus_gardener_tentagrapple");

        public static readonly Identifier SapSpeedId = new("coc", "succubus_gardener_sap_speed");

        public static readonly Identifier SicemId = new("coc", "succubus_gardener_sicem");

        public static readonly Identifier CorruptiveShowerId = new("coc", "succubus_gardener_corruptive_shower");

        public static readonly Identifier LustAuraCastId = new("coc", "succubus_gardener_lust_aura");

        public static readonly Identifier TauntId = new("coc", "succubus_gardener_taunt");

        public static readonly Identifier MotorboatId = new("coc", "succubus_gardener_motorboat");

        public static readonly Identifier TasteTheEcstasyId = new("coc", "succubus_gardener_taste_the_ecstasy");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Race = "Demon";

            CreateVagina(false, 3, 3);

            CreateBreastRow(13f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 3;

            anal.Wetness = 0;

            Tallness = 96;

            Body.Hips = 4;

            Body.Butt = 2;

            WeaponName = "tentacles";

            WeaponVerb = "lash";

            WeaponAttack = 22;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "tentaclothes";

            Strength = 85;

            Toughness = 60;

            Speed = 85;

            Intelligence = 100;

            Libido = 85;

            Sensitivity = 60;

            Corruption = 100;

            var bonusHealth = 600f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Fatigue = 0;

            Gems = Random.Shared.Next(33) + 50;

            Level = 20;

            LustVulnerability = 0;

            CreateStatusEffect(TentagrappleCooldownId, 10);
        }

        public override bool HandleStun(CombatContext context)
        {
            if (Health <= MaxHealth * 0.6f)
                return true;

            return base.HandleStun(context);
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (HasStatusEffect(TentagrappleCooldownId))
            {
                ChangeStatusValue(TentagrappleCooldownId, 0, -1);

                if (StatusEffectValue(TentagrappleCooldownId, 0) <= 0)
                    RemoveStatusEffect(TentagrappleCooldownId);
            }

            if (context.Player.HasStatusEffect(ShowerDotEffectId))
            {
                SuccubusGardenerCombatHandlers.ShowerDotEffect(context);

                if (context.Player.Lust >= context.Player.MaxLust)
                    return true;
            }

            return false;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (Health <= MaxHealth * 0.6f && Fatigue < 100)
                return new CombatAiAction { Ability = VineHealId };

            if (!HasStatusEffect(TentagrappleCooldownId))
                return new CombatAiAction { Ability = TentagrappleId };

            if (!context.Player.HasStatusEffect(GardenerSapSpeedId) && HasStatusEffect(VineHealUsedId))
                return new CombatAiAction { Ability = SapSpeedId };

            var options = new List<Identifier> { SicemId, CorruptiveShowerId, LustAuraCastId };

            if (Lust < 40)
                options.Add(TauntId);

            if (Lust >= 40)
            {
                options.Add(MotorboatId);

                options.Add(TasteTheEcstasyId);
            }

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        public override void Struggle(CombatContext context) =>

            SuccubusGardenerCombatHandlers.Struggle(context);

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            if (!context.Player.HasStatusEffect(TentagrappledId))
                return true;

            SuccubusGardenerCombatHandlers.Squeeze(context);

            TookAction = true;

            return false;
        }

        public void CleanupEffects()
        {
            var player = CoC.Instance.Player;

            if (player.HasStatusEffect(TentagrappledId))
                player.RemoveStatusEffect(TentagrappledId);

            if (player.HasStatusEffect(ShowerDotEffectId))
                player.RemoveStatusEffect(ShowerDotEffectId);

            if (player.HasStatusEffect(GardenerSapSpeedId))
                player.RemoveStatusEffect(GardenerSapSpeedId);
        }

        public override bool Defeated(bool hpVictory)
        {
            CleanupEffects();

            Scenes.SceneInstance<Scenes.Vanilla.Dungeons.LethicesKeep.SuccubusGardenerScenes>.Get()
                .FuckUpTheGardener(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            CleanupEffects();

            Scenes.SceneInstance<Scenes.Vanilla.Dungeons.LethicesKeep.SuccubusGardenerScenes>.Get()
                .SurrenderToTheGardener(hpVictory);

            return true;
        }
    }
}
