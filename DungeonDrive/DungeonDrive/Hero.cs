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
        private double xNext;
        private double yNext;

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
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    changeFacing('A');
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;

                    if (checkCollision(xNext, yNext))
                    {
                        x = xNext;
                        y = yNext;
                    }
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    changeFacing('D');
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;

                    if (checkCollision(xNext, yNext))
                    {
                        x = xNext;
                        y = yNext;
                    }
                }
                else
                {
                    changeFacing('W');
                    yNext = y - speed;

                    if (checkCollision(x, yNext))
                        y = yNext;
                }
            }
            else if (G.keys.ContainsKey(Keys.S) && !G.keys.ContainsKey(Keys.W))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    changeFacing('A');
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                    
                    if (checkCollision(xNext, yNext))
                    {
                        x = xNext;
                        y = yNext;
                    }
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    changeFacing('D');
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;

                    if (checkCollision(xNext, yNext))
                    {
                        x = xNext;
                        y = yNext;
                    }
                }
                else
                {
                    changeFacing('S');
                    yNext = y + speed;

                    if (checkCollision(x, yNext))
                        y = yNext;
                }
            }
            else if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
            {
                changeFacing('A');
                xNext = x - speed;
                
                if (checkCollision(xNext, y))
                    x = xNext;
            }
            else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
            {
                changeFacing('D');
                xNext = x + speed;

                if (checkCollision(xNext, y))
                    x = xNext;
            }
        }

        private void handleAttacking()
        {
            //Siming: Add code for when the player is attacking (Also, when he hits, make the enemies get knocked back)
            if (G.room.enemies.Count == 0)
                return;

            List<Unit> deletingList = new List<Unit>();

            // basic attack
            if (G.keys.ContainsKey(Keys.J))
            {
                foreach (Unit enemy in G.room.enemies)
                {
                    if (((enemy.x - x) * curxFacing > 0 || (enemy.y - y) * curyFacing > 0) && Math.Abs(enemy.x - x) < 1.5 && Math.Abs(enemy.y - y) < 1.2)
                    {
                        enemy.x += curxFacing * 0.2;
                        enemy.y += curyFacing * 0.2;
                        enemy.hp -= atk_dmg;
                        if (enemy.hp <= 0)
                            deletingList.Add(enemy);
                    }
                }
            }

            // iterate through current enemy list, find the enemy in the currect direction and distance, knock back
            if (G.keys.ContainsKey(Keys.K))
            {       
                foreach (Unit enemy in G.room.enemies)
                {
                    if (((enemy.x - x) * curxFacing > 0 || (enemy.y - y) * curyFacing > 0) && Math.Abs(enemy.x - x) < 1.5 && Math.Abs(enemy.y - y) < 1.5)
                    {
                        enemy.x += curxFacing;
                        enemy.y += curyFacing;
                        enemy.hp -= atk_dmg * 1.5;
                        if (enemy.hp <= 0)
                            deletingList.Add(enemy);
                    }
                }
            }

            foreach (Unit deletingEnemy in deletingList)
                G.room.enemies.Remove(deletingEnemy);
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
