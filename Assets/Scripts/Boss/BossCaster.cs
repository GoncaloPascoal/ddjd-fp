using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossCaster : MonoBehaviour
{
    [SerializeField] private GameObject aoePrefab;
    [SerializeField] private float aoeInitialDistance = 0.5f;
    [SerializeField] private int aoeNumberOfParts = 10;

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwordAOE()
    {
        var direction = transform.forward.normalized;
        var bossPosition = transform.position;
        var position = new Vector3(bossPosition.x, bossPosition.y, bossPosition.z);
        
        var newAoeObject = Instantiate(aoePrefab, 
            position + direction * aoeInitialDistance, 
            Quaternion.LookRotation(direction));
        newAoeObject.GetComponent<SwordAOE>().InitializeWithDirection(direction, aoeNumberOfParts);
    }
}
