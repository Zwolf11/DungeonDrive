using System;

namespace DungeonDrive
{
    public static class Logic
    {
        public static void resolveUnitCollisions()
        {
            // enemies attack champ

            if (!G.hero.alive) return;
            foreach(Unit enemy in G.room.enemies)
            {
                if(Math.Sqrt(Math.Pow(G.hero.x - enemy.x, 2) + Math.Pow(G.hero.y - enemy.y, 2)) < G.hero.radius + enemy.radius)
                {
                    if (enemy.atk_cd[0])
                    {
                        int dirX = Math.Sign(G.hero.x - enemy.x);
                        int dirY = Math.Sign(G.hero.y - enemy.y);
                        enemy.knockBack(G.hero, dirX * 0.05, dirY * 0.05, 0);
                        G.hero.hp -= enemy.atk_dmg;
                        enemy.sleep_sec = 0.5 * G.tickInt;
                        enemy.cd(1, 0);
                    }
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

                foreach (Projectile proj in G.hero.projectiles)
                    proj.act();

                resolveUnitCollisions();
            }

            G.form.Invalidate();
        }
    }
}
