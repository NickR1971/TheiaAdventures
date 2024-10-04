using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame : IService
{
    void CreateGame(SaveData _data);
}

public class CGame : IGame
{
    private IDialog dialog = null;
    private IGameConsole gameConsole = null;
    private IDungeon dungeon = null;

    public CGame()
    {
        Debug.Log("CGame object created");
    }

    //--------------------------------------------------------------
    // IGame interface
    //--------------------------------------------------------------
    public void CreateGame(SaveData _data)
    {
        dialog = AllServices.Container.Get<IDialog>();
        gameConsole = AllServices.Container.Get<IGameConsole>();
        dungeon = AllServices.Container.Get<IDungeon>();
        if (dungeon == null) Debug.LogError("Dungeon interface not found!");
        else dungeon.Create(_data);
    }
}
