using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private GameAgent _testAgent;
    private void Start()
    {
        _testAgent = GetComponent<GameAgent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var attacker)) {
            attacker.TakeDamage(10, _testAgent);
        }
    }
}
