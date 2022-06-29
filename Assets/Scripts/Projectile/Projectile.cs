using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _shot = false;
    private Rigidbody _rb;

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

    private float _initialYVelocity;
    private Vector3 _shootDirection;

    private int _damage;
    private bool _mindControl;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ShootAt(Vector3 target, int damage, bool mindControl = false)
    {
        if (_shot) return;

        _mindControl = mindControl;
        _damage = damage;

        Vector3 direction = target - transform.position;
        _shot = true;

        // Physics
        float gravityEnhancer = 0.09f;
        float gravity = Physics.gravity.magnitude + gravityEnhancer; 
        float projX = new Vector2(direction.x, direction.z).magnitude;
        float y = direction.y;

        float velocity = Mathf.Sqrt((25f * projX) / 0.6f); // R = v^2 * sin(2*angle*) / g

        float throwAngleTangent = (Mathf.Pow(velocity, 2f) - Mathf.Sqrt(Mathf.Pow(velocity, 4f) -
            gravity * (gravity * Mathf.Pow(projX, 2f) + 2 * y * Mathf.Pow(velocity, 2f)))) /
            (gravity * projX);

        _shootDirection = Vector3.Normalize(new Vector3(direction.x, projX * throwAngleTangent, direction.z));
        transform.rotation = Quaternion.LookRotation(_shootDirection);

        Vector3 shootVelocity = _shootDirection * velocity;
        _initialYVelocity = shootVelocity.y;

        _rb.AddForce(shootVelocity, ForceMode.VelocityChange);
    }

    private void Update()
    {
        if (!_shot) return;

        transform.rotation = Quaternion.LookRotation(new Vector3(_shootDirection.x,
            (_rb.velocity.y * _shootDirection.y) / _initialYVelocity, _shootDirection.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_mindControl && other.CompareTag("MindControlled")) return;     

        // If colliding with a player layer or enemy layer and is mind controlled
        if ((playerLayers.value & (1 << other.gameObject.layer)) > 0 || 
            (_mindControl && (enemyLayers.value & (1 << other.gameObject.layer)) > 0))
        {
            Hittable hittable = other.gameObject.GetComponent<Hittable>();

            if (hittable == null)
            {
                Debug.LogWarning("Projectile collided with entity, but it doesn't have a Hittable component!");
            }
            else
            {
                hittable.Hit(_damage);
            }
            Destroy(gameObject);
        }
        else if ((_groundLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }
    }
}
