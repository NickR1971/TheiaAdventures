using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitSkeleton : CCharacter
{
    public CUnitSkeleton(SCharacter _character) : base(_character)
    {
        AddActiveCommand(ECharacterCommand.move);
        AddActiveCommand(ECharacterCommand.attack);
    }
    public override void DoCommand(ECharacterCommand _cmd)
    {
        switch (_cmd)
        {
            case ECharacterCommand.move:
                StandartMove();
                break;
            case ECharacterCommand.attack:
                actor.AddCommand(ActorCommand.melee);
                break;
        }
    }
}
