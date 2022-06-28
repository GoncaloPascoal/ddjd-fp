using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySounds : MonoBehaviour
{
    [SerializeField] private List<string> hitSounds;
    [SerializeField] private List<string> attackSounds;
    [SerializeField] private List<string> deathSounds;
    
    protected void Play3DSound(string soundEvent, int volume = 1)
    {
        FMOD.Studio.EventInstance sEventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/" + soundEvent);
        sEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        sEventInstance.setVolume(volume);
        sEventInstance.start();
        sEventInstance.release();
    }

    public void GetHitSound(int chanceToPlay)
    {
        if (!(Random.Range(0, 100) < chanceToPlay))
            return;

        if (hitSounds.Count > 0)
        {
            Play3DSound(hitSounds[Random.Range(0, hitSounds.Count)]);
        }
    }

    public void AttackSound()
    {
        if (attackSounds.Count > 0)
            Play3DSound(attackSounds[Random.Range(0, attackSounds.Count)]);
    }

    public void DeathSound()
    {
        if (deathSounds.Count > 0)
            Play3DSound(deathSounds[Random.Range(0, deathSounds.Count)]);
    }

    public void LowHealth()
    {
        Play3DSound("character/Low_Stamina");
    }
}
