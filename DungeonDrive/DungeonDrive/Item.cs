using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;

namespace DungeonDrive
{
    public class Item
    {
        public string itemName;
        public int itemID;
        public Bitmap itemImage;
        public string itemDesc;

        public Item(int ID, string name, Bitmap image)
        {
            this.itemID = ID;
            this.itemName = name;
            this.itemImage = image;
            this.itemDesc = "no description";
        }
        public string getName()
        {
            return this.itemName;
        }
        public int getID()
        {
            return this.itemID;
        }
        public void updateDesc(string description)
        {
            this.itemDesc = description;
        }
        public String getDesc()
        {
            return this.itemDesc;
        }
    }

    public enum AtkStyle
    {
        Basic,
        Flame,
        Frozen
    }

    public class Weapon : Item{
        public int atk_damage;
        public double atk_speed;
        public double proj_speed;
        public double range;
        public double slowSec;
        public double slowFac;
        public AtkStyle style;
        
        public Weapon(int id, String name, Bitmap image)
            : base(id, name, image)
        {
            this.itemID = id;
            this.itemName = name;
            this.itemImage = image;
        }
    }


}
