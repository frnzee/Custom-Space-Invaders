using System.Collections;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    private float _speed = 2f;
    private Vector3 vDistance = new Vector3(0, 0.2f, 0);
    private bool _isMoving;

    [SerializeField] private GameObject explosionPrefab;

    private GameObject _explosion;

    private IEnumerator MovementDown()
    {
        _isMoving = true;
        transform.position -= vDistance;
        yield return new WaitForSeconds(_speed);
        _isMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void Kill()
    {
        GameManager.invaders.Remove(gameObject);
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, 1f);
    }
    private void Update()
    {
        if (!_isMoving)
        {
            StartCoroutine(MovementDown());
        }
    }
}
