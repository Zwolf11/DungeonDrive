using System;
using System.Drawing;
using System.Media;

namespace DungeonDrive
{
    public abstract class Item
    {
        protected GameState state;
        public Bitmap img;
        public String name;
        public String description;
        public bool showDes = false;

        public Item(GameState state) { this.state = state; }

        public double rdnDouble(double first, double second, Random r)
        { return Math.Round(r.NextDouble() * (second - first) + first, 2); }
    }

    public class Helmet : Item
    {
        public int defense;

        public Helmet(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(1))
            {
                case 0:
                    name = "Basic Helmet";
                    img = Properties.Resources.helmet_1;
                    defense = 1 + rand.Next(state.hero.level);
                    break;
            }

            description = name + "\nDefense: " + defense;
        }

        public Helmet(GameState state, String name, int defense) : base(state)
        {
            switch(name)
            {
                case "Basic Helmet":
                    img = Properties.Resources.helmet_1;
                    break;
            }

            this.name = name;
            this.defense = defense;
            this.description = name + "\nDefense: " + defense;
        }
    }

    public class Armor : Item
    {
        public int defense;

        public Armor(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(1))
            {
                case 0:
                    name = "Basic Armor";
                    img = Properties.Resources.armor_1;
                    defense = 1 + rand.Next(state.hero.level);
                    break;
            }

            description = name + "\nDefense: " + defense;
        }

        public Armor(GameState state, String name, int defense) : base(state)
        {
            switch (name)
            {
                case "Basic Armor":
                    img = Properties.Resources.armor_1;
                    break;
            }

            this.name = name;
            this.defense = defense;
            this.description = name + "\nDefense: " + defense;
        }
    }

    public class Legs : Item
    {
        public int defense;

        public Legs(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(1))
            {
                case 0:
                    name = "Basic Legs";
                    img = Properties.Resources.legs_1;
                    defense = 1 + rand.Next(state.hero.level);
                    break;
            }

            description = name + "\nDefense: " + defense;
        }

        public Legs(GameState state, String name, int defense) : base(state)
        {
            switch (name)
            {
                case "Basic Legs":
                    img = Properties.Resources.legs_1;
                    break;
            }

            this.name = name;
            this.defense = defense;
            this.description = name + "\nDefense: " + defense;
        }
    }

    public class Shield : Item
    {
        public int defense;

        public Shield(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(2))
            {
                case 0:
                    name = "Basic Shield";
                    img = Properties.Resources.shield_1;
                    defense = 1 + rand.Next(state.hero.level);
                    break;
                case 1:
                    name = "Diamond Shield";
                    img = Properties.Resources.shield_2;
                    defense = 3 + rand.Next(state.hero.level);
                    break;
            }

            description = name + "\nDefense: " + defense;
        }

        public Shield(GameState state, String name, int defense) : base(state)
        {
            switch (name)
            {
                case "Basic Shield":
                    img = Properties.Resources.shield_2;
                    break;
                case "Diamond Shield":
                    img = Properties.Resources.shield_1;
                    break;
            }

            this.name = name;
            this.defense = defense;
            this.description = name + "\nDefense: " + defense;
        }
    }

    public class Weapon : Item
    {
        public int level;
        public int damage;
        public bool ranged;
        public double atk_speed;
        public double proj_speed;
        public int proj_range;
        public double powerSec;
        public double powerFac;
        public GameState.AtkStyle style;
        public Bitmap projectileImg = null;

        private void setDesc()
        {
            description = name
                + "\nLVL: " + level
                + "\nATT: " + style.ToString()
                + "\nDMG: " + damage
                + "\nATK SPD: " + atk_speed;

            if (ranged)
                description += "\nRNG: " + proj_range;

            switch (style)
            {
                case GameState.AtkStyle.Frozen:
                    description += "\nSLW SEC: " + powerSec + "\nSLW FAC: " + powerFac;
                    break;
                case GameState.AtkStyle.Flame:
                    description += "\nFLM SEC: " + powerSec + "\nFLM FAC: " + powerFac;
                    break;
                case GameState.AtkStyle.Doom:
                    description += "\nSLW SEC: " + powerSec + "\nSLW FAC: " + powerFac;
                    break;
                case GameState.AtkStyle.Lightening:
                    description += "\nFLM SEC: " + powerSec + "\nFLM FAC: " + powerFac;
                    break;
                case GameState.AtkStyle.Poison:
                    description += "\nSLW SEC: " + powerSec + "\nSLW FAC: " + powerFac;
                    break;
                
            }
        }

        private void setWpnImg()
        {
            Console.WriteLine("level: " + level);
            if (ranged)
            {
                if (level == 1 || level == 2)
                    this.img = Properties.Resources.wand_1;
                else if (level > 2 && level <= 5)
                    this.img = Properties.Resources.wand_2;
                else if (level > 5 && level <= 20)
                    this.img = Properties.Resources.wand_3;
                else if (level > 20 && level <= 45)
                    this.img = Properties.Resources.wand_4;
                else if (level > 45 && level <= 60)
                    this.img = Properties.Resources.wand_5;
                else if (level > 60 && level <= 75)
                    this.img = Properties.Resources.wand_6;
                else if (level > 75)
                    this.img = Properties.Resources.wand_7;
            }
        }

        private void setProjImg()
        {
            switch (style)
            {
                case GameState.AtkStyle.Frozen:
                    projectileImg = Properties.Resources.ice;
                    break;
                case GameState.AtkStyle.Flame:
                    projectileImg = Properties.Resources.fire;
                    break;
                case GameState.AtkStyle.Poison:
                    projectileImg = Properties.Resources.poison;
                    break;
                case GameState.AtkStyle.Doom:
                    projectileImg = Properties.Resources.doom;
                    break;
                case GameState.AtkStyle.Lightening:
                    projectileImg = Properties.Resources.lightening;
                    break;
                
                default:
                    projectileImg = Properties.Resources.proj;
                    break;
            }
        }

        public Weapon(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(1))
            {
                case 0:
                    name = state.adjectives[rand.Next(state.adjectives.Length)] + " Wand";
                    level = state.hero.level;
                    damage = 1 + (int)((double)state.hero.level * rdnDouble(0.5,0.8,rand));
                    atk_speed = rdnDouble(0.3 * Math.Pow(0.99, (double)state.hero.level), 0.6 * Math.Pow(0.99, (double)state.hero.level), rand);
                    proj_speed = rdnDouble(0.2 * Math.Pow(0.99, (double)state.hero.level), 0.8 * Math.Pow(0.99, (double)state.hero.level), rand);
                    proj_range = rand.Next(5, 12);
                    style = (GameState.AtkStyle)rand.Next(0,6);
                    powerSec = rdnDouble(0.5, 2.0, rand);
                    powerFac = rdnDouble(0.3, 0.5, rand);
                    ranged = true;
                    setProjImg();
                    break;
            }

            setWpnImg();
            setDesc();
        }

        public Weapon(GameState state, String name, int level, int damage, bool ranged, double atk_speed, double proj_speed, int proj_range, double powerSec, double powerFac, int style ) : base(state)
        {
            this.name = name;
            this.level = level;
            this.damage = damage;
            this.ranged = ranged;
            this.atk_speed = atk_speed;
            this.proj_speed = proj_speed;
            this.proj_range = proj_range;
            this.powerSec = powerSec;
            this.powerFac = powerFac;
            this.style = (GameState.AtkStyle)style;

            if (ranged)
                setProjImg();
            setWpnImg();
            setDesc();
        }
    }

    public abstract class Consumable : Item
    {
        public Consumable(GameState state, Bitmap img, String name, String description) : base(state)
        {
            this.img = img;
            this.name = name;
            this.description = description;
        }

        public abstract void use();
    }

    public class SmallPotion : Consumable
    {
        private SoundPlayer sound = new SoundPlayer(Properties.Resources.potion);

        public SmallPotion(GameState state) : base(state, Properties.Resources.HP_Potion_s, "Small Potion", "Small Potion (+10 HP)") { }

        public override void use()
        {
            int hpBonus = 10;
            if (state.hero.hp + hpBonus < state.hero.full_hp)
                state.hero.hp += hpBonus;
            else
                state.hero.hp = state.hero.full_hp;

            sound.Play();
        }
    }

    public class MediumPotion : Consumable
    {
        private SoundPlayer sound = new SoundPlayer(Properties.Resources.potion);

        public MediumPotion(GameState state) : base(state, Properties.Resources.HP_Potion_m, "Medium Potion", "Medium Potion (+30 HP)") { }

        public override void use()
        {
            int hpBonus = 30;
            if (state.hero.hp + hpBonus < state.hero.full_hp)
                state.hero.hp += hpBonus;
            else
                state.hero.hp = state.hero.full_hp;

            sound.Play();
        }
    }

    public class LargePotion : Consumable
    {
        private SoundPlayer sound = new SoundPlayer(Properties.Resources.potion2);

        public LargePotion(GameState state) : base(state, Properties.Resources.HP_Postion_g, "Large Potion", "Large Potion (+50 HP)") { }

        public override void use()
        {
            int hpBonus = 50;
            if (state.hero.hp + hpBonus < state.hero.full_hp)
                state.hero.hp += hpBonus;
            else
                state.hero.hp = state.hero.full_hp;

            sound.Play();
        }
    }
}
