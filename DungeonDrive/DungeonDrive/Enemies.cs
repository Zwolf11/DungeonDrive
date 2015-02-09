using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Bat : Unit
    {
        public Bat(double x, double y) : base(x, y)
        {
            this.hp = 5;
            this.atk_dmg = 1;
            this.speed = 0.1;
        }

        public override void act()
        {
            x += Math.Cos(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
            y += Math.Sin(Math.Atan2(G.hero.y - y, G.hero.x - x)) * speed;
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.Red, DrawX, DrawY, G.size, G.size); }
    }

    public class Spider : Unit
    {
        public Spider(double x, double y) : base(x, y)
        {
            this.hp = 2;
            this.atk_dmg = 2;
            this.speed = 0.03;
        }

        public override void act()
        {
            //Mike: Add AI
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, G.size, G.size); }
    }
}
