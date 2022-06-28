
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class GameSaveManager : MonoBehaviour
{

    public List<Item> _pickedUpItems;
    public List<PressurePlate> _pressurePlatesActivated;
    public int _checkpointNumber;
    public Inventory _inventory;

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
        _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();

        Save save = new Save(this);        
        
        BinaryFormatter binaryFormatter = new BinaryFormatter(); 
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath + "/gamesave.save"));
        binaryFormatter.Serialize(file,save);
        file.Close();
        Debug.Log("Saved on " + Path.Combine(Application.persistentDataPath + "/gamesave.save"));
    }

    public void AddPickedUpItem(Item item)
    {
        _pickedUpItems.Add(item);
    }

    public void SetCheckpoint(int checkpointNumber)
    {
        _checkpointNumber = checkpointNumber;
    }

    public void AddActivatedPressurePlate(PressurePlate pressurePlate)
    {
        //_pressurePlatesActivated.Add(pressurePlate);
    }
}