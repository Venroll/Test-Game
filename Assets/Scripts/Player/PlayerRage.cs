using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRage : MonoBehaviour
{
    [Header("Rage")]
    [SerializeField] private float _maxRage = 100f;
    [SerializeField] private float _ragePerDamageReceived = 0.5f;
    [SerializeField] private float _ragePerDamageDealt = 0.4f;

    [Header("Rage Decay")]
    [SerializeField] private float _idleTimeBeforeDecay = 5f;
    [SerializeField] private float _rageDecaySpeed = 5f;

    [Header("Berserk")]
    [SerializeField] private float _berserkDuration = 6f;
    [SerializeField] private PlayerHealth _playerHealth;

    private float _currentRage;
    private float _lastCombatTime;
    private float _berserkTimer;
    private bool _isBerserk;

    public event Action<float> OnRageChanged;
    public event Action<bool> OnBerserkChanged;

    public float CurrentRage => _currentRage;
    public float MaxRage => _maxRage;
    public bool IsBerserk => _isBerserk;
    public bool CanActivate => _currentRage >= _maxRage && !_isBerserk;

    private void Awake()
    {
        if (_playerHealth == null)
            _playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        OnRageChanged?.Invoke(_currentRage / _maxRage);
    }

    private void Update()
    {
        if (_playerHealth != null && _playerHealth.IsDead)
            return;

        if (Time.timeScale <= 0f)
            return;

        if (_isBerserk)
        {
            UpdateBerserk();
            return;
        }

        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
            ActivateBerserk();

        UpdateRageDecay();
    }

    public void AddRageFromDamageReceived(float damage)
    {
        AddRage(damage * _ragePerDamageReceived);
    }

    public void AddRageFromDamageDealt(float damage)
    {
        AddRage(damage * _ragePerDamageDealt);
    }

    private void AddRage(float amount)
    {
        if (_isBerserk || amount <= 0f)
            return;

        _currentRage = Mathf.Clamp(_currentRage + amount, 0f, _maxRage);
        _lastCombatTime = Time.time;

        OnRageChanged?.Invoke(_currentRage / _maxRage);
    }

    private void UpdateRageDecay()
    {
        if (_currentRage <= 0f)
            return;

        if (Time.time - _lastCombatTime < _idleTimeBeforeDecay)
            return;

        _currentRage = Mathf.MoveTowards(
            _currentRage,
            0f,
            _rageDecaySpeed * Time.deltaTime
        );

        OnRageChanged?.Invoke(_currentRage / _maxRage);
    }

    private void ActivateBerserk()
    {
        if (!CanActivate)
            return;

        _isBerserk = true;
        _berserkTimer = _berserkDuration;

        OnBerserkChanged?.Invoke(true);
        Debug.Log("BERSERK ACTIVATED");
    }

    private void UpdateBerserk()
    {
        _berserkTimer -= Time.deltaTime;

        _currentRage = Mathf.MoveTowards(
            _currentRage,
            0f,
            (_maxRage / _berserkDuration) * Time.deltaTime
        );

        OnRageChanged?.Invoke(_currentRage / _maxRage);

        if (_berserkTimer <= 0f)
            DeactivateBerserk();
    }

    private void DeactivateBerserk()
    {
        _isBerserk = false;
        _currentRage = 0f;

        OnRageChanged?.Invoke(0f);
        OnBerserkChanged?.Invoke(false);

        Debug.Log("BERSERK ENDED");
    }
}