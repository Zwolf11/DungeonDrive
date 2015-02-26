using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DungeonDrive
{
    public class Room
    {

        //////// IF YOU WANT TO DISABLE WALL BOUNDARIES TO TEST OTHER THINGS, SET noBoundaries TO TRUE ////////
        public bool noBoundaries = false;


        public int width;
        public int height;
        public List<Obstacle> obstacles = new List<Obstacle>();
        public List<Unit> enemies = new List<Unit>();
        //public List<Door> doors = new List<Door>();
        public List<Stairs> stairs = new List<Stairs>();
        public List<int> connectedRooms = new List<int>();

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

        public const int minSizeOfInitRoom = 5;
        public const int maxSizeOfInitRoom = 9;


        public const int minSizeHallway = 2;
        public const int maxSizeHallway = 4;

        public const int maxEnemies = 20;
        public const int maxObstacles = 100;   // max number of these objects to generate in a room.
        public const int maxStairs = 100;

        public const int minRoomWidth = 20;
        public const int minRoomHeight = 20;
        public const int maxRoomWidth = 100;
        public const int maxRoomHeight = 100;

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

            } else {
                // this is not the initial C file
                addStairs(false, parentDir);
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

            /////////// IF THIS IS THE INITIAL ROOM, 

            if (parentDir == null)
            {
                int x1, y1;
                while (roomNumSpace[x1 = rand.Next(0, width - 1), y1 = rand.Next(0, height)] == -1) ;

                G.hero.x = x1 + 0.5;
                G.hero.y = y1 + 0.5;
                freeSpace[x1, y1] = false;
            }

            /////////   FIND STAIRCASE YOU ARE COMING FROM   /////////

            foreach (Stairs stair in stairs)
            {
                if (stair.path.Equals(G.pastRoom))
                {                                         
                    G.hero.x = /*G.hero.xNext = */stair.x + stair.xDirection + 0.5;      // place you on the correct side of it
                    G.hero.y = /*G.hero.yNext = */stair.y + stair.yDirection + 0.5;
                    break;
                }
            }

            recalcRoomNums();

            /////////  CONNECT ROOMS WITH HALLWAYS   ////

            int q = 0;
            bool[] roomsConnected = new bool[numRooms];


            foreach (Stairs stair in stairs)
            {
                if (q == 0)
                {
                    connectedRooms.Add(stair.roomNum);

                }

                bool connected = false;
                // find if roomNum is connected
                foreach (int num in connectedRooms)
                {
                    if (num == stair.roomNum)
                    {
                        connected = true;
                    }
                }

                if (!connected)
                {
                    double shortestDistance = width + height;
                    Stairs shortestStair = stair;

                    foreach (Stairs stair2 in stairs)
                    {

                        bool connected2 = false;

                        foreach(int num2 in connectedRooms){
                            if(num2 == stair2.roomNum){
                                connected2 = true;
                            }
                        }

                        if(connected2){
                            double dist = distanceBtwnPts(stair.x, stair.y, stair2.x, stair2.y);
                            if (dist < shortestDistance)
                            {
                                shortestDistance = dist;
                                shortestStair = stair2;
                            }
                        }
                    }

                    makeHallway(stair.x, stair.y, shortestStair.x, shortestStair.y);
                    connectedRooms.Add(stair.roomNum);
                }
                q++;
            }

            //////////   TRAVERSE ALL FILES   //////////

            for(int i = 0; i < files.Length; i++){
                matchExtension(Path.GetExtension(files[i]));     // match each file extension and spawn the corresponding object
            }

            // determine hero starting point
            // find stair that matches the pastRoom

            if (!noBoundaries)
            {
                addBoundaries();
            }
            recalcRoomNums();

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
                        otherFound();
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
            addObstacle();
        }

        public void audioFound()
        {
            addObstacle();
        }

        public void videoFound()
        {
            addObstacle();
        }

        public void imageFound()
        {
            addObstacle();
        }

        public void otherFound()
        {
            temp_sd = safe_distance;
            if((int) rand.Next(0,100) % 2 == 0){
                while (!addEnemy(new Bat(rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5))) ;
                numBats++;

            }
            else 
            {
                while (!addEnemy(new Spider(rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5))) ;
                numSpiders++;
                //while (!addEnemy(new Boss(rand.Next(0, width - 1) + 0.5, rand.Next(0, height - 1) + 0.5)));
            }            
        }

        public void addDoor()
        {

        }

        public void addStairs(bool down, String path)
        {
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

            int x;
            int y; 
            int tHeight = 1;
            int tWidth = 1;

            int sizeOfInitStairRoom = rand.Next(minSizeOfInitRoom, maxSizeOfInitRoom + 1);
            int radiusInitRoom = (int)((sizeOfInitStairRoom) / 2);

            do
            {
                x = rand.Next(radiusInitRoom, width - 2 - radiusInitRoom);
                y = rand.Next(radiusInitRoom, height - 2 - radiusInitRoom);
            } while (!freeSpace[x, y] || !freeSpace[x + xDirFromChar(direction), y + yDirFromChar(direction)] || roomNumSpace[x, y] != -1);


            int maxRandom = (int) Math.Pow(sizeOfInitStairRoom, 2) - (4 * sizeOfInitStairRoom) + 2;   // indicates how many non-border cells there are. We don't want the stairs to be on a border to avoid adjacent stair tiles.

            int stairLocation = rand.Next(0, maxRandom);
            int counter = 0;
            int stairX  = x;
            int stairY = y;

            for (int i = 0; i < sizeOfInitStairRoom; i++)
            {
                for (int j = 0; j < sizeOfInitStairRoom; j++)
                {
                    if (i != 0 && i != sizeOfInitStairRoom - 1 && j != 0 && j != sizeOfInitStairRoom - 1)
                    {
                        // a non-border cell.
                        if(counter == stairLocation){
                            stairX = x + i - radiusInitRoom;
                            stairY = y + j - radiusInitRoom;
                        }
                        counter++;
                    }
                    mergeRoom((x - radiusInitRoom) + i, (y - radiusInitRoom) + j, numRooms); 
                }
            }

            stairs.Add(new Stairs(stairX, stairY, tWidth, tHeight, roomNumSpace[stairX,stairY], down, path, direction));

            stairSpace[stairX, stairY] = true;
            freeSpace[stairX, stairY] = false;
            freeSpace[stairX + xDirFromChar(direction), stairY + yDirFromChar(direction)] = false;

            numRooms++;
            numStairs++;

            return;

        }

        public void addObstacle()
        {
           
            if (numObstacles >= (maxObstacles - 1))
            {
                return;

            }


            int x = 0;
            int y = 0;
            bool intersect = true;
            int pWidth = 1;
            int pHeight = 1;


            while (intersect)
            {
                intersect = false;

                x = rand.Next(0, width - 4);
                y = rand.Next(0, height - 3);

                // check to make sure the entire obstacle can be placed on the map without interference.
                for (int i = x; i < (x + pWidth); i++)
                {
                    for (int j = y; j < (y + pHeight); j++)
                    {
                        if (!freeSpace[i, j] || roomNumSpace[i, j] == -1)
                            intersect = true;
                    }
                }
            }
            

            obstacles.Add(new Pillar(x,y,pWidth,pHeight,roomNumSpace[x,y]));
            for(int i = x; i < (x + pWidth); i++)
            {
                for(int j = y; j < (y + pHeight); j++)
                {
                    walkingSpace[i,j] = false;
                    freeSpace[i, j] = false;
                }
            }

            numObstacles++;

        }

        public bool addEnemy(Unit e)
        {

            if (numEnemies >= (maxEnemies - 1))
            {
                return true;
            }

            if(Math.Sqrt(Math.Pow(e.x - G.hero.x, 2) + Math.Pow(e.y - G.hero.y, 2)) <= temp_sd || !freeSpace[(int)e.x, (int) e.y] || roomNumSpace[(int)e.x, (int) e.y] == -1)
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
                    if(roomNumSpace[i,j] == -1){
                        walkingSpace[i, j] = false;
                    }
                }
            }

        }

        public void makeHallway(int x1, int y1, int x2, int y2)
        {
            int wide = (int) rand.Next(minSizeHallway, maxSizeHallway + 1);
            int deltaX = x1 - x2;
            int deltaY = y1 - y2;

            int hallwayNum = numRooms;

            if (deltaX <= 0)
            {
                // x1 < x2
                for (; x1 < x2; x1++)
                {
                    for (int i = 0; i < wide; i++)
                    {
                        walkingSpace[x1, y1 - 1 + i] = true;
                        if (roomNumSpace[x1, y1 - 1 + i] == -1)
                        {
                            roomNumSpace[x1, y1 - 1 + i] = hallwayNum;
                        }
                    }
                }
                x1 -= (int) Math.Ceiling((double) wide / 2.0);
            }
            else
            {
                for (; x1 > x2; x1--)
                {
                    for (int i = 0; i < wide; i++)
                    {
                        walkingSpace[x1, y1 - 1 + i] = true;
                        if (roomNumSpace[x1, y1 - 1 + i] == -1)
                        {
                            roomNumSpace[x1, y1 - 1 + i] = hallwayNum;
                        }
                    }
                }
                x1 += (int) Math.Ceiling((double) wide / 2);
            }


            if(deltaY < 0){

                //y1 -= (int)((double)wide / 2.0);
               
                for (; y1 < y2; y1++)
                {
                    for (int i = 0; i < wide; i++)
                    {
                        walkingSpace[x1 - 1 + i, y1] = true;
                        if (roomNumSpace[x1 - 1 + i, y1] == -1)
                        {
                            roomNumSpace[x1 - 1 + i, y1] = hallwayNum;
                        }
                    }
                }
            }
            else
            {
                //y1 += (int)((double)wide / 2.0);

                for (; y1 > y2; y1--)
                {
                    for (int i = 0; i < wide; i++)
                    {
                        walkingSpace[x1 - 1 + i, y1] = true;
                        if (roomNumSpace[x1 - 1 + i, y1] == -1)
                        {
                            roomNumSpace[x1 - 1 + i, y1] = hallwayNum;
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
            
            for (int i = 0; i < G.room.width; i++){
                for (int j = 0; j < G.room.height; j++)
                {
                    if (roomNumSpace[i, j] != -1)
                    {
                        g.DrawRectangle(Pens.Black, (int)(G.width / 2 + i * G.size - G.hero.x * G.size), (int)(G.height / 2 + j * G.size - G.hero.y * G.size), G.size, G.size);

                        //g.DrawRectangle(Pens.Black, (int)(i * G.size + G.width / 2 - G.hero.x * G.size * G.hero.radius * 2 - G.size * G.hero.radius * 2), (int)(j * G.size + G.height / 2 - G.hero.y * G.size * G.hero.radius * 2 - G.size * G.hero.radius * 2), G.size, G.size);
                    }
                    //else
                    //{
                    //    g.DrawRectangle(Pens.Black, (int)(i * G.size + G.width / 2 - G.hero.x * G.size - G.size / 2), (int)(j * G.size + G.height / 2 - G.hero.y * G.size - G.size / 2), G.size, G.size);
                    //}
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

            /*foreach (Door door in doors)
                door.draw(g);*/

            for (int i = 0; i < G.hero.atk_cd.Length; i++)
                if (!G.hero.atk_cd[i])
                    g.FillEllipse(Brushes.Red, i * 30, 0, 30, 30);
        }

    }
}
