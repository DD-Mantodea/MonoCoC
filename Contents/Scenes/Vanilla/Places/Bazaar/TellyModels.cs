using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    public class OneOf<TLeft, TRight>
    {
        public bool IsLeft { get; }

        public TLeft Left { get; }

        public TRight Right { get; }

        public OneOf(bool isLeft, TLeft left, TRight right)
        {
            IsLeft = isLeft;

            Left = left;

            Right = right;
        }

        public static OneOf<TLeft, TRight> FromLeft(TLeft value) => new(true, value, default!);

        public static OneOf<TLeft, TRight> FromRight(TRight value) => new(false, default!, value);
    }

    public class TellyAnalysis
    {
        public OneOf<Identifier, Action> TellyProcessing { get; init; } = default!;

        public string TellyNym { get; init; } = "";

        public int TellyPayment { get; init; }

        public string TellyPrompt { get; init; } = "";
    }
}