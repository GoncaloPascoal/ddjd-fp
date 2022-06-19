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
    [SerializeField] private int numberJumps = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play3DSound(string soundEvent, int volume = 1)
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance(soundEvent);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.setVolume(volume);
        footsteps.start();
        footsteps.release();
    }

    public void FootstepSound()
    {
        int randomAnimation = Random.Range(1, numberFootsteps + 1);
        
        Play3DSound("event:/character/Footsteps/footsteps_" + randomAnimation);
    }

    public void JumpStartSound()
    {
        int randomAnimation = Random.Range(1, numberJumps + 1);
        
        Play3DSound("event:/character/Jump/Jump_up_" + randomAnimation, 1);
    }

    public void JumpLandSound()
    {
        int randomAnimation = Random.Range(1, numberJumps + 1);
        
        Play3DSound("event:/character/Jump/Jump_down_" + randomAnimation);
    }

    public void AttackSound()
    {
        int randomAnimation = Random.Range(1, numberAttacks + 1);
        
        Play3DSound("event:/character/Blade_Slash/Slash_" + randomAnimation);
    }

    public void RollSound()
    {
        int randomAnimation = Random.Range(1, numberRolls + 1);
        
        Play3DSound("event:/character/Dodge/Roll_" + randomAnimation);
    }

    public void PlayResurrectSound()
    {
        int randomAnimation = Random.Range(1, numberSpells + 1);
        
        Play3DSound("event:/character/mind control/Spell_" + randomAnimation);
    }
}
