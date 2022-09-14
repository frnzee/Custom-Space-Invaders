using UnityEngine;

public class OrbitalRockets : MonoBehaviour
{
    private const float DestroyTime = 0.5f;
    private const float RotationSpeed = 10f;
    private const int MaxKillCount = 5;

    [SerializeField] private GameObject _explosionPrefab;

    private int _killCounter = 0;

    private void Start()
    {
        transform.RotateAround(GameUI.Instance.SpinningEarth.GetComponent<Transform>().position, Vector3.back, -37);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Invader>())
        {
            if (_killCounter < MaxKillCount)
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
