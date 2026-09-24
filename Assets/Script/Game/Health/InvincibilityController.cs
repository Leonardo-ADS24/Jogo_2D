using System.Collections;
using UnityEngine;


public class InvicibillityController : MonoBehaviour
{

    private HealthController _healthController;
    private SpriteFlash _spriteFlash;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
        _spriteFlash = GetComponent<SpriteFlash>();
    }

    public void StartInvincibility(float invincibilityDuration, Color flashColor, int numberOfFlashes)
    {
        StartCoroutine(InvinvicilityCoroutine(invincibilityDuration, flashColor, numberOfFlashes));
    }

    private IEnumerator InvinvicilityCoroutine(float invincibilityDuration, Color flashColor, int numberOfFlashes)
    {
        _healthController.isInvincible = true;
        yield return _spriteFlash.FlashCoroutine(invincibilityDuration,flashColor, numberOfFlashes);
        _healthController.isInvincible = false;
    }    

}
