﻿using System;
using System.Windows.Forms;
using System.Drawing;



namespace DungeonDrive
{
    public class MainForm : Form
    {
        int windowstate = 1;

        private Random rand = new Random();
        private Timer timer = new Timer();

        public MainForm()
        {
            this.Text = "Dungeon Drive (D:)";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyDown += InputHandler.keyDown;
            this.KeyUp += InputHandler.keyUp;
            this.Paint += this.paint;
            this.DoubleBuffered = true;

            // for testing
            if (windowstate == 1)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Width = 1100;
                this.Height = 600;
            }

            timer.Interval = 17;
            timer.Tick += Logic.tick;
            timer.Start();
        }

        private void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            G.room.draw(g);
            G.hero.draw(g);

            //Jiang: Add Inventory system and draw on top of game if it's open
        }
    }
}
