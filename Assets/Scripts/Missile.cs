using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _missileSpeed = 10f;
    [SerializeField] private GameObject _explosionPrefab;

    private GameObject _explosion;

    public void Kill()
    {
        _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SpaceShip"))
        {
            other.gameObject.GetComponent<SpaceShip>().ShipTakesDamage();
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(_explosion, 0.5f);
        }
        if (other.gameObject.CompareTag("Bunker"))
        {
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, 0.5f);
        }
        if (other.gameObject.CompareTag("BottomBoundary"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(_missileSpeed * Time.deltaTime * Vector2.down);
    }
}
