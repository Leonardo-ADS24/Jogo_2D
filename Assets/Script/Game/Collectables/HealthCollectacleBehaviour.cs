using UnityEngine;

public class HealthCollectacleBehaviour : MonoBehaviour, ICollectableBehaviour
{

    [SerializeField]
    private float _healthAmount;

    public void OnCollected(GameObject player)
    {
        player.GetComponent<HealthController>().AddHealth(_healthAmount);
    }
    
}
