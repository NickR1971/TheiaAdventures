using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CNewGamePanel : CUI
{
    [SerializeField] private CTextLocalize textOrigin;
    [SerializeField] private CTextLocalize textClass;
    [SerializeField] private CTextLocalize textConst;
    [SerializeField] private CTextAttribute Might;
    [SerializeField] private CTextAttribute Dex;
    [SerializeField] private CTextAttribute Intel;
    [SerializeField] private CTextAttribute Pers;
    [SerializeField] private CTextAttribute Know;
    private int origin;
    private ELocalStringID[] origins = {
        ELocalStringID.game_origin_peasant,
        ELocalStringID.game_origin_artisan,
        ELocalStringID.game_origin_noble };
    private ELocalStringID[] classes = { 
        ELocalStringID.game_class_lumberjack, ELocalStringID.game_class_hunter, ELocalStringID.game_class_acolyte, ELocalStringID.game_class_adept,
        ELocalStringID.game_class_pikeman, ELocalStringID.game_class_crossbowman, ELocalStringID.game_class_monk, ELocalStringID.game_class_alchemist,
        ELocalStringID.game_class_knight, ELocalStringID.game_class_duelist, ELocalStringID.game_class_priest, ELocalStringID.game_class_sorcerer };

    private ELocalStringID[] constType = {
        ELocalStringID.game_const_balanced, ELocalStringID.game_const_scholar, ELocalStringID.game_const_barbarian, ELocalStringID.game_const_leader, ELocalStringID.game_const_agile,
        ELocalStringID.game_const_goof, ELocalStringID.game_const_orphan, ELocalStringID.game_const_genius, ELocalStringID.game_const_nerd, ELocalStringID.game_const_politician };

    private int constSelected = 0;
    private const int maxConst = 9;

    private int[] mightValues = { 3, 1, 4, 4, 2, 5, 3, 1, 2, 2 };
    private int[] dexValues =   { 3, 2, 4, 3, 4, 3, 5, 1, 2, 2 };
    private int[] intValues =   { 3, 4, 3, 2, 4, 1, 2, 5, 2, 3 };
    private int[] persValues =  { 3, 4, 3, 4, 2, 3, 2, 3, 3, 5 };
    private int[] knowValues =  { 3, 4, 1, 2, 3, 1, 2, 4, 5, 1 };

    private void Start()
    {
        InitUI();
        SetAttributes();
    }

    private void SetAttributes()
    {
        Might.SetValue(mightValues[constSelected]);
        Dex.SetValue(dexValues[constSelected]);
        Intel.SetValue(intValues[constSelected]);
        Pers.SetValue(persValues[constSelected]);
        Know.SetValue(knowValues[constSelected]);
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
        textOrigin.SetText(origins[origin]);
        n = _id % 10; n--;  n += origin * 4;
        textClass.SetText(classes[n]);

        textOrigin.RefreshText();
        textClass.RefreshText();
    }

    public void OnConstSelect( int _n)
    {
        if (_n < 0) _n = 0;
        if (_n > maxConst) _n = maxConst;
        constSelected = _n;

        textConst.SetText(constType[constSelected]);
        textConst.RefreshText();
        SetAttributes();
    }
}
