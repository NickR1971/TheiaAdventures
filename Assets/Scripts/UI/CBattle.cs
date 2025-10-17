using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public interface IBattle : IService
{
    void ShowCharacterSprite(Sprite _sprite);
    void ShowCharacterName(string _name);
    void SetCurrentCharacter(ICharacter _charSelected);
    ICharacter GetCurrentCharacter();
}

public class CBattle : CUI, IBattle
{
    private IGame game;
    private ICamera iCamera;
    private IGameConsole gameConsole;
    private ICharacterManager iCharacterManager;
    private IDungeon dungeon;
    private IGameMap gameMap;
    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject characterName;
    [SerializeField] private CTextLocalize characterOrigin;
    [SerializeField] private CTextLocalize characterClass;
    [SerializeField] private CTextAttribute Might;
    [SerializeField] private CTextAttribute Dex;
    [SerializeField] private CTextAttribute Intel;
    [SerializeField] private CTextAttribute Pers;
    [SerializeField] private CTextAttribute Know;
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
        iCharacterManager = AllServices.Container.Get<ICharacterManager>();
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
    private CActor CreateCharacter(SCharacter _chrTemp)
    {
        Cell cell;
        Sprite spr;
        GameObject prefab;
        CActor actor;

        if (_chrTemp.regularClass == ERegularClass.none) return null;
        if (!iCharacterManager.GetCharacter(_chrTemp.cType, out spr, out prefab))
            return null;
        cell = GetStartCell();
        _chrTemp.cName = _chrTemp.regularClass.ToString();
        actor = dungeon.CreateCharacter(prefab, cell).SetSprite(spr);
        actor.SetCharacter(CCharacter.Create(_chrTemp));
        return actor;
    }
    private void CreateTestCharacter()
    {
        Cell cell = CreateCharacter(iCharacterManager.CreateCharacterTemplate(EOrigin.noble, ERegularClass.knight)).GetCurrentCell();
        CreateCharacter(iCharacterManager.CreateCharacterTemplate(EOrigin.noble, ERegularClass.elementalist));
        CreateCharacter(iCharacterManager.CreateCharacterTemplate(EOrigin.undead, ERegularClass.skeleton));
        CreateCharacter(iCharacterManager.CreateCharacterTemplate(EOrigin.undead, ERegularClass.zombie));
        iCamera.SetViewPoint(cell.GetPosition());
    }
    public void ShowCharacterName(string _name)
    {
        nameChar.text = _name;
    }

    public void ShowCharacterSprite(Sprite _sprite)
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
        SAttributes a = currentCharacter.GetAttributes();
        Might.SetValue(a.might);
        Dex.SetValue(a.dexterity);
        Intel.SetValue(a.intelligence);
        Pers.SetValue(a.personality);
        Know.SetValue(a.knowledge);

        characterOrigin.SetText(iCharacterManager.GetOriginName(currentCharacter.GetOrigin()));
        characterOrigin.RefreshText();
        characterClass.SetText(iCharacterManager.GetClassName(currentCharacter.GetClass()));
        characterClass.RefreshText();
        iCamera.SetViewPoint(currentCharacter.GetPositionCell().GetPosition());
    }
    public ICharacter GetCurrentCharacter() => currentCharacter;
    public void CharCommand(int _cmd)
    {
        CharCommand((ECharacterCommand)_cmd);
    }
    public void CharCommand(ECharacterCommand _cmd)
    {
        if (currentCharacter == null) return;
        currentCharacter.AddCommand(_cmd);
    }
}
