using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera _camera;

    public int Damage { get; set; } = 10;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        DestroyWhenOffScreen();
    }
     
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovent>())
        {
            HealthController healthController = collision.GetComponent<HealthController>();
            //healthController.TakeDamage(10);
            healthController.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    private void DestroyWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if(screenPosition.x < 0 ||
           screenPosition.x > _camera.pixelWidth ||
           screenPosition.y < 0 ||
           screenPosition.y > _camera.pixelHeight)
        {
            Destroy(gameObject);
        }
    }
}
