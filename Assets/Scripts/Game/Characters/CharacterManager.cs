using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EActorType
{
    hero=0, knight=1, mage=2, cleric=3, barbarian=4,
    zombie=5, skeleton=6
}
public enum EConstitution
{
    balanced = 0, scholar = 1, barbarian = 2, leader = 3, agile = 4,
    goof = 5, orphan = 6, genius = 7, nerd = 8, politician = 9
}
public interface ICharacterManager : IService
{
    bool GetCharacter(EActorType _ctype, out Sprite _spr, out GameObject _prefab);
    SAttributes SetAttributes(EConstitution _cons);
    ELocalStringID GetConstTypeName(EConstitution _cons);
}
public class CharacterManager : MonoBehaviour, ICharacterManager
{
    [SerializeField] private Sprite[] sprites = new Sprite[7];
    [SerializeField] private GameObject[] prefabs = new GameObject[7];
    private ELocalStringID[] constType = {
        ELocalStringID.game_const_balanced, ELocalStringID.game_const_scholar,
        ELocalStringID.game_const_barbarian, ELocalStringID.game_const_leader,
        ELocalStringID.game_const_agile, ELocalStringID.game_const_goof,
        ELocalStringID.game_const_orphan, ELocalStringID.game_const_genius,
        ELocalStringID.game_const_nerd, ELocalStringID.game_const_politician };
    private readonly int[] mightValues = { 3, 1, 4, 4, 2, 5, 3, 1, 2, 2 };
    private readonly int[] dexValues =   { 3, 2, 4, 3, 4, 3, 5, 1, 2, 2 };
    private readonly int[] intValues =   { 3, 4, 3, 2, 4, 1, 2, 5, 2, 3 };
    private readonly int[] persValues =  { 3, 4, 3, 4, 2, 3, 2, 3, 3, 5 };
    private readonly int[] knowValues =  { 3, 4, 1, 2, 3, 1, 2, 4, 5, 1 };

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
    /////// ======= ICharacterManager ==========
    public bool GetCharacter(EActorType _ctype, out Sprite _spr, out GameObject _prefab)
    {
        int i = ((int)_ctype);

        _prefab = prefabs[i];
        _spr = sprites[i];
        if (_prefab == null) return false;
        return true;
    }

    public SAttributes SetAttributes(EConstitution _cons)
    {
        SAttributes a;
        int constSelected = (int)_cons;

        a.might = mightValues[constSelected];
        a.dexterity = dexValues[constSelected];
        a.intelligence = intValues[constSelected];
        a.personality = persValues[constSelected];
        a.knowledge = knowValues[constSelected];

        return a;
    }
    public ELocalStringID GetConstTypeName(EConstitution _cons) => constType[(int)_cons];
}
