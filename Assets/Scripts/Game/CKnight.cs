using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKnight : CActor
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
            case ActorCommand.run:
                SetState(ActorState.move);
                animator.SetBool("walk", true);
                animator.SetBool("run", true);
                if (!MoveForward(runSpeed)) Idle();
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
                break;
            case ActorCommand.magic:
                break;
            case ActorCommand.interact:
                break;
            case ActorCommand.use:
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
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                break;
            case ActorState.melee:
                animator.SetBool("attack", false);
                animator.SetBool("attack2", false);
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
        _cmd = new int[5];
        _cmd[0] = 1;
        _cmd[1] = 3;
        _cmd[2] = 4;
        _cmd[3] = 7;
        _cmd[4] = 8;
        return 5;
    }
}
