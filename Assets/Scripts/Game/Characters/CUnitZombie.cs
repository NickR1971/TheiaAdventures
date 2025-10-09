using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitZombie : CCharacter
{
    public CUnitZombie(Character _character) : base(_character)
    {
        AddActiveCommand(CharacterCommand.move);
        AddActiveCommand(CharacterCommand.attack);
        AddActiveCommand(CharacterCommand.interact);
        AddActiveCommand(CharacterCommand.use);
    }
    public override void DoCommand(CharacterCommand _cmd)
    {
        switch (_cmd)
        {
            case CharacterCommand.move:
                break;
            case CharacterCommand.attack:
                break;
            case CharacterCommand.interact:
                break;
            case CharacterCommand.use:
                break;
        }
    }
}
