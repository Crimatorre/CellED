using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CellED.UI.SpriteTools
{
    public class CatalogListItem : ListItem
    {
        WorldObject worldObject;
        Texture2D objectTexture;
        string objectName;

        public CatalogListItem(List parent, Texture2D texture, string name) : base(parent, name)
        {
            objectTexture = texture;
            objectName = name;
        }

        public override void OnItemSelection(float x, float y)
        {
            base.OnItemSelection(x, y);
            if (worldObject == null)
            {
                LoadReferenceObject();
                Label = string.Format("*{0}", objectName);
            }
            parentList.parent.objectHandler.StartObjectAddition(new WorldObject(worldObject, new Vector2(x, y) - parentList.parent.camera.CurrentOffset));
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

        public void LoadReferenceObject()
        {
            worldObject = new WorldObject(parentList.parent.objectHandler, objectTexture, objectName);
            worldObject.DisconnectInput();
        }
    }
}
