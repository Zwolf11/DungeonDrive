using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace DungeonDrive
{
    public class Hero : Unit
    {
        new public int DrawX { get { return G.width / 2 - G.size / 2; } }
        new public int DrawY { get { return G.height / 2 - G.size / 2; } }

        private int curxFacing = 0;
        private int curyFacing = 0;

        public Hero(double x, double y) : base(x, y)
        {
            this.hp = 10;
            this.atk_dmg = 2;
            this.speed = 0.3;
        }

        private void handleMovement()
        {
            if (G.keys.ContainsKey(Keys.W) && !G.keys.ContainsKey(Keys.S))
            {
                changeFacing('W');
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    changeFacing('A');
                    x -= Math.Sqrt(2) / 2 * speed;
                    y -= Math.Sqrt(2) / 2 * speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    changeFacing('D');
                    x += Math.Sqrt(2) / 2 * speed;
                    y -= Math.Sqrt(2) / 2 * speed;
                }
                else
                {
                    y -= speed;
                }
            }
            else if (G.keys.ContainsKey(Keys.S) && !G.keys.ContainsKey(Keys.W))
            {
                changeFacing('S');
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    changeFacing('A');
                    x -= Math.Sqrt(2) / 2 * speed;
                    y += Math.Sqrt(2) / 2 * speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    changeFacing('D');
                    x += Math.Sqrt(2) / 2 * speed;
                    y += Math.Sqrt(2) / 2 * speed;
                }
                else
                {
                    y += speed;
                }
            }
            else if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
            {
                changeFacing('A');
                x -= speed;
            }
            else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
            {
                changeFacing('D');
                x += speed;
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
                    if (((enemy.x - x) * curxFacing > 0 || (enemy.y - y) * curyFacing > 0) && Math.Abs(enemy.x - x) < 1.5 && Math.Abs(enemy.y - y) < 1.5 )
                    {
                        enemy.x += curxFacing;
                        enemy.y += curyFacing;
                        enemy.hp -= atk_dmg;
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
            curxFacing = 0;
            curyFacing = 0;
            switch (direction)
            {
                case 'W':
                    curyFacing = -1;
                    break;
                case 'S':
                    curyFacing = 1;
                    break;
                case 'A':
                    curxFacing = -1;
                    break;
                case 'D':
                    curxFacing = 1;
                    break;
            }
        }

        public override void act()
        {
            handleMovement();
            handleAttacking();
        }

        public override void draw(Graphics g) { g.FillEllipse(Brushes.RoyalBlue, DrawX, DrawY, G.size, G.size); }
    }
}
