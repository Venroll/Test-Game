
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _minPitch = -35f;
    [SerializeField] private float _maxPitch = 65f;

    [Header("Player")]
    [SerializeField] private Transform _player;

    private PlayerHealth _playerHealth;

    private float _pitch;

    private void Awake()
    {
       _playerHealth = GetComponentInParent<PlayerHealth>();
    }
    private void OnEnable()
    {
        _playerHealth.OnDied += DisableCameraMovement;
    }
    private void OnDisable()
    {
        _playerHealth.OnDied -= DisableCameraMovement;
    }
    private void Start()
    {
        // Запоминаем начальный наклон камеры
        _pitch = transform.localEulerAngles.x;

        if (_pitch > 180f)
            _pitch -= 360f;

        LockCursor();
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Игрок поворачивается по горизонтали вместе с мышью
        float yaw = mouseDelta.x * _sensitivity;
        _player.Rotate(Vector3.up * yaw);

        // Камера наклоняется вверх и вниз
        _pitch -= mouseDelta.y * _sensitivity;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisableCameraMovement()
    {
        enabled = false;
    }
}
