using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    private float _verticalVelocity = 0f; 

    [Header("Custom values")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _jumpHeight = 1.5f;

    [Header("Links")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
    }
    private void OnDisable()
    {
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
    }

    private void Update()
    {
        Vector2 input = _moveAction.action.ReadValue<Vector2>();
        Vector3 direction = transform.right * input.x + transform.forward * input.y;
        direction = direction.normalized;

        if (_charController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        if (_charController.isGrounded && _jumpAction.action.IsPressed())
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
        _verticalVelocity += _gravity * Time.deltaTime;
        Vector3 velocity = direction * _moveSpeed + Vector3.up * _verticalVelocity; 

        _charController.Move(velocity * Time.deltaTime);
    }
}
