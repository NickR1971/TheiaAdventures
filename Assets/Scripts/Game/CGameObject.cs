using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CGameObject : MonoBehaviour, IPointerClickHandler
{
    protected CPositionControl positionControl;
    protected Cell currentCell = null;

    void Update()
    {
        DoUpdate();
    }

    protected virtual void DoUpdate()
    {
        positionControl.Update();
    }
    protected void InitGameObject()
    {
        positionControl = new CPositionControl(transform);
    }
    protected abstract void OnLeftClick();
    protected abstract void OnRightClick();
    protected abstract void OnMiddleClick();
    protected abstract void CheckUnhide(CRoom _room);
    public CGameObject SetCurrentCell(Cell _cell)
    {
        if (currentCell != null) currentCell.ResetGameObject();
        currentCell = _cell;
        currentCell.SetGameObject(this);
        CRoom room = currentCell.GetRoom();
        if (room.IsHidden())
        {
            CheckUnhide(room);
        }
        return this;
    }
    public Cell GetCurrentCell() => currentCell;
    public Vector3 GetPosition() => transform.position;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                OnLeftClick();
                break;
            case PointerEventData.InputButton.Right:
                OnRightClick();
                break;
            case PointerEventData.InputButton.Middle:
                OnMiddleClick();
                break;
            default:
                break;
        }
    }
}
