using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEliteClass
{
    none,
    ranger, // hunter
    stalker, // hunter
    engeneer, // blacksmith
    technomage, // blacksmith
    bard, // minstrel
    inquisitor, // monk
    blackguard, // guard
    highpriest, // priest
    oracle, // priest
    archmage, // mage
    necromancer, // wizard
    demonlord, // warlock
    enchanter, // alchemist
    spiritlord, // elementalist
    weaponmaster, // duelist
    paladin, // knight
    champion, // knight
    templar, // knight
    spellsword, // warrior
    warlord, // warrior
    berserker // warrior
}

[Serializable]
public struct SAttributes
{
    public int might;
    public int dexterity;
    public int intelligence;
    public int knowledge;
    public int personality;
}
[Serializable]
public struct SSecondaryAttributes
{
    public int initiative;
    public int speed;
    public int reaction;
}
[Serializable]
public struct SCharactersPoints
{
    public int redHits;
    public int yellowHits;
    public int greenHits;
    public int blueHits;
    public int actions;
    public int mana;
    public int will;
}
[Serializable]
public struct SCharacter
{
    public string cName;
    public EOrigin origin;
    public ERegularClass regularClass;
    public EEliteClass eliteClass;
    public EActorType cType;
    public EConstitution typeConstitution;
    public SAttributes attributes;
    public SSecondaryAttributes secondaryAttributes;
    public SCharactersPoints points;
    public SCharactersPoints currentPoints;
    public SCharacter SetName(string _cName)
    {
        cName = _cName;
        return this;
    }
}
public enum ECharacterCommand
{
    wait = 0, move = 1, jump = 2, crouch = 3,
    attack = 4, range=5, magic = 6,
    special=7, interact = 8, use = 9
}

public abstract class CCharacter : ICharacter
{
    protected SCharacter character;
    protected const int maxCommands = 10;
    protected int[] activeCommandsList = new int[maxCommands];
    protected int activeCommandsNum;
    protected IDungeon dungeon;
    protected IGameMap gamemap;
    protected CActor actor;
    protected Cell selectedCell;
    protected ECharacterCommand selectedCommand;
    protected float threshold = 2.0f; // порогове значення перепаду висот для переміщення

    public CCharacter(SCharacter _character)
    {
        character = _character;
        activeCommandsNum = 0;
        selectedCell = null;
    }
    public static CCharacter Create(SCharacter _character)
    {
        CCharacter chr = null;

        switch(_character.cType)
        {
            case EActorType.knight:
                chr = new CUnitKnight(_character);
                break;
            case EActorType.mage:
                chr = new CUnitMage(_character);
                break;
            case EActorType.zombie:
                chr = new CUnitZombie(_character);
                break;
            case EActorType.skeleton:
                chr = new CUnitSkeleton(_character);
                break;
        }

        return chr;
    }
    public void SetActor(CActor _actor)
    {
        actor = _actor;
        if(actor==null)
        {
            dungeon = null;
            gamemap = null;
        }
        else
        {
            dungeon = AllServices.Container.Get<IDungeon>();
            gamemap = dungeon.GetGameMap();
        }
    }
    public void RotateTo(EMapDirection _dir)
    {
        EMapDirection sourceDir = actor.GetDirection();
        if (!CDirControl.IsValidDirection(_dir)) return;
        if (sourceDir == _dir) return;
        if (sourceDir == CDirControl.GetBack(_dir))
        {
            actor.AddCommand(ActorCommand.turnback);
            return;
        }
        int nLeft, nRight;
        EMapDirection testDir;
        nLeft = 0;
        testDir = sourceDir;
        do
        {
            nLeft++;
            testDir = CDirControl.GetLeft(testDir);
        }
        while (testDir != _dir);

        nRight = 0;
        testDir = sourceDir;
        do
        {
            nRight++;
            testDir = CDirControl.GetRight(testDir);
        }
        while (testDir != _dir);

        int n;
        ActorCommand cmd;
        if (nRight < nLeft)
        {
            n = nRight;
            cmd = ActorCommand.turnright;
        }
        else
        {
            n = nLeft;
            cmd = ActorCommand.turnleft;
        }
        while (n > 0)
        {
            actor.AddCommand(cmd);
            n--;
        }
    }
    public int GetCommandsList(out int[] _cmd)
    {
        _cmd = activeCommandsList;
        return activeCommandsNum;
    }
    protected void AddActiveCommand(ECharacterCommand _cmd)
    {
        if (activeCommandsNum < maxCommands)
            activeCommandsList[activeCommandsNum++] = (int)_cmd;
    }
    private int CheckLeft(Cell _cell, EMapDirection _dir, int _num)
    {
        int i = 0;
        while (i < 4)
        {
            i++;
            _dir = CDirControl.GetLeft(_dir);
            if (_cell.GetNearNumber(_dir) == _num) break;
        }
        if (i == 4) i = 0;
        return i;
    }
    private int CheckRight(Cell _cell, EMapDirection _dir, int _num)
    {
        int i = 0;
        while (i < 4)
        {
            i++;
            _dir = CDirControl.GetRight(_dir);
            if (_cell.GetNearNumber(_dir) == _num) break;
        }
        if (i == 4) i = 0;
        return i;
    }
    private EMapDirection Rotate(Cell _from, Cell _to, EMapDirection _startDir)
    {
        int n;
        int num = _to.GetNumber();
        Cell cell = _from;
        EMapDirection dir = _startDir;

        if (cell.GetNearNumber(dir) == num) return _startDir;
        if (cell.GetNearNumber(CDirControl.GetBack(dir)) == num)
        {
            actor.AddCommand(ActorCommand.turnback);
            return CDirControl.GetBack(_startDir);
        }
        n = CheckLeft(cell, dir, num);
        if (n > 0)
        {
            while (n > 0)
            {
                actor.AddCommand(ActorCommand.turnleft);
                n--;
                _startDir = CDirControl.GetLeft(_startDir);
            }
        }
        else
        {
            n = CheckRight(cell, dir, num);
            if (n > 0)
            {
                while (n > 0)
                {
                    actor.AddCommand(ActorCommand.turnright);
                    n--;
                    _startDir = CDirControl.GetRight(_startDir);
                }
            }
        }
        return _startDir;
    }
    protected void CreatePathTo(Cell _cell, int _distance)
    {
        _cell.SetValue(_distance++);
        if (_cell.GetNumber() == actor.GetCurrentCell().GetNumber()) return;

        Cell cell;
        EMapDirection dirStart, dir;

        dirStart = EMapDirection.east;
        dir = dirStart;
        do
        {
            cell = gamemap.GetCell(_cell.GetNearNumber(dir));
            dir = CDirControl.GetLeft(dir);
            if (cell == null) continue;
            if (cell.IsActive())
            {
                if (cell.GetValue() > _distance)
                    CreatePathTo(cell, _distance);
            }
        }
        while (dir != dirStart);
    }
    protected void MoveChar()
    {
        ActorCommand moveCmd = ActorCommand.walk;

        if (actor.GetCurrentCell().GetValue() > 2) moveCmd = ActorCommand.run;

        Cell cell1 = actor.GetCurrentCell();
        Cell cell2 = null, cell;
        EMapDirection dirStart, dir;

        dirStart = actor.GetDirection();
        do
        {
            dir = dirStart;
            do
            {
                cell = gamemap.GetCell(cell1.GetNearNumber(dir));
                if (cell2 == null) cell2 = cell;
                if (cell != null)
                {
                    if (cell.GetValue() < cell2.GetValue()) cell2 = cell;
                }
                dir = CDirControl.GetLeft(dir);
            }
            while (dir != dirStart);
            dirStart = Rotate(cell1, cell2, dirStart);
            actor.AddCommand(moveCmd);
            cell1 = cell2;
        }
        while (cell1.GetValue() > 0);
    }
    private bool CheckSurface(Cell _cell)
    {
        bool f = true;
        switch(_cell.GetBaseType())
        {
            case ECellType.none:
            case ECellType.water:
            case ECellType.lava:
                f = false;
                break;
        }
        return f;
    }
    private bool IsAccessCell(Cell _cell, float _h)
    {
        if (_cell == null) return false;
        if (_cell.GetGameObject() != null) return false;
        if (Mathf.Abs(_cell.GetHeight()-_h) > threshold) return false;
        return CheckSurface(_cell);
    }
    protected void ActivateAvailableCells(Cell _cell, int _distance)
    {
        const int cV = 500;
        _cell.SetActive(true);
        _cell.SetValue(cV - _distance);
        _distance--;

        if (_distance <= 0) return;

        Cell cell;
        EMapDirection dirStart, dir;
        int d = cV - _distance;

        dirStart = EMapDirection.east;
        dir = dirStart;
        do
        {
            cell = gamemap.GetCell(_cell.GetNearNumber(dir));
            dir = CDirControl.GetLeft(dir);
            if (IsAccessCell(cell, _cell.GetHeight()))
            {
                if (cell.GetValue() > d)
                    ActivateAvailableCells(cell, _distance);
            }
        }
        while (dir != dirStart);
    }
    protected void StandartMove()
    {
        if (selectedCell == null)
        {
            selectedCommand = ECharacterCommand.move;
            ActivateAvailableCells(actor.GetCurrentCell(), character.secondaryAttributes.speed + 1);
        }
        else
        {
            CreatePathTo(selectedCell, 0);
            MoveChar();
            selectedCell = null;
            gamemap.ActivateCells(false);
        }
    }
    public abstract void DoCommand(ECharacterCommand _cmd);

    //========== ICharacter
    public string GetName() => character.cName;
    public Sprite GetSprite() => actor.GetSprite();
    public void AddCommand(ECharacterCommand _cmd) => DoCommand(_cmd);
    public int GetActions(out int[] _cmd)
    {
        return GetCommandsList(out _cmd);
    }
    public void OnClickCell(int _num, int _button)
    {
        Cell cell = gamemap.GetCell(_num);
        switch(_button)
        {
            case 1: // left button
                selectedCell = cell;
                DoCommand(selectedCommand);
                break;
            case 2: // right button
                selectedCell = null;
                gamemap.ActivateCells(false);
                break;
            case 0: // middle button
                Rotate(actor.GetCurrentCell(), cell, actor.GetDirection());
                selectedCell = null;
                gamemap.ActivateCells(false);
                break;
        }
    }
    public Cell GetPositionCell()
    {
        return actor.GetCurrentCell();
    }
    public SAttributes GetAttributes() => character.attributes;

    public EOrigin GetOrigin() => character.origin;

    public ERegularClass GetClass() => character.regularClass;

    public EEliteClass GetEliteClass() => character.eliteClass;
    //======================
}
