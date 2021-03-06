using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.Core
{
    public class WorldObject
    {
        public ObjectHandler parent;
        public Texture2D outline;

        private int outlineBorderWidth = 6;
        private Vector2 outlineOffset = Vector2.One*6;

        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Z { get; set; }
        public Color Color { get; set; }
        public byte[,] ColorData { get; set; }

        public WorldObject(ObjectHandler parent, Texture2D texture, string name,
            Vector2? pos = null, float scale = 0.5f, float rotation = 0f,
            Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float z = 0.5f)
        {
            this.parent = parent;
            Texture = texture;
            Name = name;
            Pos = pos ?? Vector2.Zero;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Scale = scale;
            Rotation = rotation;
            Color = color ?? Color.White;
            Effects = spriteEffects;
            Z = z;

            CreateOutlinedTexture();
        }

        public WorldObject(WorldObject worldObject, Vector2 pos)
        {
            parent = worldObject.parent;
            Texture = worldObject.Texture;
            Name = worldObject.Name;
            Pos = pos;
            Origin = worldObject.Origin;
            Scale = worldObject.Scale;
            Rotation = worldObject.Rotation;
            Color = worldObject.Color;
            Effects = worldObject.Effects;
            Z = worldObject.Z;
            outline = worldObject.outline;
            ColorData = worldObject.ColorData;

            ConnectInput();
        }

        public void ConnectInput()
        {
            parent.MouseLeftPressed += OnMouseLeftPressed;
        }

        public void DisconnectInput()
        {
            parent.MouseLeftPressed -= OnMouseLeftPressed;
        }

        public void CreateOutlinedTexture()
        {
            Color[] colorData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(colorData);
            ColorData = Utilities.ColorDatato2DByteData(colorData, Texture.Width, Texture.Height);
            if (!FileHandler.LoadFromPng(parent.parent.GraphicsDevice, out outline, Name))
            {
                outline = new Texture2D(parent.parent.GraphicsDevice, Texture.Width + 2 * outlineBorderWidth, Texture.Height + 2 * outlineBorderWidth);
                outline.SetData(Utilities.CreateOutlineTexture(ColorData, Texture.Width, Texture.Height, outlineBorderWidth));
                FileHandler.SaveAsPng(outline, Name);
                Debug.WriteLine("Generated outline for: " + Name);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Pos, null, Color, Rotation, Origin, Scale, Effects, Z);
            if (parent.currentSelection == this && parent.CurrentOperation == ObjectHandler.ObjectOperation.Placing)
            {
                spriteBatch.Draw(outline, Pos, null, Color.Orange, Rotation, Origin + outlineOffset, Scale, Effects, 0f);
            }
            else if (parent.currentSelection == this )
            {
                spriteBatch.Draw(outline, Pos, null, Color.Yellow, Rotation, Origin + outlineOffset, Scale, Effects, 0f);
            }
        }

        private void OnMouseLeftPressed(float x, float y)
        {
            if (Contains(x, y))
            {
                if (parent.currentSelection != null)
                {
                    if (parent.currentSelection.Z >= Z)
                    {
                        parent.currentSelection = this;
                    }
                }
                else
                {
                    parent.currentSelection = this;
                }
            }
        }

        public void Destroy()
        {
            parent.MouseLeftPressed -= OnMouseLeftPressed;
        }

        private bool Contains(float x, float y)
        {
            Vector2 cameraOffset = parent.parent.camera.CurrentOffset;
            
            // Translation into textures coordinates
            float xTexture = x - cameraOffset.X + Origin.X * Scale - Pos.X;
            float yTexture = y - cameraOffset.Y + Origin.Y * Scale - Pos.Y;

            double cos = Math.Cos(-Rotation);
            double sin = Math.Sin(-Rotation);

            // Moving origing, rotating and moving origin back
            xTexture -= Origin.X * Scale;
            yTexture -= Origin.Y * Scale;

            double xRotated = xTexture * cos - yTexture * sin;
            double yRotated = yTexture * cos + xTexture * sin;

            xRotated += Origin.X * Scale;
            yRotated += Origin.Y * Scale;

            if (Effects.HasFlag(SpriteEffects.FlipVertically))
            {
                yRotated = Texture.Height * Scale - yRotated;
            }
            if (Effects.HasFlag(SpriteEffects.FlipHorizontally))
            {
                xRotated = Texture.Width * Scale - xRotated;
            }

            if (xRotated > 0 && xRotated < Texture.Width * Scale &&
                yRotated > 0 && yRotated < Texture.Height * Scale)
            {
                if (ColorData[(int)(xRotated / Scale), (int)(yRotated / Scale)] != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
