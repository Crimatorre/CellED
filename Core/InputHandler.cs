﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.Core
{
    public class InputHandler
    {
        public MouseState lastMouseState;
        public KeyboardState lastKeyboardState;

        public delegate void MouseHandler(float x, float y);
        public delegate void KeyTapped(Keys key);

        public event MouseHandler LeftClickEvent;
        public event MouseHandler MouseLeftPressedEvent;
        public event MouseHandler MouseLeftReleasedEvent;
        public event MouseHandler MouseShiftLeftPressedEvent;
        public event MouseHandler MouseShiftLeftReleasedEvent;
        public event MouseHandler MouseMovedEvent;

        public event KeyTapped KeyTappedEvent;


        public InputHandler(CellED game)
        {
            lastMouseState = Mouse.GetState();
            lastKeyboardState = Keyboard.GetState();
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            // MouseClick / MousePress handling
            if (mouseState.LeftButton == ButtonState.Pressed &&
                lastMouseState.LeftButton == ButtonState.Released &&
                keyboardState.IsKeyUp(Keys.LeftShift))
            {
                LeftClickEvent?.Invoke(mouseState.X, mouseState.Y);
                MouseLeftPressedEvent?.Invoke(mouseState.X, mouseState.Y);
            }
            if (lastMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released)
            {
                MouseLeftReleasedEvent?.Invoke(mouseState.X, mouseState.Y);
            }

            // Shift + MouseLeftPress handling
            if (mouseState.LeftButton == ButtonState.Pressed &&
                lastMouseState.LeftButton == ButtonState.Released &&
                keyboardState.IsKeyDown(Keys.LeftShift))
            {
                MouseShiftLeftPressedEvent?.Invoke(mouseState.X, mouseState.Y);
            }
            if ((lastMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released) ||
                keyboardState.IsKeyUp(Keys.LeftShift))
            {
                MouseShiftLeftReleasedEvent?.Invoke(mouseState.X, mouseState.Y);
            }

            // MouseMoved handling
            if (lastMouseState.X != mouseState.X ||
                lastMouseState.Y != mouseState.Y)
            {
                MouseMovedEvent?.Invoke(mouseState.X, mouseState.Y);
            }

            // G-key tapping
            if (IsKeyTapped(keyboardState, Keys.G))
            {
                KeyTappedEvent?.Invoke(Keys.G);
            }

            // ESC-key tapping
            if (IsKeyTapped(keyboardState, Keys.Escape))
            {
                KeyTappedEvent?.Invoke(Keys.Escape);
            }

            lastMouseState = mouseState;
            lastKeyboardState = keyboardState;
        }

        public Vector2 GetMousePos()
        {
            return new Vector2(lastMouseState.X, lastMouseState.Y);
        }

        private bool IsKeyTapped(KeyboardState keyboardState, Keys key)
        {
            if (lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }


    }
}
