using System;
using System.Windows.Forms;

namespace DungeonDrive
{
    public static class InputHandler
    {
        public static void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                G.form.Close();
            else if(e.KeyCode == Keys.Space)
            {                
                InventoryForm f1 = new InventoryForm(G.inventory);
                f1.TopMost = true;
                f1.Show();
            }
            else if(e.KeyCode == Keys.F1){
                Item item1 = new Item(1, "shield", "shield_2.png", ItemType.Equipment);
                item1.updateDesc("Defense + 2\nMAX HP + 2");
                G.inventory.addItem(item1);
            }
            else if(e.KeyCode == Keys.F2){

                if (G.quickButton1 != null)
                {
                    G.quickButton1.PerformClick();
                }
                               
            }
            G.keys[e.KeyCode] = true;
        }

        public static void keyUp(object sender, KeyEventArgs e)
        {
            G.keys.Remove(e.KeyCode);
        }

        public static void mouseUp(object sender, MouseEventArgs e)
        {
            G.hero.basicAtk();
        }
    }
}
