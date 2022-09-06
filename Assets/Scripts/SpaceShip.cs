using System;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float _shipSpeed = 15.0f;
    [SerializeField] private float _shootCooldown = 0.5f;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameStats _gameStats;

    private readonly float _maxLeft = -25f;
    private readonly float _maxRight = 25f;


    private int _health = 3;
    private int _lives = 1;

    private const float DefaultShootCooldown = 0.5f;
    private const int DefaultHealth = 3;

    private void Start()
    {
        _gameStats = GameStats.Instance;
        _gameStats.UpdateHealthLives(_health, _lives);
    }

    private void Shoot()
    {
        Instantiate(_laserPrefab, transform.position, Quaternion.identity);
    }

    public void ShipTakesDamage()
    {
        if (_health == 1)
        {
            if (_lives == 1)
            {
                --_health;
                --_lives;

                _gameStats.UpdateHealthLives(_health, _lives);

                GameManager.Instance.GameOver();
                Destroy(gameObject);
            }
            else
            {
                --_lives;
                _health = DefaultHealth;
                _gameStats.UpdateHealthLives(_health, _lives);
            }
        }
        else
        {
            --_health;
            _gameStats.UpdateHealthLives(_health, _lives);
        }
    }

    private void Update()
    {
        UpdateMoving();
        UpdateShooting();
    }

    private void UpdateMoving()
    {
        var direction = Vector3.zero;

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > _maxLeft)
        {
            direction = Vector3.left;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < _maxRight)
        {
            direction = Vector3.right;
        }

        transform.position += _shipSpeed * Time.deltaTime * direction;
    }

    private void UpdateShooting()
    {
        _shootCooldown -= Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && _shootCooldown <= 0)
        {
            Shoot();
            _shootCooldown = DefaultShootCooldown;
        }
    }
}
