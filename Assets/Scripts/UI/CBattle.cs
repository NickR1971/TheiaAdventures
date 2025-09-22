using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public interface IBattle : IService
{
    void SetCharacterSprite(Sprite _sprite);
    void SetCharacterName(string _name);
}

public class CBattle : CUI, IBattle
{
    private IGame game;
    private IGameConsole gameConsole;
    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject characterName;
    private Image imgChar;
    private Text nameChar;

    public void SetCharacterName(string _name)
    {
        nameChar.text = _name;
    }

    public void SetCharacterSprite(Sprite _sprite)
    {
        imgChar.sprite = _sprite;
    }
    private void Awake()
    {
        AllServices.Container.Register<IBattle>(this);
    }
    private void Start()
    {
        InitUI();
        game = AllServices.Container.Get<IGame>();
        game.CreateGame(CGameManager.GetData());
        CGameManager.GetData().num_scene = 2;
        imgChar = characterImage.GetComponent<Image>();
        nameChar = characterName.GetComponent<Text>();
    }
}
