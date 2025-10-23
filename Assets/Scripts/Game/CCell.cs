using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CCell : MonoBehaviour, IPointerClickHandler
{
    private IBattle battle = null;
    private Cell cell;
    private int num;
    private Renderer rend;
    private Color baseColor;
    private static CCell lastCell = null;

    public void Init(Cell _cell)
    {
        cell = _cell;
        num = cell.GetNumber();
        battle = AllServices.Container.Get<IBattle>();
        rend = GetComponent<Renderer>();
        transform.position = cell.GetPosition();
    }
    public Cell GetCell() => cell;
    public int GetNumber() => num;
    public void ChangeColor(Color _color) => rend.material.color = _color;
    public void RestoreColor() => rend.material.color = baseColor;
    public void SetColor(Color _color)
    {
        baseColor = _color;
        RestoreColor();
    }
    public bool IsActive() => gameObject.activeSelf;
    public void SetActive(bool _f) => gameObject.SetActive(_f);

    ///////////////////////////////////////////////
    // OnMouse standart events
    // OnMouseEnter(), OnMouseOver(), OnMouseExit()
    void OnMouseEnter()
    {
        if (lastCell != null) lastCell.RestoreColor();
        ChangeColor(Color.red);
        lastCell = this;
    }
    ///////////////////////////////////////////////
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
