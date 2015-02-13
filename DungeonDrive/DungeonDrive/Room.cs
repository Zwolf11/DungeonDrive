using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DungeonDrive
{
    public class Room
    {
        public int width;
        public int height;
        public List<Obstacle> obstacles = new List<Obstacle>();
        public List<Unit> enemies = new List<Unit>();
        //public List<Door> doors = new List<Door>();
        public bool[,] freeSpace;                               // something can be placed here (not in front of door, not enemy starting spot, not obstacle spot
        public bool[,] walkingSpace;
        public int heroStartingX = 0;
        public int heroStartingY = 0;

        public int numEnemies = 0;
        public int numBats = 0;
        public int numSpiders = 0;
        public int numObstacles = 0;
        public const int maxEnemies = 15;
        public const int maxObstacles = 10;

        public const int SAFE_DISTANCE = 4;

        private Random rand;

        public Room(string path)
        {
            generateRoom(path);
        }

        public void generateRoom(string path)
        {
            rand = new Random(path.GetHashCode());  // random numbers based on path seed

            String[] files = Directory.GetFiles(path);  // get files in directory

            String[] dirs = Directory.GetDirectories(path);

            // base size of room on number of objects.

            this.width = rand.Next(30, 40); // width is x-axis
            this.height = rand.Next(25, 35); // height is y-axis

            freeSpace = new bool [width,height];
            walkingSpace = new bool [width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    freeSpace[i, j] = true;
                    walkingSpace[i,j] = true;
                }
            }

            freeSpace[0, 0] = false;
            

                //Testing obstacles and enemies

            bool results = addObstacle(new Pillar(6,8,3,2));
            // VVVVV This is for testing.
            //for ( int i = 1; i < 10; i++ )
            //    enemies.Add(new Bat(i, i*2));

            for (int i = 0; i < dirs.Length; i++)
            {
                directoryFound();
            }

            Console.WriteLine("files.length = {0}", files.Length);

            for(int i = 0; i < files.Length; i++){
                Console.WriteLine("just found file {0}", files[i]);
                matchExtension(Path.GetExtension(files[i]));
            }

        }

        public void matchExtension(String extension)
        {
            switch(extension){

             //* Text Files
                    case ".txt":
                    case ".rtf":
                    case ".doc":
                    case ".docx":
                        textFound();
                        break; //*/
                    //* audio file
                    case ".mp3":
                    case ".m4a":
                    case ".wav":
                    case ".wma":
                        audioFound();
                        break; //*/
                    //* video files
                    case ".avi":
                    case ".m4v":
                    case ".mov":
                    case ".mp4":
                    case ".mpg":
                    case ".wmv":
                        videoFound();
                        break; //*/
                    //*Image files
                    case ".jpg":
                    case ".jpeg":
                    case ".bmp":
                    case ".gif":                    case ".png":
                    case ".pdf":
                        imageFound();
                        break; //*/   /*                    // Powerpoint
                    case ".ppt":
                    case ".pptx":
                    case ".pps":
                        otherFound();
                        break;
  
                    // Spreadsheet Files
                    case ".xlr":
                    case ".xls":
                    case ".xlsx":
                        otherFound();
                        break;
                    // Executable files
                    case ".exe":
                    case ".jar":
                        otherFound();
                        break;
                    // Web files
                    case ".html":
                    case ".htm":
                    case ".css":
                    case ".js":
                    case ".php":
                    case ".xhtml":
                        otherFound();
                        break;
                    // Compressed files
                    case ".7z":
                    case ".gz":
                    case ".rar":
                    case ".tar.gz":
                    case ".zip":
                    case ".zipx":
                        otherFound();
                        break;

                    // Developer files
                    case ".c":
                    case ".class":
                    case ".cpp":
                    case ".cs":
                    case ".h":
                    case ".java":
                    case ".m":
                    case ".pl":
                    case ".py":
                    case ".sh":
                    case ".sln":
                        otherFound();
                        break;
 
                    // Torrent files
                    case ".torrent":
                        otherFound();
                        break;
 //*/           
                    // Other file
                    default:
                        otherFound();
                        break;
                }
        }

        public void directoryFound()
        {
            // WORK IN PROGRESS
            addDoor();
        }

        public void textFound()
        {
            while (!addObstacle(new Pillar(rand.Next(0, width - 4), rand.Next(0, height - 5), rand.Next(1, 3), rand.Next(1, 4)))) ;
        }

        public void audioFound()
        {
            while (!addObstacle(new Pillar(rand.Next(0, width - 4), rand.Next(0, height - 5), rand.Next(1, 3), rand.Next(1, 4)))) ;
        }

        public void videoFound()
        {
            while (!addObstacle(new Pillar(rand.Next(0, width - 4), rand.Next(0, height - 5), rand.Next(1, 3), rand.Next(1, 4)))) ;
        }

        public void imageFound()
        {
            while (!addObstacle(new Pillar(rand.Next(0, width - 4), rand.Next(0, height - 5), rand.Next(1, 3), rand.Next(1, 4)))) ;
        }

        public void otherFound()
        {                       
            // randomly spawn a bat or spider
            if((int) rand.Next(0,100) % 2 == 0){
                while (!addEnemy(new Bat(rand.Next(0, width - 1), rand.Next(0, height - 1)))) ;
                numBats++;
            } else {
                while (!addEnemy(new Spider(rand.Next(0, width - 1), rand.Next(0, height - 1)))) ;
                numSpiders++;
            }
             
        }

        public void addDoor()
        {

        }

        public bool addObstacle(Obstacle o)
        {
            if (numObstacles >= maxObstacles - 1)
            {
                return true;
            }

            // check to make sure the entire obstacle can be placed on the map without interference.
            for (int i = o.x; i < (o.x + o.width); i++)
            {
                for (int j = o.y; j < (o.y + o.height); j++)
                {
                    if (!freeSpace[i, j])
                        return false;
                }
            }
            

            obstacles.Add(o);
            for(int i = o.x; i < (o.x + o.width); i++)
            {
                for(int j = o.y; j < (o.y + o.height); j++)
                {
                    Console.WriteLine("disabling coordinates:{0},{1}", i, j);
                    walkingSpace[i,j] = false;
                    freeSpace[i, j] = false;
                }
            }

            numObstacles++;

            return true;

        }

        public bool addEnemy(Unit e)
        {

            if (numEnemies >= maxEnemies - 1)
            {
                return true;
            }

            if(Math.Sqrt(Math.Pow(e.x - heroStartingX, 2) + Math.Pow(e.y - heroStartingY, 2)) <= SAFE_DISTANCE || !freeSpace[(int)e.x, (int) e.y])
            {
                return false;
            }

            enemies.Add(e);
            freeSpace[(int)e.x, (int)e.y] = false;

            numEnemies++;

            return true;
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

            /*foreach (Door door in doors)
                door.draw(g);*/
        }

    }
}
