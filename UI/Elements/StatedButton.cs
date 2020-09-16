using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.Elements
{
    public class StatedButton : Button
    {
        private bool pressState;
        private ButtonState lastState;

        public bool Pressed
        {
            get
            {
                return pressState;
            }
            set
            {
                pressState = value;
                if (value)
                {
                    State = ButtonState.Pressed;
                }
                else
                {
                    State = ButtonState.None;
                }
            }
        }

        public delegate void ButtonStateEvent(bool state);
        public event ButtonStateEvent ButtonStateChanged;

        public StatedButton(UIObjectBase parent, int width, int height, Vector2 pos)
            : base(parent, width, height, pos)
        {
        }

        protected override void OnMouseLeftClickStarted(float x, float y)
        {
            if (Contains(x, y))
            {
                lastState = State;
                State = ButtonState.Clicked;
            }
        }

        protected override void OnMouseLeftClickEnded(float x, float y)
        {
            if (State == ButtonState.Clicked)
            {
                if (Contains(x, y))
                {
                    Pressed = Pressed != true;
                    ButtonStateChanged?.Invoke(Pressed);
                }
                else
                {
                    State = lastState;
                }
            }
        }

        protected override void OnClickEnded(float x, float y)
        {
            Pressed = Pressed != true;
            ButtonStateChanged?.Invoke(Pressed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
