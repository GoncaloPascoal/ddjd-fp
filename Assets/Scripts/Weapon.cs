using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private BoxCollider _collider;

    [SerializeField] 
    private GameObject wielder;
    private Stats _wielderStats;
    private Animator _wielderAnimator;

    private readonly ISet<GameObject> _alreadyHit = new HashSet<GameObject>();

    [SerializeField] private List<string> targetsTags;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        
        _wielderStats = wielder.GetComponent<Stats>();
        _wielderAnimator = wielder.GetComponent<Animator>();
        _alreadyHit.Clear();
    }

    public void Attack()
    {
        _collider.enabled = true;
        _alreadyHit.Clear();
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }
    
    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void HitObstacle(Collider obstacle)
    {
        // Only hit if the collider is not part of the weapon wielder layer
        // (if player is wielding weapon, they can not hit themselves or other players
        // and enemies cannot hit each other)
        if (wielder.transform.root != obstacle.gameObject.transform.root && targetsTags.Contains(obstacle.tag))
        {
            Hittable hittable = obstacle.GetComponentInParent<Hittable>();
            if (hittable != null && !_alreadyHit.Contains(obstacle.gameObject.transform.root.gameObject))
            {
                hittable.Hit(_wielderStats.GetStatValue(StatName.Damage));
                _alreadyHit.Add(obstacle.gameObject.transform.root.gameObject);
            }
        }
    }

    public void SetTargetTags(List<string> newTags)
    {
        targetsTags = newTags;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_collider.enabled) HitObstacle(other);
    }
}
