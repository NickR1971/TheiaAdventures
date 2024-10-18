using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CScale
{
    private readonly static string[] strValues = { "-", "G", "F", "E", "D", "C", "B", "A", "S" };
    private readonly static int[] Values = { 0, 1, 2, 3, 4, 5, 6, 7, 10 };

    private static int CheckValue(int _n)
    {
        if (_n < 0) return 0;
        if (_n > 8) return 8;
        return _n;
    }

    public static bool IsValidValue(int _n)
    {
        if (_n < 0) return false;
        if (_n > 8) return false;
        return true;
    }

    public static string GetCValue(int _v) => strValues[CheckValue(_v)];
    public static int GetEValue(int _v) => Values[CheckValue(_v)];
}
