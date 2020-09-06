using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI
{
    public class SideBar : UIObjectBase
    {
        public Color TextColor { get; set; }

        public int Padding
        {
            get
            {
                return 8;
            }
        }

        private int toolOffset;
        private List<SideBarTool> tools;

        public SideBar(CellED parent, int width, int height, Vector2 pos, Parameters param,
            Color? color = null, Color? borderColor = null, int borderWidth = 0)
            : base(parent, width, height, pos, param, color, borderColor, borderWidth)
        {
            toolOffset = 0;
            tools = new List<SideBarTool>();
            TextColor = new Color(170, 170, 170);
        }

        public void AddTool(SideBarTool tool)
        {
            tools.Add(tool);
        }

        public Vector2 GetToolOffset(int height)
        {
            int result = toolOffset;
            toolOffset += height + Padding;
            return new Vector2(0, result);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (SideBarTool tool in tools)
            {
                tool.Draw(spriteBatch);
            }
        }

        public override void Update()
        {
            base.Update();

            foreach (SideBarTool tool in tools)
            {
                tool.Update();
            }
        }
    }
}
