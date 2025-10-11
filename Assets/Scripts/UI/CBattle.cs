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
    ICharacter GetCurrentCharacter();
}

public class CBattle : CUI, IBattle
{
    private IGame game;
    private ICamera iCamera;
    private IGameConsole gameConsole;
    private ICharacterManager charManager;
    private IDungeon dungeon;
    private IGameMap gameMap;
    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject characterName;
    [SerializeField] private Sprite[] actionSprites = new Sprite[10];
    [SerializeField] private Button[] actionButtons = new Button[10];
    private Image imgChar;
    private Text nameChar;
    private ICharacter currentCharacter;
    private const int startCellsSize = 100;
    private Cell[] startSells = new Cell[startCellsSize];
    private int countStartCells = 0;
    private int maxCells;
    private int numActions;
    private int[] charActions = null;

    private void Awake()
    {
        AllServices.Container.Register<IBattle>(this);
        currentCharacter = null;
        numActions = 0;
    }
    private void Start()
    {
        InitUI();
        iCamera = AllServices.Container.Get<ICamera>();
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
    private void OnDestroy()
    {
        AllServices.Container.UnRegister<IBattle>();
    }
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
    private bool CreateCharacterTemplate(ECharacterType _ctype, string _cName, out Character _character)
    {
        bool result = false;
        _character = new Character();

        _character.cType = _ctype;
        _character.cName = _cName;
        switch (_ctype)
        {
            case ECharacterType.knight:
                _character.attributes.might = 4;
                _character.attributes.dexterity = 3;
                _character.attributes.intelligence = 2;
                _character.attributes.knowledge = 3;
                _character.attributes.personality = 3;
                result = true;
                break;
            case ECharacterType.mage:
                _character.attributes.might = 2;
                _character.attributes.dexterity = 3;
                _character.attributes.intelligence = 4;
                _character.attributes.knowledge = 3;
                _character.attributes.personality = 3;
                result = true;
                break;
            case ECharacterType.zombie:
                _character.attributes.might = 6;
                _character.attributes.dexterity = 2;
                _character.attributes.intelligence = 1;
                _character.attributes.knowledge = 1;
                _character.attributes.personality = 1;
                result = true;
                break;
            case ECharacterType.skeleton:
                _character.attributes.might = 3;
                _character.attributes.dexterity = 3;
                _character.attributes.intelligence = 1;
                _character.attributes.knowledge = 1;
                _character.attributes.personality = 1;
                result = true;
                break;
        }

        if (result)
        {
            _character.secondaryAttributes.speed = _character.attributes.dexterity;
            _character.secondaryAttributes.initiative = _character.attributes.dexterity;
            _character.secondaryAttributes.reaction = 1;
            _character.points.redHits = CScale.GetEValue(_character.attributes.might);
            _character.points.yellowHits = _character.points.redHits / 2;
            _character.points.blueHits = 0;
            _character.points.greenHits = 0;
            _character.points.actions = CScale.GetEValue(_character.secondaryAttributes.initiative);
            _character.points.mana = CScale.GetEValue(_character.attributes.intelligence);
            _character.points.will = CScale.GetEValue(_character.attributes.personality);
            _character.currentPoints = _character.points;
        }

        return result;
    }
    private CActor CreateCharacter(ECharacterType _charType)
    {
        Cell cell;
        Sprite spr;
        GameObject prefab;
        Character chrTemp;
        CActor actor;
 
        if (!charManager.GetCharacter(_charType, out spr, out prefab))
            return null;
        cell = GetStartCell();

        if (!CreateCharacterTemplate(_charType, _charType.ToString() + cell.GetNumber(), out chrTemp))
            return null;

        actor = dungeon.CreateCharacter(prefab, cell).SetSprite(spr);
        actor.SetCharacter(CCharacter.Create(chrTemp));
        return actor;
   }
    private void CreateTestCharacter()
    {
        Cell cell = CreateCharacter(ECharacterType.zombie).GetCurrentCell();
        CreateCharacter(ECharacterType.mage);
        CreateCharacter(ECharacterType.skeleton);
        CreateCharacter(ECharacterType.knight);
        //gameConsole.ExecuteCommand("cell " + cell.GetNumber());
        iCamera.SetViewPoint(cell.GetPosition());
    }
    public void SetCharacterName(string _name)
    {
        nameChar.text = _name;
    }

    public void SetCharacterSprite(Sprite _sprite)
    {
        imgChar.sprite = _sprite;
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
    public ICharacter GetCurrentCharacter() => currentCharacter;
    public void CharCommand(int _cmd)
    {
        CharCommand((CharacterCommand)_cmd);
    }
    public void CharCommand(CharacterCommand _cmd)
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(_cmd);
    }
}
