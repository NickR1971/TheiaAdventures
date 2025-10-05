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
    public override int GetActions(out int[] _cmd)
    {
        _cmd = new int[4];
        _cmd[0] = 1;
        _cmd[1] = 3;
        _cmd[2] = 4;
        _cmd[3] = 7;
        return 4;
    }
}
