using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] invaderPrefabs;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject Bunkers;
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject UI_Canvas;
    [SerializeField] private GameObject SpinningEarth;
    [SerializeField] private GameObject InvadersSpawnField;

    [SerializeField] private SpaceShip spaceShipPrefab;

    [SerializeField] private CanvasGroup StartMenuCanvas;
    [SerializeField] private CanvasGroup GameOverMenuCanvas;

    [SerializeField] private int rows;
    [SerializeField] private int columns;

    public static List<GameObject> invaders = new();

    private SpaceShip spaceShip = null;

    private Vector3 hDistance = new(0.5f, 0, 0);
    private Vector3 vDistance = new(0, 0.25f, 0);

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

    private enum GameState
    {
        None,
        Game,
        Fail
    }

    private GameState _currentGameState = GameState.None;

    private void Start()
    {
        GameOverMenu.SetActive(false);
        StartMenu.SetActive(true);
        UI_Canvas.SetActive(false);
    }

    public void StartGame()
    {
        _currentGameState = GameState.Game;

        SpinningEarth.SetActive(true);
        Bunkers.SetActive(true);
        UI_Canvas.SetActive(true);

        _fadeOut = true;

        if (spaceShip == null)
        {
            SpawnSpaceShip();
        }

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
        _width = 3 * (columns - 1);
        _height = 3 * (rows - 1);

        for (int row = 0; row < rows; ++row)
        {
            Vector2 centerOffset = new(-_width / 2, -_height / 2);
            Vector2 rowPosition = new(centerOffset.x, (3 * row) + centerOffset.y);

            for (int column = 0; column < columns; ++column)
            {
                if (Random.Range(0, 2) == 1)
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
    }

    private IEnumerator MissileShoot()
    {
        if (invaders.Count > 0)
        {
            _isMissileLaunched = true;

            Vector2 position = invaders[Random.Range(0, invaders.Count)].transform.position;
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
        _fadeIn = true;

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
        ++_winCount;
        _moveSpeed -= 0.005f;
        _maxMoveSpeed -= 0.02f;
        _missilesCooldown -= 0.025f;

        Debug.Log("Win count: " + _winCount);
        Debug.Log("Move speed: " + _moveSpeed);
        Debug.Log("Missiles cooldown: " + _missilesCooldown);
        Debug.Log("Max speed: " + _maxMoveSpeed);

        StartGame();
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
            if (GameOverMenuCanvas.alpha <= 1)
            {
                GameOverMenuCanvas.alpha += Time.deltaTime;
                if (GameOverMenuCanvas.alpha >= 1)
                {
                    _fadeIn = false;
                }
            }
        }

        if (!_isMissileLaunched)
        {
            StartCoroutine(MissileShoot());
        }

        if (invaders.Count <= 0 && _currentGameState == GameState.Game)
        {
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
    }
}
