using CellED.Core;
using CellED.UI.DialogWindows;
using CellED.UI.Elements;
using CellED.UI.SpriteTools;
using CellED.UI.TopBarMenus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static CellED.UI.UIObjectBase;

namespace CellED.UI
{
    public class UIManager
    {

        private List<UIObjectBase> uiObjects;
        private CellED parent;
        private Color FillColor { get; set; }
        public UIManager(CellED parent)
        {
            this.parent = parent;
            uiObjects = new List<UIObjectBase>(3);
            FillColor = new Color(90, 90, 90);
            InitializeUI();
        }

        public void InitializeUI()
        {
            int topBarHeight = 24;
            int sideBarWidth = 240;

            Parameters parameters = Parameters.Filled;

            TopBar topBar = new TopBar(parent, parent.ScreenWidth/3, topBarHeight, new Vector2(parent.ScreenWidth/3, 0), parameters, FillColor);
            FileMenu fileMenu = new FileMenu(topBar);
            topBar.AddButton(fileMenu);
            topBar.AddButton(new EditMenu(topBar));

            parent.objectHandler.InputStateChanged += topBar.OnInputStateChanged;

            SideBar leftSideBar = new SideBar(parent, sideBarWidth, parent.ScreenHeight, Vector2.Zero, parameters, FillColor);
            leftSideBar.AddTool(new TextureCatalog(leftSideBar));
            leftSideBar.AddTool(new GridTools(leftSideBar));

            SideBar rightSideBar = new SideBar(parent, sideBarWidth, parent.ScreenHeight, new Vector2(parent.ScreenWidth - sideBarWidth, 0), parameters, FillColor);
            rightSideBar.AddTool(new SpriteScale(rightSideBar));
            rightSideBar.AddTool(new SpriteZValue(rightSideBar));
            rightSideBar.AddTool(new SpriteColorTint(rightSideBar));
            rightSideBar.AddTool(new SpriteRotation(rightSideBar));

            DialogWindow saveDialog = new SaveDialog(parent, 500, 300, new Vector2(parent.ScreenWidth / 2, parent.ScreenHeight / 2));
            fileMenu.saveButton.ButtonPressed += saveDialog.Show;

            uiObjects.Add(topBar);
            uiObjects.Add(leftSideBar);
            uiObjects.Add(rightSideBar);
            uiObjects.Add(saveDialog);
        }

        public void Update()
        {
            foreach (UIObjectBase ob in uiObjects)
            {
                ob.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIObjectBase ob in uiObjects)
            {
                ob.Draw(spriteBatch);
            }
        }
    }
}
