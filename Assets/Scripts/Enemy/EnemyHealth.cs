using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth;
    private float _health;

    private bool isDead = false;

    public float Health
    {
        get { return _health; }
        private set { _health = value; }
    }
    public float MaxHealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    private void Start()
    {
        _health = _maxHealth;
    }
    public void TakeDamage(float damage, GameAgent sender)
    {
        if (IsDead) { return; }

        Debug.Log($"{gameObject.name} took damage: {damage} from {sender.name}: {sender.entityFaction}");
        Health -= damage;
        if (Health <= 0)
        {
            IsDead = true;
            Debug.Log($"{gameObject.name} is dead");
        }
    }
}
