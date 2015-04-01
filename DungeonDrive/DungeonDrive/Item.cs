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

        public Item(GameState state, Bitmap img, String description)
        {
            this.state = state;
            this.img = img;
            this.description = description;
        }
    }

    public abstract class Helmet : Item
    {
        public int defense;

        public Helmet(GameState state, Bitmap img, String description, int defense) : base(state, img, description) { this.defense = defense; }
    }

    public abstract class Armor : Item
    {
        public int defense;

        public Armor(GameState state, Bitmap img, String description, int defense) : base(state, img, description) { this.defense = defense; }
    }

    public abstract class Legs : Item
    {
        public int defense;

        public Legs(GameState state, Bitmap img, String description, int defense) : base(state, img, description) { this.defense = defense; }
    }

    public abstract class Shield : Item
    {
        public int defense;

        public Shield(GameState state, Bitmap img, String description, int defense) : base(state, img, description) { this.defense = defense; }
    }

    public abstract class Weapon : Item
    {
        public int damage;
        public bool ranged;
        public Bitmap projectileImg = null;

        public Weapon(GameState state, Bitmap img, String description, int damage, bool ranged) : base(state, img, description)
        {
            this.damage = damage;
            this.ranged = ranged;
        }
    }

    public abstract class Consumable : Item
    {
        public Consumable(GameState state, Bitmap img, String description) : base(state, img, description) { }

        public abstract void use();
    }

    public class BasicShield : Shield
    {
        public BasicShield(GameState state) : base(state, Properties.Resources.shield_2, "Basic Shield", 3) { }
    }

    public class DiamondShield : Shield
    {
        public DiamondShield(GameState state) : base(state, Properties.Resources.shield_1, "Diamond Shield", 3) { }
    }

    public class Wand : Weapon
    {
        public Wand(GameState state) : base(state, Properties.Resources.wand_1, "Wand", 10, true)
        {
            projectileImg = Properties.Resources.fire;
        }
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
