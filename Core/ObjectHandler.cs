using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

        public List<WorldObject> CatalogObjects { get; set; }
        public List<WorldObject> WorldObjects { get; set; }
        public WorldObject CurrentSelection { get; set; }

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

        public void AddObjectToWorld(WorldObject worldObj)
        {
            parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
            CurrentSelection = worldObj;
            CurrentOperation = ObjectOperation.Placing;
        }

        public void ResetSelection()
        {
            if (CurrentOperation == ObjectOperation.Placing)
            {
                CurrentSelection = null;
                CurrentOperation = ObjectOperation.None;
            }
        }

        private void OnMousePosChanged(float x, float y)
        {
            if (CurrentOperation == ObjectOperation.Placing)
            {
                CurrentSelection.Pos = new Vector2(x, y);
            }
            
        }

        private void OnMouseLeftClick(float x, float y)
        {
            if (ViewPortContains(x, y))
            {
                if (CurrentOperation == ObjectOperation.Placing)
                {
                    CurrentSelection.Pos = new Vector2(x, y);
                    WorldObjects.Add(CurrentSelection);
                    CurrentSelection = null;
                    CurrentOperation = ObjectOperation.None;
                    parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldObject worldObj in WorldObjects)
            {
                worldObj.Draw(spriteBatch);
            }
            if (CurrentSelection != null)
            {
                CurrentSelection.Draw(spriteBatch);
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
