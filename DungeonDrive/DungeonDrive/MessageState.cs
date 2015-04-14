using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    class MessageState : State
    {
        private String message;
        private Font font = new Font("Arial", 24);

        public MessageState(MainForm form, String message) : base(form)
        {
            this.message = message;
        }

        public override void mouseMove(object sender, MouseEventArgs e) { }
        public override void mouseUp(object sender, MouseEventArgs e) { }
        public override void tick(object sender, EventArgs e) { }
        public override void keyUp(object sender, KeyEventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.SelectKey)
                this.close();
        }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            this.close();
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.info, form.ClientSize.Width / 4, form.ClientSize.Height / 4, form.ClientSize.Width / 2, form.ClientSize.Height / 2);

            StringFormat align = new StringFormat();
            align.Alignment = StringAlignment.Center;
            align.LineAlignment = StringAlignment.Center;

            g.DrawString(message, font, Brushes.White, new RectangleF(form.ClientSize.Width / 4, form.ClientSize.Height / 4, form.ClientSize.Width / 2, form.ClientSize.Height / 2), align);
        }
    }
}
