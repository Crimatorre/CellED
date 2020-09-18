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

        public NewDialog(CellED parent)
            : base(parent, 400, 100, new Vector2(parent.ScreenWidth / 2, parent.ScreenHeight / 2), "New Project")
        {
            textInput = new TextInput(this, Width - 30, Pos + new Vector2(15, 20));

            cancelButton = new Button(this, 60, 30, Pos + new Vector2(Width - 75, Height - 45));
            cancelButton.DisconnectInput();
            cancelButton.ButtonText = "Cancel";
            cancelButton.ButtonClicked += Hide;

            createButton = new Button(this, 60, 30, cancelButton.Pos + new Vector2(-70, 0));
            createButton.DisconnectInput();
            createButton.ButtonText = "Create";
            createButton.ButtonClicked += OnCreateClicked;
        }

        private void OnCreateClicked()
        {
            Debug.WriteLine("Project creation not implemented yet.");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isActive)
            {
                cancelButton.Draw(spriteBatch);
                createButton.Draw(spriteBatch);
                textInput.Draw(spriteBatch);
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
