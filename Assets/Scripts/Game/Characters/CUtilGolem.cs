using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUtilGolem : CCharacter
{
    public CUtilGolem(SCharacter _character) : base(_character)
    {
        AddActiveCommand(ECharacterCommand.move);
        AddActiveCommand(ECharacterCommand.attack);
        AddActiveCommand(ECharacterCommand.range);
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
            case ECharacterCommand.range:
                actor.AddCommand(ActorCommand.range);
                break;
        }
    }
}
