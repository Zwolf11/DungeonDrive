﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using System.IO;

namespace DungeonDrive
{
    public class GameState : State
    {
        public Hero hero;
        public Room room;
        public Item[][] inventory = new Item[5][];
        public Item[] actionBar = new Item[10];
        public Font font = new Font("Arial", 12);
        public int size = 32;
        public String graveyard = "C:\\graveyard";
        private SoundPlayer saveSound = new SoundPlayer(Properties.Resources.level_up);

        public GameState(MainForm form, bool load) : base(form)
        {
            for (int i = 0; i < inventory.Length; i++)
                inventory[i] = new Item[5];

            if (load)
            {
                String[] loadFile = File.ReadAllLines("save");

                hero = new Hero(this, double.Parse(loadFile[0]), double.Parse(loadFile[1]));
                room = new Room(this, loadFile[2]);
            }
            else
            {
                hero = new Hero(this, 0, 0);
                room = new Room(this, "C:\\");
            }

            inventory[0][0] = new SmallPotion(this);
            inventory[0][1] = new MediumPotion(this);
            inventory[1][1] = new Weapon(this);
            inventory[2][0] = new LargePotion(this);
            inventory[2][1] = new Shield(this);
            hero.shield = new Shield(this);
            actionBar[0] = new SmallPotion(this);
        }

        public void saveGame()
        {
            String[] save = new String[3];
            save[0] = "" + hero.x;
            save[1] = "" + hero.y;
            save[2] = room.currentRoom;
            File.WriteAllLines("save", save);

            saveSound.Play();
        }

        public override void mouseUp(object sender, MouseEventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey)
            {
                this.addChildState(new PauseState(form), false, true);
            }
            else if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                hero.dirs[0] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.LeftKey)
            {
                hero.dirs[1] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                hero.dirs[2] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.RightKey)
            {
                hero.dirs[3] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.InventoryKey)
            {
                this.addChildState(new InventoryState(form), false, false);
            }
            else if (e.KeyCode == Properties.Settings.Default.Attack1Key)
            {
                hero.attacks[0] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.Attack2Key)
            {
                hero.attacks[1] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.Attack3Key)
            {
                hero.attacks[2] = true;
            }
        }

        public override void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                hero.dirs[0] = false;
            }
            else if (e.KeyCode == Properties.Settings.Default.LeftKey)
            {
                hero.dirs[1] = false;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                hero.dirs[2] = false;
            }
            else if (e.KeyCode == Properties.Settings.Default.RightKey)
            {
                hero.dirs[3] = false;
            }
        }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            hero.basicAtk();
        }

        public override void mouseMove(object sender, MouseEventArgs e)
        {
            hero.dir = (float)Math.Atan2(e.Y - (form.ClientSize.Height / 2), e.X - (form.ClientSize.Width / 2));
        }

        private void drawActionBar(Graphics g)
        {
            int boxSize = form.ClientSize.Width / (actionBar.Length + 8);
            int barHeight = form.ClientSize.Height - (int)(1.25 * boxSize);
            int padding = form.ClientSize.Width / 300;

            for (int i = 0; i < actionBar.Length; i++)
            {
                g.DrawImage(Properties.Resources.box, (i + 4) * boxSize + padding, barHeight + padding, boxSize - padding * 2, boxSize - padding * 2);

                if(actionBar[i] != null)
                    g.DrawImage(actionBar[i].img, (i + 4) * boxSize + padding, barHeight + padding, boxSize - padding * 2, boxSize - padding * 2);
            }
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(20, 20, 20));

            room.draw(g);
            hero.draw(g);

            //drawActionBar(g);
        }

        public override void tick(object sender, EventArgs e)
        {
            hero.act();

            foreach (Unit unit in room.enemies)
                unit.act();

            foreach (Projectile proj in hero.projectiles)
                proj.act();

            if (!hero.alive) return;
            foreach (Unit enemy in room.enemies)
            {
                if (Math.Sqrt(Math.Pow(hero.x - enemy.x, 2) + Math.Pow(hero.y - enemy.y, 2)) < hero.radius + enemy.radius)
                {
                    if (enemy.atk_cd[0])
                    {
                        int dirX = Math.Sign(hero.x - enemy.x);
                        int dirY = Math.Sign(hero.y - enemy.y);
                        enemy.knockBack(hero, dirX * 0.05, dirY * 0.05, 0);
                        hero.hp -= enemy.atk_dmg;
                        enemy.sleep_sec = 0.5 * 17;
                        enemy.cd(1, 0);
                    }
                }
            }

            form.Invalidate();
        }
    }
}
