using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.5f;

    [SerializeField]
    private float _rotationSpeed = 80f;    // graus por segundo

    [SerializeField]
    private float _stopDistance = 3.5f;    // para de andar a esta distância do jogador

    private Rigidbody2D _rigidbody;
    private Transform _player;

    public bool CanMove { get; set; } = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            _player = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (_player == null)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = _player.position - transform.position;

        RotateTowards(toPlayer);

        if (CanMove && toPlayer.magnitude > _stopDistance)
        {
            _rigidbody.linearVelocity = toPlayer.normalized * _speed;
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void RotateTowards(Vector2 direction)
    {
        Quaternion target = Quaternion.LookRotation(Vector3.forward, direction);
        float angle = Mathf.MoveTowardsAngle(_rigidbody.rotation, target.eulerAngles.z, _rotationSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(angle);
    }
}