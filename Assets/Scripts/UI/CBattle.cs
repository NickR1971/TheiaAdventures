using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public interface IBattle : IService
{
    void SetCharacterSprite(Sprite _sprite);
    void SetCharacterName(string _name);
    void SetCurrentCharacter(ICharacter _charSelected);
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
    [SerializeField] private Sprite[] actionSprites = new Sprite[14];
    [SerializeField] private Button[] actionButtons = new Button[14];
    private Image imgChar;
    private Text nameChar;
    private ICharacter currentCharacter;
    private const int startCellsSize = 100;
    private Cell[] startSells = new Cell[startCellsSize];
    private int countStartCells = 0;
    private int maxCells;
    private int numActions;
    private int[] charActions = null;

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
            .SetName(_charType.ToString() + cell.GetNumber());
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
        currentCharacter = null;
        numActions = 0;
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
    private void SetListener(Button _btn, int _num)
    {
        switch(_num)
        {
            case 0:
                _btn.onClick.AddListener(() => CharCommand(0));
                break;
            case 1:
                _btn.onClick.AddListener(() => CharCommand(1));
                break;
            case 2:
                _btn.onClick.AddListener(() => CharCommand(2));
                break;
            case 3:
                _btn.onClick.AddListener(() => CharCommand(3));
                break;
            case 4:
                _btn.onClick.AddListener(() => CharCommand(4));
                break;
            case 5:
                _btn.onClick.AddListener(() => CharCommand(5));
                break;
            case 6:
                _btn.onClick.AddListener(() => CharCommand(6));
                break;
            case 7:
                _btn.onClick.AddListener(() => CharCommand(7));
                break;
            case 8:
                _btn.onClick.AddListener(() => CharCommand(8));
                break;
            case 9:
                _btn.onClick.AddListener(() => CharCommand(9));
                break;
            case 10:
                _btn.onClick.AddListener(() => CharCommand(10));
                break;
            case 11:
                _btn.onClick.AddListener(() => CharCommand(11));
                break;
            case 12:
                _btn.onClick.AddListener(() => CharCommand(12));
                break;
            case 13:
                _btn.onClick.AddListener(() => CharCommand(13));
                break;
        }
    }
    public void SetCurrentCharacter(ICharacter _charSelected)
    {
        int i;

        currentCharacter = _charSelected;
        nameChar.text = currentCharacter.GetName();
        imgChar.sprite = currentCharacter.GetSprite();
        for (i = 0; i < numActions; i++)
        {
            actionButtons[i].onClick.RemoveAllListeners();
            actionButtons[i].gameObject.SetActive(false);
        }
        numActions = currentCharacter.GetActions(out charActions);
        for (i = 0; i < numActions; i++)
        {
            actionButtons[i].gameObject.SetActive(true);
            actionButtons[i].image.sprite = actionSprites[charActions[i]];
            SetListener(actionButtons[i], charActions[i]);
        }
    }
    public void OnLeft()
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(ActorCommand.turnleft);
    }
    public void OnRight()
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(ActorCommand.turnright);
    }
    public void OnForward()
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(ActorCommand.walk);
    }
    public void OnMelee()
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(ActorCommand.melee);
    }
    public void CharCommand(int _cmd)
    {
        CharCommand((ActorCommand)_cmd);
    }
    public void CharCommand(ActorCommand _cmd)
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(_cmd);
    }
}
