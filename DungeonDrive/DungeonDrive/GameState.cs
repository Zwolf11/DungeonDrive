using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class GameState : State
    {
        public Hero hero;
        public Room room;
        public Item[][] inventory = new Item[10][];
        public ActionBar actionBar;
        public Font font = new Font("Arial", 12);
        public int size = 32;
        public String graveyard = "C:\\graveyard";
        public String pastRoom;
        public String currentRoom = "C:\\";

        public GameState(MainForm form) : base(form)
        {
            hero = new Hero(this, 0, 0);
            room = new Room(this, "C:\\");
            actionBar = new ActionBar(this, 12, new Bitmap(Properties.Resources.action_bar));

            for (int i = 0; i < inventory.Length; i++)
                inventory[i] = new Item[10];
        }

        public override void mouseUp(object sender, MouseEventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey)
            {
                this.addChildState(new PauseState(form), true);
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
                this.addChildState(new InventoryState(form), false);
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
            actionBar.getAction(e.X, e.Y);
        }

        public override void mouseMove(object sender, MouseEventArgs e)
        {
            hero.dir = (float)Math.Atan2(e.Y - (form.Height / 2), Cursor.Position.X - (form.Width / 2));
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(20, 20, 20));

            room.draw(g);
            hero.draw(g);
            actionBar.draw(g);
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
