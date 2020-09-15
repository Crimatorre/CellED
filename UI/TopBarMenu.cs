using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI
{
    public class TopBarMenu : UIObjectBase
    {
        public readonly int MenuWidth = 150;
        public readonly int MenuButtonHeight = 25;

        protected TopBar topBar;
        protected readonly string label;
        protected Vector2 labelOffset;
        protected List<TopBarMenuButton> menuButtons;
        protected Texture2D separator;
        protected Vector2 separatorOffset;

        public Vector2 buttonOffset;
        public Vector2 buttonSpacing;

        private bool menuVisible;

        public bool MenuVisible
        {
            get
            {
                return menuVisible;
            }
            set
            {
                menuVisible = value;
            }
        }

        public delegate void MouseEvent(float x, float y);
        public event MouseEvent MouseLeftPressed;
        public event MouseEvent MouseMoved;

        public TopBarMenu(TopBar parent, string label, int menuButtonNum = 1)
            : base(parent.parent, (int)parent.parent.UIFontSmall.MeasureString(label).X + 10, parent.Height - 4,
                  parent.GetNextPos((int)parent.parent.UIFontSmall.MeasureString(label).X + 10), Parameters.Filled, parent.parent.BaseColor, null, 0)
        {
            this.label = label;
            labelOffset = Pos + (new Vector2(Width, Height) - this.parent.UIFontSmall.MeasureString(label)) / 2;
            labelOffset.Floor();

            MenuVisible = false;
            menuButtons = new List<TopBarMenuButton>(menuButtonNum);
            buttonOffset = Pos + new Vector2(0, Height + 3);
            buttonSpacing = new Vector2(0, MenuButtonHeight);

            ConnectInput();
        }

        public void ConnectInput()
        {
            parent.inputHandler.MouseMovedEvent += OnMouseMoved;
            parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftClick;
        }

        public void DisconnectInput()
        {
            parent.inputHandler.MouseMovedEvent -= OnMouseMoved;
            parent.inputHandler.MouseLeftPressedEvent -= OnMouseLeftClick;
        }

        protected virtual void OnMouseLeftClick(float x, float y)
        {
            if (Contains(x, y))
            {
                MenuVisible = MenuVisible != true;
                Color = MenuVisible == true ? parent.SelectionColor : parent.HoverColor;

                if (MenuVisible)
                {
                    ConnectMenu();
                }
                else
                {
                    DisconnectMenu();
                }
            }
            else if (MenuContains(x, y) && MenuVisible)
            {
                parent.objectHandler.EnableInput();
                MouseLeftPressed?.Invoke(x, y);
                MenuVisible = false;
                Color = parent.BaseColor;
                DisconnectMenu();
            }
            /*else
            {
                Debug.WriteLine("Here");
                MenuVisible = false;
                Color = parent.BaseColor;
                DisconnectMenu();
            }*/
        }

        protected void ConnectMenu()
        {
            foreach (TopBarMenuButton button in menuButtons)
            {
                MouseMoved += button.OnMouseMoved;
                MouseLeftPressed += button.OnMouseLeftPressed;
            }
            parent.objectHandler.DisableInput();
        }

        protected void DisconnectMenu()
        {
            foreach (TopBarMenuButton button in menuButtons)
            {
                button.Color = parent.BaseColorDark;
                MouseMoved -= button.OnMouseMoved;
                MouseLeftPressed -= button.OnMouseLeftPressed;
            }
        }

        public void AddMenuButton(TopBarMenuButton button)
        {
            button.Pos = buttonOffset + button.Pos * menuButtons.Count;
            menuButtons.Add(button);
        }

        protected virtual void OnMouseMoved(float x, float y)
        {
            if (!MenuVisible)
            {
                if (Contains(x, y))
                {
                    Color = parent.HoverColor;
                }
                else
                {
                    Color = parent.BaseColor;
                }
            }
            else
            {
                MouseMoved?.Invoke(x, y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(parent.UIFontSmall, label, labelOffset, parent.TextColor);
            if (MenuVisible)
            {
                spriteBatch.Draw(separator, separatorOffset, Color.White);
                foreach (TopBarMenuButton button in menuButtons)
                {
                    button.Draw(spriteBatch);
                }
            }
        }

        public override void GenerateTexture()
        {
            base.GenerateTexture();
            separator = new Texture2D(parent.GraphicsDevice, MenuWidth, 3);
            separator.SetData(Utilities.CreateRectangleTexture(MenuWidth * 3, parent.SelectionColor));
            separatorOffset = Pos + new Vector2(0, Height);
        }

        public override void Update()
        {
            base.Update();
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

        protected bool MenuContains(float x, float y)
        {
            if (x > buttonOffset.X && x < buttonOffset.X + MenuWidth &&
                y > buttonOffset.Y && y < buttonOffset.Y + menuButtons.Count * buttonSpacing.Y)
            {
                return true;
            }
            return false;
        }
    }
}
