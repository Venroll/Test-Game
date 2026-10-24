using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Dead
}

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyPatrol _patrol;
    [SerializeField] private EnemyDetection _detection;
    [SerializeField] private EnemyCombat _combat;
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyAnimator _enemyAnim;

    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _losePlayerDelay = 3f;

    private EnemyState _currentState = EnemyState.Patrol;
    private Transform _target;
    private float _lastSeenTime;

    public EnemyState CurrentState => _currentState;
    public Transform Target => _target;

    private void Update()
    {
        if (_health != null && _health.IsDead)
        {
            ChangeState(EnemyState.Dead);
        }
        if (_currentState == EnemyState.Dead) return;

        UpdateTarget();
        UpdateState();
        UpdateAnimation();
    }

    private void UpdateTarget()
    {
        if (_target != null && _detection.CanSeeTarget(_target))
        {
            _lastSeenTime = Time.time;
            return;
        }

        Transform visibleTarget = _detection != null ? _detection.FindVisibleTarget() : null;

        if (visibleTarget != null)
        {
            _target = visibleTarget;
            _lastSeenTime = Time.time;
            return;
        }

        if (_target != null && Time.time - _lastSeenTime >= _losePlayerDelay)
        {
            _target = null;
        }
    }

    private void UpdateState()
    {
        if (_target == null)
        {
            ChangeState(EnemyState.Patrol);
            _patrol?.Tick();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            _target.position
        );

        if (distance <= _attackRange)
        {
            ChangeState(EnemyState.Attack);
            _combat?.Tick(_target);
        }
        else
        {
            ChangeState(EnemyState.Chase);

            if (_agent != null && _agent.isOnNavMesh)
                _agent.SetDestination(_target.position);
        }
    }

    private void UpdateAnimation()
    {
        if (_enemyAnim == null)
            return;

        float speed = _agent != null && _agent.isOnNavMesh ? _agent.velocity.magnitude: 0f;

        _enemyAnim.SetSpeed(speed);
    }

    private void ChangeState(EnemyState newState)
    {
        if (_currentState == newState)
            return;

        _currentState = newState;

        switch (_currentState)
        {
            case EnemyState.Patrol:
                _combat?.StopCombat();
                _agent.isStopped = false;
                break;

            case EnemyState.Chase:
                _combat?.StopCombat();
                _agent.isStopped = false;
                break;

            case EnemyState.Attack:
                _agent.isStopped = true;
                _combat?.StartCombat();
                break;

            case EnemyState.Dead:
                _target = null;
                _agent.isStopped = true;
                _agent.ResetPath();
                _combat?.StopCombat();
                _enemyAnim?.PlayDeath();
                break;
        }
    }
}
