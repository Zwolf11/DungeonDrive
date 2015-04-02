using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DungeonDrive
{
    public class Room
    {


        private GameState state;
        public String currentRoom = "C:\\";
        private String pastRoom;

        //////// IF YOU WANT TO DISABLE WALL BOUNDARIES TO TEST OTHER THINGS, SET noBoundaries TO TRUE ////////
        public bool noBoundaries = false;


        public int width;
        public int height;
        public List<Obstacle> obstacles = new List<Obstacle>();
        public List<Unit> enemies = new List<Unit>();
        public List<Door> doors = new List<Door>();
        public List<Stairs> stairs = new List<Stairs>();
        public List<int> connectedRooms = new List<int>();

        public bool[,] freeSpace;                      // something can be placed here (not in front of door, not enemy starting spot, not obstacle spot
        public bool[,] walkingSpace;                   // hero can walk here.
        public bool[,] stairSpace;                      // there are stairs in this space.
        public int[,] roomNumSpace;
        public bool[,] hallwaySpace;
        public bool[,] wallSpace;
        public bool[,] doorSpace;

        public int heroStartingX = 0;                           // where the hero is starting in the new room. Might be useless.
        public int heroStartingY = 0;

        public int numEnemies = 0;
        public int numBats = 0;
        public int numSkeletons = 0;             // current number of each of these objects
        public int numSnakes = 0;
        public int numObstacles = 0;
        public int numStairs = 0;
        public int numRooms = 0;
        public int numChests = 0;

        public const int minSizeOfInitRoom = 7;
        public const int maxSizeOfInitRoom = 13;


        public const int minSizeHallway = 2;
        public const int maxSizeHallway = 4;

        public const int maxEnemies = 15;
        public const int maxObstacles = 15;   // max number of these objects to generate in a room.
        public const int maxStairs = 50;
        public const int maxChests = 2;

        public const int minRoomWidth = 35;
        public const int minRoomHeight = 35;
        public const int maxRoomWidth = 300;
        public const int maxRoomHeight = 300;

        public const int safe_distance = 4;   // safe distance for the enemies to spawn from the player's starting position in the room.
        public double temp_sd = safe_distance;

        private Random rand;
        public Random stairsRand;
        private Bitmap floor = new Bitmap(Properties.Resources.floor);
        private Bitmap wall = new Bitmap(Properties.Resources.wall);

        public Room(GameState state, string path)
        {
            this.state = state;
            generateRoom(path);
        }

        public void generateRoom(string path)
        {
            pastRoom = currentRoom;
            currentRoom = path;

            rand = new Random(path.GetHashCode());  // random numbers based on path seed

            String[] files = Directory.GetFiles(path);  // get files in directory

            String[] dirs = Directory.GetDirectories(path); // get directories in current directory

            String parentDir = Path.GetDirectoryName(path);
            

            // base size of room on number of objects.


            ////////   GENERATE SIZES OF ROOMS    /////////

            int maxItems = Math.Min(maxEnemies + maxObstacles, files.Length);
            int maxMaxDirectories = Math.Min(dirs.Length, maxStairs) * 2;

            int widthBottom = (int) Math.Min(maxRoomWidth, minRoomWidth + (((rand.NextDouble() * .7) + .6) * (maxItems +maxMaxDirectories)));                // find the floor and ceiling of the height and width sizes
            int widthTop = (int) Math.Min(maxRoomWidth, widthBottom + ((rand.NextDouble() * .7) * (maxItems +maxMaxDirectories)));

            int heightBottom = (int) Math.Min(maxRoomHeight, minRoomHeight + (((rand.NextDouble() * .7) + .6) * (maxItems + maxMaxDirectories)));
            int heightTop = (int) Math.Min(maxRoomHeight, heightBottom + ((rand.NextDouble() * .7) * (maxItems + maxMaxDirectories)));

            Console.WriteLine("Width between {0} and {1}, and Height between {2} and {3}", widthBottom, widthTop, heightBottom, heightTop);
            this.width = rand.Next(widthBottom, widthTop); // width is x-axis

            this.height = rand.Next(heightBottom, heightTop); // height is y-axis

            Console.WriteLine("Width = {0} and Height = {1}", width, height);

            ////////   INIT THE ARRAYS   /////////

            freeSpace = new bool [width,height];
            walkingSpace = new bool [width, height];
            stairSpace = new bool[width, height];
            roomNumSpace = new int[width, height];
            hallwaySpace = new bool[width, height];
            wallSpace = new bool[width, height];
            doorSpace = new bool[width, height];
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    freeSpace[i, j] = true;
                    walkingSpace[i,j] = true;         // initalizes all the arrays
                    stairSpace[i, j] = false;
                    roomNumSpace[i, j] = -1;
                    hallwaySpace[i, j] = false;
                    wallSpace[i, j] = false;
                    doorSpace[i, j] = false;
                }
            }


            //////////   ADD STAIR UP TO PARENT UNLESS IN C: DIRECTORY ///////

            if (parentDir == null)
            {

            } else {
                // this is not the initial C file
                addStairs(false, parentDir);
            }

            Console.WriteLine("Here0");

            /////////   TRAVERSE ALL DIRECTORIES   ///////

            for (int i = 0; i < dirs.Length; i++)
            {
                if(!((File.GetAttributes(dirs[i]) & FileAttributes.Hidden) == FileAttributes.Hidden)){              // checks to make sure the directory is not hidden

                    try
                    {
                        bool temp2 = Directory.Exists(dirs[i]);
                        String[] tempStrings = Directory.GetDirectories(dirs[i]);                                               // this should throw an error is the directory is inaccessible       
                        Console.WriteLine(dirs[i] + " found");
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

            Console.WriteLine("Here1");

            /////////// IF THIS IS THE INITIAL ROOM, 

            updateFreeSpace();

            if (parentDir == null)
            {

                Console.WriteLine("InitialRoom");
                int x1, y1;
                while (roomNumSpace[x1 = rand.Next(0, width - 1), y1 = rand.Next(0, height)] == -1 || wallSpace[x1,y1]) ;

                state.hero.x = x1 + 0.5;
                state.hero.y = y1 + 0.5;
                freeSpace[x1, y1] = false;
            }

            /////////   FIND STAIRCASE YOU ARE COMING FROM   /////////

            


            foreach (Stairs stair in stairs)
            {
                if (stair.path.Equals(pastRoom))
                {                                         
                    state.hero.x = /*state.hero.xNext = */stair.x + stair.xDirection + 0.5;      // place you on the correct side of it
                    state.hero.y = /*state.hero.yNext = */stair.y + stair.yDirection + 0.5;
                    break;
                }
            }

            Console.WriteLine("Here2");

            recalcRoomNums();

            /////////  CONNECT ROOMS WITH HALLWAYS   ////



            List<Stairs>[] roomStairs = new List<Stairs>[numRooms];

            for (int i = 0; i < numRooms; i++)
            {
                roomStairs[i] = new List<Stairs>();
            }

            foreach (Stairs tempStair in stairs)
            {
                roomStairs[tempStair.roomNum].Add(tempStair);                   // have an array of lists of stairs sorted by room numbers
            }

            int staticNumRooms = numRooms;

            int breaker = 0;

            while (true)
            {

                int count = 0;
                foreach (Stairs stair in roomStairs[staticNumRooms - 1])
                {
                    count++;
                }
                if (count == numStairs || breaker > 100)
                {
                    Console.WriteLine(breaker);
                    breaker++;
                    break;
                }

                for (int i = 0; i < staticNumRooms; i++)              // go through each room
                {


                    double shortestDistance = width + height;       // want to find the shortest distance between any stair in this room and any stair not in this room.
                    Stairs source = new Stairs(state, -1, -1, -1, -1, -1, false, "", 'u', -1);                                // stair that the hallway must start from
                    Stairs closestStair = new Stairs(state, -1, -1, -1, -1, -1, false, "", 'u', -1);
                    int roomDest = -1;



                    foreach (Stairs stair in roomStairs[i])
                    {             // evaluate every stair in the current room

                        for (int j = 0; j < staticNumRooms; j++)              // go through each other room and find the closest stair
                        {
                            if (i == j) continue;                       // skip current room number, because you are trying to find the shortest distance from stair in room i, to stair in room j.

                            foreach (Stairs stair2 in roomStairs[j])
                            {
                                double newDistance;
                                if ((newDistance = distanceBtwnPts(stair.x, stair.y, stair2.x, stair2.y)) < shortestDistance)           // if the distance between the stairs is smaller than any other one found til this point, update new shortestStair and distance
                                {
                                    roomDest = j;
                                    shortestDistance = newDistance;
                                    source = stair;
                                    closestStair = stair2;
                                }

                            }
                        }

                    }


                    // closestStair should be accurate
                    if (source.x != -1 && closestStair.x != -1)         // if a close stair has been found
                    {
                        makeHallway(source.x, source.y, closestStair.x, closestStair.y, Math.Min(source.maxHallwayWidth, closestStair.maxHallwayWidth));
                        // make a hallway between the two.

                        if (roomDest > i)                       // move all the stairs from the lower room, into the higher room
                        {
                            foreach (Stairs removeStair in roomStairs[i])
                            {
                                roomStairs[roomDest].Add(removeStair);
                            }

                            roomStairs[i].Clear();

                        }
                        else
                        {
                            foreach (Stairs removeStair in roomStairs[roomDest])
                            {
                                roomStairs[i].Add(removeStair);
                            }

                            roomStairs[roomDest].Clear();

                        }


                    }



                }

                Console.WriteLine(breaker);
                breaker++;
            }

            Console.WriteLine("Out1");


            updateFreeSpace();

                //////////   TRAVERSE ALL FILES   //////////

            Console.WriteLine("Number of files = " + files.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine("File - " + i);
                    matchExtension(Path.GetExtension(files[i]), Path.GetFileName(files[i]));     // match each file extension and spawn the corresponding object
                }

            // determine hero starting point
            // find stair that matches the pastRoom

                Console.WriteLine("Out2");

            if (!noBoundaries)
            {
                addBoundaries();
            }
            recalcRoomNums();

            Console.WriteLine("Out3");
        }

        public void matchExtension(String extension, String filename)
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
                    case ".gif":
                    case ".png":
                    case ".pdf":
                        imageFound();
                        break; //*/   
/*
                    // Powerpoint
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
                        otherFound(filename);
                        break;
                }
        }

        public void directoryFound(String path)
        {
            // WORK IN PROGRESS

            addStairs(true, path) ;
        }

        public void textFound()
        {
            addObstacle("pillar");
        }

        public void audioFound()
        {
            addObstacle("chest");
        }

        public void videoFound()
        {
            addObstacle("chest");
        }

        public void imageFound()
        {
            addObstacle("chest");
        }

        public void otherFound(String filename)
        {
            temp_sd = safe_distance;
            int random = (int) rand.Next(0, 100);
            if(random <= 33){
                while (!addEnemy(new Bat(state, rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5), filename)) ;
                numBats++;

            }
            else if (random > 33 && random <= 66)
            {
                while (!addEnemy(new Skeleton(state, rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5), filename)) ;
                numSkeletons++;
                //while (!addEnemy(new Boss(rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5)));
            }
            else
            {
                while (!addEnemy(new Snake(state, rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5), filename)) ;
                numSnakes++;
            }
        }

        public void addDoor()
        {

        }

        public void addStairs(bool down, String path)
        {

            Console.WriteLine("Adding stairs for " + path);

            if (numStairs >= (maxStairs - 1))
            {
                return;
            }

            char direction;

                if (down){
                    direction = 'd';
                } else {
                    direction = 'a';
                }

                if (down)
                {
                    stairsRand = new Random(string.Concat(currentRoom, path).GetHashCode());
                }
                else
                {
                    stairsRand = new Random(string.Concat(path, currentRoom).GetHashCode());
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

            int x;
            int y; 
            int tHeight = 1;
            int tWidth = 1;

            int maxHallwayWidth = 2;

            int heightOfInitStairRoom = rand.Next(minSizeOfInitRoom, maxSizeOfInitRoom + 1);
            int widthOfInitStairRoom = rand.Next(minSizeOfInitRoom, maxSizeOfInitRoom + 1);
            int heightRadiusInitRoom = (int)((heightOfInitStairRoom) / 2);
            int widthRadiusInitRoom = (int)((widthOfInitStairRoom) / 2);

            Console.WriteLine("Here1");

            do
            {
                x = rand.Next(2 + widthRadiusInitRoom, width - 4 - widthRadiusInitRoom);
                y = rand.Next(2 + heightRadiusInitRoom, height - 4 - heightRadiusInitRoom);
            } while (!freeSpace[x, y] || !freeSpace[x + xDirFromChar(direction), y + yDirFromChar(direction)] || roomNumSpace[x, y] != -1);


            int maxRandom = (int) widthOfInitStairRoom * heightOfInitStairRoom - (2 * (widthOfInitStairRoom + heightOfInitStairRoom)) + 2;   // indicates how many non-border cells there are. We don't want the stairs to be on a border to avoid adjacent stair tiles.

            int stairLocation = rand.Next(0, maxRandom);
            int counter = 0;
            int stairX  = x;
            int stairY = y;

            Console.WriteLine("Here1.5");

            for (int i = -1; i <= widthOfInitStairRoom; i++)
            {
                for (int j = -1; j <= heightOfInitStairRoom; j++)
                {
                    if (i == -1 || i == widthOfInitStairRoom || j == -1 || j == heightOfInitStairRoom)
                    {
                        if (roomNumSpace[x + i - widthRadiusInitRoom, y + j - heightRadiusInitRoom] == -1)
                        {
                            wallSpace[x + i - widthRadiusInitRoom, y + j - heightRadiusInitRoom] = true;
                        }

                    } else{

                        if (wallSpace[x + i - widthRadiusInitRoom, y + j - heightRadiusInitRoom])
                        {
                            wallSpace[x + i - widthRadiusInitRoom, y + j - heightRadiusInitRoom] = false;
                        }

                        if (i != 0 && i != widthOfInitStairRoom - 1 && j != 0 && j != heightOfInitStairRoom - 1)
                        {
                            // a non-border cell.
                            if (counter == stairLocation)
                            {

                                stairX = x + i - widthRadiusInitRoom;
                                stairY = y + j - heightRadiusInitRoom;

                                //wallSpace[stairX, stairY] = true;

                                int minimumDistToWall = 10;
                                minimumDistToWall = Math.Min(minimumDistToWall, i);
                                minimumDistToWall = Math.Min(minimumDistToWall, j);
                                minimumDistToWall = Math.Min(minimumDistToWall, (widthOfInitStairRoom - 1) - i);
                                minimumDistToWall = Math.Min(minimumDistToWall, (heightOfInitStairRoom - 1) - j);

                                maxHallwayWidth = 2 * minimumDistToWall;

                            }
                            counter++;
                        }
                    }
                    
                    mergeRoom((x - widthRadiusInitRoom) + i, (y - heightRadiusInitRoom) + j, numRooms); 
                }
            }

            Console.WriteLine("Here2");

            stairs.Add(new Stairs(state, stairX, stairY, tWidth, tHeight, roomNumSpace[stairX,stairY], down, path, direction, maxHallwayWidth));

            stairSpace[stairX, stairY] = true;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    freeSpace[stairX + i,stairY + j] = false;
                }
            }

            //freeSpace[stairX, stairY] = false;
            //freeSpace[stairX + xDirFromChar(direction), stairY + yDirFromChar(direction)] = false;

            numRooms++;
            numStairs++;

            Console.WriteLine("ENd of add stairs");

            return;

        }

        public void addObstacle(String type)
        {
           
            if (numObstacles >= (maxObstacles - 1))
            {
                return;

            }

            int oWidth = 1;
            int oHeight = 1;

            switch (type)
            {
                case "pillar":
                    oWidth = 1;
                    oHeight = 1;
                    break;

                case "chest":
                    oWidth = 1;
                    oHeight = 1;
                    break;
            }

            int x = 0;
            int y = 0;
            bool intersect = true;

            while (intersect)
            {
                intersect = false;

                x = rand.Next(0, width - 4);
                y = rand.Next(0, height - 3);

                // check to make sure the entire obstacle can be placed on the map without interference.
                for (int i = x; i < (x + oWidth); i++)
                {
                    for (int j = y; j < (y + oHeight); j++)
                    {
                        if (!freeSpace[i, j] || roomNumSpace[i, j] == -1)
                            intersect = true;
                    }
                }
            }

            Obstacle newObs;

            switch (type)
            {
                case "pillar":
                    newObs = new Pillar(state, x, y, oWidth, oHeight, roomNumSpace[x, y]);
                    break;
                case "chest":
                    newObs = new Chest(state, x, y, oWidth, oHeight, roomNumSpace[x, y]);
                    break;
                default:
                    newObs = new Pillar(state, x, y, oWidth, oHeight, roomNumSpace[x, y]);
                    break;
            }


            obstacles.Add(newObs);
            for(int i = x; i < (x + oWidth); i++)
            {
                for(int j = y; j < (y + oHeight); j++)
                {
                    walkingSpace[i,j] = false;
                    freeSpace[i, j] = false;
                }
            }

            numObstacles++;

        }

        public bool addEnemy(Unit e, String filename)
        {

            if (numEnemies >= (maxEnemies - 1))
            {
                return true;
            }

            if(Math.Sqrt(Math.Pow(e.x - state.hero.x, 2) + Math.Pow(e.y - state.hero.y, 2)) <= temp_sd || !freeSpace[(int)e.x, (int) e.y] || roomNumSpace[(int)e.x, (int) e.y] == -1)
            {
                temp_sd *= .9;
                return false;
            }

            e.addName(filename);
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
            if( x > 2 && roomNumSpace[x-1,y] != -1 && roomNumSpace[x - 1, y] != newRoomNum){
                mergeRoom(x - 1, y, newRoomNum);
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

        public void recalcRoomNums()
        {
            foreach (Stairs stair in stairs)
            {
                stair.roomNum = roomNumSpace[stair.x,stair.y];
            }

            foreach (Unit unit in enemies)
            {
                unit.roomNum = roomNumSpace[(int) unit.x, (int) unit.y];
            }
        }

        public void addBoundaries(){

            for(int i = 0; i < width; i++){
                for (int j = 0; j < height; j++){
                    if(roomNumSpace[i,j] == -1 || wallSpace[i,j]){
                        walkingSpace[i, j] = false;
                    }
                }
            }

            foreach (Door door in doors)
            {
                for (int i = 0; i < door.width; i++)
                {
                    for (int j = 0; j < door.height; j++)
                    {
                        wallSpace[door.x + i, door.y + j] = false;
                        walkingSpace[door.x + i, door.y + j] = true;
                    }
                }
            }

        }

        public void updateFreeSpace()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (wallSpace[i, j])
                    {
                        freeSpace[i, j] = false;
                    }


                }
            }
        }

        public void makeHallway(int x1, int y1, int x2, int y2, int maxHSize)
        {
            int upperHallwaySize = Math.Min(maxHSize, maxSizeHallway);
            int wide = (int) rand.Next(minSizeHallway, upperHallwaySize + 1);
            int deltaX = x1 - x2;
            int deltaY = y1 - y2;

            int hallwayNum = numRooms;

            int halfwayInc = (int) (wide - 1) / 2;

            int door1Loc = rand.Next(0, wide - 1);
            int door2Loc = rand.Next(0, wide - 1);

            door1Loc = door2Loc = 0;

            // add an if statement here saying if it is a short hallway to just tear down all walls in it's path

            int doorCounter = 0;
            bool door1ShouldBePlaced = false;
            bool door2ShouldBePlaced = false;

            bool door1Placed = false;
            bool door2Placed = false;

            if (deltaX <= 0)
            {
                // x1 < x2
                for (; x1 < x2 + (wide - halfwayInc); x1++)
                {
                    if (!door1Placed)
                    {
                        if (wallSpace[x1, y1] && roomNumSpace[x1, y1] != hallwayNum && roomNumSpace[x1, y1] != -1)
                        {
                            door1ShouldBePlaced = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (!door2Placed)
                    {
                        if (wallSpace[x1, y1] && roomNumSpace[x1, y1] != hallwayNum && roomNumSpace[x1, y1] != -1)
                        {
                            door2ShouldBePlaced = true;

                        }
                        

                    }

                    doorCounter = 0;

                    for (int i = -1; i <= wide; i++)
                    {
                        if (i == -1 || i == wide)
                        {
                            if (roomNumSpace[x1, y1 - halfwayInc + i] == -1)
                            {
                                wallSpace[x1, y1 - halfwayInc + i] = true;
                            }
                        }
                        else
                        {
                            if (door1ShouldBePlaced && doorCounter == door1Loc){ 
                                doors.Add(new Door(state, x1, y1 - halfwayInc + i, 1, 2, roomNumSpace[x1, y1 - halfwayInc], false));
                                doorSpace[x1, y1 - halfwayInc + i] = true;
                                wallSpace[x1, y1 - halfwayInc + i] = false;
                                door1Placed = true;
                                door1ShouldBePlaced = false;
                                
                            } else if(door2ShouldBePlaced && doorCounter == door2Loc){
                                doors.Add(new Door(state, x1, y1 - halfwayInc + i, 1, 2, roomNumSpace[x1, y1 - halfwayInc], false));
                                doorSpace[x1, y1 - halfwayInc + i] = true;
                                wallSpace[x1, y1 - halfwayInc + i] = false;
                                door2Placed = true;
                                door2ShouldBePlaced = false;
                            }
                            doorCounter++;

                            walkingSpace[x1, y1 - halfwayInc + i] = true;

                            
                        }

                        if (roomNumSpace[x1, y1 - halfwayInc + i] == -1)
                        {
                            roomNumSpace[x1, y1 - halfwayInc + i] = hallwayNum;
                            hallwaySpace[x1, y1 - halfwayInc + i] = true;
                        }
                        
                        
                    }
                }
               // x1 -= halfwayInc;
            }
            else
            {
                for (; x1 >= x2 - (halfwayInc); x1--)
                {

                    if (!door1Placed)
                    {
                        if (wallSpace[x1, y1] && roomNumSpace[x1, y1] != hallwayNum && roomNumSpace[x1, y1] != -1)
                        {
                            door1ShouldBePlaced = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (!door2Placed)
                    {
                        if (wallSpace[x1, y1] && roomNumSpace[x1, y1] != hallwayNum && roomNumSpace[x1,y1] != -1)
                        {
                            door2ShouldBePlaced = true;

                        }
                        

                    }

                    for (int i = -1; i <= wide; i++)
                    {
                        if (i == -1 || i == wide)
                        {
                            if (roomNumSpace[x1, y1 - halfwayInc + i] == -1)
                            {
                                wallSpace[x1, y1 - halfwayInc + i] = true;
                            }
                        }
                        else
                        {
                            if (door1ShouldBePlaced && doorCounter == door1Loc)
                            {
                                doors.Add(new Door(state, x1, y1 - halfwayInc + i, 1, 2, roomNumSpace[x1, y1 - halfwayInc], false));
                                doorSpace[x1, y1 - halfwayInc + i] = true;
                                wallSpace[x1, y1 - halfwayInc + i] = false;
                                door1Placed = true;
                                door1ShouldBePlaced = false;

                            }
                            else if (door2ShouldBePlaced && doorCounter == door2Loc)
                            {
                                doors.Add(new Door(state, x1, y1 - halfwayInc + i, 1, 2, roomNumSpace[x1, y1 - halfwayInc], false));
                                doorSpace[x1, y1 - halfwayInc + i] = true;
                                wallSpace[x1, y1 - halfwayInc + i] = false;
                                door2Placed = true;
                                door2ShouldBePlaced = false;
                            }
                            doorCounter++;

                            walkingSpace[x1, y1 - halfwayInc + i] = true;

                            
                        }

                        if (roomNumSpace[x1, y1 - halfwayInc + i] == -1)
                        {
                            roomNumSpace[x1, y1 - halfwayInc + i] = hallwayNum;
                            hallwaySpace[x1, y1 - halfwayInc + i] = true;
                        }
                        
                    }
                }
                //x1 += halfwayInc;
            }

            


            if(deltaY < 0){

                y1 -= halfwayInc;
               
                for (; y1 <= y2; y1++)
                {

                    if (!door1Placed)
                    {
                        if (wallSpace[x2, y1] && roomNumSpace[x2, y1] != hallwayNum && roomNumSpace[x2, y1] != -1)
                        {
                            door1ShouldBePlaced = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (!door2Placed)
                    {
                        if (wallSpace[x2, y1] && roomNumSpace[x2, y1] != hallwayNum && roomNumSpace[x2, y1] != -1)
                        {
                            door2ShouldBePlaced = true;

                        }


                    }

                    doorCounter = 0;

                    for (int i = -1; i <= wide; i++)
                    {
                        if (i == -1 || i == wide)
                        {
                            if (roomNumSpace[x2 - halfwayInc + i, y1] == -1)
                            {
                                wallSpace[x2 - halfwayInc + i, y1] = true;

                            }
                        }
                        else
                        {

                            if (door1ShouldBePlaced && doorCounter == door1Loc)
                            {
                                doors.Add(new Door(state, x2 - halfwayInc + i, y1, 2, 1, roomNumSpace[x2 - halfwayInc + i, y1], true));
                                doorSpace[x2 - halfwayInc + i, y1] = true;
                                wallSpace[x2 - halfwayInc + i, y1] = false;
                                door1Placed = true;
                                door1ShouldBePlaced = false;

                            }
                            else if (door2ShouldBePlaced && doorCounter == door2Loc)
                            {
                                doors.Add(new Door(state, x2 - halfwayInc + i, y1, 2, 1, roomNumSpace[x2 - halfwayInc + i, y1], true));
                                doorSpace[x2 - halfwayInc + i, y1] = true;
                                wallSpace[x2 - halfwayInc + i, y1] = false;
                                door2Placed = true;
                                door2ShouldBePlaced = false;
                            }
                            doorCounter++;

                            walkingSpace[x2 - halfwayInc + i, y1] = true;

                            if (wallSpace[x2 - halfwayInc + i, y1] && roomNumSpace[x2 - halfwayInc + i, y1] == hallwayNum)
                            {
                                wallSpace[x2 - halfwayInc + i, y1] = false;
                            }
                        }

                        

                        walkingSpace[x2 - halfwayInc + i, y1] = true;
                        if (roomNumSpace[x2 - halfwayInc + i, y1] == -1)
                        {
                            roomNumSpace[x2 - halfwayInc + i, y1] = hallwayNum;
                            hallwaySpace[x2 - halfwayInc + i, y1] = true;
                        }
                    }
                }
            }
            else
            {
                y1 +=  wide - halfwayInc;

                for (; y1 >= y2; y1--)
                {

                    if (!door1Placed)
                    {
                        if (wallSpace[x2, y1] && roomNumSpace[x2, y1] != hallwayNum && roomNumSpace[x2, y1] != -1)
                        {
                            door1ShouldBePlaced = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (!door2Placed)
                    {
                        if (wallSpace[x2, y1] && roomNumSpace[x2, y1] != hallwayNum && roomNumSpace[x2, y1] != -1)
                        {
                            door2ShouldBePlaced = true;

                        }


                    }
                    doorCounter = 0;

                    for (int i = -1; i <= wide; i++)
                    {
                        if (i == -1 || i == wide)
                        {
                            if (roomNumSpace[x2 - halfwayInc + i, y1] == -1)
                            {
                                wallSpace[x2 - halfwayInc + i, y1] = true;
                            }
                        }
                        else
                        {
                            if (door1ShouldBePlaced && doorCounter == door1Loc)
                            {
                                doors.Add(new Door(state, x2 - halfwayInc + i, y1, 2, 1, roomNumSpace[x2 - halfwayInc + i, y1], true));
                                doorSpace[x2 - halfwayInc + i, y1] = true;
                                wallSpace[x2 - halfwayInc + i, y1] = false;
                                door1Placed = true;
                                door1ShouldBePlaced = false;

                            }
                            else if (door2ShouldBePlaced && doorCounter == door2Loc)
                            {
                                doors.Add(new Door(state, x2 - halfwayInc + i, y1, 2, 1, roomNumSpace[x2 - halfwayInc + i, y1], true));
                                doorSpace[x2 - halfwayInc + i, y1] = true;
                                wallSpace[x2 - halfwayInc + i, y1] = false;
                                door2Placed = true;
                                door2ShouldBePlaced = false;
                            }
                            doorCounter++;


                            walkingSpace[x2 - halfwayInc + i, y1] = true;

                            if (wallSpace[x2 - halfwayInc + i, y1] && roomNumSpace[x2 - halfwayInc + i, y1] == hallwayNum)
                            {
                                wallSpace[x2 - halfwayInc + i, y1] = false;
                            }
                        }

                        

                        walkingSpace[x2 - halfwayInc + i, y1] = true;
                        if (roomNumSpace[x2 - halfwayInc + i, y1] == -1)
                        {
                            roomNumSpace[x2 - halfwayInc + i, y1] = hallwayNum;
                            hallwaySpace[x2 - halfwayInc + i, y1] = true;
                        }
                    }
                }
            }

            numRooms++;
        }

        public double distanceBtwnPts(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public void draw(Graphics g)
        {
            
            for (int i = 0; i < state.room.width; i++)
            {
                for (int j = 0; j < state.room.height; j++)
                {
                    if (roomNumSpace[i, j] != -1)
                        g.DrawImage(floor, (int)(i * state.size + state.form.ClientSize.Width / 2 - state.hero.x * state.size), (int)(j * state.size + state.form.ClientSize.Height / 2 - state.hero.y * state.size));
                    if(wallSpace[i,j])
                        g.DrawImage(wall, (int)(i * state.size + state.form.ClientSize.Width / 2 - state.hero.x * state.size), (int)(j * state.size + state.form.ClientSize.Height / 2 - state.hero.y * state.size));
                }
            }
                        
            foreach (Stairs stair in stairs)
                stair.draw(g);

            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.draw(g); 
            }
            foreach (Unit enemy in enemies)
                enemy.draw(g);

            foreach (Door door in doors) {  
                door.draw(g);
            }
        }

    }
}
