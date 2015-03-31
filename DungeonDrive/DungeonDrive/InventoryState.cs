using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class InventoryState : State
    {
        public InventoryState(MainForm form) : base(form) { }

        private Item[][] getInventory()
        {
            GameState gs = (GameState)parent;
            return gs.inventory;
        }

        public override void keyUp(object sender, KeyEventArgs e) { }
        public override void mouseDown(object sender, MouseEventArgs e) { }
        public override void mouseUp(object sender, MouseEventArgs e) { }
        public override void mouseMove(object sender, MouseEventArgs e) { }
        public override void tick(object sender, EventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey || e.KeyCode == Properties.Settings.Default.InventoryKey)
            {
                this.close();
            }
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 100)), form.Width / 4, form.Height / 4, form.Width / 2, form.Height / 2);

            Item[][] inventory = getInventory();
            int boxWidth = form.Width / 2 / inventory.Length;
            int boxHeight = form.Height / 2 / inventory[0].Length;
            float paddingWidth = form.Width / 300;
            float paddingHeight = form.Height / 300;

            for (int i                                                                                                                                                                                                                                                                                                                                                                                                    = 0; i < inventory.Length; i++)
            {
                for (int j = 0; j < inventory[i].Length; j++)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(50, 50, 50)), form.Width / 4 + i * boxWidth + paddingWidth, form.Height / 4 + j * boxHeight + paddingHeight, boxWidth - 2 * paddingWidth, boxHeight - 2 * paddingHeight);
                }
            }
        }
    }
}
