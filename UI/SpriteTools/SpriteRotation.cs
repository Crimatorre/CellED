using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.SpriteTools
{
    class SpriteRotation : SideBarTool
    {
        private Slider rotSlider;
        private Scope rotScope;
        private WorldObject selectedObject;
        public SpriteRotation(SideBar sideBar) : base(sideBar, 50, "Sprite Rotation")
        {
            rotSlider = new Slider(this, 150, 4, (float)Math.PI, -(float)Math.PI, Pos + new Vector2(10, 25));
            MouseLeftPressed += rotSlider.OnMouseLeftPressed;
            MouseLeftReleased += rotSlider.OnMouseLeftReleased;
            MouseMoved += rotSlider.OnMouseMoved;

            rotScope = new Scope(this, 40, 19, Pos + new Vector2(170, 18));
            rotScope.LabelText = string.Format("{0:F2}", rotSlider.SliderMinValue);

            rotSlider.ValueChanged += OnZValueChanged;
            parent.objectHandler.ObjectSelection += OnObjectSelection;
        }

        private void OnObjectSelection(ref WorldObject ob)
        {
            selectedObject = ob;
            if (ob == null)
            {
                rotSlider.SliderValue = rotSlider.SliderMinValue;
                rotScope.LabelText = string.Format("{0:F2}", rotSlider.SliderMinValue);
            }
            else
            {
                rotSlider.SliderValue = ob.Rotation;
                rotScope.LabelText = string.Format("{0:F2}", ob.Rotation);
            }
        }

        private void OnZValueChanged(float value)
        {
            if (selectedObject != null)
            {
                selectedObject.Rotation = value;
            }
            rotScope.LabelText = string.Format("{0:F2}", value);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            rotSlider.Draw(spriteBatch);
            rotScope.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
            rotSlider.Update();
            rotScope.Update();
        }
    }
}
