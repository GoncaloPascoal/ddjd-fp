using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FloatingSoulScript : MonoBehaviour
{
    private GameObject _enemy;
    private Animator _animator;
    [SerializeField] private Bar _bar;
    [SerializeField] private float resurrectionTime = 10f;
    private DamageableEnemy _damageable;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.transform.parent.gameObject;
        _animator = _enemy.GetComponent<Animator>();
        _damageable = _enemy.GetComponent<DamageableEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        resurrectionTime -= Time.deltaTime;
        if (resurrectionTime <= 0)
        {
            _damageable.DeleteAnimator();
            _damageable.DeleteComps();
            Destroy(gameObject);
        }
    }

    public void EndResurrection()
    {
        _enemy.GetComponent<Enemy>().MindControl();
        _bar.gameObject.SetActive(true);
        _enemy.GetComponent<Hittable>().enabled = true;
        _enemy.GetComponent<Attacker>().enabled = true;
        _damageable.enabled = true;
        _damageable.ChangeHealth(_damageable.MaxHealth/2);

        foreach (var comp in _damageable.gameObject.GetComponents(typeof(CapsuleCollider)))
        {
            ((CapsuleCollider) comp).enabled = true;
        }
        _animator.applyRootMotion = false;
        Destroy(gameObject);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && InputManager.GetButtonDown("Interact"))
        {
            _animator.SetTrigger("Resurrect");
        }
    }
}
