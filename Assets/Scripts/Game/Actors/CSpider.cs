using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpider : CActor
{
    [SerializeField] private SkinnedMeshRenderer rend;
    private Animator animator;
    void Start()
    {
        initRotation = -90.0f;
        InitActor();
        animator = GetComponent<Animator>();
        walkSpeed = 3.0f;
        runSpeed = 3.0f;
    }
    protected override void Adjust()
    {
        rend.material.color = Color.black;
    }
    public override void Idle()
    {
        SetState(ActorState.idle);
    }
    public override void DoCommand(ActorCommand _cmd)
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
            case ActorCommand.jump:
                break;
            case ActorCommand.crouch:
                break;
            case ActorCommand.melee:
                SetState(ActorState.melee);
                animator.SetBool("attack2", true);
                animator.SetBool("attack", false);
                positionControl.Wait(1);
                break;
            case ActorCommand.heavyattack:
                SetState(ActorState.melee);
                animator.SetBool("attack2", false);
                animator.SetBool("attack", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.interact:
                SetState(ActorState.use);
                animator.SetBool("interact", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.hit:
                SetState(ActorState.hit);
                animator.SetBool("gethit", true);
                positionControl.Wait(1);
                break;
            case ActorCommand.die:
                SetState(ActorState.die);
                animator.SetBool("gethit", true);
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
            case ActorState.use:
                animator.SetBool("interact", false);
                break;
            case ActorState.hit:
                if (_state != ActorState.die) animator.SetBool("gethit", false);
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
