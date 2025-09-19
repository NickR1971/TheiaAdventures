using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCell : MonoBehaviour
{
    private Cell cell;
    [SerializeField] private int num;

    public void Init(Cell _cell)
    {
        cell = _cell;
        num = cell.GetNumber();
    }

    public Cell GetCell() => cell;
    public int GetNumber() => num;
}
