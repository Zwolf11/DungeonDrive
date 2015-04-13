using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Projectile
    {
        private GameState state;
        public double dmg = 1;
        public double atk_speed = 0.5;
        public double proj_speed = 0.8;
        public int proj_range = 10;
        public GameState.AtkStyle style = GameState.AtkStyle.Basic;
        public double powerSec = 1;
        public double powerFac = 0.3;
        public Bitmap proj_img = Properties.Resources.fire;
        public double x, y;
        public double x_origin, y_origin;
        public double x_speed, y_speed;
        public double radius = 0.3;

        public int frame = 0;
        public Bitmap[] animation = new Bitmap[20];
        public bool isMagic = false;

        public int DrawX { get { return (int)(x * state.size + state.form.ClientSize.Width / 2 - state.hero.x * state.size - state.size * radius); } }
        public int DrawY { get { return (int)(y * state.size + state.form.ClientSize.Height / 2 - state.hero.y * state.size - state.size * radius); } }

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
        public Projectile(GameState state, double x, double y, double x_dir, double y_dir, double proj_speed, int proj_range)
        {
            this.state = state;
            this.x = x;
            this.y = y;
            this.x_origin = x;
            this.y_origin = y;
            this.x_speed = x_dir * proj_speed;
            this.y_speed = y_dir * proj_speed;
            this.proj_range = proj_range;
        }
        public Projectile() { }
        public void setProjectile(GameState state, double x, double y, double x_dir, double y_dir)
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
                    unit.hp -= this.dmg;
                    if (this.style == GameState.AtkStyle.Frozen)
                        unit.slow(this.powerSec, this.powerFac);
                    else if (this.style == GameState.AtkStyle.Flame)
                        unit.burn(this.powerSec, this.powerFac * this.dmg);

                    state.hero.removeProj(this);
                    if (unit.hp <= 0)
                        state.hero.deletingList.Add(unit);
                }
            
            x = xNext;
            y = yNext;
        }

        public void act()
        {
            frame++;
            if (frame >= 20) { frame = 0; }
            tryMove(x + x_speed, y + y_speed);
        }

        public void draw(Graphics g)
        {
            if(!isMagic)
                g.DrawImage(this.proj_img, new Rectangle(DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size)));
            else
                g.DrawImage(this.animation[frame], new Rectangle(DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size)));     
        }
    }


    
}
