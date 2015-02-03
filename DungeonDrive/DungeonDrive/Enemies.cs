using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Bat : Unit
    {
        public Bat(double x, double y, double speed) : base(x, y, speed) { }

        public override void act()
        {
            //Mike: Fix this AI
            x -= Math.Cos(G.hero.x - x) * speed;
            y += Math.Sin(G.hero.y - y) * speed;
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.Red, (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size); }
        public override void drawTouching(Graphics g) { g.FillEllipse(Brushes.Green, (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size); }
    }

    public class Spider : Unit
    {
        public Spider(double x, double y, double speed) : base(x, y, speed) { }

        public override void act()
        {
            //Mike: Add AI
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.Brown, (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size); }
        public override void drawTouching(Graphics g) { g.FillEllipse(Brushes.Green, (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size); }
    }
}
