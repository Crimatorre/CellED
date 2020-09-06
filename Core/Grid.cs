using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CellED.Core
{
    public class Grid
    {
        private Texture2D _tileTexture;
        private Texture2D _tileFilledTexture;
        private Texture2D _tileDottedTexture;
        private Dictionary<(int, int),Tile> _tileList;
        private CellED _game;

        public Vector2 tileSize;

        public Vector2 Size { get; set; }
        public Vector2 screenOffset;
        public Grid(CellED game)
        {
            //Size = new Vector2(xSize, ySize);
            _tileTexture = game.Content.Load<Texture2D>("Textures/tile");
            _tileFilledTexture = game.Content.Load<Texture2D>("Textures/tileFilled");
            _tileDottedTexture = game.Content.Load<Texture2D>("Textures/tileDotted");
            _game = game;

            screenOffset = new Vector2(game.ScreenWidth/2, game.ScreenHeight/2);
            tileSize = new Vector2(48, 24);

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _tileList = new Dictionary<(int, int), Tile>();

            // creating empty tile in the center of a screen
            /*for (int x = -9; x < 10; x ++)
            {
                for (int y = -9; y < 10; y++)
                {
                    _tileList.Add(new Tile(this, _game, new Vector2(x, y)));
                }
            }*/
            Tile centerTile = new Tile(this, _game, (0, 0));
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
                _game.inputHandler.LeftClickEvent -= pair.Value.OnLeftClick;
            }
        }

        public void EnableInput()
        {
            foreach (KeyValuePair<(int, int), Tile> pair in _tileList)
            {
                _game.inputHandler.LeftClickEvent += pair.Value.OnLeftClick;
            }
        }

        public void Update()
        {
            //offset += Vector2.One * 3;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_game.State.HasFlag(CellED.ProgramState.GridEnabled))
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
                    spriteBatch.Draw(tileTexture, tilePos, Color.White);
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
    }
}
