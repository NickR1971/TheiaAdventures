using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGolem : CActor
{
    [SerializeField] private SkinnedMeshRenderer rend;
    private Animator animator;
    void Start()
    {
        InitActor();
        animator = GetComponent<Animator>();
        walkSpeed = 1.0f;
        runSpeed = 2.0f;
    }
    protected override void Adjust()
    {
        //rend.material.color = Color.red;
    }
    public override void Idle()
    {
        SetState(ActorState.idle);
    }
    public override void DoCommand(ActorCommand _cmd, int param)
    {
        switch (_cmd)
        {
            case ActorCommand.run:
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
            case ActorCommand.turnback:
                SetState(ActorState.move);
                TurnBackward();
                break;
            case ActorCommand.attack:
                SetState(ActorState.attack);
                animator.SetBool("attack", true);
                animator.SetBool("range", false);
                positionControl.Wait(2.0f);
                break;
            case ActorCommand.range:
                SetState(ActorState.attack);
                animator.SetBool("attack", false);
                animator.SetBool("range", true);
                positionControl.Wait(2.667f);
                break;
            case ActorCommand.hit:
                SetState(ActorState.hit);
                animator.SetBool("hit", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.die:
                SetState(ActorState.die);
                animator.SetBool("hit", true);
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
            case ActorState.attack:
                animator.SetBool("attack", false);
                animator.SetBool("range", false);
                break;
            case ActorState.hit:
                if (_state != ActorState.die) animator.SetBool("hit", false);
                break;
            case ActorState.die:
                animator.SetBool("gethit", false);
                animator.SetBool("die", false);
                break;
            default:
                break;
        }
        state = _state;
    }
}
