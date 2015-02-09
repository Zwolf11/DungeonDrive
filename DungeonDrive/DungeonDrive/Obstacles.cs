﻿using System;
using System.Drawing;

namespace DungeonDrive
{
    public abstract class Obstacle
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public int DrawX { get { return (int)(x * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2); } }
        public int DrawY { get { return (int)(y * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2); } }

        public Obstacle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public abstract void draw(Graphics g);
    }

    public class Pillar : Obstacle
    {
        public Pillar(int x, int y, int width, int height) : base(x, y, width, height) { }

        public override void draw(Graphics g) { g.FillRectangle(Brushes.Gray, DrawX, DrawY, G.size * width, G.size * height); }
    }
}
