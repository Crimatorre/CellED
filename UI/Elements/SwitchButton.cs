using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.Elements
{
    public class SwitchButton : UIObjectBase
    {
        private bool switchParameter;
        public UIObjectBase Parent { get; set; }
        public Texture2D SliderTexture { get; protected set; }
        public Vector2 SliderPos { get; protected set; }
        public int SliderSize { get; protected set; }
        public bool SwitchParameter
        {
            get
            {
                return switchParameter;
            }
            set
            {
                switchParameter = value;
                SwitchSliderPos();
            }
        }

        public delegate void SwitchButtonEvent(bool state);
        public event SwitchButtonEvent SwitchValueChanged;

        public SwitchButton(UIObjectBase parent, int height, Vector2 pos)
            : base(parent.parent, height * 2, height, pos, Parameters.Filled, null, null, 0)
        {
            Parent = parent;
            SliderPos = Pos + Vector2.One;
            SliderSize = height - 2;
            SwitchParameter = false;
            Color = Parent.parent.BaseColorDark;
        }

        public void OnMouseClick(float x, float y)
        {
            if (Contains(x, y))
            {
                SwitchParameter = SwitchParameter == true ? false : true;
                SwitchSliderPos();
            }
        }

        private void SwitchSliderPos()
        {
            if (SwitchParameter)
            {
                SliderPos = Pos + Vector2.One + new Vector2(SliderSize + 2, 0);
                Color = parent.HoverColor;
            }
            else
            {
                SliderPos = Pos + Vector2.One;
                Color = parent.BaseColorDark;
            }
            SwitchValueChanged?.Invoke(SwitchParameter);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(SliderTexture, SliderPos, parent.BaseColorLight);
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

        public override void GenerateTexture()
        {
            base.GenerateTexture();
            SliderTexture = new Texture2D(parent.GraphicsDevice, Height - 2, Height - 2);
            SliderTexture.SetData(Utilities.CreateRectangleTexture((Height - 2) * (Height - 2)));
        }
    }
}
