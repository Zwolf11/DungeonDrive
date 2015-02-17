using System;
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
            if (stuned) return;

            double xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            double yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            //If bat units are below a certain HP threshold, they will start running from the player
            //Only a basic placeholder for future additions. Eventually, I will add more dynamic behaviors on top of this (ex. bats' escape route will prioritize nearby mobs and then turn on the player
            if (this.hp < 5)
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
}
