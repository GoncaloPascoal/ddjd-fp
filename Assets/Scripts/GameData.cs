using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatsDictionary))]
[CustomPropertyDrawer(typeof(ItemPickupDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

public static class GameData
{
    public static Dictionary<int, int> Levels = new Dictionary<int, int>()
    {
        {1, 1}
    };
    
    public static bool InCheckpoint = false;
}
