using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.Elements
{
    public class TextInput : UIObjectBase
    {
        private string text;
        private string separator;
        private bool separatorVisible;
        private int separatorTime;
        private readonly Vector2 textOffset;
        private bool isSelected;
        private Texture2D borderTexture;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                if (!value)
                {
                    separatorTime = 0;
                    separatorVisible = false;
                }
                else
                {
                    separatorVisible = true;
                }
            }
        }

        private bool SeparatorVisible
        {
            get
            {
                return separatorVisible;
            }
            set
            {
                separatorVisible = value;
                separator = value ? "|" : "";
            }
        }

        public TextInput(UIObjectBase parent, int width, Vector2 pos)
            : base(parent.parent, width, 25, pos, Parameters.Filled, parent.parent.BaseColorDark, null, 1)
        {
            textOffset = new Vector2(4, 5);
            GenerateBorder(1);
        }

        public void ConnectInput()
        {
            parent.Window.TextInput += OnTextInput;
            parent.inputHandler.MouseLeftPressedEvent += OnMouseClick;
        }

        private void OnMouseClick(float x, float y)
        {
            if (Contains(x, y))
            {
                isSelected = true;
                separator = "|";
            }
            else
            {
                isSelected = false;
                separator = "";
            }
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (isSelected)
            {
                char ch = e.Character;
                if (parent.UIFontSmall.Characters.Contains(ch))
                {
                    text += ch;
                }
                else if (e.Key == Keys.Back)
                {
                    if (text.Length > 0)
                    {
                        text = text.Remove(text.Length - 1);
                    }
                }
            }
        }

        public void DisconnectInput()
        {
            parent.Window.TextInput += OnTextInput;
            parent.inputHandler.MouseLeftPressedEvent += OnMouseClick;
            isSelected = false;
            separatorTime = 0;
        }

        public override void GenerateBorder(int borderWidth)
        {
            BorderColor = parent.BaseColorLight;
            borderTexture = new Texture2D(parent.GraphicsDevice, Width, Height);
            borderTexture.SetData(Utilities.CreateRectangularBorder(new Color[Width * Height], Width, Height, 1, Color.White));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(parent.UIFontSmall, string.Format("{0}{1}", text, separator), Pos + textOffset, parent.TextColor);
            if (isSelected)
            {
                spriteBatch.Draw(borderTexture, Pos, BorderColor);
            }
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

        public override void Update()
        {
            if (isSelected)
            {
                if (separatorTime > 40)
                {
                    separatorTime = 0;
                    SeparatorVisible = SeparatorVisible == false;
                }
                separatorTime++;
            }
        }
    }
}
