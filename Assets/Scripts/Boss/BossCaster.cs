using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossCaster : MonoBehaviour
{
    [SerializeField] private GameObject aoePrefab;
    [SerializeField] private float aoeInitialDistance = 0.5f;
    [SerializeField] private int aoeNumberOfParts = 10;

    [SerializeField] private GameObject spellProjectilePrefab;

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

    public void CastProjectilesPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        CastProjectiles(player);
    }

    public void CastProjectiles(GameObject target)
    {
        var offset = transform.right;
        var firstPosition = transform.position + Vector3.up * 3 + offset * 2;
        
        for (int i = 0; i < 4; ++i)
        {
            var timeBeforeShooting = Random.Range(2f, 5f);
            var position = firstPosition - offset * i;
            var newProjectile = Instantiate(spellProjectilePrefab, transform.position, transform.rotation);
            newProjectile.GetComponent<ProjectileSpell>().InitializeWithPositionTarget(position, target, timeBeforeShooting);
        }
    }
}
