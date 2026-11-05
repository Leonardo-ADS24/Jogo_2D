using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [field: SerializeField] public string WeaponName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } = 10;
    [field: SerializeField] public float TimeBetweenShots { get; private set; } = 0.3f;
    [field: SerializeField] public float BulletSpeed { get; private set; } = 8f;
    [field: SerializeField] public GameObject BulletPrefab { get; private set; } // opcional
}