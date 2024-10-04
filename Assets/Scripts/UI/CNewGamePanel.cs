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

    public void ToGame()
    {
        SceneManager.LoadScene("SceneBase");
    }
    
    public void ToLogo()
    {
        CGameManager.SetGameData(null);
        SceneManager.LoadScene("SceneLogo");
    }
}
