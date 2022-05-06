using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _shot = false;
    private Rigidbody _rb;
    

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
        var throwDirection = Vector3.Normalize(new Vector3(direction.x, proj_x * throwAngleTangent, direction.z));

        _rb.AddForce(throwDirection * velocity, ForceMode.VelocityChange);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
