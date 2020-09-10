using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.SpriteTools
{
    public class SpriteZValue : SideBarTool
    {
        private Slider zSlider;
        private Scope zScope;
        private WorldObject selectedObject;
        public SpriteZValue(SideBar sideBar) : base(sideBar, 50, "Sprite Z-Offset")
        {
            zSlider = new Slider(this, 150, 4, 1f, 0f, Pos + new Vector2(10, 25));
            MouseLeftPressed += zSlider.OnMouseLeftPressed;
            MouseLeftReleased += zSlider.OnMouseLeftReleased;
            MouseMoved += zSlider.OnMouseMoved;

            zScope = new Scope(this, 40, 19, Pos + new Vector2(170, 18));
            zScope.LabelText = string.Format("{0:F2}", zSlider.SliderMinValue);

            zSlider.ValueChanged += OnZValueChanged;
            parent.objectHandler.ObjectSelection += OnObjectSelection;
        }

        private void OnObjectSelection(ref WorldObject ob)
        {
            selectedObject = ob;
            if (ob == null)
            {
                zSlider.SliderValue = zSlider.SliderMinValue;
                zScope.LabelText = string.Format("{0:F2}", zSlider.SliderMinValue);
            }
            else
            {
                zSlider.SliderValue = ob.Z;
                zScope.LabelText = string.Format("{0:F2}", ob.Z);
            }
        }

        private void OnZValueChanged(float value)
        {
            if (selectedObject != null)
            {
                selectedObject.Z = value;
            }
            zScope.LabelText = string.Format("{0:F2}", value);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            zSlider.Draw(spriteBatch);
            zScope.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
            zSlider.Update();
            zScope.Update();
        }
    }
}
