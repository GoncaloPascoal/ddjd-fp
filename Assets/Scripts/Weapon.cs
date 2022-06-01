using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] 
    private int baseDamage;
    
    private BoxCollider _collider;

    [SerializeField] 
    private GameObject wielder;

    private Animator _wielderAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _wielderAnimator = wielder.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wielderAnimator == null)
            return;

        _collider.enabled = _wielderAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackNormal");
    }

    public void Attack()
    {
        _collider.enabled = true;
    }

    private void HitObstacle(Collider obstacle)
    {
        // only hit if the collider is not part of the weapon wilder layer
        // (if player is wielding weapon, they can not hit themselves or other players
        // and enemies cannot hit each other)
        if (wielder.layer != obstacle.gameObject.layer)
        {
            var hittable = obstacle.GetComponent<Hittable>();

            if (hittable != null)
            {
                hittable.GetHit(baseDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HitObstacle(other);
    }
}
