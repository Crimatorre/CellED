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

        public void StartObjectAddition(WorldObject worldObj)
        {
            CurrentOperation = ObjectOperation.Placing;
            CurrentSelection = worldObj;
            parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
        }

        public void EndObjectAddition()
        {
            CurrentOperation = ObjectOperation.None;
            CurrentSelection = null;
            parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
        }

        private void OnMousePosChanged(float x, float y)
        {
            if (CurrentOperation == ObjectOperation.Placing)
            {
                CurrentSelection.Pos = new Vector2(x, y) - parent.camera.CurrentOffset;
            }
            
        }

        private void OnMouseLeftClick(float x, float y)
        {
            if (ViewPortContains(x, y))
            {
                if (CurrentOperation == ObjectOperation.Placing)
                {
                    CurrentSelection.Pos = new Vector2(x, y) - parent.camera.CurrentOffset;
                    WorldObjects.Add(CurrentSelection);
                    CurrentSelection = new WorldObject(CurrentSelection, CurrentSelection.Pos);
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
