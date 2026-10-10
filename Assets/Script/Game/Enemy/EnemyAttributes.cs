using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attributes", menuName = "Scriptable Objects/Enemy Attributes")]

public class EnemyAttributes : ScriptableObject
{
    [field: SerializeField]
    public float Speed {  get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float PlayerAwarenessDistance { get; private set; }

    [field: SerializeField]
    public int KillScore { get; private set; }

    [field: SerializeField]
    public float DamageAmount { get; private set; }

    [field: SerializeField]
    [field: Range(0,1)]
    public float ChanceOfCollectableDrop { get; private set; }

    [field: SerializeField]
    public float ScreenBorder { get; private set; }

    [field: SerializeField]
    public float ObstacleCheckCircleRadius { get; private set; }

    [field: SerializeField]
    public float ObstacleCheckDistance { get; private set; }

    [field: SerializeField]
    public LayerMask ObstacleLayerMask { get; private set; }


}
