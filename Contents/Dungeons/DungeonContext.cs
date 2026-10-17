using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Dungeons
{

    public class DungeonContext
    {
        public DungeonDefinition Definition = null!;

        public DungeonScene SceneOwner = null!;

        public int FloorIndex;

        public int X;

        public int Y;

        public DungeonFloorDefinition Floor => Definition.Floors[FloorIndex];

        public string CurrentRoom => Floor.RoomAt(X, Y);

        public (int X, int Y) Target(DungeonDirection direction) => direction switch
        {
            DungeonDirection.North => (X, Y - 1),

            DungeonDirection.South => (X, Y + 1),

            DungeonDirection.West => (X - 1, Y),

            _ => (X + 1, Y)
        };

        public bool CanMove(DungeonDirection direction)
        {
            var (tx, ty) = Target(direction);

            if (!Floor.Walkable(tx, ty))
                return false;

            var from = CurrentRoom;

            var to = Floor.RoomAt(tx, ty);

            if (from.Length == 0 || to.Length == 0)
                return true;

            var link = Floor.LinkFor(from, to);

            if (link == null || link.LockedBy.Path.Length == 0)
                return true;

            return DungeonConditionRegistry.Evaluate(link.LockedBy, this);
        }

        public void Move(DungeonDirection direction)
        {
            if (!CanMove(direction))
                return;

            (X, Y) = Target(direction);

            Scene.View.Time.PassMinutes(5);

            DungeonSession.Sync(this);

            SceneOwner.Render();
        }

        public void StartCombat(Enemy enemy)
        {
            DungeonSession.LastCombatLost = false;

            CombatScene.Start(enemy, () =>
            {
                if (DungeonSession.LastCombatLost)
                    HandleDefeat();
                else
                    Scene.Go(SceneOwner);
            });
        }

        public void HandleDefeat()
        {
            if (DungeonDefeatRegistry.Has(Definition.OnDefeat))
            {
                DungeonDefeatRegistry.Invoke(Definition.OnDefeat, this);

                return;
            }

            Write("Dungeons.Defeat.Fallback".Localized());

            LeaveDungeon();
        }

        public void LeaveDungeon()
        {
            DungeonSession.End();

            UI.Components.Views.GameView.Instance?.DungeonMap?.Hide();

            UI.Components.Views.GameView.Instance?.DungeonMiniMap?.Hide();

            Scene.Go<CampScene>();
        }

        public void EnterRoom(string roomId)
        {
            var (x, y) = Floor.Find(roomId);

            if (x < 0)
                return;

            X = x;

            Y = y;

            DungeonSession.Sync(this);

            SceneOwner.Render();
        }

        public void Refresh() => SceneOwner.Render();

        public void ChangeFloor(int floorIndex, string roomId)
        {
            if (floorIndex < 0 || floorIndex >= Definition.Floors.Count)
                return;

            FloorIndex = floorIndex;

            var (x, y) = Floor.Find(roomId);

            if (x < 0)
                return;

            X = x;

            Y = y;

            DungeonSession.Sync(this);

            SceneOwner.Render();
        }

        public void Write(string text) => Scene.OutputText(text);

        public void Leave() => Scene.Go<CampScene>();
    }
}