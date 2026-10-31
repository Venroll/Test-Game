using UnityEngine;
using UnityEngine.UI;

public class RageBarUI : MonoBehaviour
{
    [SerializeField] private PlayerRage _playerRage;
    [SerializeField] private Image _rageImage;

    private void OnEnable()
    {
        if (_playerRage == null)
            return;

        _playerRage.OnRageChanged += UpdateRageBar;
        UpdateRageBar(_playerRage.CurrentRage / _playerRage.MaxRage);
    }

    private void OnDisable()
    {
        if (_playerRage == null)
            return;

        _playerRage.OnRageChanged -= UpdateRageBar;
    }

    private void UpdateRageBar(float ragePercent)
    {
        _rageImage.fillAmount = ragePercent;
    }
}