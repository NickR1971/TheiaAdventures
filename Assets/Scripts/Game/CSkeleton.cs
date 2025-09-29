using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkeleton : CActor
{
    private Animator animator;

    void Start()
    {
        InitActor();
        animator = GetComponent<Animator>();
        walkSpeed = 1.0f;
        runSpeed = 2.0f;
    }
    public override void SetState(ActorState _state)
    {
        state = _state;
        switch (state)
        {
            case ActorState.walk:
                animator.SetBool("walk", true);
                if (!MoveForward(walkSpeed)) Idle();
                break;
            case ActorState.run:
                animator.SetBool("walk", true);
                animator.SetBool("run", true);
                if (!MoveForward(runSpeed)) Idle();
                break;
            case ActorState.melee:
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                animator.SetBool("attack", true);
                positionControl.Wait(1);
                break;
            case ActorState.die:
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
                animator.SetBool("die", true);
                positionControl.Wait(1);
                break;
            case ActorState.idle:
                animator.SetBool("attack", false);
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                break;
            default:
                animator.SetBool("die", false);
                break;
        }
    }

    public override void Turn(float _angle)
    {
        positionControl.Rotate(_angle);
    }

    public override void Idle()
    {
        SetState(ActorState.idle);
    }
}
