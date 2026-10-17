using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/desert/Naga.hx（level 2 沙漠纳迦）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/Naga.hjson 与 NagaCombatHandlers.cs。
    [Register<NagaEnemy>("coc:naga")]
    public class NagaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 3, 2);

            CreateBreastRow(4f);

            Body.Get<LowerBodyPart>().Id = new Identifier("coc", "naga");

            Body.Get<SkinPart>().Tone = "mediterranean-toned";

            Body.SetButtRating("large");

            var hair = Body.Get<HairPart>();

            hair.Color = "brown";

            hair.Length = 16;

            WeaponName = "fist";

            ArmorName = "scales";
        }

        // 原版 Naga.hx:10 react(PlayerWaited)：被缠住时放松 → 淫欲 + 伤害
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            if (!player.HasStatusEffect(StatusEffectIds.NagaBind))
                return true;

            context.Clear();

            context.Append("Scenes.Naga.React.GripTightensRelax".Localized());

            player.TakeLustDamage(player.Sensitivity / 5 + 5, true);

            player.HealthChange(-(5 + context.Random.Next(5)));

            return false;
        }

        // 原版 Naga.hx:26 struggle()：挣脱盘绕，否则被勒紧
        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.NagaBind))
                return;

            context.Clear();

            if (context.Random.Next(3) == 0 || context.Random.Next(80) < player.Strength / 1.5f)
            {
                context.Append("Scenes.Naga.Struggle.WriggleFreeCoils".Localized());

                player.RemoveStatusEffect(StatusEffectIds.NagaBind);
            }
            else
            {
                context.Append("Scenes.Naga.Struggle.GripTightensStruggle".Localized());

                player.TakeLustDamage(player.Sensitivity / 10 + 2, true);

                player.HealthChange(-(7 + context.Random.Next(5)));
            }
        }

        // 原版 Naga.hx:92 defeated() -> nagaRapeChoice()
        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.NagaScene>.Get().NagaRapeChoice();

            return true;
        }

        // 原版 Naga.hx:96 won()
        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Scenes.Naga.Won.EyesWideLeaves".Localized());

                CoC.Instance.Player.Orgasm("Generic");

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.NagaScene>.Get().NagaFUCKSJOOOOOO();

            return true;
        }
    }
}