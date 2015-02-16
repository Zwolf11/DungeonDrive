using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Bat : Unit
    {
        private double xNext;
        private double yNext;

        public Bat(double x, double y)
            : base(x, y)
        {
            this.hp = 30;
            this.atk_dmg = 1;
            this.speed = 0.1;
        }

        public override void act()
        {
            if (stuned) return;

            xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            //If bat units are below a certain HP threshold, they will start running from the player
            //Only a basic placeholder for future additions. Eventually, I will add more dynamic behaviors on top of this (ex. bats' escape route will prioritize nearby mobs and then turn on the player
            if (this.hp < 5)
            {
                xNext = x - Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed * 0.6;
                yNext = y - Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed * 0.6;
            }

            if (checkCollision(xNext, yNext))
            {
                x = xNext;
                y = yNext;
            }
            else
            {
                while (!checkCollision(x, y))
                {
                    x -= G.rnd.Next(-1, 2) * speed;
                    y -= G.rnd.Next(-1, 2) * speed;
                }
            }
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.Red, DrawX, DrawY, G.size, G.size); }
    }

    public class Spider : Unit
    {
        private double xNext;
        private double yNext;

        public Spider(double x, double y)
            : base(x, y)
        {
            this.hp = 15;
            this.atk_dmg = 2;
            this.speed = 0.03;
        }

        public override void act()
        {
            if (stuned) return;

            //Mike: Add AI
            xNext = x + Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            yNext = y + Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;

            if ((G.hero.x - x) < 5 && (G.hero.y - y) < 5)
            {
                this.speed = 0.08;
            }
            else
                this.speed = 0.03;

            if (checkCollision(xNext, yNext))
            {
                x = xNext;
                y = yNext;
            }
            else
            {
                while (!checkCollision(x, y))
                {
                    x -= G.rnd.Next(-1, 2) * speed;
                    y -= G.rnd.Next(-1, 2) * speed;
                }
            }
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, G.size, G.size); }
    }
}
