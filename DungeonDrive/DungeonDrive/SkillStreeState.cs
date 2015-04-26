using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace DungeonDrive
{
    class SkillStreeState : State
    {
        public static int availablePoints = 0;
        //horizonal 
        public static int skillList = 5;
        //verticle, spell level
        public static int skillLevel = 3;
        public static Spell spellSelected;


        public static Spell[] spellStored = new Spell[skillList];

        private Rectangle[,] skillFrame = new Rectangle[skillList, skillLevel];
        private Bitmap[,] skillFrameImages = new Bitmap[skillList, skillLevel];

        private Rectangle[,] skillSet = new Rectangle[skillList, skillLevel];
        // already learnt spells
        private Bitmap[,] skillSetImages = new Bitmap[skillList, skillLevel];
        private Bitmap[,] _skillSetImages = new Bitmap[skillList, skillLevel]; // images of spells that has not learnt
        // not learnt spells
        //private Bitmap[,] _skillSetImages = new Bitmap[skillList, skillLevel];

        //frames of a learnt spell
        private Bitmap spellFrame = Properties.Resources.frame_7_blue;
        //frames of a not leant spell, but can be leant
        private Bitmap _spellFrame = Properties.Resources.frame_0_grey;
        //frames of a not learnt spell, and the hero doesnt fullfill the pre-req 
        private Bitmap _NAspellFrame = Properties.Resources.frame_4_grey;

        private int iconSize;
        private Bitmap highLight = Properties.Resources.frame_0_red;

        private Rectangle skillTreeRectangle;
        private int skillSelected, levelSelected;
        private int skillMouseOver, levelMouseOver;
        private bool selectedOrNot = false, mouseOverOrNot = false;
        private bool isSettled = false;
        private int toLeft = 10, toTop = 10;
        private Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red);
        private int padding = 12;


        public SkillStreeState(MainForm form)
            : base(form)
        {
           

            for (int i = 0; i < skillList; i++)
            {
                for (int j = 0; j < skillLevel; j++)
                {
                    _skillSetImages[i, j] = Properties.Resources.empty;
                    if (GameState.heroSkill[i, j]) // if hero learnt this spell
                    {
                        skillSetImages[i, j] = Properties.Resources.empty;
                        
                        skillFrameImages[i, j] = this.spellFrame;

                    }
                    else
                    {

                        if (skillIsAvailable(i, j)) // skill is available
                        {
                            skillSetImages[i, j] = Properties.Resources.empty;
                            skillFrameImages[i, j] = this._spellFrame;


                        }
                        else
                        {
                            skillSetImages[i, j] = Properties.Resources.empty;
                            skillFrameImages[i, j] = this._NAspellFrame;
                        }

                    }
                }
            }

            addSpell(new LighteningBall(), 0);
            addSpell(new RuneOfFire(), 1);
            addSpell(new EnergyBarrier(), 2);
            addSpell(new CrusingFireBall(), 3);
            addSpell(new Pyroblast(), 4);


        }
        public bool skillIsAvailable(int i, int j)
        {

            if (j == 0) { return true; }
            else
            {
                if (GameState.heroSkill[i, j - 1])
                {
                    return true;
                }
            }
            return false;

        }

        private void addSpell(Spell spell, int i)
        {

            for (int j = 0; j < skillLevel; j++)
            {
                skillSetImages[i, j] = spell.spellIcon[j];
                _skillSetImages[i,j] = spell._spellIcon[j];
                spellStored[i] = spell;
            }

        }

        private GameState state { get { return (GameState)parent; } }
        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey || e.KeyCode == Properties.Settings.Default.SkillTreeKey)
                this.close();
        }
        public override void mouseMove(object sender, MouseEventArgs e)
        {
            Rectangle click = new Rectangle(e.X, e.Y, 1, 1);
            if (selectedOrNot == true)
            {
                for (int j = 0; j < skillLevel; j++)
                {
                    if (skillFrame[this.skillSelected, j].Contains(click))
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
                    if (skillFrame[i, j].Contains(click))
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
            int size = form.ClientSize.Height / 10;
            int y = form.ClientSize.Height / (skillLevel + 1);
            int x = form.ClientSize.Width / (skillList + 1);
            iconSize = size;
            skillFrame[i, j] = new Rectangle(form.ClientSize.Width - size / 2 - (x * (i + 1)), form.ClientSize.Height - size / 2 - (y * (j + 1)), size, size);
            return skillFrame[i, j];

        }

        private RectangleF getIconBoxBounds(int i, int j)
        {

            Rectangle rect = this.skillTreeRectangle;
            int size = form.ClientSize.Height / 10;
            int y = form.ClientSize.Height / (skillLevel + 1);
            int x = form.ClientSize.Width / (skillList + 1);
            iconSize = size;
            skillSet[i, j] = new Rectangle(form.ClientSize.Width - size / 2 - (x * (i + 1)) + padding / 2, form.ClientSize.Height - size / 2
                - (y * (j + 1)) + padding / 2, size - padding, size - padding);
            return new Rectangle(form.ClientSize.Width - size / 2 - (x * (i + 1)) + padding / 2, form.ClientSize.Height - size / 2
                - (y * (j + 1)) + padding / 2, size - padding, size - padding);

        }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            Rectangle click = new Rectangle(e.X, e.Y, 1, 1);
            if (e.Button == MouseButtons.Left)
            {
                
                if (selectedOrNot == true)
                {
                    for (int j = 0; j < skillLevel; j++)
                    {
                        if (skillFrame[this.skillSelected, j].Contains(click))
                        {
                            this.levelSelected = j;
                            selectedOrNot = true;
                            tryToLearn(this.skillSelected, this.levelSelected);
                            return;
                        }

                    }
                    selectedOrNot = false;
                    return;
                }

                for (int i = 0; i < skillList; i++)
                {
                    for (int j = 0; j < skillLevel; j++)
                    {
                        if (skillFrame[i, j].Contains(click))
                        {
                            this.skillSelected = i;
                            this.levelSelected = j;
                            selectedOrNot = true; 
                            spellSelected = spellStored[i];
                            return;
                        }
                    }

                }
                this.selectedOrNot = false;
            }
            else if(e.Button == MouseButtons.Right){
                spellSelected = null;
                for (int j = 0; j < skillLevel; j++)
                {
                    if (skillFrame[this.skillSelected, j].Contains(click))
                    {
                        
                        tryToUnlearn(this.skillSelected, j);
                    }

                }
            }

        }
        public override void mouseUp(object sender, MouseEventArgs e)
        {
        }



        private void tryToLearn(int i, int j) {

            if (skillIsAvailable(i, j) == true)
            {
                skillFrameImages[i, j] = spellFrame;
                GameState.heroSkill[i,j] = true;
                this.skillFrameImages[i, j] = spellFrame;
            }
            else { 
                
            }       
        }

        private void tryToUnlearn(int i, int j) {
            if (GameState.heroSkill[i,j]) {
                if ( j == SkillStreeState.skillLevel-1)
                {
                    GameState.heroSkill[i, j] = false;
                    this.skillFrameImages[i,j] = _spellFrame;
                }
                else if(GameState.heroSkill[i,j+1] == false){

                    GameState.heroSkill[i, j] = false;
                    this.skillFrameImages[i, j] = _spellFrame;
                }
            }
            else { }
        
        }
        public override void tick(object sender, EventArgs e)
        {

            if (selectedOrNot == true)
            {
                if ((this.skillFrame[skillSelected, levelSelected].X - this.form.ClientSize.Width / 2) > 25)
                {
                    isSettled = false;
                    for (int j = 0; j < skillLevel; j++)
                    {
                        this.skillFrame[this.skillSelected, j].X -= 25;
                        this.skillSet[this.skillSelected, j].X -= 25;
                    }

                }
                else if ((this.skillFrame[skillSelected, levelSelected].X < this.form.ClientSize.Width / 2))
                {
                    isSettled = false;
                    for (int j = 0; j < skillLevel; j++)
                    {
                        this.skillFrame[this.skillSelected, j].X += 25;
                        this.skillSet[this.skillSelected, j].X += 25;
                    }
                }
                else
                {
                    isSettled = true;
                    
                }
            }

            


        }
        public override void keyUp(object sender, KeyEventArgs e)
        {

        }

        private Rectangle getBackgroundRectangle(int top, int left)
        {

            return new Rectangle(this.form.ClientSize.Width / left, this.form.ClientSize.Height / top,
                (left - 2) * form.ClientSize.Width / left, (top - 2) * form.ClientSize.Height / top);
        }

        private void drawSingleList(PaintEventArgs e, int i)
        {
            Graphics g = e.Graphics;

            for (int j = 0; j < skillLevel; j++)
            {
                g.DrawImage(this.skillFrameImages[i, j], this.skillFrame[i, j]);
                if (GameState.heroSkill[i, j])
                    g.DrawImage(skillSetImages[i, j], this.skillSet[i, j]);
                else {
                    g.DrawImage(_skillSetImages[i, j], this.skillSet[i, j]);
                }

            }

        }

        private void connectSkillList(PaintEventArgs e, int i)
        {
            Graphics g = e.Graphics;
            
            pen.Width = 10;
            for (int j = 0; j < skillLevel - 1; j++)
            {
                if (skillIsAvailable(i, j+1)) { this.pen.Color = System.Drawing.Color.Green; }
                else { this.pen.Color = System.Drawing.Color.Gray; }
                g.DrawLine(pen, new Point(skillFrame[i, j].X + iconSize / 2, skillFrame[i, j].Y), 
                    new Point(skillFrame[i, j + 1].X + iconSize / 2, skillFrame[i, j + 1].Y + +iconSize));
                
            }
        }
        public override void paint(object sender, PaintEventArgs e)
        {

            skillTreeRectangle = this.getBackgroundRectangle(toLeft, toTop);
            Graphics g = e.Graphics;

            // g.DrawImage(this.skillTreeBackGround, this.skillTreeRectangle);

            if (selectedOrNot == false)
            {
                for (int i = 0; i < skillList; i++)
                {
                    for (int j = 0; j < skillLevel; j++)
                    {
                        if (GameState.heroSkill[i, j] == true)
                        {
                            g.DrawImage(skillFrameImages[i,j], getBoxBounds(i, j));
                            g.DrawImage(skillSetImages[i, j], getIconBoxBounds(i, j));
                        }
                        else
                        {
                            g.DrawImage(skillFrameImages[i,j], getBoxBounds(i, j));
                            g.DrawImage(_skillSetImages[i, j], getIconBoxBounds(i, j));
                        }

                    }
                }
            }
            else if (selectedOrNot == true)
            {
                g.DrawImage(_spellFrame, this.skillFrame[this.skillSelected, this.levelSelected]);
                drawSingleList(e, this.skillSelected);


                if (isSettled) { 
                    connectSkillList(e, this.skillSelected);                    
                }
            }
            if (mouseOverOrNot) { g.DrawImage(Properties.Resources.empty, this.skillFrame[this.skillMouseOver, this.levelMouseOver]); }

        }
    }
}