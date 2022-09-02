using System.Collections;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    private GameObject _explosion;
    public void Kill()
    {
        GameManager.invaders.Remove(gameObject);
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, 0.5f);
    }
}
