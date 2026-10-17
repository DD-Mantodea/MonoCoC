using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Configuration;
using MonoCoC.Graphics.Shaders;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC
{
    public class CoC : Game
    {
        public static CoC Instance;

        public GraphicsDeviceManager GraphicsDeviceManager;

        public SpriteBatch SpriteBatch;

        public UIManager UIManager;

        public Player Player;

        public GameFlags Flags = new();

        public CoC()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            IsMouseVisible = true;

            Instance = this;
        }

        protected override void Initialize()
        {
            UILayout.Load();

            TextureRegistry.Instance.RegisterProvider("coc", path => ManagerInstance<TextureManager>.Instance[path]);

            ConfigureGraphics();

            InitializeLocalizationAndFonts();

            InitializeContent();

            InitializeRuntime();

            base.Initialize();
        }

        private void ConfigureGraphics()
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = ScreenWidth;

            GraphicsDeviceManager.PreferredBackBufferHeight = ScreenHeight;

            GraphicsDeviceManager.PreferMultiSampling = true;

            GraphicsDeviceManager.IsFullScreen = false;

            GraphicsDeviceManager.ApplyChanges();
        }

        private void InitializeLocalizationAndFonts()
        {
            RichTextDefaults.FontResolver = path =>
            {
                var parts = path.Split(',');

                return UI.UIFonts.GetAt(parts[0].Trim(), int.Parse(parts[1].Trim()));
            };

            OptionStore.LoadGlobal();

            ManagerInstance<LocalizationManager>.Instance.Use("en-us");

            UI.UIFonts.Initialize();

            LanguageService.ApplySaved();

            UI.UIFonts.ApplyToParser(Contents.Scenes.Scene.ScriptContext);
        }

        private static void InitializeContent()
        {
            _ = ManagerInstance<BodyPartManager>.Instance;

            _ = ManagerInstance<StatTypeManager>.Instance;

            _ = ManagerInstance<CharCreationRulesManager>.Instance;

            _ = ManagerInstance<ItemManager>.Instance;

            PerkIds.Touch();

            Contents.Commands.VanillaCommands.Register();

            Contents.Input.Hotkeys.Register();
        }

        private void InitializeRuntime()
        {
            Player = new Player();

            UIManager = new(new ViewContainer());

            UIManager.Instance = UIManager;

            ShaderManager.Instance.Initialize();

            Window.TextInput += OnTextInput;
        }

        private static readonly char[] SpecialCharacters = ['\a', '\b', '\n', '\r', '\f', '\t', '\v'];

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (!IsActive)
                return;

            if (Array.IndexOf(SpecialCharacters, e.Character) >= 0)
                return;

            UIManager?.DispatchTextInput(e.Character);
        }

        public Player NewPlayer()
        {
            Player.RemoveStatuses();

            Player = new Player();

            return Player;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MonoCoC.Contents.Input.HotkeyRegistry.Update();

            ShaderManager.Instance.Update();

            try
            {
                UserInput.Update(IsActive);

                if (UserInput.IsJustPress(Keys.F1))
                    UI.Console.ConsoleOverlay.Toggle();

                UIManager.Update(gameTime);

                UserInput.Dispatch(IsActive);

                // 杈撳叆鍙兘鍒囨崲鍦烘櫙/瑙嗗浘锛氬湪 Draw 鍓嶈ˉ涓€娆℃柊鏍戠殑 Update
                UIManager.UpdateAfterInput(gameTime);

                base.Update(gameTime);
            }
            catch (Exception)
            {

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            UIManager.Draw(SpriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
