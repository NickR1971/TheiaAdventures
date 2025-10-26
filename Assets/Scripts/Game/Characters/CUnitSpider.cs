using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitSpider : CCharacter
{
    public CUnitSpider(SCharacter _character) : base(_character)
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
                actor.AddCommand(ActorCommand.melee);
                break;
            case ECharacterCommand.interact:
                actor.AddCommand(ActorCommand.interact);
                break;
        }
    }
}
