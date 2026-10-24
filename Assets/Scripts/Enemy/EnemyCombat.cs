using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private GameAgent _gameAgent;

    [Header("Attack")]
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _attackRadius = 0.8f;
    [SerializeField] private float _attackCooldown = 1.2f;
    [SerializeField] private float _attackDistance = 2f;

    private Transform _target;
    private float _nextAttackTime;
    private bool _isAttacking;
    private bool _combatActive;

    public void StartCombat()
    {
        _combatActive = true;
    }
    public void StopCombat()
    {
        _combatActive = false;
        _isAttacking = false;
        _target = null;
    }

    public void Tick(Transform target)
    {
        if (!_combatActive || target == null || _isAttacking) return;
        _target = target;
        if (Vector3.Distance(transform.position, target.position) > _attackDistance) return;
        if (Time.time < _nextAttackTime) return;

        _isAttacking = true;
        _nextAttackTime = Time.time + _attackCooldown;

        FaceTarget(target);
        _enemyAnimator?.PlayAttack();
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        if (direction.magnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void DealDamage()
    {
        if (!_combatActive || _target == null) return;

        if (Vector3.Distance(transform.position, _target.position) > _attackDistance + 0.5f) return;

        Vector3 origin = _attackPoint != null ? _attackPoint.position : transform.position + Vector3.up;

        Collider[] hits = Physics.OverlapSphere(origin, _attackRadius, _targetLayers);
        HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

        foreach (Collider hit in hits)
        {
            IDamagable targetHealth = hit.GetComponent<IDamagable>();
            if (targetHealth == null || targetHealth.IsDead) continue;

            targetHealth.TakeDamage(_damage, _gameAgent);
        }
    }

    public void AttackFinished()
    {
        _isAttacking = false;
    }
}
