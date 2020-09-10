using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
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
            base.OnItemSelection(x, y);
            parentList.parent.objectHandler.StartObjectAddition(new WorldObject(WorldObj, new Vector2(x, y) - parentList.parent.camera.CurrentOffset));
        }

        public override void OnItemDiselection()
        {
            base.OnItemDiselection();
            parentList.parent.objectHandler.EndObjectAddition();
        }

        protected override void ConnectInput()
        {
            base.ConnectInput();
            parentList.parent.inputHandler.KeyTappedEvent += OnKeyPress;
        }

        protected override void DisconnectInput()
        {
            base.DisconnectInput();
            parentList.parent.inputHandler.KeyTappedEvent -= OnKeyPress;
        }

        private void OnKeyPress(Keys key)
        {
            if (key == Keys.Escape)
            {
                if (State == ItemState.Selected)
                {
                    OnItemDiselection();
                }
            }
        }
    }
}
