using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 原版 EncapsulationPod.hx:18：豆荚每回合只播放独白，逻辑在 BeforeCombatAction（无招式 handler）。
    [Register<EncapsulationPodEnemy>("coc:encapsulation_pod")]
    public class EncapsulationPodEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Tallness = 120;

            Body.Get<SkinPart>().Tone = "purple";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 0;

            WeaponName = "pod";

            ArmorName = "pod";
        }

        public static readonly MonoCoC.Registry.Identifier RoundId = new("coc", "encapsulation_pod_round");

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (IsCrippled)
            {
                context.Append("Scenes.EncapsulationPod.PerformCombatAction.HomeCozy".Localized());

                return true;
            }

            var round = HasStatusEffect(RoundId) ? (int)StatusEffectValue(RoundId, 0) : 0;

            var player = context.Player;

            if (round == 0)
            {
                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.WarmWetShiver".Localized());

                if (player.Balls > 0)
                {
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.BallsWord".Localized());

                    if (player.HasVagina())
                        context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.AndWord".Localized());
                }

                if (player.HasVagina())
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.VaginaWord".Localized());
                else if (player.Balls == 0)
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.TaintWord".Localized());

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.ClimbFurther".Localized());

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.BlankLine".Localized());
            }
            else if (round == 2)
            {
                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.TentaclesSquirm".Localized());

                if (player.Balls > 0)
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.WrapsTop".Localized());
                else
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.OneLead".Localized());

                if (player.CockTotal() > 0)
                {
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.ProdsCock".Localized());

                    context.Append(player.Cocks.Exists(c => c.SheathOn)

                        ? "Scenes.EncapsulationPod.EncapsulationPodAI.SheathWord".Localized()

                        : "Scenes.EncapsulationPod.EncapsulationPodAI.BaseWord".Localized());

                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.SqueezeRelax".Localized());
                }
                else if (player.HasVagina())
                {
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.ProdsGroin".Localized(player.VaginaDescript(0)));

                    if (player.GetClitLength() > 4)
                        context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.BrushesClit".Localized());
                }
                else
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.ProdsBeforeCurl".Localized());

                if (player.CockTotal() > 1)
                {
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.OtherLead".Localized());

                    context.Append(player.CockTotal() == 2

                        ? "Scenes.EncapsulationPod.EncapsulationPodAI.SameTreatment".Localized(player.CockDescript(1))

                        : "Scenes.EncapsulationPod.EncapsulationPodAI.SameTreatmentPlural".Localized());
                }

                if (player.HasVagina())
                {
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.ViolationSwift".Localized(player.VaginaDescript()));

                    context.Append(player.GetClitLength() > 3

                        ? "Scenes.EncapsulationPod.EncapsulationPodAI.DiscoversSize".Localized()

                        : "Scenes.EncapsulationPod.EncapsulationPodAI.EasyTime".Localized());
                }

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.LuckyStalk".Localized());

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.MassContinues".Localized());

                if (player.Pregnancy.Incubation > 0 && player.Pregnancy.Incubation < 120)
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.PregnantWord".Localized());

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.BellyCloser".Localized());

                if (player.BiggestBreastSize() < 1)
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.YourChest".Localized());
                else
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.UndersideWord".Localized(player.AllBreastsDescript()));

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.GodsAbove".Localized());

                if (player.Lust < 65)
                    player.ApplyStats(new DynStat(GameStats.Lust, 65 - player.Lust));

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.BlankLineTwo".Localized());
            }
            else if (round == 3)
            {
                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.WetWarmPressure".Localized(player.NippleDescript(0)));

                if (player.HasFuckableNipples())
                    context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.ArousalLeaks".Localized());

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.MoansEscape".Localized());

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.TentaclesBusy".Localized(player.AllBreastsDescript()));

                if (player.Lust < 85)
                    player.ApplyStats(new DynStat(GameStats.Lust, 85 - player.Lust));

                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.BlankLineThree".Localized());
            }
            else
            {
                context.Append("Scenes.EncapsulationPod.EncapsulationPodAI.EyesCloudOver".Localized());

                player.ApplyStats(new DynStat(GameStats.Lust, 1f));

                player.Lust = player.MaxLust;

                Scenes.SceneInstance<Dungeons.DeepCaveFungusScene>.Get().LoseToThisShitPartII();

                return true;
            }

            if (!HasStatusEffect(RoundId))
                CreateStatusEffect(RoundId, 2, 0, 0, 0);
            else
                ChangeStatusValue(RoundId, 0, 1);

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Dungeons.DeepCaveFungusScene>.Get().EncapsulationVictory();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Dungeons.DeepCaveFungusScene>.Get().LoseToThisShitPartII();

            return true;
        }
    }
}