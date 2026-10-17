using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Dungeons
{

    public class DungeonScene : Scene
    {
        public readonly DungeonContext _context;

        public DungeonScene(DungeonDefinition definition, int floorIndex, int x, int y)
        {
            _context = new DungeonContext
            {
                Definition = definition,

                FloorIndex = floorIndex,

                X = x,

                Y = y,

                SceneOwner = this
            };

            ActiveContext = _context;
        }

        public static DungeonContext? ActiveContext { get; set; }

        public DungeonContext DungeonContext => _context;

        public static void Enter(Identifier id)
        {
            var definition = DungeonManager.Get(id)

                ?? throw new InvalidOperationException("Common.Text.Dungeon".Localized(id));

            var floor = definition.Floors[0];

            var (x, y) = floor.Find(definition.Start);

            var scene = new DungeonScene(definition, 0, x, y);

            DungeonSession.Start(scene.DungeonContext);

            Go(scene);
        }

        public static void Resume()
        {
            if (ActiveContext != null)
                Go(ActiveContext.SceneOwner);
        }

        public override void Entry() => Render();

        public virtual void Render()
        {
            DungeonSession.Sync(_context);

            UI.Components.Views.GameView.Instance?.DungeonMap?.Hide();

            UI.Components.Views.GameView.Instance?.DungeonMiniMap?.Show(_context);

            ClearOutput();

            ClearButtons();

            var name = _context.Definition.NameKey.Length > 0

                ? _context.Definition.NameKey.Localized()

                : _context.Definition.Id.Path;

            OutputText("Common.Text.BBNN".Localized(name));

            var roomId = _context.CurrentRoom;

            var room = roomId.Length > 0 && _context.Floor.Rooms.TryGetValue(roomId, out var found) ? found : null;

            if (room != null && !string.IsNullOrEmpty(room.Handler.Path))
                DungeonRoomRegistry.Invoke(room.Handler, _context);
            else
                OutputText("Dungeons.Room.Unknown".Localized());

            AddDirectionButton(6, "North", DungeonDirection.North);

            AddDirectionButton(11, "South", DungeonDirection.South);

            AddDirectionButton(10, "West", DungeonDirection.West);

            AddDirectionButton(12, "East", DungeonDirection.East);

            if (room != null && room.StairsTo.Length > 0)
            {
                var parts = room.StairsTo.Split(':', 2);

                if (parts.Length == 2 && int.TryParse(parts[0], out var targetFloor))
                {
                    var targetRoom = parts[1];

                    AddButton(5, "Dungeons.Button.Stairs".Localized(),

                        () => _context.ChangeFloor(targetFloor, targetRoom));
                }
            }

            AddButton(13, "Common.Button.Map".Localized(), ShowMap);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveDungeon);
        }

        public virtual void LeaveDungeon()
        {
            DungeonSession.End();

            UI.Components.Views.GameView.Instance?.DungeonMap?.Hide();

            UI.Components.Views.GameView.Instance?.DungeonMiniMap?.Hide();

            Go<CampScene>();
        }

        public virtual void AddDirectionButton(int slot, string label, DungeonDirection direction)
        {
            if (_context.CanMove(direction))
                AddButton(slot, label, () => _context.Move(direction));
            else
                AddButtonDisabled(slot, label, "");
        }

        public virtual void ShowMap()
        {
            ClearOutput();

            ClearButtons();

            var name = _context.Definition.NameKey.Length > 0

                ? _context.Definition.NameKey.Localized()

                : _context.Definition.Id.Path;

            OutputText("Common.Text.BUUBN".Localized(name));

            OutputText("Common.Text.NIScrollToZoomHold".Localized());

            UI.Components.Views.GameView.Instance?.DungeonMap?.Show(_context);

            AddButton(0, "Common.Button.CloseMap".Localized(), CloseMap);
        }

        public virtual void CloseMap()
        {
            UI.Components.Views.GameView.Instance?.DungeonMap?.Hide();

            Render();
        }

        public virtual string BuildMapText()
        {
            var text = new System.Text.StringBuilder();

            var name = _context.Definition.NameKey.Length > 0

                ? _context.Definition.NameKey.Localized()

                : _context.Definition.Id.Path;

            text.Append("Common.Text.BUUBNN".Localized(name));

            var floor = _context.Floor;

            for (var y = 0; y < floor.Height; y++)
            {
                for (var x = 0; x < floor.Width; x++)
                {
                    var c = floor.CharAt(x, y);

                    if (c == '#')
                    {
                        text.Append("   ");

                        continue;
                    }

                    if (c == '.')
                    {
                        text.Append(" · ");

                        continue;
                    }

                    text.Append(x == _context.X && y == _context.Y ? "[@]" : "[ ]");
                }

                text.Append('\n');
            }

            text.Append("Common.Text.NBULegendUB".Localized());

            text.Append("Common.Text.YouAreHereN".Localized());

            text.Append("Common.Text.RoomN".Localized());

            text.Append("Common.Text.OpenSpaceN".Localized());

            var exits = new System.Collections.Generic.List<string>();

            foreach (var direction in new[]
                     {
                         DungeonDirection.North, DungeonDirection.South,

                         DungeonDirection.West, DungeonDirection.East
                     })
            {
                var (tx, ty) = direction switch
                {
                    DungeonDirection.North => (_context.X, _context.Y - 1),

                    DungeonDirection.South => (_context.X, _context.Y + 1),

                    DungeonDirection.West => (_context.X - 1, _context.Y),

                    _ => (_context.X + 1, _context.Y)
                };

                if (!floor.Walkable(tx, ty))
                    continue;

                var label = direction.ToString();

                var to = floor.RoomAt(tx, ty);

                var link = _context.CurrentRoom.Length > 0 && to.Length > 0

                    ? floor.LinkFor(_context.CurrentRoom, to)

                    : null;

                if (link != null && link.LockedBy.Path.Length > 0
                    && !DungeonConditionRegistry.Evaluate(link.LockedBy, _context))
                {
                    exits.Add("Common.Text.Locked".Localized(label));

                    continue;
                }

                exits.Add(label);
            }

            text.Append("\n<b>Exits:</b> " + (exits.Count > 0 ? string.Join(", ", exits) : "none") + "Common.Text.N".Localized());

            return text.ToString();
        }
    }
}