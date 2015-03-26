using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DungeonDrive
{



    public class Item
    {

        public string itemName;
        public int itemID;
        public string itemImage;
        public ItemType itemType;
        public string itemDesc;

        public Item(int ID, string name, string image, ItemType type)
        {
            this.itemID = ID;
            this.itemName = name;
            this.itemImage = image;
            this.itemType = type;
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
        public String  getDesc()
        {
            return this.itemDesc;
        }
        public ItemType getItemType()
        {
            return this.itemType;
        }
    }
    public enum ItemType
    {
        Weapon, 
        Armor, 
        Amulet,
        Consumable,
        Quest
    }

    public enum AtkStyle {     
        Flame,
        Frozen,
        Melee    
    }

    public class Weapon : Item{
        public int levelRequirement;
        int[] atk_damage = new int[2];
        double atk_speed;
        double speed;
        double range;
        double slowSec;
        double slowFac;

        public Weapon(int id, String name, String image, ItemType itemType,  // Item Parents
            int levelRequirement, int[] dmg, AtkStyle attackStyle,double atk_speed, double speed,
            double range, double slowSec, double slowFac)
            : base(id, name, image, itemType)
        {
            this.levelRequirement = levelRequirement;
            this.atk_damage = dmg;
            this.atk_speed = atk_speed;
            this.speed = speed;
            this.range = range;
            this.slowFac = slowFac;
            this.slowSec = slowSec;
            this.itemDesc = this.itemName + "\n" + "Attack style: " + attackStyle.ToString() + "\n"
                
                + "attack speed / projectile speed" + this.atk_speed + " | "+ this.speed+ "\n" +
                "attack range" + this.range + "\nDMG:" + this.atk_damage[0] + "-" + this.atk_damage[1]; 
        }
    
    }


}
