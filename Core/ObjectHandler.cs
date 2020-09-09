using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.Core
{
    public class ObjectHandler
    {
        public enum ObjectOperation
        {
            None,
            Placing,
            Editing
        }
        public CellED parent;
        public ObjectOperation CurrentOperation;
        public WorldObject currentSelection;

        public List<WorldObject> CatalogObjects { get; set; }
        public List<WorldObject> WorldObjects { get; set; }
        public WorldObject NewObject { get; set; }

        public delegate void MouseEvent(float x, float y);
        public delegate void ObjectEvent(ref WorldObject ob);
        public event MouseEvent MouseLeftPressed;
        public event ObjectEvent ObjectSelection;

        public ObjectHandler(CellED parent)
        {
            this.parent = parent;
            InitializeObjects();
            CurrentOperation = ObjectOperation.None;
            parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftClick;
        }

        private void InitializeObjects()
        {
            CatalogObjects = FileHandler.WorldObjectsFromTextures(this, parent.GraphicsDevice, "Content/Textures");
            WorldObjects = new List<WorldObject>();
        }

        public void StartObjectAddition(WorldObject worldObj)
        {
            CurrentOperation = ObjectOperation.Placing;
            currentSelection = null;
            NewObject = worldObj;
            parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
        }

        public void EndObjectAddition()
        {
            CurrentOperation = ObjectOperation.None;
            NewObject = null;
            parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
        }

        private void OnMousePosChanged(float x, float y)
        {
            if (CurrentOperation == ObjectOperation.Placing)
            {
                NewObject.Pos = new Vector2(x, y) - parent.camera.CurrentOffset;
            }
            
        }

        private void OnMouseLeftClick(float x, float y)
        {
            if (ViewPortContains(x, y))
            {
                if (CurrentOperation == ObjectOperation.Placing)
                {
                    NewObject.Pos = new Vector2(x, y) - parent.camera.CurrentOffset;
                    WorldObjects.Add(NewObject);
                    NewObject = new WorldObject(NewObject, NewObject.Pos);
                }
                if (CurrentOperation == ObjectOperation.None && !parent.grid.EditModeEnabled)
                {
                    currentSelection = null;
                    MouseLeftPressed?.Invoke(x, y);
                    ObjectSelection?.Invoke(ref currentSelection);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldObject worldObj in WorldObjects)
            {
                worldObj.Draw(spriteBatch);
            }
            if (NewObject != null)
            {
                NewObject.Draw(spriteBatch);
            }
        }

        private bool ViewPortContains(float x, float y)
        {
            if (x > 240 && x < parent.ScreenWidth - 240)
            {
                return true;
            }
            return false;
        }
    }
}
