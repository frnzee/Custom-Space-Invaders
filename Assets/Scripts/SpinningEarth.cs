using UnityEngine;

public class SpinningEarth : MonoBehaviour
{
    [SerializeField] private float _baseSpeed;

    private void Update()
    {
        transform.Rotate(0, 0, _baseSpeed * Time.deltaTime, Space.Self);
    }
}
