
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _player;

    [Header("Detection")]
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _viewAngle = 120f;

    [Header("Rear Detection")]
    [SerializeField] private float _rearViewDistance = 2f;
    [SerializeField] private float _rearViewAngle = 90f;
    [SerializeField] private float _rearDetectionTime = 1f;

    [Header("Raycast")]
    [SerializeField] private Transform _eyes;
    [SerializeField] private LayerMask _obstacleLayers;

    private float _rearDetectionTimer;

    private void Awake()
    {
        if (_eyes == null)
            _eyes = transform;

        if (_player == null)
        {
            PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

            if (playerHealth != null)
                _player = playerHealth.transform;
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
        if (target == null)
        {
            _rearDetectionTimer = 0f;
            return false;
        }

        Vector3 origin = _eyes.position;
        Vector3 targetPosition = target.position + Vector3.up;
        Vector3 direction = targetPosition - origin;

        if (direction.sqrMagnitude > _viewDistance * _viewDistance)
        {
            _rearDetectionTimer = 0f;
            return false;
        }

        if (_obstacleLayers.value != 0 &&
            Physics.Raycast(origin, direction.normalized,
                direction.magnitude, _obstacleLayers))
        {
            _rearDetectionTimer = 0f;
            return false;
        }

        float frontAngle = Vector3.Angle(_eyes.forward, direction);

        if (frontAngle <= _viewAngle * 0.5f)
        {
            _rearDetectionTimer = 0f;
            return true;
        }


        float rearAngle = Vector3.Angle(-_eyes.forward, direction);

        if (direction.sqrMagnitude <= _rearViewDistance * _rearViewDistance &&
            rearAngle <= _rearViewAngle * 0.5f)
        {
            _rearDetectionTimer += Time.deltaTime;

            if (_rearDetectionTimer >= _rearDetectionTime)
                return true;
        }
        else
        {
            _rearDetectionTimer = 0f;
        }

        return false;
    }
}
