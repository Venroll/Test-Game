
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _player;

    [Header("Detection")]
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _viewAngle = 120f;

    [Header("Raycast")]
    [SerializeField] private Transform _eyes;
    [SerializeField] private LayerMask _obstacleLayers;

    private void Awake()
    {
        if (_eyes == null)
            _eyes = transform;
        if (_player == null)
        {
            _player = FindAnyObjectByType<PlayerHealth>().transform;
        }
    }

    public Transform FindVisibleTarget()
    {
        if (_player == null)
            return null;

        return CanSeeTarget(_player) ? _player : null;
    }

    public bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        Vector3 origin = _eyes.position;
        Vector3 targetPosition = target.position + Vector3.up;
        Vector3 direction = targetPosition - origin;

        if (direction.magnitude > _viewDistance) return false;

        float angle = Vector3.Angle(_eyes.forward, direction);

        if (angle > _viewAngle * 0.5f) return false;
        if (_obstacleLayers.value != 0 && Physics.Raycast(origin, direction.normalized, direction.magnitude, _obstacleLayers))
        {
            return false;
        }

        return true;
    }
}
