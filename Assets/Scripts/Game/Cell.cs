using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private readonly Vector3 position;
    private readonly int number;
    private GameObject obj;
    private ECellType baseType;
    private int[] nearList;
    private CRoom room;
    private CGameObject curGameObject;
    private const int maxValue = 999;
    private int tempValue;

    public Cell(Vector3 _position, int _number, int[] _nearList)
    {
        position = _position;
        number = _number;
        baseType = ECellType.none;
        nearList = _nearList;
        curGameObject = null;
        tempValue = maxValue;
    }
    public void SetGameObject(CGameObject _obj)
    {
        curGameObject = _obj;
    }
    public CGameObject GetGameObject() => curGameObject;
    public void SetBaseType(ECellType _type) => baseType = _type;
    public ECellType GetBaseType() => baseType;
    public Vector3 GetPosition() => position;
    public float GetHeight() => position.y;
    public int GetNearNumber(EMapDirection _direction) => nearList[(int)_direction];
    public void AddRoom(CRoom _room) => room = _room;
    public CRoom GetRoom() => room;
    public int GetNumber() => number;

    public void SetObject(GameObject _obj)
    {
        obj = _obj;
        obj.transform.position = position;
        obj.GetComponent<CCell>().Init(this);
    }

    public void SetColor(Color _color)
    {
        if (obj == null) Debug.Log($"Cell #{number} mark not instantiated!");
        else obj.GetComponent<Renderer>().material.color = _color;
        
    }

    public void SetActive(bool _f) => obj.SetActive(_f);
    public bool IsActive() => obj.activeSelf;
    public void SetValue(int _v) => tempValue = _v;
    public void ResetValue() => tempValue = maxValue;
    public int GetValue() => tempValue;
}

