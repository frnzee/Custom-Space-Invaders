using UnityEngine;

public class TripleShot : MonoBehaviour
{
    void Start()
    {
        SpaceShip.Instance._tripleShotIsActive = true;
        Destroy(gameObject);
    }
}
