
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerRage _playerRage;
    [SerializeField] private WeaponAttack _weaponAttack;
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
        if (_playerRage == null)
            _playerRage = GetComponent<PlayerRage>();
        if (_weaponAttack == null)
            _weaponAttack = GetComponentInChildren<WeaponAttack>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_health / _maxHealth);
    }

    public void TakeDamage(float damage, GameAgent sender)
    {
        if (_isDead || damage <= 0f)
            return;

        if (_playerRage != null && _playerRage.IsBerserk)
            return;

        

        _weaponAttack?.StartHitReaction();

        float actualDamage = Mathf.Min(damage, _health);

        HealthChange(actualDamage);

        _playerRage?.AddRageFromDamageReceived(actualDamage);

        Debug.Log($"{gameObject.name} took damage: {actualDamage}");

        if (sender != null)
            Debug.Log($"Damage from {sender.name}: {sender.entityFaction}");

        if (_health <= 0f)
        {
            AudioManager.Instance.PlayDeathHit();
            Die();
            return;
        }
        else
        {
            AudioManager.Instance.PlayPlayerHit();
        }

        _animator?.SetTrigger("GetHit");
    }
    public void Heal(float amount)
    {
        if (_isDead || amount <= 0f)
            return;

        _health = Mathf.Clamp(_health + amount, 0f, _maxHealth);

        OnHealthChanged?.Invoke(_health / _maxHealth);
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

        MusicManager.Instance.PlayDefeatMusic();

        _animator?.SetTrigger("Die");
        _animator?.SetBool("IsDead", true);

        OnDied?.Invoke();
    }
}
