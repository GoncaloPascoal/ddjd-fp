
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _gameSaveManager;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (_gameSaveManager == null)
        {
            _gameSaveManager = this;
            LoadSave();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void CreateGameSaveFile()
    {
        var save = GameData.GetSaveData();

        BinaryFormatter binaryFormatter = new BinaryFormatter(); 
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath + "/gamesave.save"));
        binaryFormatter.Serialize(file,save);
        file.Close();
        Debug.Log("Saved on " + Path.Combine(Application.persistentDataPath + "/gamesave.save"));
    }

    public void LoadSave()
    {
        var path = Application.persistentDataPath + "/gamesave.save";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            Save save = binaryFormatter.Deserialize(stream) as Save;

            GameData.SetSaveData(save);
            
            Debug.Log("Loaded save");
        }
    }
}