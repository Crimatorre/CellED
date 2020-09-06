using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.Core
{
    public static class Utilities
    {
        public static Color[] CreateRectangleTexture(int length, Color? color = null)
        {
            Color[] colorData = new Color[length];
            for (int i = 0; i < length; i++)
            {
                colorData[i] = color ?? Color.White;
            }

            return colorData;
        }

        public static Color[,] ColorData1Dto2D(Color[] colorData1D, int width, int height)
        {
            Color[,] colorData2D = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colorData2D[x, y] = colorData1D[x + y * width];
                }
            }
            return colorData2D;
        }

        public static Color[] ColorData2Dto1D(Color[,] colorData2D, int width, int height)
        {
            Color[] colorData1D = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colorData1D[x + y * width] = colorData2D[x, y];
                }
            }
            return colorData1D;
        }

        public static Color[] CreateGappedRectangularBorder(Color[] colorData1D, int width, int height, int borderWidth, Color borderColor, Vector2 textSize)
        {
            Color[,] colorData2D = ColorData1Dto2D(colorData1D, width, height);
            return CreateGappedRectangularBorder(colorData2D, width, height, borderWidth, borderColor, textSize);
        }

        public static Color[] CreateGappedRectangularBorder(Color[,] colorData2D, int width, int height, int borderWidth, Color borderColor, Vector2 textSize)
        {

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < borderWidth || x > width - (borderWidth + 1) ||
                        y < borderWidth || y > height - (borderWidth + 1))
                    {
                        if (( x < 3 || x > 3 + textSize.X + 3) || (y > textSize.Y))
                        {
                            colorData2D[x, y] = borderColor;
                        }
                    }
                }
            }

            return ColorData2Dto1D(colorData2D, width, height); ;
        }

            public static Color[] CreateRectangularBorder(Color[,] colorData2D, int width, int height, int borderWidth, Color borderColor)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < borderWidth || x > width - (borderWidth + 1) ||
                        y < borderWidth || y > height - (borderWidth + 1))
                    {
                        colorData2D[x, y] = borderColor;
                    }
                }
            }

            return ColorData2Dto1D(colorData2D, width, height);
        }

        public static Color[] CreateRectangularBorder(Color[] colorData1D, int width, int height, int borderWidth, Color borderColor)
        {
            Color[,] colorData2D = ColorData1Dto2D(colorData1D, width, height);
            return CreateRectangularBorder(colorData2D, width, height, borderWidth, borderColor);
        }
    }
}
