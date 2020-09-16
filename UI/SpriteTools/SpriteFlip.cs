using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.SpriteTools
{
    public class SpriteFlip : SideBarTool
    {
        private WorldObject selectedObject;

        public StatedButton VerticalFlipButton { get; set; }
        public StatedButton HorizontalFlipButton { get; set; }

        public SpriteFlip(SideBar sideBar) : base(sideBar, 50, "Sprite Flip")
        {
            VerticalFlipButton = new StatedButton(this, (Width - 20) / 2, 28, Pos + new Vector2(10, 10));
            VerticalFlipButton.ButtonText = "Vertical";
            VerticalFlipButton.ButtonStateChanged += OnVerticalStateChenged;

            HorizontalFlipButton = new StatedButton(this, VerticalFlipButton.Width, VerticalFlipButton.Height, VerticalFlipButton.Pos + new Vector2(VerticalFlipButton.Width, 0));
            HorizontalFlipButton.ButtonText = "Horizontal";
            HorizontalFlipButton.ButtonStateChanged += OnHorizontalStateChanged;

            parent.objectHandler.ObjectSelection += OnObjectSelection;
        }

        private void OnObjectSelection(ref WorldObject ob)
        {
            selectedObject = ob;
            if (ob == null)
            {
                VerticalFlipButton.Pressed = false;
                HorizontalFlipButton.Pressed = false;
            }
            else
            {
                VerticalFlipButton.Pressed = ob.Effects.HasFlag(SpriteEffects.FlipVertically);
                HorizontalFlipButton.Pressed = ob.Effects.HasFlag(SpriteEffects.FlipHorizontally);
            }
        }

        private void OnHorizontalStateChanged(bool state)
        {
            if (selectedObject != null)
            {
                if (state)
                {
                    selectedObject.Effects |= SpriteEffects.FlipHorizontally;
                }
                else
                {
                    selectedObject.Effects &= ~SpriteEffects.FlipHorizontally;
                }
            }
        }

        private void OnVerticalStateChenged(bool state)
        {
            if (selectedObject != null)
            {
                if (state)
                {
                    selectedObject.Effects |= SpriteEffects.FlipVertically;
                }
                else
                {
                    selectedObject.Effects &= ~SpriteEffects.FlipVertically;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            VerticalFlipButton.Draw(spriteBatch);
            HorizontalFlipButton.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
            VerticalFlipButton.Update();
            HorizontalFlipButton.Update();
        }
    }
}
