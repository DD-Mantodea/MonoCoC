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

        public static void Start(Enemy enemy, Action returnToCamp, bool canFlee = true, bool enemyFirst = false, string introDescription = "")
        {
            var context = new CombatContext
            {
                Player = Player,

                Enemy = enemy,

                Random = Random.Shared,

                Write = OutputText,

                AddButton = (slot, text, action) => AddButton(slot, text, action),

                AddButtonDisabled = (slot, text, hint) => AddButtonDisabled(slot, text, hint),

                Clear = () => ClearOutput(),

                ClearButtons = () => ClearButtons(),

                ReturnToCamp = returnToCamp,

                CanFlee = canFlee,

                IntroDescription = introDescription
            };

            context.WriteAction = text => context.Log.Add(text);

            context.Append = text =>
            {
                if (context.Log.Count > 0)
                    context.Log[^1] += text;
                else
                    context.Log.Add(text);
            };

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

            if (context.Enemies.Count <= 1)
            {
                View.ShowCombatStats(context.Enemy);

                return;
            }

            View.ShowCombatStats(context.Enemies, context.CurrTarget, index =>
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

            UI.Components.Views.GameView.Instance?.HideCombatStats();

            Go<CampScene>();
        }
    }
}