using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CNewGamePanel : CUI
{
    private void Start()
    {
        InitUI();
    }

    public void ToBattle()
    {
        SceneManager.LoadScene("SceneBattle");
    }
    
    public void ToLogo()
    {
        SceneManager.LoadScene("SceneLogo");
    }
}
