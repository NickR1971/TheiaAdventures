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
        runSpeed = 5.0f;
    }
    protected override void Adjust()
    {
        rend.material.color = Color.black;
    }
    public override void Idle()
    {
        SetState(ActorState.idle);
    }
    public override void DoCommand(ActorCommand _cmd, int _param)
    {
        float ftime;

        switch (_cmd)
        {
            case ActorCommand.run:
            case ActorCommand.walk:
                SetState(ActorState.move);
                animator.SetBool("leap", false);
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
                SetState(ActorState.move);
                animator.SetBool("walk", false);
                if (_param == 0)
                {
                    animator.SetBool("leap", true);
                    positionControl.Wait(0.45f);
                }
                if (_param == 1)
                {
                    animator.SetBool("leap", false);
                    JumpToTarget(runSpeed);
                }
                break;
            case ActorCommand.crouch:
                break;
            case ActorCommand.attack:
                SetState(ActorState.attack);
                animator.SetBool("attack2", false);
                animator.SetBool("attack", false);
                animator.SetBool("attack3", false);
                ftime = 0.8f;
                switch (_param)
                {
                    case 1:
                        animator.SetBool("attack", true);
                        break;
                    case 2:
                        animator.SetBool("attack2", true);
                        break;
                    case 3:
                        animator.SetBool("attack3", true);
                        break;
                    default:
                        animator.SetBool("attack", true);
                        break;
                }
                positionControl.Wait(ftime);
                break;
            case ActorCommand.interact:
                SetState(ActorState.use);
                animator.SetBool("interact", true);
                positionControl.Wait(2.0f);
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
                animator.SetBool("leap", false);
                break;
            case ActorState.attack:
                animator.SetBool("attack", false);
                animator.SetBool("attack2", false);
                animator.SetBool("attack3", false);
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
