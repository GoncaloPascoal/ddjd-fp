using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance footsteps;
    
    [SerializeField] private int numberFootsteps = 5;
    [SerializeField] private int numberAttacks = 5;
    [SerializeField] private int numberRolls = 6;
    [SerializeField] private int numberSpells = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FootstepSound()
    {
        int randomAnimation = Random.Range(1, numberFootsteps + 1);
        
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/character/Footsteps/footsteps_" + randomAnimation);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }

    public void AttackSound()
    {
        int randomAnimation = Random.Range(1, numberAttacks + 1);
        
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/character/Blade_Slash/Slash_" + randomAnimation);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }

    public void RollSound()
    {
        int randomAnimation = Random.Range(1, numberRolls + 1);
        
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/character/Dodge/Roll_" + randomAnimation);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }

    public void PlayResurrectSound()
    {
        int randomAnimation = Random.Range(1, numberSpells + 1);
        
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/character/mind control/Spell_" + randomAnimation);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }
}
