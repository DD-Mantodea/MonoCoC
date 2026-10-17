using System;
using System.Collections.Generic;
using System.Globalization;

namespace MonoCoC.Scripting
{
    public static class ExpressionEvaluator
    {
        private enum TokenType
        {
            Number,

            Text,

            Name,

            Symbol,

            End
        }

        private readonly struct Token(TokenType type, string text, double number)
        {
            public TokenType Type { get; } = type;

            public string Text { get; } = text;

            public double Number { get; } = number;
        }

        public static bool TryEvaluate(string source, ScriptContext context, out ScriptValue result)
        {
            result = ScriptValue.Null;

            try
            {
                var tokens = Tokenize(source);

                var index = 0;

                result = ParseConditional(tokens, ref index, context);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();

            var index = 0;

            while (index < source.Length)
            {
                var character = source[index];

                if (char.IsWhiteSpace(character))
                {
                    index++;

                    continue;
                }

                if (char.IsDigit(character) || character == '.')
                {
                    var start = index;

                    while (index < source.Length && (char.IsDigit(source[index]) || source[index] == '.'))

                        index++;

                    tokens.Add(new Token(TokenType.Number, source[start..index], double.Parse(source[start..index], CultureInfo.InvariantCulture)));

                    continue;
                }

                if (character == '\'' || character == '"')
                {
                    index++;

                    var builder = new System.Text.StringBuilder();

                    while (index < source.Length && source[index] != character)
                    {
                        if (source[index] == '\\' && index + 1 < source.Length)
                        {
                            builder.Append(source[index + 1]);

                            index += 2;

                            continue;
                        }

                        builder.Append(source[index]);

                        index++;
                    }

                    index++;

                    tokens.Add(new Token(TokenType.Text, builder.ToString(), 0));

                    continue;
                }

                if (char.IsLetter(character) || character == '_')
                {
                    var start = index;

                    while (index < source.Length && (char.IsLetterOrDigit(source[index]) || source[index] == '_' || source[index] == '.'))

                        index++;

                    tokens.Add(new Token(TokenType.Name, source[start..index], 0));

                    continue;
                }

                var matched = false;

                foreach (var symbol in Symbols)
                {
                    if (index + symbol.Length > source.Length || string.CompareOrdinal(source, index, symbol, 0, symbol.Length) != 0)
                        continue;

                    tokens.Add(new Token(TokenType.Symbol, symbol, 0));

                    index += symbol.Length;

                    matched = true;

                    break;
                }

                if (!matched)
                    throw new FormatException($"Unexpected character '{character}'");
            }

            tokens.Add(new Token(TokenType.End, "", 0));

            return tokens;
        }

        private static readonly string[] Symbols = ["===", "!==", "||", "&&", "==", "!=", ">=", "<=", ">", "<", "+", "-", "*", "/", "%", "!", "?", ":", "(", ")"];

        private static ScriptValue ParseConditional(List<Token> tokens, ref int index, ScriptContext context)
        {
            var condition = ParseBinary(tokens, ref index, context, 0);

            if (tokens[index].Type != TokenType.Symbol || tokens[index].Text != "?")
                return condition;

            index++;

            var whenTrue = ParseConditional(tokens, ref index, context);

            if (tokens[index].Type != TokenType.Symbol || tokens[index].Text != ":")
                throw new FormatException("Missing ':' in conditional");

            index++;

            var whenFalse = ParseConditional(tokens, ref index, context);

            return condition.ToBool() ? whenTrue : whenFalse;
        }

        private static ScriptValue ParseBinary(List<Token> tokens, ref int index, ScriptContext context, int minPrecedence)
        {
            var left = ParseUnary(tokens, ref index, context);

            while (true)
            {
                var token = tokens[index];

                if (token.Type is not (TokenType.Symbol or TokenType.Name))
                    return left;

                var precedence = Precedence(token.Text.ToLowerInvariant());

                if (precedence < 0 || precedence < minPrecedence)
                    return left;

                index++;

                var right = ParseBinary(tokens, ref index, context, precedence + 1);

                left = Apply(token.Text.ToLowerInvariant(), left, right);
            }
        }

        private static ScriptValue ParseUnary(List<Token> tokens, ref int index, ScriptContext context)
        {
            var token = tokens[index];

            if (token.Type == TokenType.Symbol && token.Text == "!")
            {
                index++;

                return ScriptValue.FromBool(!ParseUnary(tokens, ref index, context).ToBool());
            }

            if (token.Type == TokenType.Symbol && token.Text == "-")
            {
                index++;

                return ScriptValue.FromFloat(-ParseUnary(tokens, ref index, context).ToFloat());
            }

            return ParsePrimary(tokens, ref index, context);
        }

        private static ScriptValue ParsePrimary(List<Token> tokens, ref int index, ScriptContext context)
        {
            var token = tokens[index];

            switch (token.Type)
            {
                case TokenType.Number:
                    index++;

                    return ScriptValue.FromFloat(token.Number);

                case TokenType.Text:
                    index++;

                    return ScriptValue.FromString(token.Text);

                case TokenType.Name:
                    index++;

                    if (string.Equals(token.Text, "true", StringComparison.OrdinalIgnoreCase))
                        return ScriptValue.FromBool(true);

                    if (string.Equals(token.Text, "false", StringComparison.OrdinalIgnoreCase))
                        return ScriptValue.FromBool(false);

                    if (context.TryGet(token.Text, out var value))
                        return value;

                    throw new FormatException($"Unknown identifier '{token.Text}'");

                case TokenType.Symbol when token.Text == "(":
                {
                    index++;

                    var inner = ParseConditional(tokens, ref index, context);

                    if (tokens[index].Type != TokenType.Symbol || tokens[index].Text != ")")
                        throw new FormatException("Missing ')'");

                    index++;

                    return inner;
                }

                default:
                    throw new FormatException($"Unexpected token '{token.Text}'");
            }
        }

        private static int Precedence(string symbol) => symbol switch
        {
            "||" or "or" => 10,

            "&&" or "and" => 20,

            ">" or "gt" or ">=" or "gte" or "ge" => 30,

            "<" or "lt" or "<=" or "lte" or "le" => 30,

            "=" or "==" or "===" or "eq" => 30,

            "!=" or "!==" or "ne" or "neq" => 30,

            "+" or "-" => 40,

            "*" or "/" or "%" => 50,

            _ => -1
        };

        private static ScriptValue Apply(string symbol, ScriptValue left, ScriptValue right) => symbol switch
        {
            ">" or "gt" => ScriptValue.FromBool(left.ToFloat() > right.ToFloat()),

            ">=" or "gte" or "ge" => ScriptValue.FromBool(left.ToFloat() >= right.ToFloat()),

            "<" or "lt" => ScriptValue.FromBool(left.ToFloat() < right.ToFloat()),

            "<=" or "lte" or "le" => ScriptValue.FromBool(left.ToFloat() <= right.ToFloat()),

            "=" or "==" or "===" or "eq" => ScriptValue.FromBool(left.SameAs(right)),

            "!=" or "!==" or "ne" or "neq" => ScriptValue.FromBool(!left.SameAs(right)),

            "+" => ScriptValue.FromFloat(left.ToFloat() + right.ToFloat()),

            "-" => ScriptValue.FromFloat(left.ToFloat() - right.ToFloat()),

            "*" => ScriptValue.FromFloat(left.ToFloat() * right.ToFloat()),

            "/" => ScriptValue.FromFloat(left.ToFloat() / right.ToFloat()),

            "%" => ScriptValue.FromFloat(left.ToFloat() % right.ToFloat()),

            "||" or "or" => ScriptValue.FromBool(left.ToBool() || right.ToBool()),

            "&&" or "and" => ScriptValue.FromBool(left.ToBool() && right.ToBool()),

            _ => throw new FormatException($"Unsupported operator '{symbol}'")
        };
    }
}