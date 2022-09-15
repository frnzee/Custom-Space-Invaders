using UnityEngine;

public class OrbitalRockets : MonoBehaviour
{
    private const float RotationSpeed = 10f;
    private const int MaxKillCount = 5;

    private int _killCounter = 0;

    private void Start()
    {
        transform.RotateAround(GameUI.Instance.SpinningEarth.GetComponent<Transform>().position, Vector3.back, -37);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Invader>())
        {
            other.gameObject.GetComponent<Invader>().Kill();

            if (_killCounter < MaxKillCount)
            {
                ++_killCounter;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.RotateAround(GameUI.Instance.SpinningEarth.GetComponent<Transform>().position, Vector3.back, RotationSpeed * Time.deltaTime);
    }
}
