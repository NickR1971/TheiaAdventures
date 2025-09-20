using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CCell : MonoBehaviour, IPointerClickHandler
{
    private IGameConsole gameConsole = null;
    private Cell cell;
    private int num;

    public void Init(Cell _cell)
    {
        cell = _cell;
        num = cell.GetNumber();
        gameConsole = AllServices.Container.Get<IGameConsole>();
    }

    public Cell GetCell() => cell;
    public int GetNumber() => num;

    public void OnPointerClick(PointerEventData eventData)
    {
        string btn;

        switch(eventData.button)
        {
            case PointerEventData.InputButton.Left:
                btn = " [left]";
                break;
            case PointerEventData.InputButton.Right:
                btn = " [right]";
                break;
            case PointerEventData.InputButton.Middle:
                btn = " [middle]";
                break;
            default:
                btn = " [???]";
                break;
        }
        gameConsole.ShowMessage("cell " + num + btn);
    }
}
