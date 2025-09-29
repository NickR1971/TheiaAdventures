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
    public override void Turn(float _angle)
    {
        positionControl.Rotate(_angle);
    }

    public override void Idle()
    {
        SetState(ActorState.idle);
    }
    public override void SetState(ActorState _state)
    {
        state = _state;
        switch (state)
        {
            case ActorState.walk:
                if (!MoveForward(walkSpeed)) Idle();
                break;
            case ActorState.run:
                if (!MoveForward(runSpeed)) Idle();
                break;
            case ActorState.melee:
                break;
            case ActorState.die:
                break;
            case ActorState.idle:
                break;
            default:
                break;
        }
    }
}
