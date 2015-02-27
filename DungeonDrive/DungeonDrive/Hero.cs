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

        public List<Projectile> projectiles = new List<Projectile>();
        private List<Projectile> deletingProj = new List<Projectile>();
        public List<Unit> deletingList = new List<Unit>();

        private SoundPlayer attack1;
        private SoundPlayer attack2;
        private SoundPlayer attack3;

        private float dir = 0;
        private bool shooting = false;
        public bool alive = true;

        public Hero(double x, double y)
            : base(x, y)
        {
            this.hp = 20;
            this.full_hp = hp;
            this.atk_dmg = 2;
            this.atk_speed = 0.2;
            this.speed = 0.3;
            this.radius = 0.49;
            try
            {
                attack1 = new SoundPlayer(@"attack1.wav");
                attack2 = new SoundPlayer(@"attack2.wav");
                attack3 = new SoundPlayer(@"attack3.wav");
            }
            catch (FileNotFoundException) { }
        }

        private void handleMovement()
        {
            if (knockback)
                knockBacked();

            // get cursor dir
            dir = (float)Math.Atan2(Cursor.Position.Y - (G.height / 2), Cursor.Position.X - (G.width / 2));

            double xNext = x;
            double yNext = y;

            if (G.keys.ContainsKey(Keys.W) && !G.keys.ContainsKey(Keys.S))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;
                }
                else
                    yNext = y - speed;
            }
            else if (G.keys.ContainsKey(Keys.S) && !G.keys.ContainsKey(Keys.W))
            {
                if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                {
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                }
                else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                {
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                }
                else
                    yNext = y + speed;
            }
            else if (G.keys.ContainsKey(Keys.A) && !G.keys.ContainsKey(Keys.D))
                xNext = x - speed;
            else if (G.keys.ContainsKey(Keys.D) && !G.keys.ContainsKey(Keys.A))
                xNext = x + speed;

            tryMove(xNext, yNext);
        }

        private void handleAttacking()
        {
            // toggle melee/projectiles
            if (G.keys.ContainsKey(Keys.T))
            {
                if (atk_cd[4])
                {
                    shooting = !shooting;
                    cd(1, 4);
                }
            }

            // knockback skill
            if (G.keys.ContainsKey(Keys.E))
            {
                if (atk_cd[1])
                {
                    foreach (Unit enemy in G.room.enemies)
                    {
                        if (Math.Abs(enemy.x - x) < 1.2 && Math.Abs(enemy.y - y) < 1.2)
                        {
                            try { attack2.Play(); }
                            catch (FileNotFoundException) { }
                            double factor = 1 / Math.Sqrt(Math.Pow(enemy.x - x, 2) + Math.Pow(enemy.y - y, 2));
                            knockBack(enemy, (enemy.x - x) * factor, (enemy.y - y) * factor, 0.4);
                            enemy.hp -= 2 + 0.8 *atk_dmg;
                            if (enemy.hp <= 0)
                                deletingList.Add(enemy);
                        }
                    }
                    cd(2, 1);
                }
            }

            if (deletingList.Count > 0)
            {
                foreach (Unit deletingEnemy in deletingList)
                    G.room.enemies.Remove(deletingEnemy);
                deletingList.Clear();
            }

            if (deletingProj.Count > 0)
            {
                foreach (Projectile proj in deletingProj)
                    projectiles.Remove(proj);
                deletingProj.Clear();
            }
        }


        public void basicAtk()
        {
            // melee
            if (!shooting && atk_cd[0])
            {
                foreach (Unit enemy in G.room.enemies)
                {
                    if (Math.Abs(enemy.x - (Math.Cos(dir) * 2 + x)) < 2 && Math.Abs(enemy.y - (Math.Sin(dir) * 2 + y)) < 2 && Math.Abs(enemy.x - x) < 1.05 && Math.Abs(enemy.y - y) < 1.05)
                    {
                        try { attack1.Play(); }
                        catch (FileNotFoundException) { }
                        knockBack(enemy, Math.Cos((double)dir) * 0.5, Math.Sin((double)dir) * 0.5, 0);
                        enemy.hp -= atk_dmg;
                        if (enemy.hp <= 0)
                            deletingList.Add(enemy);
                    }
                }
                cd(atk_speed, 0);
            }

            // projectiles

            if (shooting && atk_cd[2])
            {
                attack3.Play();
                projectiles.Add(new Arrow(x, y, Math.Cos(dir), Math.Sin(dir)));
                cd(atk_speed * 4, 2);
            }
        }

        public void removeProj(Projectile proj)
        {
            deletingProj.Add(proj);
        }

        public override void act()
        {
            if (!alive) return;
            if (hp <= 0) alive = false;
            handleAttacking();
            handleMovement();
        }
        
        public override void draw(Graphics g)
        {
            if (!alive)
            {
                g.DrawString("Game Over", new Font("Arial", 20), Brushes.White, new PointF(G.width / 2 - 60, 5));
                return;
            }

            foreach (Projectile proj in projectiles)
                proj.draw(g);
           
            g.FillEllipse(Brushes.RoyalBlue, DrawX, DrawY, (int)(radius * 2 * G.size), (int)(radius * 2 * G.size));
            
            // facing indicator
            g.FillEllipse(Brushes.Yellow, (float)(Math.Cos(dir) * 10 + G.width / 2 - 5), (float)(Math.Sin(dir) * 10 + G.height / 2 - 5), 10, 10);
            drawHpBar(g);
        }
    }
}
