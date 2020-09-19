using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.DialogWindows
{
    public class SaveDialog : DialogWindow
    {
        private List fileList;
        private TextInput textInput;
        private Button saveButton;
        private Button cancelButton;
        private string projectName;
        private string warningMessage;
        private bool showWarning;
        private Vector2 warningOffset;
        private List<ListItem> currentFiles;

        public SaveDialog(CellED parent) : base(parent, 500, 271, new Vector2(parent.ScreenWidth / 2, parent.ScreenHeight / 2), "Save Project")
        {
            fileList = new List(this, Width - 30, 16, 10, Pos + new Vector2(15, 20), Color.Transparent, true);
            fileList.DisconnectInput();
            fileList.ItemSelected += OnItemSelection;

            textInput = new TextInput(this, Width - 30, fileList.Pos + new Vector2(0, 5 + fileList.Height));
            textInput.TextChanged += OnTextChanged;

            cancelButton = new Button(this, 60, 30, Pos + new Vector2(Width - 75, Height - 45));
            cancelButton.DisconnectInput();
            cancelButton.ButtonText = "Cancel";
            cancelButton.ButtonClicked += Hide;

            saveButton = new Button(this, 60, 30, cancelButton.Pos + new Vector2(-70, 0));
            saveButton.DisconnectInput();
            saveButton.ButtonText = "Save";
            saveButton.ButtonClicked += OnSaveClicked;

            projectName = warningMessage = "";
            showWarning = false;
            warningOffset = textInput.Pos + new Vector2(5, textInput.Height + 12);

            CreateFileListItems();
        }

        private void OnSaveClicked()
        {
            if (projectName != null)
            {
                if (projectName.Length > 0)
                {
                    FileHandler.SaveProject(parent.objectHandler.WorldObjects, projectName, true);
                    Hide();
                }
            }
        }

        private void OnTextChanged(string text)
        {
            if (fileList.CurrentSelection != null)
            {
                fileList.CurrentSelection.State = ListItem.ItemState.None;
                fileList.CurrentSelection = null;
            }
            CheckOverwrite(text);
            projectName = text;
        }

        private void CheckOverwrite(string text)
        {
            if (parent.CurrentFile != text)
            {
                ListItem item = currentFiles.Find(obj => obj.Label == text);
                if (item != null)
                {
                    if (item.Label != parent.CurrentFile)
                    {
                        showWarning = true;
                        warningMessage = "Do you intend to overwrite?";
                    }
                }
                else
                {
                    showWarning = false;
                    warningMessage = "";
                }
            }
            else
            {
                showWarning = false;
                warningMessage = "";
            }
        }

        private void OnItemSelection(ListItem item)
        {
            projectName = item.Label;
            textInput.Text = item.Label;
            CheckOverwrite(item.Label);
        }

        private void CreateFileListItems()
        {
            currentFiles = FileHandler.GetProjectFiles(fileList);
            fileList.SetItemList(currentFiles);
        }

        public override void Hide()
        {
            base.Hide();
            fileList.DisconnectInput();
            textInput.DisconnectInput();
            cancelButton.DisconnectInput();
            saveButton.DisconnectInput();

            if (fileList.CurrentSelection != null)
            {
                fileList.CurrentSelection.State = ListItem.ItemState.None;
                fileList.CurrentSelection = null;
            }
            textInput.Text = "";
            warningMessage = "";
            showWarning = false;
        }

        public override void Show()
        {
            base.Show();
            fileList.ConnectInput();
            textInput.ConnectInput();
            cancelButton.ConnectInput();
            saveButton.ConnectInput();
            CreateFileListItems();
            if (parent.CurrentFile != "")
            {
                textInput.Text = parent.CurrentFile;
                projectName = parent.CurrentFile;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isActive)
            {
                fileList.Draw(spriteBatch);
                textInput.Draw(spriteBatch);
                cancelButton.Draw(spriteBatch);
                saveButton.Draw(spriteBatch);
                if (showWarning)
                {
                    spriteBatch.DrawString(parent.UIFontSmall, warningMessage, warningOffset, parent.TextColor);
                }
            }
        }

        public override void Update()
        {
            base.Update();
            fileList.Update();
            textInput.Update();
            cancelButton.Update();
            saveButton.Update();
        }
    }
}
