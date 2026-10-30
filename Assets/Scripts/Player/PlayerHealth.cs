
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxHealth = 100f;

    private bool _isDead;
    private float _health;

    public event Action<float> OnHealthChanged;
    public event Action OnDied;

    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public bool IsDead {
        get { return _isDead; }
        set { _isDead = value;}
    }

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_health / _maxHealth);
    }

    public void TakeDamage(float damage, GameAgent sender)
    {
        if (_isDead || damage <= 0f)
            return;

        Debug.Log($"{gameObject.name} took damage: {damage}");
        Debug.Log($"Before hit: {_health}");
        HealthChange(damage);
        Debug.Log($"After hit: {_health}");
        if (sender != null)
        {
            Debug.Log($"Damage from {sender.name}: {sender.entityFaction}");
        }

        if (_health <= 0f)
        {
            Die();
            return;
        }

        _animator?.SetTrigger("GetHit");
    }

    private void HealthChange(float amount)
    {
        _health -= amount;
        OnHealthChanged?.Invoke(_health / _maxHealth);
    }

    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;

        _animator?.SetTrigger("Die");
        _animator?.SetBool("IsDead", true);

        OnDied?.Invoke();
    }
}
