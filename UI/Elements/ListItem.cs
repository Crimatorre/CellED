using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.Elements
{
    public class ListItem
    {
        public enum ItemState
        {
            None,
            Hovered,
            Selected
        }

        private int slot;
        protected List parentList;


        public int Slot
        {
            get
            {
                return slot;
            }

            set
            {
                if (value == -1)
                {
                    DisconnectInput();
                }
                else
                {
                    ConnectInput();
                }
                slot = value;
                UpdatePos();
            }
        }
        public string Label { get; set; }
        public Vector2 ItemSize { get; private set; }
        public Vector2 Pos { get; private set; }
        public ItemState State { get; set; }

        public ListItem(List parent, string label)
        {
            parentList = parent;
            ItemSize = parent.ItemSize;
            Label = label;
            Pos = parent.Pos;
            Slot = -1;
            State = ItemState.None;
        }

        private void ConnectInput()
        {
            parentList.MouseLeftClickEvent += OnMouseLeftClick;
            parentList.MousePosChanged += OnMousePosChanged;
        }

        private void DisconnectInput()
        {
            parentList.MouseLeftClickEvent -= OnMouseLeftClick;
            parentList.MousePosChanged -= OnMousePosChanged;
        }

        private void UpdatePos()
        {
            Pos = parentList.Pos + Vector2.One + new Vector2(0, Slot * (ItemSize.Y + 1));
        }

        public virtual void OnMouseLeftClick(float x, float y)
        {
            if (Contains(x, y))
            {
                if (State == ItemState.Selected)
                {
                    parentList.CurrentSelection = null;
                    State = ItemState.Hovered;
                }
                else
                {
                    if (parentList.CurrentSelection != null)
                    {
                        parentList.CurrentSelection.State = ItemState.None;
                    }
                    
                    State = ItemState.Selected;
                    parentList.CurrentSelection = this;
                }
            }
            else
            {
                State = ItemState.None;
            }
        }

        private void OnMousePosChanged(float x, float y)
        {
            if (State != ItemState.Selected)
            {
                if (Contains(x, y))
                {
                    State = ItemState.Hovered;
                }
                else
                {
                    State = ItemState.None;
                }
            }
        }

        protected bool Contains(float x, float y)
        {
            if (x > Pos.X && x < Pos.X + ItemSize.X &&
                y > Pos.Y - 1 && y < Pos.Y + (ItemSize.Y + 1))
            {
                return true;
            }
            return false;
        }
    }
}
