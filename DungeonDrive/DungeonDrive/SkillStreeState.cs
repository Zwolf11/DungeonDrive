using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace DungeonDrive
{
    class SkillStreeState : State
    {
        public static int availablePoints = 0;
        public static int skillList = 7;
        public static int skillLevel = 3;

        private Bitmap[,] skillFrameImages = new Bitmap[10, 10];
        private Bitmap[,] skillSetImages = new Bitmap[10, 10];

        private Bitmap skillFrame1 = Properties.Resources.frame_0_acid;
        private Bitmap skillFrame2 = Properties.Resources.frame_1_acid;
        private Bitmap skillFrame3 = Properties.Resources.frame_8_acid;
        private int iconSize;
        private Bitmap highLight = Properties.Resources.frame_0_red;

        private Rectangle skillTreeRectangle;
        private int skillSelected, levelSelected;
        private int skillMouseOver, levelMouseOver;
        private bool selectedOrNot = false, mouseOverOrNot = false;
        private bool isSettled = false;
        private int toLeft = 10, toTop = 10;


        private Rectangle[,] skillSet = new Rectangle[skillList, skillLevel];
        
        public SkillStreeState(MainForm form) : base(form) {

            //addSpell(LighteningBall LighteningBall, );
            
            for (int i = 0; i < skillList; i++ )
            {
                 for (int j = 0; j < skillLevel; j++ )
                 {
                     skillFrameImages[i, j] = Properties.Resources.ghost2;
                 }
            }

            skillFrameImages[0, 0] = Properties.Resources.frame_0_eerie;
            skillFrameImages[0, 1] = Properties.Resources.frame_7_eerie;
            skillFrameImages[0, 2] = Properties.Resources.frame_8_eerie;
            skillFrameImages[1, 0] = Properties.Resources.frame_0_jade;
            skillFrameImages[1, 1] = Properties.Resources.frame_3_jade;
            skillFrameImages[1, 2] = Properties.Resources.frame_9_jade;
            skillFrameImages[2, 0] = Properties.Resources.frame_0_orange;
            skillFrameImages[2, 1] = Properties.Resources.frame_2_orange;
            skillFrameImages[2, 2] = Properties.Resources.frame_5_orange;
            skillFrameImages[3, 0] = Properties.Resources.frame_0_sky;
            skillFrameImages[3, 1] = Properties.Resources.frame_4_sky;
            skillFrameImages[3, 2] = Properties.Resources.frame_4_sky;
            skillFrameImages[4, 0] = Properties.Resources.frame_0_royal;
            skillFrameImages[4, 1] = Properties.Resources.frame_4_royal;
            skillFrameImages[4, 2] = Properties.Resources.frame_6_royal;
            skillFrameImages[5, 0] = Properties.Resources.frame_0_acid;
            skillFrameImages[5, 1] = Properties.Resources.frame_1_acid;
            skillFrameImages[5, 2] = Properties.Resources.frame_8_acid;
            skillFrameImages[6, 0] = Properties.Resources.frame_0_magenta;
            skillFrameImages[6, 1] = Properties.Resources.frame_2_magenta;
            skillFrameImages[6, 2] = Properties.Resources.frame_5_magenta;
        
        }

        private void addSpell(Spell spell, int i) {

            for (int j = 0; j < skillLevel; j++ )
            {
                skillSetImages[i, j] = spell.spellIcon[j];
            }

        }

        private GameState state { get { return (GameState)parent; } }
        public override void keyDown(object sender, KeyEventArgs e) { 
        }
        public override void mouseMove(object sender, MouseEventArgs e) {
            Rectangle click = new Rectangle(e.X, e.Y, 1, 1);
            if (selectedOrNot == true)
            {
                for (int j = 0; j < skillLevel; j++)
                {
                    if (skillSet[this.skillSelected, j].Contains(click))
                    {
                        this.levelMouseOver = j;
                        this.skillMouseOver = this.skillSelected;
                        mouseOverOrNot = true;
                        return;
                    }

                }
                mouseOverOrNot = false;
                return;
            }     
            for (int i = 0; i < skillList; i++)
            {
                for (int j = 0; j < skillLevel; j++)
                {
                    if (skillSet[i, j].Contains(click))
                    {
                        skillMouseOver = i;
                        levelMouseOver = j;
                        mouseOverOrNot = true;
                        return;
                    }
                }
            }

            mouseOverOrNot = false;
        }
        private RectangleF getBoxBounds(int i, int j)
        {

            Rectangle rect = this.skillTreeRectangle;
            int size = form.ClientSize.Height/10;
            int padding = form.ClientSize.Height / 10;
            int y = (rect.Height + (form.ClientSize.Height - rect.Height) / -size) - padding;
            int x = (form.ClientSize.Width - rect.Width) / 2 + padding;
            iconSize = size;
            skillSet[i, j] = new Rectangle(x + size * i * 2, y - size * j * 2, size, size);
            return skillSet[i, j];
            
        }
        public override void mouseDown(object sender, MouseEventArgs e) {
            Rectangle click = new Rectangle(e.X, e.Y, 1, 1);
            if (selectedOrNot == true) {
                for (int j = 0; j < skillLevel; j++)
                {
                    if (skillSet[this.skillSelected, j].Contains(click))
                    {
                        this.levelSelected = j;
                        selectedOrNot = true;
                        return;
                    }
                    
                }
                selectedOrNot = false;
                return;
            }            
            for (int i = 0; i < skillList; i++ )
            {
                for (int j = 0; j < skillLevel; j++ )
                {
                    if (skillSet[i, j].Contains(click)) { 
                        this.skillSelected = i;
                        this.levelSelected = j;
                        selectedOrNot = true; return; 
                    }
                }
                
            }
            this.selectedOrNot = false;
            
        }
        public override void mouseUp(object sender, MouseEventArgs e) { 
        }
        public override void tick(object sender, EventArgs e) {

            if (selectedOrNot == true)
            {
                if ( (this.skillSet[skillSelected, levelSelected].X - this.form.ClientSize.Width / 2) >15)
                {
                    isSettled = false;
                    for (int j = 0; j < skillLevel; j++ )
                    {
                        this.skillSet[this.skillSelected,j].X -= 15;
                    }
                    
                }
                else if ((this.skillSet[skillSelected, levelSelected].X < this.form.ClientSize.Width / 2))
                {
                    isSettled = false;
                    for (int j = 0; j < skillLevel; j++)
                    {
                        this.skillSet[this.skillSelected, j].X += 15;
                    }
                }
                else {
                    isSettled = true;
                }                                 
            }
            

        }
        public override void keyUp(object sender, KeyEventArgs e) { 

        }

        private Rectangle getBackgroundRectangle(int top, int left){
            
            return new Rectangle(this.form.ClientSize.Width/left, this.form.ClientSize.Height/top,
                (left-2)*form.ClientSize.Width/left, (top-2)*form.ClientSize.Height/top);
        }

        private void drawSingleList(PaintEventArgs e, int i) {
            Graphics g = e.Graphics;

            for (int j = 0; j < skillLevel; j++ )
            {
                g.DrawImage(this.skillFrameImages[i, j], this.skillSet[i, j]);
                
            }

        }

        private void connectSkillList(PaintEventArgs e, int i) {
            Graphics g = e.Graphics;
             Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red);
             pen.Width = 10;
            for (int j = 0; j < skillLevel-1; j++ )
            {

                g.DrawLine(pen, new Point(skillSet[i, j].X + iconSize / 2, skillSet[i, j].Y), new Point(skillSet[i, j + 1].X + iconSize / 2, skillSet[i, j + 1].Y + +iconSize ));
            }
        }
        public override void paint(object sender, PaintEventArgs e) {

            skillTreeRectangle = this.getBackgroundRectangle(toLeft,toTop);
            Graphics g = e.Graphics;
           
           // g.DrawImage(this.skillTreeBackGround, this.skillTreeRectangle);

            if (selectedOrNot == false)
            {
                for (int i = 0; i < skillList; i++)
                {
                    for (int j = 0; j < skillLevel; j++ )
                    {
                        g.DrawImage(skillFrameImages[i, j], getBoxBounds(i, j));
                    }
                }
            }
            else if (selectedOrNot == true)
            {
                g.DrawImage(this.skillFrameImages[this.skillSelected, this.levelSelected], this.skillSet[this.skillSelected, this.levelSelected]);
                drawSingleList(e, this.skillSelected);                
                g.DrawImage(Properties.Resources.ice, this.skillSet[this.skillSelected, this.levelSelected]);
                if (isSettled) { connectSkillList(e, this.skillSelected); }
            }
            if (mouseOverOrNot) { g.DrawImage(this.highLight, this.skillSet[this.skillMouseOver, this.levelMouseOver]); }
            
        }
    }
}
