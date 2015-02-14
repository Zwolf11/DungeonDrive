using System;

namespace DungeonDrive
{
    public static class Logic
    {
        public static void tick(object sender, EventArgs e)
        {
            if (G.newRoom)              // boolean flagged when new level is loaded.
            {
                System.Threading.Thread.Sleep(500);                       // the thread sleeps for 500 milliseconds when a new level is entered. Without it, the hero sometimes moves right back up the stairs.
                G.newRoom = false;
            }
            else
            {

                G.hero.act();

                foreach (Unit unit in G.room.enemies)
                    unit.act();
            }

            G.form.Invalidate();
        }
    }
}
