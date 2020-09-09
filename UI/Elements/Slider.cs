using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.Elements
{
    public class Slider : UIObjectBase
    {
        public Vector2 sliderPos;

        private float sliderValue;

        public Texture2D SliderTexture { get; protected set; }
        public Color SliderColor { get; protected set; }
        public Texture2D RailTexture { get; protected set; }
        public Vector2 SliderOffset { get; protected set; }
        public float SliderValue
        {
            get
            {
                return sliderValue;
            }
            set
            {
                sliderValue = value;

                float xMin = Pos.X + SliderOffset.X;
                float xMax = Pos.X - (SliderOffset.X + Height * 2) + Width;
                sliderPos.X = (sliderValue - SliderMinValue) / (SliderMaxValue - SliderMinValue) * (xMax - xMin) + xMin;
            }
        }
        public float SliderMinValue { get; set; }
        public float SliderMaxValue { get; set; }
        public bool isPressed { get; protected set; }
        private float MousePadding { get; set; }

        public delegate void ValueEvent(float value);
        public event ValueEvent ValueChanged;

        public Slider(UIObjectBase parent, int railWidth, int railHeight, float maxValue, float minValue, Vector2 pos)
            : base(parent.parent, railWidth, railHeight, pos, Parameters.Filled, null, null, 0)
        {
            SliderMaxValue = maxValue;
            SliderMinValue = minValue;
            SliderValue = minValue;
            SliderOffset = new Vector2(3, - (Height * 2 - Height / 2));
            sliderPos = Pos + SliderOffset;
            SliderColor = parent.parent.BaseColorLight;
        }

        public void OnMouseLeftPressed(float x, float y)
        {
            if (Contains(x, y))
            {
                isPressed = true;
                SliderColor = parent.HoverColor;
                MousePadding = x - sliderPos.X; 
            }
        }

        public void OnMouseLeftReleased(float x, float y)
        {
            if (isPressed)
            {
                isPressed = false;
                SliderColor = parent.BaseColorLight;
            }
        }

        public void OnMouseMoved(float x, float y)
        {
            if (isPressed)
            {
                float xNext = x - MousePadding;
                float xMin = Pos.X + SliderOffset.X;
                float xMax = Pos.X - (SliderOffset.X + Height * 2) + Width;
                if (xNext < Pos.X + SliderOffset.X)
                {
                    sliderPos.X = Pos.X + SliderOffset.X;
                }
                else if (xNext > Pos.X - (SliderOffset.X + Height * 2) + Width)
                {
                    sliderPos.X = Pos.X - (SliderOffset.X + Height * 2) + Width;
                }
                else
                {
                    sliderPos.X = xNext;
                }
                CalculateValue(sliderPos.X, xMin, xMax);
            }
        }

        private void CalculateValue(float xPos, float xMin, float xMax)
        {
            SliderValue = (SliderMaxValue - SliderMinValue) * (xPos - xMin) / (xMax - xMin) + SliderMinValue;
            ValueChanged?.Invoke(SliderValue);
        }

        public override void GenerateTexture()
        {
            SliderTexture = new Texture2D(parent.GraphicsDevice, Height * 2, Height * 4);
            RailTexture = new Texture2D(parent.GraphicsDevice, Width, Height);

            SliderTexture.SetData(Utilities.CreateRectangleTexture(SliderTexture.Width * SliderTexture.Height));
            RailTexture.SetData(Utilities.CreateRectangleTexture(RailTexture.Width * RailTexture.Height));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RailTexture, Pos, parent.BaseColorDark);
            spriteBatch.Draw(SliderTexture, sliderPos, SliderColor);
        }

        public override void Update()
        {
            base.Update();
        }

        private bool Contains(float x, float y)
        {
            if (x > sliderPos.X && x < sliderPos.X + Height * 2 &&
                y > sliderPos.Y && y < sliderPos.Y + Height * 4)
            {
                return true;
            }
            return false;
        }
    }
}
