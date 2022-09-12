using UnityEngine;

public class SlowdownSpaceShip : MonoBehaviour
{
    private void Start()
    {
        SpaceShip.Instance._slowDownIsActive = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Boundary>())
        {
            Destroy(gameObject);
        }
    }

}