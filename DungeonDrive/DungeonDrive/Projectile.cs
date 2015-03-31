using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Projectile
    {
        private GameState state;
        public static int dmg = 1;
        public static double atk_speed = 0.5;
        public static double proj_speed = 0.8;
        public static int proj_range = 10;
        public static AtkStyle style = AtkStyle.Basic;
        public static double slowSec = 1;
        public static double slowFac = 0.3;

        public double x, y;
        public double x_origin, y_origin;
        public double x_speed, y_speed;
        public double radius = 0.3;

        public int DrawX { get { return (int)(x * state.size + state.form.Width / 2 - state.hero.x * state.size - state.size * radius); } }
        public int DrawY { get { return (int)(y * state.size + state.form.Height / 2 - state.hero.y * state.size - state.size * radius); } }

        public enum AtkStyle
        {
            Basic,
            Flame,
            Frozen
        }

        public Projectile(GameState state, double x, double y, double x_dir, double y_dir)
        {
            this.state = state;
            this.x = x;
            this.y = y;
            this.x_origin = x;
            this.y_origin = y;
            this.x_speed = x_dir * proj_speed;
            this.y_speed = y_dir * proj_speed;
        }

        public void tryMove(double xNext, double yNext)
        {
            if (Math.Sqrt(Math.Pow(x - x_origin, 2) + Math.Pow(y - y_origin, 2)) >= proj_range)
                state.hero.removeProj(this);

            int left = (int)(xNext - radius);
            int top = (int)(yNext - radius);
            int width = (int)(radius * 2 + (xNext - (int)xNext < radius || 1 - (xNext - (int)xNext) < radius ? 2 : 1));
            int height = (int)(radius * 2 + (yNext - (int)yNext < radius || 1 - (yNext - (int)yNext) < radius ? 2 : 1));

            for (int i = left; i < left + width; i++)
                for (int j = top; j < top + height; j++)
                    if (i < 0 || i >= state.room.width || j < 0 || j >= state.room.height || !state.room.walkingSpace[i, j])
                        state.hero.removeProj(this);
            
            foreach (Unit unit in state.room.enemies)
                if (Math.Sqrt(Math.Pow(xNext - unit.x, 2) + Math.Pow(yNext - unit.y, 2)) < radius + unit.radius)
                {
                    unit.hp -= Projectile.dmg;
                    if (Projectile.style == AtkStyle.Frozen)
                    {
                        Console.WriteLine("Frozen");
                        unit.slow(Projectile.slowSec, Projectile.slowFac);
                    }
                    state.hero.removeProj(this);
                    if (unit.hp <= 0)
                        state.hero.deletingList.Add(unit);
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
            g.DrawImage(Properties.Resources.fire, new Rectangle(DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size)));
        }
    }
}
