using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CellED.Core;
using System;
using System.Diagnostics;
using CellED.UI;
using System.Collections.Generic;
using System.Dynamic;

namespace CellED
{
    public class CellED : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteBatch spriteBatchUI;

        public readonly int ScreenWidth;
        public readonly int ScreenHeight;

        public readonly Color BaseColor;
        public readonly Color BaseColorDark;
        public readonly Color BaseColorLight;
        public readonly Color SelectionColor;
        public readonly Color HoverColor;
        public readonly Color TextColor;

        public SpriteFont UIFont;
        public SpriteFont UIFontSmall;
        public InputHandler inputHandler;
        public Camera camera;
        public ObjectHandler objectHandler;
        public Grid grid;
        public UIManager uiManager;

        public string CurrentFile { get; set; }
        public ProgramState State { get; set; }
        public Vector2 Offset { get; private set; }

        [Flags]
        public enum ProgramState
        {
            None = 0,
            GridEnabled = 1,
            GridInputDisabled = 2,
            IsScrolling = 4,
            IsPlacingItem = 8,
        }

        public CellED()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            ScreenWidth = 1280; //1600
            ScreenHeight = 720; //900

            BaseColor = new Color(90, 90, 90);
            BaseColorDark = new Color(70, 70, 70);
            BaseColorLight = new Color(170, 170, 170);
            SelectionColor = new Color(0, 184, 204);
            HoverColor = new Color(0, 113, 133);
            TextColor = new Color(220, 220, 220);

            ScreenWidth = 1600;
            ScreenHeight = 900;

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //fps

            State = ProgramState.GridEnabled;
            Offset = Vector2.Zero;
        }

        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatchUI = new SpriteBatch(GraphicsDevice);
            UIFont = Content.Load<SpriteFont>("Fonts/UIFont");
            UIFontSmall = Content.Load<SpriteFont>("Fonts/UIFontSmall");

            inputHandler = new InputHandler(this);
            objectHandler = new ObjectHandler(this);

            grid = new Grid(this);
            camera = new Camera(this);
            uiManager = new UIManager(this);
            
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
                
            //UpdateMouseOffset();
            grid.Update();
            inputHandler.Update();
            uiManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.CameraPosition);

            grid.Draw(spriteBatch);
            objectHandler.Draw(spriteBatch);

            spriteBatch.End();

            // UI drawing
            spriteBatchUI.Begin();

            uiManager.Draw(spriteBatchUI);

            spriteBatchUI.End();

            base.Draw(gameTime);
        }

        public void Quit()
        {
            Exit();
        }
    }
}
