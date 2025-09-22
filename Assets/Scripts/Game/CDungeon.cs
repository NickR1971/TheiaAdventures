using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeon : IService
{
    void Create(uint _gameID);
    int GetSequenceNumber(uint _max);
    IGameMap GetGameMap();
}

public class CDungeon : MonoBehaviour, IDungeon
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject solidPrefab;
    [SerializeField] private GameObject[] roomPrefabs = new GameObject[16];
    private IDialog dialog = null;
    private IGameConsole gameConsole = null;
    private uint gameID = 0;
    private CRand buildSequence;
    private CellCoordsCalculator cellCalculator;
    private CRoom[] map;
    private int currentRoomNumber;
    private const int mapWidth = 10;
    private const int mapHeight = 10;

    private void Awake()
    {
        AllServices.Container.Register<IDungeon>(this);
        ISaveLoad sl = AllServices.Container.Get<ISaveLoad>();
        if (sl.IsHexCell()) cellCalculator = new CellHexCalculator();
        else cellCalculator = new CellSquareCalculator();
        map = new CRoom[mapWidth * mapHeight];
        cellCalculator.CreateMap(mapWidth, mapHeight);
    }

    private void Start()
    {
        dialog = AllServices.Container.Get<IDialog>();
        gameConsole = AllServices.Container.Get<IGameConsole>();
        if (gameID != 0) BuildGame();
    }

    private int GetRoomNumber(int _x, int _y) => _y * mapWidth + _x;

    private bool CheckPosition(int _x, int _y)
    {
        if (_x < 1 || _y < 1) return false;
        if (_x > (mapWidth - 2) || _y > (mapHeight - 2)) return false; // don't use border rooms
        if (map[GetRoomNumber(_x, _y)] != null) return false;
        return true;
    }

    private bool CreateRoom(int _x, int _y, bool _north = true, bool _south = true, bool _west = true, bool _east = true)
    {
        if (CheckPosition(_x, _y) == false) return false;

        int roomNumber = GetRoomNumber(_x, _y);
        int roomtype = 0;

        if (_east) roomtype += 1;
        if (_west) roomtype += 2;
        if (_south) roomtype += 4;
        if (_north) roomtype += 8;

        if (roomtype == 15) _north = _south = _west = _east = false;

        map[roomNumber] = Instantiate(roomPrefabs[roomtype], CRoom.CalcPosition(_x, _y), Quaternion.identity, transform).GetComponent<CRoom>();
        map[roomNumber]
            .Init(this)
            .SetWalls(_north, _south, _west, _east)
            .SetBasePosition(_x, _y);
        return true;
    }

    private int GetFreeNearList(int _x, int _y, out EMapDirection[] _freedir)
    {
        int n = 0;
        _freedir = new EMapDirection[4];
        CRoom currentRoom = map[GetRoomNumber(_x, _y)];

        if (CheckPosition(_x + 1, _y))
        {
            if (currentRoom.IsFreeEast()) _freedir[n++] = EMapDirection.east;
        }

        if (CheckPosition(_x - 1, _y))
        {
            if (currentRoom.IsFreeWest()) _freedir[n++] = EMapDirection.west;
        }

        if (CheckPosition(_x, _y + 1))
        {
            if (currentRoom.IsFreeNorth()) _freedir[n++] = EMapDirection.north;
        }

        if (CheckPosition(_x, _y - 1))
        {
            if (currentRoom.IsFreeSouth()) _freedir[n++] = EMapDirection.south;
        }
        //Debug.Log($"Check x={_x} y={_y} n={n}");
        return n;
    }

    private int GenerateMapFrom(int _x,int _y, int _number, bool _north = true, bool _south = true, bool _west = true, bool _east = true)
    {
        EMapDirection dir;
        int n, x, y;

        if (!CreateRoom(_x, _y, _north, _south, _west, _east))  { Debug.LogError($"Can't creat room at x={_x} y={_y}!");  }
        _number--;
        while (_number > 0)
        {
            n = GetFreeNearList(_x, _y, out EMapDirection[] dirFree);

            _north = _south = _west = _east = true;

            if (n == 0) return _number;
            if (n == 1) dir = dirFree[0];
            else dir = dirFree[GetSequenceNumber((uint)n)];
            x = _x; y = _y;
            switch (dir)
            {
                case EMapDirection.north:
                    _south = false;
                    y++;
                    break;
                case EMapDirection.south:
                    _north = false;
                    y--;
                    break;
                case EMapDirection.east:
                    _west = false;
                    x++;
                    break;
                case EMapDirection.west:
                    _east = false;
                    x--; 
                    break;
            }
            n = GetSequenceNumber(100);

            // шанс на кімнату без стін
            if (n < 4) _north = _south = _west = _east = false;
            else if (n < 97) // шанс гарантованого тупика
            {
                if(!_north)
                {
                    switch(GetSequenceNumber(6))
                    {
                        case 1:
                            _south = false;
                            break;
                        case 2:
                            _west = false;
                            break;
                        case 3:
                            _east = false;
                            break;
                        case 4:
                            _south = false;
                            _west = false;
                            break;
                        case 5:
                            _west = false;
                            _east = false;
                            break;
                        default:
                            _east = false;
                            _south = false;
                            break;
                    }
                }
                else if (!_south)
                {
                    switch (GetSequenceNumber(6))
                    {
                        case 1:
                            _north = false;
                            break;
                        case 2:
                            _west = false;
                            break;
                        case 3:
                            _east = false;
                            break;
                        case 4:
                            _north = false;
                            _west = false;
                            break;
                        case 5:
                            _west = false;
                            _east = false;
                            break;
                        default:
                            _east = false;
                            _north = false;
                            break;
                    }
                }
                else if (!_west)
                {
                    switch (GetSequenceNumber(6))
                    {
                        case 1:
                            _north = false;
                            break;
                        case 2:
                            _south = false;
                            break;
                        case 3:
                            _east = false;
                            break;
                        case 4:
                            _north = false;
                            _south = false;
                            break;
                        case 5:
                            _south = false;
                            _east = false;
                            break;
                        default:
                            _east = false;
                            _north = false;
                            break;
                    }
                }
                else if (!_east)
                {
                    switch (GetSequenceNumber(6))
                    {
                        case 1:
                            _north = false;
                            break;
                        case 2:
                            _west = false;
                            break;
                        case 3:
                            _south = false;
                            break;
                        case 4:
                            _north = false;
                            _west = false;
                            break;
                        case 5:
                            _west = false;
                            _south = false;
                            break;
                        default:
                            _south = false;
                            _north = false;
                            break;
                    }
                }
            }

            if (x == 1) _west = true;
            if (x == mapWidth - 2) _east = true;
            if (y == 1) _south = true;
            if (y == mapHeight - 2) _north = true;

            _number = GenerateMapFrom(x, y, _number, _north, _south, _west, _east);
        }
        return 0;
    }
    private void OnCreateCell(Cell _cell)
    {
        _cell.AddRoom(map[currentRoomNumber]);
        _cell.SetObject(Instantiate(cellPrefab, transform));
    }

    private void BuildGame()
    {
        buildSequence = new CRand(gameID);
        const int maxroom = 50;
        const int startX = 2;
        const int startY = 2;

        int n = maxroom - GenerateMapFrom(startX, startY, maxroom);
        Debug.Log($"Create {n} rooms");

        int x, y;
        for (y = 0; y < mapHeight; y++)
        {
            for (x = 0; x < mapWidth; x++)
            {
                currentRoomNumber = y * mapWidth + x;
                if (map[currentRoomNumber] == null)
                {
                    Instantiate(solidPrefab, CRoom.CalcPosition(x, y), Quaternion.identity, transform);
                }
            }
        }
   
        cellCalculator.SetOnCreateCellAction(OnCreateCell);
        for (y = 0; y < mapHeight; y++)
        {
            for(x=0;x<mapWidth;x++)
            {
                currentRoomNumber = y * mapWidth + x;
                if (map[currentRoomNumber] == null) continue;
                cellCalculator.Build(x, y, CRoom.CalcPosition(x, y));
            }
        }
    }

    //---------------------------------
    // IDungeon
    //---------------------------------
    public void Create(uint _gameID)
    {
        gameID = _gameID;
        if (dialog != null) BuildGame();
    }

    public int GetSequenceNumber(uint _max)
    {
        if (buildSequence == null)
        {
            Debug.LogError("Build sequence not initialized!");
            return 0;
        }
        return (int)buildSequence.Dice(_max)-1;
    }

    public IGameMap GetGameMap()
    {
        return cellCalculator;
    }
}
