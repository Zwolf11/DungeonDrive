using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace DungeonDrive
{
    public class Hero : Unit
    {
        private int curxFacing = 0;
        private int curyFacing = 0;
        private bool attacking = false;

        public Hero(double x, double y, double speed) : base(x, y, speed) {
            this.hp = 10;
            this.atk_dmg = 2;
        }

        private void handleMovement()
        {
            if (G.keys.ContainsKey(Keys.W) && !G.keys.ContainsKey(Keys.S))
            {
                changeFacing('W');
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    changeFacing('A');
                    G.hero.x -= Math.Sqrt(2) / 2 * G.hero.speed;
                    G.hero.y -= Math.Sqrt(2) / 2 * G.hero.speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    changeFacing('D');
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
                changeFacing('S');
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    changeFacing('A');
                    G.hero.x -= Math.Sqrt(2) / 2 * G.hero.speed;
                    G.hero.y += Math.Sqrt(2) / 2 * G.hero.speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    changeFacing('D');
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
                changeFacing('A');
                G.hero.x -= G.hero.speed;
            }
            else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
            {
                changeFacing('D');
                G.hero.x += G.hero.speed;
            }
        }

        private void handleAttacking()
        {
            //Siming: Add code for when the player is attacking (Also, when he hits, make the enemies get knocked back)
            if (G.room.enemies.Count == 0)
                return;

            // iterate through current enemy list, find the enemy in the currect direction and distance, knock back
            if (G.keys.ContainsKey(Keys.J))
            {
                List<Unit> deletingList = new List<Unit>();
                    
                foreach (Unit enemy in G.room.enemies)
                {
                    if (((enemy.x - G.hero.x) * G.hero.curxFacing > 0 || (enemy.y - G.hero.y) * G.hero.curyFacing > 0) && Math.Abs(enemy.x - G.hero.x) < 1.5 && Math.Abs(enemy.y - G.hero.y) < 1.5 )
                    {
                        enemy.x += G.hero.curxFacing;
                        enemy.y += G.hero.curyFacing;
                        enemy.hp -= G.hero.atk_dmg;
                        if (enemy.hp <= 0)
                            deletingList.Add(enemy);
                    }
                }

                foreach (Unit deletingEnemy in deletingList)
                    G.room.enemies.Remove(deletingEnemy);
            }
        }

        private void changeFacing(char direction)
        {
            G.hero.curxFacing = 0;
            G.hero.curyFacing = 0;
            switch (direction)
            {
                case 'W':
                    G.hero.curyFacing = -1;
                    break;
                case 'S':
                    G.hero.curyFacing = 1;
                    break;
                case 'A':
                    G.hero.curxFacing = -1;
                    break;
                case 'D':
                    G.hero.curxFacing = 1;
                    break;
            }
        }

        public override void act()
        {
            handleMovement();
            handleAttacking();
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.RoyalBlue, G.width / 2 - G.size / 2, G.height / 2 - G.size / 2, G.size, G.size); }
    }
}
