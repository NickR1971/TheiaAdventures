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
    ELocalStringID GetOriginName(EOrigin _origin);
    SCharacter CreateCharacterTemplate(EOrigin _origin, ERegularClass _rClass);
}
public class CharacterManager : MonoBehaviour, ICharacterManager
{
    [SerializeField] private Sprite[] sprites = new Sprite[7];
    [SerializeField] private GameObject[] prefabs = new GameObject[7];
    private ELocalStringID[] origins = {
        ELocalStringID.game_origin_peasant,
        ELocalStringID.game_origin_artisan,
        ELocalStringID.game_origin_noble,
        ELocalStringID.game_const_barbarian,
        ELocalStringID.game_origin_animal,
        ELocalStringID.game_origin_demon,
        ELocalStringID.game_origin_elemental,
        ELocalStringID.game_origin_elf,
        ELocalStringID.game_origin_giant,
        ELocalStringID.game_origin_goblin,
        ELocalStringID.game_origin_ogre,
        ELocalStringID.game_origin_orc,
        ELocalStringID.game_origin_monster,
        ELocalStringID.core_empty,  // dragon
        ELocalStringID.game_origin_undead,
        ELocalStringID.game_origin_vampire
    };
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

    public ELocalStringID GetOriginName(EOrigin _origin) => origins[(int)_origin];

    public SCharacter CreateCharacterTemplate(EOrigin _origin, ERegularClass _rClass)
    {
        SCharacter tempCharacter = new SCharacter();

        tempCharacter.origin = _origin;
        tempCharacter.regularClass = _rClass;

        switch(_rClass)
        {
            case ERegularClass.knight:
                tempCharacter.attributes = SetAttributes(EConstitution.leader);
                tempCharacter.cType = EActorType.knight;
                break;
            case ERegularClass.mage:
                tempCharacter.attributes = SetAttributes(EConstitution.genius);
                tempCharacter.cType = EActorType.mage;
                break;
            case ERegularClass.wizard:
                tempCharacter.attributes = SetAttributes(EConstitution.scholar);
                tempCharacter.cType = EActorType.mage;
                break;
            case ERegularClass.sorcerer:
                tempCharacter.attributes = SetAttributes(EConstitution.balanced);
                tempCharacter.attributes.might--;
                tempCharacter.attributes.intelligence++;
                tempCharacter.cType = EActorType.mage;
                break;
            case ERegularClass.zombie:
                tempCharacter.attributes = SetAttributes(EConstitution.goof);
                if (tempCharacter.origin != EOrigin.undead)
                {
                    tempCharacter.origin = EOrigin.undead;
                    Debug.Log("origin turn to undead for " + _rClass.ToString());
                }
                tempCharacter.cType = EActorType.zombie;
                break;
            case ERegularClass.skeleton:
                tempCharacter.attributes = SetAttributes(EConstitution.agile);
                if (tempCharacter.origin != EOrigin.undead)
                {
                    tempCharacter.origin = EOrigin.undead;
                    Debug.Log("origin turn to undead for " + _rClass.ToString());
                }
                tempCharacter.cType = EActorType.skeleton;
                break;
            default:
                tempCharacter.attributes = SetAttributes(EConstitution.balanced);
                tempCharacter.cType = EActorType.hero;
                break;
        }

        if (tempCharacter.origin == EOrigin.undead)
        {
            tempCharacter.attributes.personality = 1;
        }

        tempCharacter.secondaryAttributes.speed = tempCharacter.attributes.dexterity;
        tempCharacter.secondaryAttributes.initiative = tempCharacter.attributes.dexterity;
        tempCharacter.secondaryAttributes.reaction = 1;
        tempCharacter.points.redHits = CScale.GetEValue(tempCharacter.attributes.might);
        tempCharacter.points.yellowHits = tempCharacter.points.redHits / 2;
        tempCharacter.points.blueHits = 0;
        tempCharacter.points.greenHits = 0;
        tempCharacter.points.actions = CScale.GetEValue(tempCharacter.secondaryAttributes.initiative);
        tempCharacter.points.mana = CScale.GetEValue(tempCharacter.attributes.intelligence);
        tempCharacter.points.will = CScale.GetEValue(tempCharacter.attributes.personality);
        tempCharacter.currentPoints = tempCharacter.points;

        return tempCharacter;
    }
}
