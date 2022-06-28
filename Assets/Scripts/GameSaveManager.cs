
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
        }
        else
        {
            DestroyObject(gameObject);
        }
    }
    
    public void CreateGameSaveFile()
    {
        Save save = new Save();        
        
        BinaryFormatter binaryFormatter = new BinaryFormatter(); 
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath + "/gamesave.save"));
        binaryFormatter.Serialize(file,save);
        file.Close();
        Debug.Log("Saved on " + Path.Combine(Application.persistentDataPath + "/gamesave.save"));
    }

    
}