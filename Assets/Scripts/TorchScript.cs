using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : EntitySounds
{
    private string soundType;

    private float timeToEnd = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(1, 3) == 1) soundType = "Ambient/Torch/Torch_D";
        else soundType = "Ambient/Torch/Torch_F";
    }

    // Update is called once per frame
    void Update()
    {
        timeToEnd -= Time.deltaTime;
        if (timeToEnd <= 0)
        {
            Play3DSound(soundType, 0.005f);
            timeToEnd = 15f; // both torch sounds have 15 seconds duration
        }
    }
}
