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
    
    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetHit(int damage)
    {
        curHp -= damage;
        Debug.Log("Ouch! Current HP: " + curHp + ".");
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
}
