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
        gameConsole.Show();
        switch(eventData.button)
        {
            case PointerEventData.InputButton.Left:
                cell.SetColor(Color.blue);
                break;
            case PointerEventData.InputButton.Right:
                gameConsole.ShowMessage("check cell " + num);
                break;
            case PointerEventData.InputButton.Middle:
                gameConsole.ExecuteCommand("cell " + num);
                break;
            default:
                gameConsole.ShowMessage("unknown event at cell " + num);
                break;
        }
    }
}
