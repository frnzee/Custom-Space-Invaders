using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private GameObject laser;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private GameObject explosionPrefab;
    private GameObject _explosion;

    private bool _isShooting = false;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
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

