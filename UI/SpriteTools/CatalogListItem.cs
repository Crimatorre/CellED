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

        public override void OnItemSelection(float x, float y)
        {
            parentList.parent.objectHandler.StartObjectAddition(new WorldObject(WorldObj, new Vector2(x, y) - parentList.parent.camera.CurrentOffset));
        }

        public override void OnItemDiselection()
        {
            parentList.parent.objectHandler.EndObjectAddition();
        }
    }
}
