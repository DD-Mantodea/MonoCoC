using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{

    [Register<NaughtyNunsHabitBehavior>("coc:naughty_nuns_habit")]
    public class NaughtyNunsHabitBehavior : ItemBehavior
    {
        public static void CockWorship()
        {
            var enemy = CombatController.Instance?.Context.Enemy;

            var player = CoC.Instance.Player;

            if (enemy == null)
                return;

            Scene.ClearOutput();

            Scene.ClearButtons();

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Three".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Four".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Five".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Six".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Seven".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Eight".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Worship.Nine".Localized());

            player.SlimeFeed();

            player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            if (player.HasVagina() && player.Body.Get<VaginaPart>().Virgin)
                player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            if (enemy is MinotaurEnemy || enemy is MinotaurMobEnemy)
                player.MinoCumAddiction(3);

            if (enemy.ShortName == "Ceraph")
                CoC.Instance.Flags.Set(GameFlagIds.CeraphBeatenAndRapedCounter,

                    CoC.Instance.Flags.Get(GameFlagIds.CeraphBeatenAndRapedCounter) + 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                Scene.Go<CampScene>();
        }

        // 原版 NaughtyNunsHabit.hx:96 naughtyNunMasturbationMenu()
        public static void MasturbationMenu()
        {
            var player = CoC.Instance.Player;

            Scene.ClearButtons();

            void Next(string labelKey, Action action, string hintKey)
            {
                var button = Scene.AddNextButton(labelKey.Localized(), action);

                button?.Hint = hintKey.Localized();
            }

            if (!player.IsTaur())
            {
                if (player.HasCock())
                {
                    Next("Scenes.NaughtyNunsHabit.Menu.RosaryCock", RosaryCockMasturbation, "Scenes.NaughtyNunsHabit.Menu.RosaryCockHint");

                    Next("Scenes.NaughtyNunsHabit.Menu.Baptism", SelfBaptism, "Scenes.NaughtyNunsHabit.Menu.BaptismHint");
                }

                if (player.HasVagina())
                    Next("Scenes.NaughtyNunsHabit.Menu.CrossVagina", CrossVaginalMasturbation, "Scenes.NaughtyNunsHabit.Menu.CrossVaginaHint");

                Next("Scenes.NaughtyNunsHabit.Menu.RosaryAss", RosaryAssMasturbation, "Scenes.NaughtyNunsHabit.Menu.RosaryAssHint");

                Next("Scenes.NaughtyNunsHabit.Menu.CrossAss", CrossAnalMasturbation, "Scenes.NaughtyNunsHabit.Menu.CrossAssHint");

                if (player.Corruption > 40)
                    Next("Scenes.NaughtyNunsHabit.Menu.Punishment", Punishment, "Scenes.NaughtyNunsHabit.Menu.PunishmentHint");
            }

            Scene.AddButton(14, "Common.Button.Back".Localized(), () => SceneInstance<MasturbationScene>.Get().MasturbateMenu());
        }

        // 原版 :116 naughtyNunRosaryCockMasturbation()
        public static void RosaryCockMasturbation()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Three".Localized(
                player.CockTotal() > 1 ? "one of your cocks" : "your [cock]"));

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Four".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Five".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Six".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Seven".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Eight".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryCock.Nine".Localized());

            player.Orgasm("Dick");

            SceneInstance<MasturbationScene>.Get().UpdateMasturbation();

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.ReturnToCamp(1));
        }

        // 原版 :147 naughtyNunRosaryAssMasturbation()
        public static void RosaryAssMasturbation()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Three".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Four".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Five".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Six".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Seven".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.RosaryAss.Eight".Localized());

            player.Orgasm("Anal");

            player.ButtChange(12, true);

            SceneInstance<MasturbationScene>.Get().UpdateMasturbation();

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.ReturnToCamp(1));
        }

        // 原版 :167 naughtyNunCrossVaginalMasturbation()
        public static void CrossVaginalMasturbation()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Three".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Four".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Five".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Six".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Seven".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Eight".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossVagina.Nine".Localized());

            player.CuntChange(12, true);

            player.Orgasm("Vaginal");

            SceneInstance<MasturbationScene>.Get().UpdateMasturbation();

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.ReturnToCamp(1));
        }

        // 原版 :187 naughtyNunCrossAnalMasturbation()
        public static void CrossAnalMasturbation()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.Three".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.Four".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.Five".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.Six".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.CrossAnal.Seven".Localized());

            player.ButtChange(12, true);

            player.Orgasm("Anal");

            SceneInstance<MasturbationScene>.Get().UpdateMasturbation();

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.ReturnToCamp(1));
        }

        // 原版 :205 naughtyNunSelfBaptism()
        public static void SelfBaptism()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.NaughtyNunsHabit.Baptism.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Baptism.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Baptism.Three".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Baptism.Four".Localized());

            // 原版 :218-219 lowMedHighCum 拼出的两句可变文本
            var lowAmount = "a few strings of jizz shooting from your cock";

            var medAmount = "rope after rope of thick jizz shooting from your cock";

            var highAmount = "an absurd fountain of jizz shooting from your cock";

            var lowCover = "your face is covered in your own cum";

            var medCover = "you're painted white with your own cum";

            var highCover = "absolutely covered in the thick mass of your own cum";

            Scene.OutputText("Scenes.NaughtyNunsHabit.Baptism.Five".Localized(
                player.LowMedHighCum(lowAmount, medAmount, highAmount),

                player.LowMedHighCum(lowCover, medCover, highCover)));

            Scene.OutputText("Scenes.NaughtyNunsHabit.Baptism.Six".Localized());

            player.Orgasm("Dick");

            SceneInstance<MasturbationScene>.Get().UpdateMasturbation();

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.ReturnToCamp(1));
        }

        // 原版 :226 naughtyNunPunishment()
        public static void Punishment()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.One".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Two".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Three".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Four".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Five".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Six".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Seven".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Eight".Localized());

            Scene.OutputText("Scenes.NaughtyNunsHabit.Punishment.Nine".Localized());

            player.Orgasm("Generic");

            SceneInstance<MasturbationScene>.Get().UpdateMasturbation();

            // 原版 :233 Utils.rand(2) == 0 → 堕落 -1
            if (Random.Shared.Next(2) == 0)
                player.Corruption = Math.Max(0, player.Corruption - 1);

            Scene.AddNextButton("Common.Button.Next".Localized(), () => Scene.ReturnToCamp(1));
        }
    }
}