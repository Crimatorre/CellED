using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.DialogWindows
{
    public class SaveDialog : DialogWindow
    {
        public SaveDialog(CellED parent, int width, int height, Vector2 pos) : base(parent, width, height, pos, "Save Project")
        {

        }
    }
}
