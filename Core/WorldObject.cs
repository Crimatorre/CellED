using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.Core
{
    public class WorldObject
    {
        public ObjectHandler parent;

        private Vector2 _pos;

        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Z { get; set; }
        public Color Color { get; set; }

        public WorldObject(ObjectHandler parent, Texture2D texture, string name,
            Vector2? pos = null, float scale = 0.5f, float rotation = 0f,
            Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float z = 0.01f)
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
        }

        public WorldObject(WorldObject worldObject, Vector2 pos)
        {
            this.parent = worldObject.parent;
            Texture = worldObject.Texture;
            Name = worldObject.Name;
            Pos = pos;
            Origin = worldObject.Origin;
            Scale = worldObject.Scale;
            Rotation = worldObject.Rotation;
            Color = worldObject.Color;
            Effects = worldObject.Effects;
            Z = worldObject.Z;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Pos, null, Color, Rotation, Origin, Scale, Effects, Z);
        }
    }
}
