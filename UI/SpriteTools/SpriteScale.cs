using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.SpriteTools
{
    public class SpriteScale : SideBarTool
    {
        private WorldObject selectedObject;

        public Slider ScaleSlider { get; private set; }

        public SpriteScale(SideBar sideBar) : base(sideBar, 50, "Sprite Scale")
        {
            selectedObject = null;

            ScaleSlider = new Slider(this, 150, 4, 2f, 0.1f, Pos + new Vector2(10, 25));
            MouseLeftPressed += ScaleSlider.OnMouseLeftPressed;
            MouseLeftReleased += ScaleSlider.OnMouseLeftReleased;
            MouseMoved += ScaleSlider.OnMouseMoved;
            ScaleSlider.ValueChanged += OnScaleValueChanged;
            parent.objectHandler.ObjectSelection += OnObjectSelection;
        }

        private void OnObjectSelection(ref WorldObject ob)
        {
            selectedObject = ob;
            if (ob == null) {
                ScaleSlider.SliderValue = ScaleSlider.SliderMinValue;
            }
            else
            {
                ScaleSlider.SliderValue = ob.Scale;
            }
        }

        private void OnScaleValueChanged(float value)
        {
            if (selectedObject != null)
            {
                selectedObject.Scale = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            ScaleSlider.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
            ScaleSlider.Update();
        }
    }
}
