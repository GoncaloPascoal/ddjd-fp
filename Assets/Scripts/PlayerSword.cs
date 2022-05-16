using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSword : MonoBehaviour
{

    //[SerializeField] private Vector3 outPos;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 startRot;
    [SerializeField] private Vector3 endRot;

    [SerializeField] private float swingTime;
    private float _curSwingTime;
    
    [SerializeField] private float cooldown;
    private float _curCooldown;

    private StarterAssetsInputs starterAssetsInputs;
    
    private bool _swingingSword;

    private MeshRenderer _renderer;
    private Collider _col;

    private Vector3 angleCrossProduct;

    void Awake()
    {
    }
    
    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = transform.parent.parent.gameObject.GetComponent<StarterAssetsInputs>();
        _renderer = GetComponent<MeshRenderer>();
        _col = GetComponent<Collider>();
        _swingingSword = false;
        EndSwing();
    }

    // Update is called once per frame
    void Update()
    {
        _curCooldown -= Time.deltaTime;
        if (_swingingSword)
        {
            _curSwingTime -= Time.deltaTime;
            var deltaAngle = Time.deltaTime * (endRot - startRot).magnitude / swingTime;
            transform.RotateAround(transform.parent.position, Vector3.up, deltaAngle);
            if (_curSwingTime <= 0)
            {
                EndSwing();
            }
        }
        
        if (starterAssetsInputs.swing)
        {
            if (!_swingingSword && _curCooldown <= 0)
            {
                StartSwing();
                _swingingSword = true;
                _curSwingTime = swingTime;
                _curCooldown = cooldown;
            }
        }
    }

    void EndSwing()
    {
        _renderer.enabled = false;
        _col.enabled = false;
        _swingingSword = false;
    }

    void StartSwing()
    {
        _renderer.enabled = true;
        _col.enabled = true;
        _swingingSword = true;
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.RotateAround(transform.parent.position, Vector3.up, startRot.y);
    }
}
