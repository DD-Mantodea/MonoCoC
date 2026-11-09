using System;
using System.Text;

namespace MonoCoC.Scripting
{
    // 正文解析器：全部手写扫描，不用正则。
    //
    // 原版（Haxe classes/parser/Parser.hx）的结构部分也是手写扫描（indexOf/charAt 循环），
    // 正则只用在「换行归一 / 转义 / 兽人语」这类纯文本杂活上。这里保持一致：
    // 一次遍历处理 <b>/<br>、[b: ...]、[bstart]、[pg+/-]、[if (...){a|b}]、转义与未知 tag。
    public class Parser
    {
        // 打开后会把「[if] 条件求值失败」打到控制台，排查 tag 用
        public static bool Debug;

        private bool _bold;

        private bool _italic;

        private bool _underline;

        private bool _strike;

        private readonly System.Collections.Generic.List<(bool Size, bool Color)> _fontStack = [];

        private readonly System.Collections.Generic.List<string> _fontColors = [];

        private int _fontSize = 20;

        private string _fontFamily = "pala";

        private bool _speech;

        private bool _emittedBold;

        private bool _emittedItalic;

        private bool _emittedUnderline;

        private bool _emittedStrike;

        private string? _emittedFont;

        private int _lastPageBreak = int.MaxValue;

        private bool _textSinceBreak;

        public Parser(ScriptContext context) => Context = context;

        public ScriptContext Context { get; }

        public void Reset()
        {
            _bold = false;

            _italic = false;

            _underline = false;

            _strike = false;

            _fontStack.Clear();

            _fontSize = Context.FontSize;

            _fontFamily = Context.FontFamily;

            _fontColors.Clear();

            _speech = false;

            _emittedBold = false;

            _emittedItalic = false;

            _emittedUnderline = false;

            _emittedStrike = false;

            _emittedFont = null;

            _lastPageBreak = int.MaxValue;
        }

        public string Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            _textSinceBreak = false;

            var result = ParseInternal(ExpandFormats(NormalizeHtml(Normalize(text))));

            if (result.Length > 0 && result[^1] != '\n')
                _lastPageBreak = 0;

            return result;
        }

        private static string Normalize(string text) => text.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\\n", "\n");

        // ── HTML 小标签：<b> / <i> / <u> / <br> / <font> / <span> / <div> ──────
        private static string NormalizeHtml(string text)
        {
            if (text.IndexOf('<') < 0)
                return text;

            var builder = new StringBuilder(text.Length);

            var index = 0;

            while (index < text.Length)
            {
                if (text[index] != '<')
                {
                    builder.Append(text[index]);

                    index++;

                    continue;
                }

                if (TryHtmlTag(text, index, out var replacement, out var length))
                {
                    builder.Append(replacement);

                    index += length;

                    continue;
                }

                builder.Append('<');

                index++;
            }

            return builder.ToString();
        }

        // 识别一个 HTML 标签；允许 `< b >`、`< / b >` 这种带空格的写法
        private static bool TryHtmlTag(string text, int start, out string replacement, out int length)
        {
            replacement = "";

            length = 0;

            var index = start + 1;

            SkipSpaces(text, ref index);

            var closing = false;

            if (index < text.Length && text[index] == '/')
            {
                closing = true;

                index++;

                SkipSpaces(text, ref index);
            }

            if (index >= text.Length)
                return false;

            var letter = char.ToLowerInvariant(text[index]);

            // <b> / <i> / <u>（<br> 以 b 开头，先让它过，再单独判）
            if (letter is 'b' or 'i' or 'u')
            {
                var after = index + 1;

                SkipSpaces(text, ref after);

                if (after < text.Length && text[after] == '>')
                {
                    replacement = "[" + letter + (closing ? "end]" : "start]");

                    length = after + 1 - start;

                    return true;
                }
            }

            // <br> / <br/>
            if (letter == 'b' && index + 1 < text.Length && char.ToLowerInvariant(text[index + 1]) == 'r')
            {
                var after = index + 2;

                SkipSpaces(text, ref after);

                if (after < text.Length && text[after] == '/')
                {
                    after++;

                    SkipSpaces(text, ref after);
                }

                if (after < text.Length && text[after] == '>')
                {
                    replacement = "\n";

                    length = after + 1 - start;

                    return true;
                }
            }

            // <font color="#rrggbb" size="36"> / </font> → [fontopen:size;color] / [/font]
            if (index + 4 <= text.Length
                && string.Compare(text, index, "font", 0, 4, StringComparison.OrdinalIgnoreCase) == 0)
            {
                var close = text.IndexOf('>', index + 4);

                if (close >= 0)
                {
                    length = close + 1 - start;

                    if (closing)
                    {
                        replacement = "[/font]";

                        return true;
                    }

                    var attrs = text.Substring(index + 4, close - index - 4);

                    var size = TryGetAttribute(attrs, "size", out var sizeText) && int.TryParse(sizeText, out var parsed) ? parsed : 0;

                    var color = TryGetAttribute(attrs, "color", out var colorText) ? colorText : "";

                    replacement = "[fontopen:" + size + ";" + color + "]";

                    return true;
                }
            }

            // <span ...> / <div ...>：整段丢掉
            foreach (var name in FontLikeTags)
            {
                if (index + name.Length > text.Length)
                    continue;

                if (string.Compare(text, index, name, 0, name.Length, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                var close = text.IndexOf('>', index + name.Length);

                if (close < 0)
                    continue;

                replacement = "";

                length = close + 1 - start;

                return true;
            }

            return false;
        }

        private static bool TryGetAttribute(string attrs, string name, out string value)
        {
            value = "";

            var index = attrs.IndexOf(name, StringComparison.OrdinalIgnoreCase);

            if (index < 0)
                return false;

            var equals = attrs.IndexOf('=', index + name.Length);

            if (equals < 0)
                return false;

            var start = equals + 1;

            while (start < attrs.Length && char.IsWhiteSpace(attrs[start]))

                start++;

            if (start >= attrs.Length)
                return false;

            var quote = attrs[start];

            if (quote != '"' && quote != '\'')
                return false;

            var end = attrs.IndexOf(quote, start + 1);

            if (end <= start)
                return false;

            value = attrs.Substring(start + 1, end - start - 1);

            return true;
        }

        private static readonly string[] FontLikeTags = ["font", "span", "div"];

        private static void SkipSpaces(string text, ref int index)
        {
            while (index < text.Length && char.IsWhiteSpace(text[index]))

                index++;
        }

        // ── [say: ...] / [b: ...] → [saystart]...[sayend] / [bstart]...[bend] ──
        private static string ExpandFormats(string text)
        {
            if (text.IndexOf('[') < 0)
                return text;

            var builder = new StringBuilder(text.Length);

            var index = 0;

            while (index < text.Length)
            {
                // 前一格是反斜杠 → 这个 '[' 是转义字面量，原样输出
                if (text[index] != '[' || (index > 0 && text[index - 1] == '\\'))
                {
                    builder.Append(text[index]);

                    index++;

                    continue;
                }

                var close = FindClose(text, index);

                if (close < 0)
                {
                    builder.Append(text, index, text.Length - index);

                    break;
                }

                var content = text.Substring(index + 1, close - index - 1);

                if (TryFormatColon(content, out var format, out var bodyStart))
                {
                    builder.Append('[').Append(format).Append("start]");

                    builder.Append(ExpandFormats(content.Substring(bodyStart).TrimStart(' ')));

                    builder.Append('[').Append(format).Append("end]");

                    index = close + 1;

                    continue;
                }

                // 不是 [b: ...] 这种：只吐一个 '[', 继续往后扫（里面可能还套着 [b: ...]）
                builder.Append('[');

                index++;
            }

            return builder.ToString();
        }

        // `say:xxx` 或 `[b|i|u]{1,3}:xxx`
        private static bool TryFormatColon(string content, out string format, out int bodyStart)
        {
            format = "";

            bodyStart = 0;

            if (content.Length >= 4
                && content[3] == ':'

                && content.AsSpan(0, 3).Equals("say", StringComparison.OrdinalIgnoreCase))
            {
                format = "say";

                bodyStart = 4;

                return true;
            }

            var index = 0;

            while (index < content.Length && index < 3 && IsBiu(content[index]))

                index++;

            if (index > 0 && index < content.Length && content[index] == ':')
            {
                format = content[..index].ToLowerInvariant();

                bodyStart = index + 1;

                return true;
            }

            return false;
        }

        private static bool IsBiu(char c) => c is 'b' or 'B' or 'i' or 'I' or 'u' or 'U' or 's' or 'S';

        private string ParseInternal(string text)
        {
            var builder = new StringBuilder();

            var index = 0;

            while (index < text.Length)
            {
                var open = text.IndexOf('[', index);

                if (open < 0)
                {
                    AppendText(builder, text, index, text.Length - index);

                    break;
                }

                var escaped = open > 0 && text[open - 1] == '\\';

                AppendText(builder, text, index, open - index - (escaped ? 1 : 0));

                if (escaped)
                {
                    builder.Append('[');

                    index = open + 1;

                    continue;
                }

                var close = FindClose(text, open);

                if (close < 0)
                {
                    builder.Append(text, open, text.Length - open);

                    break;
                }

                builder.Append(HandleTag(text.Substring(open + 1, close - open - 1), builder));

                index = close + 1;
            }

            return builder.ToString();
        }

        private string HandleTag(string tag, StringBuilder builder)
        {
            if (TryFormatSwitch(tag, out var format, out var start))
                return ApplyFormat(format, start, builder);

            if (IsIfTag(tag))
                return HandleIf(tag);

            if (tag.StartsWith("size:", StringComparison.OrdinalIgnoreCase))
            {
                var body = tag[5..];

                var separator = body.IndexOfAny([';', ',']);

                var sizeText = separator >= 0 ? body[..separator] : body;

                var family = separator >= 0 ? body[(separator + 1)..].Trim() : Context.FontFamily;

                if (int.TryParse(sizeText.Trim(), out var parsedSize) && parsedSize > 0)
                {
                    _fontStack.Add((true, false));

                    _fontSize = parsedSize;

                    _fontFamily = family.Length > 0 ? family : Context.FontFamily;

                    return $"/f[{_fontFamily},{parsedSize}]";
                }

                return "";
            }

            if (tag.Equals("sizeend", StringComparison.OrdinalIgnoreCase))
            {
                if (_fontStack.Count == 0)
                    return "";

                var (hasSize, _) = _fontStack[^1];

                _fontStack.RemoveAt(_fontStack.Count - 1);

                if (!hasSize)
                    return "";

                _fontSize = Context.FontSize;

                _fontFamily = Context.FontFamily;

                return "/fd";
            }

            if (tag.StartsWith("fontopen:", StringComparison.OrdinalIgnoreCase))
            {
                var body = tag[9..];

                var split = body.IndexOf(';');

                var sizeText = split >= 0 ? body[..split] : body;

                var color = split >= 0 ? body[(split + 1)..] : "";

                var size = int.TryParse(sizeText, out var parsedSize) ? parsedSize : 0;

                if (color.Length == 7 && color[0] == '#')
                    color += "ff";

                _fontStack.Add((size > 0, color.Length > 0));

                if (color.Length > 0)
                    _fontColors.Add(color);

                var output = "";

                if (size > 0)
                {
                    _fontSize = size;

                    output += $"/f[{Context.FontFamily},{size}]";
                }

                if (color.Length > 0)
                    output += $"/c[{color}]";

                return output;
            }

            if (tag.Equals("/font", StringComparison.OrdinalIgnoreCase))
            {
                if (_fontStack.Count == 0)
                    return "";

                var (hasSize, hasColor) = _fontStack[^1];

                _fontStack.RemoveAt(_fontStack.Count - 1);

                if (hasColor && _fontColors.Count > 0)
                    _fontColors.RemoveAt(_fontColors.Count - 1);

                var output = "";

                if (hasSize)
                {
                    _fontSize = Context.FontSize;

                    output += "/fd";
                }

                if (hasColor)
                    output += "/cd";

                return output;
            }

            if (tag.StartsWith("color:", StringComparison.OrdinalIgnoreCase))
            {
                var color = tag[6..].Trim();

                // RichText 的 /c[] 用 #RRGGBBAA；原版只给 #RRGGBB
                if (color.Length == 7 && color[0] == '#')
                    color += "ff";

                return color.Length == 0 ? "" : $"/c[{color}]";
            }

            if (tag.Equals("/color", StringComparison.OrdinalIgnoreCase))
                return "/cd";

            if (TryPageBreak(tag, out var offset))
                return HandlePageBreak(offset);

            // 原版 DoubleArgLookups.hx:167-173：Ember 代词标签
            if (tag.StartsWith("ember ", StringComparison.OrdinalIgnoreCase))
            {
                var pronoun = tag[6..].Trim().ToLowerInvariant();

                var ember = Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.EmberScene>.Get();

                return pronoun switch
                {
                    "ey" => ember.EmberMF("he", "she"),

                    "em" => ember.EmberMF("him", "her"),

                    "eir" => ember.EmberMF("his", "her"),

                    "eirs" => ember.EmberMF("his", "hers"),

                    "emself" => ember.EmberMF("himself", "herself"),

                    "man" => ember.EmberMF("man", "woman"),

                    _ => "[" + tag + "]",
                };
            }

            if (Context.TryGet(tag, out var value))
            {
                var resolved = Escape(value.ToText());

                if (resolved.AsSpan().IndexOfAnyExcept(" \t\r\n") >= 0)
                    _textSinceBreak = true;

                return resolved;
            }

            return "[" + tag + "]";
        }

        // `saystart` / `sayend` / `bstart` / `biuend`（字母部分 1-3 个，中间不能有空格）
        private static bool TryFormatSwitch(string tag, out string format, out bool start)
        {
            format = "";

            start = false;

            if (tag.EndsWith("start", StringComparison.OrdinalIgnoreCase))
                start = true;
            else if (!tag.EndsWith("end", StringComparison.OrdinalIgnoreCase))
                return false;

            var prefix = tag[..^(start ? 5 : 3)];

            if (prefix.Equals("say", StringComparison.OrdinalIgnoreCase))
            {
                format = "say";

                return true;
            }

            if (prefix.Length is < 1 or > 3)
                return false;

            foreach (var c in prefix)
                if (!IsBiu(c))
                    return false;

            format = prefix.ToLowerInvariant();

            return true;
        }

        // `pg` / `pg+` / `pg---`
        private static bool TryPageBreak(string tag, out int offset)
        {
            offset = 0;

            if (tag.Length < 2
                || char.ToLowerInvariant(tag[0]) != 'p'

                || char.ToLowerInvariant(tag[1]) != 'g')

                return false;

            for (var index = 2; index < tag.Length; index++)
                if (tag[index] == '+')
                    offset++;
                else if (tag[index] == '-')
                    offset--;
                else
                    return false;

            return true;
        }

        // `if` / `if(...)`；`iffy` 这种不是
        private static bool IsIfTag(string tag)
        {
            if (tag.Length < 2
                || char.ToLowerInvariant(tag[0]) != 'i'

                || char.ToLowerInvariant(tag[1]) != 'f')

                return false;

            if (tag.Length == 2)
                return true;

            var next = tag[2];

            return !(char.IsLetterOrDigit(next) || next == '_');
        }

        private string ApplyFormat(string format, bool start, StringBuilder builder)
        {
            var quote = "";

            if (format == "say")
            {
                _speech = start;

                _italic = start;

                quote = start ? "\u201c" : "\u201d";
            }
            else
                foreach (var letter in format)
                    switch (letter)
                    {
                        case 'b':
                            _bold = start;

                            break;

                        case 'i':
                            _italic = start;

                            break;

                        case 'u':
                            _underline = start;

                            break;

                        case 's':
                            _strike = start;

                            break;
                    }

            EmitState(builder);

            return quote;
        }

        private void EmitState(StringBuilder builder)
        {
            if (_emittedBold == _bold && _emittedItalic == _italic && _emittedUnderline == _underline && _emittedStrike == _strike)
                return;

            _emittedBold = _bold;

            _emittedItalic = _italic;

            _emittedUnderline = _underline;

            _emittedStrike = _strike;

            var font = FontName();

            if (_emittedFont != font)
            {
                builder.Append("/fd");

                if (font != null)
                    builder.Append($"/f[{font},{_fontSize}]");

                _emittedFont = font;
            }

            builder.Append(_underline ? "/tu" : _strike ? "/ts" : "/td");
        }

        private string FontName()
        {
            if (_bold && _italic)
                return Context.BoldItalicFont;

            if (_bold)
                return Context.BoldFont;

            if (_italic)
                return Context.ItalicFont;

            return null;
        }

        private string HandlePageBreak(int offset)
        {
            if (_textSinceBreak)
                _lastPageBreak = 0;

            _textSinceBreak = false;

            var lines = 2 + offset - _lastPageBreak;

            if (lines <= 0)
                return "";

            _lastPageBreak += lines;

            var result = new string('\n', lines);

            if (_speech)
                result += "\u201c";

            return result;
        }

        private string HandleIf(string tag)
        {
            var index = 2;

            while (index < tag.Length && char.IsWhiteSpace(tag[index]))

                index++;

            var negate = false;

            if (index < tag.Length && tag[index] == '!')
            {
                negate = true;

                index++;

                while (index < tag.Length && char.IsWhiteSpace(tag[index]))

                    index++;
            }

            if (index >= tag.Length || tag[index] != '(')
                return "[" + tag + "]";

            var conditionEnd = FindMatching(tag, index, '(', ')');

            if (conditionEnd < 0)
                return "[" + tag + "]";

            var condition = tag.Substring(index + 1, conditionEnd - index - 1);

            index = conditionEnd + 1;

            while (index < tag.Length && char.IsWhiteSpace(tag[index]))

                index++;

            if (index >= tag.Length || tag[index] != '{')
                return "[" + tag + "]";

            var bodyEnd = FindMatching(tag, index, '{', '}');

            if (bodyEnd < 0)
                return "[" + tag + "]";

            // 先走轻量求值（纯 tag / 数字 / 简单比较）；搞不定再用 Roslyn 动态执行
            // 「本项目形式」的 C# 条件（Player.Corruption > 20、Flags.Get(...) >= 50 这类）。
            if (!ExpressionEvaluator.TryEvaluate(condition, Context, out var value))
            {
                if (!ConditionEvaluator.TryEvaluate(condition, out var scripted, out var error))
                {
                    if (Debug)
                        Console.WriteLine($"[if] 条件求值失败: {condition}  ({error})");

                    return "[" + tag + "]";
                }

                value = ScriptValue.FromBool(scripted);
            }

            var body = tag.Substring(index + 1, bodyEnd - index - 1);

            var split = SplitBranch(body);

            var chosen = (negate ? !value.ToBool() : value.ToBool()) ? split[0] : split[1];

            return ParseInternal(chosen);
        }

        private static string[] SplitBranch(string body)
        {
            var depth = 0;

            for (var index = 0; index < body.Length; index++)
            {
                var character = body[index];

                if (character is '(' or '[' or '{')
                    depth++;

                if (character is ')' or ']' or '}')
                    depth--;

                if (character == '|' && depth == 0)
                    return [body[..index], body[(index + 1)..]];
            }

            return [body, ""];
        }

        private static int FindClose(string text, int open)
        {
            var depth = 0;

            for (var index = open; index < text.Length; index++)
            {
                var character = text[index];

                if (character == '\\')
                {
                    index++;

                    continue;
                }

                if (character == '[')
                    depth++;

                if (character == ']')
                {
                    depth--;

                    if (depth == 0)
                        return index;
                }
            }

            return -1;
        }

        private static int FindMatching(string text, int open, char start, char end)
        {
            var depth = 0;

            for (var index = open; index < text.Length; index++)
            {
                var character = text[index];

                if (character == '\\')
                {
                    index++;

                    continue;
                }

                if (character == start)
                    depth++;

                if (character == end)
                {
                    depth--;

                    if (depth == 0)
                        return index;
                }
            }

            return -1;
        }

        private void AppendText(StringBuilder builder, string text, int start, int count)
        {
            if (count <= 0)
                return;

            for (var index = start; index < start + count; index++)
            {
                if (char.IsWhiteSpace(text[index]))
                    continue;

                _textSinceBreak = true;

                break;
            }

            builder.Append(text, start, count);
        }

        private static string Escape(string value) => value?.Replace("/", "//") ?? "";
    }
}