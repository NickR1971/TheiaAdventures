using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitKnight : CCharacter
{
    public CUnitKnight(Character _character) : base(_character)
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
                StandartMove();
                break;
            case CharacterCommand.attack:
                actor.AddCommand(ActorCommand.melee);
                break;
            case CharacterCommand.interact:
                actor.AddCommand(ActorCommand.interact);
                break;
            case CharacterCommand.use:
                actor.AddCommand(ActorCommand.use);
                break;
        }
    }
}
