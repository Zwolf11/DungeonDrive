using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public static class G
    {
        public static int width = 500;
        public static int height = 500;
        public static int size = 32;
        public static int tickInt = 17;
        public static String pastRoom = "";
        public static String currentRoom = "C:\\";
        public static bool newRoom = false;
        public static MainForm form = new MainForm();
        public static Hero hero = new Hero(0, 0);
        public static Inventory inventory = new Inventory();
        public static System.Windows.Forms.Button quickButton1;       
        public static System.Windows.Forms.Button quickButton2;
        public static Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();
        public static Bitmap proj_img = new Bitmap(Properties.Resources.fire);
        public static Bitmap action_bar = new Bitmap(Properties.Resources.action_bar); // change action_bar to any image!
        public static Room room = new Room(currentRoom);
        public static ActionBar actionBar = new ActionBar(12, action_bar);
        public static Font txtFont = new Font("Arial", 10);
        public static String graveyard = currentRoom + "graveyard";
    }
}
