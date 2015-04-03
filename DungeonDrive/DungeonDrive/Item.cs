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
                    img = Properties.Resources.shield_2;
                    defense = 1 + rand.Next(state.hero.level);
                    break;
                case 1:
                    name = "Diamond Shield";
                    img = Properties.Resources.shield_1;
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
        public int damage;
        public bool ranged;
        public double atk_speed;
        public double proj_speed;
        public int proj_range;
        public double powerSec;
        public double powerFac;
        public GameState.AtkStyle style;
        public Bitmap projectileImg = null;

        public Weapon(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(1))
            {
                case 0:
                    name = "Basic Wand";
                    img = Properties.Resources.wand_1;
                    atk_speed = rdnDouble(0.3 * Math.Pow(0.99, (double)state.hero.level), 0.6 * Math.Pow(0.99, (double)state.hero.level), rand);
                    proj_speed = rdnDouble(0.2 * Math.Pow(0.99, (double)state.hero.level), 0.8 * Math.Pow(0.99, (double)state.hero.level), rand);
                    proj_range = rand.Next(5, 12);
                    style = (GameState.AtkStyle)rand.Next(0,3);
                    switch (style)
                    {
                        case GameState.AtkStyle.Frozen:
                            projectileImg = Properties.Resources.ice;
                            break;
                        case GameState.AtkStyle.Flame:
                            projectileImg = Properties.Resources.fire;
                            break;
                        default:
                            projectileImg = Properties.Resources.fire;
                            break;
                    }
                    powerSec = rdnDouble(0.5, 2.0, rand);
                    powerFac = rdnDouble(0.3, 0.5, rand);
                    ranged = true;
                    damage = 1 + rand.Next(state.hero.level);
                    break;
            }

            description = name
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
            }
        }

        public Weapon(GameState state, String name, int damage) : base(state)
        {
            switch (name)
            {
                case "Basic Wand":
                    img = Properties.Resources.wand_1;
                    projectileImg = Properties.Resources.fire;
                    ranged = true;
                    break;
            }

            this.name = name;
            description = name + "\nDamage: " + damage + "\nRanged: " + ranged;
            this.damage = damage;
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
