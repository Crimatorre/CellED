using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
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
        public bool inputDisabled;

        private Vector2 mousePadding;

        public List<WorldObject> CatalogObjects { get; set; }
        public List<WorldObject> WorldObjects { get; set; }

        public WorldObject newObject;

        public delegate void MouseEvent(float x, float y);
        public delegate void ObjectEvent(ref WorldObject ob);
        public delegate void UIEvent(bool newState);
        public event MouseEvent MouseLeftPressed;
        public event ObjectEvent ObjectSelection;
        public event UIEvent InputStateChanged;

        public ObjectHandler(CellED parent)
        {
            this.parent = parent;
            InitializeObjects();
            CurrentOperation = ObjectOperation.None;
            EnableInput();
        }

        public void EnableInput()
        {
            parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftClick;
            parent.inputHandler.KeyTappedEvent += OnKeyPress;
            inputDisabled = false;
        }

        public void DisableInput()
        {
            parent.inputHandler.MouseLeftPressedEvent -= OnMouseLeftClick;
            parent.inputHandler.KeyTappedEvent -= OnKeyPress;
            inputDisabled = true;
        }

        private void OnKeyPress(Keys key)
        {
            if (key == Keys.G)
            {
                MoveSelectedItem();
            }
            if (key == Keys.Escape)
            {
                if (CurrentOperation == ObjectOperation.None && currentSelection != null)
                {
                    currentSelection = null;
                }
            }
            if (key == Keys.Delete)
            {
                if (CurrentOperation == ObjectOperation.None && currentSelection != null)
                {
                    Debug.WriteLine(WorldObjects.Remove(currentSelection));
                    currentSelection.Destroy();
                    currentSelection = null;
                    InputStateChanged?.Invoke(true);
                    ObjectSelection?.Invoke(ref currentSelection);
                }
            }
        }

        private void MoveSelectedItem()
        {
            if (CurrentOperation == ObjectOperation.None && currentSelection != null)
            {
                CurrentOperation = ObjectOperation.Placing;
                mousePadding = (currentSelection.Pos + parent.camera.CurrentOffset) - parent.inputHandler.GetMousePos();
                parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
                InputStateChanged?.Invoke(false);
            }
            else if (CurrentOperation == ObjectOperation.Placing && currentSelection != null)
            {
                currentSelection.Pos = parent.inputHandler.GetMousePos() + mousePadding - parent.camera.CurrentOffset;
                parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
                CurrentOperation = ObjectOperation.None;
                InputStateChanged?.Invoke(true);
            }
        }

        private void InitializeObjects()
        {
            CatalogObjects = FileHandler.WorldObjectsFromTextures(this, parent.GraphicsDevice, "Content/Textures");
            WorldObjects = new List<WorldObject>();
        }

        public void StartObjectAddition(WorldObject worldObj)
        {
            if (!inputDisabled)
            {
                CurrentOperation = ObjectOperation.Placing;
                currentSelection = null;
                newObject = worldObj;
                parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
                parent.grid.EditModeEnabled = false;
                InputStateChanged?.Invoke(false);
            }
        }

        public void EndObjectAddition()
        {
            if (!inputDisabled)
            {
                CurrentOperation = ObjectOperation.None;
                newObject.Destroy();
                newObject = null;
                parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
                InputStateChanged?.Invoke(true);
            }
        }

        private void OnMousePosChanged(float x, float y)
        {
            if (CurrentOperation == ObjectOperation.Placing && newObject != null)
            {
                newObject.Pos = new Vector2(x, y) - parent.camera.CurrentOffset;
            }
            else if (CurrentOperation == ObjectOperation.Placing && currentSelection != null)
            {
                currentSelection.Pos = new Vector2(x, y) + mousePadding - parent.camera.CurrentOffset;
            }
            
        }

        private void OnMouseLeftClick(float x, float y)
        {
            if (ViewPortContains(x, y))
            {
                if (CurrentOperation == ObjectOperation.Placing && newObject != null)
                {
                    newObject.Pos = new Vector2(x, y) - parent.camera.CurrentOffset;
                    WorldObjects.Add(newObject);
                    newObject = new WorldObject(newObject, newObject.Pos);
                }
                if (CurrentOperation == ObjectOperation.None && !parent.grid.EditModeEnabled)
                {
                    currentSelection = null;
                    MouseLeftPressed?.Invoke(x, y);
                    ObjectSelection?.Invoke(ref currentSelection);
                    if (currentSelection != null)
                    {
                        InputStateChanged?.Invoke(false);
                    }
                    else
                    {
                        InputStateChanged?.Invoke(true);
                    }
                }
                if (CurrentOperation == ObjectOperation.Placing && currentSelection != null)
                {
                    currentSelection.Pos = new Vector2(x, y) + mousePadding - parent.camera.CurrentOffset;
                    parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
                    CurrentOperation = ObjectOperation.None;
                    InputStateChanged?.Invoke(true);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldObject worldObj in WorldObjects)
            {
                worldObj.Draw(spriteBatch);
            }
            if (newObject != null)
            {
                newObject.Draw(spriteBatch);
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
