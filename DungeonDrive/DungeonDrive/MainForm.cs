using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class MainForm : Form
    {
        int windowstate = 0;

        private Random rand = new Random();
        private Timer timer = new Timer();

        public MainForm()
        {
            this.Text = "Dungeon Drive (D:)";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyDown += InputHandler.keyDown;
            this.KeyUp += InputHandler.keyUp;
            this.MouseDown += InputHandler.mouseUp;
            this.Paint += this.paint;
            this.DoubleBuffered = true;

            G.width = Screen.PrimaryScreen.Bounds.Width;
            G.height = Screen.PrimaryScreen.Bounds.Height;

            // for testing
            if (windowstate == 1)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Width = 1100;
                this.Height = 600;
                G.width = 1100;
                G.height = 600;
            }

            timer.Interval = G.tickInt;
            timer.Tick += Logic.tick;
            timer.Start();
        }

        public void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(Color.FromArgb(20, 20, 20));

            G.room.draw(g);
            G.hero.draw(g);
            G.actionBar.draw(g);
            //Jiang: Add Inventory system and draw on top of game if it's open
        }
    }
}
