using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CUtil
{
    public static bool CheckNameForSave(string _name) => (_name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);

    public static string GetWord(string _str)
    {
        int i;

        for(i=0; i<_str.Length;i++)
        {
            if (_str[i] <= ' ') break;
        }
        return _str.Substring(0, i);
    }

    public static float StringToFloat(string _str)
    {
        return float.Parse(_str.Replace('.', ','));
    }

    public static int StringToInt(string _str)
    {
        return int.Parse(GetWord(_str));
    }

    public static bool IsDigit(char _c)
    {
        switch (_c)
        {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return true;
        }

        return false;
    }

    public static void LogConsole(string _msg)
    {
        AllServices.Container.Get<IGameConsole>().ShowMessage(_msg);
    }
    public static bool SolveQuadraticEcuation(float _a, float _b, float _c, out float _x1, out float _x2)
    {
        float d = (_b * _b) - (4.0f * _a * _c);
        _x1 = _x2 = 0;
        if (d < 0) return false;

        d = Mathf.Sqrt(d);
        _x1 = (-_b + d) / (2.0f * _a);
        _x2 = (-_b - d) / (2.0f * _a);
        return true;
    }
}
