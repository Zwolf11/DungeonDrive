using System;
using System.Windows.Forms;
using System.Drawing;

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
