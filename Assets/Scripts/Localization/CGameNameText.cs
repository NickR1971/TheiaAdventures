using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGameNameText : MonoBehaviour
{
    private ILocalization localization;
    private Text textField;
    void Start()
    {
        localization = AllServices.Container.Get<ILocalization>();
        textField = GetComponent<Text>();
        RefreshText();
        localization.SetOnChange(RefreshText);
    }
    private void OnDestroy()
    {
        localization.RemoveOnChange(RefreshText);
    }
    public void RefreshText()
    {
        int ver1, ver2, ver3;
        CGameManager.GetVersion(out ver1, out ver2, out ver3);
        textField.text = localization.GetString(ELocalStringID.game_name)
            + " version " + ver1 + "." + ver2 + "." + ver3;
    }

}
