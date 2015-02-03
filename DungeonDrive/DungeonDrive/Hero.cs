using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class Hero : Unit
    {
        private bool attacking = false;

        public Hero(double x, double y, double speed) : base(x, y, speed) { }

        private void handleMovement()
        {
            if (G.keys.ContainsKey(Keys.W) && !G.keys.ContainsKey(Keys.S))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    G.hero.x -= Math.Sqrt(2) / 2 * G.hero.speed;
                    G.hero.y -= Math.Sqrt(2) / 2 * G.hero.speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    G.hero.x += Math.Sqrt(2) / 2 * G.hero.speed;
                    G.hero.y -= Math.Sqrt(2) / 2 * G.hero.speed;
                }
                else
                {
                    G.hero.y -= G.hero.speed;
                }
            }
            else if (G.keys.ContainsKey(Keys.S) && !G.keys.ContainsKey(Keys.W))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    G.hero.x -= Math.Sqrt(2) / 2 * G.hero.speed;
                    G.hero.y += Math.Sqrt(2) / 2 * G.hero.speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    G.hero.x += Math.Sqrt(2) / 2 * G.hero.speed;
                    G.hero.y += Math.Sqrt(2) / 2 * G.hero.speed;
                }
                else
                {
                    G.hero.y += G.hero.speed;
                }
            }
            else if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
            {
                G.hero.x -= G.hero.speed;
            }
            else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
            {
                G.hero.x += G.hero.speed;
            }
        }

        private void handleAttacking()
        {
            //Siming: Add code for when the player is attacking (Also, when he hits, make the enemies get knocked back)
        }

        public override void act()
        {
            if(!attacking)
                handleMovement();
            else
                handleAttacking();
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.RoyalBlue, G.width / 2 - G.size / 2, G.height / 2 - G.size / 2, G.size, G.size); }
    }
}
