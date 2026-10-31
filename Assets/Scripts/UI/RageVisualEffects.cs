using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RageVisualEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerRage _playerRage;
    [SerializeField] private Volume _volume;
    [SerializeField] private Image _redOverlay;

    [Header("Vignette")]
    [SerializeField] private float _targetVignetteIntensity = 0.45f;

    [Header("Red Overlay")]
    [SerializeField] private float _targetOverlayAlpha = 0.25f;

    [Header("Transition")]
    [SerializeField] private float _transitionSpeed = 2f;

    private Vignette _vignette;

    private float _currentVignetteIntensity;
    private float _currentOverlayAlpha;

    private float _targetIntensity;
    private float _targetAlpha;

    private void Awake()
    {
        if (_volume != null && _volume.profile != null)
        {
            _volume.profile.TryGet(out _vignette);
        }

        if (_vignette != null)
            _currentVignetteIntensity = _vignette.intensity.value;

        if (_redOverlay != null)
        {
            Color color = _redOverlay.color;
            color.a = 0f;
            _redOverlay.color = color;
        }
    }

    private void OnEnable()
    {
        if (_playerRage == null)
            return;

        _playerRage.OnBerserkChanged += SetBerserkEffects;

        SetBerserkEffects(_playerRage.IsBerserk);
    }

    private void OnDisable()
    {
        if (_playerRage != null)
            _playerRage.OnBerserkChanged -= SetBerserkEffects;
    }

    private void Update()
    {
        UpdateVignette();
        UpdateRedOverlay();
    }

    private void SetBerserkEffects(bool isActive)
    {
        if (isActive)
        {
            _targetIntensity = _targetVignetteIntensity;
            _targetAlpha = _targetOverlayAlpha;
        }
        else
        {
            _targetIntensity = 0f;
            _targetAlpha = 0f;
        }
    }

    private void UpdateVignette()
    {
        if (_vignette == null)
            return;

        _currentVignetteIntensity = Mathf.MoveTowards(
            _currentVignetteIntensity,
            _targetIntensity,
            _transitionSpeed * Time.deltaTime
        );

        _vignette.intensity.value = _currentVignetteIntensity;
    }

    private void UpdateRedOverlay()
    {
        if (_redOverlay == null)
            return;

        _currentOverlayAlpha = Mathf.MoveTowards(
            _currentOverlayAlpha,
            _targetAlpha,
            _transitionSpeed * Time.deltaTime
        );

        Color color = _redOverlay.color;
        color.a = _currentOverlayAlpha;
        _redOverlay.color = color;
    }
}