using System.Collections;
using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{


    [SerializeField]
    private float _invincibilityDuration;

    private InvicibillityController _invincibilityController;


    private void Awake()
    {
        _invincibilityController = GetComponent<InvicibillityController>();

    }

    public void StartInvincibility()
    {
        _invincibilityController.StartInvincibility(_invincibilityDuration);
    }
}
