using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBase : CUI
{
    private void Start()
    {
        InitUI();
        CGameManager.GetData().num_scene = 1;
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
