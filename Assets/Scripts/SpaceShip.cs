using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float shootCooldown = 0.25f;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image livesBar;
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Sprite[] livesSprites;

    private GameObject _explosion;

    private float _maxLeft = -25f;
    private float _maxRight = 25f;
    private int _health = 3;
    private int _lives = 3;
    
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

    private void Explosion()
    {
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(_explosion, 2f);
    }

    public void ShipTakesDamage()
    {
        if (_health == 0)
        {
            if (_lives == 1)
            {
                _lives = 0;
                gameObject.SetActive(false);
                Debug.Log("GameOver");
            }
            else
            {
                --_lives;
                livesBar.sprite = livesSprites[_lives - 1];
                _health = 3;
                Explosion();
                Debug.Log("Lifes :" + _lives + " Health: " + _health);
            }
        }
        else
        {
            --_health;
            healthBar.sprite = healthSprites[_health - 1];
            Debug.Log("Health: " + _health);
            Explosion();
        }
    }
}

