using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUnitType
{
    unknown, knight, mage, priest, guard,
    warrior, orc, goblin, spider, golem,
    zombie, skeleton, deathknight, lich
}
public enum EActorType
{
    none=-1, knight=0, mage=1, goblin=2, priest=3, barbarian=4,
    zombie=5, skeleton=6, spider=7, golem=8
}
public enum EConstitution
{
    balanced = 0, scholar = 1, barbarian = 2, leader = 3, agile = 4,
    goof = 5, orphan = 6, genius = 7, nerd = 8, politician = 9,
    animal = 10
}
public enum EOrigin
{
    peasant = 0, artisan = 1, noble = 2, barbarian = 3,
    animal = 4, demon = 5, elemental = 6, elf = 7,
    giant = 8, goblin = 9, ogre = 10, orc = 11,
    monster = 12, dragon = 13, undead = 14, vampire = 15,
    construct=16, golem=17
}
public enum ERegularClass
{
    none = 0,
    knight = 1, mage = 2, zombie = 3, skeleton = 4,
    adept = 5, alchemist = 6, wizard = 7, warlock = 8,
    sorcerer = 9, elementalist = 10, warrior = 11,
    guard=12, priest=13, savager=14,
    herbalist, blacksmith, battlemage, gladiator,
    lumberjack, hunter, pikeman, crossbowman,
    acolyte, monk, shaman, pilgrim,
    minstrel, duelist
}
public enum EEliteClass
{
    none,
    ranger, // hunter
    stalker, // hunter
    engeneer, // blacksmith
    technomage, // blacksmith
    bard, // minstrel
    inquisitor, // monk
    blackguard, // guard
    eliteguard, // guard
    highpriest, // priest
    oracle, // priest
    archmage, // mage
    necromancer, // wizard
    demonlord, // warlock
    enchanter, // alchemist
    spiritlord, // elementalist
    weaponmaster, // duelist
    paladin, // knight
    champion, // knight
    templar, // knight
    spellsword, // warrior
    warlord, // warrior
    berserker // warrior
}

public interface ICharacterManager : IService
{
    bool GetCharacter(SCharacter _character, out Sprite _spr, out GameObject _prefab);
    SAttributes SetAttributes(EConstitution _cons);
    ELocalStringID GetConstTypeName(EConstitution _cons);
    ELocalStringID GetOriginName(EOrigin _origin);
    ELocalStringID GetClassName(ERegularClass _rClass);
    SCharacter SetStandartName(ref SCharacter _character);
    SCharacter CreateCharacterTemplate(
        EOrigin _origin, ERegularClass _rClass, EConstitution _constitution, EActorType _actor);
    bool GenerateStandartCharacter(EUnitType _unitType, out SCharacter _character);
    void AddCharacter(SCharacter _character);
}
public class CharacterManager : MonoBehaviour, ICharacterManager
{
    [SerializeField] private Sprite[] sprites = new Sprite[36];
    [SerializeField] private GameObject[] prefabs = new GameObject[9];
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
        ELocalStringID.game_origin_dragon,
        ELocalStringID.game_origin_undead,
        ELocalStringID.game_origin_vampire,
        ELocalStringID.game_origin_construct,
        ELocalStringID.game_origin_golem
    };
    private ELocalStringID[] constType = {
        ELocalStringID.game_const_balanced, ELocalStringID.game_const_scholar,
        ELocalStringID.game_const_barbarian, ELocalStringID.game_const_leader,
        ELocalStringID.game_const_agile, ELocalStringID.game_const_goof,
        ELocalStringID.game_const_orphan, ELocalStringID.game_const_genius,
        ELocalStringID.game_const_nerd, ELocalStringID.game_const_politician,
        ELocalStringID.game_origin_animal };
    private readonly int[] mightValues = { 3, 1, 4, 4, 2, 5, 3, 1, 2, 2, 4 };
    private readonly int[] dexValues =   { 3, 2, 4, 3, 4, 3, 5, 1, 2, 2, 2 };
    private readonly int[] intValues =   { 3, 4, 3, 2, 4, 1, 2, 5, 2, 3, 1 };
    private readonly int[] persValues =  { 3, 4, 3, 4, 2, 3, 2, 3, 3, 5, 3 };
    private readonly int[] knowValues =  { 3, 4, 1, 2, 3, 1, 2, 4, 5, 1, 3 };
    private ELocalStringID[] classes = {
        ELocalStringID.core_empty, ELocalStringID.game_class_knight, ELocalStringID.game_class_mage,
        ELocalStringID.game_class_zombie, ELocalStringID.game_class_skeleton,
        ELocalStringID.game_class_adept, ELocalStringID.game_class_alchemist,
        ELocalStringID.game_class_wizard, ELocalStringID.game_class_warlock,
        ELocalStringID.game_class_sorcerer, ELocalStringID.game_class_elementalist,
        ELocalStringID.game_class_warrior, ELocalStringID.game_class_guard,
        ELocalStringID.game_class_priest, ELocalStringID.game_class_savager,
        //////////////////////////////
        ELocalStringID.game_class_lumberjack, ELocalStringID.game_class_hunter,
        ELocalStringID.game_class_acolyte, ELocalStringID.game_class_pikeman,
        ELocalStringID.game_class_crossbowman,
        ELocalStringID.game_class_monk, ELocalStringID.game_class_duelist
    };

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
    private int SelectDefaultPortrait(SCharacter _chr)
    {
        int num;

        switch (_chr.cType)
        {
            case EActorType.knight:
                switch (_chr.regularClass)
                {
                    case ERegularClass.knight:
                        num = 2;
                        break;
                    case ERegularClass.guard:
                        num = 29;
                        break;
                    case ERegularClass.warrior:
                        num = 25;
                        break;
                    default:
                        num = 24;
                        break;
                }
                if (_chr.origin == EOrigin.undead) num = 26;
                break;
            case EActorType.mage:
                if (_chr.origin == EOrigin.undead) num = 23;
                else
                {
                    switch (_chr.regularClass)
                    {
                        case ERegularClass.alchemist:
                            num = 33;
                            break;
                        case ERegularClass.elementalist:
                            num = 21;
                            break;
                        case ERegularClass.wizard:
                            num = 27;
                            break;
                        default:
                            num = 3;
                            break;
                    }
                }
                break;
            case EActorType.barbarian:
                num = 35;
                break;
            case EActorType.goblin:
                num = 5;
                break;
            case EActorType.priest:
                num = 4;
                break;
            case EActorType.skeleton:
                num = 30;
                break;
            case EActorType.zombie:
                num = 31;
                break;
            case EActorType.spider:
                num = 6;
                break;
            case EActorType.golem:
                num = 20;
                break;
            default:
                num = 1;
                break;
        }
        return num;
    }
    /////// ======= ICharacterManager ==========
    public bool GetCharacter(SCharacter _character, out Sprite _spr, out GameObject _prefab)
    {
        int i = (int)_character.cType;
        if (i < 0) { _prefab = null; _spr = null; return false; }

        _prefab = prefabs[i];
        _spr = sprites[_character.portraitIndex];
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
    public ELocalStringID GetClassName(ERegularClass _rClass) => classes[(int)_rClass];
    public SCharacter CreateCharacterTemplate(
        EOrigin _origin,
        ERegularClass _rClass,
        EConstitution _constitution,
        EActorType _actor)
    {
        SCharacter templateCharacter = new SCharacter();

        templateCharacter.origin = _origin;
        templateCharacter.regularClass = _rClass;
        templateCharacter.typeConstitution = _constitution;
        templateCharacter.attributes = SetAttributes(templateCharacter.typeConstitution);
        templateCharacter.cType = _actor;
        templateCharacter.numPC = -1;

        if (templateCharacter.typeConstitution == EConstitution.balanced)
        {
            switch (templateCharacter.origin)
            {
                case EOrigin.noble:
                    templateCharacter.attributes.personality++;
                    break;
                case EOrigin.artisan:
                    templateCharacter.attributes.knowledge++;
                    break;
                case EOrigin.peasant:
                    templateCharacter.attributes.might++;
                    break;
            }
            switch (_rClass)
            {
                case ERegularClass.knight:
                    templateCharacter.attributes.might++;
                    templateCharacter.attributes.intelligence--;
                    break;
                case ERegularClass.warrior:
                    templateCharacter.attributes.might++;
                    templateCharacter.attributes.personality--;
                    break;
                case ERegularClass.guard:
                case ERegularClass.savager:
                    templateCharacter.attributes.might++;
                    templateCharacter.attributes.knowledge--;
                    break;
                case ERegularClass.mage:
                case ERegularClass.alchemist:
                    templateCharacter.attributes.might--;
                    templateCharacter.attributes.knowledge++;
                    break;
                case ERegularClass.wizard:
                case ERegularClass.adept:
                    templateCharacter.attributes.might--;
                    templateCharacter.attributes.intelligence++;
                    break;
                case ERegularClass.sorcerer:
                case ERegularClass.elementalist:
                case ERegularClass.warlock:
                    templateCharacter.attributes.might--;
                    templateCharacter.attributes.personality++;
                    break;
                case ERegularClass.priest:
                    templateCharacter.attributes.dexterity--;
                    templateCharacter.attributes.personality++;
                    break;
                default:
                    Debug.Log("Class not corrected " + _rClass.ToString());
                    break;
            }
        }
        switch (_rClass)
        {
            case ERegularClass.zombie:
            case ERegularClass.skeleton:
                if (templateCharacter.origin != EOrigin.undead)
                {
                    templateCharacter.origin = EOrigin.undead;
                    Debug.Log("origin turn to undead for " + _rClass.ToString());
                }
                break;
        }
        switch (templateCharacter.origin)
        {
            case EOrigin.barbarian:
                templateCharacter.typeConstitution = EConstitution.barbarian;
                templateCharacter.attributes = SetAttributes(templateCharacter.typeConstitution);
                break;
            case EOrigin.undead:
                templateCharacter.attributes.personality = 1;
                break;
            case EOrigin.animal:
                templateCharacter.attributes.intelligence = 1;
                break;
        }

        templateCharacter.portraitIndex = SelectDefaultPortrait(templateCharacter);

        templateCharacter.secondaryAttributes.speed = templateCharacter.attributes.dexterity;
        templateCharacter.secondaryAttributes.initiative = templateCharacter.attributes.dexterity;
        templateCharacter.secondaryAttributes.reaction = 1;
        templateCharacter.points.redHits = CScale.GetEValue(templateCharacter.attributes.might);
        templateCharacter.points.yellowHits = templateCharacter.points.redHits / 2;
        templateCharacter.points.blueHits = 0;
        templateCharacter.points.greenHits = 0;
        templateCharacter.points.actions = CScale.GetEValue(templateCharacter.secondaryAttributes.initiative);
        templateCharacter.points.mana = CScale.GetEValue(templateCharacter.attributes.intelligence);
        templateCharacter.points.will = CScale.GetEValue(templateCharacter.attributes.personality);
        templateCharacter.currentPoints = templateCharacter.points;

        return templateCharacter;
    }
    public SCharacter SetStandartName(ref SCharacter _character)
    {
        string chrName;
        switch(_character.origin)
        {
            case EOrigin.artisan:
            case EOrigin.peasant:
            case EOrigin.noble:
                chrName = _character.regularClass.ToString();
                break;
            default:
                chrName = _character.origin.ToString();
                break;
        }
        _character.cName = chrName;
        return _character;
    }
    public void AddCharacter(SCharacter _character)
    {
        if (string.IsNullOrEmpty(_character.cName)) SetStandartName(ref _character);
        CGameManager.GetData().AddCharacter(_character);
    }
    public bool GenerateStandartCharacter(EUnitType _unitType, out SCharacter _character)
    {
        bool r = false;
        
        _character = new SCharacter();

        switch(_unitType)
        {
            case EUnitType.knight:
                _character = CreateCharacterTemplate(
                    EOrigin.noble, ERegularClass.knight, EConstitution.leader, EActorType.knight);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_knight);
                r = true;
                break;
            case EUnitType.guard:
                _character = CreateCharacterTemplate(
                    EOrigin.peasant, ERegularClass.guard, EConstitution.balanced, EActorType.knight);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_guard);
                r = true;
                break;
            case EUnitType.warrior:
                _character = CreateCharacterTemplate(
                    EOrigin.artisan, ERegularClass.warrior, EConstitution.balanced, EActorType.knight);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_warrior);
                r = true;
                break;
            case EUnitType.deathknight:
                _character = CreateCharacterTemplate(
                    EOrigin.undead, ERegularClass.knight, EConstitution.balanced, EActorType.knight);
                r = true;
                break;
            case EUnitType.zombie:
                _character = CreateCharacterTemplate(
                    EOrigin.undead, ERegularClass.zombie, EConstitution.goof, EActorType.zombie);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_zombie);
                r = true;
                break;
            case EUnitType.skeleton:
                _character = CreateCharacterTemplate(
                    EOrigin.undead, ERegularClass.skeleton, EConstitution.goof, EActorType.skeleton);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_skeleton);
                r = true;
                break;
            case EUnitType.spider:
                _character = CreateCharacterTemplate(
                    EOrigin.animal, ERegularClass.savager, EConstitution.animal, EActorType.spider);
                _character.cName = CLocalization.GetString(ELocalStringID.game_animal_spider);
                r = true;
                break;
            case EUnitType.goblin:
                _character = CreateCharacterTemplate(
                    EOrigin.goblin, ERegularClass.warrior, EConstitution.balanced, EActorType.goblin);
                _character.cName = CLocalization.GetString(ELocalStringID.game_origin_goblin);
                r = true;
                break;
            case EUnitType.lich:
                _character = CreateCharacterTemplate(
                    EOrigin.undead, ERegularClass.mage, EConstitution.genius, EActorType.mage);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_necromancer);
                r = true;
                break;
            case EUnitType.mage:
                _character = CreateCharacterTemplate(
                    EOrigin.noble, ERegularClass.mage, EConstitution.genius, EActorType.mage);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_mage);
                r = true;
                break;
            case EUnitType.orc:
                _character = CreateCharacterTemplate(
                    EOrigin.orc, ERegularClass.warrior, EConstitution.balanced, EActorType.barbarian);
                _character.cName = CLocalization.GetString(ELocalStringID.game_origin_orc);
                r = true;
                break;
            case EUnitType.priest:
                _character = CreateCharacterTemplate(
                    EOrigin.artisan, ERegularClass.priest, EConstitution.balanced, EActorType.priest);
                _character.cName = CLocalization.GetString(ELocalStringID.game_class_priest);
                r = true;
                break;
            case EUnitType.golem:
                _character = CreateCharacterTemplate(
                    EOrigin.golem, ERegularClass.warrior, EConstitution.goof, EActorType.golem);
                _character.cName = CLocalization.GetString(ELocalStringID.game_origin_golem);
                r = true;
                break;
        }

        return r;
    }
}
