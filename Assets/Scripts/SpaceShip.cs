using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float shipSpeed = 15.0f;
    [SerializeField] private float shootCooldown = 0.25f;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject explosionPrefab;

    public SpaceShipStats spaceShipStats;
    
    private Image healthBar;
    private Image livesBar;
    
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Sprite[] livesSprites;

    private GameObject _explosion;
    private GameObject _healthLives;

    private readonly float _maxLeft = -25f;
    private readonly float _maxRight = 25f;
    
    private int _health;
    private int _lives;

    private bool _isShooting = false;

    private Action<bool> _failGameDelegate;

    private void Start()
    {
        
        healthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Image>();
        livesBar = GameObject.FindGameObjectWithTag("Lives Bar").GetComponent<Image>();

        healthBar.sprite = healthSprites[spaceShipStats.currentHealth];
        livesBar.sprite = livesSprites[spaceShipStats.currentLives];
    }
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
        Instantiate(laserPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(shootCooldown);
        _isShooting = false;
    }

    private void Explosion()
    {
        _explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(_explosion, 0.5f);
    }

    public void Initialize(Action<bool> failGame)
    {
        _failGameDelegate = failGame;
    }

    private void HealthDelegate(int health)
    {
        _health = health;
        Debug.Log("Space ship health " + _health + " Incoming health" + health);
    }
    public void ShipTakesDamage()
    {
        if (_health == 1)
        {
            if (_lives == 1)
            {
                spaceShipStats.currentHealth = 0;
                spaceShipStats.currentLives = 0;
                healthBar.sprite = healthSprites[spaceShipStats.currentHealth];
                livesBar.sprite = livesSprites[spaceShipStats.currentLives];
                Destroy(gameObject);
                _failGameDelegate(true);
            }
            else
            {
                --spaceShipStats.currentLives;
                livesBar.sprite = livesSprites[spaceShipStats.currentLives];                
                _health = 3;
                healthBar.sprite = healthSprites[spaceShipStats.currentHealth];
                Explosion();
            }
        }
        else
        {
            --spaceShipStats.currentHealth;
            healthBar.sprite = healthSprites[spaceShipStats.currentHealth];
            Explosion();
        }
    }
}

