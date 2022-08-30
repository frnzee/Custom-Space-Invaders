using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float shootCooldown = 0.25f;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject explosionPrefab;
    private float _maxLeft = -25f;
    private float _maxRight = 25f;
    private GameObject _explosion;

    private bool _isShooting = false;

    private void Update()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > _maxLeft)
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < _maxRight)
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !_isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        _isShooting = true;
        Instantiate(laser, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(shootCooldown);
        _isShooting = false;
    }
    public void ShipDies()
    {
        gameObject.SetActive(false);
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(_explosion, 2f);
    }
}

