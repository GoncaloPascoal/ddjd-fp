using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundsScript : EntitySounds
{
    private float timeToPlay = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToPlay -= Time.deltaTime;
        if (timeToPlay <= 0)
        {
            int bugsSoundNumber = Random.Range(1, 7);
            Play3DSound("Ambient/Bugs/Bugs_" + bugsSoundNumber);
            
            int sandSoundNumber = Random.Range(1, 8);
            Play3DSound("Ambient/Sand/Sand fall_" + sandSoundNumber);

            timeToPlay = Random.Range(5, 11); // plays a random sound in a random interval between 5 and 10 seconds
        }
    }
}
