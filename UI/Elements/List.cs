using CellED.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;

namespace CellED.UI.Elements
{
    public class List : UIObjectBase
    {
        private Texture2D ItemTexture;
        private List<Color> ItemColors;
        private UIObjectBase parentObject;
        private bool isMouseInside;
        private Vector2 itemTextOffset;
        private Color itemBaseColor;
        private bool isScrollable;
        private LinkedList<ListItem> currentScrollItems;
        private int firstScrollIndex;
        private int lastScrollIndex;

        public List<ListItem> ListItems { get; private set; }
        public List<ListItem> CurrentListItems { get; private set; }
        public Vector2 ItemSize { get; private set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; private set; }
        public int MaxPages { get; private set; }
        public ListItem CurrentSelection { get; set; }

        public delegate void MouseEvent(float x, float y);
        public delegate void PageEvent(int page);
        public event MouseEvent MouseLeftClickEvent;
        public event MouseEvent MousePosChanged;
        public event PageEvent PageChanged;

        public List(UIObjectBase parent, int width, int itemHeight, int itemsPerPage, Vector2 pos, Color? itemBaseColor = null, bool isScrollable = false)
            : base(parent.parent, width, (itemHeight + 1) * itemsPerPage + 1, pos, Parameters.Filled, new Color(70, 70, 70), null, 0)
        {
            ItemSize = new Vector2(width - 2, itemHeight);
            ListItems = new List<ListItem>();
            CurrentListItems = new List<ListItem>();
            parentObject = parent;
            itemTextOffset = new Vector2(3, ItemSize.Y / 2 - (int)this.parent.UIFontSmall.MeasureString("Item").Y / 2);

            CurrentPage = 1;
            ItemsPerPage = itemsPerPage;
            isMouseInside = false;
            this.itemBaseColor = itemBaseColor ?? parent.parent.BaseColor;
            this.isScrollable = isScrollable;

            ConnectInput();
            CreateItemTextures();
        }

        public void ConnectInput()
        {
            parent.inputHandler.MouseLeftPressedEvent += OnMouseClick;
            parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
        }

        public void DisconnectInput()
        {
            parent.inputHandler.MouseLeftPressedEvent -= OnMouseClick;
            parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
        }

        private void OnMousePosChanged(float x, float y)
        {
            if (Contains(x, y))
            {
                MousePosChanged?.Invoke(x, y);
                isMouseInside = true;
            }
            else if (isMouseInside == true)
            {
                MousePosChanged?.Invoke(x, y);
                isMouseInside = false;
            }
        }

        private void OnMouseClick(float x, float y)
        {
            if (Contains(x, y))
            {
                MouseLeftClickEvent?.Invoke(x, y);
            }
        }

        private bool Contains(float x, float y)
        {
            if (x > Pos.X && x < Pos.X + Width &&
                y > Pos.Y && y < Pos.Y + Height)
            {
                return true;
            }
            return false;
        }

        private void SetCurrentPage(int pageIndex)
        {
            if (pageIndex > 0 && pageIndex <= MaxPages )
            {
                CurrentPage = pageIndex;

                for (int i = 0; i < CurrentListItems.Count; i++)
                {
                    CurrentListItems[i].Slot = -1;
                }

                CurrentListItems.Clear();

                int startIndex = ItemsPerPage * (CurrentPage - 1);
                int endIndex = startIndex + ItemsPerPage > ListItems.Count ? ListItems.Count : startIndex + ItemsPerPage;
                int count = 0;

                for (int i = startIndex; i < endIndex; i++)
                {
                    ListItem item = ListItems[i];
                    item.Slot = count;
                    count++;
                    CurrentListItems.Add(item);
                }
            }
        }

        public void NextPage()
        {
            SetCurrentPage(CurrentPage + 1);
            PageChanged?.Invoke(CurrentPage);
        }

        public void LastPage()
        {
            SetCurrentPage(CurrentPage - 1);
            PageChanged?.Invoke(CurrentPage);
        }

        public void SetItemList(List<ListItem> itemList)
        {
            if (!isScrollable)
            {
                ListItems = itemList;
                MaxPages = (int)Math.Ceiling((float)ListItems.Count / (float)ItemsPerPage);
                SetCurrentPage(1);
            }
            else
            {
                ListItems = itemList;
                SetupScrollableList();
            }
        }

        private void SetupScrollableList()
        {
            foreach (ListItem item in ListItems)
            {
                item.Slot = -1;
            }

            firstScrollIndex = 0;
            lastScrollIndex = ListItems.Count < ItemsPerPage ? ListItems.Count : ItemsPerPage - 1;
            currentScrollItems = new LinkedList<ListItem>();

            for (int i = firstScrollIndex; i <= lastScrollIndex; i++)
            {
                currentScrollItems.AddLast(ListItems[i]);
                currentScrollItems.Last.Value.Slot = i;
            }

            parent.inputHandler.MouseScrollUp += OnScrollUp;
            parent.inputHandler.MouseScrollDown += OnScrollDown;
        }

        private void OnScrollUp(float x, float y)
        {
            if (Contains(x, y))
            {
                if (firstScrollIndex > 0)
                {
                    firstScrollIndex--;
                    lastScrollIndex--;
                    currentScrollItems.AddFirst(ListItems[firstScrollIndex]);
                    int counter = 0;
                    bool hoverCorrected = false;

                    foreach (ListItem item in currentScrollItems)
                    {
                        item.Slot = counter;
                        counter++;
                    }

                    currentScrollItems.Last.Value.Slot = -1;
                    currentScrollItems.RemoveLast();
                }
            }
        }

        private void OnScrollDown(float x, float y)
        {
            if (Contains(x, y))
            {
                if (lastScrollIndex < ListItems.Count - 1)
                {
                    firstScrollIndex++;
                    lastScrollIndex++;
                    currentScrollItems.AddLast(ListItems[lastScrollIndex]);
                    int counter = -1;
                    bool hoverCorrected = false;

                    foreach (ListItem item in currentScrollItems)
                    {
                        item.Slot = counter;
                        counter++;
                    }

                    currentScrollItems.RemoveFirst();
                }
            }
        }

        private void CreateItemTextures()
        {
            ItemColors = new List<Color>(3);
            ItemColors.Add(itemBaseColor); // base color (grey by default)
            ItemColors.Add(parent.HoverColor); // list cyan = hovered
            ItemColors.Add(parent.SelectionColor); // cyan = selected

            int x = (int)ItemSize.X;
            int y = (int)ItemSize.Y;

            ItemTexture = new Texture2D(parent.GraphicsDevice, x, y);
            ItemTexture.SetData(Utilities.CreateRectangleTexture(x * y));

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!isScrollable)
            {
                foreach (ListItem item in CurrentListItems)
                {
                    spriteBatch.Draw(ItemTexture, item.Pos, null, ItemColors[(int)item.State], 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(parent.UIFontSmall, item.Label, item.Pos + itemTextOffset, parent.TextColor);
                }
            }
            else
            {
                foreach (ListItem item in currentScrollItems)
                {
                    spriteBatch.Draw(ItemTexture, item.Pos, null, ItemColors[(int)item.State], 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(parent.UIFontSmall, item.Label, item.Pos + itemTextOffset, parent.TextColor);
                }
            }
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
