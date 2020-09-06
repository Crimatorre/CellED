using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI
{
    public class TopBar : UIObjectBase
    {
        public TopBar(CellED parent, int width, int height, Vector2 pos, Parameters param,
            Color? color = null, Color? borderColor = null, int borderWidth = 0)
            : base(parent, width, height, pos, param, color, borderColor, borderWidth)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
