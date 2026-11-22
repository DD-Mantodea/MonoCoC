using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.UI.Components.Views;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class CommandContext
    {
        public Player Player => CoC.Instance.Player;

        public GameFlags Flags => CoC.Instance.Flags;

        public GameView? View => GameView.Current;

        public Action<string> Output = _ => { };

        public Action Clear = () => { };

        public Action Close = () => { };

        public List<CommandContext> Sources { get; } = [];
    }
}
