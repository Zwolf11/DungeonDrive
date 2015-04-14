using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace DungeonDrive
{
    class SkillStreeState : State
    {
        public static int availablePoints = 0;
        private Bitmap skillFrame1 = Properties.Resources.frame_0_acid;
        private Bitmap skillFrame2 = Properties.Resources.frame_1_acid;
        private Bitmap skillFrame3 = Properties.Resources.frame_8_acid;
        private Bitmap skillTreeBackGround = Properties.Resources.skilltreeabackground;
        private Rectangle skillTreeRectangle;
        private int selected;
        private bool isSelected = false;
        private int toLeft = 10, toTop = 10;
        
        private int skillList = 7;
        private Rectangle[] skillSet = new Rectangle[7];

        public SkillStreeState(MainForm form) : base(form) { }
        private GameState state { get { return (GameState)parent; } }
        public override void keyDown(object sender, KeyEventArgs e) { 
        }
        public override void mouseMove(object sender, MouseEventArgs e) { 
        }
        private RectangleF getBoxBounds(int i)
        {

            Rectangle rect = this.skillTreeRectangle;
            int size = form.ClientSize.Height/10;
            int padding = form.ClientSize.Height / 30;
            int y = (rect.Height + (form.ClientSize.Height - rect.Height) / -size) - padding;
            int x = (form.ClientSize.Width - rect.Width) / 2 + padding;
            skillSet[i] = new Rectangle(x + size * i * 2, y, size, size);
            return new Rectangle( x + size*i*2,y, size, size);
            
        }
        public override void mouseDown(object sender, MouseEventArgs e) {
            Rectangle click = new Rectangle(e.X, e.Y, 1, 1);

            for (int i = 0; i < skillList; i++ )
            {
                if (skillSet[i].Contains(click)) { this.selected =  i; isSelected = true; return; }
            }
            this.isSelected = false;
            
        }
        public override void mouseUp(object sender, MouseEventArgs e) { 
        }
        public override void tick(object sender, EventArgs e) {

        }
        public override void keyUp(object sender, KeyEventArgs e) { 
        }

        private Rectangle getBackgroundRectangle(int top, int left){
            
            return new Rectangle(this.form.ClientSize.Width/left, this.form.ClientSize.Height/top,
                (left-2)*form.ClientSize.Width/left, (top-2)*form.ClientSize.Height/top);
        }

        public override void paint(object sender, PaintEventArgs e) {

            skillTreeRectangle = this.getBackgroundRectangle(toLeft,toTop);
            Graphics g = e.Graphics;

            g.DrawImage(this.skillTreeBackGround, this.skillTreeRectangle);
            for (int i = 0; i < this.skillList; i++ )
            {
                g.DrawImage(skillFrame1, getBoxBounds(i));                
            }
            g.DrawImage(this.skillFrame2, getBoxBounds(this.selected));
            if(isSelected == true){
                g.DrawImage(Properties.Resources.ice, getBoxBounds(this.selected));
            }

        }
    }
}
