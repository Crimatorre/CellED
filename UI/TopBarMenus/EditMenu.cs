using System;
using System.Collections.Generic;
using System.Text;

namespace CellED.UI.TopBarMenus
{
    public class EditMenu : TopBarMenu
    {
        public EditMenu(TopBar parent) : base(parent, "Edit", 3)
        {
            AddMenuButton(new TopBarMenuButton(this, "Blahblah"));
            AddMenuButton(new TopBarMenuButton(this, "Blah Blahblah"));
        }
    }
}
