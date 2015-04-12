﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.IO;

namespace DungeonDrive
{
    public class Hero : Unit
    {
        new public int DrawX { get { return (int)(state.form.ClientSize.Width / 2 - state.size * radius); } }
        new public int DrawY { get { return (int)(state.form.ClientSize.Height / 2 - state.size * radius); } }

        public List<Projectile> projectiles = new List<Projectile>();
        private List<Projectile> deletingProj = new List<Projectile>();
        public List<Unit> deletingList = new List<Unit>();
        public Projectile weapon_proj;
        private bool testing = true;

        private SoundPlayer attack1;
        private SoundPlayer attack2;
        private SoundPlayer attack3;
        private SoundPlayer level_up;
        private Bitmap[,] imgs = new Bitmap[8, 8];
        private int imgDir = 0;

        public Helmet helmet = null;
        public Armor armor = null;
        public Legs legs = null;
        public Shield shield = null;
        public Weapon weapon = null;

        public float dir = 0;
        public bool[] dirs = { false, false, false, false };
        public bool[] attacks = { false, false, false };

        public Hero(GameState state, double x, double y)
            : base(state, x, y)
        {
            this.hp = 20;
            this.full_hp = hp;
            this.atk_dmg = 2;
            this.atk_speed = 0.2;
            this.speed = 0.3;
            this.radius = 0.49;
            this.exp = 0.0;
            this.expcap = 10.0;
            this.level = 1;
            this.status = "Normal";
            this.poison_sec = 0;

            this.weapon_proj = new Projectile();
            // init hero imges
            //w
            imgs[2, 0] = new Bitmap(Properties.Resources.w1);
            imgs[2, 1] = new Bitmap(Properties.Resources.w2);
            imgs[2, 2] = new Bitmap(Properties.Resources.w3);
            imgs[2, 3] = new Bitmap(Properties.Resources.w4);
            imgs[2, 4] = new Bitmap(Properties.Resources.w5);
            imgs[2, 5] = new Bitmap(Properties.Resources.w6);
            imgs[2, 6] = new Bitmap(Properties.Resources.w7);
            imgs[2, 7] = new Bitmap(Properties.Resources.w8);
            //wd
            imgs[3, 0] = new Bitmap(Properties.Resources.wd1);
            imgs[3, 1] = new Bitmap(Properties.Resources.wd2);
            imgs[3, 2] = new Bitmap(Properties.Resources.wd3);
            imgs[3, 3] = new Bitmap(Properties.Resources.wd4);
            imgs[3, 4] = new Bitmap(Properties.Resources.wd5);
            imgs[3, 5] = new Bitmap(Properties.Resources.wd6);
            imgs[3, 6] = new Bitmap(Properties.Resources.wd7);
            imgs[3, 7] = new Bitmap(Properties.Resources.wd8);
            //d
            imgs[4, 0] = new Bitmap(Properties.Resources.d1);
            imgs[4, 1] = new Bitmap(Properties.Resources.d2);
            imgs[4, 2] = new Bitmap(Properties.Resources.d3);
            imgs[4, 3] = new Bitmap(Properties.Resources.d4);
            imgs[4, 4] = new Bitmap(Properties.Resources.d5);
            imgs[4, 5] = new Bitmap(Properties.Resources.d6);
            imgs[4, 6] = new Bitmap(Properties.Resources.d7);
            imgs[4, 7] = new Bitmap(Properties.Resources.d8);
            //sd
            imgs[5, 0] = new Bitmap(Properties.Resources.sd1);
            imgs[5, 1] = new Bitmap(Properties.Resources.sd2);
            imgs[5, 2] = new Bitmap(Properties.Resources.sd3);
            imgs[5, 3] = new Bitmap(Properties.Resources.sd4);
            imgs[5, 4] = new Bitmap(Properties.Resources.sd5);
            imgs[5, 5] = new Bitmap(Properties.Resources.sd6);
            imgs[5, 6] = new Bitmap(Properties.Resources.sd7);
            imgs[5, 7] = new Bitmap(Properties.Resources.sd8);
            //s
            imgs[6, 0] = new Bitmap(Properties.Resources.s1);
            imgs[6, 1] = new Bitmap(Properties.Resources.s2);
            imgs[6, 2] = new Bitmap(Properties.Resources.s3);
            imgs[6, 3] = new Bitmap(Properties.Resources.s4);
            imgs[6, 4] = new Bitmap(Properties.Resources.s5);
            imgs[6, 5] = new Bitmap(Properties.Resources.s6);
            imgs[6, 6] = new Bitmap(Properties.Resources.s7);
            imgs[6, 7] = new Bitmap(Properties.Resources.s8);
            //sa
            imgs[7, 0] = new Bitmap(Properties.Resources.sa1);
            imgs[7, 1] = new Bitmap(Properties.Resources.sa2);
            imgs[7, 2] = new Bitmap(Properties.Resources.sa3);
            imgs[7, 3] = new Bitmap(Properties.Resources.sa4);
            imgs[7, 4] = new Bitmap(Properties.Resources.sa5);
            imgs[7, 5] = new Bitmap(Properties.Resources.sa6);
            imgs[7, 6] = new Bitmap(Properties.Resources.sa7);
            imgs[7, 7] = new Bitmap(Properties.Resources.sa8);
            //a
            imgs[0, 0] = new Bitmap(Properties.Resources.a1);
            imgs[0, 1] = new Bitmap(Properties.Resources.a2);
            imgs[0, 2] = new Bitmap(Properties.Resources.a3);
            imgs[0, 3] = new Bitmap(Properties.Resources.a4);
            imgs[0, 4] = new Bitmap(Properties.Resources.a5);
            imgs[0, 5] = new Bitmap(Properties.Resources.a6);
            imgs[0, 6] = new Bitmap(Properties.Resources.a7);
            imgs[0, 7] = new Bitmap(Properties.Resources.a8);
            //wa
            imgs[1, 0] = new Bitmap(Properties.Resources.wa1);
            imgs[1, 1] = new Bitmap(Properties.Resources.wa2);
            imgs[1, 2] = new Bitmap(Properties.Resources.wa3);
            imgs[1, 3] = new Bitmap(Properties.Resources.wa4);
            imgs[1, 4] = new Bitmap(Properties.Resources.wa5);
            imgs[1, 5] = new Bitmap(Properties.Resources.wa6);
            imgs[1, 6] = new Bitmap(Properties.Resources.wa7);
            imgs[1, 7] = new Bitmap(Properties.Resources.wa8);

            // for testing
            if (testing)
            {
                this.atk_dmg = 100;
                this.full_hp = 1000;
                this.hp = this.full_hp;
            }

            attack1 = new SoundPlayer(Properties.Resources.attack1);
            attack2 = new SoundPlayer(Properties.Resources.attack2);
            attack3 = new SoundPlayer(Properties.Resources.attack3);
            level_up = new SoundPlayer(Properties.Resources.level_up);
        }

        public void equipWeapon(Weapon weapon)
        {
            // change stats for projectiles
            if (weapon.ranged)
            {
                this.weapon = weapon;
                weapon_proj.dmg = weapon.damage;
                weapon_proj.atk_speed = weapon.atk_speed;
                weapon_proj.proj_speed = weapon.proj_speed;
                weapon_proj.proj_range = weapon.proj_range;
                weapon_proj.style = weapon.style;
                weapon_proj.powerSec = weapon.powerSec;
                weapon_proj.powerFac = weapon.powerFac;
                weapon_proj.proj_img = weapon.projectileImg;
                /*
                Projectile.dmg = weapon.damage;
                Projectile.atk_speed = weapon.atk_speed;
                Projectile.proj_speed = weapon.proj_speed;
                Projectile.proj_range = weapon.proj_range;
                Projectile.style = weapon.style;
                Projectile.powerSec = weapon.powerSec;
                Projectile.powerFac = weapon.powerFac;
                Projectile.proj_img = weapon.projectileImg;*/
            }

            // change stats for hero when using non-ranged weapons
            else
            {
            }
        }

        public void equipHelmet(Helmet helmet)
        {
        }

        public void equipArmor(Armor armor)
        {
        }

        public void equipLegs(Legs legs)
        {
        }

        public void equipShield(Shield shield)
        {
        }

        private void handleCursor()
        {
            foreach (Unit enemy in state.room.enemies)
                enemy.displayname = Math.Sqrt(Math.Pow(Cursor.Position.X - (enemy.DrawX + enemy.radius * state.size), 2) + Math.Pow(Cursor.Position.Y - (enemy.DrawY + enemy.radius * state.size), 2)) <= enemy.radius * state.size;

            //            foreach (KeyValuePair<Item, PointF> entry in state.room.droppedItems)
            //                entry.Key.showDes = Math.Sqrt(Math.Pow(entry.Value.X - Cursor.Position.X, 2) + Math.Pow(entry.Value.Y - Cursor.Position.Y, 2)) <= state.size;
        }

        private void handleMovement()
        {
            if (knockback)
                knockBacked();

            if (dir <= -7.0 / 8.0 * Math.PI || dir >= 7.0 / 8.0 * Math.PI) imgDir = 0;
            else if (dir > -7.0 / 8.0 * Math.PI && dir <= -5.0 / 8.0 * Math.PI) imgDir = 1;
            else if (dir > -5.0 / 8.0 * Math.PI && dir <= -3.0 / 8.0 * Math.PI) imgDir = 2;
            else if (dir > -3.0 / 8.0 * Math.PI && dir <= -1.0 / 8.0 * Math.PI) imgDir = 3;
            else if (dir > -1.0 / 8.0 * Math.PI && dir <= 1.0 / 8.0 * Math.PI) imgDir = 4;
            else if (dir > 1.0 / 8.0 * Math.PI && dir <= 3.0 / 8.0 * Math.PI) imgDir = 5;
            else if (dir > 3.0 / 8.0 * Math.PI && dir <= 5.0 / 8.0 * Math.PI) imgDir = 6;
            else if (dir > 5.0 / 8.0 * Math.PI && dir <= 7.0 / 8.0 * Math.PI) imgDir = 7;

            double xNext = x;
            double yNext = y;

            if (dirs[0] && !dirs[2])
            {
                if (dirs[1] && !dirs[3])
                {
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;
                }
                else if (dirs[3] && !dirs[1])
                {
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y - Math.Sqrt(2) / 2 * speed;
                }
                else
                    yNext = y - speed;
                animate();
            }
            else if (dirs[2] && !dirs[0])
            {
                if (dirs[1] && !dirs[3])
                {
                    xNext = x - Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                }
                else if (dirs[3] && !dirs[1])
                {
                    xNext = x + Math.Sqrt(2) / 2 * speed;
                    yNext = y + Math.Sqrt(2) / 2 * speed;
                }
                else
                    yNext = y + speed;
                animate();
            }
            else if (dirs[1] && !dirs[3])
            {
                xNext = x - speed;
                animate();
            }
            else if (dirs[3] && !dirs[1])
            {
                xNext = x + speed;
                animate();
            }

            tryMove(xNext, yNext, this);
        }

        private void animate()
        { animFrame = (animFrame + 0.3) % 8; }

        private void handleAttacking()
        {
            /*            // toggle melee/projectiles
                        if (attacks[2])
                        {
                            if (atk_cd[4])
                            {
                                shooting = !shooting;
                                cd(1, 4);
                            }

                            attacks[2] = false;
                        }
             * */

            // knockback skill
            if (attacks[0])
            {
                if (atk_cd[1])
                {
                    foreach (Unit enemy in state.room.enemies)
                    {
                        if (Math.Abs(enemy.x - x) < 1.2 && Math.Abs(enemy.y - y) < 1.2)
                        {
                            try { attack2.Play(); }
                            catch (FileNotFoundException) { }
                            double factor = 1 / Math.Sqrt(Math.Pow(enemy.x - x, 2) + Math.Pow(enemy.y - y, 2));
                            knockBack(enemy, (enemy.x - x) * factor, (enemy.y - y) * factor, 0.4);
                            enemy.hp -= 2 + 0.8 * atk_dmg;
                            /*if (status.Equals("Cursed"))
                                this.full_hp -= 5;*/
                            if (enemy.hp <= 0)
                                deletingList.Add(enemy);
                        }
                    }
                    cd(2, 1);
                }

                attacks[0] = false;
            }

            // delete skill; doesn't actually delete the file, but moves it into a 'graveyard' directory in C:\
            if (attacks[1])
            {
                if (!Directory.Exists(state.graveyard))
                {
                    Directory.CreateDirectory(state.graveyard);
                }

                if (atk_cd[3])
                {
                    foreach (Unit enemy in state.room.enemies)
                    {
                        if (Math.Abs(enemy.x - x) < 1.2 && Math.Abs(enemy.y - y) < 1.2)
                        {
                            deletingList.Add(enemy);
                            File.Move(state.room.currentRoom + "\\" + enemy.filename, state.graveyard + "\\" + enemy.filename);
                        }
                    }
                    cd(5, 3);
                }

                attacks[1] = false;
            }

            if (deletingList.Count > 0)
            {
                foreach (Unit deletingEnemy in deletingList)
                {
                    if (state.room.currentRoom.Equals(state.graveyard))
                    {
                        //Console.WriteLine(state.room.currentRoom);
                        experience(deletingEnemy, 1.5);
                    }
                    experience(deletingEnemy, 1.0);
                    state.room.enemies.Remove(deletingEnemy);
                    Random rand = new Random();
                    if (rand.Next(5) == 0)
                        state.room.droppedItems.Add(state.randomItem(), new PointF((float)deletingEnemy.x, (float)deletingEnemy.y));
                    if (testing)
                        state.room.droppedItems.Add(new Weapon(state), new PointF((float)deletingEnemy.x, (float)deletingEnemy.y));

                }
                deletingList.Clear();
            }

            if (deletingProj.Count > 0)
            {
                foreach (Projectile proj in deletingProj)
                    projectiles.Remove(proj);
                deletingProj.Clear();
            }
        }

        private void handleAilment()
        {
            if (this.status.Equals("Poisoned"))
            {
                if (poison_sec == 0)
                {
                    poison_sec = 300;
                    speed -= speed * 0.5;
                }

                --poison_sec;

                if (poison_sec == 0)
                {
                    speed = 0.3;
                    status = "Normal";
                }
            }
            /*else if (this.status.Equals("Cursed"))
            {
                if (curse_sec == 0)
                {
                    curse_sec = 300;
                }
                --curse_sec;
                
                if (curse_sec == 0)
                {
                    this.status = "Normal";
                }
            }*/
        }

        public void basicAtk()
        {

            
            if (this.weapon != null && this.weapon.ranged)
            {
                if (atk_cd[2])
                {
                    attack3.Play();
                    weapon_proj.setProjectile(state, x, y, Math.Cos(dir), Math.Sin(dir));
                    projectiles.Add(weapon_proj);
                    cd(weapon_proj.atk_speed, 2);
                }
            }

            // melee
            else
            {
                if (atk_cd[0])
                {
                    foreach (Unit enemy in state.room.enemies)
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
            }
        }

        public void experience(Unit enemy, double multiplier)
        {
            this.exp += enemy.exp * multiplier;
            if (this.exp >= this.expcap)
                levelUp();
        }

        public void levelUp()
        {
            try { level_up.Play(); }
            catch (FileNotFoundException) { }
            this.full_hp += 10;
            this.hp = this.full_hp;
            this.atk_dmg += 1;
            this.atk_speed -= 0.01;
            this.exp -= this.expcap;
            this.expcap *= 1.5;
            this.level += 1;
        }

        public void removeProj(Projectile proj)
        {
            deletingProj.Add(proj);
        }

        public override void act()
        {
            if (hp <= 0)
            {
                state.addChildState(new GameOverState(state.form), false, true);
                return;
            }
            handleAttacking();
            handleMovement();
            handleCursor();
            handleAilment();
        }

        public void drawExpBar(Graphics g)
        { g.FillRectangle(Brushes.Yellow, DrawX, DrawY - 3, (int)(radius * 2 * state.size * this.exp / this.expcap), 2); }

        public override void draw(Graphics g)
        {
            foreach (Projectile proj in projectiles)
                proj.draw(g);

            // cd indicator
            for (int i = 0; i < state.hero.atk_cd.Length; i++)
                if (!state.hero.atk_cd[i])
                    g.FillEllipse(Brushes.Red, i * 30, 0, 30, 30);

            //Console.WriteLine(imgDir);
            g.DrawImage(imgs[imgDir, (int)animFrame], DrawX - 6, DrawY - 6, (int)(radius * 3 * state.size), (int)(radius * 3 * state.size));

            drawHpBar(g);
            drawExpBar(g);
        }
    }
}
