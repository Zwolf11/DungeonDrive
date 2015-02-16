using System;
using System.Drawing;
using System.Threading;

namespace DungeonDrive
{
    public abstract class Unit
    {
        public double x;
        public double y;
        public double speed = 0.01;
        public double hp = 1;
        public double atk_dmg = 1;
        public double atk_speed = 1;
        public bool stuned = false;
        public int roomNum = -1;

        public bool[] atk_cd = new bool[5];      // flags for different skill's availability

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2); } }

        public Unit(double x, double y)
        {
            this.x = x;
            this.y = y;
            for (int i = 0; i < atk_cd.Length; i++) atk_cd[i] = true;
        }

        public abstract void act();
        public abstract void draw(Graphics g);

        private void movSleep(double sec)
        {
            Thread.Sleep((int)sec * 1000);
            stuned = false;
        }

        private void atkSleep(double sec, int i)
        {
            atk_cd[i] = false;
            Thread.Sleep((int)sec * 1000);
            atk_cd[i] = true;
        }

        private void mov(double factor, double x_dist, double y_dist)
        {
            double x_final = x + x_dist;
            double y_final = y + y_dist;
            double avg_x = x_dist / factor;
            double avg_y = y_dist / factor;
            while (Math.Round(Math.Abs(x_final - x),6) <= Math.Abs(x_dist) && Math.Round(Math.Abs(y_final - y),6) <= Math.Abs(y_dist))
            {
                x += avg_x;
                y += avg_y;
            }
            stuned = false;
        }

        public void stun(double sec)
        {
            // disable moving flag for certain given seconds
            stuned = true;
            new Thread(() => movSleep(sec)).Start();
        }

        public void cd(double sec, int i)
        {
            // disable the attack boolean at index i for certain given seconds
            new Thread(() => atkSleep(sec, i)).Start();
        }

        public void knockBack(double factor, double x_dist, double y_dist)
        {
            stuned = true;
            Thread move = new Thread(() => mov(factor, x_dist, y_dist));
            move.Start();
        }

        public Boolean checkCollision(double x, double y)
        {
            foreach (Unit enemy in G.room.enemies)
                if ( enemy != this && Math.Sqrt(Math.Pow(x - enemy.x, 2) + Math.Pow(y - enemy.y, 2)) <= 1)
                    return false;

            if (!(this is Hero))
            {
                //Console.WriteLine("Enemy");
                if (Math.Sqrt(Math.Pow(x - G.hero.x, 2) + Math.Pow(y - G.hero.y, 2)) <= 1)
                    return false;
            }
            else
            {
                //Console.WriteLine("Hero");
                // checks if the center of the hero is within a staircase.
                if (G.room.stairSpace[(int)(x + .5), (int)(y + .5)])
                {
                    foreach (Stairs stair in G.room.stairs)
                    {
                        if (stair.x == (int) (x + .5) && stair.y == (int) (y + .5))
                        {
                           // move to this room.
                            G.room = new Room(stair.path);
                            return false;
                        }
                    }
                }
            }

            // This is to detect collisions with the room walls and obstacles. - Jake
            //Console.WriteLine("G.room.height = {0}, width = {1}, x = {2}, y = {3}", G.room.height, G.room.width, x, y);
//            if((x < 0) || (((int)(x+1)) > G.room.width - 1) || (y < 0) || (((int)(y+1)) > G.room.height - 1) || !G.room.walkingSpace[(int)(x + 1), (int)(y + 1)] || !G.room.walkingSpace[(int)x, (int)y] || !G.room.walkingSpace[(int)x, (int)(y + 1)] || !G.room.walkingSpace[(int)(x + 1), (int)y])
//                return false;

            return true;
        }
    }
}
