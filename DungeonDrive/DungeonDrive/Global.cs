using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DungeonDrive
{
    public static class G
    {
        public static int width = 500;
        public static int height = 500;
        public static int size = 30;
        public static MainForm form = new MainForm();
        public static Room room = new Room("C:");
        public static Hero hero = new Hero(0, 0, 0.3);
        public static Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();
    }
}
