using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    bool isPostProcessingEnabled;

    private void Start()
    {
        string jsonData = File.ReadAllText(Application.persistentDataPath + "CCGameSettings.json");
        Settings settings = JsonUtility.FromJson<Settings>(jsonData);

        isPostProcessingEnabled = System.Convert.ToBoolean(settings.PPEnabled);
    }

    private void OnLevelWasLoaded()
    {
        if (Instance != this)
        {
            Destroy(Instance);
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindObjectOfType<Volume>().enabled = isPostProcessingEnabled;
        }
    }

    public void SwitchPostProcessing(bool value)
    {
        Settings settings = new Settings();

        isPostProcessingEnabled = value;
        FindObjectOfType<Volume>().enabled = isPostProcessingEnabled;
        settings.PPEnabled = System.Convert.ToInt32(value);
        string jsonData = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.persistentDataPath + "CCGameSettings.json", jsonData);
    }
}

[System.Serializable]
public class Settings
{
    public int PPEnabled;
}