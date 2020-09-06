using CellED.Core;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI
{
    public class UIObjectBase
    {
        [Flags]
        public enum Parameters
        {
            None = 0,
            Filled = 1,
            Bordered = 2,
            Hidden = 4
        }

        public CellED parent;

        protected Parameters parameters;

        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Pos { get; set; }
        public Color Color { get; set; }
        public Color BorderColor { get; set; }
        public Texture2D Texture { get; set; }
        public int BorderWidth { get; set; }

        public UIObjectBase(CellED parent, int width, int height, Vector2 pos, Parameters param, Color? color = null, Color? borderColor = null, int borderWidth = 0)
        {
            this.parent = parent;
            Width = width;
            Height = height;
            Pos = pos;
            Color = color ?? Color.White;
            BorderColor = borderColor ?? Color.Black;
            BorderWidth = borderWidth;
            parameters = param;

            if (param.HasFlag(Parameters.Filled))
            {
                GenerateTexture();
            }
            if (param.HasFlag(Parameters.Bordered))
            {
                GenerateBorder(BorderWidth);
            }
        }

        public virtual void GenerateBorder(int borderWidth)
        {
            if (Texture == null)
            {
                Texture = new Texture2D(parent.GraphicsDevice, Width, Height);
            }
            Color[] colorData = new Color[Width * Height];
            Texture.GetData(colorData);
            Texture.SetData(Utilities.CreateRectangularBorder(colorData, Width, Height, borderWidth, BorderColor));
        }

        public virtual void GenerateTexture()
        {
            Texture = new Texture2D(parent.GraphicsDevice, Width, Height);
            Texture.SetData(Utilities.CreateRectangleTexture(Width * Height, Color));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!parameters.HasFlag(Parameters.Hidden))
            {
                spriteBatch.Draw(Texture, Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }

        public virtual void Update()
        {

        }
    }
}
