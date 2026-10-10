using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private Transform _player;
    [SerializeField] private InputActionReference _lookAction;

    [Header("Custom values")]
    [SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private float _minPitch = 1.0f;
    [SerializeField] private float _maxPitch = 1.0f;

    private float _pitch;

    private void OnEnable()
    {
        _lookAction.action.Enable();
    }
    private void OnDisable()
    {
        _lookAction.action.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CursorLock();
        }
        Vector2 lookInput = _lookAction.action.ReadValue<Vector2>();
        float mouseX = lookInput.x * _sensitivity;
        float mouseY = lookInput.y * _sensitivity;

        _player.Rotate(Vector3.up * mouseX);

        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        _cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void CursorLock()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
