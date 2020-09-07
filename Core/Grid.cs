using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CellED.Core
{
    public class Grid
    {
        private float zValue;
        private bool showOnTop;
        public enum TileOperation
        {
            None,
            Adding,
            Removing
        }

        private Texture2D _tileTexture;
        private Texture2D _tileFilledTexture;
        private Texture2D _tileDottedTexture;
        private Dictionary<(int, int),Tile> _tileList;
        public CellED parent;

        public Vector2 tileSize;
        public Vector2 screenOffset;

        public Vector2 Size { get; set; }
        public TileOperation CurrentOperation { get; set; }
        public bool EditModeEnabled { get; set; }
        public bool GridEnabled { get; set; }
        public bool ShowOnTop
        {
            get
            {
                return showOnTop;
            }
            set
            {
                showOnTop = value;
                zValue = showOnTop == true ? 0f : 1f;
            }
        }
        
        public Grid(CellED game)
        {
            //Size = new Vector2(xSize, ySize);
            _tileTexture = game.Content.Load<Texture2D>("Textures/tile");
            _tileFilledTexture = game.Content.Load<Texture2D>("Textures/tileFilled");
            _tileDottedTexture = game.Content.Load<Texture2D>("Textures/tileDotted");
            parent = game;

            CurrentOperation = TileOperation.None;
            GridEnabled = true;
            EditModeEnabled = true;
            ShowOnTop = false;

            screenOffset = new Vector2(game.ScreenWidth/2, game.ScreenHeight/2);
            tileSize = new Vector2(48, 24);

            InitializeGrid();
            parent.inputHandler.MouseLeftPressedEvent += OnMouseLeftPressed;
            parent.inputHandler.MouseLeftReleasedEvent += OnMouseLeftReleased;
        }

        private void OnMouseLeftReleased(float x, float y)
        {
            if (EditModeEnabled && GridEnabled)
            {
                parent.inputHandler.MouseMovedEvent -= OnMousePosChanged;
            }
        }

        private void OnMouseLeftPressed(float x, float y)
        {
            if (ViewPortContains(x, y))
            {
                if (EditModeEnabled && GridEnabled)
                {
                    parent.inputHandler.MouseMovedEvent += OnMousePosChanged;
                    ChangeTileState(x, y);
                }
            }
        }

        private void OnMousePosChanged(float x, float y)
        {
            ChangeTileState(x, y);
        }

        private void InitializeGrid()
        {
            _tileList = new Dictionary<(int, int), Tile>();
            Tile centerTile = new Tile(this, parent, (0, 0));
            _tileList.Add((0, 0), centerTile);
        }

        public void AddTile(Tile tile)
        {
            _tileList.Add(tile.Pos, tile);
        }

        public bool HasTileAtPos((int, int) pos)
        {
            if (_tileList.ContainsKey(pos))
            {
                return true;
            }
            return false;
        }

        public Tile GetTileAtPos((int, int) pos)
        {
            return _tileList[pos];
        }

        public bool RemoveTileAt((int, int) pos)
        {
            if (_tileList.Count > 1)
            {
                _tileList.Remove(pos);
                return true;
            }
            return false;
        }

        public void DisableInput()
        {
            foreach (KeyValuePair<(int, int), Tile> pair in _tileList)
            {
                parent.inputHandler.LeftClickEvent -= pair.Value.OnLeftClick;
            }
        }

        public void EnableInput()
        {
            foreach (KeyValuePair<(int, int), Tile> pair in _tileList)
            {
                parent.inputHandler.LeftClickEvent += pair.Value.OnLeftClick;
            }
        }

        public void Update()
        {
            //offset += Vector2.One * 3;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GridEnabled)
            {
                foreach (KeyValuePair<(int, int), Tile> pair in _tileList)
                {
                    Tile tile = pair.Value;
                    Vector2 tilePos = IsoToCartesian(new Vector2(tile.Pos.X, tile.Pos.Y));
                    Texture2D tileTexture = _tileTexture;
                    if (tile.currentState == Tile.State.Border)
                    {
                        tileTexture = _tileDottedTexture;
                    }
                    spriteBatch.Draw(tileTexture, tilePos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, zValue);
                }
            }
        }

        public Vector2 CartesianToIso(Vector2 cartPos)
        {
            float x = cartPos.X;
            float y = cartPos.Y - tileSize.Y/2;
            float isoX = (x - 2 * y) / tileSize.X;
            float isoY = (2 * x / tileSize.X) - isoX;

            Vector2 result = new Vector2(isoX, isoY);
            result.Floor();

            return result;
        }

        public Vector2 IsoToCartesian(Vector2 isoPos)
        {
            float x = isoPos.X;
            float y = isoPos.Y;
            Vector2 cartPos = new Vector2((float)Math.Floor((x + y) * tileSize.X / 2), (float)Math.Floor((y - x) * tileSize.Y / 2));
            return cartPos;
        }

        public (int X, int Y) Vector2ToTuple(Vector2 xy)
        {
            return ((int)xy.X, (int)xy.Y);
        }

        private void ChangeTileState(float x, float y)
        {
            Vector3 cameraPos = parent.camera.CameraPosition.Translation;

            (int X, int Y) isoXY = Vector2ToTuple(CartesianToIso(new Vector2(x - cameraPos.X, y - cameraPos.Y)));

            if (_tileList.ContainsKey(isoXY))
            {
                if (_tileList[isoXY].currentState == Tile.State.Border && Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                {
                    _tileList[isoXY].CreateNeighbours();
                }
                else if (_tileList[isoXY].currentState == Tile.State.Border && Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                {
                    _tileList[isoXY].ChangeNeighborState();
                }
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
