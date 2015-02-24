using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Bat : Unit
    {
        public Bat(double x, double y) : base(x, y)
        {
            this.full_hp = 30;
            this.hp = full_hp;
            this.atk_dmg = 1;
            this.speed = 0.1;
            this.radius = 0.4;
            this.origin_x = x;
            this.origin_y = y;
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            double xNext;
            double yNext;

            //If bat units are below a certain HP threshold, they will start running from the player
            //Only a basic placeholder for future additions. Eventually, I will add more dynamic behaviors on top of this (ex. bats' escape route will prioritize nearby mobs and then turn on the player
            if (this.hp < 10)
            {
                xNext = x - Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed * 0.6;
                yNext = y - Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed * 0.6;
                tryMove(xNext, yNext);
                return;
            }

            if (Math.Abs(G.hero.x - x) < 7 && Math.Abs(G.hero.y - y) < 7)
            {
                //Player draws aggro from bats if he is close enough
                xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
                tryMove(xNext, yNext);
                return;
            }
            else
            {
                //Player is far away enough to drop aggro
                if (this.x == this.origin_x || this.y == this.origin_y)
                {
                    //Original position has been reacehd, do not move
                    return;
                }
                else
                {
                    //Move towards original position
                    xNext = x + Math.Cos(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                    yNext = y + Math.Sin(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                    tryMove(xNext, yNext);
                    return;
                }
            }

            //tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.FillEllipse(Brushes.Red, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size));
            g.FillRectangle(this.hp <= 0.4 * this.full_hp ? Brushes.Red : Brushes.Green, DrawX, DrawY - 5, (int)(radius * 2 * G.size * this.hp / this.full_hp), 2);
        }
    }

    public class Spider : Unit
    {
        public Spider(double x, double y) : base(x, y)
        {
            this.full_hp = 15;
            this.hp = full_hp;
            this.atk_dmg = 2;
            this.speed = 0.03;
            this.radius = 0.4;
            this.origin_x = x;
            this.origin_y = y;
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            double xNext;
            double yNext;

            if (Math.Abs(G.hero.x - x) < 7 && Math.Abs(G.hero.y - y) < 7)
            {
                //Player draws aggro from bats if he is close enough
                xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
                tryMove(xNext, yNext);
            }
            else
            {
                //Player is far away enough to drop aggro
                if (this.x == this.origin_x || this.y == this.origin_y)
                {
                    //Original position has been reacehd, do not move
                    return;
                }
                else
                {
                    //Move towards original position
                    xNext = x + Math.Cos(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                    yNext = y + Math.Sin(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                    tryMove(xNext, yNext);
                }
            }

            //double xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            //double yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            if ((G.hero.x - x) < 5 && (G.hero.y - y) < 5)
                this.speed = 0.08;
            else
                this.speed = 0.03;

            //tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size));
            g.FillRectangle(this.hp <= 0.4 * this.full_hp ? Brushes.Red : Brushes.Green, DrawX, DrawY - 5, (int)(radius * 2 * G.size * this.hp / this.full_hp), 2);
        }
    }

    public class Boss : Unit
    {
        public Boss(double x, double y)
            : base(x, y)
        {
            this.full_hp = 200;
            this.hp = full_hp;
            this.atk_dmg = 5;
            this.speed = 0.02;
            this.radius = 0.6;
            this.teleport = true;
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            double xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            double yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            if (Math.Abs(G.hero.x - x) < 5 && Math.Abs(G.hero.y - y) < 5 && this.teleport)
            {
                this.teleport = false;

                //If hero is 5 units close to the boss, have the boss teleport behind the player

                if ((G.hero.x - x) < 0 && (G.hero.y - y) < 0)
                {
                    //Boss is in bottom-right direction with respect to hero
                    x = G.hero.x - 0.5;
                    y = G.hero.y - 0.5;
                    return;
                }
                else if ((G.hero.x - x) < 0 && (G.hero.y - y) > 0)
                {
                    //Boss is in upper-right direction with respect to hero
                    x = G.hero.x - 0.5;
                    y = G.hero.y + 0.5;
                    return;
                }
                else if ((G.hero.x - x) > 0 && (G.hero.y - y) < 0)
                {
                    //Boss is in bottom-left direction with respect to hero
                    x = G.hero.x + 0.5;
                    y = G.hero.y - 0.5;
                    return;
                }
                else if ((G.hero.x - x) > 0 && (G.hero.y - y) > 0)
                {
                    //Boss is in upper-left direction with respect to hero
                    x = G.hero.x + 0.5;
                    y = G.hero.y + 0.5;
                    return;
                }
                else if ((G.hero.x - x) == 0 && (G.hero.y - y) < 0)
                {
                    //Boss is directly south of the player
                    y = G.hero.y - 0.5;
                    return;
                }
                else if ((G.hero.x - x) == 0 && (G.hero.y - y) > 0)
                {
                    //Boss is directly north of the player
                    y = G.hero.y + 0.5;
                    return;
                }
                else if ((G.hero.x - x) < 0 && (G.hero.y - y) == 0)
                {
                    //Boss is directly east of the player
                    x = G.hero.x - 0.5;
                    return;
                }
                else if ((G.hero.x - x) > 0 && (G.hero.y - y) == 0)
                {
                    //Boss is directly west of the player
                    x = G.hero.x + 0.5;
                    return;
                }
            }
            else
                this.speed = 0.02;

            tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.FillEllipse(Brushes.Black, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size));
            g.FillRectangle(this.hp <= 0.4 * this.full_hp ? Brushes.Red : Brushes.Green, DrawX, DrawY - 5, (int)(radius * 2 * G.size * this.hp / this.full_hp), 2);
        }
    }
}
