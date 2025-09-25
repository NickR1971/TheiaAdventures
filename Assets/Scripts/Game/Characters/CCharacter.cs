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

public class CCharacter
{
    protected string chrName;
    protected ECharacterType chrType;
    protected int cellNum;
    protected EMapDirection dir;
    protected CAttributes attributes;
    protected CSecondaryAttributes secondaryAttributes;
    protected CCharactersPoints points;

    public CCharacter()
    {
        chrName = "Hero";
        dir = EMapDirection.east;
        chrType = ECharacterType.hero;
        attributes.might = 3;
        attributes.dexterity = 3;
        attributes.intelligence = 3;
        attributes.knowledge = 3;
        attributes.personality = 3;
    }
}
