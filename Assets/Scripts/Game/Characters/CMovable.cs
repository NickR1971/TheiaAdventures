using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/******************************************
 * Basic algorithms for moving characters
 ******************************************/
public abstract class CMovable
{
    protected IDungeon dungeon;
    protected IGameMap gamemap;
    protected CActor actor;
    protected Cell selectedCell;
    protected ECharacterCommand selectedCommand;
    protected float threshold = 2.0f; // порогове значення перепаду висот для переміщення
    protected float topHeight;
    public void SetActor(CActor _actor)
    {
        actor = _actor;
        if (actor == null)
        {
            dungeon = null;
            gamemap = null;
        }
        else
        {
            dungeon = AllServices.Container.Get<IDungeon>();
            gamemap = dungeon.GetGameMap();
        }
    }
    protected void RotateTo(EMapDirection _dir)
    {
        EMapDirection sourceDir = actor.GetDirection();
        if (!CDirControl.IsValidDirection(_dir)) return;
        if (sourceDir == _dir) return;
        if (sourceDir == CDirControl.GetBack(_dir))
        {
            actor.AddCommand(ActorCommand.turnback);
            return;
        }
        int nLeft, nRight;
        EMapDirection testDir;
        nLeft = 0;
        testDir = sourceDir;
        do
        {
            nLeft++;
            testDir = CDirControl.GetLeft(testDir);
        }
        while (testDir != _dir);

        nRight = 0;
        testDir = sourceDir;
        do
        {
            nRight++;
            testDir = CDirControl.GetRight(testDir);
        }
        while (testDir != _dir);

        int n;
        ActorCommand cmd;
        if (nRight < nLeft)
        {
            n = nRight;
            cmd = ActorCommand.turnright;
        }
        else
        {
            n = nLeft;
            cmd = ActorCommand.turnleft;
        }
        while (n > 0)
        {
            actor.AddCommand(cmd);
            n--;
        }
    }
    private int CheckLeft(Cell _cell, EMapDirection _dir, int _num)
    {
        int i = 0;
        while (i < 4)
        {
            i++;
            _dir = CDirControl.GetLeft(_dir);
            if (_cell.GetNearNumber(_dir) == _num) break;
        }
        if (i == 4) i = 0;
        return i;
    }
    private int CheckRight(Cell _cell, EMapDirection _dir, int _num)
    {
        int i = 0;
        while (i < 4)
        {
            i++;
            _dir = CDirControl.GetRight(_dir);
            if (_cell.GetNearNumber(_dir) == _num) break;
        }
        if (i == 4) i = 0;
        return i;
    }
    protected EMapDirection Rotate(Cell _from, Cell _to, EMapDirection _startDir)
    {
        int n;
        int num = _to.GetNumber();
        Cell cell = _from;
        EMapDirection dir = _startDir;

        if (cell.GetNearNumber(dir) == num) return _startDir;
        if (cell.GetNearNumber(CDirControl.GetBack(dir)) == num)
        {
            actor.AddCommand(ActorCommand.turnback);
            return CDirControl.GetBack(_startDir);
        }
        n = CheckLeft(cell, dir, num);
        if (n > 0)
        {
            while (n > 0)
            {
                actor.AddCommand(ActorCommand.turnleft);
                n--;
                _startDir = CDirControl.GetLeft(_startDir);
            }
        }
        else
        {
            n = CheckRight(cell, dir, num);
            if (n > 0)
            {
                while (n > 0)
                {
                    actor.AddCommand(ActorCommand.turnright);
                    n--;
                    _startDir = CDirControl.GetRight(_startDir);
                }
            }
        }
        return _startDir;
    }
    protected void CreatePathTo(Cell _cell, int _distance)
    {
        _cell.SetValue(_distance++);
        if (_cell.GetNumber() == actor.GetCurrentCell().GetNumber()) return;

        Cell cell;
        EMapDirection dirStart, dir;

        dirStart = EMapDirection.east;
        dir = dirStart;
        do
        {
            cell = gamemap.GetCell(_cell.GetNearNumber(dir));
            dir = CDirControl.GetLeft(dir);
            if (cell == null) continue;
            if (cell.IsActive())
            {
                if (cell.GetValue() > _distance)
                    CreatePathTo(cell, _distance);
            }
        }
        while (dir != dirStart);
    }
    protected void MoveChar()
    {
        ActorCommand moveCmd = ActorCommand.walk;

        if (actor.GetCurrentCell().GetValue() > 2) moveCmd = ActorCommand.run;

        Cell cell1 = actor.GetCurrentCell();
        Cell cell2 = null, cell;
        EMapDirection dirStart, dir;

        dirStart = actor.GetDirection();
        do
        {
            dir = dirStart;
            do
            {
                cell = gamemap.GetCell(cell1.GetNearNumber(dir));
                if (cell2 == null) cell2 = cell;
                if (cell != null)
                {
                    if (cell.GetValue() < cell2.GetValue()) cell2 = cell;
                }
                dir = CDirControl.GetLeft(dir);
            }
            while (dir != dirStart);
            dirStart = Rotate(cell1, cell2, dirStart);
            actor.AddCommand(moveCmd);
            cell1 = cell2;
        }
        while (cell1.GetValue() > 0);
    }
    private bool CheckSurface(Cell _cell)
    {
        bool f = true;
        switch (_cell.GetBaseType())
        {
            case ECellType.none:
            case ECellType.water:
            case ECellType.lava:
                f = false;
                break;
        }
        return f;
    }
    private bool IsAccessCell(Cell _cell, float _h)
    {
        if (_cell == null) return false;
        if (_cell.GetGameObject() != null) return false;
        if (Mathf.Abs(_cell.GetHeight() - _h) > threshold) return false;
        return CheckSurface(_cell);
    }
    protected void ActivateAvailableCells(Cell _cell, int _distance)
    {
        const int cV = 500;
        _cell.SetActive(true);
        _cell.SetValue(cV - _distance);
        _distance--;

        if (_distance <= 0) return;

        Cell cell;
        EMapDirection dirStart, dir;
        int d = cV - _distance;

        dirStart = EMapDirection.east;
        dir = dirStart;
        do
        {
            cell = gamemap.GetCell(_cell.GetNearNumber(dir));
            dir = CDirControl.GetLeft(dir);
            if (IsAccessCell(cell, _cell.GetHeight()))
            {
                if (cell.GetValue() > d)
                    ActivateAvailableCells(cell, _distance);
            }
        }
        while (dir != dirStart);
    }
    protected void StandartMove(int _speed)
    {
        if (selectedCell == null)
        {
            selectedCommand = ECharacterCommand.move;
            ActivateAvailableCells(actor.GetCurrentCell(), _speed + 1);
        }
        else
        {
            CreatePathTo(selectedCell, 0);
            MoveChar();
            selectedCell = null;
            gamemap.ActivateCells(false);
        }
    }
    private void ActivateCellsToSprint(Cell _cell, int _distance)
    {
        Cell cell;
        EMapDirection dirStart, dir;
        int i;

        topHeight = _cell.GetHeight() + 0.5f;
        dirStart = EMapDirection.east;
        dir = dirStart;
        do
        {
            cell = _cell;
            for (i = 0; i < _distance; i++)
            {
                cell = gamemap.GetCell(cell.GetNearNumber(dir));
                if (cell == null) break;
                if (cell.GetHeight() > topHeight) break;
                if (CheckSurface(cell) && cell.GetGameObject() == null)
                {
                    cell.SetActive(true);
                    cell.SetValue((int)dir);
                }
                else break;
            }
            dir = CDirControl.GetLeft(dir);
        }
        while (dir != dirStart);
    }
    private void ActivateCellsToTeleport(Cell _cell, int _distance)
    {
        float distance = (float)_distance + 0.2f;
        int maxCells = gamemap.GetHeight() * gamemap.GetWidth();
        int i;
        Cell cell;
        
        for (i = 0; i < maxCells; i++)
        {
            cell = gamemap.GetCell(i);
            if (cell == null) continue;
            if (cell.IsHidden()) continue;
            if (CheckSurface(cell) && cell.GetGameObject() == null)
            {
                if (Vector3.Distance(_cell.GetPosition(), cell.GetPosition()) < distance)
                    cell.SetActive(true);
            }
        }
    }
    private void ActivateCellsToJump(Cell _cell, int _distance, float _roof)
    {
        Cell cell;
        EMapDirection dirStart, dir;
        int i;

        topHeight = _cell.GetHeight() + ((float)_distance);
        if (topHeight > _roof) topHeight = _roof;
        dirStart = EMapDirection.east;
        dir = dirStart;
        do
        {
            cell = _cell;
            for (i = 0; i < _distance; i++)
            {
                cell = gamemap.GetCell(cell.GetNearNumber(dir));
                if (cell == null) break;
                if (cell.GetHeight() > topHeight) break;
                if (CheckSurface(cell) && cell.GetGameObject() == null)
                {
                    cell.SetActive(true);
                    cell.SetValue((int)dir);
                }
            }
            dir = CDirControl.GetLeft(dir);
        }
        while (dir != dirStart);
    }
    private void JumpTo(Cell _cell)
    {
        actor.SetTarget(_cell);
        actor.SetTopJump(topHeight);
        actor.AddCommand(ActorCommand.jump);
    }
    protected void StandartJump(int _distance)
    {
        const float roof = 7.0f; // стандартне обмеження висоти у підземеллях
        if (selectedCell == null)
        {
            selectedCommand = ECharacterCommand.jump;
            ActivateCellsToJump(actor.GetCurrentCell(), _distance, roof);
        }
        else
        {
            RotateTo((EMapDirection)selectedCell.GetValue());
            JumpTo(selectedCell);
            selectedCell = null;
            gamemap.ActivateCells(false);
        }
    }
    protected void StandartTeleport(int _distance)
    {
        if (selectedCell == null)
        {
            selectedCommand = ECharacterCommand.jump;
            ActivateCellsToTeleport(actor.GetCurrentCell(), _distance);
        }
        else
        {
            actor.SetTarget(selectedCell);
            actor.AddCommand(ActorCommand.jump);
            actor.AddCommand(ActorCommand.jump, 1);
            selectedCell = null;
            gamemap.ActivateCells(false);
        }
    }
    private void SprintTo(Cell _cell)
    {
        EMapDirection dir = (EMapDirection)selectedCell.GetValue();
        Cell cell = actor.GetCurrentCell();
        do
        {
            cell = gamemap.GetCell(cell.GetNearNumber(dir));
            if (!(CheckSurface(cell) && cell.GetGameObject() == null)) break;
            actor.AddCommand(ActorCommand.jump);
        } while (cell != _cell);

        if (cell != _cell) Debug.Log("Sprint failed!");
    }
    protected void StandartSprint(int _speed)
    {
        if (selectedCell == null)
        {
            selectedCommand = ECharacterCommand.jump;
            ActivateCellsToSprint(actor.GetCurrentCell(), _speed);
        }
        else
        {
            RotateTo((EMapDirection)selectedCell.GetValue());
            SprintTo(selectedCell);
            selectedCell = null;
            gamemap.ActivateCells(false);
        }
    }
}
