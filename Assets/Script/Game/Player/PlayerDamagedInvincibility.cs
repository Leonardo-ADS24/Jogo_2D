using System.Collections;
using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{


    [SerializeField]
    private float _invincibilityDuration;

    [SerializeField]
    private Color _flashColor;

    [SerializeField]
    private int _numberOfFlashes;

    private InvicibillityController _invincibilityController;


    private void Awake()
    {
        _invincibilityController = GetComponent<InvicibillityController>();

    }

    public void StartInvincibility()
    {
        _invincibilityController.StartInvincibility(_invincibilityDuration,_flashColor,_numberOfFlashes);
    }
}
