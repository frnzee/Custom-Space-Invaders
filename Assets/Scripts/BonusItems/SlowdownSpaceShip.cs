using UnityEngine;

public class SlowdownSpaceShip : MonoBehaviour
{
    private void Start()
    {
        SpaceShip.Instance.SlowDown();
        Destroy(gameObject);
    }
}
