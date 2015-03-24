﻿using System;
using System.Drawing;
using System.Threading;

namespace DungeonDrive
{
    public abstract class Unit
    {
        public double x;
        public double y;
        public double origin_x;
        public double origin_y;
        public double center_x;
        public double center_y;
        public double speed = 0.01;
        public double radius = 0.5;
        public double full_hp = 1;
        public double hp = 1;
        public double atk_dmg = 1;
        public double atk_speed = 1;
        public bool stuned = false;
        public bool teleport = false;
        public bool moving = false;
        public bool lunge = false;
        public int roomNum = -1;
        public double exp;
        public double expcap;
        public int level;

        public bool knockback = false;
        public double x_dist = 0;
        public double y_dist = 0;
        public double x_final = 0;
        public double y_final = 0;
        public double sleep_sec = 0;        // amount of time that unit can't move

        public bool[] atk_cd = new bool[5];      // flags for different skill's availability

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size * radius); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size * radius); } }

        public Unit(double x, double y)
        {
            this.x = x;
            this.y = y;
            for (int i = 0; i < atk_cd.Length; i++) atk_cd[i] = true;
        }

        public abstract void act();
        public abstract void draw(Graphics g);

        private void atkSleep(double sec, int i)
        {
            atk_cd[i] = false;
            Thread.Sleep((int)(sec * 1000));
            atk_cd[i] = true;
        }

        private void slowSleep(double sec, double fac)
        {
            this.speed *= fac;
            Thread.Sleep((int)(sec * 1000));
            this.speed /= fac;
        }

        public void cd(double sec, int i)
        {
            // disable the attack boolean at index i for certain given seconds
            new Thread(() => atkSleep(sec, i)).Start();
        }

        public void slow(double sec, double fac)
        {
            // slows down for sec second and factor of fac
            new Thread(() => slowSleep(sec, fac)).Start();
        }

        public void drawHpBar(Graphics g)
        {   g.FillRectangle(this.hp <= 0.4 * this.full_hp ? Brushes.Red : Brushes.Green, DrawX, DrawY - 5, (int)(radius * 2 * G.size * this.hp / this.full_hp), 2); }

        public void knockBack(Unit unit, double x_dist, double y_dist, double sleep_sec)
        {
            unit.x_dist = x_dist;
            unit.y_dist = y_dist;
            unit.x_final = unit.x + unit.x_dist;
            unit.y_final = unit.y + unit.y_dist;
            unit.sleep_sec = sleep_sec * G.tickInt;
            unit.knockback = true;
        }

        public void knockBacked()
        {
            int moves = 100;
            for (int i = 0; i < moves; i++)
            {
                if (tryMove(x + x_dist / moves, y + y_dist / moves) && (Math.Abs(x_final - x) <= Math.Abs(x_dist) || Math.Abs(y_final - y) <= Math.Abs(y_dist)))
                {
                    x += x_dist / 100;
                    y += y_dist / 100;
                }
                else
                    break;
            }
            knockback = false;
        }

        public bool tryMove(double xNext, double yNext)
        {
            int left = (int)(xNext - radius);
            int top = (int)(yNext - radius);
            int width = (int)(radius * 2 + (xNext - (int)xNext < radius || 1 - (xNext - (int)xNext) < radius ? 2 : 1));
            int height = (int)(radius * 2 + (yNext - (int)yNext < radius || 1 - (yNext - (int)yNext) < radius ? 2 : 1));

            bool canMove = left >= 0 && left + width < G.room.width && top >= 0 && top + height < G.room.height;

            for (int i = left; canMove && i < left + width; i++)
                for (int j = top; canMove && j < top + height; j++)
                    canMove = G.room.walkingSpace[i, j];

            if (this != G.hero)
            {
                foreach (Unit unit in G.room.enemies)
                    if (this != unit && Math.Sqrt(Math.Pow(xNext - unit.x, 2) + Math.Pow(yNext - unit.y, 2)) < radius + unit.radius)
                        return false;
            }
            else
            {
                foreach (Stairs stairs in G.room.stairs)
                    if (Math.Abs(stairs.x + 0.5 - x) < radius && Math.Abs(stairs.y + 0.5 - y) < radius)
                    {
                        G.room = new Room(stairs.path);
                        return false;
                    }
            }

            if (canMove)
            {
                x = xNext;
                y = yNext;
            }
            else
            {
                if ((int)(x - radius) > (int)(xNext - radius))
                {
                    x = (int)x + radius + 0.001;
                    tryMove(x, yNext);
                }
                else if ((int)(x + radius) < (int)(xNext + radius))
                {
                    x = (int)x + 1 - radius - 0.001;
                    tryMove(x, yNext);
                }

                if ((int)(y - radius) > (int)(yNext - radius))
                {
                    y = (int)y + radius + 0.001;
                    tryMove(xNext, y);
                }
                else if ((int)(y + radius) < (int)(yNext + radius))
                {
                    y = (int)y + 1 - radius - 0.001;
                    tryMove(xNext, y);
                }
            }

            return canMove;
        }
    }
}
