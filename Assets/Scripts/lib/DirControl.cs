using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMapDirection
{
    center = 0, north = 1, northeast = 2, east = 3, southeast = 4,
    south = 5, southwest = 6, west = 7, northwest = 8
}

public static class CDirControl
{
    private static bool[] directions = { false, true, true, true, true, true, true, true, true };
    private static EMapDirection[] backDirections = { EMapDirection.center,
        EMapDirection.south, EMapDirection.southwest, EMapDirection.west, EMapDirection.northwest,
        EMapDirection.north, EMapDirection.northeast, EMapDirection.east, EMapDirection.southeast };
    public static void SetHex(bool _f)
    {
        directions[(int)EMapDirection.north] = !_f;
        directions[(int)EMapDirection.south] = !_f;
    }
    public static EMapDirection GetLeft(EMapDirection _start)
    {
        int i = (int)_start;
        do
        {
            i++;
            if (i == 9) i = 0;
        } while (!directions[i]);

        return (EMapDirection)i;
    }
    public static EMapDirection GetRight(EMapDirection _start)
    {
        int i = (int)_start;
        do
        {
            i--;
            if (i < 0) i = 8;
        } while (!directions[i]);

        return (EMapDirection)i;
    }
    public static EMapDirection GetBack(EMapDirection _start) => backDirections[(int)_start];
}
