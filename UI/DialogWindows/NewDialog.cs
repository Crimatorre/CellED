using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.DialogWindows
{
    public class NewDialog : DialogWindow
    {
        private TextInput textInput;
        private Button cancelButton;
        private Button createButton;
        private bool showWarning;
        private string warningMessage = "Do you intend to overwrite?";
        private Vector2 warningOffset;


        public string ProjectName { get; private set; }

        public NewDialog(CellED parent)
            : base(parent, 400, 100, new Vector2(parent.ScreenWidth / 2, parent.ScreenHeight / 2), "New Project")
        {
            textInput = new TextInput(this, Width - 30, Pos + new Vector2(15, 20));
            textInput.TextChanged += OnTextChanged;

            cancelButton = new Button(this, 60, 30, Pos + new Vector2(Width - 75, Height - 45));
            cancelButton.DisconnectInput();
            cancelButton.ButtonText = "Cancel";
            cancelButton.ButtonClicked += Hide;

            createButton = new Button(this, 60, 30, cancelButton.Pos + new Vector2(-70, 0));
            createButton.DisconnectInput();
            createButton.ButtonText = "Create";
            createButton.ButtonClicked += OnCreateClicked;

            warningOffset = Pos + new Vector2(20, 62);
        }

        private void OnTextChanged(string text)
        {
            ProjectName = text;
            if (text == parent.CurrentFile && text != "")
            {
                showWarning = true;
            }
            else
            {
                showWarning = false;
            }
        }

        private void OnCreateClicked()
        {
            if (ProjectName != null)
            {
                if (ProjectName.Length > 0)
                {
                    DateTime now = DateTime.Now;
                    if (parent.CurrentFile == "" && parent.objectHandler.WorldObjects.Count > 0)
                    {
                        parent.CurrentFile = string.Format("unnamed-{0}{1}{2}{3}{4}", now.Day, now.Month, now.Year, now.Hour, now.Minute);
                        FileHandler.SaveProject(parent.objectHandler.WorldObjects, parent.CurrentFile);
                    }

                    FileHandler.LoadProject(".template", parent.objectHandler);
                    parent.CurrentFile = ProjectName;
                    FileHandler.SaveProject(parent.objectHandler.WorldObjects, parent.CurrentFile);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isActive)
            {
                cancelButton.Draw(spriteBatch);
                createButton.Draw(spriteBatch);
                textInput.Draw(spriteBatch);
                if (showWarning)
                {
                    spriteBatch.DrawString(parent.UIFontSmall, warningMessage, warningOffset, parent.TextColor);
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            cancelButton.DisconnectInput();
            createButton.DisconnectInput();
            textInput.DisconnectInput();
        }

        public override void Show()
        {
            base.Show();
            cancelButton.ConnectInput();
            createButton.ConnectInput();
            textInput.ConnectInput();
        }

        public override void Update()
        {
            base.Update();
            cancelButton.Update();
            createButton.Update();
            textInput.Update();
        }
    }
}
