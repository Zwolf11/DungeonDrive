using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Projectile
    {
        public int dmg = 1;
        public double x, y;
        private double x_origin, y_origin;
        public double x_speed, y_speed;
        public double speed = 1;
        public double distance = 100;
        public double radius = 0.05;

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size * radius); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size * radius); } }

        public Projectile(double x, double y, double x_dir, double y_dir)
        {
            this.x = x;
            this.y = y;
            this.x_origin = x;
            this.y_origin = y;
        }

        public abstract void act();
        public abstract void draw(Graphics g);

        public void tryMove(double xNext, double yNext)
        {
            if (Math.Sqrt(Math.Pow(x - x_origin, 2) + Math.Pow(y - y_origin, 2)) >= distance)
                G.hero.removeProj(this);

            int left = (int)(xNext - radius);
            int top = (int)(yNext - radius);
            int width = (int)(radius * 2 + (xNext - (int)xNext < radius || 1 - (xNext - (int)xNext) < radius ? 2 : 1));
            int height = (int)(radius * 2 + (yNext - (int)yNext < radius || 1 - (yNext - (int)yNext) < radius ? 2 : 1));

            for (int i = left; i < left + width; i++)
                for (int j = top; j < top + height; j++)
                    if (i < 0 || i >= G.room.width || j < 0 || j >= G.room.height || !G.room.walkingSpace[i, j])
                        G.hero.removeProj(this);

            foreach (Unit unit in G.room.enemies)
                if (Math.Sqrt(Math.Pow(xNext - unit.x, 2) + Math.Pow(yNext - unit.y, 2)) < radius + unit.radius)
                {
                    unit.hp -= this.dmg;
                    this.dmg = 0;
                    G.hero.removeProj(this);
                    if (unit.hp == 0)
                        G.hero.deletingList.Add(unit);
                }

            x = xNext;
            y = yNext;
        }
    }

    public class Arrow : Projectile
    {
        public Arrow(double x, double y, double x_dir, double y_dir)
            : base(x, y, x_dir, y_dir)
        {
            this.dmg = 1;
            this.speed = 0.8;
            this.x_speed = x_dir * speed;
            this.y_speed = y_dir * speed;
            this.distance = 50;
        }

        public override void act()
        {
            tryMove(x + x_speed, y + y_speed);
        }

        public override void draw(Graphics g)
        {
            g.FillEllipse(Brushes.Red, DrawX, DrawY, 5, 5);
        }
    }
}
