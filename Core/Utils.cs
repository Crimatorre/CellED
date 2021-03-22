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


        public static Color[] CreateGappedRectangularBorder(int width, int height, int borderWidth, Color borderColor, Vector2 textSize)
        {
            Color[] colorData1D = new Color[width * height];
            return CreateGappedRectangularBorder(colorData1D, width, height, borderWidth, borderColor, textSize);
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
                        else
                        {
                            colorData2D[x, y] = Color.Transparent;
                        }
                    }
                }
            }

            return ColorData2Dto1D(colorData2D, width, height); ;
        }

        internal static byte[,] ColorDatato2DByteData(Color[] colorData, int width, int height)
        {
            byte[,] byteData2D = new byte[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (colorData[x + y * width].A < 255 / 2)
                    {
                        byteData2D[x, y] = 0;
                    }
                    else
                    {
                        byteData2D[x, y] = 1;
                    }
                }
            }
            return byteData2D;
        }

        public static Color[] CreateOutlineTexture(Color[] colorData, int width, int height, int borderWidth)
        {
            Color[,] colorData2D = ColorData1Dto2D(colorData, width, height);
            Color[,] outlinedData = new Color[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (colorData2D[x, y] != Color.Transparent)
                    {
                        for (int u = -borderWidth; u <= borderWidth; u++)
                        {
                            for (int v = -borderWidth; v <= borderWidth; v++)
                            {
                                if (u + x >= 0 && u + x < width &&
                                    v + y >= 0 && v + y < height)
                                {
                                    outlinedData[x + u, y + v] = Color.White;
                                }
                            }
                        }
                    }
                }
            }

            CutDataFromOther(ref outlinedData, colorData2D, width, height);

            return ColorData2Dto1D(outlinedData, width, height);
        }

        // Revisit algorithm, that creates a bit bigger data than original and creates outline
        public static Color[] CreateOutlineTexture(byte[,] colorData, int width, int height, int borderWidth)
        {
            Color[] outlineColorData = new Color[(width + borderWidth * 2) * (height + borderWidth * 2)];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (colorData[x, y] != 0)
                    {
                        for (int u = -borderWidth; u <= borderWidth; u++)
                        {
                            for (int v = -borderWidth; v <= borderWidth; v++)
                            {
                                if (u + x + borderWidth >= 0 && u + x + borderWidth < width + borderWidth * 2 &&
                                    v + y + borderWidth >= 0 && v + y + borderWidth < height + borderWidth * 2)
                                {
                                    if (x + u >= width || y + v >= height || x + u <= 0 || y + v <= 0)
                                    {
                                        outlineColorData[(x + u + borderWidth) + (y + v + borderWidth) * (width + borderWidth * 2)] = Color.White;
                                    }
                                    else if (colorData[x + u, y + v] != 1)
                                    {
                                        outlineColorData[(x + u + borderWidth) + (y + v + borderWidth) * (width + borderWidth * 2)] = Color.White;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return outlineColorData;
        }

        private static Color[] ByteDataToColorData(byte[,] outlineData, int width, int height)
        {
            Color[] colorData1D = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (outlineData[x, y] != 0)
                    {
                        colorData1D[x + y * width] = Color.White;
                    }
                    else
                    {
                        colorData1D[x + y * width] = Color.Transparent;
                    }
                }
            }
            return colorData1D;
        }

        private static void CutDataFromOther(ref Color[,] outlinedData, Color[,] colorData2D, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (colorData2D[x, y] != Color.Transparent)
                    {
                        outlinedData[x, y] = Color.Transparent;
                    }
                }
            }
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
