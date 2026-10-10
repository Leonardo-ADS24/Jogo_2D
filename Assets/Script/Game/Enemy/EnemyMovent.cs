using System.Collections;
using UnityEngine;

public class EnemyMovent : MonoBehaviour
{
    [SerializeField]
    private EnemyAttributes _enemyAttributes; 

   

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10];
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;

        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _enemyAttributes.ScreenBorder && _targetDirection.x < 0) ||
            (screenPosition.x > _camera.pixelWidth -  _enemyAttributes.ScreenBorder && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }

        if ((screenPosition.y < _enemyAttributes.ScreenBorder && _targetDirection.y < 0) ||
            (screenPosition.y > _camera.pixelHeight - _enemyAttributes.ScreenBorder && _targetDirection.y > 0))
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }
    
    private void HandleObstacles()
    {
        _obstacleAvoidanceCooldown -= Time.deltaTime;

        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_enemyAttributes.ObstacleLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _enemyAttributes.ObstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _enemyAttributes.ObstacleCheckDistance);

        for (int i = 0; i < numberOfCollisions; i++) 
        { 
            var obstacleCollision = _obstacleCollisions[i];

            if(obstacleCollision.collider.gameObject == gameObject)
            {
                continue;
            }

            if (_obstacleAvoidanceCooldown <= 0)
            {
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal;
                _obstacleAvoidanceCooldown = 0.5f;
            }

            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyAttributes.RotationSpeed * Time.deltaTime);



            _targetDirection =  rotation * Vector2.up;
            break;
            
        }
    }




    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,_enemyAttributes.RotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        _rigidbody.linearVelocity = transform.up * _enemyAttributes.Speed;
    }
}
