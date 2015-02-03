using System;
using System.Collections.Generic;
using System.Drawing;

namespace DungeonDrive
{
    public class Room
    {
        public int width;
        public int height;
        public List<Obstacle> obstacles = new List<Obstacle>();
        public List<Unit> enemies = new List<Unit>();

        private Random rand;

        public Room(string path)
        {
            generateRoom(path);
        }

        public void generateRoom(string path)
        {
            rand = new Random(path.GetHashCode());

            this.width = rand.Next(5, 20);
            this.height = rand.Next(5, 20);

            //Testing obstacles and enemies
            /*obstacles.Add(new Pillar(0, 0, 3, 2));
            enemies.Add(new Bat(0, 0, 0.01));*/

            //Jake: Search folder and add enemies, obstacles, etc.
        }

        public void draw(Graphics g)
        {
            for (int i = 0; i < G.room.width; i++)
                for (int j = 0; j < G.room.height; j++)
                    g.DrawRectangle(Pens.Black, (int)(i * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(j * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size);

            foreach (Obstacle obstacle in obstacles)
                obstacle.draw(g);

            foreach (Unit enemy in enemies)
                enemy.draw(g);
        }
    }
}
