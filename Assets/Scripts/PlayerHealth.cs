using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth;
    private float _health;

    [Header("Events")]
    [SerializeField] private UnityEvent<float> HpChangedPercent;
    public float Health { 
        get { return _health; }
        private set { _health = value; }
    }
    public float MaxHealth { 
        get { return _maxHealth; } 
        private set { _maxHealth = value; }
    }

    private void Start()
    {
        _health = _maxHealth;
    }
    public void TakeDamage(float damage, GameAgent sender)
    {
        Debug.Log($"{gameObject.name} took damage: {damage} from {sender.name}: {sender.entityFaction}");
        Health -= damage;
        HpChangedPercent?.Invoke(_health / _maxHealth);
        if (Health <= 0)
        {
            Debug.Log("Player is dead");
            Destroy(gameObject);
        }
    }
}
