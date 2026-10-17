using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ClaraEnemy>("coc:clara")]
    public class ClaraEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 2);

            CreateBreastRow(BreastCups.F);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Tallness = 76;

            Body.Hips = 10;

            Body.Butt = 8;

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Hoofed;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Color = "brown";

            hair.Length = 13;

            Strength = 37;

            Toughness = 55;

            Speed = 35;

            Intelligence = 60;

            Libido = 25;

            Sensitivity = 45;

            Corruption = 40;

            WeaponName = "mace";

            WeaponVerb = "smack";

            WeaponAttack = 10;

            ArmorName = "tough hide";

            ArmorDefense = 5;

            Level = 5;

            Gems = Random.Shared.Next(5) + 25;

            Body.Get<TailPart>().Id = BodyPartTypes.Cow;
        }

        public override void CombatRoundUpdate(CombatContext context)
        {
            base.CombatRoundUpdate(context);

            var player = context.Player;

            if (player.HasStatusEffect(StatusEffectIds.ClaraFoughtInCamp)
                && player.StatusEffectValue(StatusEffectIds.ClaraCombatRounds, 0) >= 10)

                Health = 0;

            if (!player.HasStatusEffect(StatusEffectIds.ClaraCombatRounds))
                player.CreateStatusEffect(StatusEffectIds.ClaraCombatRounds, 1, 0, 0, 0);
            else
                player.ChangeStatusValue(StatusEffectIds.ClaraCombatRounds, 0, 1);

            if (Health > 0 && Lust < MaxLust && !player.HasStatusEffect(StatusEffectIds.ClaraFoughtInCamp))
                ClaraCombatHandlers.BonusBaseLustDamage(context);
        }

        public override bool BeforeCombatAction(CombatContext context)
        {

            if (Health <= 0)
                return true;

            if (Health < 50 && context.Random.Next(2) == 0)
            {
                ClaraCombatHandlers.LacticAcid(context);

                return true;
            }

            if (context.Player.HasStatusEffect(StatusEffectIds.Blind))
            {
                ClaraCombatHandlers.GropesBlind(context);

                return true;
            }

            return false;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<MarblePurification>.Get().DefeatClaraCuntInAFight();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<MarblePurification>.Get().LoseToClara();

            return true;
        }
    }
}