using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitSkeleton : CCharacter
{
    public CUnitSkeleton(Character _character) : base(_character)
    {
        AddActiveCommand(CharacterCommand.move);
        AddActiveCommand(CharacterCommand.attack);
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
        }
    }
}
