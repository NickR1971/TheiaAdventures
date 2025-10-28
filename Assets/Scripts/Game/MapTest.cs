using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : ConsoleService
{
    private ICamera iCamera;
    private IDungeon dungeon;
    private IGameMap map;
    private int currentCell;
    private Cell cell;

    private void Start()
    {
        Init();
        AddCommand("cell", OnConsole,"cell number OR direction and distance");
        AddCommand("map", onMap, "map id|hide|unhide");
        iCamera = AllServices.Container.Get<ICamera>();
        dungeon = AllServices.Container.Get<IDungeon>();
        map = dungeon.GetGameMap();
    }
    private void SelectCurrentCell()
    {
            cell = map.GetCell(currentCell);
            if (cell == null)
            {
                gameConsole.ShowMessage($"cell #{currentCell} not found!");
            }
            else
            {
                cell.SetColor(Color.black);
                iCamera.SetViewPoint(cell.GetPosition());
                gameConsole.ShowMessage($"cell #{currentCell} is found at {cell.GetPosition()}");
            }
    }
    private void onMap(string _str)
    {
        if (_str == "hide")
        {
            gameConsole.ShowMessage("hide command not supported now");
        }
        else if(_str=="id")
        {
            gameConsole.ShowMessage("Map ID is: " + CGameManager.GetData().id);
        }
        else if (_str == "unhide")
        {
            CRoom room;
            int x, y;

            for (y = 1; y < dungeon.GetHeight(); y++)
            {
                for (x = 1; x < dungeon.GetWidth(); x++)
                {
                    room = dungeon.GetRoom(x, y);
                    if (room != null) room.Unhide();
                }
            }
        }
        else gameConsole.ShowMessage("none command recognized");

    }
    private void OnConsole(string _str)
    {
        if( CUtil.IsDigit(_str[0]))
        {
            currentCell = CUtil.StringToInt(_str);
            SelectCurrentCell();
        }
        else
        {
            int n = 2;
            EMapDirection dir = EMapDirection.center;

            switch(_str[0])
            {
                case 'n':
                    dir = EMapDirection.north;
                    if (_str[1] == 'e')       {    n = 3;    dir = EMapDirection.northeast;      }
                    if (_str[1] == 'w')       {    n = 3;    dir = EMapDirection.northwest;      }
                    break;
                case 's':
                    dir = EMapDirection.south;
                    if (_str[1] == 'e')      {    n = 3;  dir = EMapDirection.southeast;    }
                    if (_str[1] == 'w')      {    n = 3;  dir = EMapDirection.southwest;    }
                    break;
                case 'e':                    dir = EMapDirection.east;                    break;
                case 'w':                    dir = EMapDirection.west;                    break;
            }
            if (dir == EMapDirection.center)
            {
                    gameConsole.ShowMessage($"{_str}: unknown direction!");
            }
            else
            {
                if (_str.Length < n || !CUtil.IsDigit(_str[n]))
                {
                    gameConsole.ShowMessage("Distance not defined!");
                    return;
                }
                int l = CUtil.StringToInt(_str.Substring(n));
                if (cell == null)
                {
                    gameConsole.ShowMessage("Start cell not defined!");
                    return;
                }
                Cell c = cell;
                int i = 0;
                do
                {
                    c = map.GetCell(c.GetNearNumber(dir));
                    if (c == null) break;
                    c.SetColor(Color.cyan);
                } while (++i < l);
            }
        }
    }
}
