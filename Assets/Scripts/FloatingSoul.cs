using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FloatingSoul : MonoBehaviour
{
    [SerializeField] private Bar _bar;
    [SerializeField] private float resurrectionTime = 10f;

    private static readonly string[] PromptButtons = { "E", "(A)" };
    private const string PromptAction = "Resurrect the Dead";

    private GameObject _enemy;
    private Animator _enemyAnimator;
    private ThirdPersonController _player;
    private Attacker _attacker;
    private Staggerable _staggerable;
    private DamageableEnemy _damageable;

    private bool _promptEnabled;
    private bool _playerInRange = false;
    private bool _isBeingResurrected = false;
    
    private void Start()
    {
        _promptEnabled = true;
        _enemy = gameObject.transform.parent.gameObject;

        _enemyAnimator = _enemy.GetComponent<Animator>();
        _damageable = _enemy.GetComponent<DamageableEnemy>();
        _attacker = _enemy.GetComponent<Attacker>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
        _staggerable = _enemy.GetComponent<Staggerable>();
    }

    private void Update()
    {
        if (_isBeingResurrected) return;

        if (_playerInRange && InputManager.Action("Interact").WasPressedThisFrame())
        {
            _player.StartResurrection(_enemyAnimator);
            _promptEnabled = false;
            _isBeingResurrected = true;
            HUD.Instance.HideButtonPrompt();
            return;
        }

        resurrectionTime -= Time.deltaTime;
        if (resurrectionTime <= 0)
        {
            _damageable.DeleteAnimator();
            _damageable.DeleteComponents();
            if (_promptEnabled && _playerInRange) HUD.Instance.HideButtonPrompt();
            Destroy(gameObject);
        }
    }

    public void EndResurrection()
    {
        _bar.gameObject.SetActive(true);

        _enemy.GetComponent<Hittable>().enabled = true;
        if (_attacker != null){
            _attacker.enabled = true; // Ranged enemy does not have an attacker
            _attacker.EndAttack();
        }
        if (_staggerable != null) _staggerable.ActivateStagger();

        _enemy.transform.Find("Backstab").gameObject.GetComponent<BoxCollider>().enabled = true;
        _damageable.enabled = true;
        _damageable.ChangeHealth(_damageable.MaxHealth / 2);
        _enemy.GetComponent<Enemy>().MindControl();
        _enemyAnimator.applyRootMotion = false;

        foreach (Component comp in _damageable.gameObject.GetComponents(typeof(CapsuleCollider)))
        {
            ((CapsuleCollider) comp).enabled = true;
        }

        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_promptEnabled && other.CompareTag("Player"))
        {
            HUD.Instance.ShowButtonPrompt(PromptButtons, PromptAction);
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_promptEnabled && other.CompareTag("Player"))
        {
            HUD.Instance.HideButtonPrompt();
            _playerInRange = false;
        }
    }
}
