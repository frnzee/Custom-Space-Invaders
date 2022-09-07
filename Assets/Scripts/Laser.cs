using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _laserSpeed = 50f;
    [SerializeField] private GameObject _explosionPrefab;

    private GameObject _explosion;

    private const float DestroyTime = 0.5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Invaders"))
        {
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, DestroyTime);
            other.gameObject.GetComponent<Invader>().Kill();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Missile"))
        {
            other.gameObject.GetComponent<Missile>().Kill();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Boundary") ||
            other.gameObject.CompareTag("Bunker"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(_laserSpeed * Time.deltaTime * Vector2.up);
    }
}
