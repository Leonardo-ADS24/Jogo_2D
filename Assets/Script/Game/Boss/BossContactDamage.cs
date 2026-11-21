using UnityEngine;

public class BossContactDamage : MonoBehaviour
{
    [SerializeField]
    private float _damage = 20f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
        }
    }
}