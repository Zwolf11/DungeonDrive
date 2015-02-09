using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Unit
    {
        public double x;
        public double y;
        public double speed = 0.01;
        public int hp = 1;
        public int atk_dmg = 1;

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2); } }

        public Unit(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public abstract void act();
        public abstract void draw(Graphics g);
    }
}
