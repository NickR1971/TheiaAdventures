using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CCell : MonoBehaviour, IPointerClickHandler
{
    private IBattle battle = null;
    private Cell cell;
    private int num;

    public void Init(Cell _cell)
    {
        cell = _cell;
        num = cell.GetNumber();
        battle = AllServices.Container.Get<IBattle>();
    }

    public Cell GetCell() => cell;
    public int GetNumber() => num;

    public void OnPointerClick(PointerEventData eventData)
    {
        ICharacter curChar = battle.GetCurrentCharacter();
        
        if (curChar == null) return;

        switch(eventData.button)
        {
            case PointerEventData.InputButton.Left:
                curChar.OnClickCell(num, 1);
                break;
            case PointerEventData.InputButton.Right:
                curChar.OnClickCell(num, 2);
                break;
            case PointerEventData.InputButton.Middle:
                curChar.OnClickCell(num, 0);
                break;
        }
    }
}
