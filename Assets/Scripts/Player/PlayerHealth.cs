using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxHealth;
    private bool isDead = false;
    private float _health;

    [Header("Events")]
    [SerializeField] private UnityEvent<float> HpChangedPercent;
    [SerializeField] private UnityEvent Die;
    public float Health { 
        get { return _health; }
        private set { _health = value; }
    }
    public float MaxHealth { 
        get { return _maxHealth; } 
        private set { _maxHealth = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set {  isDead = value; }
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
        HpChangedPercent?.Invoke(_health / _maxHealth);

        if (_animator == null) { return; }
        
        _animator.SetTrigger("GetHit");
        if (Health <= 0)
        {
            IsDead = true;
            _animator.SetTrigger("Die");
            _animator.SetBool("IsDead", isDead);
            Die?.Invoke();
        }
    }
}
