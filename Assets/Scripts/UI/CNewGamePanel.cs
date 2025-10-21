using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CNewGamePanel : CUI
{
    [SerializeField] private Button buttonOk;
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
    private void Start()
    {
        InitUI();
        Might.SetValue(0);
        Dex.SetValue(0);
        Intel.SetValue(0);
        Pers.SetValue(0);
        Know.SetValue(0);
        iCharacterManager = AllServices.Container.Get<ICharacterManager>();
    }
    private void ShowSelected(SCharacter _character)
    {
        character = _character;
        buttonOk.interactable = true;
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
        SceneManager.LoadScene("SceneBase");
        iCharacterManager.AddCharacter(character);
        iCharacterManager.AddCharacter(
            iCharacterManager.CreateCharacterTemplate(
                EOrigin.artisan, ERegularClass.guard,EConstitution.balanced,EActorType.knight));
        if (character.regularClass == ERegularClass.mage)
        {
            iCharacterManager.AddCharacter(
                iCharacterManager.CreateCharacterTemplate(
                    EOrigin.artisan, ERegularClass.warrior, EConstitution.balanced, EActorType.knight));
            iCharacterManager.AddCharacter(
                iCharacterManager.CreateCharacterTemplate(
                    EOrigin.peasant, ERegularClass.adept, EConstitution.balanced, EActorType.mage));
        }
        else
        {
            iCharacterManager.AddCharacter(
                iCharacterManager.CreateCharacterTemplate(
                    EOrigin.artisan, ERegularClass.wizard, EConstitution.scholar, EActorType.mage));
            iCharacterManager.AddCharacter(
                iCharacterManager.CreateCharacterTemplate(
                    EOrigin.barbarian, ERegularClass.warrior, EConstitution.balanced, EActorType.knight));
        }
        iCharacterManager.AddCharacter(
            iCharacterManager.CreateCharacterTemplate(
                EOrigin.goblin, ERegularClass.warrior, EConstitution.balanced, EActorType.goblin));
    }

    // cancel create new game
    public void ToLogo()
    {
        CGameManager.SetGameData(null);
        SceneManager.LoadScene("SceneLogo");
    }
    public void OnCreateKnight()
    {
        SCharacter chr = iCharacterManager.CreateCharacterTemplate(
            EOrigin.noble, ERegularClass.knight, EConstitution.leader, EActorType.knight);
        chr.cName = "sir Marcus";
        ShowSelected(chr);
    }
    public void OnCreateMage()
    {
        SCharacter chr = iCharacterManager.CreateCharacterTemplate(
            EOrigin.noble, ERegularClass.mage, EConstitution.genius, EActorType.mage);
        chr.cName = "Henner";
        ShowSelected(chr);
    }
}
