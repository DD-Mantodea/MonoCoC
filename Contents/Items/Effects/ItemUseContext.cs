using MonoCoC.Contents.Creatures;
using System;

namespace MonoCoC.Contents.Items.Effects
{
    public readonly record struct ItemUseContext(
        Item Item,

        Player Player,

        Action<string> Write,

        Random Random
    );
}