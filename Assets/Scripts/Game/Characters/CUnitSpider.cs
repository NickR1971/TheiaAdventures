using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitSpider : CCharacter
{
    public CUnitSpider(SCharacter _character) : base(_character)
    {
        AddActiveCommand(ECharacterCommand.move);
        AddActiveCommand(ECharacterCommand.jump);
        AddActiveCommand(ECharacterCommand.attack);
        AddActiveCommand(ECharacterCommand.range);
        AddActiveCommand(ECharacterCommand.special);
        AddActiveCommand(ECharacterCommand.interact);
    }
    public override void DoCommand(ECharacterCommand _cmd)
    {
        switch (_cmd)
        {
            case ECharacterCommand.move:
                StandartMove();
                break;
            case ECharacterCommand.jump:
                StandartJump();
                break;
            case ECharacterCommand.attack:
                actor.AddCommand(ActorCommand.melee);
                break;
            case ECharacterCommand.range:
                actor.AddCommand(ActorCommand.range);
                break;
            case ECharacterCommand.special:
                actor.AddCommand(ActorCommand.heavyattack);
                break;
            case ECharacterCommand.interact:
                actor.AddCommand(ActorCommand.interact);
                break;
        }
    }
}
