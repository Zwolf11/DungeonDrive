using System;
using System.Drawing;
using System.Media;

namespace DungeonDrive
{
    public abstract class Item
    {
        protected GameState state;
        public Bitmap img;
        public String description;
        public bool showDes = false;

        public Item(GameState state) { this.state = state; }
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
                    this.img = Properties.Resources.helmet_1;
                    defense = 1 + rand.Next(state.hero.level);
                    this.description = "Basic Helmet\nDefense: " + defense;
                    break;
            }
        }

        public Helmet(GameState state, Bitmap img, String description, int defense) : base(state)
        {
            this.img = img;
            this.description = description;
            this.defense = defense;
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
                    this.img = Properties.Resources.armor_1;
                    defense = 1 + rand.Next(state.hero.level);
                    this.description = "Basic Armor\nDefense: " + defense;
                    break;
            }
        }

        public Armor(GameState state, Bitmap img, String description, int defense) : base(state)
        {
            this.img = img;
            this.description = description;
            this.defense = defense;
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
                    this.img = Properties.Resources.legs_1;
                    defense = 1 + rand.Next(state.hero.level);
                    this.description = "Basic Legs\nDefense: " + defense;
                    break;
            }
        }

        public Legs(GameState state, Bitmap img, String description, int defense) : base(state)
        {
            this.img = img;
            this.description = description;
            this.defense = defense;
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
                    this.img = Properties.Resources.shield_2;
                    defense = 1 + rand.Next(state.hero.level);
                    this.description = "Basic Shield\nDefense: " + defense;
                    break;
                case 1:
                    this.img = Properties.Resources.shield_1;
                    defense = 3 + rand.Next(state.hero.level);
                    this.description = "Diamond Shield\nDefense: " + defense;
                    break;
            }
        }

        public Shield(GameState state, Bitmap img, String description, int defense) : base(state)
        {
            this.img = img;
            this.description = description;
            this.defense = defense;
        }
    }

    public class Weapon : Item
    {
        public int damage;
        public bool ranged;
        public Bitmap projectileImg = null;

        public Weapon(GameState state) : base(state)
        {
            Random rand = new Random();

            switch(rand.Next(1))
            {
                case 0:
                    this.img = Properties.Resources.wand_1;
                    projectileImg = Properties.Resources.fire;
                    damage = 1 + rand.Next(state.hero.level);
                    ranged = true;
                    this.description = "Basic Wand\nDamage: " + damage + "\nRanged: " + ranged;
                    break;
            }
        }

        public Weapon(GameState state, Bitmap img, String description, int damage, bool ranged) : base(state)
        {
            this.img = img;
            this.description = description;
            this.damage = damage;
            this.ranged = ranged;
        }
    }

    public abstract class Consumable : Item
    {
        public Consumable(GameState state, Bitmap img, String description) : base(state)
        {
            this.img = img;
            this.description = description;
        }

        public abstract void use();
    }

    public class SmallPotion : Consumable
    {
        private SoundPlayer sound = new SoundPlayer(Properties.Resources.potion);

        public SmallPotion(GameState state) : base(state, Properties.Resources.HP_Potion_s, "Small Potion (+10 HP)") { }

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

        public MediumPotion(GameState state) : base(state, Properties.Resources.HP_Potion_m, "Medium Potion (+30 HP)") { }

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

        public LargePotion(GameState state) : base(state, Properties.Resources.HP_Postion_g, "Large Potion (+50 HP)") { }

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
