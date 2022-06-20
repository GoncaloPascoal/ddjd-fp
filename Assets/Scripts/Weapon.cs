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

    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void HitObstacle(Collider obstacle)
    {
        // Only hit if the collider is not part of the weapon wielder layer
        // (if player is wielding weapon, they can not hit themselves or other players
        // and enemies cannot hit each other)
        if (wielder.transform.root != obstacle.gameObject.transform.root)
        {
            Hittable hittable = obstacle.GetComponentInParent<Hittable>();
            if (hittable != null && !_alreadyHit.Contains(obstacle.gameObject.transform.root.gameObject))
            {
                hittable.Hit((int) _wielderStats.GetStatValue(StatName.Damage));
                _alreadyHit.Add(obstacle.gameObject.transform.root.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_collider.enabled)
            HitObstacle(other);
    }
}
