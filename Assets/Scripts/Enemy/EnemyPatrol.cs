
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Route")]
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _pointReachDistance = 0.5f;

    private int _currentPointIndex;
    private bool _destinationSet;

    public void Tick()
    {
        if (_points == null || _points.Length == 0)
            return;

        if (_agent == null || !_agent.isOnNavMesh)
            return;

        if (_agent.isStopped)
            _agent.isStopped = false;

        if (!_destinationSet)
        {
            SetDestination();
            return;
        }

        if (_agent.pathPending)
            return;

        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _destinationSet = false;
            return;
        }

        if (_agent.hasPath &&
            _agent.remainingDistance <= _pointReachDistance)
        {
            _currentPointIndex =
                (_currentPointIndex + 1) % _points.Length;

            _destinationSet = false;
        }
    }

    private void SetDestination()
    {
        Transform point = _points[_currentPointIndex];

        if (point == null)
        {
            return;
        }

        bool success = _agent.SetDestination(point.position);

        if (success)
        {
            _destinationSet = true;
        }
        else
        {
            Debug.LogWarning(
                $"{name}: не удалось задать путь к точке {point.name}"
            );
        }
    }

    public void ResetPatrol()
    {
        _destinationSet = false;
    }
}
