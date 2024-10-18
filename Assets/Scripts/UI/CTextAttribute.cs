using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTextAttribute : MonoBehaviour
{
    private Text textField;
    [SerializeField] private int value = 0;

    void Start()
    {
        textField = GetComponent<Text>();
        RefreshText();
    }

    public int GatValue() => value;
    
    public bool SetValue(int _v)
    {
        if (!CScale.IsValidValue(_v)) return false;
        value = _v;
        RefreshText();
        return true;
    }

    public void RefreshText()
    {
        textField.text = CScale.GetCValue(value);
    }
}
