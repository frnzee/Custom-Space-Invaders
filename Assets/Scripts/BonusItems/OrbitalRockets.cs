using UnityEngine;

public class OrbitalRockets : MonoBehaviour
{
    private const float DestroyTime = 0.5f;
    private const float RotationSpeed = 10f;

    [SerializeField] private GameObject _explosionPrefab;

    private int _killCounter = 0;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Invader>())
        {
            if (_killCounter < 4)
            {
                other.gameObject.GetComponent<Invader>().Kill();
                ++_killCounter;

                GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(_explosion, DestroyTime);
            }
            else
            {
                other.gameObject.GetComponent<Invader>().Kill();
                Destroy(gameObject);

                GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(_explosion, DestroyTime);
            }
        }
    }

    private void Update()
    {
        transform.RotateAround(GameUI.Instance.SpinningEarth.GetComponent<Transform>().position, Vector3.back, RotationSpeed * Time.deltaTime);
    }
}
