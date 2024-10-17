using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CNewGamePanel : CUI
{
    [SerializeField] private CTextLocalize textOrigin;
    [SerializeField] private CTextLocalize textClass;
    private int origin;
    private ELocalStringID[] origins = {
        ELocalStringID.game_origin_peasant,
        ELocalStringID.game_origin_artisan,
        ELocalStringID.game_origin_noble };
    private ELocalStringID[] classes = { 
        ELocalStringID.game_class_lumberjack, ELocalStringID.game_class_hunter, ELocalStringID.game_class_acolyte, ELocalStringID.game_class_adept,
        ELocalStringID.game_class_pikeman, ELocalStringID.game_class_crossbowman, ELocalStringID.game_class_acolyte, ELocalStringID.game_class_adept,
        ELocalStringID.game_class_knight, ELocalStringID.game_class_duelist, ELocalStringID.game_class_acolyte, ELocalStringID.game_class_adept };

    private void Start()
    {
        InitUI();
    }

    public void ToGame()
    {
        SceneManager.LoadScene("SceneBase");
    }
    
    // cancel create new game
    public void ToLogo()
    {
        CGameManager.SetGameData(null);
        SceneManager.LoadScene("SceneLogo");
    }

    // on select origin
    public void OnOrigin(int _id)
    {
        int n;

        origin = _id / 10;
        textOrigin.strID = origins[origin].ToString();
        n = _id % 10;
        n--;
        n += origin * 4;
        textClass.strID = classes[n].ToString();

        textOrigin.RefreshText();
        textClass.RefreshText();
    }
}
