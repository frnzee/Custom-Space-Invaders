using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    private static SpaceShip _instance;
    public static SpaceShip Instance
    {
        get
        {
            if (!_instance)
            {
                Debug.LogError("Instance not specified");
            }
            return _instance;
        }
    }

    private const float MaxLeft = -25f;
    private const float MaxRight = 25f;
    private const float DefaultShipSpeed = 20f;
    private const float SlowdownTime = 10f;
    private const float TripleShotTime = 10f;
    private const int DefaultHealth = 3;

    [SerializeField] private Laser _laserPrefab;

    public int _health = 3;
    public int _lives = 1;

    private float _shipSpeed = 20f;
    private float _tripleShotTimer = 10f;
    private float _slowDownTimer = 10f;

    public bool _readyToShoot = true;
    public bool _tripleShotIsActive = false;
    public bool _slowDownIsActive = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        GameStatsUI.Instance.UpdateHealthLives(_health, _lives);
    }

    private void Shot()
    {
        Laser laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        _readyToShoot = false;
        if (_tripleShotIsActive)
        {
            laser.TripleShot();
        }
    }

    public void ShipTakesDamage()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Game)
        {
            if (_health == 1)
            {
                if (_lives == 1)
                {
                    --_health;
                    --_lives;

                    GameStatsUI.Instance.UpdateHealthLives(_health, _lives);
                    GameManager.Instance.GameOver();
                    Destroy(gameObject);
                }
                else
                {
                    --_lives;
                    _health = DefaultHealth;
                    GameStatsUI.Instance.UpdateHealthLives(_health, _lives);
                }
            }
            else
            {
                --_health;
                GameStatsUI.Instance.UpdateHealthLives(_health, _lives);
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Game)
        {
            UpdateMoving();
            UpdateShooting();
            UpdateTripleShot();
            UpdateSlowDown();
        }
    }

    private void UpdateTripleShot()
    {
        if (_tripleShotIsActive && _tripleShotTimer > 0)
        {
            _tripleShotTimer -= Time.deltaTime;
            if (_tripleShotTimer <= 0)
            {
                _tripleShotIsActive = false;
                _tripleShotTimer = TripleShotTime;
            }
        }
    }

    private void UpdateSlowDown()
    {
        if (_slowDownIsActive && _slowDownTimer > 0)
        {
            _slowDownTimer -= Time.deltaTime;
            if (_slowDownTimer <= 0)
            {
                _slowDownIsActive = false;
                _slowDownTimer = SlowdownTime;
            }
        }
    }

    private void UpdateMoving()
    {
        var direction = Vector3.zero;

        if (_slowDownIsActive)
        {
            _shipSpeed = DefaultShipSpeed / 2;
        }
        else
        {
            _shipSpeed = DefaultShipSpeed;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > MaxLeft)
        {
            direction = Vector3.left;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < MaxRight)
        {
            direction = Vector3.right;
        }

        transform.position += _shipSpeed * Time.deltaTime * direction;
    }

    private void UpdateShooting()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && _readyToShoot == true)
        {
            Shot();
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
