using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/HarpyMobCombatHandlers.cs
    [Register<HarpyMobEnemy>("coc:harpy_mob")]
    public class HarpyMobEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 0);

            Tallness = Random.Shared.Next(8) + 70;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "claw";

            ArmorName = "armor";
        }

        // 原版 HarpyMob.hx:16 react(PlayerWaited)：被擒住时放松 → 整群继续殴打
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            var player = context.Player;

            if (!player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.HarpyBind))
                return true;

            context.Clear();

            context.Append("Combat.HarpyMob.Wait.Hammered".Localized());

            player.HealthChange(-(80 + context.Random.Next(40)));

            return false;
        }

        // 原版 HarpyMob.hx:20 被擒时玩家的行动即挣扎
        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.HarpyBind))
                return;

            Combat.Handlers.HarpyMobCombatHandlers.Struggle(context);
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(Flags.GameFlagIds.HelHarpiesDefeated, 1);

            CombatController.Instance?.Context.Append("Combat.Hel.HarpyMobVictory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Combat.Hel.HarpyMobDefeat".Localized());

            return false;
        }
    }
}