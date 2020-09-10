using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.Elements
{
    public class Scope : UIObjectBase
    {
        private string _labelText;

        public string LabelText
        {
            get
            {
                return _labelText;
            }
            set
            {
                _labelText = value;
                Vector2 textSize = parent.UIFontSmall.MeasureString(_labelText) / 2;
                textSize.Round();
                TextOffset = new Vector2(Width / 2, Height / 2 + 1) - textSize;
                TextOffset.Round();
            }
        }
        private Vector2 TextOffset { get; set; }

        public Scope(UIObjectBase parent, int width, int height, Vector2 pos)
            : base(parent.parent, width, height, pos, Parameters.Filled, parent.parent.BaseColorDark, null, 0)
        {
            LabelText = "text";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(parent.UIFontSmall, LabelText, Pos + TextOffset, parent.TextColor);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
