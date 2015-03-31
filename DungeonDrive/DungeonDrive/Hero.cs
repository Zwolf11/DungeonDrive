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
        new public int DrawX { get { return (int)(state.form.Width / 2 - state.size * radius); } }
        new public int DrawY { get { return (int)(state.form.Height / 2 - state.size * radius); } }

        public List<Projectile> projectiles = new List<Projectile>();
        private List<Projectile> deletingProj = new List<Projectile>();
        public List<Unit> deletingList = new List<Unit>();

        private SoundPlayer attack1;
        private SoundPlayer attack2;
        private SoundPlayer attack3;
        private SoundPlayer level_up;
        private Random r;

        public float dir = 0;
        private bool shooting = false;
        public bool alive = true;
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

            // for testing
            this.atk_dmg = 100;
            r = new Random();

            Projectile.style = Projectile.AtkStyle.Frozen;

            try
            {
                attack1 = new SoundPlayer(@"attack1.wav");
                attack2 = new SoundPlayer(@"attack2.wav");
                attack3 = new SoundPlayer(@"attack3.wav");
                level_up = new SoundPlayer(@"level_up.wav");
            }
            catch (FileNotFoundException) { }
        }

        private void handleCursor()
        {
            foreach (Unit enemy in state.room.enemies)
                enemy.displayname = Math.Sqrt(Math.Pow(Cursor.Position.X - (enemy.DrawX + enemy.radius * state.size), 2) + Math.Pow(Cursor.Position.Y - (enemy.DrawY + enemy.radius * state.size), 2)) <= enemy.radius * state.size;
        }

        private void handleMovement()
        {
            if (knockback)
                knockBacked();

            dir = (float)Math.Atan2(Cursor.Position.Y - (state.form.Height / 2), Cursor.Position.X - (state.form.Width / 2));

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
            }
            else if (dirs[1] && !dirs[3])
                xNext = x - speed;
            else if (dirs[3] && !dirs[1])
                xNext = x + speed;

            tryMove(xNext, yNext);
        }

        private void handleAttacking()
        {
            // toggle melee/projectiles
            if (attacks[2])
            {
                if (atk_cd[4])
                {
                    shooting = !shooting;
                    cd(1, 4);
                }

                attacks[2] = false;
            }

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
                            enemy.hp -= 2 + 0.8 *atk_dmg;
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
                        Console.WriteLine(state.room.currentRoom);
                        experience(deletingEnemy, 1.5);
                    }
                    experience(deletingEnemy, 1.0);
                    getRdnWeapon();
                    
                    state.room.enemies.Remove(deletingEnemy);
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

        private double rdnDouble(double first, double second)
        { return Math.Round(r.NextDouble() * (second - first) + first,2); }

        private void getRdnWeapon()
        {
            // new weapon
            Weapon newWeapon = new Weapon(0, "rdn_name", Properties.Resources.fire);
            newWeapon.atk_damage = r.Next(WeaponStats.atk_damage[0], WeaponStats.atk_damage[1]);
            newWeapon.atk_speed = rdnDouble(WeaponStats.atk_speed[0], WeaponStats.atk_speed[1]);
            newWeapon.proj_speed = rdnDouble(WeaponStats.proj_speed[0], WeaponStats.proj_speed[1]);
            newWeapon.range = r.Next(WeaponStats.range[0], WeaponStats.range[1]);
            newWeapon.slowSec = rdnDouble(WeaponStats.slowSec[0], WeaponStats.slowSec[1]);
            newWeapon.slowFac = rdnDouble(WeaponStats.slowFac[0], WeaponStats.slowFac[1]);
            newWeapon.style = (AtkStyle)r.Next(0, 3);
            newWeapon.setDesc();
            Console.WriteLine(newWeapon.itemDesc);
        }

        public void basicAtk()
        {
            if (!alive) return;

            // melee
            if (!shooting && atk_cd[0])
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

            // projectiles

            if (shooting && atk_cd[2])
            {
                attack3.Play();
                projectiles.Add(new Projectile(state, x, y, Math.Cos(dir), Math.Sin(dir)));
                cd(Projectile.atk_speed, 2);
            }
        }

        public void experience(Unit enemy, double multiplier)
        {
            this.exp += enemy.exp*multiplier;
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

            //update weapon system
            WeaponStats.atk_damage[0] += 1;
            WeaponStats.atk_damage[1] += 1;
            WeaponStats.atk_speed[0] /= 1.1;
            WeaponStats.atk_speed[1] /= 1.1;
            WeaponStats.proj_speed[0] /= 1.1;
            WeaponStats.proj_speed[1] /= 1.1;

            /*
             * this only works for current room
            for (int i = 0; i < state.room.enemies.Count; i++)
            {
                state.room.enemies[i].full_hp += 5;
                state.room.enemies[i].hp = state.room.enemies[i].full_hp;
                state.room.enemies[i].atk_dmg += 3;
                state.room.enemies[i].speed *= 0.01;
                state.room.enemies[i].level++;
            }
             */
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
            handleCursor();
        }

        public void drawExpBar(Graphics g)
        { g.FillRectangle(Brushes.Yellow, DrawX, DrawY - 3, (int)(radius * 2 * state.size * this.exp / this.expcap), 2); }
        
        public override void draw(Graphics g)
        {
            if (!alive)
            {
                g.DrawString("Game Over", new Font("Arial", 20), Brushes.White, new PointF(state.form.Width / 2 - 60, 5));
                return;
            }

            foreach (Projectile proj in projectiles)
                proj.draw(g);
           
            g.FillEllipse(Brushes.RoyalBlue, DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            
            // facing indicator
            g.FillEllipse(Brushes.Yellow, (float)(Math.Cos(dir) * 10 + state.form.Width / 2 - 5), (float)(Math.Sin(dir) * 10 + state.form.Height / 2 - 5), 10, 10);

            // cd indicator
            for (int i = 0; i < state.hero.atk_cd.Length; i++)
                if (!state.hero.atk_cd[i])
                    g.FillEllipse(Brushes.Red, i * 30, 0, 30, 30);
            
            drawHpBar(g);
            drawExpBar(g);
        }
    }
}
