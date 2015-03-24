using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace DungeonDrive
{



    public class ActionBar
    {

        public int x, y;
        public int number_of_buttons;
        

        private Bitmap actionBarImage;
        public List<Rectangle> actionList = new List<Rectangle>();
        public int width, height;
        public int selected = -1;
        public ActionBar(int number_of_buttons, Bitmap image)
        {
            this.actionBarImage = new Bitmap(image);
            this.number_of_buttons = number_of_buttons;
            this.width = image.Width / number_of_buttons;
            this.height = this.width;
            this.x = findMidX(G.width, actionBarImage.Width);
            this.y = G.height - this.height;
            int offset = 0;
            for (int i = 0; i < this.number_of_buttons; i++)
            {
                actionList.Add(new Rectangle(x + offset, y, this.width, this.height));
                offset = offset + this.width;
            }
           // act();          
        }

        private int findMidX(int outterWidth, int innerWidth) {

            int temp = outterWidth / 2 - innerWidth / 2;
            return temp;

        }

        public void act() {
            this.selected = -1;
        }
        public int getAction(int xCordinates, int yCordinates) {
            Rectangle temp;
            for (int i = 0; i < this.number_of_buttons; i++ )
            {
                temp = actionList.ElementAt(i);
                
                if ((temp.Location.X < xCordinates && xCordinates < temp.Location.X + width)
                    && ( yCordinates > temp.Location.Y)
                    ) {
                        this.selected = i;
                        return i;
                }
            }
            return -1;
            
        }

        public void draw(Graphics g)
        {   
           // g.DrawImage();
            g.DrawImage(actionBarImage,
                new Rectangle(this.x, this.y,
                    (int)(G.action_bar.Width), (int)(G.action_bar.Height)));
            
            Pen pen = new Pen(Color.Black, 6);
            for (int i = 0; i < this.number_of_buttons; i++)
            {
                g.DrawRectangle(pen, actionList.ElementAt(i));
               // g.FillRectangle
            }
            if (selected != -1) {
                g.DrawString( this.selected + "Selected", new Font("Arial", 20), Brushes.White, new PointF(G.width / 2 - 60, 5));
                g.DrawRectangle(new Pen(Color.BlueViolet,6), actionList.ElementAt(this.selected));
            }
           
        }


    }


}
