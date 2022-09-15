using UnityEngine;

public class TripleShot : MonoBehaviour
{
    void Start()
    {
        SpaceShip.Instance.TripleShot();
        Destroy(gameObject);
    }
}
