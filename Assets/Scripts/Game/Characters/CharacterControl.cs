using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    protected CActor actor;
    protected CCharacter characterData;
    protected bool playable;

    private void Awake()
    {
        actor = GetComponent<CActor>();
        playable = false;
    }
    void Start()
    {
        
    }
    public void SetCharacterData(CCharacter _charData)
    {
        characterData = _charData;
    }
    public bool IsPlayable() => playable;
}
