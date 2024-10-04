using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitle : CUI
{
    private void Start()
    {
        InitUI();
        if (AllServices.Container.Get<IMainMenu>().IsGameExist()) uiManager.CloseUI();
    }
    public void OnButton()
    {
        uiManager.CloseUI();
    }
}
