using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    string GetName();
    Sprite GetSprite();
    void AddCommand(ActorCommand _cmd);
}
