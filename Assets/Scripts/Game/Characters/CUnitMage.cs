using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitMage : CCharacter
{
    public CUnitMage(Character _character) : base(_character)
    {
        AddActiveCommand(CharacterCommand.move);
        AddActiveCommand(CharacterCommand.attack);
        AddActiveCommand(CharacterCommand.magic);
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
            case CharacterCommand.magic:
                break;
            case CharacterCommand.interact:
                break;
            case CharacterCommand.use:
                break;
        }
    }
}
