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

        public SideBarTool(SideBar sideBar, int height, string name = "Item")
            : base(sideBar.parent, sideBar.Width - sideBar.Padding * 2, height,
                  sideBar.Pos + new Vector2(sideBar.Padding) + sideBar.GetToolOffset(height),
                  Parameters.None, null, sideBar.TextColor, 1)
        {
            Name = name;
            TextOffset = new Vector2(5, (int)(-TextSize.Y / 2));
            GenerateBorder(1);
            sideBar.parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftPressed;
        }

        // passes event further to ui elements 
        public void OnMouseLeftPressed(float x, float y)
        {
            MouseLeftPressed?.Invoke(x, y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(parent.UIFont, Name, Pos + TextOffset, BorderColor);
        }

        public override void GenerateBorder(int borderWidth)
        {
            if (Texture == null)
            {
                Texture = new Texture2D(parent.GraphicsDevice, Width, Height);
            }
            Color[] colorData = new Color[Width * Height];
            Texture.GetData(colorData);
            Texture.SetData(Utilities.CreateGappedRectangularBorder(colorData, Width, Height, borderWidth, BorderColor, TextSize));
        }
    }
}
