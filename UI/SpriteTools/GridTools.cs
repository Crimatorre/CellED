using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.SpriteTools
{
    public class GridTools : SideBarTool
    {
        private Grid grid;

        public SwitchButton GridSwitch { get; set; }
        public SwitchButton GridEditSwitch { get; set; }
        public SwitchButton GridTopBottomSwitch { get; set; }

        public string GridSwitchLabel { get; set; }
        public string GridEditSwitchLabel { get; set; }
        public string GridTopBottomSwitchLabel { get; set; }


        public GridTools(SideBar sideBar) : base(sideBar, 110, "Grid Tools")
        {
            grid = parent.grid;

            GridSwitch = new SwitchButton(this, 18, Pos + new Vector2(Width - 50, 20));
            GridEditSwitch = new SwitchButton(this, 18, Pos + new Vector2(Width - 50, 45));
            GridTopBottomSwitch = new SwitchButton(this, 18, Pos + new Vector2(Width - 50, 70));

            GridSwitch.SwitchParameter = grid.GridEnabled;
            GridEditSwitch.SwitchParameter = grid.EditModeEnabled;
            GridTopBottomSwitch.SwitchParameter = grid.ShowOnTop;

            GridSwitchLabel = "Isometric grid :";
            GridEditSwitchLabel = "Grid edit mode :";
            GridTopBottomSwitchLabel = "Show grid on top:";

            GridSwitch.SwitchValueChanged += OnGridSwitchChanged;
            GridEditSwitch.SwitchValueChanged += OnGridEditSwitchChanged;
            GridTopBottomSwitch.SwitchValueChanged += OnGridTopBottomSwitchChanged;

            MouseLeftPressed += GridSwitch.OnMouseClick;
            MouseLeftPressed += GridEditSwitch.OnMouseClick;
            MouseLeftPressed += GridTopBottomSwitch.OnMouseClick;
        }

        private void OnGridTopBottomSwitchChanged(bool state)
        {
            grid.ShowOnTop = state;
        }

        private void OnGridEditSwitchChanged(bool state)
        {
            grid.EditModeEnabled = state;
        }

        private void OnGridSwitchChanged(bool state)
        {
            grid.GridEnabled = state;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            GridSwitch.Draw(spriteBatch);
            GridEditSwitch.Draw(spriteBatch);
            GridTopBottomSwitch.Draw(spriteBatch);

            spriteBatch.DrawString(parent.UIFontSmall, GridSwitchLabel, Pos + new Vector2(15, 20), parent.TextColor);
            spriteBatch.DrawString(parent.UIFontSmall, GridEditSwitchLabel, Pos + new Vector2(15, 45), parent.TextColor);
            spriteBatch.DrawString(parent.UIFontSmall, GridTopBottomSwitchLabel, Pos + new Vector2(15, 70), parent.TextColor);
        }

        public override void Update()
        {
            base.Update();
            GridSwitch.Update();
            GridEditSwitch.Update();
            GridTopBottomSwitch.Update();
        }
    }
}
