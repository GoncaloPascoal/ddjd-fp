using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public static String GameObjectToHash(GameObject gameObject)
    {
        var hash = new Hash128();
        hash.Append(gameObject.name);
        hash.Append(gameObject.tag);
        
        var position = gameObject.transform.position;
        hash.Append(position.x);
        hash.Append(position.y);
        hash.Append(position.z);
        
        var rotation = gameObject.transform.rotation;
        hash.Append(rotation.x);
        hash.Append(rotation.y);
        hash.Append(rotation.z);
        
        var localScale = gameObject.transform.localScale;
        hash.Append(localScale.x);
        hash.Append(localScale.y);
        hash.Append(localScale.z);
        
        return hash.ToString();
    }
}
