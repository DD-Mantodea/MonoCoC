using MonoCoC.Contents.Combat;
using System;

namespace MonoCoC.Contents.Creatures
{

    public class PlayerScope : IDisposable
    {
        public readonly Player original;

        public readonly Player current;

        public bool disposed;

        public PlayerScope(Player original, Player current)
        {
            this.original = original;

            this.current = current;
        }

        public static PlayerScope SwapTo(Player next)
        {
            var scope = new PlayerScope(CoC.Instance.Player, next);

            CoC.Instance.Player = next;

            if (CombatController.Instance is { } combat)
                combat.Context.Player = next;

            return scope;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            CoC.Instance.Player = original;

            if (CombatController.Instance is { } combat)
                combat.Context.Player = original;
        }
    }
}