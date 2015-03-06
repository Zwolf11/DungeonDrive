using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Projectile
    {
        public static int dmg = 1;
        public static double atk_speed = 0.5;
        public static double speed = 0.8;
        public static double range = 10;
        public static AtkStyle style = AtkStyle.Basic;
        public static double slowSec = 1;
        public static double slowFac = 0.3;

        public double x, y;
        public double x_origin, y_origin;
        public double x_speed, y_speed;
        public double radius = 0.3;

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size * radius); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size * radius); } }

        public enum AtkStyle
        {
            Basic,
            Flame,
            Frozen
        }

        public Projectile(double x, double y, double x_dir, double y_dir)
        {
            this.x = x;
            this.y = y;
            this.x_origin = x;
            this.y_origin = y;
            this.x_speed = x_dir * speed;
            this.y_speed = y_dir * speed;
        }

        public void tryMove(double xNext, double yNext)
        {
            if (Math.Sqrt(Math.Pow(x - x_origin, 2) + Math.Pow(y - y_origin, 2)) >= range)
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
                    unit.hp -= Projectile.dmg;
                    if (Projectile.style == AtkStyle.Frozen)
                    {
                        Console.WriteLine("Frozen");
                        unit.slow(Projectile.slowSec, Projectile.slowFac);
                    }
                    G.hero.removeProj(this);
                    if (unit.hp <= 0)
                        G.hero.deletingList.Add(unit);
                }
            
            x = xNext;
            y = yNext;
        }

        public void act()
        {
            tryMove(x + x_speed, y + y_speed);
        }

        public void draw(Graphics g)
        {
            g.DrawImage(G.proj_img, new Rectangle(DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size)));
        }
    }
}
