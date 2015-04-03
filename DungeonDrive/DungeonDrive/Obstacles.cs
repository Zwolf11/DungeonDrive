using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Obstacle
    {
        protected GameState state;
        public int x;
        public int y;
        public int width;
        public int height;
        public int roomNum;


        public int DrawX { get { return (int)(x * state.size + state.form.ClientSize.Width / 2 - state.hero.x * state.size); } }
        public int DrawY { get { return (int)(y * state.size + state.form.ClientSize.Height / 2 - state.hero.y * state.size); } }
 

        public Obstacle(GameState state, int x, int y, int width, int height, int roomNum)
        {
            this.state = state;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.roomNum = roomNum;
        }

        public abstract void draw(Graphics g);
    }

    public class Pillar : Obstacle
    {
        private Bitmap img = new Bitmap(Properties.Resources.pillar);

        public Pillar(GameState state, int x, int y, int width, int height, int roomNum) : base(state, x, y, width, height, roomNum) { }

        public override void draw(Graphics g)
        { 
           g.DrawImage(img, DrawX, DrawY, state.size, state.size);
        }
    }

    public class Chest : Obstacle
    {

        public bool closed = true;
        private Bitmap openImg = new Bitmap(Properties.Resources.chest_open);
        private Bitmap closedImg = new Bitmap(Properties.Resources.chest_closed);
 

        public Chest(GameState state, int x, int y, int width, int height, int roomNum) : base(state, x, y, width, height, roomNum) { }

        public override void draw(Graphics g)
        {
            if(closed)
                g.DrawImage(closedImg, DrawX, DrawY, state.size, state.size);
           else
               g.DrawImage(openImg, DrawX, DrawY, state.size, state.size);
   
        }
    }

    public class Stairs : Obstacle
    {
        public bool down;
        public String path;
        public char direction;
        public int xDirection;
        public int yDirection;
        public Font font = new Font("Arial", 10);
        private Bitmap stairUp = new Bitmap(Properties.Resources.stairUp);
        private Bitmap stairDown = new Bitmap(Properties.Resources.stairDown);
        public int maxHallwayWidth = 10;
        public int centerX, centerY;


        public Stairs(GameState state, int x, int y, int width, int height, int roomNum, bool down, String path, char direction, int maxHallwayWidth, int centerX, int centerY ) : base(state, x, y, width, height, roomNum) {
            this.down = down;
            this.path = path;
            this.direction = direction;
            this.maxHallwayWidth = maxHallwayWidth;
            this.centerX = centerX;
            this.centerY = centerY;

            switch (direction)
            {
                case 'w':
                    xDirection = 0;
                    yDirection = -1;
                    break;
                case 's':
                    xDirection = 0;
                    yDirection = 1;
                    break;
                case 'a':
                    xDirection = -1;
                    yDirection = 0;
                    break;
                case 'd':
                    xDirection = 1;
                    yDirection = 0;
                    break;
            }
        }





        public override void draw(Graphics g)
        {
            if (down)
            {
                g.DrawImage(stairDown, DrawX, DrawY, state.size * width, state.size * height);
                //g.FillRectangle(Brushes.IndianRed, DrawX, DrawY, state.size * width, state.size * height);
            }
            else
            {
                g.DrawImage(stairUp, DrawX, DrawY, state.size * width, state.size * height);
                //g.FillRectangle(Brushes.Green, DrawX, DrawY, state.size * width, state.size * height);
            }

            string roomNumString = roomNum.ToString();

            g.DrawString(path.Substring(path.LastIndexOf('\\') + 1) + " " + roomNumString , font, Brushes.White, new PointF(DrawX, DrawY - state.size / 2));
        }
    }

    public class Door : Obstacle
    {
        public bool vertical;
        public int maxPositive;
        public int maxNegative;
        public bool closed = true;
        private Bitmap doorClosed; 
        private Bitmap doorOpen;

        public Door(GameState state, int x, int y, int width, int height, int roomNum, bool vertical, int maxNegative, int maxPositive) : base(state, x,y,width,height, roomNum) {

            if(vertical){
                doorClosed = new Bitmap(Properties.Resources.doorClosedVertical);
                doorOpen = new Bitmap(Properties.Resources.doorOpenVertical);
            } else {
                doorClosed = new Bitmap(Properties.Resources.doorClosedHorizontal);
                doorOpen = new Bitmap(Properties.Resources.doorOpenHorizontal);
            }
            this.vertical = vertical;
            this.maxPositive = maxPositive;
            this.maxNegative = maxNegative;
        }

        public bool switchClosed(){
            if(closed){
                closed = false;
                state.room.walkingSpace[x, y] = true;
                state.room.walkingSpace[(int) x + (width / 2), (int) y + (height / 2)] = true;
            } else {
                closed = true;
                state.room.walkingSpace[x, y] = false;
                state.room.walkingSpace[(int)x + (width / 2), (int)y + (height / 2)] = false;
            }

            return closed;
        }

        public int getNegativeRoom()
        {
            if (vertical)
            {
                return state.room.roomNumSpace[x - 1, y];
            }
            else
            {
                return state.room.roomNumSpace[x, y - 1];
            }
        }

        public int getPositiveRoom()
        {
            if (vertical)
            {
                return state.room.roomNumSpace[x + 1, y];
            }
            else
            {
                return state.room.roomNumSpace[x, y + 1];
            }
        }

        public override void draw(Graphics g)
        {
            if (closed)
            {
                g.DrawImage(doorClosed, DrawX, DrawY, state.size * width, state.size * height);
            }
            else
            {
                g.DrawImage(doorOpen, DrawX, DrawY, state.size * width, state.size * height);
            }
        }
    }
}
