using UnityEngine;

public class SpinningEarth : MonoBehaviour
{
    public float _baseSpeed;

    void Update()
    {
        transform.Rotate(0, 0, _baseSpeed * Time.deltaTime, Space.Self);
    }
}
