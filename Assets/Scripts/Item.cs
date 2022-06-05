using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private new string name;
    
    void Start()
    {
    }

    public void Print()
    {
        Debug.Log(name);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

