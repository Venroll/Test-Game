using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image _healthImage;

    private void OnEnable()
    {
        if (_playerHealth == null)
            return;

        _playerHealth.OnHealthChanged += UpdateHealthBar;

        UpdateHealthBar(
            _playerHealth.Health / _playerHealth.MaxHealth
        );
    }

    private void OnDisable()
    {
        if (_playerHealth == null)
            return;

        _playerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float healthPercent)
    {
        _healthImage.fillAmount = healthPercent;
    }
}