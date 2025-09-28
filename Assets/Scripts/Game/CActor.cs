using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ActorState { idle, walk, run, melee, range, die }

public enum ActorCommand { wait, walk, run, turnleft, turnright, jump, crouch, melee, range, interact, use, die }

public abstract class CActor : CGameObject, IPointerClickHandler
{
    protected IDungeon dungeon;
    protected IGameMap gameMap;
    protected Cell currentCell;
    protected ActorState state;
    protected EMapDirection dir;
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
                Turn(-turnAngle);
                dir = CDirControl.GetLeft(dir);
                break;
            case ActorCommand.turnright:
                Turn(turnAngle);
                dir = CDirControl.GetRight(dir);
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
        dungeon = AllServices.Container.Get<IDungeon>();
        gameMap = dungeon.GetGameMap();
        bool isHex = CGameManager.IsHexCell();
        if (isHex) turnAngle = 60.0f;
        else turnAngle = 45.0f;
        CDirControl.SetHex(isHex);
        InitGameObject();
        state = ActorState.idle;
        walkSpeed = 1.0f;
        runSpeed = 2.0f;
        dir = EMapDirection.east;
        positionControl.Rotate(90.0f); // turn model to east
    }
    protected bool MoveForward(float _speed)
    {
        Cell cell = gameMap.GetCell(currentCell.GetNearNumber(dir));
        if (cell == null) return false;
        positionControl.MoveTo(cell.GetPosition(), _speed);
        currentCell = cell;
        return true;
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

    public void SetCurrentCell(Cell _cell) { currentCell = _cell; }
    public void AddCommand(ActorCommand _cmd)
    {
        cmdList.Enqueue(new Command(_cmd));
    }

    public ActorState GetState() => state;
    public abstract void SetState(ActorState _state);
    public abstract void Turn(float _angle);
    public abstract void Idle();

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                AddCommand(ActorCommand.turnleft);
                break;
            case PointerEventData.InputButton.Right:
                AddCommand(ActorCommand.turnright);
                break;
            case PointerEventData.InputButton.Middle:
                AddCommand(ActorCommand.walk);
                break;
            default:
                break;
        }
    }
}
