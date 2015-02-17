using System;

namespace DungeonDrive
{
    public static class Logic
    {
        public static void resolveUnitCollisions()
        {
            foreach(Unit enemy in G.room.enemies)
            {
                if(Math.Sqrt(Math.Pow(G.hero.x - enemy.x, 2) + Math.Pow(G.hero.y - enemy.y, 2)) < G.hero.radius + enemy.radius)
                {
                    int dirX = Math.Sign(G.hero.x - enemy.x);
                    int dirY = Math.Sign(G.hero.y - enemy.y);
                    //G.hero.knockBack(0.5, dirX, dirY); //This is knocking the hero back way too far. Can't figure out why
                }
            }
        }

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

                resolveUnitCollisions();
            }

            G.form.Invalidate();
        }
    }
}
