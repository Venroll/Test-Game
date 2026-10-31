
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

    [SerializeField] private EnemyArenaManager _arenaManager;

    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _losePlayerDelay = 3f;

    private bool _isInCombat;
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

        if (_currentState == EnemyState.Dead)
            return;

        UpdateTarget();
        UpdateState();
        UpdateAnimation();
    }

    private void UpdateTarget()
    {
        if (_target != null)
        {
            if (_detection != null && _detection.CanSeeTarget(_target))
            {
                _lastSeenTime = Time.time;
                return;
            }

            if (Time.time - _lastSeenTime >= _losePlayerDelay)
            {
                _target = null;
            }

            return;
        }

        Transform visibleTarget = _detection != null
            ? _detection.FindVisibleTarget()
            : null;

        if (visibleTarget != null)
        {
            _target = visibleTarget;
            _lastSeenTime = Time.time;
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

        float distanceSqr = (transform.position - _target.position).sqrMagnitude;

        if (distanceSqr <= _attackRange * _attackRange)
        {
            ChangeState(EnemyState.Attack);
            _combat?.Tick(_target);
        }
        else
        {
            ChangeState(EnemyState.Chase);

            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_target.position);
            }
        }
    }

    private void UpdateAnimation()
    {
        if (_enemyAnim == null)
            return;

        float speed = _agent != null && _agent.isOnNavMesh
            ? _agent.velocity.magnitude
            : 0f;

        _enemyAnim.SetSpeed(speed);
    }

    private void ChangeState(EnemyState newState)
    {
        if (_currentState == newState)
            return;

        bool wasInCombat =
        _currentState == EnemyState.Chase ||
        _currentState == EnemyState.Attack;

        bool willBeInCombat =
            newState == EnemyState.Chase ||
            newState == EnemyState.Attack;

        _currentState = newState;

        if (!wasInCombat && willBeInCombat)
            EnterCombat();
        else if (wasInCombat && !willBeInCombat)
            ExitCombat();

        switch (_currentState)
        {
            case EnemyState.Patrol:
                _combat?.StopCombat();

                if (_agent != null)
                    _agent.isStopped = false;
                break;

            case EnemyState.Chase:
                _combat?.StopCombat();

                if (_agent != null)
                    _agent.isStopped = false;
                break;

            case EnemyState.Attack:
                if (_agent != null)
                    _agent.isStopped = true;

                _combat?.StartCombat();
                break;

            case EnemyState.Dead:
                _target = null;

                if (_agent != null)
                {
                    _agent.isStopped = true;
                    _agent.ResetPath();
                }

                _combat?.StopCombat();
                _enemyAnim?.PlayDeath();
                break;
        }
    }
    private void EnterCombat()
    {
        if (_isInCombat)
            return;

        _isInCombat = true;
        _arenaManager?.EnemyEnteredCombat();
    }

    private void ExitCombat()
    {
        if (!_isInCombat)
            return;

        _isInCombat = false;
        _arenaManager?.EnemyLeftCombat();
    }
}
