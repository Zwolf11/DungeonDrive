using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DungeonDrive
{
    public class ActionBar
    {
        private GameState state;
        public int x, y;
        public int number_of_buttons;
        private Bitmap actionBarImage;
        public List<Rectangle> actionList = new List<Rectangle>();
        public int width, height;
        public int selected = -1;

        public ActionBar(GameState state, int number_of_buttons, Bitmap image)
        {
            this.actionBarImage = image;
            this.number_of_buttons = number_of_buttons;
            this.width = image.Width / number_of_buttons;
            this.height = this.width;
            this.x = findMidX(state.form.Width, actionBarImage.Width);
            this.y = state.form.Height - this.height;

            int offset = 0;
            for (int i = 0; i < this.number_of_buttons; i++)
            {
                actionList.Add(new Rectangle(x + offset, y, this.width, this.height));
                offset = offset + this.width;
            }         
        }

        private int findMidX(int outterWidth, int innerWidth)
        {
            return outterWidth / 2 - innerWidth / 2;
        }

        public void act()
        {
            this.selected = -1;
        }

        public int getAction(int xCordinates, int yCordinates)
        {
            Rectangle temp;

            for (int i = 0; i < this.number_of_buttons; i++)
            {
                temp = actionList.ElementAt(i);
                
                if ((temp.Location.X < xCordinates && xCordinates < temp.Location.X + width) && ( yCordinates > temp.Location.Y))
                {
                    this.selected = i;
                    return i;
                }
            }

            return -1;
        }

        public void draw(Graphics g)
        {
            g.DrawImage(actionBarImage, x, y);
            
            Pen pen = new Pen(Color.Black, 6);

            for (int i = 0; i < this.number_of_buttons; i++)
                g.DrawRectangle(pen, actionList.ElementAt(i));

            if (selected != -1)
            {
                //g.DrawString(this.selected + "Selected", new Font("Arial", 20), Brushes.White, new PointF(state.form.Width / 2 - 60, 5));
                g.DrawRectangle(new Pen(Color.BlueViolet,6), actionList.ElementAt(this.selected));
            }
        }
    }
}
