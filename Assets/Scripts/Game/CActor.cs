using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorState { idle, move, melee, range, magic, use, hit, die }

public enum ActorCommand { wait, walk, run, turnleft, turnright, turnback, jump, crouch,
    melee, heavyattack, range, magic, interact, use, die }

public abstract class CActor : CGameObject
{
    protected IBattle battle;
    protected IDungeon dungeon;
    protected IGameMap gameMap;
    protected ActorState state;
    protected EMapDirection dir;
    protected float walkSpeed;
    protected float runSpeed;
    protected float turnAngle;
    protected Sprite charSprite;
    protected CCharacter character;

    private class Command
    {
        public ActorCommand command;
        public Command(ActorCommand _cmd) { command = _cmd; }
    }

    private Queue<Command> cmdList = new Queue<Command>();

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
        SetCurrentCell(cell);
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
    protected void TurnBackward()
    {
        positionControl.Rotate(180.0f);
        dir = CDirControl.GetBack(dir);
    }
    protected override void DoUpdate()
    {
        base.DoUpdate();

        if (positionControl.IsBusy()) return;
        if (cmdList.Count == 0)
        {
            if(state!= ActorState.die ) Idle();
            return;
        }

        Command cmd = cmdList.Dequeue();
        DoCommand(cmd.command);
    }
    public void SetCharacter(CCharacter _character)
    {
        character = _character;
        character.SetActor(this);
    }
    public CCharacter GetGaracter() => character;
    public EMapDirection GetDirection() => dir;
    public string GetName() => character.GetName();
    public CActor SetSprite(Sprite _spr) { charSprite = _spr; return this; }
    public Sprite GetSprite() => charSprite;
    public void AddCommand(ActorCommand _cmd) => cmdList.Enqueue(new Command(_cmd));
    public ActorState GetState() => state;
    public abstract void DoCommand(ActorCommand _cmd);
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
        battle.SetCurrentCharacter(character);
    }
}
