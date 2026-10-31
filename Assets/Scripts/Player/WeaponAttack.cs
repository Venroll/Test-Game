using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAttack : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _attackTrigger = "Attack";

    [Header("Attack")]
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float hitRadius = 0.6f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask targetLayers;

    [Header("Rage")]
    [SerializeField] private PlayerRage _playerRage;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private float _lifestealPercent = 0.3f;

    private GameAgent gameAgent;
    private bool isAttacking;
    private bool isGettingHit;
    private float nextAttackTime;

    private void Awake()
    {
        if (gameAgent == null)
            gameAgent = GetComponentInParent<GameAgent>();
        if (_playerRage == null)
            _playerRage = GetComponentInParent<PlayerRage>();
        if (_playerHealth == null)
            _playerHealth = GetComponentInParent<PlayerHealth>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (isAttacking || isGettingHit || Time.time < nextAttackTime)
            return;

        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        _animator.SetTrigger(_attackTrigger);
    }
    public void DealDamage()
    {
        Vector3 origin = attackPoint != null ? attackPoint.position : transform.position + Vector3.up;

        Vector3 hitPosition = origin + transform.forward * _attackRange;

        Collider[] hits = Physics.OverlapSphere(hitPosition,hitRadius,targetLayers);

        HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

        foreach (Collider hit in hits)
        {
            var target = hit.GetComponent<IDamagable>();

            if (target == null || target.IsDead)
                continue;

            if (hit.transform.root == transform.root)
                continue;

            if (!damagedTargets.Add(target))
                continue;

            target.TakeDamage(_damage, gameAgent);

            _playerRage?.AddRageFromDamageDealt(_damage);

            if (_playerRage != null && _playerRage.IsBerserk)
            {
                float healAmount = _damage * _lifestealPercent;
                _playerHealth?.Heal(healAmount);
            }
        }
    }
    public void AttackFinished()
    {
        isAttacking = false;
    }
    public void StartHitReaction()
    {
        isAttacking = false;
        isGettingHit = true;

        _animator.ResetTrigger(_attackTrigger);
    }
    public void FinishHitReaction()
    {
        isGettingHit = false;
    }
}