using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float laserSpeed = 15f;

    private float _lifeTime = 2f;

    void Update()
    {
        transform.Translate(laserSpeed * Time.deltaTime * Vector2.up);
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Invaders"))
        {
            collision.gameObject.GetComponent<Invaders>().Kill();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Missile"))
        {
            collision.gameObject.GetComponent<Missile>().Kill();
            Destroy(gameObject);
        }
    }
}
