using UnityEngine;

public class OrbitalRockets : MonoBehaviour
{
    public Transform target;
    private int _killCounter = 0;
    public const float rotationSpeed = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Invaders"))
        {
            if (_killCounter < 4)
            {
                collision.gameObject.GetComponent<Invader>().Kill();
                ++_killCounter;
            }
            else
            {
                collision.gameObject.GetComponent<Invader>().Kill();
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        transform.RotateAround(target.position, Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
