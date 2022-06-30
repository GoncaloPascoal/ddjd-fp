using UnityEngine;

public class DoorSounds : EntitySounds
{
    public void OpenDoorSound()
    {
        Play3DSound("Ambient/Door/Door_open");
    }
    
    public void CloseDoorSound()
    {
        Play3DSound("Ambient/Door/Door_close");
    }
}