using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.Elements
{
    public class Button : UIObjectBase
    {
        enum ButtonState
        {
            None,
            Clicked
        }

        private string _buttonText;

        private ButtonState State { get; set; }
        private Texture2D ButtonTexture { get; set; }
        private List<Color> ButtonColors { get; set; }

        public string ButtonText
        {
            get
            {
                return _buttonText;
            }
            set
            {
                _buttonText = value;
                Vector2 textSize = parent.UIFontSmall.MeasureString(_buttonText) / 2;
                textSize.Round();
                ButtonTextOffset = new Vector2(Width / 2, Height / 2) - textSize;
                ButtonTextOffset.Round();
            }
        }
        private Vector2 ButtonTextOffset { get; set; }

        public delegate void ButtonEvent();
        public event ButtonEvent ButtonClicked;

        public Button(UIObjectBase parent, int width, int height, Vector2 pos)
            : base(parent.parent, width, height, pos, Parameters.Filled, parent.parent.BaseColorDark, null, 0)
        {
            State = ButtonState.None;
            ConnectInput();
        }

        public void ConnectInput()
        {
            parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftClickStarted;
            parent.inputHandler.MouseLeftReleasedEvent += OnMouseLeftClickEnded;
        }

        public void DisconnectInput()
        {
            parent.inputHandler.MouseLeftPressedEvent -= OnMouseLeftClickStarted;
            parent.inputHandler.MouseLeftReleasedEvent -= OnMouseLeftClickEnded;
        }

        private void OnMouseLeftClickStarted(float x, float y)
        {
            if (Contains(x, y))
            {
                State = ButtonState.Clicked;
            }
        }

        private void OnMouseLeftClickEnded(float x, float y)
        {
            if (State == ButtonState.Clicked)
            {
                State = ButtonState.None;
                if (Contains(x, y))
                {
                    ButtonClicked?.Invoke();
                }
            }
        }

        public override void GenerateTexture()
        {
            base.GenerateTexture();

            int buttonWidth = Width - 2;
            int buttonHeight = Height - 2;
            ButtonTexture = new Texture2D(parent.GraphicsDevice, buttonWidth, buttonHeight);
            ButtonTexture.SetData(Utilities.CreateRectangleTexture(buttonWidth * buttonHeight));

            ButtonColors = new List<Color>(2);
            ButtonColors.Add(parent.BaseColor);
            ButtonColors.Add(parent.HoverColor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(ButtonTexture, Pos + Vector2.One, null, ButtonColors[(int)State], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            if (ButtonText != null)
            {
                spriteBatch.DrawString(parent.UIFontSmall, ButtonText, Pos + ButtonTextOffset, parent.TextColor);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        private bool Contains(float x, float y)
        {
            if (x > Pos.X && x < Pos.X + Width &&
                y > Pos.Y && y < Pos.Y + Height)
            {
                return true;
            }
            return false;
        }
    }
}
