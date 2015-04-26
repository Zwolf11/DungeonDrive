using System;
using System.Collections.Generic;

namespace DungeonDrive
{
    public class LevelInfo
    {

        public GameState state;
        public String dirName;
        public List<LevelInfo> subdirs = new List<LevelInfo>();
        public bool[] chestsOpened;
        public bool[] roomsDrawn;
        //public bool[][] drawingSpace;
        public bool[] doorsOpened;

        public bool initialized = false;


        public LevelInfo(GameState state, String dirName, bool levelShouldBeSet)
        {
            this.state = state;
            this.dirName = dirName;
            if (levelShouldBeSet)
            {
                setLevel();
            }
        }

        public void setLevel()
        {
            setDoors();
            setRooms();
            setChests();
            initialized = true;
        }

        public void setDoors()
        {
            doorsOpened = new bool[state.room.numRooms * 2];


            for (int i = 0; i < state.room.numRooms * 2; i++)
            {
                doorsOpened[i] = true;
            }

            foreach(Door door in state.room.doors)
            {
                doorsOpened[door.id] = door.closed;
            }

            foreach(Door door in state.room.doorsNotDrawn)
            {
                doorsOpened[door.id] = door.closed;
            }

        }

        public void setRooms()
        {
            
            roomsDrawn = new bool[state.room.maxStairs];
            for (int i = 0; i < state.room.maxStairs; i++)
            {
                if (i < state.room.numRooms)
                {
                    roomsDrawn[i] = state.room.roomDrawn[i];
                }
                else
                {
                    roomsDrawn[i] = false;
                }
            }


        }

        public void setChests()
        {
            chestsOpened = new bool[state.room.maxObstacles];

            for (int i = 0; i < state.room.maxObstacles; i++)
            {
                chestsOpened[i] = true;
            }

            foreach(Obstacle obs in state.room.obstacles)
            {
                if (obs is Chest)
                {
                    Chest chest = (Chest) obs;
                    chestsOpened[obs.id] = chest.closed;
                }
            }

            foreach (Obstacle obs in state.room.obstaclesNotDrawn)
            {
                if (obs is Chest)
                {
                    Chest chest = (Chest)obs;
                    chestsOpened[obs.id] = chest.closed;
                }
            }
        }

        public LevelInfo getSubDir(String subDirName)
        {
            foreach (LevelInfo levelInfo in subdirs)
            {
                if (levelInfo.dirName.Equals(subDirName))
                {
                    return levelInfo;
                }
            }

            return null;
        }

    }
}
