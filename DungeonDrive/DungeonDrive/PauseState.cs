﻿using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class PauseState : State
    {
        private int selection = 0;
        private Font titleFont = new Font("Arial", 36);
        private Font selectionFont = new Font("Arial", 16);

        private String[] options = new String[]
        {
            "Resume",
            "Save Game",
            "Options",
            "Exit"
        };

        public PauseState(MainForm form) : base(form) { }

        private void trySaveGame()
        {
            GameState gs = (GameState)parent;
            gs.saveGame();
        }

        public override void keyUp(object sender, KeyEventArgs e) { }
        public override void mouseDown(object sender, MouseEventArgs e) { }
        public override void mouseUp(object sender, MouseEventArgs e) { }
        public override void mouseMove(object sender, MouseEventArgs e) { }
        public override void tick(object sender, EventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey)
            {
                this.close();
            }
            else if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                if (--selection < 0)
                    selection = options.Length - 1;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                selection = (selection + 1) % options.Length;
            }
            else if (e.KeyCode == Properties.Settings.Default.SelectKey)
            {
                if (selection == 0)
                    this.close();
                else if (selection == 1)
                    trySaveGame();
                else if (selection == 2)
                    this.addChildState(new OptionsState(form), true);
                else if (selection == 3)
                    parent.close();
            }

            form.Invalidate();
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 100)), form.Width / 4, form.Height / 4, form.Width / 2, form.Height / 2);

            StringFormat align = new StringFormat();
            align.Alignment = StringAlignment.Center;
            align.LineAlignment = StringAlignment.Far;

            g.DrawString("Paused", titleFont, Brushes.White, new RectangleF(form.Width / 4, form.Height / 4, form.Width / 2, form.Height / 4), align);

            String selectString = "";
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selection)
                    selectString += "> ";
                selectString += options[i];
                if (i == selection)
                    selectString += " <";
                if (i != options.Length - 1)
                    selectString += "\n";
            }
            align.LineAlignment = StringAlignment.Near;
            g.DrawString(selectString, selectionFont, Brushes.White, new RectangleF(form.Width / 4, form.Height / 2, form.Width / 2, form.Height / 4), align);
        }
    }
}
