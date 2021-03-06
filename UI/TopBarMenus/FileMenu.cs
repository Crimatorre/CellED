using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CellED.UI.TopBarMenus
{
    public class FileMenu : TopBarMenu
    {
        public TopBarMenuButton newProjectButton;
        public TopBarMenuButton saveButton;
        public TopBarMenuButton loadButton;
        public TopBarMenuButton exitButton;

        public FileMenu(TopBar parent) : base(parent, "File", 4)
        {
            newProjectButton = new TopBarMenuButton(this, "New Project");
            saveButton = new TopBarMenuButton(this, "Save Project");
            loadButton = new TopBarMenuButton(this, "Load Project");
            exitButton = new TopBarMenuButton(this, "Exit");

            exitButton.ButtonPressed += ExitProgram;

            AddMenuButton(newProjectButton);
            AddMenuButton(saveButton);
            AddMenuButton(loadButton);
            AddMenuButton(exitButton);
        }

        private void ExitProgram()
        {
            parent.Quit();
        }
    }
}
