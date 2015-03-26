using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DungeonDrive
{
    public class Inventory
    {
        int maxVolumn;
        LinkedList<Item> itemList = new LinkedList<Item>();
        public Inventory()
        {
            this.maxVolumn = 40;
            /*
             * 
             *  This is the initialization of a bunch of fake items. 
             * public Item(int ID, string name, string image, ItemType type)
             */
            //public Item(int ID, string name, string image, ItemType type)
            /*
            Item item1 = new Item(1, "potion_m", "HP_Potion_m.png", ItemType.Consumable);
            
            Item item2 = new Item(2, "postion_s", "HP_Potion_s.png", ItemType.Consumable);
            
            Item item3 = new Item(2, "shied1", "shield_1.png", ItemType.Armor);
            
            Weapon weapon1 = new Weapon(2, "shied1", "shield_1.png", ItemType.Armor, AtkStyle.Flame);
            this.itemList.AddLast(weapon1);
            this.itemList.AddLast(item1);
            this.itemList.AddLast(item2);
            this.itemList.AddLast(item3);
            */


        }
        public void addItem(Item item)
        {
            itemList.AddLast(item);
        }

        public int getItemQuantity(Item item) {
            return 1;
        }
        public LinkedList<Item> getItemList()
        {

            return this.itemList;
        }

        
        public void testInventory()
        {

            foreach (Item item in itemList)
            {
            //    Console.WriteLine("Name: " + item.getName() + "ID:  " + item.getID() + "Type: " + item.getItemTypeInString());
            }


        }
    }
}