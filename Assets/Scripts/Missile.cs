using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _missileSpeed = 10f;
    [SerializeField] private GameObject _explosionPrefab;

    private GameObject _explosion;

    private const float DestroyTime = 0.5f;

    public void Kill()
    {
        _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, DestroyTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SpaceShip"))
        {
            other.gameObject.GetComponent<SpaceShip>().ShipTakesDamage();
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(_explosion, DestroyTime);
        }
        if (other.gameObject.CompareTag("Bunker"))
        {
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(_explosion, DestroyTime);
        }
        if (other.gameObject.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(_missileSpeed * Time.deltaTime * Vector2.down);
    }
}
