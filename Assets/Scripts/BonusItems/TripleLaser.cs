using UnityEngine;

public class TripleLaser : MonoBehaviour
{
    private const float DestroyTime = 0.5f;

    [SerializeField] float _laserSpeed = 50f;
    [SerializeField] private Laser _laserPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    private Laser[] _tripleLasers;

    public void Start()
    {
        SpaceShip.Instance._readyToShoot = false;
        _tripleLasers[0] = Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, 60));
        _tripleLasers[1] = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        _tripleLasers[2] = Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, -60));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Invader>())
        {
            GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, DestroyTime);

            other.gameObject.GetComponent<Invader>().Kill();

            Destroy(gameObject);
        }

        if (other.gameObject.GetComponent<Missile>())
        {
            other.gameObject.GetComponent<Missile>().Kill();

            Destroy(gameObject);
        }

        if (other.gameObject.GetComponent<Boundary>() ||
            other.gameObject.GetComponent<Bunker>())
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(_laserSpeed * Time.deltaTime * Vector2.up);
    }

    private void OnDestroy()
    {
        SpaceShip.Instance._readyToShoot = true;
    }
}
