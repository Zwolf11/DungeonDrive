using System;

namespace DungeonDrive
{
    public static class Logic
    {
        public static void tick(object sender, EventArgs e)
        {
            G.hero.act();

            foreach (Unit unit in G.room.enemies)
                unit.act();

            G.form.Invalidate();
        }
    }
}
