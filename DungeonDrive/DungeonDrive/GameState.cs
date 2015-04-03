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
        public Font font = new Font("Arial", 12);
        public int size = 40;
        public String graveyard = "C:\\graveyard";
        private SoundPlayer saveSound = new SoundPlayer(Properties.Resources.level_up);
        public String currentRoom = "C:\\";
        public String pastRoom;

        public GameState(MainForm form, bool load) : base(form)
        {
            hero = new Hero(this, 0, 0);
            for (int i = 0; i < inventory.Length; i++)
                inventory[i] = new Item[5];

            if (load)
                loadGame();
            else
            {
                room = new Room(this, "C:\\");
                inventory[0][0] = randomItem();
            }
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
            if(item is Helmet && hero.helmet == null)
            {
                hero.helmet = (Helmet)item;
                return true;
            }
            else if (item is Armor && hero.armor == null)
            {
                hero.armor = (Armor)item;
                return true;
            }
            else if (item is Legs && hero.legs == null)
            {
                hero.legs = (Legs)item;
                return true;
            }
            else if (item is Shield && hero.shield == null)
            {
                hero.shield = (Shield)item;
                return true;
            }
            else if (item is Weapon && hero.weapon == null)
            {
                hero.weapon = (Weapon)item;
                return true;
            }

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
            List<String> save = new List<String>();

            if (pastRoom != null) save.Add(pastRoom);
            else save.Add("NULL");

            save.Add(room.currentRoom);

            save.Add("" + hero.x);
            save.Add("" + hero.y);
            save.Add("" + hero.level);
            save.Add("" + hero.exp);
            save.Add("" + hero.full_hp);
            save.Add("" + hero.hp);
            save.Add("" + hero.atk_dmg);
            save.Add("" + hero.atk_speed);

            if (hero.helmet == null) save.Add("NULL");
            else save.Add(hero.helmet.name + "_" + hero.helmet.defense);
            if (hero.armor == null) save.Add("NULL");
            else save.Add(hero.armor.name + "_" + hero.armor.defense);
            if (hero.legs == null) save.Add("NULL");
            else save.Add(hero.legs.name + "_" + hero.legs.defense);
            if (hero.shield == null) save.Add("NULL");
            else save.Add(hero.shield.name + "_" + hero.shield.defense);
            if (hero.weapon == null) save.Add("NULL");
            else save.Add(hero.weapon.name + "_" + hero.weapon.damage);

            for(int j=0;j<inventory[0].Length;j++)
                for(int i=0;i<inventory.Length;i++)
                {
                    if (inventory[i][j] == null)
                        save.Add("NULL");
                    else
                    {
                        if(inventory[i][j] is Helmet)
                        {
                            Helmet helmet = (Helmet)inventory[i][j];
                            save.Add("HELMET_" + helmet.name + "_" + helmet.defense);
                        }
                        else if (inventory[i][j] is Armor)
                        {
                            Armor armor = (Armor)inventory[i][j];
                            save.Add("ARMOR_" + armor.name + "_" + armor.defense);
                        }
                        else if (inventory[i][j] is Legs)
                        {
                            Legs legs = (Legs)inventory[i][j];
                            save.Add("LEGS_" + legs.name + "_" + legs.defense);
                        }
                        else if (inventory[i][j] is Shield)
                        {
                            Shield shield = (Shield)inventory[i][j];
                            save.Add("SHIELD_" + shield.name + "_" + shield.defense);
                        }
                        else if (inventory[i][j] is Weapon)
                        {
                            Weapon weapon = (Weapon)inventory[i][j];
                            save.Add("WEAPON_" + weapon.name + "_" + weapon.damage);
                        }
                        else
                        {
                            save.Add(inventory[i][j].name);
                        }
                    }
                }

            File.WriteAllLines("save", save);

            saveSound.Play();
        }

        private void loadGame()
        {
            String[] loadFile = File.ReadAllLines("save");

            if(loadFile[0] != "NULL")
                pastRoom = loadFile[0];
            room = new Room(this, loadFile[1]);

            hero.x = double.Parse(loadFile[2]);
            hero.y = double.Parse(loadFile[3]);
            hero.level = int.Parse(loadFile[4]);
            hero.exp = double.Parse(loadFile[5]);
            hero.full_hp = double.Parse(loadFile[6]);
            hero.hp = double.Parse(loadFile[7]);
            hero.atk_dmg = double.Parse(loadFile[8]);
            hero.atk_speed = double.Parse(loadFile[9]);

            String[] helmet = loadFile[10].Split('_');
            if (helmet[0] != "NULL")
                hero.helmet = new Helmet(this, helmet[0], int.Parse(helmet[1]));
            String[] armor = loadFile[11].Split('_');
            if (armor[0] != "NULL")
                hero.armor = new Armor(this, armor[0], int.Parse(armor[1]));
            String[] legs = loadFile[12].Split('_');
            if (legs[0] != "NULL")
                hero.legs = new Legs(this, legs[0], int.Parse(legs[1]));
            String[] shield = loadFile[13].Split('_');
            if (shield[0] != "NULL")
                hero.shield = new Shield(this, shield[0], int.Parse(shield[1]));
            String[] weapon = loadFile[14].Split('_');
            if (weapon[0] != "NULL")
                hero.weapon = new Weapon(this, weapon[0], int.Parse(weapon[1]));

            int loc = 15;
            for (int j = 0; j < inventory[0].Length; j++)
                for (int i = 0; i < inventory.Length; i++)
                {
                    String[] item = loadFile[loc++].Split('_');

                    if(item[0] != "NULL")
                    {
                        if (item[0] == "HELMET")
                            inventory[i][j] = new Helmet(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "ARMOR")
                            inventory[i][j] = new Armor(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "LEGS")
                            inventory[i][j] = new Legs(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "SHIELD")
                            inventory[i][j] = new Shield(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "WEAPON")
                            inventory[i][j] = new Weapon(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "Small Potion")
                            inventory[i][j] = new SmallPotion(this);
                        else if (item[0] == "Medium Potion")
                            inventory[i][j] = new MediumPotion(this);
                        else if (item[0] == "Large Potion")
                            inventory[i][j] = new LargePotion(this);
                    }
                }
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
                                room.droppedItems.Remove(entry.Key);

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

                    if (room.doorSpace[(int) x, (int) y])
                    {
                        foreach (Door door in room.doors)
                        {
                            if ((Math.Sqrt(Math.Pow(door.x - x, 2) + Math.Pow(door.y - y, 2)) < 1)  ||   (Math.Sqrt(Math.Pow((door.x + door.width - 1) - x,2) + Math.Pow((door.y + door.height - 1) - y, 2)) < 1))
                            {
                                // this is the correct door
                                if (!door.switchClosed())
                                {
                                    room.updateDrawingGrid(door.getNegativeRoom());
                                    room.updateDrawingGrid(door.getPositiveRoom());
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void mouseMove(object sender, MouseEventArgs e)
        {
            hero.dir = (float)Math.Atan2(e.Y - (form.ClientSize.Height / 2), e.X - (form.ClientSize.Width / 2));
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(20, 20, 20));

            room.draw(g);
            hero.draw(g);
        }

        public override void tick(object sender, EventArgs e)
        {
            hero.act();

            foreach (Unit unit in room.enemies)
                unit.act();

            foreach (Projectile proj in hero.projectiles)
                proj.act();

            foreach (Unit enemy in room.enemies)
                if (Math.Sqrt(Math.Pow(hero.x - enemy.x, 2) + Math.Pow(hero.y - enemy.y, 2)) < hero.radius + enemy.radius)
                    enemy.attackHero();

            form.Invalidate();
        }
    }
}
