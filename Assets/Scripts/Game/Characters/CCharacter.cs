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
    protected CActor actor;

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
    }
    public void SetActor(CActor _actor) => actor = _actor;
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
    public string GetName() => character.cName;
    public Sprite GetSprite() => actor.GetSprite();
    public abstract void DoCommand(CharacterCommand _cmd);

    public void AddCommand(ActorCommand _cmd)
    {
        actor.AddCommand(_cmd);
    }

    public int GetActions(out int[] _cmd)
    {
        return actor.GetActions(out _cmd);
    }
}
