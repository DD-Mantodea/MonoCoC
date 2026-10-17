using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Combat.Handlers;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AkbalUnsealedEnemy>("coc:akbal_unsealed")]
    public class AkbalUnsealedEnemy : Enemy
    {

        public float RageBonus;

        public float RageMod => 1f + 0.05f * CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + RageBonus;

        public int MeleeActions;

        public int RangedActions;

        public int MagicActions;

        public int Dodges;

        public int GotStunned;

        public float AttemptStunned;

        public int PsiAssaultCooldown;

        public int TerraFireCooldown;

        public int SwoopCooldown;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(15f, 2.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 4;

            CreateBreastRow(0f);

            Tallness = 95;

            Body.SetHipsRating("slender");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "dark red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 36;

            WeaponName = "fist";

            WeaponVerb = "punch";

            WeaponAttack = 30;

            ArmorName = "demonic skin";

            ArmorDefense = 30;

            LustVulnerability = 0.2f;

            Gems = (int)Math.Round((500 + Random.Shared.Next(100)) * (1 + 0.15f * CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery)));

            CreatePerk(PerkIds.BlindImmune);

            CreatePerk(PerkIds.StunImmune);

            CreatePerk(PerkIds.SpeedyRecovery);

            CreatePerk(PerkIds.Frustration);
        }

        public override void HandleDamaged(CombatContext context, float damage, CombatDamageKind kind)
        {
            switch (kind)
            {
                case CombatDamageKind.Melee:
                    MeleeActions++;

                    break;

                case CombatDamageKind.Ranged:
                    RangedActions++;

                    break;

                case CombatDamageKind.Magical:
                    MagicActions++;

                    break;
            }
        }

        public override bool ShouldMove(CombatDistance newPos, bool forceAction = false)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) >= 5)
                return base.ShouldMove(newPos, forceAction);

            if (IsFlying)
                return false;

            var total = MeleeActions + RangedActions + MagicActions;

            if (newPos == CombatDistance.Distant && total > 0 &&
                (MeleeActions >= total / 3 || (MeleeActions >= total / 4 && !CoC.Instance.Player.Weapon.IsRanged())))

                return true;

            return false;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (PsiAssaultCooldown > 0) PsiAssaultCooldown--;
            if (TerraFireCooldown > 0) TerraFireCooldown--;
            if (SwoopCooldown > 0) SwoopCooldown--;

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) >= 5)
                return false;

            if (!HasStatusEffect(StatusEffectIds.AkbalFireUsed) && HasFatigue(20, FatigueType.Magical))
            {
                AkbalUnsealedCombatHandlers.TerraFire(context, null!);

                return true;
            }

            if (!context.Player.HasStatusEffect(StatusEffectIds.TrueWhispered) &&
                HasFatigue(10, FatigueType.Magical) && PsiAssaultCooldown <= 0)
            {
                AkbalUnsealedCombatHandlers.PsiAssault(context, null!);

                return true;
            }

            if (Dodges >= 4 && HasFatigue(10, FatigueType.Magical))
            {
                AkbalUnsealedCombatHandlers.FireBreath(context, null!);

                return true;
            }

            return false;
        }

        public bool ShouldHeal
        {
            get
            {
                if (HasStatusEffect(StatusEffectIds.AkbalHealUsed))
                    return false;

                var hpRatio = MaxHealth <= 0 ? 1f : Health / MaxHealth;

                var threshold = CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) >= 5 ? 0.4f : 0.3f;

                return hpRatio < threshold || LustPercent > 70f;
            }
        }

        public bool ShouldFlyUp
        {
            get
            {
                if (IsFlying || !HasStatusEffect(StatusEffectIds.AkbalHealUsed))
                    return false;

                var hpRatio = MaxHealth <= 0 ? 1f : Health / MaxHealth;

                return hpRatio < (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) >= 5 ? 0.3f : 0.4f);
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.Vanilla.Areas.Forest.AkbalScene.AkbalQuestWinHook();

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.Vanilla.Areas.Forest.AkbalScene.AkbalQuestLoseHook();

            return false;
        }
    }
}