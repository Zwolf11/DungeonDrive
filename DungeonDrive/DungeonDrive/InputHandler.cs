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
                int[] damage = {10, 100};
                Weapon item1 = new Weapon(1, "x", "shield_2.png", ItemType.Weapon, 5, damage, AtkStyle.Flame, 1, 1,1,1,1);
                
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
            G.actionBar.getAction(e.X, e.Y);
        }
        
    }
}
