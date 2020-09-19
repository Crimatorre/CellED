using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CellED.UI.SpriteTools
{
    public class TextureCatalog : SideBarTool
    {
        public List CatalogList { get; private set; }
        private Button LastButton { get; set; }
        private Button NextButton { get; set; }
        private Scope PageScope { get; set; }

        public TextureCatalog(SideBar sideBar) : base(sideBar, 350, "Catalog")
        {
            int catalogWidth = Width * 8 / 10;
            Vector2 catalogPos = Pos + new Vector2(Width / 10);
            CatalogList = new List(this, catalogWidth, 18, 15, catalogPos);
            CreateCatalogItems();

            Vector2 lastButtonOffset = CatalogList.Pos + new Vector2(0, CatalogList.Height + 2);
            Vector2 nextButtonOffset = CatalogList.Pos + new Vector2(CatalogList.Width * 2 / 3 + 1, CatalogList.Height + 2);
            Vector2 scopeOffset = CatalogList.Pos + new Vector2(CatalogList.Width / 3, CatalogList.Height + 2);
            LastButton = new Button(this, catalogWidth / 3, 25, lastButtonOffset);
            NextButton = new Button(this, catalogWidth / 3, 25, nextButtonOffset);
            PageScope = new Scope(this, catalogWidth / 3 + 2, 25, scopeOffset);

            PageScope.LabelText = string.Format("{0}/{1}", CatalogList.CurrentPage, CatalogList.MaxPages);
            NextButton.ButtonText = ">";
            LastButton.ButtonText = "<";

            LastButton.ButtonClicked += CatalogList.LastPage;
            NextButton.ButtonClicked += CatalogList.NextPage;

            CatalogList.PageChanged += OnPageChanged;
        }

        private void OnPageChanged(int page)
        {
            PageScope.LabelText = string.Format("{0}/{1}", page, CatalogList.MaxPages);
        }

        private void CreateCatalogItems()
        {
            List<WorldObject> catalogObjects = parent.objectHandler.CatalogObjects;

            List<ListItem> itemList = new List<ListItem>();
            
            foreach (var catalogItem in catalogObjects)
            {
                itemList.Add(new CatalogListItem(CatalogList, catalogItem.Texture, catalogItem.Name));
            }

            CatalogList.SetItemList(itemList);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            CatalogList.Draw(spriteBatch);
            LastButton.Draw(spriteBatch);
            PageScope.Draw(spriteBatch);
            NextButton.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
            CatalogList.Update();
            LastButton.Update();
            NextButton.Update();
            PageScope.Update();
        }
    }
}
