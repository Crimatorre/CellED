using CellED.Core;
using CellED.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.SpriteTools
{
    public class SpriteColorTint : SideBarTool
    {
        private Slider rSlider;
        private Slider gSlider;
        private Slider bSlider;
        private Slider aSlider;

        private readonly string rText = "R:";
        private readonly string gText = "G:";
        private readonly string bText = "B:";
        private readonly string aText = "A:";

        private readonly Vector2 rTextOffset;
        private readonly Vector2 gTextOffset;
        private readonly Vector2 bTextOffset;
        private readonly Vector2 aTextOffset;

        private WorldObject selectedObject;

        public SpriteColorTint(SideBar sideBar) : base(sideBar, 150, "Sprite Color Tint")
        {
            rSlider = new Slider(this, 175, 4, 255f, 0f, Pos + new Vector2(35, 25));
            MouseLeftPressed += rSlider.OnMouseLeftPressed;
            MouseLeftReleased += rSlider.OnMouseLeftReleased;
            MouseMoved += rSlider.OnMouseMoved;

            gSlider = new Slider(this, 175, 4, 255f, 0f, Pos + new Vector2(35, 55));
            MouseLeftPressed += gSlider.OnMouseLeftPressed;
            MouseLeftReleased += gSlider.OnMouseLeftReleased;
            MouseMoved += gSlider.OnMouseMoved;

            bSlider = new Slider(this, 175, 4, 255f, 0f, Pos + new Vector2(35, 85));
            MouseLeftPressed += bSlider.OnMouseLeftPressed;
            MouseLeftReleased += bSlider.OnMouseLeftReleased;
            MouseMoved += bSlider.OnMouseMoved;

            aSlider = new Slider(this, 175, 4, 255f, 0f, Pos + new Vector2(35, 115));
            MouseLeftPressed += aSlider.OnMouseLeftPressed;
            MouseLeftReleased += aSlider.OnMouseLeftReleased;
            MouseMoved += aSlider.OnMouseMoved;

            rTextOffset = rSlider.Pos - new Vector2(20, 6);
            gTextOffset = gSlider.Pos - new Vector2(20, 6);
            bTextOffset = bSlider.Pos - new Vector2(20, 6);
            aTextOffset = aSlider.Pos - new Vector2(20, 6);

            rSlider.ValueChanged += OnRValueChanged;
            gSlider.ValueChanged += OnGValueChanged;
            bSlider.ValueChanged += OnBValueChanged;
            aSlider.ValueChanged += OnAValueChanged;

            parent.objectHandler.ObjectSelection += OnObjectSelection;
        }

        private void OnObjectSelection(ref WorldObject ob)
        {
            selectedObject = ob;
            if (ob == null)
            {
                rSlider.SliderValue = rSlider.SliderMinValue;
                gSlider.SliderValue = gSlider.SliderMinValue;
                bSlider.SliderValue = bSlider.SliderMinValue;
                aSlider.SliderValue = aSlider.SliderMinValue;
            }
            else
            {
                rSlider.SliderValue = ob.Color.R;
                gSlider.SliderValue = ob.Color.G;
                bSlider.SliderValue = ob.Color.B;
                aSlider.SliderValue = ob.Color.A;
            }
        }

        private void OnRValueChanged(float value)
        {
            if (selectedObject != null)
            {
                Color oldColor = selectedObject.Color;
                selectedObject.Color = new Color((int)value, oldColor.G, oldColor.B, oldColor.A);
            }
        }

        private void OnGValueChanged(float value)
        {
            if (selectedObject != null)
            {
                Color oldColor = selectedObject.Color;
                selectedObject.Color = new Color(oldColor.R, (int)value, oldColor.B, oldColor.A);
            }
        }

        private void OnBValueChanged(float value)
        {
            if (selectedObject != null)
            {
                Color oldColor = selectedObject.Color;
                selectedObject.Color = new Color(oldColor.R, oldColor.G, (int)value, oldColor.A);
            }
        }

        private void OnAValueChanged(float value)
        {
            if (selectedObject != null)
            {
                Color oldColor = selectedObject.Color;
                selectedObject.Color = new Color(oldColor.R, oldColor.G, oldColor.B, (int)value);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            rSlider.Draw(spriteBatch);
            gSlider.Draw(spriteBatch);
            bSlider.Draw(spriteBatch);
            aSlider.Draw(spriteBatch);

            spriteBatch.DrawString(parent.UIFontSmall, rText, rTextOffset, parent.TextColor);
            spriteBatch.DrawString(parent.UIFontSmall, gText, gTextOffset, parent.TextColor);
            spriteBatch.DrawString(parent.UIFontSmall, bText, bTextOffset, parent.TextColor);
            spriteBatch.DrawString(parent.UIFontSmall, aText, aTextOffset, parent.TextColor);

        }

        public override void Update()
        {
            base.Update();
            rSlider.Update();
            gSlider.Update();
            bSlider.Update();
            aSlider.Update();
        }
    }
}
