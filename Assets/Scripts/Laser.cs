using UnityEngine;

public class Laser : MonoBehaviour
{
    private const float DestroyTime = 0.5f;

    [SerializeField] float _laserSpeed = 50f;
    [SerializeField] private Laser _laserPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    public void TripleShot()
    {
        SpaceShip.Instance._readyToShoot = false;

        Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, 60));
        Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, -60));

        ++SpaceShip.Instance.laserCounter;
        ++SpaceShip.Instance.laserCounter;
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
        --SpaceShip.Instance.laserCounter;
    }
}
