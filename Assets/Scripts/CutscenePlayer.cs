using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    private PlayableDirector _timeline;
    
    [SerializeField] private bool skippable = true;
    [SerializeField] private bool managesObjects = true;

    public GameObject cutsceneElements;

    public List<GameObject> neededInCutscene;

    public List<GameObject> objectsToDestroy;
    public List<GameObject> objectsNotToSpawn;

    private void Awake()
    {
        _timeline = GetComponent<PlayableDirector>();

        if (!managesObjects) return;
        
        foreach (var objectToDestroy in objectsToDestroy)
        {
            Destroy(objectToDestroy);
        }

        foreach (var objectInScene in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (objectInScene.transform.parent == null)
            {
                if (neededInCutscene.Contains(objectInScene) || objectInScene == cutsceneElements)
                {
                    objectInScene.SetActive(true);
                }
                else if (objectInScene != gameObject)
                {
                    objectInScene.SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        if (skippable && InputManager.Action("Cancel").WasPressedThisFrame())
        {
            CutsceneEnd();
        }
    }

    public void CutsceneEnd()
    {
        if (!managesObjects) return;
        
        Destroy(cutsceneElements);

        foreach (var objectInScene in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (objectInScene.transform.parent == null 
                && !objectInScene.activeInHierarchy 
                && !objectInScene.CompareTag("Cutscene")
                && !objectsNotToSpawn.Contains(objectInScene))
            {
                objectInScene.SetActive(true);
            }
        }
        
        Destroy(gameObject);
    }

    public void CutsceneEndChangeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
