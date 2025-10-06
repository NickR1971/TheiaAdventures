using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWizard : CActor
{
    private Animator animator;
    void Start()
    {
        InitActor();
        animator = GetComponent<Animator>();
        walkSpeed = 1.0f;
        runSpeed = 2.0f;
    }
    public override void Idle()
    {
        SetState(ActorState.idle);
    }
    public override void DoCommand(ActorCommand _cmd)
    {
        switch (_cmd)
        {
            case ActorCommand.walk:
                SetState(ActorState.move);
                animator.SetBool("walk", true);
                if (!MoveForward(walkSpeed)) Idle();
                break;
            case ActorCommand.turnleft:
                SetState(ActorState.move);
                TurnLeft();
                break;
            case ActorCommand.turnright:
                SetState(ActorState.move);
                TurnRight();
                break;
            case ActorCommand.jump:
                break;
            case ActorCommand.crouch:
                break;
            case ActorCommand.melee:
                SetState(ActorState.melee);
                animator.SetBool("attack", true);
                animator.SetBool("attack2", false);
                positionControl.Wait(1);
                break;
            case ActorCommand.heavyattack:
                SetState(ActorState.melee);
                animator.SetBool("attack", false);
                animator.SetBool("attack2", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.range:
                SetState(ActorState.range);
                animator.SetBool("range", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.magic:
                SetState(ActorState.magic);
                animator.SetBool("magic", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.interact:
                break;
            case ActorCommand.use:
                SetState(ActorState.use);
                animator.SetBool("use", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.die:
                SetState(ActorState.die);
                animator.SetBool("die", true);
                positionControl.Wait(1);
                break;
            default:
                Idle();
                break;
        }
    }
    private void SetState(ActorState _state)
    {
        if (state == _state) return;
        switch (state)
        {
            case ActorState.move:
                animator.SetBool("walk", false);
                break;
            case ActorState.melee:
                animator.SetBool("attack", false);
                animator.SetBool("attack2", false);
                break;
            case ActorState.magic:
                animator.SetBool("magic", false);
                break;
            case ActorState.range:
                animator.SetBool("range", false);
                break;
            case ActorState.use:
                animator.SetBool("use", false);
                break;
            case ActorState.die:
                animator.SetBool("die", false);
                break;
            default:
                break;
        }
        state = _state;
    }
    public override int GetActions(out int[] _cmd)
    {
        if (activeCommandsNum == 0)
        {
            AddActiveCommand(ActorCommand.walk);
            AddActiveCommand(ActorCommand.turnleft);
            AddActiveCommand(ActorCommand.turnright);
            AddActiveCommand(ActorCommand.melee);
            AddActiveCommand(ActorCommand.heavyattack);
            AddActiveCommand(ActorCommand.range);
            AddActiveCommand(ActorCommand.magic);
            AddActiveCommand(ActorCommand.use);
            AddActiveCommand(ActorCommand.die);
        }
        _cmd = activeCommandsList;
        return activeCommandsNum;
    }
}
