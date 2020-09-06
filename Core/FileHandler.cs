using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CellED.Core
{
    public static class FileHandler
    {
        public static List<WorldObject> WorldObjectsFromTextures(ObjectHandler objectHandler, GraphicsDevice graphicsDevice, string pathToFolder)
        {
            if (Directory.Exists(pathToFolder))
            {
                string path = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), pathToFolder);
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles("*.png");

                FileStream fStream;
                List<WorldObject> worldObjects = new List<WorldObject>();

                foreach (FileInfo file in files)
                {
                    fStream = new FileStream(pathToFolder + "/" + file.Name, FileMode.Open);
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fStream);
                    worldObjects.Add(new WorldObject(objectHandler, texture, Path.GetFileNameWithoutExtension(file.Name)));
                    fStream.Dispose();
                }

                return worldObjects;
            }
            else
            {
                Debug.WriteLine("Directory not found.");
                return null;
            }
        }
    }
}
