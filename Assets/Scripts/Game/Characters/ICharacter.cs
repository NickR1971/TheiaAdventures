using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    string GetName();
    Sprite GetSprite();
    void AddCommand(ECharacterCommand _cmd);
    int GetActions(out int[] _cmd);
    void OnClickCell(int _num, int _button);
    Cell GetPositionCell();
    SAttributes GetAttributes();
    EOrigin GetOrigin();
    EActorType GetActorType();
    ERegularClass GetClass();
    EEliteClass GetEliteClass();
}
