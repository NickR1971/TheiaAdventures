using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : ISaveLoad
{
	private CSaveFile saveFile;
	private IMainMenu mainMenu;

	public SaveLoad(CSaveFile _saveFile, IMainMenu _mainMenu)
    {
		mainMenu = _mainMenu;
		saveFile = _saveFile;
    }

	public string GetProfile() => saveFile.GetProfile();

	public bool SetProfile(string _name)
	{
		return saveFile.SetProfile(_name);
	}

	public bool IsSavedGameExist() => saveFile.IsSavedFileExist();
	public bool IsSavedGameExist(string _name) => saveFile.IsSavedFileExist(_name);

	public void Save(string _name)
	{
		CGameManager.OnSave();
		saveFile.Save(_name, CGameManager.GetData());
	}

	public void Load(string _name)
	{
		if (IsSavedGameExist())
		{
			SaveData data = CGameManager.GetData();
			saveFile.Load(_name, out data);
			CGameManager.SetGameData(data); //-----??
			mainMenu.GoToMainScene();
		}
		else
			Debug.LogError("There is no save data!");
	}

	public void RemoveSave(string _name)
	{
		saveFile.ResetData(_name);
	}

	public string[] GetSavedList() => saveFile.GetSavedList();

	public string[] GetCommentList() => saveFile.GetCommentList();
}
