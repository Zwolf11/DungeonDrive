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
        public static bool[,] heroSkill = new bool[SkillStreeState.skillList, SkillStreeState.skillLevel]; 
        public Font font = new Font("Arial", 12);
        public int size = 40;
        private int konami = 0;
        private bool startTutorial = false;
        public String graveyard = "C:\\graveyard";
        private SoundPlayer saveSound = new SoundPlayer(Properties.Resources.level_up);
        
        // If you want to change the starting room, initialize currentRoom to that directory.
        // Be sure to use \\ instead of a single \
        public String currentRoom = "C:\\";

        public AllLevelInfo allLevelInfo;
        
        public String pastRoom;
        private int mouseX, mouseY;
        public double angle = -1;

        private Bitmap mouseDoorImg = Properties.Resources.trigger_door;
        private Bitmap mouseNotDoorImg = Properties.Resources.trigger_door_not;
        private Bitmap mouseChestImg = Properties.Resources.chest_trigger;
        private Bitmap mouseImg = null;

        public GameState(MainForm form, bool load) : base(form)
        {
            hero = new Hero(this, 0, 0);
            for (int i = 0; i < inventory.Length; i++)
                inventory[i] = new Item[5];

            if (load)
                loadGame();
            else
            {
                allLevelInfo = new AllLevelInfo(this);
                room = new Room(this, currentRoom);
                initSkillTree();
                startTutorial = true;
            }
        }
        private void initSkillTree(){
            for (int i = 0; i < SkillStreeState.skillList; i++ )
            {
                for (int j = 0; j < SkillStreeState.skillLevel; j ++ )
                {
                    GameState.heroSkill[i,j] = false;
                }
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

        private void beginTutorial() { startTutorial = false; addChildState(new MessageState(form, "Welcome to Dungeon Drive (D:)! This game creates dungeons from your file system.", tutorial2), false, true); }
        private void tutorial2() { addChildState(new MessageState(form, "You are currently in your C: Drive. There are stairs that lead to deeper folders.", tutorial3), false, true); }
        private void tutorial3() { addChildState(new MessageState(form, "Enemies, chests, and obstacles are placed in the dungeons depending on what kinds of files are in your folders.", tutorial4), false, true); }
        private void tutorial4() { addChildState(new MessageState(form, "Click to attack, press space to open your inventory, and I bet you can figure out the rest."), false, true); }

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
            else save.Add(hero.helmet.name + "_" + hero.helmet.level + "_" + hero.helmet.hp + "_" + hero.helmet.hp_reg);
            if (hero.armor == null) save.Add("NULL");
            else save.Add(hero.armor.name + "_" + hero.armor.level + "_" + hero.armor.hp);
            if (hero.legs == null) save.Add("NULL");
            else save.Add(hero.legs.name + "_" + hero.legs.level + "_" + hero.legs.hp + "_" + hero.legs.ms);
            if (hero.shield == null) save.Add("NULL");
            else save.Add(hero.shield.name + "_" + hero.shield.level + "_" + hero.shield.hp + "_" + hero.shield.blockDmg + "_" + hero.shield.blockChan);
            if (hero.weapon == null) save.Add("NULL");
            else save.Add(hero.weapon.name + "_" + hero.weapon.level + "_" + hero.weapon.damage + "_" + hero.weapon.ranged + "_" + hero.weapon.atk_speed + "_" + hero.weapon.proj_speed + "_" + hero.weapon.proj_range + "_" + hero.weapon.powerSec + "_" + hero.weapon.powerFac + "_" + (int)hero.weapon.style);

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
                            save.Add("HELMET_" + helmet.name + "_" + helmet.level + "_" + helmet.hp + "_" + helmet.hp_reg);
                        }
                        else if (inventory[i][j] is Armor)
                        {
                            Armor armor = (Armor)inventory[i][j];
                            save.Add("ARMOR_" + armor.name + "_" + armor.level + "_" + armor.hp);
                        }
                        else if (inventory[i][j] is Legs)
                        {
                            Legs legs = (Legs)inventory[i][j];
                            save.Add("LEGS_" + legs.name + "_" + legs.level + "_" + legs.hp + "_" + legs.ms);
                        }
                        else if (inventory[i][j] is Shield)
                        {
                            Shield shield = (Shield)inventory[i][j];
                            save.Add("SHIELD_" + shield.name + "_" + shield.level + "_" + shield.hp + "_" + shield.blockDmg + "_" + shield.blockChan);
                        }
                        else if (inventory[i][j] is Weapon)
                        {
                            Weapon weapon = (Weapon)inventory[i][j];
                            save.Add("WEAPON_" + weapon.name + "_" + weapon.level + "_" + weapon.damage + "_" + weapon.ranged + "_" + weapon.atk_speed + "_" + weapon.proj_speed + "_" + weapon.proj_range + "_" + weapon.powerSec + "_" + weapon.powerFac + "_" + (int)weapon.style);
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
                hero.helmet = new Helmet(this, helmet[0], int.Parse(helmet[1]), double.Parse(helmet[2]), double.Parse(helmet[3]));
            String[] armor = loadFile[11].Split('_');
            if (armor[0] != "NULL")
                hero.armor = new Armor(this, armor[0], int.Parse(armor[1]), double.Parse(armor[2]));
            String[] legs = loadFile[12].Split('_');
            if (legs[0] != "NULL")
                hero.legs = new Legs(this, legs[0], int.Parse(legs[1]), double.Parse(legs[2]), double.Parse(legs[3]));
            String[] shield = loadFile[13].Split('_');
            if (shield[0] != "NULL")
                hero.shield = new Shield(this, shield[0], int.Parse(shield[1]), double.Parse(shield[2]), double.Parse(shield[3]), double.Parse(shield[4]));
            String[] weapon = loadFile[14].Split('_');
            if (weapon[0] != "NULL")
                hero.weapon = new Weapon(this, weapon[0], int.Parse(weapon[1]), double.Parse(weapon[2]), bool.Parse(weapon[3]), double.Parse(weapon[4]), double.Parse(weapon[5]), int.Parse(weapon[6]), double.Parse(weapon[7]), double.Parse(weapon[8]), int.Parse(weapon[9]) );

            int loc = 15;
            for (int j = 0; j < inventory[0].Length; j++)
                for (int i = 0; i < inventory.Length; i++)
                {
                    String[] item = loadFile[loc++].Split('_');

                    if(item[0] != "NULL")
                    {
                        if (item[0] == "HELMET")
                            inventory[i][j] = new Helmet(this, item[1], int.Parse(item[2]), double.Parse(item[3]), double.Parse(item[4]));
                        else if (item[0] == "ARMOR")
                            inventory[i][j] = new Armor(this, item[1], int.Parse(item[2]), double.Parse(item[3]));
                        else if (item[0] == "LEGS")
                            inventory[i][j] = new Legs(this, item[1], int.Parse(item[2]), double.Parse(item[3]), double.Parse(item[4]));
                        else if (item[0] == "SHIELD")
                            inventory[i][j] = new Shield(this, item[1], int.Parse(item[2]), double.Parse(item[3]), double.Parse(item[4]), double.Parse(item[5]));
                        else if (item[0] == "WEAPON")
                            inventory[i][j] = new Weapon(this, item[1], int.Parse(item[2]), double.Parse(item[3]), bool.Parse(item[4]), double.Parse(item[5]), double.Parse(item[6]), int.Parse(item[7]), double.Parse(item[8]), double.Parse(item[9]), int.Parse(item[10]));
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
            if (e.KeyCode == Keys.Up && (konami == 0 || konami == 1))
                konami++;
            else if (e.KeyCode == Keys.Down && (konami == 2 || konami == 3))
                konami++;
            else if (e.KeyCode == Keys.Left && (konami == 4 || konami == 6))
                konami++;
            else if (e.KeyCode == Keys.Right && (konami == 5 || konami == 7))
                konami++;
            else if (e.KeyCode == Keys.B && konami == 8)
                konami++;
            else if (e.KeyCode == Keys.A && konami == 9)
                konami++;
            else if (e.KeyCode == Keys.Tab) {
                spellChange();
            }
            else if (e.KeyCode == Keys.Enter && konami == 10)
            {
                for (int i = 0; i < 10; i++)
                    hero.levelUp();

                for (int i = 0; i < inventory.Length; i++)
                    for (int j = 0; j < inventory[i].Length; j++)
                        if (inventory[i][j] == null)
                            inventory[i][j] = randomItem();
            }
            else
                konami = 0;

            if (e.KeyCode == Properties.Settings.Default.CloseKey)
                this.addChildState(new PauseState(form), false, true);
            else if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[2] = true;
                else if (hero.status.Equals("Paralyzed"))
                    hero.dirs[0] = false;
                else
                    hero.dirs[0] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.LeftKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[3] = true;
                else if (hero.status.Equals("Paralyzed"))
                    hero.dirs[1] = false;
                else
                    hero.dirs[1] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[0] = true;
                else if (hero.status.Equals("Paralyzed"))
                    hero.dirs[2] = false;
                else
                    hero.dirs[2] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.RightKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[1] = true;
                else if (hero.status.Equals("Paralyzed"))
                    hero.dirs[3] = false;
                else
                    hero.dirs[3] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.InventoryKey)
                this.addChildState(new InventoryState(form), false, false);
            else if (e.KeyCode == Properties.Settings.Default.Attack1Key)
                hero.attacks[0] = true;
            else if (e.KeyCode == Properties.Settings.Default.Attack2Key)
                hero.attacks[1] = true;
            else if (e.KeyCode == Properties.Settings.Default.Attack3Key)
                hero.attacks[2] = true;
            else if (e.KeyCode == Properties.Settings.Default.SkillTreeKey)
                this.addChildState(new SkillStreeState(form), false, false);
            
        }

        public override void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[2] = false;
                else
                    hero.dirs[0] = false;

                mouseImg = null;
            }
            else if (e.KeyCode == Properties.Settings.Default.LeftKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[3] = false;
                else
                    hero.dirs[1] = false;

                mouseImg = null;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[0] = false;
                else
                    hero.dirs[2] = false;

                mouseImg = null;
            }
            else if (e.KeyCode == Properties.Settings.Default.RightKey)
            {
                if (hero.status.Equals("Cursed"))
                    hero.dirs[1] = false;
                else
                    hero.dirs[3] = false;

                mouseImg = null;
            }
        }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (hero.status.Equals("Binded Arm") || hero.status.Equals("Paralyzed"))
                {
                    hero.bind_remove++;
                    if (hero.bind_remove == 15)
                    {
                        hero.status = "Normal";
                        hero.bind_remove = 0;
                        angle = 0;
                        hero.basicAtk();
                    }
                }
                else
                {
                    angle = 0;
                    hero.basicAtk();
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                if (hero.status.Equals("Binded Head") || hero.status.Equals("Paralyzed"))
                {
                    hero.bind_remove++;
                    if (hero.bind_remove == 15)
                    {
                        hero.status = "Normal";
                        hero.bind_remove = 0;
                    }
                }
                else
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

                        foreach (Obstacle ob in room.obstacles)
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

                        if (room.doorSpace[(int)x, (int)y])
                        {
                            Door clickedDoor = new Door(this, -1, -1, 0, 0, 0, true, 0, 0, false, -1);
                            foreach (Door door in room.doors)
                            {
                                if ((Math.Sqrt(Math.Pow(door.x - x, 2) + Math.Pow(door.y - y, 2)) < 1) || (Math.Sqrt(Math.Pow((door.x + door.width - 1) - x, 2) + Math.Pow((door.y + door.height - 1) - y, 2)) < 1))
                                {
                                    // this is the correct door
                                    if (!door.switchClosed())
                                    {
                                        clickedDoor = door;
                                        break;
                                    }
                                }
                            }
                            if (clickedDoor.x != -1)
                            {
                                if(!clickedDoor.locked)
                                room.updateDrawingGrid(clickedDoor.getNegativeRoom());
                                room.updateDrawingGrid(clickedDoor.getPositiveRoom());
                            }
                        }
                    }
                    else { this.hero.specialAtk(); }
                }



                /*float x = (float)((e.X - form.ClientSize.Width / 2.0) / size + hero.x);
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

                    foreach (Obstacle ob in room.obstacles)
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

                    if (room.doorSpace[(int)x, (int)y])
                    {
                        Door clickedDoor = new Door(this, -1, -1, 0, 0, 0, true, 0, 0);
                        foreach (Door door in room.doors)
                        {
                            if ((Math.Sqrt(Math.Pow(door.x - x, 2) + Math.Pow(door.y - y, 2)) < 1) || (Math.Sqrt(Math.Pow((door.x + door.width - 1) - x, 2) + Math.Pow((door.y + door.height - 1) - y, 2)) < 1))
                            {
                                // this is the correct door
                                if (!door.switchClosed())
                                {
                                    clickedDoor = door;
                                    break;
                                }
                            }
                        }
                        if (clickedDoor.x != -1)
                        {
                            room.updateDrawingGrid(clickedDoor.getNegativeRoom());
                            room.updateDrawingGrid(clickedDoor.getPositiveRoom());
                        }
                    }
                }
                else { this.hero.specialAtk(); }*/
               
            }
        }

        public void spellChange() {

            int index = 0;
            int loop = 0;
            for (int i = 0; i < SkillStreeState.skillList; i++ )
            {
                if (SkillStreeState.spellSelected== null) { break; }
                if(SkillStreeState.spellSelected.Equals(SkillStreeState.spellStored[i]))
                    index = i+1;
            }
            
            for (int i = index; loop < SkillStreeState.skillList; i++)
            {
                i = i % SkillStreeState.skillList;
                if(GameState.heroSkill[i,0]){
                    SkillStreeState.spellSelected = SkillStreeState.spellStored[i];
                    break;
                }
                loop++;
            }
            
            

            
        }
        public override void mouseMove(object sender, MouseEventArgs e)
        {
            hero.dir = (float)Math.Atan2(e.Y - (form.ClientSize.Height / 2), e.X - (form.ClientSize.Width / 2));
            
            float x = (float)((e.X - form.ClientSize.Width / 2.0) / size + hero.x);
            float y = (float)((e.Y - form.ClientSize.Height / 2.0) / size + hero.y);
            mouseImg = null;
                
            foreach (Obstacle ob in room.obstacles)
                if (Math.Sqrt(Math.Pow(ob.x - x, 2) + Math.Pow(ob.y - y, 2)) < 1 && ob is Chest)
                {
                    Chest chest = (Chest)ob;
                    if (chest.closed)
                    {
                        mouseImg = mouseChestImg;
                        this.mouseX = e.X;
                        this.mouseY = e.Y;
                    }
                    break;
                }

            if (room.doorSpace[(int)x, (int)y])
            {
                if (Math.Sqrt(Math.Pow(x - hero.x, 2) + Math.Pow(y - hero.y, 2)) < 2)
                    mouseImg = mouseDoorImg;
                else
                    mouseImg = mouseNotDoorImg;
            }

            this.mouseX = e.X;
            this.mouseY = e.Y;
        }

        private Bitmap rotateImg(Bitmap oldBitmap, double angle)
        {
            Bitmap newBitmap = new Bitmap(oldBitmap.Width * 2, oldBitmap.Height * 2);
            Graphics g = Graphics.FromImage(newBitmap);
            g.TranslateTransform((float)oldBitmap.Width, (float)oldBitmap.Height);
            g.RotateTransform((float)angle + 45);
            g.TranslateTransform(-(float)oldBitmap.Width, -(float)oldBitmap.Height);
            g.DrawImage(oldBitmap, new Point(oldBitmap.Width, 0));
            return newBitmap;
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(20, 20, 20));

            room.draw(g);

            if (hero.weapon != null && angle >= 0)
            {
                double heroAngle = Math.Atan2(mouseY - form.ClientSize.Height / 2, mouseX - form.ClientSize.Width / 2);
                g.DrawImage(rotateImg(hero.weapon.img, (angle + heroAngle) * 180 / Math.PI - 45), form.ClientSize.Width / 2 - size, form.ClientSize.Height / 2 - size, size * 2, size * 2);
            }

            hero.draw(g);

            if(mouseImg != null)
                g.DrawImage(mouseImg, this.mouseX - mouseImg.Width / 2, this.mouseY - mouseImg.Height / 2);
        }

        public override void tick(object sender, EventArgs e)
        {
            if (angle >= 0)
                angle += Math.PI / 16;
            if (angle > Math.PI / 2)
                angle = -1;

            if(startTutorial)
                beginTutorial();

            hero.act();

            foreach (Unit unit in room.enemies)
            {
                unit.setDir();
                unit.act();
            }
            foreach (Projectile proj in room.projectiles)
                proj.act();

            foreach (Unit enemy in room.enemies)
                if (Math.Sqrt(Math.Pow(hero.x - enemy.x, 2) + Math.Pow(hero.y - enemy.y, 2)) < hero.radius + enemy.radius)
                    enemy.attackHero();

            form.Invalidate();
        }
    }
}
