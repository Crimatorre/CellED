using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.Core
{
    public class Camera
    {
        private CellED _game;
        private Vector2 lastMousePos;

        public Matrix CameraPosition { get; set; }
        public Vector2 CurrentOffset { get; private set; }
        public Camera(CellED game)
        {
            _game = game;
            CurrentOffset = new Vector2(game.ScreenWidth / 2, game.ScreenHeight / 2);
            CameraPosition = Matrix.CreateTranslation(CurrentOffset.X, CurrentOffset.Y, 0f);
            game.inputHandler.MouseShiftLeftPressedEvent += OnMousePress;
            game.inputHandler.MouseShiftLeftReleasedEvent += OnMouseRelease;
        }

        public void ResetCamera()
        {
            CurrentOffset = new Vector2(_game.ScreenWidth / 2, _game.ScreenHeight / 2);
            CameraPosition = Matrix.CreateTranslation(CurrentOffset.X, CurrentOffset.Y, 0f);
        }

        private void OnMousePress(float x, float y)
        {
            lastMousePos = new Vector2(x, y);
            _game.inputHandler.MouseMovedEvent += OnMouseMoved;
        }

        private void OnMouseRelease(float x, float y)
        {
            _game.inputHandler.MouseMovedEvent -= OnMouseMoved;
        }

        private void OnMouseMoved(float x, float y)
        {
            Vector2 newMousePos = new Vector2(x, y);
            CurrentOffset += newMousePos - lastMousePos;
            lastMousePos = newMousePos;
            CameraPosition = Matrix.CreateTranslation(CurrentOffset.X, CurrentOffset.Y, 0f);
        }
    }
}
