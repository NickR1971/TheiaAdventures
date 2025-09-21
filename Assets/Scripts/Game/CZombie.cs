﻿using System.Collections;
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

    public override void SetState(ActorState _state)
    {
        state = _state;
        switch (state)
        {
            case ActorState.walk:
                animator.SetBool("walk", true);
                positionControl.MoveForward(walkSpeed);
                break;
            case ActorState.run:
                animator.SetBool("walk", true);
                animator.SetBool("run", true);
                positionControl.MoveForward(runSpeed);
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
                animator.SetBool("die2",true);
                positionControl.Wait(1);
                break;
            case ActorState.idle:
                animator.SetBool("attack", false);
                animator.SetBool("run", false);
                animator.SetBool("walk", false);
                break;
            default:
                animator.SetBool("die1",false);
                animator.SetBool("die2",false);
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
