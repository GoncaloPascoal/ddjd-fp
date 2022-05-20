using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _shot = false;
    private Rigidbody _rb;
    
    [Header("Stats")] [SerializeField] private int damage = 1;
    
    [Header("Layers")]
    [SerializeField]
    [Tooltip("What layers are considered as the player?")]
    private LayerMask playerLayers;

    [Tooltip("What other layers can this collide with")]
    private LayerMask _groundLayers;

    private float initialYVelocity;
    private Vector3 throwDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShootAt(Vector3 target)
    {
        if (_shot) return;

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
    void Update()
    {
        if (!_shot) return;

        transform.rotation = Quaternion.LookRotation(new Vector3(throwDirection.x,
            (_rb.velocity.y * throwDirection.y) / initialYVelocity, throwDirection.z));
    }
    
    void OnTriggerEnter(Collider col)
    {
        // If colliding with a player layer
        if ((playerLayers.value & (1 << col.gameObject.layer)) > 0)
        {
            Hittable hitScript = col.gameObject.GetComponent<Hittable>();
            if (hitScript == null)
            {
                Debug.LogWarning("Projectile collided with player, but player doesn't have a Hittable component!");
            }
            else
            {
                hitScript.GetHit(damage);
                Destroy(gameObject);
            }
        }
        else if ((_groundLayers.value & (1 << col.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }
    }

}
