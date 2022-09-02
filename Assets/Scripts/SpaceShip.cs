using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float shipSpeed = 15.0f;
    [SerializeField] private float shootCooldown = 0.25f;

    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject explosionPrefab;
    
    [SerializeField] private Image healthBar;
    [SerializeField] private Image livesBar;
    
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Sprite[] livesSprites;

    private GameObject _explosion;

    private readonly float _maxLeft = -25f;
    private readonly float _maxRight = 25f;
    
    private int _health = 3;
    private int _lives = 3;
    
    private bool _isShooting = false;

    private Action<bool> gameFail;

    private void Update()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > _maxLeft)
        {
            transform.position += shipSpeed * Time.deltaTime * Vector3.left;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < _maxRight)
        {
            transform.position += shipSpeed * Time.deltaTime * Vector3.right;
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
        Destroy(_explosion, 0.5f);
    }

    public void ShipTakesDamage()
    {
        if (_health == 1)
        {
            if (_lives == 1)
            {
                _health = 0;
                _lives = 0;
                livesBar.sprite = livesSprites[_lives];
                healthBar.sprite = healthSprites[_health];
                gameObject.SetActive(false);
                Debug.Log("GameOver");
            }
            else
            {
                --_lives;
                livesBar.sprite = livesSprites[_lives];                
                _health = 3;
                healthBar.sprite = healthSprites[_health];
                Explosion();
            }
        }
        else
        {
            --_health;
            healthBar.sprite = healthSprites[_health];
            Explosion();
        }
    }
}

