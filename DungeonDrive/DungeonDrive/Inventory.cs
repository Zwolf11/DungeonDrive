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
            this.maxVolumn = 9;
            /*
             * 
             *  This is the initialization of a bunch of fake items. 
             * public Item(int ID, string name, string image, ItemType type)
             */
            //public Item(int ID, string name, string image, ItemType type)
            Item item1 = new Item(1, "berry", "Properties.Resources.c2", ItemType.Consumable);
            item1.updateDesc("Eat this berry magic will happen!");
            Item item2 = new Item(2, "berry2", "Properties.Resources.c3", ItemType.Consumable);
            item2.updateDesc("Eat this berry2 magic will happen!");
            this.itemList.AddLast(item1);
            this.itemList.AddLast(item2);
            //  Application.EnableVisualStyles();
            //   Application.SetCompatibleTextRenderingDefault(false);
            //  Application.Run(new Form1(8));
            // Form1 f1 = new Form1();

            //f1.Show();

        }
        public void addItem(Item item)
        {
            itemList.AddLast(item);
        }

        public LinkedList<Item> getItemList()
        {

            return this.itemList;
        }

        public void testInventory()
        {

            foreach (Item item in itemList)
            {
                Console.WriteLine("Name: " + item.getName() + "ID:  " + item.getID() + "Type: " + item.getItemTypeInString());
            }


        }
    }
}