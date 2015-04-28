using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace DungeonDrive
{
    public class GameOverState : State
    {
        private Font titleFont = new Font("Arial", 36);

        public GameOverState(MainForm form) : base(form) { }

        public override void keyUp(object sender, KeyEventArgs e) { }
        public override void mouseDown(object sender, MouseEventArgs e) { }
        public override void mouseUp(object sender, MouseEventArgs e) { }
        public override void mouseMove(object sender, MouseEventArgs e) { }
        public override void tick(object sender, EventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.SelectKey || e.KeyCode == Properties.Settings.Default.CloseKey)
            {
                parent.close();
                form.Invalidate();
            }
        }

        public override void updateInput()
        {
            GamePadState current = GamePad.GetState(PlayerIndex.One);

            if (current.IsConnected && current.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                parent.close();
                form.Invalidate();
            }
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.info, form.ClientSize.Width / 4, form.ClientSize.Height / 4, form.ClientSize.Width / 2, form.ClientSize.Height / 2);

            StringFormat align = new StringFormat();
            align.Alignment = StringAlignment.Center;
            align.LineAlignment = StringAlignment.Center;

            g.DrawString("Game Over", titleFont, Brushes.White, new RectangleF(form.ClientSize.Width / 4, form.ClientSize.Height / 4, form.ClientSize.Width / 2, form.ClientSize.Height / 2), align);
        }
    }
}
