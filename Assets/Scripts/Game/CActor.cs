using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorState { idle, walk, run, melee, range, die }

public enum ActorCommand { wait, walk, run, turnleft, turnright, jump, crouch, melee, range, interact, use, die }

public abstract class CActor : CGameObject
{
    protected ActorState state;
    protected float walkSpeed;
    protected float runSpeed;
    protected float turnAngle;

    private class Command
    {
        public ActorCommand command;

        public Command(ActorCommand _cmd) { command = _cmd; }
    }

    private Queue<Command> cmdList = new Queue<Command>();

    private void DoCommand(Command _cmd)
    {
        switch(_cmd.command)
        {
            case ActorCommand.run:
                SetState(ActorState.run);
                break;
            case ActorCommand.walk:
                SetState(ActorState.walk);
                break;
            case ActorCommand.melee:
                SetState(ActorState.melee);
                break;
            case ActorCommand.turnleft:
                Turn(-60);
                break;
            case ActorCommand.turnright:
                Turn(60);
                break;
            case ActorCommand.die:
                SetState(ActorState.die);
                break;
            default:
                Idle();
                break;
        }
    }
    protected void InitActor()
    {
        ISaveLoad sl = AllServices.Container.Get<ISaveLoad>();
        if (sl.IsHexCell()) turnAngle = 30.0f;
        else turnAngle = 90.0f;
        InitGameObject();
        state = ActorState.idle;
        walkSpeed = 1.0f;
        runSpeed = 2.0f;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        if (positionControl.IsBusy()) return;
        if (cmdList.Count == 0) return;

        Command cmd = cmdList.Dequeue();
        DoCommand(cmd);
        if (cmd.command == ActorCommand.die) cmdList.Clear();
    }

    public void AddCommand(ActorCommand _cmd)
    {
        cmdList.Enqueue(new Command(_cmd));
    }

    public ActorState GetState() => state;
    public abstract void SetState(ActorState _state);
    public abstract void Turn(float _angle);
    public abstract void Idle();
}
