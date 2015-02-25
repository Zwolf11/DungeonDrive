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
        public int itemPower;
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

        public void updateItemPower(int power)
        {
            this.itemPower = power;
        }
        public string getName()
        {
            return this.itemName;
        }
        public int getID()
        {
            return this.itemID;
        }
        public String getItemTypeInString()
        {
            if (this.itemType.Equals(ItemType.Consumable))
            {

                return "Consumable";
            }
            else if (this.itemType.Equals(ItemType.Equipment))
            {

                return "Equipment";
            }
            else if (this.itemType.Equals(ItemType.Quest))
            {

                return "Quest";
            }
            else
            {

                return "unknown";
            }
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

        Equipment,
        Consumable,
        Quest
    }
}
