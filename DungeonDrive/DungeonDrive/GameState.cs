using System;
using System.Collections.Generic;
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
        public String currentRoom = "C:\\";
        public String pastRoom;

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

            inventory[0][0] = randomItem();
            inventory[0][1] = randomItem();
            inventory[1][1] = randomItem();
            inventory[2][0] = randomItem();
            inventory[2][1] = randomItem();
            hero.shield = new Shield(this);
            actionBar[0] = new SmallPotion(this);
        }

        public Item randomItem()
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            switch(rand.Next(6))
            {
                case 0:
                    return new Weapon(this);
                case 1:
                    return new Shield(this);
                case 2:
                    return new Helmet(this);
                case 3:
                    return new Armor(this);
                case 4:
                    return new Legs(this);
                case 5:
                    switch(rand.Next(3))
                    {
                        case 0:
                            return new SmallPotion(this);
                        case 1:
                            return new MediumPotion(this);
                        case 2:
                            return new LargePotion(this);
                    }
                    break;
            }

            return null;
        }

        public bool tryPickupItem(Item item)
        {
            for(int j=0;j<inventory[0].Length;j++)
                for(int i=0;i<inventory.Length;i++)
                    if(inventory[i][j] == null)
                    {
                        inventory[i][j] = item;
                        return true;
                    }

            return false;
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
            if (e.Button == MouseButtons.Left)
            {
                hero.basicAtk();
            }
            else if(e.Button == MouseButtons.Right)
            {
                float x = (float)((e.X - form.ClientSize.Width / 2.0) / size + hero.x);
                float y = (float)((e.Y - form.ClientSize.Height / 2.0) / size + hero.y);
                
                if (Math.Sqrt(Math.Pow(x - hero.x, 2) + Math.Pow(y - hero.y, 2)) < 2)
                {
                    foreach (KeyValuePair<Item, PointF> entry in room.droppedItems)
                        if (Math.Sqrt(Math.Pow(entry.Value.X - x, 2) + Math.Pow(entry.Value.Y - y, 2)) < 1)
                        {
                            if (tryPickupItem(entry.Key))
                            {
                                inventory[0][0] = entry.Key;
                                room.droppedItems.Remove(entry.Key);
                            }
                            break;
                        }

                    foreach(Obstacle ob in room.obstacles)
                        if (Math.Sqrt(Math.Pow(ob.x - x, 2) + Math.Pow(ob.y - y, 2)) < 1 && ob is Chest)
                        {
                            Chest chest = (Chest)ob;
                            if (chest.closed)
                            {
                                chest.closed = false;
                                room.droppedItems.Add(randomItem(), new PointF(ob.x + 0.5f, ob.y + 0.5f));
                            }
                            break;
                        }
                }
            }
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
                if (Math.Sqrt(Math.Pow(hero.x - enemy.x, 2) + Math.Pow(hero.y - enemy.y, 2)) < hero.radius + enemy.radius)
                    enemy.attackHero();

            form.Invalidate();
        }
    }
}
