using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    string GetName();
    Sprite GetSprite();
    void AddCommand(CharacterCommand _cmd);
    int GetActions(out int[] _cmd);
    void OnClickCell(int _num, int _button);
}
