using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Unit
    {
        public double x;
        public double y;
        public double speed;

        public Unit(double x, double y, double speed)
        {
            this.x = x;
            this.y = y;
            this.speed = speed;
        }

        public abstract void act();
        public abstract void draw(Graphics g);
    }
}
