using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.Core
{
    public class Tile
    {
        public enum State {Floor, Border}
        private enum NIndex {T, TR, R, BR, B, BL, L, TL, Count}
        private List<(int, int)> NCoords;

        public State currentState;

        private CellED _game;
        private Grid _parent;

        #region Properties

        public (int X, int Y) Pos { get; set; }


        #endregion

        public Tile(Grid parent, CellED game, (int, int) isoPos)
        {
            _parent = parent;
            _game = game;
            Pos = isoPos;
            currentState = State.Border;

            NCoords = new List<(int, int)>();
            NCoords.Add((1, -1));
            NCoords.Add((1, 0));
            NCoords.Add((1, 1));
            NCoords.Add((0, 1));
            NCoords.Add((-1, 1));
            NCoords.Add((-1, 0));
            NCoords.Add((-1, -1));
            NCoords.Add((0, -1));

            game.inputHandler.LeftClickEvent += OnLeftClick;
        }

        public void OnLeftClick(float x, float y)
        {
            Vector3 cameraPos = _game.camera.CameraPosition.Translation;

            (int X, int Y) isoXY = Vector2ToTuple(_parent.CartesianToIso(new Vector2(x - cameraPos.X, y - cameraPos.Y)));
            
            if (isoXY == Pos)
            {
                if (currentState == State.Border && Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                {
                    CreateNeighbours();
                    currentState = State.Floor;
                }
                else if (currentState == State.Border && Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                {
                    ChangeNeighborState();
                    if (_parent.RemoveTileAt(Pos))
                    {
                        _game.inputHandler.LeftClickEvent -= OnLeftClick;
                    }
                }
                else if (currentState == State.Floor)
                {
                    currentState = State.Border;
                }
            }
        }

        private void CreateNeighbours()
        {
            for (int i = 0; i < (int)NIndex.Count; i++)
            {
                (int X, int Y) nPos = (Pos.X + NCoords[i].Item1, Pos.Y + NCoords[i].Item2);
                if (!_parent.HasTileAtPos(nPos))
                {
                    Tile newTile = new Tile(_parent, _game, nPos);
                    _parent.AddTile(newTile);
                }
            }
        }

        private void ChangeNeighborState()
        {
            for (int i = 0; i < (int)NIndex.Count; i++)
            {
                (int X, int Y) nPos = (Pos.X + NCoords[i].Item1, Pos.Y + NCoords[i].Item2);
                if (_parent.HasTileAtPos(nPos))
                {
                    Tile tile = _parent.GetTileAtPos(nPos);
                    if (tile.currentState != State.Border)
                    {
                        tile.currentState = State.Border;
                    }
                }
            }
        }

        public (int X, int Y) Vector2ToTuple(Vector2 xy)
        {
            return ((int)xy.X, (int)xy.Y);
        }
    }
}
