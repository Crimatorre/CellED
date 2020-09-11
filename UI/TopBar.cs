using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI
{
    public class TopBar : UIObjectBase
    {
        protected List<TopBarMenu> menu;
        protected Vector2 nextPos;
        private bool inputState;

        public TopBar(CellED parent, int width, int height, Vector2 pos, Parameters param,
            Color? color = null, Color? borderColor = null, int borderWidth = 0)
            : base(parent, width, height, pos, param, color, borderColor, borderWidth)
        {
            nextPos = new Vector2(2, 2);
            menu = new List<TopBarMenu>();
            inputState = true;
        }

        public void AddButton(TopBarMenu button)
        {
            menu.Add(button);
        }

        public Vector2 GetNextPos(int width)
        {
            Vector2 returnValue = nextPos;
            nextPos.X += width + 1;
            return Pos + returnValue;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (TopBarMenu tbButton in menu)
            {
                tbButton.Draw(spriteBatch);
            }
        }

        public void OnInputStateChanged(bool newState)
        {
            if (newState != inputState)
            {
                if (newState)
                {
                    foreach (TopBarMenu tbButton in menu)
                    {
                        tbButton.ConnectInput();
                    }
                }
                else
                {
                    foreach (TopBarMenu tbButton in menu)
                    {
                        tbButton.DisconnectInput();
                    }
                }
                inputState = newState;
            }
        }

        public override void Update()
        {
            base.Update();
            foreach (TopBarMenu tbButton in menu)
            {
                tbButton.Update();
            }
        }
    }
}
