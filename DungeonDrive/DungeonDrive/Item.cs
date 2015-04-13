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
        public int level;
        public String description;
        public bool showDes = false;

        public Item(GameState state) { this.state = state; }

        public double rdnDouble(double first, double second, Random r)
        { return r.NextDouble() * (second - first) + first; }

        public String genName(int level)
        {
            String name = "";
            if (level == 1 || level == 2)
                name = state.tierAdj[0];
            else if (level > 2 && level <= 5)
                name = state.tierAdj[1];
            else if (level > 5 && level <= 10)
                name = state.tierAdj[2];
            else if (level > 10 && level <= 15)
                name = state.tierAdj[3];
            else if (level > 15 && level <= 20)
                name = state.tierAdj[4];
            else if (level > 20 && level <= 25)
                name = state.tierAdj[5];
            else if (level > 25 && level <= 30)
                name = state.tierAdj[6];
            else if (level > 30 && level <= 35)
                name = state.tierAdj[7];
            else if (level > 35 && level <= 40)
                name = state.tierAdj[8];
            else if (level > 40 && level <= 45)
                name = state.tierAdj[9];
            else if (level > 45 && level <= 50)
                name = state.tierAdj[10];
            else if (level > 50 && level <= 55)
                name = state.tierAdj[11];
            else if (level > 55 && level <= 60)
                name = state.tierAdj[12];
            else if (level > 60 && level <= 65)
                name = state.tierAdj[13];
            else if (level > 65 && level <= 70)
                name = state.tierAdj[14];
            else if (level > 70)
                name = state.tierAdj[15];

            return name;
        }
    }

    public class Helmet : Item
    {
        public double hp;
        public double hp_reg;

        public Helmet(GameState state)
            : base(state)
        {
            Random rand = new Random();
            level = state.hero.level;
            name = state.adjectives[rand.Next(state.adjectives.Length)] + " " + genName(level);

            switch (rand.Next(1))
            {
                case 0:
                    name += " Helmet";
                    hp = rdnDouble(0.1 * state.hero.base_full_hp * Math.Pow(1.01, (double)state.hero.level), 0.2 * state.hero.base_full_hp * Math.Pow(1.01, (double)state.hero.level), rand);
                    hp_reg = rdnDouble(0.01 * Math.Pow(1.1, (double)state.hero.level), 0.03 * Math.Pow(1.1, (double)state.hero.level), rand);
                    setImg();
                    break;
            }

            setDesc();
        }

        public Helmet(GameState state, String name, int level, double hp, double hp_reg)
            : base(state)
        {
            this.name = name;
            this.level = level;
            this.hp = hp;
            this.hp_reg = hp_reg;
            setImg();
            setDesc();
        }

        private void setImg()
        {
            if (level == 1 || level == 2)
                this.img = Properties.Resources.helmet_1;
            else if (level > 2 && level <= 5)
                this.img = Properties.Resources.helmet_2;
            else if (level > 5 && level <= 10)
                this.img = Properties.Resources.helmet_3;
            else if (level > 10 && level <= 15)
                this.img = Properties.Resources.helmet_4;
            else if (level > 15 && level <= 20)
                this.img = Properties.Resources.helmet_5;
            else if (level > 20 && level <= 25)
                this.img = Properties.Resources.helmet_6;
            else if (level > 25 && level <= 30)
                this.img = Properties.Resources.helmet_7;
            else if (level > 30 && level <= 35)
                this.img = Properties.Resources.helmet_8;
            else if (level > 35 && level <= 40)
                this.img = Properties.Resources.helmet_9;
            else if (level > 40 && level <= 45)
                this.img = Properties.Resources.helmet_10;
            else if (level > 45 && level <= 50)
                this.img = Properties.Resources.helmet_11;
            else if (level > 50 && level <= 55)
                this.img = Properties.Resources.helmet_12;
            else if (level > 55 && level <= 60)
                this.img = Properties.Resources.helmet_13;
            else if (level > 60 && level <= 65)
                this.img = Properties.Resources.helmet_14;
            else if (level > 65 && level <= 70)
                this.img = Properties.Resources.helmet_15;
            else if (level > 70)
                this.img = Properties.Resources.helmet_16;
        }

        private void setDesc()
        {
            description = name
                + "\nLVL:  " + level
                + "\nHP : +" + Math.Round(hp,2)
                + "\nHP REG: +" + Math.Round(hp_reg,2);
        }
    }

    public class Armor : Item
    {
        public double hp;

        public Armor(GameState state)
            : base(state)
        {
            Random rand = new Random();
            level = state.hero.level;
            name = state.adjectives[rand.Next(state.adjectives.Length)] + " " + genName(level);

            switch (rand.Next(1))
            {
                case 0:
                    name += " Armor";
                    hp = rdnDouble(0.3 * state.hero.base_full_hp * Math.Pow(1.01, (double)state.hero.level), 0.6 * state.hero.base_full_hp * Math.Pow(1.01, (double)state.hero.level), rand);
                    setImg();
                    break;
            }

            setDesc();
        }

        public Armor(GameState state, String name, int level, double hp)
            : base(state)
        {
            this.name = name;
            this.level = level;
            this.hp = hp;
            setImg();
            setDesc();
        }

        private void setImg()
        {
            if (level == 1 || level == 2)
                this.img = Properties.Resources.armor_1;
            else if (level > 2 && level <= 5)
                this.img = Properties.Resources.armor_2;
            else if (level > 5 && level <= 10)
                this.img = Properties.Resources.armor_3;
            else if (level > 10 && level <= 15)
                this.img = Properties.Resources.armor_4;
            else if (level > 15 && level <= 20)
                this.img = Properties.Resources.armor_5;
            else if (level > 20 && level <= 25)
                this.img = Properties.Resources.armor_6;
            else if (level > 25 && level <= 30)
                this.img = Properties.Resources.armor_7;
            else if (level > 30 && level <= 35)
                this.img = Properties.Resources.armor_8;
            else if (level > 35 && level <= 40)
                this.img = Properties.Resources.armor_9;
            else if (level > 40 && level <= 45)
                this.img = Properties.Resources.armor_10;
            else if (level > 45 && level <= 50)
                this.img = Properties.Resources.armor_11;
            else if (level > 50 && level <= 55)
                this.img = Properties.Resources.armor_12;
            else if (level > 55 && level <= 60)
                this.img = Properties.Resources.armor_13;
            else if (level > 60 && level <= 65)
                this.img = Properties.Resources.armor_14;
            else if (level > 65 && level <= 70)
                this.img = Properties.Resources.armor_15;
            else if (level > 70)
                this.img = Properties.Resources.armor_16;
        }

        private void setDesc()
        {
            description = name
                + "\nLVL:  " + level
                + "\nHP : +" + Math.Round(hp,2);
        }
    }

    public class Legs : Item
    {
        public double hp;
        public double ms;

        public Legs(GameState state) : base(state)
        {
            Random rand = new Random();
            level = state.hero.level;
            name = state.adjectives[rand.Next(state.adjectives.Length)] + " " + genName(level);

            switch(rand.Next(1))
            {
                case 0:
                    name += " Legs";
                    hp = rdnDouble(0.1 * state.hero.base_full_hp * Math.Pow(1.01, (double)state.hero.level), 0.2 * state.hero.base_full_hp * Math.Pow(1.01, (double)state.hero.level), rand);
                    ms = rdnDouble(0.1 * Math.Pow(1.001, (double)state.hero.level), 0.3 * Math.Pow(1.001, (double)state.hero.level), rand);
                    setImg();
                    break;
            }

            setDesc();
        }

        public Legs(GameState state, String name, int level, double hp, double ms) : base(state)
        {
            this.name = name;
            this.level = level;
            this.hp = hp;
            this.ms = ms;
            setImg();
            setDesc();
        }

        private void setImg()
        {
            if (level == 1 || level == 2)
                this.img = Properties.Resources.legs_1;
            else if (level > 2 && level <= 5)
                this.img = Properties.Resources.legs_2;
            else if (level > 5 && level <= 10)
                this.img = Properties.Resources.legs_3;
            else if (level > 10 && level <= 15)
                this.img = Properties.Resources.legs_4;
            else if (level > 15 && level <= 20)
                this.img = Properties.Resources.legs_5;
            else if (level > 20 && level <= 25)
                this.img = Properties.Resources.legs_6;
            else if (level > 25 && level <= 30)
                this.img = Properties.Resources.legs_7;
            else if (level > 30 && level <= 35)
                this.img = Properties.Resources.legs_8;
            else if (level > 35 && level <= 40)
                this.img = Properties.Resources.legs_9;
            else if (level > 40 && level <= 45)
                this.img = Properties.Resources.legs_10;
            else if (level > 45 && level <= 50)
                this.img = Properties.Resources.legs_11;
            else if (level > 50 && level <= 55)
                this.img = Properties.Resources.legs_12;
            else if (level > 55 && level <= 60)
                this.img = Properties.Resources.legs_13;
            else if (level > 60 && level <= 65)
                this.img = Properties.Resources.legs_14;
            else if (level > 65 && level <= 70)
                this.img = Properties.Resources.legs_15;
            else if (level > 70)
                this.img = Properties.Resources.legs_16;
        }

        private void setDesc()
        {
            description = name 
                + "\nLVL:  " + level
                + "\nHP : +" + Math.Round(hp,2)
                + "\nMS : +" + Math.Round(ms,2);
        }
    }

    public class Shield : Item
    {
        public Shield(GameState state) : base(state)
        {
            Random rand = new Random();
            level = state.hero.level;
            name = state.adjectives[rand.Next(state.adjectives.Length)] + " " + genName(level);

            switch(rand.Next(1))
            {
                case 0:
                    name += " Shield";
                    setImg();
                    break;
            }

            setDesc();
        }

        public Shield(GameState state, String name, int level) : base(state)
        {
            this.name = name;
            this.level = level;
            setImg();
            setDesc();
        }

        private void setImg()
        {
            if (level == 1 || level == 2)
                this.img = Properties.Resources.shield_1;
            else if (level > 2 && level <= 5)
                this.img = Properties.Resources.shield_2;
            else if (level > 5 && level <= 10)
                this.img = Properties.Resources.shield_3;
            else if (level > 10 && level <= 15)
                this.img = Properties.Resources.shield_4;
            else if (level > 15 && level <= 20)
                this.img = Properties.Resources.shield_5;
            else if (level > 20 && level <= 25)
                this.img = Properties.Resources.shield_6;
            else if (level > 25 && level <= 30)
                this.img = Properties.Resources.shield_7;
            else if (level > 30 && level <= 35)
                this.img = Properties.Resources.shield_8;
            else if (level > 35 && level <= 40)
                this.img = Properties.Resources.shield_9;
            else if (level > 40 && level <= 45)
                this.img = Properties.Resources.shield_10;
            else if (level > 45 && level <= 50)
                this.img = Properties.Resources.shield_11;
            else if (level > 50 && level <= 55)
                this.img = Properties.Resources.shield_12;
            else if (level > 55 && level <= 60)
                this.img = Properties.Resources.shield_13;
            else if (level > 60 && level <= 65)
                this.img = Properties.Resources.shield_14;
            else if (level > 65 && level <= 70)
                this.img = Properties.Resources.shield_15;
            else if (level > 70)
                this.img = Properties.Resources.shield_16;
        }

        private void setDesc()
        {
            description = name 
                + "\nLVL:  " + level;
        }
    }

    public class Weapon : Item
    {
        public double damage;
        public bool ranged;
        public double atk_speed;
        public double proj_speed;
        public int proj_range;
        public double powerSec;
        public double powerFac;
        public GameState.AtkStyle style;
        public Bitmap projectileImg = null;

        public Weapon(GameState state)
            : base(state)
        {
            Random rand = new Random();
            level = state.hero.level;
            name = state.adjectives[rand.Next(state.adjectives.Length)] + " " + genName(level);

            switch (rand.Next(2))
            {
                case 0:
                    name += " Wand";
                    damage = 1 + (int)((double)state.hero.level * rdnDouble(1.5, 1.8, rand));
                    atk_speed = rdnDouble(0.3 * Math.Pow(0.99, (double)state.hero.level), 0.6 * Math.Pow(0.99, (double)state.hero.level), rand);
                    proj_speed = rdnDouble(0.2 * Math.Pow(0.99, (double)state.hero.level), 0.8 * Math.Pow(0.99, (double)state.hero.level), rand);
                    proj_range = rand.Next(5, 12);
                    style = (GameState.AtkStyle)rand.Next(0, 6);
                    powerSec = rdnDouble(0.5, 2.0, rand);
                    powerFac = rdnDouble(0.3, 0.5, rand);
                    ranged = true;
                    setProjImg();
                    break;
                case 1:
                    name += " Sword";
                    damage = state.hero.atk_dmg + (int)((double)state.hero.level * rdnDouble(1.5, 1.8, rand));
                    atk_speed = state.hero.atk_speed - rdnDouble(0.001 * Math.Pow(1.01, (double)state.hero.level), 0.005 * Math.Pow(1.01, (double)state.hero.level), rand);
                    style = (GameState.AtkStyle)rand.Next(0, 6);
                    powerSec = rdnDouble(0.8, 2.3, rand);
                    powerFac = rdnDouble(0.6, 0.8, rand);
                    ranged = false;
                    break;
            }

            setImg();
            setDesc();
        }

        public Weapon(GameState state, String name, int level, double damage, bool ranged, double atk_speed, double proj_speed, int proj_range, double powerSec, double powerFac, int style)
            : base(state)
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
            setImg();
            setDesc();
        }

        private void setDesc()
        {
            description = name
                + "\nLVL:  " + level
                + "\nATT:  " + style.ToString()
                + "\nDMG:  " + Math.Round(damage,2)
                + "\nATK SPD:  " + Math.Round(atk_speed,2);

            if (ranged)
                description += "\nRNG:  " + proj_range;

            switch (style)
            {
                case GameState.AtkStyle.Frozen:
                    description += "\nSLW SEC:  " + Math.Round(powerSec,2) + "\nSLW FAC:  " + Math.Round(powerFac,2);
                    break;
                case GameState.AtkStyle.Flame:
                    description += "\nFLM SEC:  " + Math.Round(powerSec,2) + "\nFLM FAC:  " + Math.Round(powerFac,2);
                    break;
                case GameState.AtkStyle.Doom:
                    description += "\nSLW SEC:  " + Math.Round(powerSec,2) + "\nSLW FAC:  " + Math.Round(powerFac,2);
                    break;
                case GameState.AtkStyle.Lightening:
                    description += "\nFLM SEC:  " + Math.Round(powerSec,2) + "\nFLM FAC:  " + Math.Round(powerFac,2);
                    break;
                case GameState.AtkStyle.Poison:
                    description += "\nSLW SEC:  " + Math.Round(powerSec,2) + "\nSLW FAC:  " + Math.Round(powerFac,2);
                    break;
            }
        }

        private void setImg()
        {
            if (ranged)
            {
                if (level == 1 || level == 2)
                    this.img = Properties.Resources.wand_1;
                else if (level > 2 && level <= 5)
                    this.img = Properties.Resources.wand_2;
                else if (level > 5 && level <= 10)
                    this.img = Properties.Resources.wand_3;
                else if (level > 10 && level <= 15)
                    this.img = Properties.Resources.wand_4;
                else if (level > 15 && level <= 20)
                    this.img = Properties.Resources.wand_5;
                else if (level > 20 && level <= 25)
                    this.img = Properties.Resources.wand_6;
                else if (level > 25 && level <= 30)
                    this.img = Properties.Resources.wand_7;
                else if (level > 30 && level <= 35)
                    this.img = Properties.Resources.wand_8;
                else if (level > 35 && level <= 40)
                    this.img = Properties.Resources.wand_9;
                else if (level > 40 && level <= 45)
                    this.img = Properties.Resources.wand_10;
                else if (level > 45 && level <= 50)
                    this.img = Properties.Resources.wand_11;
                else if (level > 50 && level <= 55)
                    this.img = Properties.Resources.wand_12;
                else if (level > 55 && level <= 60)
                    this.img = Properties.Resources.wand_13;
                else if (level > 60 && level <= 65)
                    this.img = Properties.Resources.wand_14;
                else if (level > 65 && level <= 70)
                    this.img = Properties.Resources.wand_15;
                else if (level > 70)
                    this.img = Properties.Resources.wand_16;
            }
            else
            {
                if (level == 1 || level == 2)
                    this.img = Properties.Resources.sword_1;
                else if (level > 2 && level <= 5)
                    this.img = Properties.Resources.sword_2;
                else if (level > 5 && level <= 10)
                    this.img = Properties.Resources.sword_3;
                else if (level > 10 && level <= 15)
                    this.img = Properties.Resources.sword_4;
                else if (level > 15 && level <= 20)
                    this.img = Properties.Resources.sword_5;
                else if (level > 20 && level <= 25)
                    this.img = Properties.Resources.sword_6;
                else if (level > 25 && level <= 30)
                    this.img = Properties.Resources.sword_7;
                else if (level > 30 && level <= 35)
                    this.img = Properties.Resources.sword_8;
                else if (level > 35 && level <= 40)
                    this.img = Properties.Resources.sword_9;
                else if (level > 40 && level <= 45)
                    this.img = Properties.Resources.sword_10;
                else if (level > 45 && level <= 50)
                    this.img = Properties.Resources.sword_11;
                else if (level > 50 && level <= 55)
                    this.img = Properties.Resources.sword_12;
                else if (level > 55 && level <= 60)
                    this.img = Properties.Resources.sword_13;
                else if (level > 60 && level <= 65)
                    this.img = Properties.Resources.sword_14;
                else if (level > 65 && level <= 70)
                    this.img = Properties.Resources.sword_15;
                else if (level > 70)
                    this.img = Properties.Resources.sword_16;
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
