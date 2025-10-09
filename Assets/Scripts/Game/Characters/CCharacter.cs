using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CAttributes
{
    public int might;
    public int dexterity;
    public int intelligence;
    public int knowledge;
    public int personality;
}

public struct CSecondaryAttributes
{
    public int initiative;
    public int speed;
    public int reaction;
}

public struct CCharactersPoints
{
    public int redHits;
    public int yellowHits;
    public int greenHits;
    public int blueHits;
    public int actions;
    public int mana;
    public int will;
}
public struct Character
{
    public string cName;
    public ECharacterType cType;
    public CAttributes attributes;
    public CSecondaryAttributes secondaryAttributes;
    public CCharactersPoints points;
    public CCharactersPoints currentPoints;
}
public enum CharacterCommand
{
    wait = 0, move = 1, jump = 2, crouch = 3,
    attack = 4, range=5, magic = 6,
    special=7, interact = 8, use = 9
}

public abstract class CCharacter : ICharacter
{
    protected Character character;
    protected const int maxCommands = 10;
    protected int[] activeCommandsList = new int[maxCommands];
    protected int activeCommandsNum;
    protected IDungeon dungeon;
    protected IGameMap gamemap;
    protected CActor actor;
    protected Cell selectedCell;
    protected CharacterCommand selectedCommand;

    public static CCharacter Create(Character _character)
    {
        CCharacter chr = null;

        switch(_character.cType)
        {
            case ECharacterType.knight:
                chr = new CUnitKnight(_character);
                break;
            case ECharacterType.mage:
                chr = new CUnitMage(_character);
                break;
            case ECharacterType.zombie:
                chr = new CUnitZombie(_character);
                break;
            case ECharacterType.skeleton:
                chr = new CUnitSkeleton(_character);
                break;
        }

        return chr;
    }
    public CCharacter(Character _character)
    {
        character = _character;
        activeCommandsNum = 0;
        selectedCell = null;
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

    protected void AddActiveCommand(CharacterCommand _cmd)
    {
        if (activeCommandsNum < maxCommands)
            activeCommandsList[activeCommandsNum++] = (int)_cmd;
    }
    public int GetCommandsList(out int[] _cmd)
    {
        _cmd = activeCommandsList;
        return activeCommandsNum;
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
    protected void RotateToSelectedCell()
    {
        int n;
        int num = selectedCell.GetNumber();
        Cell cell = actor.GetCurrentCell();
        EMapDirection dir = actor.GetDirection();

        if (cell.GetNearNumber(dir) == num) return;
        if (cell.GetNearNumber(CDirControl.GetBack(dir)) == num)
        {
            actor.AddCommand(ActorCommand.turnback);
            return;
        }
        n = CheckLeft(cell, dir, num);
        if (n > 0)
        {
            while (n > 0)
            {
                actor.AddCommand(ActorCommand.turnleft);
                n--;
            }
            return;
        }
        n = CheckRight(cell, dir, num);
        if (n > 0)
        {
            while (n > 0)
            {
                actor.AddCommand(ActorCommand.turnright);
                n--;
            }
            return;
        }
    }
    private bool IsAccessCell(Cell _cell)
    {
        if (_cell == null) return false;
        if (_cell.GetGameObject() != null) return false;
        if (_cell.GetHeight() > 3.0f) return false;
        return true;
    }
    protected void ActivateNearCells()
    {
        EMapDirection dirForward, dirBackward, dirLeft, dirRight;
        int i;
        int[] left = new int[9];
        int[] right = new int[9];
        Cell cell = actor.GetCurrentCell();

        for (i = 0; i < 9; i++) left[i] = right[i] = 0;
        dirForward = actor.GetDirection();
        i = 0; dirLeft = dirRight = dirForward;
        dirBackward = CDirControl.GetBack(dirForward);
        do
        {
            left[i] = cell.GetNearNumber(dirLeft);
            right[i] = cell.GetNearNumber(dirRight);

            if (!IsAccessCell(gamemap.GetCell(left[i]))) left[i] = 0;
            if (!IsAccessCell(gamemap.GetCell(right[i]))) right[i] = 0;
            i++;
            dirLeft = CDirControl.GetLeft(dirLeft);
            dirRight = CDirControl.GetRight(dirRight);
        }
        while (dirLeft != dirBackward);
        
        left[i] = right[i] = cell.GetNearNumber(dirBackward);
        if (gamemap.GetCell(left[i]) == null) left[i] = right[i] = 0;
        else if (gamemap.GetCell(left[i]).GetHeight() > 3.0f) left[i] = right[i] = 0;

        for (i = 0; i < 9; i++)
        {
            if (left[i] != 0) gamemap.GetCell(left[i]).SetActive(true);
            if (right[i] != 0) gamemap.GetCell(right[i]).SetActive(true);
        }
    }
    public abstract void DoCommand(CharacterCommand _cmd);

    //========== ICharacter
    public string GetName() => character.cName;
    public Sprite GetSprite() => actor.GetSprite();
    public void AddCommand(CharacterCommand _cmd) => DoCommand(_cmd);
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
                selectedCell = cell;
                RotateToSelectedCell();
                selectedCell = null;
                gamemap.ActivateCells(false);
                break;
        }
    }
    //======================
}
