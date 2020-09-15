using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace CellED.UI.DialogWindows
{
    public class LoadDialog : DialogWindow
    {
        private List fileList;
        private Button loadButton;
        private Button cancelButton;
        private string selectedFile;
        public LoadDialog(CellED parent)
            : base(parent, 500, 300, new Vector2(parent.ScreenWidth / 2, parent.ScreenHeight / 2), "Load Project")
        {
            fileList = new List(this, Width - 30, 16, 13, Pos + new Vector2(15, 20), Color.Transparent, true);
            fileList.DisconnectInput();
            fileList.ItemSelected += OnItemSelection;

            cancelButton = new Button(this, 60, 30, Pos + new Vector2(Width - 75, Height - 45));
            cancelButton.DisconnectInput();
            cancelButton.ButtonText = "Cancel";
            cancelButton.ButtonClicked += Hide;

            loadButton = new Button(this, 60, 30, cancelButton.Pos + new Vector2(-70, 0));
            loadButton.DisconnectInput();
            loadButton.ButtonText = "Load";
            loadButton.ButtonClicked += OnLoadClicked;

            CreateFileListItems();
        }

        private void OnLoadClicked()
        {
            if (selectedFile != null)
            {
                FileHandler.LoadProject(selectedFile, parent.objectHandler);
                parent.camera.ResetCamera();
                Hide();
            }
        }

        private void OnItemSelection(ListItem item)
        {
            selectedFile = item.Label;
        }

        private void CreateFileListItems()
        {
            fileList.SetItemList(FileHandler.GetProjectFiles(fileList));
        }

        public override void Hide()
        {
            base.Hide();
            loadButton.DisconnectInput();
            cancelButton.DisconnectInput();
            fileList.DisconnectInput();
            if (fileList.CurrentSelection != null)
            {
                fileList.CurrentSelection.State = ListItem.ItemState.None;
                fileList.CurrentSelection = null;
            }
        }

        public override void Show()
        {
            base.Show();
            fileList.ConnectInput();
            loadButton.ConnectInput();
            cancelButton.ConnectInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isActive)
            {
                fileList.Draw(spriteBatch);
                loadButton.Draw(spriteBatch);
                cancelButton.Draw(spriteBatch);
            }
        }

        public override void Update()
        {
            base.Update();
            fileList.Update();
            loadButton.Update();
            cancelButton.Update();
        }
    }

    public class FileLoader
    {
        private string fileName;
        private ObjectHandler objectHandler;
        public FileLoader(string fileName, ref ObjectHandler objectHandler)
        {
            this.fileName = fileName;
            this.objectHandler = objectHandler;
        }
    }
}
