using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField] private string _speedParameter = "Speed";
    [SerializeField] private string _attackTrigger = "Attack";
    [SerializeField] private string _hitTrigger = "Hit";
    [SerializeField] private string _deathTrigger = "Die";

    private int _speedHash;
    private int _attackHash;
    private int _hitHash;
    private int _deathHash;

    private void Awake()
    {
        _speedHash = Animator.StringToHash(_speedParameter);
        _attackHash = Animator.StringToHash(_attackTrigger);
        _hitHash = Animator.StringToHash(_hitTrigger);
        _deathHash = Animator.StringToHash(_deathTrigger);
    }
    public void SetSpeed(float speed)
    {
        if (_animator == null) return;

        _animator.SetFloat(_speedHash, speed, 0.1f, Time.deltaTime);
    }

    public void PlayAttack()
    {
        if (_animator != null)
            _animator.SetTrigger(_attackHash);
    }

    public void PlayHit()
    {
        if (_animator != null)
            _animator.SetTrigger(_hitHash);
    }

    public void PlayDeath()
    {
        if (_animator != null)
        {
            _animator.ResetTrigger(_attackHash);
            _animator.ResetTrigger(_hitHash);
            _animator.SetTrigger(_deathHash);
        }
    }
    public void ResetAttackTrigger()
    {
        _animator.ResetTrigger("Attack");
    }
}