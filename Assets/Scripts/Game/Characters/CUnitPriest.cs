using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitPriest : CCharacter
{
    public CUnitPriest(SCharacter _character) : base(_character)
    {
        AddActiveCommand(ECharacterCommand.move);
        AddActiveCommand(ECharacterCommand.attack);
        AddActiveCommand(ECharacterCommand.magic);
        AddActiveCommand(ECharacterCommand.interact);
        AddActiveCommand(ECharacterCommand.use);
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
            case ECharacterCommand.magic:
                actor.AddCommand(ActorCommand.magic);
                break;
            case ECharacterCommand.interact:
                actor.AddCommand(ActorCommand.interact);
                break;
            case ECharacterCommand.use:
                actor.AddCommand(ActorCommand.use);
                break;
        }
    }
}
