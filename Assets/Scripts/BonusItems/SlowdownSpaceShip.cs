using UnityEngine;

public class SlowdownSpaceShip : MonoBehaviour
{
    private void Start()
    {
        SpaceShip.Instance._slowDownIsActive = true;
        Destroy(gameObject);
    }
}
