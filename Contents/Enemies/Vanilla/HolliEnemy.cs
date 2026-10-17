using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<HolliEnemy>("coc:holli")]
    public class HolliEnemy : Enemy
    {

        public bool IsBurning;

        public int JojoPyre;

        public static Identifier ConstrictStatus => StatusEffectIds.HolliConstrict;

        public bool _teasedWhileBurning;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12, 2, BodyPartTypes.Human);

            Balls = 0;

            BallSize = 0;

            CreateVagina(false, 2, 3);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 20);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = Random.Shared.Next(12) + 55;

            Body.Hips = 10;

            Body.Butt = 8;

            Body.Get<SkinPart>().Tone = "black";

            var hair = Body.Get<HairPart>();

            hair.Color = "sandy-blonde";

            hair.Length = 15;

            Strength = 150;

            Toughness = 80;

            Speed = 80;

            Intelligence = 85;

            Libido = 75;

            Sensitivity = 40;

            Corruption = 80;

            WeaponName = "branches";

            WeaponVerb = "branchy thwack";

            ArmorName = "bark";

            ArmorDefense = 40;

            Lust = 20;

            LustVulnerability = 0.2f;

            Level = 20;

            Gems = 0;

        }

        public override void CombatRoundUpdate(CombatContext context)
        {
            base.CombatRoundUpdate(context);

            HolliBonusHealing(context);
        }

        public void HolliBonusHealing(CombatContext context)
        {
            if (!IsBurning)
            {
                if (HealthRatio() < 1 && Health > 1)
                {
                    context.Append("Combat.Holli.HolliBonusHealing.WhatWoundsYou".Localized());

                    HealthChange(25);
                }

                if (Lust > 20 && LustRatio() <= 0.99f)
                {
                    context.Append("Combat.Holli.HolliBonusHealing.ASingleRent".Localized());

                    Lust -= 10;
                }
            }
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited
                || !context.Player.HasStatusEffect(ConstrictStatus))

                return true;

            WaitForConstrict(context, true);

            TookAction = true;

            return false;
        }

        public override void RunFail(CombatContext context)
        {

            context.Clear();

            context.Append("Combat.Holli.RunFail.You".Localized());

            if (context.Player.CanFly())
                context.Append("Combat.Holli.RunFail.BeatYourWings".Localized());

            context.Append("Combat.Holli.RunFail.TryToEscape".Localized(ShortName));

            if (!context.Player.HasPerk(PerkIds.Juggernaut) && ArmorPerk != "Heavy")
                context.Player.TakeDamage(15);

            context.Append("Combat.Holli.RunFail.Text".Localized());

            context.Player.CreateStatusEffect(ConstrictStatus, 0, 0, 0, 0);
        }

        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            player.ChangeStatusValue(ConstrictStatus, 1, 9);

            context.Clear();

            if (player.Body.Get<TonguePart>().Id != BodyPartTypes.Human)
            {
                context.Append("Combat.Holli.StruggleOutOfHolli.YouCanT".Localized());

                player.RemoveStatusEffect(ConstrictStatus);
            }
            else if (player.Strength / 10f + context.Random.Next(20) + 1
                     + player.StatusEffectValue(ConstrictStatus, 0) > 30)
            {
                context.Append("Combat.Holli.StruggleOutOfHolli.YouManageTo".Localized());

                player.RemoveStatusEffect(ConstrictStatus);
            }
            else
            {
                context.Append("Combat.Holli.StruggleOutOfHolli.YouTryTo".Localized());

                WaitForConstrict(context, false);

                return;
            }

            TookAction = true;
        }

        public void WaitForConstrict(CombatContext context, bool newScreen)
        {
            if (newScreen)
                context.Clear();

            context.Append("Combat.Holli.WaitForHolliConstrict.TheOminousRoseate".Localized());

            Lust -= 20;

            if (Lust < 20)
                Lust = 20;

            context.Player.TakeLustDamage(15 + context.Player.Sensitivity / 5f, true);
        }

        public override void ApplyTease(float lustDelta)
        {
            if (IsBurning)
            {
                _teasedWhileBurning = true;

                return;
            }

            base.ApplyTease(lustDelta);
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            if (_teasedWhileBurning)
            {
                _teasedWhileBurning = false;

                context.WriteAction("Combat.Holli.Teased.HolliDoesnT".Localized());

                return;
            }

            base.OnTeased(context, lustDelta);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<HolliScene>.Get().DefeatHolli();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<HolliScene>.Get().EnjoyYourBadEndBIYAAAATCH();

            return true;
        }
    }
}