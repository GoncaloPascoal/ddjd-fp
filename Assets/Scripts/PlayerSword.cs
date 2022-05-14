using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSword : MonoBehaviour
{

    //[SerializeField] private Vector3 outPos;
    [SerializeField]
    private Vector3 startRot;
    [SerializeField]
    private Vector3 endRot;

    [SerializeField] private float swingTime;
    private float _curSwingTime;
    
    [SerializeField] private float cooldown;
    private float _curCooldown;

    private Vector3 _curRot;

    private StarterAssetsInputs starterAssetsInputs;
    
    private bool _swingingSword;

    private MeshRenderer _renderer;
    private Collider _col;

    void Awake()
    {
    }
    
    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = transform.parent.gameObject.GetComponent<StarterAssetsInputs>();
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
            _curRot = (endRot - startRot) * (_curSwingTime / swingTime) + startRot;
            transform.rotation = Quaternion.Euler(_curRot);
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
                _curRot = startRot;
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

    void Swing()
    {
        
    }

    void StartSwing()
    {
        _renderer.enabled = true;
        _col.enabled = true;
        _swingingSword = true;
    }
}
