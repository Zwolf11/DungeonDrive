using System;
using System.Collections.Generic;

namespace DungeonDrive
{
    public class AllLevelInfo
    {
        GameState state;
        public List<LevelInfo> allLevels = new List<LevelInfo>();

        public AllLevelInfo(GameState state, String currentRoom)
        {
            this.state = state;
            addLevel(new LevelInfo(state, currentRoom, false));
        }

        public void addLevel(LevelInfo newLevel)
        {
            allLevels.Add(newLevel);
        }

        public void loadLevel(String path)
        {

            foreach (LevelInfo tempLevelInfo in allLevels)
            {
                if (tempLevelInfo.dirName.Equals(path))
                {
                    //Console.WriteLine("Found the file");
                    //update doorsss
                    /*
                    foreach (Door door in state.room.doorsNotDrawn)
                    {
                        if (door.id < tempLevelInfo.doorsOpened.Length)
                        {
                            door.closed = tempLevelInfo.doorsOpened[door.id];
                        }
                    }
                    */

                    // update chests

                    foreach (Obstacle obs in state.room.obstacles)
                    {
                        //Console.WriteLine("Obs");
                    }

                    for (int i = 0; i < state.room.obstacles.Count; i++)
                    {
                        //Console.WriteLine("Checking obstacles " + i);
                        if (state.room.obstacles[i] is Chest)
                        {
                            //Console.WriteLine("Found a chest");
                            if (state.room.obstacles[i].id < tempLevelInfo.chestsOpened.Length)
                            {
                                //Console.WriteLine("Changed chest");
                                Chest chest = (Chest)state.room.obstacles[i];
                                chest.closed = tempLevelInfo.chestsOpened[chest.id];
                                state.room.obstacles[i] = chest;
                            }
                        }
                    }

                    // update room

                    //Console.WriteLine("numRooms = " + state.room.numRooms);
                    if (tempLevelInfo.type.Equals("dungeon"))
                    {
                        for (int i = 0; i < state.room.numRooms; i++)
                        {
                            //if (i < state.room.roomDrawn.Length && i < tempLevelInfo.roomsDrawn.Length)
                            //{
                            //Console.WriteLine("Checking room " + i);

                            if (tempLevelInfo.roomsDrawn[i])
                            {
                                //Console.WriteLine("TRYING TO DRAW ROOM " + i);
                                state.room.updateDrawingGrid(i);
                            }
                            //}
                        }

                        state.room.updateDoorCollisions();
                    }
                    else if (tempLevelInfo.type.Equals("cave"))
                    {
                        for (int i = 0; i < state.room.width; i++)
                        {
                            for (int j = 0; j < state.room.height; j++)
                            {
                                state.room.drawingSpace[i, j] = tempLevelInfo.drawingSpace[i, j];
                            }
                        }
                    }

                }
            }

        }

        public bool levelAlreadyExists(String path)
        {
            foreach (LevelInfo levelInfo in allLevels)
            {
                if(levelInfo.dirName.Equals(path)){
                    return true;
                }
            }

            return false;
        }

        public void updateLevel()
        {
            LevelInfo tempLevelInfo = new LevelInfo(state,"C://",false);
            bool levelSet = false;

            foreach (LevelInfo levelInfo in allLevels)
            {
                if (levelInfo.dirName.Equals(state.room.currentRoom))
                {
                    levelSet = true;
                    tempLevelInfo = levelInfo;
                }
            }

            if (levelSet)
            {
                allLevels.Remove(tempLevelInfo);

                allLevels.Add(new LevelInfo(state, state.room.currentRoom, true));

            }
        }
    }
}
