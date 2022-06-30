using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : EntitySounds
{
    private float _timeToEnd = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeToEnd -= Time.deltaTime;
        if (_timeToEnd <= 0)
        {
            Play3DSound("Menu/Background_music");
            _timeToEnd = 35f;
        }
    }
}
