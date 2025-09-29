using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public interface IBattle : IService
{
    void SetCharacterSprite(Sprite _sprite);
    void SetCharacterName(string _name);
}

public class CBattle : CUI, IBattle
{
    private IGame game;
    private IGameConsole gameConsole;
    private ICharacterManager charManager;
    private IDungeon dungeon;
    private IGameMap gameMap;
    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject characterName;
    private Image imgChar;
    private Text nameChar;
    private const int startCellsSize = 100;
    private Cell[] startSells = new Cell[startCellsSize];
    private int countStartCells = 0;
    private int maxCells;

    private void SelectStartCells()
    {
        int i;
        Cell cell;

        maxCells = gameMap.GetWidth() * gameMap.GetHeight();
        for (i = 0; i < maxCells; i++)
        {
            cell = gameMap.GetCell(i);
            if (cell == null) continue;
            if (cell.GetBaseType() == ECellType.start)
            {
                startSells[countStartCells++] = cell;
                if (countStartCells == startCellsSize) break;
            }
        }
    }
    private Cell GetStartCell()
    {
        Cell cell;
        int n;
        n = dungeon.GetSequenceNumber((uint)countStartCells);
        cell = startSells[n];

        countStartCells--; // remove selected cell from list
        startSells[n] = startSells[countStartCells];

        return cell;
    }
    private CActor CreateCharacter(ECharacterType _charType)
    {
        Cell cell;
        Sprite spr;
        GameObject prefab;
 
        if (!charManager.GetCharacter(_charType, out spr, out prefab))
            return null;
        cell = GetStartCell();

        return dungeon.CreateCharacter(prefab, cell)
            .SetSprite(spr)
            .SetName(CLocalization.GetString(ELocalStringID.game_class_zombie) + cell.GetNumber());
   }
    private void CreateTestCharacter()
    {
        Cell cell = CreateCharacter(ECharacterType.zombie).GetCurrentCell();
        CreateCharacter(ECharacterType.mage);
        CreateCharacter(ECharacterType.skeleton);
        CreateCharacter(ECharacterType.knight);
        gameConsole.ExecuteCommand("cell " + cell.GetNumber());
    }
    public void SetCharacterName(string _name)
    {
        nameChar.text = _name;
    }

    public void SetCharacterSprite(Sprite _sprite)
    {
        imgChar.sprite = _sprite;
    }
    private void Awake()
    {
        AllServices.Container.Register<IBattle>(this);
    }
    private void Start()
    {
        InitUI();
        game = AllServices.Container.Get<IGame>();
        gameConsole = AllServices.Container.Get<IGameConsole>();
        charManager = AllServices.Container.Get<ICharacterManager>();
        dungeon = AllServices.Container.Get<IDungeon>();
        game.CreateGame(CGameManager.GetData());
        CGameManager.GetData().num_scene = 2;
        imgChar = characterImage.GetComponent<Image>();
        nameChar = characterName.GetComponent<Text>();

        gameMap = dungeon.GetGameMap();
        SelectStartCells();
        CreateTestCharacter();
    }
}
