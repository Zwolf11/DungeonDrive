using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.IO;

namespace DungeonDrive
{
    public class Hero : Unit
    {
        new public int DrawX { get { return (int)(G.width / 2 - G.size * radius); } }
        new public int DrawY { get { return (int)(G.height / 2 - G.size * radius); } }

        private int curxFacing = 0;
        private int curyFacing = 0;

        private SoundPlayer attack1 = new SoundPlayer(@"attack1.wav");
        private SoundPlayer attack2 = new SoundPlayer(@"attack2.wav");
        private Image hero_imageS = Image.FromFile(@"heroS.png");
        private Image hero_imageW = Image.FromFile(@"heroW.png");
        private Image hero_imageA = Image.FromFile(@"heroA.png");
        private Image hero_imageD = Image.FromFile(@"heroD.png");
        private Image hero_image;

        public Hero(double x, double y) : base(x, y)
        {
            this.hp = 10;
            this.atk_dmg = 2;
            this.atk_speed = 1;
            this.speed = 0.3;
            this.radius = 0.49;
            hero_image = hero_imageS;
        }

        private void handleMovement()
        {
            double xNext = x;
            double yNext = y;

            if (G.keys.ContainsKey(Keys.W) && !G.keys.ContainsKey(Keys.S))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    hero_image = hero_imageA;
                    changeFacing('A');
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    hero_image = hero_imageD;
                    changeFacing('D');
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;
                }
                else
                {
                    hero_image = hero_imageW;
                    changeFacing('W');
                    yNext = y - speed;
                }
            }
            else if (G.keys.ContainsKey(Keys.S) && !G.keys.ContainsKey(Keys.W))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    hero_image = hero_imageA;
                    changeFacing('A');
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    hero_image = hero_imageD;
                    changeFacing('D');
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                }
                else
                {
                    hero_image = hero_imageS;
                    changeFacing('S');
                    yNext = y + speed;
                }
            }
            else if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
            {
                hero_image = hero_imageA;
                changeFacing('A');
                xNext = x - speed;
            }
            else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
            {
                hero_image = hero_imageD;
                changeFacing('D');
                xNext = x + speed;
            }

            tryMove(xNext, yNext);
        }

        private void knockBack(Unit enemy, double x_dist, double y_dist)
        {
            enemy.x_dist = x_dist;
            enemy.y_dist = y_dist;
            enemy.x_final = enemy.x + enemy.x_dist;
            enemy.y_final = enemy.y + enemy.y_dist;
            enemy.knockback = true;
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
                if (atk_cd[0])
                {
                    foreach (Unit enemy in G.room.enemies)
                    {
                        if (((enemy.x - x) * curxFacing > 0 || (enemy.y - y) * curyFacing > 0) && Math.Abs(enemy.x - x) < 1.2 && Math.Abs(enemy.y - y) < 1.2)
                        {
                            try {attack1.Play();} catch (FileNotFoundException e) {}
                            knockBack(enemy, curxFacing * 0.2, curyFacing * 0.2);
                            enemy.hp -= atk_dmg;
                            if (enemy.hp <= 0)
                                deletingList.Add(enemy);
                        }
                    }
                    cd(atk_speed, 0);
                }
            }

            // iterate through current enemy list, find the enemy in the currect direction and distance, knock back
            if (G.keys.ContainsKey(Keys.K))
            {       
                if (atk_cd[1])
                {
                    foreach (Unit enemy in G.room.enemies)
                    {
                        if (((enemy.x - x) * curxFacing > 0 || (enemy.y - y) * curyFacing > 0) && Math.Abs(enemy.x - x) < 1.5 && Math.Abs(enemy.y - y) < 1.5)
                        {
                            try {attack2.Play();} catch (FileNotFoundException e) {}
                            knockBack(enemy, curxFacing, curyFacing);
                            enemy.hp -= atk_dmg * 1.5;
                            if (enemy.hp <= 0)
                                deletingList.Add(enemy);
                        }
                    }
                    cd(2, 1);
                }
            }

            foreach (Unit deletingEnemy in deletingList)
                G.room.enemies.Remove(deletingEnemy);
        }

        public void changeFacing(char direction)
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

        public override void draw(Graphics g)
        { 
            //g.FillEllipse(Brushes.RoyalBlue, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size));
            g.DrawImage(hero_image, new Point(DrawX, DrawY));
        }
    }
}
