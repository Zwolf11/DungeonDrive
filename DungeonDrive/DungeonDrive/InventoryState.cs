using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    public class InventoryState : State
    {
        private Item selection = null;
        private PointF selectOrigin = new PointF(-1, -1);
        private Point dragLoc = new Point(-1, -1);
        private Point mouse = new Point(-1, -1);
        private bool dragging = false;
        private GameState state { get { return (GameState)parent; } }

        private Bitmap boxImg = new Bitmap(Properties.Resources.box);
        private Bitmap helmetImg = new Bitmap(Properties.Resources.helmet);
        private Bitmap armorImg = new Bitmap(Properties.Resources.armor);
        private Bitmap legsImg = new Bitmap(Properties.Resources.legs);
        private Bitmap shieldImg = new Bitmap(Properties.Resources.shield);
        private Bitmap weaponImg = new Bitmap(Properties.Resources.weapon);
        private Bitmap infoImg = new Bitmap(Properties.Resources.info);

        public InventoryState(MainForm form) : base(form) { }

        private RectangleF getBoxBounds(float i, float j)
        {
            Item[][] inventory = state.inventory;
            float boxSize = form.ClientSize.Height / (inventory.Length + 4);
            float padding = form.ClientSize.Height / 300;

            float left = form.ClientSize.Width / 2.0f - (inventory.Length + 1.5f) / 2.0f * boxSize + (i + 2) * boxSize + padding;
            float top = (j + 2) * boxSize + padding;
            float size = boxSize - 2 * padding;

            return new RectangleF(left, top, size, size);
        }

        public override void keyUp(object sender, KeyEventArgs e) { }
        public override void tick(object sender, EventArgs e) { }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            Item[][] inventory = state.inventory;
            Rectangle click = new Rectangle(e.X, e.Y, 1, 1);

            for(int i=0;i<inventory.Length;i++)
                for(int j=0;j<inventory[i].Length;j++)
                    if(getBoxBounds(i, j).Contains(click))
                    {
                        selection = inventory[i][j];
                        inventory[i][j] = null;
                        selectOrigin = new Point(i, j);
                        dragLoc = new Point(e.X, e.Y);

                        mouse = e.Location;
                        form.Invalidate();
                        return;
                    }

            if (getBoxBounds(-1, 0.5f).Contains(click))
            {
                if (state.hero.shield != null)
                {
                    selection = state.hero.shield;
                    state.hero.shield = null;
                    selectOrigin = new Point(-1, 1);
                }
            }
            else if (getBoxBounds(-2, 0).Contains(click))
            {
                if (state.hero.helmet != null)
                {
                    selection = state.hero.helmet;
                    state.hero.helmet = null;
                    selectOrigin = new Point(-2, 0);
                }
            }
            else if (getBoxBounds(-2, 1).Contains(click))
            {
                if (state.hero.armor != null)
                {
                    selection = state.hero.armor;
                    state.hero.armor = null;
                    selectOrigin = new Point(-2, 1);
                }
            }
            else if (getBoxBounds(-2, 2).Contains(click))
            {
                if (state.hero.legs != null)
                {
                    selection = state.hero.legs;
                    state.hero.legs = null;
                    selectOrigin = new Point(-2, 2);
                }
            }
            else if (getBoxBounds(-3, 0.5f).Contains(click))
            {
                if (state.hero.weapon != null)
                {
                    selection = state.hero.weapon;
                    state.hero.weapon = null;
                    selectOrigin = new PointF(-3, 0.5f);
                }
            }

            dragLoc = new Point(e.X, e.Y);
            mouse = e.Location;
            form.Invalidate();
        }

        public override void mouseUp(object sender, MouseEventArgs e)
        {
            if (selection != null)
            {
                if (!dragging)
                {
                    Item[][] inventory = state.inventory;

                    if (selection is Weapon)
                    {
                        if (state.hero.weapon != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.weapon;
                        state.hero.weapon = (Weapon)selection;
                    }
                    else if (selection is Helmet)
                    {
                        if (state.hero.helmet != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.helmet;
                        state.hero.helmet = (Helmet)selection;
                    }
                    else if (selection is Armor)
                    {
                        if (state.hero.armor != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.armor;
                        state.hero.armor = (Armor)selection;
                    }
                    else if (selection is Legs)
                    {
                        if (state.hero.legs != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.legs;
                        state.hero.legs = (Legs)selection;
                    }
                    else if (selection is Shield)
                    {
                        if (state.hero.shield != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.shield;
                        state.hero.shield = (Shield)selection;
                    }
                    else if (selection is Consumable)
                    {
                        Consumable item = (Consumable)selection;
                        item.use();
                    }
                    selection = null;
                }
                else
                {
                    Item[][] inventory = state.inventory;
                    Rectangle click = new Rectangle(e.X, e.Y, 1, 1);

                    for (int i = 0; i < inventory.Length; i++)
                        for (int j = 0; j < inventory[i].Length; j++)
                            if (getBoxBounds(i, j).Contains(click))
                            {
                                if (inventory[i][j] != null)
                                    inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = inventory[i][j];
                                inventory[i][j] = selection;
                                selection = null;

                                dragging = false;
                                form.Invalidate();
                                return;
                            }

                    if (selection is Shield && getBoxBounds(-1, 0.5f).Contains(click))
                    {
                        if (state.hero.shield != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.shield;
                        state.hero.shield = (Shield)selection;
                    }
                    else if (selection is Helmet && getBoxBounds(-2, 0).Contains(click))
                    {
                        if (state.hero.helmet != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.helmet;
                        state.hero.helmet = (Helmet)selection;
                    }
                    else if (selection is Armor && getBoxBounds(-2, 1).Contains(click))
                    {
                        if (state.hero.armor != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.armor;
                        state.hero.armor = (Armor)selection;
                    }
                    else if (selection is Legs && getBoxBounds(-2, 2).Contains(click))
                    {
                        if (state.hero.legs != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.legs;
                        state.hero.legs = (Legs)selection;
                    }
                    else if (selection is Weapon && getBoxBounds(-3, 0.5f).Contains(click))
                    {
                        if (state.hero.weapon != null)
                            inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = state.hero.weapon;
                        state.hero.weapon = (Weapon)selection;
                    }
                    else
                    {
                        inventory[(int)selectOrigin.X][(int)selectOrigin.Y] = selection;
                    }
                    selection = null;
                }

                dragging = false;
                form.Invalidate();
            }
        }

        public override void mouseMove(object sender, MouseEventArgs e)
        {
            if (selection != null)
            {
                mouse = e.Location;
                if (!dragging && Math.Sqrt(Math.Pow(e.X - dragLoc.X, 2) + Math.Pow(e.Y - dragLoc.Y, 2)) > 20)
                    dragging = true;

                form.Invalidate();
            }
        }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey || e.KeyCode == Properties.Settings.Default.InventoryKey)
                this.close();
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Item[][] inventory = state.inventory;
            int boxSize = form.ClientSize.Height / (inventory.Length + 4);

            for (int i = 0; i < inventory.Length; i++)
                for (int j = 0; j < inventory[i].Length; j++)
                {
                    g.DrawImage(boxImg, getBoxBounds(i, j));

                    if(inventory[i][j] != null)
                        g.DrawImage(inventory[i][j].img, getBoxBounds(i, j));
                }

            g.DrawImage(shieldImg, getBoxBounds(-1, 0.5f));
            if(state.hero.shield != null)
                g.DrawImage(state.hero.shield.img, getBoxBounds(-1, 0.5f));
            g.DrawImage(armorImg, getBoxBounds(-2, 1));
            if (state.hero.armor != null)
                g.DrawImage(state.hero.armor.img, getBoxBounds(-2, 1));
            g.DrawImage(helmetImg, getBoxBounds(-2, 0));
            if (state.hero.helmet != null)
                g.DrawImage(state.hero.helmet.img, getBoxBounds(-2, 0));
            g.DrawImage(legsImg, getBoxBounds(-2, 2));
            if (state.hero.legs != null)
                g.DrawImage(state.hero.legs.img, getBoxBounds(-2, 2));
            g.DrawImage(weaponImg, getBoxBounds(-3, 0.5f));
            if (state.hero.weapon != null)
                g.DrawImage(state.hero.weapon.img, getBoxBounds(-3, 0.5f));

            RectangleF infoBox = getBoxBounds(-3, inventory.Length - 2);
            infoBox.Width *= 3;
            infoBox.Height *= 2;
            g.DrawImage(infoImg, infoBox);

            if (selection != null)
            {
                Font font = new Font("Arial", form.ClientSize.Height / 60);
                g.DrawString(selection.description, font, Brushes.White, new PointF(infoBox.X + infoBox.Width / 20, infoBox.Y + infoBox.Height / 20));
            }

            if (selection != null)
            {
                if(dragging)
                    g.DrawImage(selection.img, mouse.X - boxSize / 2, mouse.Y - boxSize / 2, boxSize, boxSize);
                else
                    g.DrawImage(selection.img, getBoxBounds((int)selectOrigin.X, (int)selectOrigin.Y));
            }
        }
    }
}
