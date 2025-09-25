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

    private void CreateTestCharacter()
    {
        Cell[] startSells = new Cell[100];
        Cell cell;
        int countStartCells = 0;
        int maxCells;
        int i;
        Sprite spr;
        GameObject prefab;

        if (!charManager.GetCharacter(ECharacterType.zombie, out spr, out prefab)) return;
        maxCells = gameMap.GetWidth() * gameMap.GetHeight();
        SetCharacterSprite(spr);
        SetCharacterName(CLocalization.GetString(ELocalStringID.game_class_zombie));
        for (i = 0; i < maxCells; i++)
        {
            cell = gameMap.GetCell(i);
            if (cell == null) continue;
            if (cell.GetBaseType() == ECellType.start)
            {
                startSells[countStartCells] = cell;
                countStartCells++;
                if (countStartCells == 100) break;
            }
        }
        i = dungeon.GetSequenceNumber((uint)countStartCells);
        cell = startSells[i];
        dungeon.CreateCharacter(prefab, cell);
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
        gameConsole.Show();
        CreateTestCharacter();
    }
}
