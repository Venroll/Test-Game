using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private EnemyAnimator _enemyAnim;
    [SerializeField] private EnemyCombat _enemyCombat;
    private float _health;

    public event Action OnDied;

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
        if (_enemyCombat == null)
            _enemyCombat = GetComponentInChildren<EnemyCombat>();
    }
    public void TakeDamage(float damage, GameAgent sender)
    {
        if (IsDead) { return; }
        _enemyCombat?.CancelAttack();

        Debug.Log($"{gameObject.name} took damage: {damage} from {sender.name}: {sender.entityFaction}");
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;

            AudioManager.Instance.PlayEnemyDeath(transform.position);

            OnDied?.Invoke();

            Debug.Log($"{gameObject.name} is dead");
        }
        else
        {
            AudioManager.Instance.PlayEnemyHit(transform.position);
        }
        _enemyAnim?.PlayHit();
    }
}
