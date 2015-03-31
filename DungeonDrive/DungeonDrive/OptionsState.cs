using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class OptionsState : State
    {
        private int selection = 0;
        private Font titleFont = new Font("Arial", 36);
        private Font selectionFont = new Font("Arial", 16);

        private String[] options;

        public OptionsState(MainForm form) : base(form) { updateOptions(); }

        private void updateOptions()
        {
            options = new String[]
            {
                "Close: " + Properties.Settings.Default.CloseKey.ToString(),
                "Up: " + Properties.Settings.Default.UpKey.ToString(),
                "Left: " + Properties.Settings.Default.LeftKey.ToString(),
                "Down: " + Properties.Settings.Default.DownKey.ToString(),
                "Right: " + Properties.Settings.Default.RightKey.ToString(),
                "Select: " + Properties.Settings.Default.SelectKey.ToString(),
                "Inventory: " + Properties.Settings.Default.InventoryKey.ToString(),
                "Attack1: " + Properties.Settings.Default.Attack1Key.ToString(),
                "Attack2: " + Properties.Settings.Default.Attack2Key.ToString(),
                "Attack3: " + Properties.Settings.Default.Attack3Key.ToString(),
                "Fullscreen: " + Properties.Settings.Default.FullScreen.ToString(),
                "Exit"
            };
        }

        private void rebind(object sender, KeyEventArgs e)
        {
            if (selection == 0)
                Properties.Settings.Default.CloseKey = e.KeyCode;
            else if (selection == 1)
                Properties.Settings.Default.UpKey = e.KeyCode;
            else if (selection == 2)
                Properties.Settings.Default.LeftKey = e.KeyCode;
            else if (selection == 3)
                Properties.Settings.Default.DownKey = e.KeyCode;
            else if (selection == 4)
                Properties.Settings.Default.RightKey = e.KeyCode;
            else if (selection == 5)
                Properties.Settings.Default.SelectKey = e.KeyCode;
            else if (selection == 6)
                Properties.Settings.Default.InventoryKey = e.KeyCode;
            else if (selection == 7)
                Properties.Settings.Default.Attack1Key = e.KeyCode;
            else if (selection == 8)
                Properties.Settings.Default.Attack2Key = e.KeyCode;
            else if (selection == 9)
                Properties.Settings.Default.Attack3Key = e.KeyCode;

            updateOptions();
            Properties.Settings.Default.Save();

            form.KeyDown -= this.rebind;
            form.KeyDown += this.keyDown;

            form.Invalidate();
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
                if (selection >= 0 && selection <= 9)
                {
                    if (selection == 0)
                        Properties.Settings.Default.CloseKey = Keys.None;
                    else if (selection == 1)
                        Properties.Settings.Default.UpKey = Keys.None;
                    else if (selection == 2)
                        Properties.Settings.Default.LeftKey = Keys.None;
                    else if (selection == 3)
                        Properties.Settings.Default.DownKey = Keys.None;
                    else if (selection == 4)
                        Properties.Settings.Default.RightKey = Keys.None;
                    else if (selection == 5)
                        Properties.Settings.Default.SelectKey = Keys.None;
                    else if (selection == 6)
                        Properties.Settings.Default.InventoryKey = Keys.None;
                    else if (selection == 7)
                        Properties.Settings.Default.Attack1Key = Keys.None;
                    else if (selection == 8)
                        Properties.Settings.Default.Attack2Key = Keys.None;
                    else if (selection == 9)
                        Properties.Settings.Default.Attack3Key = Keys.None;

                    updateOptions();

                    form.KeyDown += this.rebind;
                    form.KeyDown -= this.keyDown;
                }
                else if (selection == 10)
                {
                    form.setFullscreen(!Properties.Settings.Default.FullScreen);
                    updateOptions();
                }
                else if (selection == 11)
                {
                    this.close();
                }
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

            g.DrawString("Options", titleFont, Brushes.White, new RectangleF(0, 0, form.Width, 0.3f * form.Height), align);

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
            g.DrawString(selectString, selectionFont, Brushes.White, new RectangleF(0, 0.3f * form.Height, form.Width, 0.7f * form.Height), align);
        }
    }
}
