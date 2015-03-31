﻿using System;
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

        public int DrawX { get { return (int)(x * state.size + state.form.Width / 2 - state.hero.x * state.size); } }
        public int DrawY { get { return (int)(y * state.size + state.form.Height / 2 - state.hero.y * state.size); } }

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
        private Bitmap img = new Bitmap(Properties.Resources.chest_closed);

        public Pillar(GameState state, int x, int y, int width, int height, int roomNum) : base(state, x, y, width, height, roomNum) { }

        public override void draw(Graphics g)
        { 
            g.FillRectangle(Brushes.Gray, DrawX, DrawY, state.size * width, state.size * height);
            //g.DrawImage(img, DrawX, DrawY, state.size, state.size);
        }
    }

    public class Chest : Obstacle
    {
        private Bitmap img = new Bitmap(Properties.Resources.chest_closed);

        public Chest(GameState state, int x, int y, int width, int height, int roomNum) : base(state, x, y, width, height, roomNum) { }

        public override void draw(Graphics g)
        {
            g.DrawImage(img, DrawX, DrawY, state.size, state.size);
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

        public Stairs(GameState state, int x, int y, int width, int height, int roomNum, bool down, String path, char direction ) : base(state, x, y, width, height, roomNum) {
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
                g.DrawImage(stairDown, DrawX, DrawY, state.size * width, state.size * height);
                //g.FillRectangle(Brushes.IndianRed, DrawX, DrawY, state.size * width, state.size * height);
            }
            else
            {
                g.DrawImage(stairUp, DrawX, DrawY, state.size * width, state.size * height);
                //g.FillRectangle(Brushes.Green, DrawX, DrawY, state.size * width, state.size * height);
            }

            g.DrawString(path.Substring(path.LastIndexOf('\\') + 1), font, Brushes.White, new PointF(DrawX, DrawY - state.size / 2));
        }
    }

    public class Door : Obstacle
    {
        public bool vertical;
        

        public Door(GameState state, int x, int y, int width, int height, int roomNum, bool vertical) : base(state, x,y,width,height, roomNum) {
            this.vertical = vertical;
        }
  
        public override void draw(Graphics g)
        {
            g.FillRectangle(Brushes.Olive, DrawX, DrawY, state.size * width, state.size * height);
        }
    }
}
