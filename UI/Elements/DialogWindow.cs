using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.Elements
{
    public class DialogWindow : UIObjectBase
    {
        public DialogWindow(CellED parent, int width, int height, Vector2 pos, Parameters param,
            Color? color = null, Color? borderColor = null, int borderWidth = 0)
            : base(parent, width, height, pos, param, color, borderColor, borderWidth)
        {
        }
    }
}
