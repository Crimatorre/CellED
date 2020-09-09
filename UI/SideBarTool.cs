using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI
{
    public class SideBarTool : UIObjectBase
    {
        private string Name { get; set; }
        private Vector2 TextOffset { get; set; }
        private Vector2 TextSize
        {
            get
            {
                return parent.UIFont.MeasureString(Name);
            }
        }

        public delegate void MouseEvent(float x, float y);
        public event MouseEvent MouseLeftPressed;
        public event MouseEvent MouseLeftReleased;
        public event MouseEvent MouseMoved;

        public SideBarTool(SideBar sideBar, int height, string name = "Item")
            : base(sideBar.parent, sideBar.Width - sideBar.Padding * 2, height,
                  sideBar.Pos + new Vector2(sideBar.Padding) + sideBar.GetToolOffset(height),
                  Parameters.None, null, sideBar.TextColor, 1)
        {
            Name = name;
            TextOffset = new Vector2(5, (int)(-TextSize.Y / 2));
            GenerateBorder(1);
            sideBar.parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftPressed;
            sideBar.parent.inputHandler.MouseLeftReleasedEvent += OnMouseLeftReleased;
            sideBar.parent.inputHandler.MouseMovedEvent += OnMouseMoved;
        }

        // Event passing further for objects 
        private void OnMouseMoved(float x, float y)
        {
            MouseMoved?.Invoke(x, y);
        }

        private void OnMouseLeftReleased(float x, float y)
        {
            MouseLeftReleased?.Invoke(x, y);
        }
         
        public void OnMouseLeftPressed(float x, float y)
        {
            if (Contains(x, y))
            {
                MouseLeftPressed?.Invoke(x, y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(parent.UIFont, Name, Pos + TextOffset, BorderColor);
        }

        public override void GenerateBorder(int borderWidth)
        {
            Texture = new Texture2D(parent.GraphicsDevice, Width, Height);
            Color[] colorData = new Color[Width * Height];
            Texture.SetData(Utilities.CreateGappedRectangularBorder(colorData, Width, Height, borderWidth, BorderColor, TextSize));
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
