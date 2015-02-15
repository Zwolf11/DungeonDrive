using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Unit
    {
        public double x;
        public double y;
        public double speed = 0.01;
        public double hp = 1;
        public double atk_dmg = 1;
        public int roomNum = -1;

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2); } }

        public Unit(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public abstract void act();
        public abstract void draw(Graphics g);

        public Boolean checkCollision(double x, double y)
        {
            foreach (Unit enemy in G.room.enemies)
                if ( enemy != this && Math.Sqrt(Math.Pow(x - enemy.x, 2) + Math.Pow(y - enemy.y, 2)) <= 1)
                    return false;

            if (!(this is Hero))
            {
                if (Math.Sqrt(Math.Pow(x - G.hero.x, 2) + Math.Pow(y - G.hero.y, 2)) <= 1)
                    return false;
            }
            else
            {
                // checks if the center of the hero is within a staircase.
                if (G.room.stairSpace[(int)(x + .5), (int)(y + .5)])
                {
                    foreach (Stairs stair in G.room.stairs)
                    {
                        if (stair.x == (int) (x + .5) && stair.y == (int) (y + .5))
                        {
                           // move to this room.
                            G.room = new Room(stair.path);
                        }
                    }
                }
            }
            // This is to detect collisions with the room walls and obstacles. - Jake
            if(x < (0) || x > (G.room.width - 1) || y < (0) || y > (G.room.height - 1) || !G.room.walkingSpace[(int)x + 1, (int)y + 1] || !G.room.walkingSpace[(int)x, (int)y] || !G.room.walkingSpace[(int)x, (int)y + 1] || !G.room.walkingSpace[(int)x + 1, (int)y])
                return false;

                return true;
        }
    }
}
