﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using System.IO;

namespace DungeonDrive
{
    public class TitleState : State
    {
        private int selection = 0;
        private Font titleFont = new Font("Arial", 36);
        private Font selectionFont = new Font("Arial", 16);
        private SoundPlayer errorSound = new SoundPlayer(@"attack1.wav");

        private String[] options = new String[]
        {
            "New Game",
            "Load Game",
            "Options",
            "Exit"
        };

        public TitleState(MainForm form) : base(form) { }

        public override void keyUp(object sender, KeyEventArgs e) { }
        public override void mouseDown(object sender, MouseEventArgs e) { }
        public override void mouseUp(object sender, MouseEventArgs e) { }
        public override void mouseMove(object sender, MouseEventArgs e) { }
        public override void tick(object sender, EventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Properties.Settings.Default.CloseKey)
            {
                form.Close();
            }
            else if(e.KeyCode == Properties.Settings.Default.UpKey)
            {
                if(--selection < 0)
                    selection = options.Length - 1;
            }
            else if(e.KeyCode == Properties.Settings.Default.DownKey)
            {
                selection = (selection + 1) % options.Length;
            }
            else if(e.KeyCode == Properties.Settings.Default.SelectKey)
            {
                if(selection == 0)
                    this.addChildState(new GameState(form, false), true);
                else if (selection == 1)
                {
                    if (File.Exists("save"))
                        this.addChildState(new GameState(form, true), true);
                    else
                        errorSound.Play();
                }
                else if (selection == 2)
                    this.addChildState(new OptionsState(form), true);
                else if (selection == 3)
                    form.Close();
            }

            form.Invalidate();
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(20, 20, 20));

            StringFormat align = new StringFormat();
            align.Alignment = StringAlignment.Center;
            align.LineAlignment = StringAlignment.Far;

            g.DrawString("Dungeon Drive (D:)", titleFont, Brushes.White, new RectangleF(0, 0, form.Width, form.Height / 2), align);

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
            g.DrawString(selectString, selectionFont, Brushes.White, new RectangleF(0, form.Height / 2, form.Width, form.Height / 2), align);
        }
    }
}
