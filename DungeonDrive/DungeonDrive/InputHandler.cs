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
                f1.Show();
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
