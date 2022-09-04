using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] invaderPrefabs;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject Bunkers;
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject SpinningEarth;
    [SerializeField] private GameObject InvadersSpawnField;

    [SerializeField] private SpaceShip spaceShipPrefab;

    [SerializeField] private CanvasGroup StartMenuCanvas;

    [SerializeField] private int rows;
    [SerializeField] private int columns;

    public SpaceShipStats spaceShipStats;
    private SpaceShip spaceShip;
    private Action<int> _currentSpaceShipHealth;

    public static List<GameObject> invaders = new();

    private Vector3 hDistance = new(0.5f, 0, 0);
    private Vector3 vDistance = new(0, 0.2f, 0);

    private bool _isMissileLaunched = false;
    private bool _isMovingRight = true;
    private bool _fadeOut = false;
    private bool _fadeIn = false;

    private int _winCount = 0;


    private float _width;
    private float _height;
    private float _missilesCooldown = 3f;
    private float _moveSpeed = 0.02f;
    private float _maxMoveSpeed = 0.3f;
    private float _moveTimer = 1f;
    private readonly float _maxLeft = -25f;
    private readonly float _maxRight = 25f;

    private Action<int> _healthDelegate;

    private enum GameState
    {
        None,
        Game,
        Win,
        Fail
    }

    public void GetHealth(Action<int> health)
    {
        _currentSpaceShipHealth = health;
    }

    private GameState _currentGameState = GameState.None;

    private void Start()
    {
        GameOverMenu.SetActive(false);
        StartMenu.SetActive(true);
    }

    public void StartGame()
    {
        _currentGameState = GameState.Game;

        SpinningEarth.SetActive(true);
        Bunkers.SetActive(true);

        _fadeOut = true;

        _width = 3 * (columns - 1);
        _height = 3 * (rows - 1);

        SpawnSpaceShip();

        SpawnInvaders();
    }

    private void SpawnSpaceShip()
    {
        Vector2 shipPosition = new(0, -12f);
        spaceShip = Instantiate(spaceShipPrefab, transform);
        spaceShip.transform.position = shipPosition;
        spaceShip.Initialize(FailGameDelegate);
    }

    private void SpawnInvaders()
    {
        for (int row = 0; row < rows; ++row)
        {
            Vector2 centerOffset = new(-_width / 2, -_height / 2);
            Vector2 rowPosition = new(centerOffset.x, (3 * row) + centerOffset.y);

            for (int column = 0; column < columns; ++column)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    GameObject invader = Instantiate(invaderPrefabs[row], InvadersSpawnField.transform);
                    Vector2 position = rowPosition;
                    position.x += 3 * column;
                    invader.transform.localPosition = position;
                }
            }
        }

        foreach (GameObject invader in GameObject.FindGameObjectsWithTag("Invaders"))
        {
            invaders.Add(invader);
        }
    }

    private void FailGameDelegate(bool gameFailed)
    {
        if (gameFailed)
        {
            GameOver();
        }
        else
        {
            Debug.Log("success delegate arrived to the final destination");
        }
    }

    private IEnumerator MissileShoot()
    {
        if (invaders.Count > 0)
        {
            _isMissileLaunched = true;

            Vector2 position = invaders[UnityEngine.Random.Range(0, invaders.Count)].transform.position;
            Instantiate(missilePrefab, position, Quaternion.identity);

            yield return new WaitForSeconds(_missilesCooldown);

            _isMissileLaunched = false;
        }
    }

    private void MoveInvaders()
    {
        if (invaders.Count > 0)
        {
            int hitSide = 0;
            for (int i = 0; i < invaders.Count; ++i)
            {
                if (_isMovingRight)
                {
                    invaders[i].transform.position += hDistance;
                }
                else
                {
                    invaders[i].transform.position -= hDistance;
                }
                if (invaders[i].transform.position.x > _maxRight || invaders[i].transform.position.x < _maxLeft)
                {
                    ++hitSide;
                }
            }

            if (hitSide > 0)
            {
                for (int i = 0; i < invaders.Count; ++i)
                {
                    invaders[i].transform.position -= vDistance;
                }
                _isMovingRight = !_isMovingRight;

                _missilesCooldown -= 0.05f;
            }

            _moveTimer = GetMoveSpeed();
        }
    }

    private float GetMoveSpeed()
    {
        float moveSpeed = invaders.Count * _moveSpeed;

        if (moveSpeed < _maxMoveSpeed)
        {
            return _maxMoveSpeed;
        }
        else
        {
            return moveSpeed;
        }
    }

    public void GameOver()
    {
        _currentGameState = GameState.Fail;

        SpinningEarth.SetActive(false);
        Bunkers.SetActive(false);
        GameOverMenu.SetActive(true);

        invaders.Clear();

        GameObject[] spawnedInvaders = GameObject.FindGameObjectsWithTag("Invaders");
        foreach (GameObject invader in spawnedInvaders)
        {
            Destroy(invader);
        }
        Destroy(spaceShip);
    }

    public void Win()
    {
        if (_currentGameState == GameState.Win)
        {
            _healthDelegate(spaceShipStats.currentHealth);
            Debug.Log("Delegate came");
            Destroy(GameObject.FindGameObjectWithTag("SpaceShip"));
            ++_winCount;
            _moveSpeed -= 0.005f;
            _missilesCooldown -= 0.025f;

            Debug.Log("Win count: " + _winCount);
            Debug.Log(_moveSpeed);
            Debug.Log(_missilesCooldown);

            StartGame();
            
        }
    }

    private void Update()
    {
        if (_fadeOut)
        {
            if (StartMenuCanvas.alpha >= 0)
            {
                StartMenuCanvas.alpha -= Time.deltaTime;
                if (StartMenuCanvas.alpha <= 0)
                {
                    _fadeOut = false;
                    StartMenu.SetActive(false);
                }
            }
        }

        if (_fadeIn)
        {
            if (StartMenuCanvas.alpha >= 0)
            {
                StartMenuCanvas.alpha -= Time.deltaTime;
                if (StartMenuCanvas.alpha <= 0)
                {
                    _fadeIn = false;
                    StartMenu.SetActive(false);
                }
            }
        }

        if (!_isMissileLaunched)
        {
            StartCoroutine(MissileShoot());
        }

        if (invaders.Count <= 0 && _currentGameState == GameState.Game)
        {
            _currentGameState = GameState.Win;
            Win();
        }

        if (_moveTimer <= 0)
        {
            MoveInvaders();
        }
        _moveTimer -= Time.deltaTime;
    }

    public void Reset()
    {
        SceneManager.LoadScene("SpaceInvadersScene");
        Debug.Log("reset");
    }
}
