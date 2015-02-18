﻿using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Bat : Unit
    {
        public Bat(double x, double y) : base(x, y)
        {
            this.hp = 30;
            this.atk_dmg = 1;
            this.speed = 0.1;
            this.radius = 0.4;
        }

        public override void act()
        {
            if (knockback)
            {
                if (Math.Round(Math.Abs(x_final - x), 6) <= Math.Abs(x_dist) && Math.Round(Math.Abs(y_final - y), 6) <= Math.Abs(y_dist) ) 
                    //&& tryMove(x + x_dist, y + y_dist))
                {
                    x += x_dist;
                    y += y_dist;
                    return;
                }
                else
                    knockback = false;
            }

            if (stuned) return;

            double xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            double yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            //If bat units are below a certain HP threshold, they will start running from the player
            //Only a basic placeholder for future additions. Eventually, I will add more dynamic behaviors on top of this (ex. bats' escape route will prioritize nearby mobs and then turn on the player
            if (this.hp < 10)
            {
                xNext = x - Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed * 0.6;
                yNext = y - Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed * 0.6;
            }

            tryMove(xNext, yNext);
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.Red, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size)); }
    }

    public class Spider : Unit
    {
        public Spider(double x, double y) : base(x, y)
        {
            this.hp = 15;
            this.atk_dmg = 2;
            this.speed = 0.03;
            this.radius = 0.4;
        }

        public override void act()
        {
            if (knockback)
            {
                if (Math.Round(Math.Abs(x_final - x), 6) <= Math.Abs(x_dist) && Math.Round(Math.Abs(y_final - y), 6) <= Math.Abs(y_dist) )
                //&& tryMove(x + x_dist, y + y_dist))
                {
                    x += x_dist;
                    y += y_dist;
                    return;
                }
                else
                    knockback = false;
            }

            if (stuned) return;

            double xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            double yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            if ((G.hero.x - x) < 5 && (G.hero.y - y) < 5)
                this.speed = 0.08;
            else
                this.speed = 0.03;

            tryMove(xNext, yNext);
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size)); }
    }

    public class Boss : Unit
    {
        public Boss(double x, double y)
            : base(x, y)
        {
            this.hp = 200;
            this.atk_dmg = 5;
            this.speed = 0.02;
            this.radius = 0.6;
            this.teleport = true;
        }

        public override void act()
        {
            if (knockback)
            {
                if (Math.Round(Math.Abs(x_final - x), 6) <= Math.Abs(x_dist) && Math.Round(Math.Abs(y_final - y), 6) <= Math.Abs(y_dist))
                //&& tryMove(x + x_dist, y + y_dist))
                {
                    x += x_dist/2;
                    y += y_dist/2;
                    return;
                }
                else
                    knockback = false;
            }

            if (stuned) return;

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

        public override void draw(Graphics g) { g.FillEllipse(Brushes.Black, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size)); }
    }
}
