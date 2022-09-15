using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField] float _laserSpeed = 50f;
    [SerializeField] private Laser _laserPrefab;

    public void TripleShot()
    {
        SpaceShip.Instance.ReadyToShoot(false);

        Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, 60));
        Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, -60));

        ++SpaceShip.Instance.laserCounter;
        ++SpaceShip.Instance.laserCounter;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Invader>())
        {
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
        SpaceShip.Instance.ReadyToShoot(true);
        --SpaceShip.Instance.laserCounter;
    }
}
