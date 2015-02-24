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
        public static int tickInt = 17;
        public static String pastRoom = "";
        public static String currentRoom = "C:\\";
        public static bool newRoom = false;
        public static MainForm form = new MainForm();
        public static Hero hero = new Hero(0, 0);
        public static Inventory inventory = new Inventory();
        public static Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();
        public static Room room = new Room(currentRoom);
    }
}
