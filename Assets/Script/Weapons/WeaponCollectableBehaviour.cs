using UnityEngine;

public class WeaponCollectableBehaviour : MonoBehaviour, ICollectableBehaviour
{
    [SerializeField]
    private WeaponData _weapon;

    public void OnCollected(GameObject player)
    {
        player.GetComponent<PlayerShoot>().EquipWeapon(_weapon);
    }
}