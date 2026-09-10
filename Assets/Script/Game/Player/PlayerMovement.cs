using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]   
    private float _rotationSpeed;

    [SerializeField]
    private float _screenBorder;

    private Rigidbody2D _rigidbody; 
    private Vector2 _movmentInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movmentInputSmoothVelocity;
    private Camera _camera;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        SetPLayerVelocity();
        RotateDirectionOfInput();
    }

    private void SetPLayerVelocity()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movmentInput,
            ref _movmentInputSmoothVelocity,
            0.1f);

        _rigidbody.linearVelocity = _smoothedMovementInput * _speed;

        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if((screenPosition.x < _screenBorder && _rigidbody.linearVelocity.x < 0) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _rigidbody.linearVelocity.x > 0))
        {   
            _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocity.y);
        }

        if ((screenPosition.y < _screenBorder && _rigidbody.linearVelocity.y < 0) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _rigidbody.linearVelocity.y > 0))
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        }
    }


    private void RotateDirectionOfInput()
    {
        if (_movmentInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _rigidbody.MoveRotation(rotation);
        }

    }

    private void OnMove(InputValue inputValue)
    {
       _movmentInput = inputValue.Get<Vector2>();
    }
}
