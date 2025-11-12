using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CZombie : CActor
{
    private Animator animator;

    void Start()
    {
        InitActor();
        animator = GetComponent<Animator>();
        walkSpeed = 0.5f;
        runSpeed = 2.0f;
    }
    public override void DoCommand(ActorCommand _cmd, int _param)
    {
        switch (_cmd)
        {
            case ActorCommand.walk:
                SetState(ActorState.move);
                animator.SetBool("walk", true);
                animator.SetBool("run", false);
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
            case ActorCommand.turnback:
                SetState(ActorState.move);
                TurnBackward();
                break;
            case ActorCommand.jump:
                break;
            case ActorCommand.crouch:
                break;
            case ActorCommand.attack:
                SetState(ActorState.attack);
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                animator.SetBool("attack", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.heavyattack:
                SetState(ActorState.attack);
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                animator.SetBool("attack", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.range:
                break;
            case ActorCommand.magic:
                break;
            case ActorCommand.interact:
                SetState(ActorState.hit);
                animator.SetBool("die1", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.use:
                break;
            case ActorCommand.die:
                SetState(ActorState.die);
                animator.SetBool("die2", true);
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
        switch (state) // reset previus state flags
        {
            case ActorState.move:
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                break;
            case ActorState.attack:
                animator.SetBool("attack", false);
                break;
            case ActorState.hit:
                animator.SetBool("die1", false);
                break;
            case ActorState.die:
                animator.SetBool("die2", false);
                break;
        }
        state = _state;
    }
    public override void Idle()
    {
        SetState(ActorState.idle);
    }
}
