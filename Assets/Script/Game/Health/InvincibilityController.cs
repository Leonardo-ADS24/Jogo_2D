using System.Collections;
using UnityEngine;


public class InvicibillityController : MonoBehaviour
{

    private HealthController _healthController;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
    }

    public void StartInvincibility(float invincibilityDuration)
    {
        StartCoroutine(InvinvicilityCoroutine(invincibilityDuration));
    }

    private IEnumerator InvinvicilityCoroutine(float invincibilityDuration)
    {
        _healthController.isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        _healthController.isInvincible = false;
    }    

}
