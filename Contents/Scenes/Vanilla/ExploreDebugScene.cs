using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<ExploreDebugScene>("coc:explore_debug")]
    public class ExploreDebugScene : Scene
    {

        public override void Entry()
        {
            DoExploreDebug();
        }

        public void DoExploreDebug()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Exploredebug.DoExploreDebug.MonstersMenuLabel".Localized());

            AddButton(0, "Common.Button.Monsters".Localized(), () => ExploreDebugMonsters());
        }

        public void ExploreDebugMonsters(int monsterIdx = 0)
        {
            ClearOutput();

            ClearButtons();

            if (monsterIdx == 0)
                OutputText("Scenes.Exploredebug.ExploreDebugMonsters.FightAllWarning".Localized());

            if (monsterIdx >= AllMonsters().Count)
                OutputText("Scenes.Exploredebug.ExploreDebugMonsters.AllMonstersFought".Localized());
            else
            {
                var m = AllMonsters()[monsterIdx]();

                m.OnDefeated = (hpVictory) =>
                {
                    CombatController.InCombat = false;

                    CombatController.Instance?.ClearStatuses();

                    StatScreenRefresh();

                    ExploreDebugMonsters(monsterIdx + 1);
                };

                m.OnWon = (hpVictory) =>
                {
                    CombatController.InCombat = false;

                    CombatController.Instance?.ClearStatuses();

                    StatScreenRefresh();

                    ExploreDebugMonsters(monsterIdx + 1);
                };

                OutputText("Scenes.Exploredebug.ExploreDebugMonsters.NextMonsterAnnounce".Localized(
                    m.Article,

                    m.ShortName
                ));

                AddButton(0, "Common.Button.Fight".Localized(), () => DoFight(m));

                AddButton(1, "Common.Button.Skip".Localized(), () => ExploreDebugMonsters(monsterIdx + 1));

                AddButton(2, "Common.Button.Heal".Localized(), DoHeal);
            }

            if (monsterIdx > 1)
                AddButton(6, "Common.Button.GoBack".Localized(), () => ExploreDebugMonsters(monsterIdx - 1));

            AddButton(9, "Common.Button.Enough".Localized(), () => Go<CampScene>());
        }

        private static List<Func<Enemy>>? _allMonsters;

        private static List<Func<Enemy>> AllMonsters()
        {
            if (_allMonsters != null)
                return _allMonsters;

            _allMonsters = [];

            foreach (var id in EnemyRegistry.Definitions.Ids)
            {
                var monsterId = id;

                _allMonsters.Add(() => EnemyRegistry.Create(monsterId));
            }

            return _allMonsters;
        }

        public void DoFight(Enemy m)
        {

            OutputText("Scenes.Exploredebug.DoFight.CombatStartingLabel".Localized());

            CombatScene.Start(m, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DoHeal()
        {

            Player.Health = Player.MaxHealth;

            Player.Lust = 0;

        }

    }
}