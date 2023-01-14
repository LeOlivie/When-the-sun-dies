using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public static void Save<T>(string key, T saveData)
    {
        string jsonDataString = JsonUtility.ToJson(saveData, false);
        PlayerPrefs.SetString(key, jsonDataString);
    }

    public static T Load<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string jsonString = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(jsonString);
        }
        else
        {
            throw new System.Exception("Save key not found.");
        }
    }
}
