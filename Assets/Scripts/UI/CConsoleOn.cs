using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CConsoleOn : MonoBehaviour
{
    private IGameConsole gameConsole;

    // Start is called before the first frame update
    private void Start()
    {
        gameConsole = AllServices.Container.Get<IGameConsole>();
    }

    public void OnButton()
    {
        gameConsole.Show();
    }
}
