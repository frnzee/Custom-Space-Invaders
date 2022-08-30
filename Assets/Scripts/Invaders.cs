using System.Collections;
using UnityEngine;

public class Invaders : MonoBehaviour
{
/*    private float _speed = 50f;
    private Vector3 vDistance = new Vector3(0, 0.2f, 0);
    private Vector3 hDistance = new Vector3(0.2f, 0, 0);
    private bool _isMovingRight = true;
    private float _MaxLeft = -25f;
    private float _MaxRight = 25f;*/


    [SerializeField] private GameObject explosionPrefab;
    private GameObject _explosion;
    public void Kill()
    {
        GameManager.invaders.Remove(gameObject);
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(_explosion, 1f);
    }

    /*    private IEnumerator MoveRight()
        {
            transform.position += hDistance;
            if (transform.position.x > _MaxRight)
            {
                _isMovingRight = !_isMovingRight;
                transform.position -= vDistance;
            }
            yield return new WaitForSeconds(_speed);
        }
        private IEnumerator MoveLeft()
        {
            transform.position -= hDistance;
            if (transform.position.x < _MaxLeft)
            {
                _isMovingRight = !_isMovingRight;
                transform.position -= vDistance;
            }
            yield return new WaitForSeconds(_speed);
        }
        private void FixedUpdate()
        {
            if (_isMovingRight)
            {
                StartCoroutine(MoveRight());
            }
            else
            {
                StartCoroutine(MoveLeft());
            }
        }*/
}
