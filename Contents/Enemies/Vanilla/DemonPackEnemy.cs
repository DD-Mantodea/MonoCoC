using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 原版 DemonPack.hx:10 的两招已迁移为数据驱动的 AI（coc:pack_attack / coc:pack_lust_attack，见 PackCombatHandlers）。
    [Register<DemonPackEnemy>("coc:demon_pack")]
    public class DemonPackEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(18, 2);

            CreateCock(18, 2, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 1;

            CreateVagina(false, 3, 3);

            Tallness = Random.Shared.Next(8) + 70;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "claws";

            ArmorName = "demonic skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.Scene.ClearOutput();

            if (hpVictory)
            {
                Scenes.Scene.OutputText("Scenes.DemonPack.Defeated.LastDemonFalls".Localized());

                CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Lust, 1));

                Combat.CombatScene.CleanupAfterCombat();

                return true;
            }

            Scenes.Scene.OutputText("Scenes.DemonPack.Defeated.DemonsMasturbate".Localized());

            Scenes.Scene.OutputText("Scenes.DemonPack.Defeated.RapeThemPrompt".Localized());

            Scenes.Scene.DoYesNo(RapeDemons, () => Combat.CombatScene.CleanupAfterCombat());

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            if (player.IsGenderless())
            {
                Scenes.Scene.OutputText(hpVictory

                    ? "Scenes.DemonPack.Won.GenderlessBeatenDown".Localized()

                    : "Scenes.DemonPack.Won.OffersHerselfMocked".Localized());

                Combat.CombatScene.CleanupAfterCombat();

                return true;
            }

            if (hpVictory)
            {
                Scenes.Scene.OutputText("Scenes.DemonPack.Won.BeatDownOasisSand".Localized());

                Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.Oasis>.Get().OasisSexing();

                return true;
            }

            Scenes.Scene.OutputText("Scenes.DemonPack.Won.MindsLustFails".Localized());

            if (HasStatusEffect(StatusEffectIds.Infested))
                Scenes.Scene.OutputText("Scenes.DemonPack.Won.DemonsUnconcerned".Localized());

            if (player.Cocks.Count > 0)
            {
                Scenes.Scene.OutputText(player.Cocks.Count > 1

                    ? "Scenes.DemonPack.Won.EachOfYourLead".Localized()

                    : "Scenes.DemonPack.Won.YourThrobLead".Localized());

                Scenes.Scene.OutputText("Scenes.DemonPack.Won.CocksThrob".Localized());

                if (player.HasVagina())
                    Scenes.Scene.OutputText("Scenes.DemonPack.Won.AndYourJoiner".Localized());
            }

            if (player.Vaginas.Count > 0)
            {
                if (!player.HasCock())
                    Scenes.Scene.OutputText("Scenes.DemonPack.Won.YourBurnsLead".Localized());

                Scenes.Scene.OutputText("Scenes.DemonPack.Won.VaginaBurns".Localized(player.VaginaDescript(0)));
            }

            Scenes.Scene.OutputText("Scenes.DemonPack.Won.GrabsBreastDesperate".Localized());

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.Oasis>.Get().OasisSexing();

            return true;
        }

        public void RapeDemons()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.OutputText("Scenes.DemonPack.RapeDemons.StepsIntoThrong".Localized());

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.Oasis>.Get().OasisSexing();
        }
    }
}