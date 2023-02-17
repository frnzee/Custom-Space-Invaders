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
    private const int DefaultLives = 1;

    [SerializeField] private Laser _laserPrefab;

    private MobileJoystick _mobileJoystick;
    private ShootButton _shootButton;

    public int LaserCounter = 0;

    public int Health { get; private set; }
    public int Lives { get; private set; }

    private float _shipSpeed = DefaultShipSpeed;
    private float _tripleShotTimer = TripleShotTime;
    private float _slowDownTimer = SlowdownTime;
    private float _mobileInput;

    private bool _readyToShoot = true;
    private bool _tripleShotIsActive = false;
    private bool _slowDownIsActive = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Health = DefaultHealth;
        Lives = DefaultLives;
        GameStatsUI.Instance.UpdateHealthLives(Health, Lives);
    }

    public void InitializeControls(MobileJoystick mobileJoystick, ShootButton shootButton)
    {
        _mobileJoystick = mobileJoystick;
        _shootButton = shootButton;
    }

    private void Shot()
    {
        Laser laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        _readyToShoot = false;

        ++LaserCounter;

        if (_tripleShotIsActive)
        {
            laser.TripleShot();
        }
    }

    public void ReadyToShoot(bool state)
    {
        _readyToShoot = state;
    }

    public void TripleShot()
    {
        _tripleShotIsActive = true;
    }

    public void SlowDown()
    {
        _slowDownIsActive = true;
    }

    public void ShipTakesDamage()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Game)
        {
            if (Health == 1)
            {
                if (Lives == 1)
                {
                    --Health;
                    --Lives;

                    GameStatsUI.Instance.UpdateHealthLives(Health, Lives);
                    GameManager.Instance.GameOver();
                    Destroy(gameObject);
                }
                else
                {
                    --Lives;
                    Health = DefaultHealth;
                    GameStatsUI.Instance.UpdateHealthLives(Health, Lives);
                }
            }
            else
            {
                --Health;
                GameStatsUI.Instance.UpdateHealthLives(Health, Lives);
            }
        }
    }

    public void HealUp()
    {
        if (Health >= 5 && Lives < 5)
        {
            ++Lives;
            Health = 1;
        }
        else if (Lives >= 5)
        {
        }
        else
        {
            ++Health;
        }
    }

    public void LivesUp()
    {
        if (Lives >= 5)
        {
            Health = 5;
        }
        else
        {
            ++Lives;
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

        _mobileInput = _mobileJoystick.InputVector.x;
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

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || _mobileInput < 0) && transform.position.x > MaxLeft)
        {
            direction = Vector3.left;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || _mobileInput > 0) && transform.position.x < MaxRight)
        {
            direction = Vector3.right;
        }

        if (_mobileInput == 0)
        {
            _shipSpeed = DefaultShipSpeed;
        }
        else
        {
            _shipSpeed *= Mathf.Abs(_mobileInput);
        }

        transform.position += _shipSpeed * Time.deltaTime * direction;
    }

    private void UpdateShooting()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || _shootButton.ButtonPressed) && _readyToShoot && LaserCounter <= 0)
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
