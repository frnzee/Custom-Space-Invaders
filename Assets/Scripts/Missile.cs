using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] float missileSpeed = 10f;

    private float _lifeTime = 3f;
    public GameObject explosionPrefab;
    private GameObject _explosion;

    void Update()
    {
        transform.Translate(missileSpeed * Time.deltaTime * Vector2.down);
        Destroy(gameObject, _lifeTime);
    }

    public void Kill()
    {
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, 1f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SpaceShip"))
        {
            collision.gameObject.GetComponent<SpaceShip>().ShipDies();            
        }
    }
}
