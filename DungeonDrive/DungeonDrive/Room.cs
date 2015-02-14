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
        public List<Stairs> stairs = new List<Stairs>();

        public bool[,] freeSpace;                      // something can be placed here (not in front of door, not enemy starting spot, not obstacle spot
        public bool[,] walkingSpace;                   // hero can walk here.
        public bool[,] stairSpace;                      // there are stairs in this space.

        public int heroStartingX = 0;                           // where the hero is starting in the new room. Might be useless.
        public int heroStartingY = 0;

        public int numEnemies = 0;
        public int numBats = 0;
        public int numSpiders = 0;             // current number of each of these objects
        public int numObstacles = 0;
        public int numStairs = 0;

        public const int maxEnemies = 15;
        public const int maxObstacles = 10;   // max number of these objects to generate in a room.
        public const int maxStairs = 10;

        public const int minRoomWidth = 6;
        public const int minRoomHeight = 6;
        public const int maxRoomWidth = 40;
        public const int maxRoomHeight = 30;

        public const int safe_distance = 4;   // safe distance for the enemies to spawn from the player's starting position in the room.
        public double temp_sd = safe_distance;

        private Random rand;

        public Room(string path)
        {

            generateRoom(path);
        }

        public void generateRoom(string path)
        {

            rand = new Random(path.GetHashCode());  // random numbers based on path seed

            String[] files = Directory.GetFiles(path);  // get files in directory

            String[] dirs = Directory.GetDirectories(path); // get directories in current directory

            String parentDir = Path.GetDirectoryName(path);

            Console.WriteLine("Directory parent of {0} = {1}", path, parentDir);
            
            // base size of room on number of objects.

            int widthBottom = (int) Math.Min(maxRoomWidth, minRoomWidth + (((rand.NextDouble() * .5) + .2) * (files.Length + dirs.Length)));                // find
            int widthTop = (int)Math.Min(maxRoomWidth, widthBottom + ((rand.NextDouble() * .5) * (files.Length + dirs.Length)));

            int heightBottom = (int) Math.Min(maxRoomHeight, minRoomHeight + (((rand.NextDouble() * .5) + .2) * (files.Length + dirs.Length)));
            int heightTop = (int)Math.Min(maxRoomHeight, heightBottom + ((rand.NextDouble() * .5) * (files.Length + dirs.Length)));

            this.width = rand.Next(widthBottom, widthTop); // width is x-axis
            this.height = rand.Next(heightBottom, heightTop); // height is y-axis

            freeSpace = new bool [width,height];
            walkingSpace = new bool [width, height];
            stairSpace = new bool[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    freeSpace[i, j] = true;
                    walkingSpace[i,j] = true;         // initalizes all the boolean arrays
                    stairSpace[i, j] = false;
                }
            }

            Console.WriteLine("Here");

            if (parentDir == null)
            {
                // this is the initial C file
                G.hero.x = rand.Next(2, width - 2);
                G.hero.y = rand.Next(2, height - 2);
                freeSpace[(int) G.hero.x, (int) G.hero.y] = false;
            }
            else
            {            
                addStairs(rand.Next(1,width - 2), rand.Next(1,height-2), 1,1, false, parentDir, 'r');
            }
          

            for (int i = 0; i < dirs.Length; i++)
            {
                if(!((File.GetAttributes(dirs[i]) & FileAttributes.Hidden) == FileAttributes.Hidden)){              // checks to make sure the directory is not hidden

                    try
                    {
                        bool temp2 = Directory.Exists(dirs[i]);                                               // this should throw an error is the directory is inaccessible       
                        directoryFound(dirs[i]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("found hidden directory");

                        Console.WriteLine("{0}", e.ToString());
                    }
                }
                else
                {
                    // found hidden file
                }
            }


            G.pastRoom = G.currentRoom;
            G.currentRoom = path;

            foreach (Stairs stair in stairs)
            {
                Console.WriteLine("comparing {0} and {1}", stair.path, G.pastRoom);
                if (stair.path.Equals(G.pastRoom))
                {
                    Console.WriteLine("Gets here");
                    // found the stairs you are coming from
                    G.hero.changeFacing(stair.direction);
                    Console.WriteLine("Also gets here");
                    G.hero.x = G.hero.xNext = stair.x + stair.xDirection;
                    G.hero.y = G.hero.yNext = stair.y + stair.yDirection;
                    Console.WriteLine("yep. Here too");
                    break;
                }
            }


            Console.WriteLine("Matching {0} file", files.Length);
            for(int i = 0; i < files.Length; i++){
                Console.WriteLine("Matches the extension of the number {0} file - {1}", i , files[i]);
                matchExtension(Path.GetExtension(files[i]));     // match each file extension and spawn the corresponding object
            }

            // determine hero starting point
            // find stair that matches the pastRoom

            Console.WriteLine("exited");
            G.newRoom = true;

            //Console.WriteLine("Changed pastRoom to {0} and currentRoom to {1}", G.pastRoom, G.currentRoom);

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

        public void directoryFound(String path)
        {
            // WORK IN PROGRESS

            while (!addStairs(rand.Next(1, width - 2), rand.Next(1, height - 2), 1, 1, true, path, 'r')) ;
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
            Console.WriteLine("other Found");
            // randomly spawn a bat or spider

            temp_sd = safe_distance;
            if((int) rand.Next(0,100) % 2 == 0){
                while (!addEnemy(new Bat(rand.Next(0, width - 1), rand.Next(0, height - 1))))
                {
                    Console.WriteLine("loop1");
                }
                numBats++;

            } else {
                while (!addEnemy(new Spider(rand.Next(0, width - 1), rand.Next(0, height - 1))))
                {
                    Console.WriteLine("loop2");
                }
                numSpiders++;
            }

            Console.WriteLine("out");
             
        }

        public void addDoor()
        {

        }

        public bool addStairs(int x, int y, int width, int height, bool down, String path, char direction)
        {
            if (numStairs >= (maxStairs - 1))
            {
                return true;
            }

            if (direction == 'r')
            {

                direction = 'd';

                switch ((int)rand.Next(0, 4))
                {
                    case 0:
                        direction = 'w';
                        break;
                    case 1:
                        direction = 'd';
                        break;
                    case 2:
                        direction = 's';
                        break;
                    case 3:
                        direction = 'a';
                        break;
                }
            }


            if (!freeSpace[x, y] || !freeSpace[x + xDirFromChar(direction), y + yDirFromChar(direction)])
            {
                return false;
            }

            stairs.Add(new Stairs(x, y, width, height, down, path, direction));

            stairSpace[x, y] = true;
            freeSpace[x, y] = false;
            freeSpace[x + xDirFromChar(direction), y + yDirFromChar(direction)] = false;

            numStairs++;

            return true;

        }

        public bool addObstacle(Obstacle o)
        {
            if (numObstacles >= (maxObstacles - 1))
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
                    //Console.WriteLine("disabling coordinates:{0},{1}", i, j);
                    walkingSpace[i,j] = false;
                    freeSpace[i, j] = false;
                }
            }

            numObstacles++;

            return true;

        }

        public bool addEnemy(Unit e)
        {

            if (numEnemies >= (maxEnemies - 1))
            {
                return true;
            }

            if(Math.Sqrt(Math.Pow(e.x - G.hero.x, 2) + Math.Pow(e.y - G.hero.y, 2)) <= temp_sd || !freeSpace[(int)e.x, (int) e.y])
            {
                temp_sd *= .8;
                Console.WriteLine("failed");
                return false;
            }

            enemies.Add(e);
            freeSpace[(int)e.x, (int)e.y] = false;

            numEnemies++;

            return true;
        }

        public int xDirFromChar(char c)
        {

            switch (c)
            {
                case 'w':
                case 's':
                    return 0;
                case 'a':
                    return -1;
                case 'd':
                    return 1;
            }
            return 0;
        }

        public int yDirFromChar(char c)
        {
            switch (c)
            {
                case 'w':
                    return -1;
                case 's':
                    return 1;
                case 'a':
                case 'd':
                    return 0;
            }
            return 0;
        }

        public void draw(Graphics g)
        {

            for (int i = 0; i < G.room.width; i++)
                for (int j = 0; j < G.room.height; j++)
                    g.DrawRectangle(Pens.Black, (int)(i * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(j * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size);

            foreach (Stairs stair in stairs)
                stair.draw(g);

            foreach (Obstacle obstacle in obstacles)
                obstacle.draw(g);

            foreach (Unit enemy in enemies)
                enemy.draw(g);

            /*foreach (Door door in doors)
                door.draw(g);*/


        }

        public String getParent(String path)
        {

            
            return "";
        }

    }
}
