using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CellED.UI
{
    public class TopBarMenuButton : UIObjectBase
    {
        private TopBarMenu menu;

        protected string label;
        protected Vector2 labelOffset;

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                labelOffset = new Vector2(5, Height / 2 - parent.UIFontSmall.MeasureString(label).Y / 2);
                labelOffset.Ceiling();
            }
        }

        public delegate void ButtonPress();
        public event ButtonPress ButtonPressed;

        public TopBarMenuButton(TopBarMenu parent, string label)
            : base(parent.parent, parent.MenuWidth, parent.MenuButtonHeight, parent.buttonSpacing, Parameters.Filled, parent.parent.BaseColorDark, null, 0)
        {
            menu = parent;
            Label = label;
        }

        public void OnMouseMoved(float x, float y)
        {
            if (Contains(x, y))
            {
                Color = parent.HoverColor;
            }
            else
            {
                Color = parent.BaseColorDark;
            }
        }

        public void OnMouseLeftPressed(float x, float y)
        {
            if (Contains(x, y))
            {
                ButtonPressed?.Invoke();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(parent.UIFontSmall, Label, Pos + labelOffset, parent.TextColor);
        }

        protected bool Contains(float x, float y)
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