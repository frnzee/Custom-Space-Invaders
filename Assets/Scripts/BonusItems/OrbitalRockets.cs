using UnityEngine;

public class OrbitalRockets : MonoBehaviour
{
    private const float RotationSpeed = 10f;
    private const int MaxKillCount = 5;
    private const float DistanceModifier = 2f;
    private const float StartingPositionAngle = -50;

    private int _killCounter = 0;

    private void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y * Random.Range(0, DistanceModifier));
        transform.RotateAround(GameUI.Instance.SpinningEarth.GetComponent<Transform>().position, Vector3.back, StartingPositionAngle);
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
