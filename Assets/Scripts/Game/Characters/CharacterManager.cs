using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterType
{
    hero=0, knight=1, mage=2, cleric=3, barbarian=4,
    zombie=5, skeleton=6
}
public interface ICharacterManager : IService
{
    bool GetCharacter(ECharacterType _ctype, out Sprite _spr, out GameObject _prefab);
}
public class CharacterManager : MonoBehaviour, ICharacterManager
{
    [SerializeField] private Sprite[] sprites = new Sprite[7];
    [SerializeField] private GameObject[] prefabs = new GameObject[7];

    public bool GetCharacter(ECharacterType _ctype, out Sprite _spr, out GameObject _prefab)
    {
        int i = ((int)_ctype);

        _prefab = prefabs[i];
        _spr = sprites[i];
        if (_prefab == null) return false;
        return true;
    }

    private void Awake()
    {
        AllServices.Container.Register<ICharacterManager>(this);
    }
    void Start()
    {
        CGameManager.SetCharacterInterface(this);
    }
    void OnDestroy()
    {
        AllServices.Container.UnRegister<ICharacterManager>();
        CGameManager.SetCharacterInterface(null);
    }
}
