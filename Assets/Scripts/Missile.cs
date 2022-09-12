using UnityEngine;

public class Missile : MonoBehaviour
{
    private const float DestroyTime = 0.5f;

    [SerializeField] private float _missileSpeed = 10f;
    [SerializeField] private GameObject _explosionPrefab;

    private GameObject _explosion;

    public void Kill()
    {
        _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, DestroyTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SpaceShip>())
        {
            other.gameObject.GetComponent<SpaceShip>().ShipTakesDamage();

            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, DestroyTime);

            Destroy(gameObject);
        }

        if (other.gameObject.GetComponent<Bunker>())
        {
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(_explosion, DestroyTime);
        }

        if (other.gameObject.GetComponent<Boundary>())
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(_missileSpeed * Time.deltaTime * Vector2.down);
    }
}
