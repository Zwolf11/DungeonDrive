using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace DungeonDrive
{
    class MessageState : State
    {
        private String message;
        private Font font = new Font("Arial", 24);
        private Action func;

        public MessageState(MainForm form, String message, Action func = null) : base(form)
        {
            this.message = message;
            this.func = func;
        }

        public override void mouseMove(object sender, MouseEventArgs e) { }
        public override void mouseUp(object sender, MouseEventArgs e) { }
        public override void tick(object sender, EventArgs e) 
        {
            GamePadState current = GamePad.GetState(PlayerIndex.One);

            if (current.IsConnected)
                updateInput();
        }
        public override void keyUp(object sender, KeyEventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.SelectKey)
            {
                if(func != null)
                    func();
                this.close();
            }
        }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            if (func != null)
                func();
            this.close();
        }

        public override void updateInput()
        {
            GamePadState current = GamePad.GetState(PlayerIndex.One);

            if (current.IsConnected && current.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                if (func != null)
                    func();
                this.close();
                System.Threading.Thread.Sleep(200);
            }
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
