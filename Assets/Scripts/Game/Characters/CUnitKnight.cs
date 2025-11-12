using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitKnight : CCharacter
{
    public CUnitKnight(SCharacter _character) : base(_character)
    {
        AddActiveCommand(ECharacterCommand.move);
        AddActiveCommand(ECharacterCommand.jump);
        AddActiveCommand(ECharacterCommand.attack);
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
            case ECharacterCommand.jump:
                StandartSprint(character.secondaryAttributes.speed * 2);
                break;
            case ECharacterCommand.attack:
                actor.AddCommand(ActorCommand.attack);
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
