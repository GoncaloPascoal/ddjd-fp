using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutscenePlayer : MonoBehaviour
{
    private PlayableDirector _timeline;
    
    public GameObject cutsceneElements;

    // Start is called before the first frame update
    void Start()
    {
        _timeline = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CutsceneEnd()
    {
        Destroy(cutsceneElements);

        foreach (var objectInScene in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (objectInScene.transform.parent == null && !objectInScene.activeInHierarchy)
            {
                objectInScene.SetActive(true);
            }
        }
    }
}
