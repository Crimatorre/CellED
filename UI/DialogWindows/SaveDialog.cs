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
    public class SaveDialog : DialogWindow
    {
        private List fileList;
        public SaveDialog(CellED parent) : base(parent, 500, 300, new Vector2(parent.ScreenWidth / 2, parent.ScreenHeight / 2), "Save Project")
        {
            fileList = new List(this, Width - 30, 16, 10, Pos + new Vector2(15, 20), Color.Transparent, true);
            fileList.DisconnectInput();
            fileList.ItemSelected += OnItemSelection;
            CreateFileListItems();
        }

        private void OnItemSelection(ListItem item)
        {
            if (!FileHandler.SaveProject(parent.objectHandler.WorldObjects, item.Label))
            {
                if (item.Label == parent.CurrentFile)
                {
                    FileHandler.SaveProject(parent.objectHandler.WorldObjects, item.Label, true);
                }
                else
                {
                    Debug.WriteLine("Do you hand to rewrite other file?");
                }
            }
        }

        private void CreateFileListItems()
        {
            List<ListItem> items = new List<ListItem>();
            for (int i = 0; i < 15; i++)
            {
                items.Add(new ListItem(fileList, "Item " + i));
            }
            fileList.SetItemList(items);
        }

        public override void Hide()
        {
            base.Hide();
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isActive)
            {
                fileList.Draw(spriteBatch);
            }
        }

        public override void Update()
        {
            base.Update();
            fileList.Update();
        }
    }
}
