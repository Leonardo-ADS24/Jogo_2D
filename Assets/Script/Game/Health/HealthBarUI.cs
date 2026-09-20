using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _healthBarForegroundImage;

    public void UpdateHealthBar( HealthController healthController)
    {
        _healthBarForegroundImage.fillAmount = healthController.ReaminingHealtPercentage;
    }
}
