using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CBase : CUI
{
    [SerializeField] private Text textID;
    [SerializeField] private InputField newID;
    private void Start()
    {
        InitUI();
        CGameManager.GetData().num_scene = 1;
        textID.text = CGameManager.GetData().id.ToString();
        newID.text = CGameManager.GetData().id.ToString();
    }
    public void OnNewID()
    {
        int vID;
        if (int.TryParse(newID.text, out vID))
        {
            CGameManager.GetData().id = (uint)vID;
            textID.text = CGameManager.GetData().id.ToString();
        }
    }
    public void ToBattle()
    {
        SceneManager.LoadScene("SceneBattle");
    }

    public void ToMenu()
    {
        uiManager.CloseUI();
    }
}
