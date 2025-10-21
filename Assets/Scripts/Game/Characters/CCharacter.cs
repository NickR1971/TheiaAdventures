using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int portraitIndex;
    public int numPC;
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

public abstract class CCharacter : CMovable, ICharacter
{
    protected SCharacter character;
    protected const int maxCommands = 10;
    protected int[] activeCommandsList = new int[maxCommands];
    protected int activeCommandsNum;
    private int numInParty;

    protected CCharacter(SCharacter _character)
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
            case EActorType.goblin:
                chr = new CUnitGoblin(_character);
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
    private int GetCommandsList(out int[] _cmd)
    {
        _cmd = activeCommandsList;
        return activeCommandsNum;
    }
    protected void AddActiveCommand(ECharacterCommand _cmd)
    {
        if (activeCommandsNum < maxCommands)
            activeCommandsList[activeCommandsNum++] = (int)_cmd;
    }
    protected void StandartMove()
    {
        StandartMove(character.secondaryAttributes.speed);
    }
    public abstract void DoCommand(ECharacterCommand _cmd);

    //========== ICharacter
    public string GetName() => character.cName;
    public Sprite GetSprite() => actor.GetSprite();
    public void AddCommand(ECharacterCommand _cmd) => DoCommand(_cmd);
    public int GetActions(out int[] _cmd) => GetCommandsList(out _cmd);
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
    public EActorType GetActorType() => character.cType;

    public void SetNumInParty(int _num) => numInParty = _num;

    public int GetNumInParty() => numInParty;
    //======================
}
