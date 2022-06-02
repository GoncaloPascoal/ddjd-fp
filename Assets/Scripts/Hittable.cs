using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Maximum HP.")]
    int maxHp;
    int curHp;
    private Enemy _enemy;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetHit(int damage)
    {
        curHp -= damage;
        Debug.Log("Ouch! Current HP: " + curHp + ".");
        
        if (curHp <= 0)
            Death();
    }

    public void GetHitBackstab(int damage)
    {
        curHp -= damage;
        Debug.Log("Ouch! Backstab! Current HP: " + curHp + ".");
        if (curHp < 0)
        {
            _enemy.mindControl();
        }
    }

    void Death()
    {
        if (gameObject.CompareTag("Player"))
        {
            return;
        }

        _animator.applyRootMotion = true;

        foreach (var comp in GetComponents(typeof(Component)))
        {
            if (comp != _animator && comp != transform && comp != this)
            {
                Destroy(comp);
            }
        }
        _animator.SetTrigger("Die");
    }

    public void EndDeath()
    {
        Destroy(_animator);
        Destroy(this);
    }
}
