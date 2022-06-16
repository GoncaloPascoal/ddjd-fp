using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _shot = false;
    private Rigidbody _rb;
    
    [Header("Stats")] [SerializeField] private int damage = -10;
    
    [Header("Layers")]
    [SerializeField]
    [Tooltip("What layers are considered as the player?")]
    private LayerMask playerLayers;
    
    [SerializeField]
    [Tooltip("What layers are considered as the enemy?")]
    private LayerMask enemyLayers;

    [SerializeField]
    [Tooltip("What other layers can this collide with")]
    private LayerMask _groundLayers;

    private float initialYVelocity;
    private Vector3 throwDirection;
    
    private bool mindControl = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ShootAt(Vector3 target, bool isMindControlled = false)
    {
        if (_shot) return;

        mindControl = isMindControlled;

        var direction = target - transform.position;
        _shot = true;

        // physics :D
        var gravity = Physics.gravity.magnitude;
        var proj_x = new Vector2(direction.x, direction.z).magnitude;
        var y = direction.y;
        
        var velocity = Mathf.Sqrt((25f * proj_x) / 0.6f); // R = v^2 * sin(2*angle*) / g

        float throwAngleTangent = (Mathf.Pow(velocity, 2f) - Mathf.Sqrt(Mathf.Pow(velocity, 4f) -
                                                                        gravity * (gravity * Mathf.Pow(proj_x, 2f) + 2 * y * Mathf.Pow(velocity, 2f)))) /
                                  (gravity * proj_x);
        

        // var throwDirection = Vector3.Normalize(new Vector3(direction.x, proj_x * throwAngleTangent, direction.z)); 
        throwDirection = Vector3.Normalize(new Vector3(direction.x, proj_x * throwAngleTangent, direction.z));

        transform.rotation = Quaternion.LookRotation(throwDirection);

        var shootVelocity = throwDirection * velocity;
        initialYVelocity = shootVelocity.y;

        _rb.AddForce(shootVelocity, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_shot) return;

        transform.rotation = Quaternion.LookRotation(new Vector3(throwDirection.x,
            (_rb.velocity.y * throwDirection.y) / initialYVelocity, throwDirection.z));
    }
    
    private void OnTriggerEnter(Collider col)
    {

        if (mindControl && col.CompareTag("MindControlled")) return;       
        // If colliding with a player layer 
        // or enemy layer and is mind controlled
        if ((playerLayers.value & (1 << col.gameObject.layer)) > 0 || 
            (mindControl && (enemyLayers.value & (1 << col.gameObject.layer)) > 0))
        {
            Damageable damageable = col.gameObject.GetComponent<Damageable>();
            
            if (damageable == null)
            {
                Debug.LogWarning("Projectile collided with entity, but it doesn't have a Damageable component!");
            }
            else
            {
                Debug.Log("Hit");
                Debug.Log(damageable);
                damageable.ChangeHealth(damage);
            }
            Destroy(gameObject);
        }
        else if ((_groundLayers.value & (1 << col.gameObject.layer)) > 0)
        {
            Debug.Log("Hit terrain");
            Destroy(gameObject);
        }
    }
}
