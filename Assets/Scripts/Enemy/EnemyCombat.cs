
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Rotation")]
    [SerializeField] private Transform _enemyVisuals;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private float _rotationThreshold = 5f;

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
        if (!_combatActive || target == null || _isAttacking)
            return;

        _target = target;

        FaceTarget(target);

        Vector3 direction = target.position - _enemyVisuals.position;
        direction.y = 0f;

        float angle = Vector3.Angle(_enemyVisuals.forward, direction);

        if (angle > _rotationThreshold)
            return;

        if ((transform.position - target.position).sqrMagnitude > _attackDistance * _attackDistance)
            return;

        if (Time.time < _nextAttackTime)
            return;

        _isAttacking = true;
        _nextAttackTime = Time.time + _attackCooldown;

        _enemyAnimator?.PlayAttack();
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = target.position - _enemyVisuals.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _enemyVisuals.rotation = Quaternion.RotateTowards(
            _enemyVisuals.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    public void DealDamage()
    {
        if (!_combatActive || _target == null)
            return;

        float maxDistance = _attackDistance + 0.5f;

        if ((transform.position - _target.position).sqrMagnitude >
            maxDistance * maxDistance)
            return;

        Vector3 origin = _attackPoint != null
            ? _attackPoint.position
            : transform.position + Vector3.up;

        Collider[] hits = Physics.OverlapSphere(
            origin,
            _attackRadius,
            _targetLayers
        );

        HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

        foreach (Collider hit in hits)
        {
            IDamagable targetHealth = hit.GetComponent<IDamagable>();

            if (targetHealth == null || targetHealth.IsDead)
                continue;

            if (!damagedTargets.Add(targetHealth))
                continue;

            targetHealth.TakeDamage(_damage, _gameAgent);
        }
    }

    public void AttackFinished()
    {
        _isAttacking = false;
    }
    public void CancelAttack()
    {
        _isAttacking = false;
        _enemyAnimator?.ResetAttackTrigger();
    }
}
