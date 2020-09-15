using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CellED.UI.Elements
{
    public class DialogWindow : UIObjectBase
    {
        protected Texture2D borderTexture;
        protected Vector2 borderOffset;
        protected readonly string windowName;
        protected Vector2 windowNameOffset;

        public bool isActive;

        public delegate void MouseEvent(float x, float y);
        public event MouseEvent MouseMoved;
        public event MouseEvent MouseLeftPressed;

        public DialogWindow(CellED parent, int width, int height, Vector2 pos, string windowName)
            : base(parent, width, height, pos, Parameters.Filled, parent.BaseColor, parent.TextColor, 1)
        {
            this.windowName = windowName;
            Pos -= new Vector2(width / 2, height / 2);
            isActive = false;
            GenerateBorder(BorderWidth);
        }

        public virtual void Show()
        {
            isActive = true;
            parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftPressed;
            parent.inputHandler.MouseMovedEvent += OnMouseMoved;
            parent.inputHandler.KeyTappedEvent += OnKeyPressed;
            parent.objectHandler.DisableInput();
        }

        private void OnKeyPressed(Keys key)
        {
            if (key == Keys.Escape)
            {
                Hide();
            }
        }

        private void OnMouseMoved(float x, float y)
        {
            if (Contains(x, y))
            {
                MouseMoved?.Invoke(x, y);
            }
        }

        private void OnMouseLeftPressed(float x, float y)
        {
            if (Contains(x, y))
            {
                MouseLeftPressed?.Invoke(x, y);
            }
        }

        public virtual void Hide()
        {
            isActive = false;
            parent.inputHandler.MouseLeftPressedEvent -= OnMouseLeftPressed;
            parent.inputHandler.MouseMovedEvent -= OnMouseMoved;
            parent.inputHandler.KeyTappedEvent -= OnKeyPressed;
            //parent.objectHandler.EnableInput();
        }

        public override void GenerateBorder(int borderWidth)
        {
            borderTexture = new Texture2D(parent.GraphicsDevice, Width - 12, Height - 14);
            Vector2 textSize = parent.UIFont.MeasureString(windowName);
            borderOffset = Pos + new Vector2(6, 8);
            windowNameOffset = borderOffset + new Vector2(4, -(int)textSize.Y / 2);
            borderTexture.SetData(Utilities.CreateGappedRectangularBorder(borderTexture.Width, borderTexture.Height, borderWidth, BorderColor, textSize));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                base.Draw(spriteBatch);
                spriteBatch.Draw(borderTexture, borderOffset, BorderColor);
                spriteBatch.DrawString(parent.UIFont, windowName, windowNameOffset, parent.TextColor);
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
