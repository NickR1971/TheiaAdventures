using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitZombie : CCharacter
{
    public CUnitZombie(SCharacter _character) : base(_character)
    {
        AddActiveCommand(ECharacterCommand.move);
        AddActiveCommand(ECharacterCommand.attack);
        AddActiveCommand(ECharacterCommand.interact);
    }
    public override void DoCommand(ECharacterCommand _cmd)
    {
        switch (_cmd)
        {
            case ECharacterCommand.move:
                StandartMove();
                break;
            case ECharacterCommand.attack:
                actor.AddCommand(ActorCommand.attack);
                break;
            case ECharacterCommand.interact:
                actor.AddCommand(ActorCommand.attack);
                break;
        }
    }
}
