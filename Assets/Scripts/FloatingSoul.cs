using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FloatingSoul : MonoBehaviour
{
    private GameObject _enemy;
    private Animator _enemyAnimator;
    private ThirdPersonController _player;
    private Attacker _attacker;
    [SerializeField] private Bar _bar;
    [SerializeField] private float resurrectionTime = 10f;
    private DamageableEnemy _damageable;

    private static readonly string[] PromptButtons = { "E", "(A)" };
    private const string PromptAction = "Resurrect the Dead";
    private bool _promptEnabled;

    private void Start()
    {
        _promptEnabled = true;
        _enemy = gameObject.transform.parent.gameObject;
        _enemyAnimator = _enemy.GetComponent<Animator>();
        _damageable = _enemy.GetComponent<DamageableEnemy>();
        _attacker = _enemy.GetComponent<Attacker>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        resurrectionTime -= Time.deltaTime;
        if (resurrectionTime <= 0)
        {
            _damageable.DeleteAnimator();
            _damageable.DeleteComponents();
            Destroy(gameObject);
        }
    }

    public void EndResurrection()
    {
        _bar.gameObject.SetActive(true);
        _enemy.GetComponent<Hittable>().enabled = true;
        if(_attacker != null) _attacker.enabled = true; //ranged enemy does not have an attacker
        _enemy.transform.Find("Backstab").gameObject.GetComponent<BoxCollider>().enabled = true;
        _damageable.enabled = true;
        _damageable.ChangeHealth(_damageable.MaxHealth / 2);
        _attacker.EndAttack();
        _enemy.GetComponent<Enemy>().MindControl();

        foreach (var comp in _damageable.gameObject.GetComponents(typeof(CapsuleCollider)))
        {
            ((CapsuleCollider) comp).enabled = true;
        }
        _enemyAnimator.applyRootMotion = false;
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_promptEnabled && other.CompareTag("Player"))
        {
            HUD.Instance.ShowButtonPrompt(PromptButtons, PromptAction);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && InputManager.Action("Interact").WasPressedThisFrame())
        {
            _player.StartResurrection(_enemyAnimator);
            _promptEnabled = false;
            HUD.Instance.HideButtonPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_promptEnabled && other.CompareTag("Player")) HUD.Instance.HideButtonPrompt();
    }
}
