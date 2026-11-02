using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, ICollectableBehaviour
{

    [SerializeField]
    private float _invincibillityDuration;

    [SerializeField]
    private Color _invincibillityColor;

    [SerializeField]
    private int _numberOfFlashes;


    public void OnCollected(GameObject player)
    {
        var invinvibillityController = player.GetComponent<InvicibillityController>();

        invinvibillityController.StartInvincibility(_invincibillityDuration, _invincibillityColor, _numberOfFlashes);
    }
}
