using UnityEngine;

public class SpinningEarth : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 2 * Time.deltaTime, Space.Self);
    }
}
