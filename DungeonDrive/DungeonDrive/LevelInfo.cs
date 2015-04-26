using System;

public class LevelInfo
{

    public State state;
    public String dirName;
    public ArrayList<LevelInfo> subdirs = new ArrayList<LevelInfo>();
    public bool[] chestsOpened;
    public bool[] roomsDrawn;
    //public bool[][] drawingSpace;
    public bool[] doorsOpened;


	public LevelInfo(State state, String dirName)
	{
        this.state = state;
        this.dirName = dirName;
	}

    public void setLevel()
    {
        setDoors();
        setRooms();
        setChests();
    }

    public void setDoors()
    {
        doorsOpened = new bool[state.room.numRooms * 2];

        
        for (int i = 0; i < state.room.numRooms * 2; i++)
        {
            doorsOpened[i] = true;
        }

        for (int i = 0; i < state.room.doors.Length; i++)
        {
            doorsOpened[state.room.doors.get(i).id] = state.room.doors.get(i).closed;
        }

        for (int i = 0; i < state.room.doorsNotDrawn.Length; i++)
        {
            doorsOpened[state.room.doorsNotDrawn.get(i).id] = state.room.doorsNotDrawn.get(i).closed;
        }

    }

    public void setRooms()
    {
        roomsRevealed = new bool[state.room.maxStairs];
        for(int i = 0; i < state.room.maxStairs; i++)
        {
            if (i < state.room.numRooms)
            {
                roomsRevealed[i] = state.room.roomsDrawn[i];
            }
            else
            {
                roomsRevealed[i] = false;
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

        for (int i = 0; i < state.room.obstacles.Length; i++)
        {
            chestsOpened[state.room.doors.get(i).id] = state.room.doors.get(i).closed;
        }

        for (int i = 0; i < state.room.obstaclesNotDrawn.Length; i++)
        {
            chestsOpened[state.room.obstaclesNotDrawn.get(i).id] = state.room.obstaclesNotDrawn.get(i).closed;
        }
    }

}
