using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    class SkillStreeState : State
    {

        private Bitmap skillFrame = Properties.Resources.lighteningball_1_20_12;
        public SkillStreeState(MainForm form) : base(form) { }
        private GameState state { get { return (GameState)parent; } }
        public override void keyDown(object sender, KeyEventArgs e) { 
        }
        public override void mouseMove(object sender, MouseEventArgs e) { 
        }
        private RectangleF getBoxBounds()
        {

            float boxSize = form.ClientSize.Height / 5;
            float padding = form.ClientSize.Height / 300;

            float left = form.ClientSize.Width / 3;
            float top = 2 * form.ClientSize.Height / 3;
            float size = boxSize - 2 * padding;

            return new RectangleF(left, top, size, size);
        }
        public override void mouseDown(object sender, MouseEventArgs e) { 
        }
        public override void mouseUp(object sender, MouseEventArgs e) { 
        }
        public override void tick(object sender, EventArgs e) { 
        }
        public override void keyUp(object sender, KeyEventArgs e) { 
        }

        
        public override void paint(object sender, PaintEventArgs e) {


           // Rectangle rect = new Rectangle(form.ClientSize.Width/2, form.ClientSize.Height/2, 50,50);

            Graphics g = e.Graphics;
            g.DrawImage(skillFrame, getBoxBounds());


        }
    }
}
