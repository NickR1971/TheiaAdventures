using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitZombie : CCharacter
{
    public CUnitZombie(Character _character) : base(_character)
    {
        AddActiveCommand(CharacterCommand.move);
        AddActiveCommand(CharacterCommand.attack);
        AddActiveCommand(CharacterCommand.interact);
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
                actor.AddCommand(ActorCommand.melee);
                break;
        }
    }
}
