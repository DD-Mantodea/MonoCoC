using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<IzumiEnemy>("coc:izumi")]
    public class IzumiEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 3, 2);

            Tallness = 9 * 12 + 0;

            Body.Get<SkinPart>().Tone = "creamy-white";

            var hair = Body.Get<HairPart>();

            hair.Color = "golden";

            hair.Length = 25;

            WeaponName = "fist";

            ArmorName = "silken kimono";
        }

        public override void Struggle(Combat.CombatContext context) =>

            Combat.Handlers.IzumiCombatHandlers.Struggle(context);

        public override bool React(Combat.CombatContext context, Combat.CombatReactionContext reaction)
        {
            if (reaction == Combat.CombatReactionContext.PlayerWaited)
                return Combat.Handlers.IzumiCombatHandlers.OnPlayerWaited(context);

            return true;
        }

        public override void CombatRoundUpdate(Combat.CombatContext context)
        {
            base.CombatRoundUpdate(context);

            var player = context.Player;

            if (player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Chokeslam))
                player.ChangeStatusValue(Datas.Vanilla.StatusEffectIds.Chokeslam, 0, -1);

            // 原版 Izumi.hx:84-86：乳压窒息期间 Izumi 不行动，由玩家挣脱
            if (player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Titsmother))
                TookAction = true;
        }

        public override Identifier? ForcedAbility(Combat.CombatContext context)
        {
            var player = context.Player;

            // 原版 Izumi.hx:89-93：血量 ≤25% 时发动一次乳压窒息
            if (!HasStatusEffect(Datas.Vanilla.StatusEffectIds.UsedTitsmother)
                && MaxHealth > 0

                && Health / MaxHealth <= 0.25f

                && !player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Titsmother))
            {
                CreateStatusEffect(Datas.Vanilla.StatusEffectIds.UsedTitsmother, 0, 0, 0, 0);

                return new Identifier("coc", "izumi_tit_smother");
            }

            if (!player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Chokeslam))
                return null;

            if (player.StatusEffectValue(Datas.Vanilla.StatusEffectIds.Chokeslam, 0) > 0)
                return null;

            player.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Chokeslam);

            return new Identifier("coc", "izumi_ground_pound");
        }

        // 原版 Izumi.hx:105 cleanup()：清除战斗遗留状态
        private static void Cleanup()
        {
            var player = CoC.Instance.Player;

            player.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Chokeslam);

            if (player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Titsmother))
                player.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Titsmother);
        }

        public override bool Defeated(bool hpVictory)
        {
            Cleanup();

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.IzumiScene>.Get().TouchThatFluffyHorn();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var flags = CoC.Instance.Flags;

            flags.Set(Flags.GameFlagIds.IzumiTimesLostFight,

                flags.Get(Flags.GameFlagIds.IzumiTimesLostFight) + 1);

            if (CoC.Instance.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Titsmother))
            {
                Cleanup();

                Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.IzumiScene>.Get().DeathBySnuSnuIMeanGiantOniTits();

                return true;
            }

            Cleanup();

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.IzumiScene>.Get().FuckedUpByAFuckhugeOni();

            return true;
        }
    }
}