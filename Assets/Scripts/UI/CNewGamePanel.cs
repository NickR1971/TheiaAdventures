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
    private ICharacterManager iCharacterManager;
    private SCharacter character;
    private bool isSelected;
    private void Start()
    {
        InitUI();
        Might.SetValue(0);
        Dex.SetValue(0);
        Intel.SetValue(0);
        Pers.SetValue(0);
        Know.SetValue(0);
        isSelected = false;
        iCharacterManager = AllServices.Container.Get<ICharacterManager>();
    }
    private void ShowSelected(SCharacter _character)
    {
        character = _character;
        isSelected = true;
        textOrigin.SetText(iCharacterManager.GetOriginName(_character.origin));
        textClass.SetText(iCharacterManager.GetClassName(_character.regularClass));
        textConst.SetText(iCharacterManager.GetConstTypeName(_character.typeConstitution));
        textConst.RefreshText();
        textOrigin.RefreshText();
        textClass.RefreshText();
        Might.SetValue(_character.attributes.might);
        Dex.SetValue(_character.attributes.dexterity);
        Intel.SetValue(_character.attributes.intelligence);
        Pers.SetValue(_character.attributes.personality);
        Know.SetValue(_character.attributes.knowledge);
    }
    public void ToGame()
    {
        if (isSelected) SceneManager.LoadScene("SceneBase");
    }
    
    // cancel create new game
    public void ToLogo()
    {
        CGameManager.SetGameData(null);
        SceneManager.LoadScene("SceneLogo");
    }
    public void OnCreateKnight()
    {
        ShowSelected(iCharacterManager.CreateCharacterTemplate(EOrigin.noble, ERegularClass.knight));
    }
    public void OnCreateMage()
    {
        ShowSelected(iCharacterManager.CreateCharacterTemplate(EOrigin.noble, ERegularClass.mage));
    }
}
