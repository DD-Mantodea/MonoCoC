using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla;

namespace MonoCoC.Contents.Combat
{
    public class CombatScene(CombatContext context) : Scene
    {
        public CombatContext CombatContext { get; } = context;

        public CombatController Controller = null!;

        public static Enemy? LastEnemy;

        public static CombatOutcome? LastOutcome;

        public static Action? ReturnToCampAction;

        public static void Start(Enemy enemy, Action returnToCamp, bool canFlee = true, bool enemyFirst = false, string introDescription = "")
        {
            var context = new CombatContext
            {
                Player = Player,

                Enemy = enemy,

                Random = Random.Shared,

                CanFlee = canFlee,

                IntroDescription = introDescription
            };

            ReturnToCampAction = returnToCamp;

            Go(new CombatScene(context));
        }

        public static void StartMultiple(IReadOnlyList<Enemy> enemies, Action returnToCamp, bool canFlee = true, string introDescription = "")
        {
            if (enemies.Count == 0)
            {
                returnToCamp();

                return;
            }

            Start(enemies[0], returnToCamp, canFlee, false, introDescription);

            var context = ((CombatScene)Current!).CombatContext;

            for (var index = 1; index < enemies.Count; index++)
                context.AddEnemy(enemies[index]);
        }

        public override void Entry()
        {
            CombatContext.ReturnView ??= MonoCoC.UI.Components.Views.GameView.Current;

            MonoCoC.UI.Components.Views.GameView.SwitchTo<MonoCoC.UI.Components.Views.CombatView>(CombatContext);

            RefreshStats(CombatContext);

            Controller = new CombatController
            {
                Context = CombatContext
            };

            Controller.Start();

            if (CombatContext.EnemyFirst)
                Controller.EnemyTurn();
        }

        public static void RefreshStats(CombatContext context)
        {
            if (MonoCoC.UI.Components.Views.GameView.Current is not MonoCoC.UI.Components.Views.CombatView view)
                return;

            if (context.Enemies.Count <= 1)
            {
                view.ShowCombatStats(context.Enemy);

                return;
            }

            view.ShowCombatStats(context.Enemies, context.CurrTarget, index =>
            {
                context.SetTarget(index);

                CombatMenu();
            });
        }

        public static void CombatMenu()
        {
            if (Current is CombatScene scene)
                scene.Controller.ShowMenu();
        }

        public static void CleanupAfterCombat()
        {
            CombatController.InCombat = false;

            CombatController.Instance = null;

            var returnView = (Current as CombatScene)?.CombatContext.ReturnView;

            if (returnView is not null)
                MonoCoC.UI.Components.Views.GameView.SwitchTo(returnView);
        }
    }
}