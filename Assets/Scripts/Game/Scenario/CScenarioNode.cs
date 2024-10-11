using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELocationPrimaryType
{
    unknown, // невідома територія
    plain, // рівнини
    forest, //ліс
    swamp, // болото
    desert, // пустеля
    mountain, // гори
    underworld // підзем'я
}
public enum ELocationSecondaryType
{
    unknown, // невідома територія
    river, // річка
    coast, // узбережжя моря чи озера
    urban // міська територія
}

public class CLocation
{
    private ELocationPrimaryType primaryType;
    private ELocationSecondaryType secondaryType;

    public CLocation()
    {
        primaryType = ELocationPrimaryType.unknown;
        secondaryType = ELocationSecondaryType.unknown;
    }
    public ELocationPrimaryType GetPrimaryType() => primaryType;
    public ELocationSecondaryType GetSecondaryType() => secondaryType;
}

public class CScenarioNode
{
    private CLocation currentLocation;
}
