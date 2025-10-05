using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorState { idle, walk, run, melee, range, die }

public enum ActorCommand { wait=0, walk=1, run=2, turnleft=3, turnright=4, jump=5, crouch=6,
    melee=7, heavyattack=8, range=9, magic=10, interact=11, use=12, die=13 }

public abstract class CActor : CGameObject, ICharacter
{
    protected IBattle battle;
    protected IDungeon dungeon;
    protected IGameMap gameMap;
    protected Cell currentCell;
    protected ActorState state;
    protected ActorCommand lastCommand;
    protected EMapDirection dir;
    protected float walkSpeed;
    protected float runSpeed;
    protected float turnAngle;
    protected Sprite charSprite;
    protected string charName;

    private class Command
    {
        public ActorCommand command;
        public Command(ActorCommand _cmd) { command = _cmd; }
    }

    private Queue<Command> cmdList = new Queue<Command>();

    private void DoCommand(Command _cmd)
    {
        lastCommand = _cmd.command;

        switch(_cmd.command)
        {
            case ActorCommand.run:
                SetState(ActorState.run);
                break;
            case ActorCommand.walk:
                SetState(ActorState.walk);
                break;
            case ActorCommand.turnleft:
                TurnLeft();
                break;
            case ActorCommand.turnright:
                TurnRight();
                break;
            case ActorCommand.jump:
                break;
            case ActorCommand.crouch:
                break;
            case ActorCommand.melee:
                SetState(ActorState.melee);
                break;
            case ActorCommand.heavyattack:
                SetState(ActorState.melee);
                break;
            case ActorCommand.range:
                break;
            case ActorCommand.magic:
                break;
            case ActorCommand.interact:
                break;
            case ActorCommand.use:
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
        battle = AllServices.Container.Get<IBattle>();
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
    protected void TurnLeft()
    {
        positionControl.Rotate(-turnAngle);
        dir = CDirControl.GetLeft(dir);
    }
    protected void TurnRight()
    {
        positionControl.Rotate(turnAngle);
        dir = CDirControl.GetRight(dir);
    }
    protected override void DoUpdate()
    {
        base.DoUpdate();

        if (positionControl.IsBusy()) return;
        if (cmdList.Count == 0)
        {
            Idle();
            return;
        }

        Command cmd = cmdList.Dequeue();
        DoCommand(cmd);
        if (cmd.command == ActorCommand.die) cmdList.Clear();
    }

    public CActor SetCurrentCell(Cell _cell) { currentCell = _cell; return this; }
    public Cell GetCurrentCell() { return currentCell; }
    public CActor SetName(string _name) { charName = _name; return this; }
    public string GetName() { return charName; }
    public CActor SetSprite(Sprite _spr) { charSprite = _spr; return this; }
    public Sprite GetSprite() { return charSprite; }
    public void AddCommand(ActorCommand _cmd)
    {
        cmdList.Enqueue(new Command(_cmd));
    }
    public ActorState GetState() => state;
    public abstract int GetActions(out int[] _cmd);
    public abstract void SetState(ActorState _state);
    public abstract void Idle();
    protected override void OnLeftClick()
    {
        AddCommand(ActorCommand.melee);
    }
    protected override void OnRightClick()
    {
        Idle();
    }
    protected override void OnMiddleClick()
    {
        battle.SetCurrentCharacter(this);
    }

}
