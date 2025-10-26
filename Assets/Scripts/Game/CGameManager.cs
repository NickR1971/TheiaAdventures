using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*****************************************************************
 * Semantic Versioning expresses versions as MAJOR.MINOR.PATCH,
 * where MAJOR introduces one or more breaking changes,
 * MINOR introduces one or more backward-compatible API changes,
 * and PATCH only introduces bug fixes with no API changes at all.
 * */
[Serializable]
public class SaveData
{
    public uint id;
    public int versionMajor;
    public int versionMinor;
    public int versionPatch;
    public string comment;
    public uint num_scene;
    public const int maxCharacters = 30;
    public SCharacter[] charList;
    public int numCharacters;
    public SaveData()
    {
        charList = new SCharacter[maxCharacters];
        numCharacters = 0;
    }
    public int AddCharacter(SCharacter _character)
    {
        if (numCharacters == maxCharacters) return -1;
        charList[numCharacters] = _character;
        charList[numCharacters].numPC = numCharacters;
        return numCharacters++;
    }
}


public static class CGameManager
{
	private static event Action onSave;
    public const int versionMajor=0;
    public const int versionMinor=2;
    public const int versionPatch=0;
    private static SaveData gameData = null;
    private static ICharacterManager iCharacterManager = null;

    public static void GetVersion(out int _verMajor, out int _verMinor, out int _verPatch)
    {
        _verMajor = versionMajor;
        _verMinor = versionMinor;
        _verPatch = versionPatch;
    }
    public static void SetCharacterInterface(ICharacterManager _characterManager)
    {
        iCharacterManager = _characterManager;
    }

	public static void SetGameData(SaveData _data)
    {
        gameData = _data;
    }

    public static SaveData GetData() => gameData;

    public static void OnSave()
    {
        if (gameData == null) return;
        onSave?.Invoke();

        DateTime dt = DateTime.Now;
        if (gameData.comment == null) gameData.comment = "";
        gameData.comment += dt.ToString();
        gameData.versionMajor = versionMajor;
        gameData.versionMinor = versionMinor;
        gameData.versionPatch = versionPatch;
    }
    public static void AddOnSaveAction(Action _a)
    {
        onSave += _a;
    }

    public static void RemoveOnSaveAction(Action _a)
    {
        onSave -= _a;
    }
    public static bool IsHexCell() => true; // select hex or square cell

}
