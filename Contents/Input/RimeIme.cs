using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LibRimeSharp;
using LibRimeSharp.Data;
using Microsoft.Win32;
using Microsoft.Xna.Framework.Input;

namespace MonoCoC.Contents.Input
{
    public static class RimeIme
    {
        private static RimeAPI? _api;

        private static RimeSession? _session;

        private static bool _initialized;

        private static bool _available;

        public static bool Available
        {
            get
            {
                EnsureInitialized();

                return _available;
            }
        }

        public static bool HasComposition
        {
            get
            {
                if (!Available || _session is null)
                    return false;

                return (_session.Context?.Composition.Preedit.Length ?? 0) > 0;
            }
        }

        public static bool IsAsciiMode => _session?.Status?.IsAsciiMode ?? true;

        public static void ToggleLanguage()
        {
            if (!Available || _session is null)
                return;

            _session.SetOption("ascii_mode", !IsAsciiMode);

            _stateKnown = true;

            _lastChineseMode = !IsAsciiMode;
        }

        public static string Preedit
        {
            get
            {
                if (!Available || _session is null)
                    return "";

                return _session.GetInput();
            }
        }

        public static IReadOnlyList<RimeCandidate> Candidates
        {
            get
            {
                if (!Available || _session is null)
                    return [];

                return _session.Context?.Menu.Candidates ?? [];
            }
        }

        public static int HighlightedIndex =>
            _session?.GetSelectCandidateIndex() ?? -1;

        public static int CompositionCursor =>
            _session?.Context?.Composition.CursorPos ?? 0;

        public static void SetCompositionCaret(int position)
        {
            if (!Available || _session is null)
                return;

            _session.SetCaretPos(position);
        }

        public static void SetCompositionInput(string input)
        {
            if (!Available || _session is null)
                return;

            if (input.Length == 0)
            {
                _session.ClearComposition();

                return;
            }

            _session.SetInput(input);
        }

        public static void RebuildComposition(string input)
        {
            if (!Available || _session is null)
                return;

            _session.ClearComposition();

            foreach (var character in input)
                _session.ProcessKey(character);
        }

        public static bool ProcessKey(Keys key, out string commit)
        {
            commit = "";

            if (!Available || _session is null)
                return false;

            var keyCode = ToKeyCode(key);

            if (keyCode < 0)
                return false;

            var consumed = _session.ProcessKey(keyCode, RimeModifier.None);

            commit = _session.CommitText;

            return consumed || commit.Length > 0;
        }

        public static void ResetComposition()
        {
            if (!Available || _session is null)
                return;

            _session.Escape();
        }

        public static bool SystemLayoutIsChinese =>
            DetectSystemState(out var layoutChinese, out var imeActive, out _) && layoutChinese && imeActive;

        private static bool DetectSystemState(out bool layoutChinese, out bool imeActive, out string detail)
        {
            layoutChinese = false;

            imeActive = false;

            detail = "";

            try
            {
                var window = GetForegroundWindow();

                if (window == IntPtr.Zero)
                {
                    detail = "no-window";

                    return false;
                }

                var thread = GetWindowThreadProcessId(window, out _);

                var layout = GetKeyboardLayout(thread);

                var language = (int)((long)layout & 0xFFFF);

                detail = $"lang=0x{language:X4}";

                layoutChinese = (language & 0x3FF) == 0x04;

                if (!layoutChinese)
                {
                    detail += " layout=en";

                    return false;
                }

                var context = ImmGetContext(window);

                if (context == IntPtr.Zero)
                {
                    var imeWindow = ImmGetDefaultIMEWnd(window);

                    if (imeWindow != IntPtr.Zero)
                    {
                        var open = SendMessage(imeWindow, WmImeControl, ImeGetOpenStatus, IntPtr.Zero);

                        var conversion = SendMessage(imeWindow, WmImeControl, ImeGetConversionMode, IntPtr.Zero);

                        detail += $" ime-wnd=1 open={open != IntPtr.Zero} conv=0x{conversion.ToInt64():X}";

                        imeActive = open != IntPtr.Zero && (conversion.ToInt64() & ImeCModeNative) != 0;

                        return imeActive;
                    }

                    detail += " no-ime-context";

                    return true;
                }

                try
                {
                    if (!ImmGetOpenStatus(context))
                    {
                        detail += " ctx=1 ime-closed";

                        return false;
                    }

                    if (!ImmGetConversionStatus(context, out var conversion, out _))
                    {
                        detail += " conv-failed";

                        imeActive = true;

                        return true;
                    }

                    detail += $" ctx=1 conv=0x{conversion:X}";

                    imeActive = (conversion & ImeCModeNative) != 0;

                    return imeActive;
                }
                finally
                {
                    ImmReleaseContext(window, context);
                }
            }
            catch (Exception exception)
            {
                detail = "error=" + exception.Message;

                return false;
            }
        }

        private const int ImeCModeNative = 0x0001;

        private const int WmImeControl = 0x0283;

        private const int ImeGetConversionMode = 0x0001;

        private const int ImeGetOpenStatus = 0x0005;

        private static long _lastLayoutSync;

        private static bool _stateKnown;

        private static bool _lastLayoutChinese;

        private static bool _lastChineseMode;

        public static void Tick()
        {
            if (!Available || _session is null)
                return;

            var now = Environment.TickCount64;

            if (now - _lastLayoutSync < 100)
                return;

            _lastLayoutSync = now;

            DetectSystemState(out var layoutChinese, out _, out _);

            var layoutChanged = _stateKnown && layoutChinese != _lastLayoutChinese;

            var chinese = !_stateKnown
                ? layoutChinese
                : layoutChanged
                    ? layoutChinese
                    : _lastChineseMode;

            _stateKnown = true;

            _lastLayoutChinese = layoutChinese;

            _lastChineseMode = chinese;

            if (chinese != !IsAsciiMode)
                _session.SetOption("ascii_mode", !chinese);
        }

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr window);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr window, int message, int wParam, IntPtr lParam);

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr window);

        [DllImport("imm32.dll")]
        private static extern bool ImmGetConversionStatus(IntPtr context, out int conversion, out int sentence);

        [DllImport("imm32.dll")]
        private static extern bool ImmGetOpenStatus(IntPtr context);

        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr window, IntPtr context);

        public static void SyncWithSystemLayout()        {
            if (!Available || _session is null)
                return;

            var chinese = SystemLayoutIsChinese;

            if (chinese == !IsAsciiMode)
                return;

            _session.SetOption("ascii_mode", !chinese);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr window, out uint processId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint threadId);

        private static void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;

            try
            {
                var baseDirectory = AppContext.BaseDirectory;

                var share = Path.Combine(baseDirectory, "Share");

                var user = Path.Combine(baseDirectory, "User");

                if (!Directory.Exists(share))
                    throw new DirectoryNotFoundException("Rime Share directory not found: " + share);

                if (!Directory.Exists(user))
                    throw new DirectoryNotFoundException("Rime User directory not found: " + user);

                Directory.CreateDirectory(Path.Combine(user, "logs"));

                _api = RimeAPI.Instance;

                var traits = new RimeTraits
                {
                    AppName = "MonoCoC",
                    SharedDataDir = share,
                    UserDataDir = user,
                    LogDir = Path.Combine(user, "logs"),
                    DistributionName = "MonoCoC",
                    DistributionCodeName = "monococ",
                    DistributionVersion = "1.0"
                };

                _api.Setup(traits);

                _api.Initialize(traits);

                _api.StartMaintenance(true);

                _api.JoinMaintenanceThread();

                _session = _api.CreateSession();

                _session.SetOption("ascii_mode", !SystemLayoutIsChinese);

                _available = true;
            }
            catch (Exception exception)
            {
                _available = false;

                UI.Console.ConsoleOverlay.WriteError("[IME] Rime initialization failed: " + exception.Message);
            }
        }

        private static int ToKeyCode(Keys key)
        {
            if (key is >= Keys.A and <= Keys.Z)
                return 'a' + (key - Keys.A);

            if (key is >= Keys.D0 and <= Keys.D9)
                return '0' + (key - Keys.D0);

            if (key is >= Keys.NumPad0 and <= Keys.NumPad9)
                return '0' + (key - Keys.NumPad0);

            return key switch
            {
                Keys.Space => (int)X11KeySym.Space,
                Keys.Back => (int)X11KeySym.BackSpace,
                Keys.Enter => (int)X11KeySym.Return,
                Keys.Escape => (int)X11KeySym.Escape,
                Keys.Up => (int)X11KeySym.Up,
                Keys.Down => (int)X11KeySym.Down,
                Keys.PageUp => (int)X11KeySym.PageUp,
                Keys.PageDown => (int)X11KeySym.PageDown,
                Keys.OemPeriod => '.',
                Keys.OemComma => ',',
                Keys.OemMinus => '-',
                Keys.OemSemicolon => ';',
                Keys.OemQuestion => '/',
                Keys.OemQuotes => '\'',
                Keys.OemOpenBrackets => '[',
                Keys.OemCloseBrackets => ']',
                _ => -1
            };
        }
    }
}
