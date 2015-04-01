/*using System;
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
}*/