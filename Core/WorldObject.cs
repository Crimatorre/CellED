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

        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D Outline { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Z { get; set; }
        public Color Color { get; set; }
        public Color[,] ColorData { get; set; }

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
            Outline = new Texture2D(parent.parent.GraphicsDevice, texture.Width, texture.Height);

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
            Outline = worldObject.Outline;
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
            ColorData = Utilities.ColorData1Dto2D(colorData, Texture.Width, Texture.Height);
            Outline.SetData(Utilities.CreateOutlineTexture(colorData, Texture.Width, Texture.Height, 5));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Pos, null, Color, Rotation, Origin, Scale, Effects, Z);
            if (parent.currentSelection == this && parent.CurrentOperation == ObjectHandler.ObjectOperation.Placing)
            {
                spriteBatch.Draw(Outline, Pos, null, Color.Orange, Rotation, Origin, Scale, Effects, 0f);
            }
            else if (parent.currentSelection == this )
            {
                spriteBatch.Draw(Outline, Pos, null, Color.Yellow, Rotation, Origin, Scale, Effects, 0f);
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

            if (xRotated > 0 && xRotated < Texture.Width * Scale &&
                yRotated > 0 && yRotated < Texture.Height * Scale)
            {
                if (ColorData[(int)(xRotated / Scale), (int)(yRotated / Scale)] != Color.Transparent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
