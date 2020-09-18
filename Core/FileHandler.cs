﻿using CellED.UI.Elements;
using CellED.UI.SpriteTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CellED.Core
{
    public static class FileHandler
    {
        public static List<(Texture2D, string)> WorldObjectsFromTextures(ObjectHandler objectHandler, GraphicsDevice graphicsDevice, string pathToFolder)
        {
            if (Directory.Exists(pathToFolder))
            {
                string path = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), pathToFolder);
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles("*.png");

                FileStream fStream;
                List<(Texture2D, string)> worldObjects = new List<(Texture2D, string)>();

                foreach (FileInfo file in files)
                {
                    fStream = new FileStream(string.Format("{0}/{1}", pathToFolder, file.Name), FileMode.Open);
                    worldObjects.Add(
                        (PremultiplyAlpha(Texture2D.FromStream(graphicsDevice, fStream)), Path.GetFileNameWithoutExtension(file.Name))
                    );
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

        public static List<ListItem> GetProjectFiles(List parent)
        {
            string pathToDirectory = "Projects";
            if (Directory.Exists(pathToDirectory))
            {
                string path = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), pathToDirectory);
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles("*.prj");
                List<ListItem> listItems = new List<ListItem>(files.Length);

                foreach (FileInfo file in files)
                {
                    listItems.Add(new ListItem(parent, Path.GetFileNameWithoutExtension(file.Name)));
                }

                return listItems;
            }
            Debug.WriteLine("Directory doesn't exist.");
            return null;
        }

        public static bool SaveProject(List<WorldObject> worldObjects, string fileName, bool overwrite = false)
        {
            string pathToDirectory = "Projects";
            Directory.CreateDirectory(pathToDirectory);

            if (File.Exists(string.Format("{0}/{1}.prj", pathToDirectory, fileName)) && !overwrite)
            {
                return false;
            }

            using (StreamWriter writer = new StreamWriter(string.Format("{0}/{1}.prj", pathToDirectory, fileName)))
            {
                foreach(WorldObject obj in worldObjects)
                {
                    string objCSV = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                        obj.Name, obj.Pos.X, obj.Pos.Y, obj.Scale, obj.Rotation, obj.Color.R, obj.Color.G, obj.Color.B, obj.Color.A,
                        (int)obj.Effects, obj.Z);
                    writer.WriteLine(objCSV);
                }
            }
            return true;
        }

        public static bool LoadProject(string fileName, ObjectHandler objectHandler)
        {
            string pathToDirectory = "Projects";
            if (Directory.Exists(pathToDirectory))
            {
                if (File.Exists(string.Format("{0}/{1}.prj", pathToDirectory, fileName)))
                {
                    List<WorldObject> objects = new List<WorldObject>();
                    List<Task<WorldObject>> tasks = new List<Task<WorldObject>>();

                    using (StreamReader reader = new StreamReader(string.Format("{0}/{1}.prj", pathToDirectory, fileName)))
                    {
                        string data;

                        while ((data = reader.ReadLine()) != null)
                        {
                            string[] dataArray = data.Split(";");
                            if (dataArray.Length == 11)
                            {
                                tasks.Add(Task.Run(() => WorldObjectFromString(dataArray, objectHandler)));
                            }
                        }
                    }

                    Thread.Sleep(100);
                    Task.WaitAll(tasks.ToArray());

                    foreach (Task<WorldObject> task in tasks)
                    {
                        objects.Add(task.Result);
                    }
                    objectHandler.WorldObjects = objects;

                    return false;
                }
            }
            return false;
        }

        public static WorldObject WorldObjectFromString(string[] dataArray, ObjectHandler objectHandler)
        {
            
            
            string name = dataArray[0];
            Vector2 pos = new Vector2(float.Parse(dataArray[1]), float.Parse(dataArray[2]));
            (Texture2D, string) catalogItem = objectHandler.CatalogObjects.Find(obj => obj.Item2 == name);

            WorldObject newObject = new WorldObject(objectHandler, catalogItem.Item1, catalogItem.Item2, pos)
            {
                Scale = float.Parse(dataArray[3]),
                Rotation = float.Parse(dataArray[4]),
                Color = new Color(int.Parse(dataArray[5]), int.Parse(dataArray[6]), int.Parse(dataArray[7]), int.Parse(dataArray[8])),
                Effects = (SpriteEffects)int.Parse(dataArray[9]),
                Z = float.Parse(dataArray[10])
            };

            return newObject;
        }

        private static byte ApplyAlpha(byte color, byte alpha)
        {
            var fc = color / 255.0f;
            var fa = alpha / 255.0f;
            var fr = (int)(255.0f * fc * fa);
            if (fr < 0)
            {
                fr = 0;
            }
            if (fr > 255)
            {
                fr = 255;
            }
            return (byte)fr;
        }

        public static Texture2D PremultiplyAlpha(Texture2D texture)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < data.Length; ++i)
            {
                byte a = data[i].A;

                data[i].R = ApplyAlpha(data[i].R, a);
                data[i].G = ApplyAlpha(data[i].G, a);
                data[i].B = ApplyAlpha(data[i].B, a);
            }

            texture.SetData(data);
            return texture;
        }
    }
}
