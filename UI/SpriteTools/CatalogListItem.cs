using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.SpriteTools
{
    public class CatalogListItem : ListItem
    {
        WorldObject WorldObj { get; set; }
        public CatalogListItem(List parent, WorldObject worldObj) : base(parent, worldObj.Name)
        {
            WorldObj = worldObj;
        }

        public override void OnMouseLeftClick(float x, float y)
        {
            if (Contains(x, y))
            {
                if (parentList.parent.objectHandler.CurrentSelection != null)
                {
                    Debug.WriteLine(parentList.parent.objectHandler.CurrentSelection.Name == WorldObj.Name);
                    if (parentList.parent.objectHandler.CurrentSelection.Name == WorldObj.Name)
                    {
                        if (State == ItemState.Selected)
                        {
                            parentList.parent.objectHandler.ResetSelection();
                        }
                    }
                    else
                    {
                        parentList.parent.objectHandler.AddObjectToWorld(new WorldObject(WorldObj, new Vector2(x, y)));
                    }
                }
                else
                {
                    parentList.parent.objectHandler.AddObjectToWorld(new WorldObject(WorldObj, new Vector2(x, y)));
                }
            }
            base.OnMouseLeftClick(x, y);
        }
    }
}
