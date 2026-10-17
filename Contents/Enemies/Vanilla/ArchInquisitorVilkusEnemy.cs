using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Contents.Combat;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ArchInquisitorVilkusEnemy>("coc:arch_inquisitor_vilkus")]
    public class ArchInquisitorVilkusEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(6, 1.5f, BodyPartTypes.Human);

            Balls = 0;

            BallSize = 0;

            Tallness = 9 * 6;

            Body.Get<SkinPart>().Tone = "ashen";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "estoc";

            ArmorName = "Inquisitor's Robes";
        }

        public bool SecondPhase;

        public override bool Defeated(bool hpVictory)
        {
            if (!SecondPhase)
            {
                SecondPhase = true;

                CombatController.Instance?.Context.Append("Combat.Hel.VilkusFirstPhase".Localized());

                Stats.AddModifier(GameStats.MaxHealth, Contents.Stats.ModifierOperation.Addition,

                    () => 2400f, new Stats.ModifierSources.TemporarySource(Guid.NewGuid()));

                Health = MaxHealth;

                ArmorDefense = 60;

                Level = 35;

                WeaponAttack = 40;

                Corruption = 100;

                LustVulnerability = 0.7f;

                Lust = 0;

                ShortName = "Vilkus, the Last Flame";

                RemoveStatusEffect(StatusEffectIds.ChargeWeapon);

                Health = MaxHealth;

                return true;
            }

            CoC.Instance.Flags.Set(GameFlagIds.VilkusDefeatDay, Scenes.Scene.View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress,

                CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) | 256);

            CoC.Instance.Player.CreateKeyItem(Items.KeyItemIds.TalismanOfFlame);

            CoC.Instance.Player.UpgradeBeautifulSword();

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.WizardTower.DefeatedByVilkus.FallIncapableOfCombat".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.DefeatedByVilkus.TwirlsHisEstoc".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.VilkusGameOver.AfterLifeSnuffedOut".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.VilkusGameOver.GustOfWindEnters".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.VilkusGameOver.SitsOnWeatheredThrone".Localized());

            Scenes.Scene.GameOver();

            return true;
        }

        public static readonly MonoCoC.Registry.Identifier WhitefireId = new("coc", "arch_inquisitor_vilkus_whitefire");

        public static readonly MonoCoC.Registry.Identifier BlindId = new("coc", "arch_inquisitor_vilkus_blind");

        public static readonly MonoCoC.Registry.Identifier ArouseId = new("coc", "arch_inquisitor_vilkus_arouse");

        public static readonly MonoCoC.Registry.Identifier HealId = new("coc", "arch_inquisitor_vilkus_heal");

        public static readonly MonoCoC.Registry.Identifier ChargeId = new("coc", "arch_inquisitor_vilkus_charge_weapon");

        public static readonly MonoCoC.Registry.Identifier EnergyWaveId = new("coc", "arch_inquisitor_vilkus_energy_wave");

        public static readonly MonoCoC.Registry.Identifier AttackId = new("coc", "arch_inquisitor_vilkus_attack");

        public static readonly MonoCoC.Registry.Identifier SacrificeId = new("coc", "arch_inquisitor_vilkus_sacrifice");

        public override bool HandleFear(CombatContext context)
        {
            context.Append("Combat.Hel.VilkusFear".Localized());

            CombatController.Instance!.Resolver.EnemyBasicAttack();

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.Blinded && Random.Shared.Next(Math.Max(1, (int)context.Player.Intelligence)) > Random.Shared.Next(Math.Max(1, (int)Intelligence)))
            {
                context.Append("Combat.Hel.VilkusReflectBlind".Localized());

                CreateStatusEffect(StatusEffectIds.Blind, 5 * (int)context.Player.SpellMod() + 2, 0, 0, 0);

                return false;
            }

            return true;
        }

        public override void HandleDamaged(CombatContext context, float damage, CombatDamageKind kind)
        {
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (SecondPhase && !HasStatusEffect(StatusEffectIds.Attacks))
                CreateStatusEffect(StatusEffectIds.Attacks, 2, 0, 0, 0);

            if (Lust >= 100)
                return new CombatAiAction { Ability = SacrificeId };

            if (Health <= MaxHealth * 0.2f && !SecondPhase)
                return new CombatAiAction { Ability = HealId };

            if (context.Player.Lust / context.Player.MaxLust >= 0.5f && Random.Shared.Next(3) == 0)
                return new CombatAiAction { Ability = ArouseId };

            if (Health >= 200 && Random.Shared.Next(2) == 0)
                return new CombatAiAction { Ability = WhitefireId };

            if (!context.Player.HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(4) == 0)
                return new CombatAiAction { Ability = BlindId };

            if (!HasStatusEffect(StatusEffectIds.ChargeWeapon))
                return new CombatAiAction { Ability = ChargeId };

            if (SecondPhase && Random.Shared.Next(2) == 0)
                return new CombatAiAction { Ability = EnergyWaveId };

            return new CombatAiAction { Ability = AttackId };
        }

    }
}