using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Obstacle
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2); } }

        public Obstacle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public abstract void draw(Graphics g);
    }

    public class Pillar : Obstacle
    {
        public Pillar(int x, int y, int width, int height) : base(x, y, width, height) { }

        public override void draw(Graphics g) { g.FillRectangle(Brushes.Gray, DrawX, DrawY, G.size * width, G.size * height); }
    }

    public class Stairs : Obstacle
    {
        public bool down;
        public String path;
        public char direction;
        public int xDirection;
        public int yDirection;

        public Stairs(int x, int y, int width, int height, bool down, String path, char direction ) : base(x, y, width, height) {
            this.down = down;
            this.path = path;
            this.direction = direction;

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
                g.FillRectangle(Brushes.IndianRed, DrawX, DrawY, G.size * width, G.size * height);
            }
            else 
            {
                g.FillRectangle(Brushes.Green, DrawX, DrawY, G.size * width, G.size * height);
            }
        }
    }

    public class Door : Obstacle
    {
        public bool vertical;
        

        public Door(int x, int y, int width, int height, bool vertical) : base(x,y,width,height) {
            this.vertical = vertical;
        }
  
        public override void draw(Graphics g)
        {
            g.FillRectangle(Brushes.Olive, DrawX, DrawY, G.size * width, G.size * height);
        }
    }
}
