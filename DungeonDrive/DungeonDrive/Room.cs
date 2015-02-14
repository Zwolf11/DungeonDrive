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
        public int[,] roomNumSpace;

        public int heroStartingX = 0;                           // where the hero is starting in the new room. Might be useless.
        public int heroStartingY = 0;

        public int numEnemies = 0;
        public int numBats = 0;
        public int numSpiders = 0;             // current number of each of these objects
        public int numObstacles = 0;
        public int numStairs = 0;
        public int numRooms = 0;

        public const int maxEnemies = 15;
        public const int maxObstacles = 10;   // max number of these objects to generate in a room.
        public const int maxStairs = 10;

        public const int minRoomWidth = 15;
        public const int minRoomHeight = 15;
        public const int maxRoomWidth = 75;
        public const int maxRoomHeight = 75;

        public const int safe_distance = 4;   // safe distance for the enemies to spawn from the player's starting position in the room.
        public double temp_sd = safe_distance;

        private Random rand;
        public Random stairsRand;

        public Room(string path)
        {

            generateRoom(path);
        }

        public void generateRoom(string path)
        {

            G.pastRoom = G.currentRoom;
            G.currentRoom = path;

            rand = new Random(path.GetHashCode());  // random numbers based on path seed

            String[] files = Directory.GetFiles(path);  // get files in directory

            String[] dirs = Directory.GetDirectories(path); // get directories in current directory

            String parentDir = Path.GetDirectoryName(path);
            
            // base size of room on number of objects.


            ////////   GENERATE SIZES OF ROOMS    /////////

            int maxItems = Math.Min(maxEnemies + maxObstacles, files.Length);
            int maxMaxDirectories = Math.Min(dirs.Length, maxStairs) / 3;

            int widthBottom = (int) Math.Min(maxRoomWidth, minRoomWidth + (((rand.NextDouble() * .3) + .3) * (maxItems +maxMaxDirectories)));                // find the floor and ceiling of the height and width sizes
            int widthTop = (int) Math.Min(maxRoomWidth, widthBottom + ((rand.NextDouble() * .3) * (maxItems +maxMaxDirectories)));

            int heightBottom = (int) Math.Min(maxRoomHeight, minRoomHeight + (((rand.NextDouble() * .3) + .3) * (maxItems + maxMaxDirectories)));
            int heightTop = (int) Math.Min(maxRoomHeight, heightBottom + ((rand.NextDouble() * .3) * (maxItems + maxMaxDirectories)));

            this.width = rand.Next(widthBottom, widthTop); // width is x-axis
            this.height = rand.Next(heightBottom, heightTop); // height is y-axis


            ////////   INIT THE ARRAYS   /////////

            freeSpace = new bool [width,height];
            walkingSpace = new bool [width, height];
            stairSpace = new bool[width, height];
            roomNumSpace = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    freeSpace[i, j] = true;
                    walkingSpace[i,j] = true;         // initalizes all the arrays
                    stairSpace[i, j] = false;
                    roomNumSpace[i, j] = -1;
                }
            }


            //////////   ADD STAIR UP TO PARENT UNLESS IN C: DIRECTORY ///////

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


            /////////   TRAVERSE ALL DIRECTORIES   ///////

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
                        Console.WriteLine("{0}", e.ToString());
                    }
                }
                else
                {
                    // found hidden file
                }
            }

            /////////   FIND STAIRCASE YOU ARE COMING FROM   /////////

            foreach (Stairs stair in stairs)
            {
                if (stair.path.Equals(G.pastRoom))
                {                                                               
                    G.hero.changeFacing(stair.direction);
                    G.hero.x = G.hero.xNext = stair.x + stair.xDirection;      // place you on the correct side of it
                    G.hero.y = G.hero.yNext = stair.y + stair.yDirection;
                    break;
                }
            }

            //////////   TRAVERSE ALL FILES   //////////

            for(int i = 0; i < files.Length; i++){
                matchExtension(Path.GetExtension(files[i]));     // match each file extension and spawn the corresponding object
            }

            // determine hero starting point
            // find stair that matches the pastRoom

            G.newRoom = true;

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

            while (!addStairs(rand.Next(2, width - 3), rand.Next(2, height - 3), 1, 1, true, path, 'r')) ;
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
            temp_sd = safe_distance;
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

        public bool addStairs(int x, int y, int width, int height, bool down, String path, char direction)
        {
            if (numStairs >= (maxStairs - 1))
            {
                return true;
            }

            if (direction == 'r')
            {
                if (down){
                    direction = 'd';
                } else {
                    direction = 'a';
                }

                if (down)
                {
                    stairsRand = new Random(string.Concat(G.currentRoom, path).GetHashCode());
                }
                else
                {
                    stairsRand = new Random(string.Concat(path, G.currentRoom).GetHashCode());
                }

                switch ((int)stairsRand.Next(0, 4))
                {
                    case 0:
                        if (down) {
                            direction = 'w';
                        } else {
                            direction = 's';
                        }
                        break;
                    case 1:
                        if (down) {
                            direction = 'd';
                        } else {
                            direction = 'a';
                        }
                        break;
                    case 2:
                        if (down) {
                            direction = 's';
                        } else {
                            direction = 'w';
                        }
                        break;
                    case 3:
                        if (down) {
                            direction = 'a';
                        } else {
                            direction = 'd';
                        }
                        break;
                }
            }


            if (!freeSpace[x, y] || !freeSpace[x + xDirFromChar(direction), y + yDirFromChar(direction)] || roomNumSpace[x,y] != -1)
            {
                return false;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mergeRoom((x - 1) + i, (y - 1) + j, numRooms); 
                }
            }

            stairs.Add(new Stairs(x, y, width, height, down, path, direction));

            stairSpace[x, y] = true;
            freeSpace[x, y] = false;
            freeSpace[x + xDirFromChar(direction), y + yDirFromChar(direction)] = false;

            numRooms++;
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
                temp_sd *= .9;
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

        public void mergeRoom(int x, int y, int newRoomNum){

            roomNumSpace[x,y] = newRoomNum;
            if( x > 2 && roomNumSpace[x-1,y] != -1 && roomNumSpace[x-1,y] != newRoomNum){
                mergeRoom(x-1,y,newRoomNum);
            }
            if( (x + 1) < (width - 1) && roomNumSpace[x+1,y] != -1 && roomNumSpace[x+1,y] != newRoomNum){
                mergeRoom(x+1,y,newRoomNum);
            }
            if( y > 2 && roomNumSpace[x,y-1] != -1 && roomNumSpace[x,y-1] != newRoomNum){
                mergeRoom(x,y-1,newRoomNum);
            }
            if( (y + 1) < (height - 1) && roomNumSpace[x,y+1] != -1 && roomNumSpace[x,y+1] != newRoomNum){
                mergeRoom(x,y+1,newRoomNum);
            }
        }

        public void draw(Graphics g)
        {

            for (int i = 0; i < G.room.width; i++)
                for (int j = 0; j < G.room.height; j++)
                    //if(i == 0 || i == G.room.width - 1 || j == 0 || j == G.room.height - 1 || roomNumSpace[i,j] != -1)
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

    }
}
