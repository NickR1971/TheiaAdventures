﻿using System;

public interface ILocalization : IService
{
    UsedLocal GetCurrentLanguage();
    void LoadLanguage(UsedLocal _language);
    string GetString(ELocalStringID _id);
    string GetString(string _key);
    void SetOnChange(Action _action);
    void RemoveOnChange(Action _action);
}
