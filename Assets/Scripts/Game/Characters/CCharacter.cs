using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CAttributes
{
    int might;
    int dexterity;
    int intelligence;
    int willPower;
    int knowledge;
}

public struct CSecondaryAttributes
{
    int initiative;
    int speed;
    int reaction;
}

public struct CCharactersPoints
{
    int redHits;
    int yellowHits;
    int greenHits;
    int blueHits;
    int actions;
    int mana;
    int will;
}

public class CCharacter
{
    protected CAttributes attributes;
    protected CSecondaryAttributes secondaryAttributes;
    protected CCharactersPoints points;
}
