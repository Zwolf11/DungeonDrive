﻿using System;
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
        public Item.AtkStyle style = Item.AtkStyle.Basic;
        public double powerSec = 1;
        public double powerFac = 0.3;
        public Bitmap proj_img = Properties.Resources.fire;
        public double x, y;
        public double x_origin, y_origin;
        public double x_speed, y_speed;
        public double radius = 0.3;
        public bool friendlyFire = true;
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
        public virtual void tryMove(double xNext, double yNext)
        {
            if (Math.Sqrt(Math.Pow(x - x_origin, 2) + Math.Pow(y - y_origin, 2)) >= proj_range)
                endingEffect();

            int left = (int)(xNext - radius);
            int top = (int)(yNext - radius);
            int width = (int)(radius * 2 + (xNext - (int)xNext < radius || 1 - (xNext - (int)xNext) < radius ? 2 : 1));
            int height = (int)(radius * 2 + (yNext - (int)yNext < radius || 1 - (yNext - (int)yNext) < radius ? 2 : 1));

            for (int i = left; i < left + width; i++)
                for (int j = top; j < top + height; j++)
                    if (i < 0 || i >= state.room.width || j < 0 || j >= state.room.height || !state.room.walkingSpace[i, j])
                        endingEffect();

            foreach (Unit unit in state.room.enemies)
                if (Math.Sqrt(Math.Pow(xNext - unit.x, 2) + Math.Pow(yNext - unit.y, 2)) < radius + unit.radius)
                {
                    unit.hp -= this.dmg;
                    if (this.style == Item.AtkStyle.Frozen)
                        unit.slow(this.powerSec, this.powerFac);
                    else if (this.style == Item.AtkStyle.Flame)
                        unit.burn(this.powerSec, this.powerFac * this.dmg);

                    endingEffect();
                    if (unit.hp <= 0)
                        state.hero.deletingList.Add(unit);

                    state.hero.inCombat = true;
                    state.hero.combatCd = 3 * 17;
                }

            x = xNext;
            y = yNext;
        }

        public virtual void endingEffect()
        {
            state.room.removeProj(this);
        }

        public virtual void act()
        {
            frame++;
            if (frame >= 20) { frame = 0; }

            tryMove(x + x_speed, y + y_speed);


        }

        public void draw(Graphics g)
        {
            if (!isMagic)
                g.DrawImage(this.proj_img, new Rectangle(DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size)));
            else
                g.DrawImage(this.animation[frame], new Rectangle(DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size)));
        }
    }


    public class explodingProjectiles : Projectile
    {
        private GameState state;
        private int timer = 0;
        public explodingProjectiles(GameState state, double x, double y, double x_dir, double y_dir, double proj_speed, int proj_range)
            : base(state, x, y, x_dir, y_dir, proj_speed, proj_range)
        {
            this.state = state;
        }

        public override void endingEffect()
        {
            this.x_speed = 0;
            this.y_speed = 0;
        }



    }

    public class enemyProjectile : Projectile
    {
        private GameState state;
        public enemyProjectile(GameState state, double x, double y, double x_dir, double y_dir, double proj_speed, int proj_range)
            : base(state, x, y, x_dir, y_dir, proj_speed, proj_range)
        {
            this.state = state;
        }
        public override void tryMove(double xNext, double yNext)
        {
            if (Math.Sqrt(Math.Pow(x - x_origin, 2) + Math.Pow(y - y_origin, 2)) >= proj_range)
                endingEffect();

            int left = (int)(xNext - radius);
            int top = (int)(yNext - radius);
            int width = (int)(radius * 2 + (xNext - (int)xNext < radius || 1 - (xNext - (int)xNext) < radius ? 2 : 1));
            int height = (int)(radius * 2 + (yNext - (int)yNext < radius || 1 - (yNext - (int)yNext) < radius ? 2 : 1));

            for (int i = left; i < left + width; i++)
                for (int j = top; j < top + height; j++)
                    if (i < 0 || i >= state.room.width || j < 0 || j >= state.room.height || !state.room.walkingSpace[i, j])
                        endingEffect();


            if (Math.Sqrt(Math.Pow(xNext - state.hero.x, 2) + Math.Pow(yNext - state.hero.y, 2)) < radius + state.hero.radius)
            {
                state.hero.hp -= this.dmg;
                if (this.style == Item.AtkStyle.Frozen)
                    state.hero.slow(this.powerSec, this.powerFac);
                else if (this.style == Item.AtkStyle.Flame)
                    state.hero.burn(this.powerSec, this.powerFac * this.dmg);

                endingEffect();
                if (state.hero.hp <= 0)
                {
                    state.addChildState(new GameOverState(state.form), false, true);
                    return;
                }
            }

            x = xNext;
            y = yNext;
        }

    }

}