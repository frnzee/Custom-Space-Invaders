using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        None,
        Game,
        Fail
    }

    private static GameManager _instance;
    public static GameManager Instance
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

    private GameState _currentGameState = GameState.None;

    [SerializeField] private GameObject _invadersSpawnField;
    [SerializeField] private Invader[] _invaderPrefabs;
    [SerializeField] private SpaceShip _spaceShipPrefab;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GameStats _gameStats;
    [SerializeField] private InvadersBase _invadersBase;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;

    public static List<Invader> invaders = new();

    private SpaceShip _spaceShip = null;

    private int _winCount = 0;

    private float _width;
    private float _height;

    private const int ShiftSpaceShipDown = -12;
    private const float SpeedReducer = 0.005f;
    private const float MaxSpeedReducer = 0.02f;
    private const float MissilesCooldownReducer = 0.05f;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _gameUI.InitializeMenu();
        _gameStats.DisableGameStats();
    }

    public void StartGame()
    {
        _currentGameState = GameState.Game;

        _gameStats.InitGameStats();
        _gameUI.StartMenuFadeOut();

        if (_spaceShip == null)
        {
            SpawnSpaceShip();
        }

        SpawnInvaders();
    }

    private void SpawnSpaceShip()
    {
        Vector2 shipPosition = new(0, ShiftSpaceShipDown);
        _spaceShip = Instantiate(_spaceShipPrefab, transform);
        _spaceShip.transform.position = shipPosition;
    }

    private void SpawnInvaders()
    {
        _width = 3 * (_columns - 1);
        _height = 3 * (_rows - 1);

        for (int i = 0; i < _rows; ++i)
        {
            var centerOffset = new Vector2(-_width / 2, -_height / 2);
            var rowPosition = new Vector2(centerOffset.x, 3 * i + centerOffset.y);

            for (int j = 0; j < _columns; ++j)
            {
                if (Random.Range(0, 2) == 1)
                {
                    Invader invader = Instantiate(_invaderPrefabs[i], _invadersSpawnField.transform);
                    Vector2 position = rowPosition;
                    position.x += 3 * j;
                    invader.transform.localPosition = position;
                    invaders.Add(invader);
                }
            }
        }
    }

    public void GameOver()
    {
        _currentGameState = GameState.Fail;

        _gameUI.GameOverMenuFadeIn();

        foreach (Invader invader in invaders)
        {
            Destroy(invader);
        }
        invaders.Clear();

        Destroy(_spaceShip);
    }

    private void Win()
    {
        ++_winCount;
        _invadersBase.UpdateInvadersSpeed(SpeedReducer, MaxSpeedReducer, MissilesCooldownReducer);
        Debug.Log("Win count: " + _winCount);
        StartGame();
    }

    private void Update()
    {
        if (invaders.Count <= 0 && _currentGameState == GameState.Game)
        {
            Win();
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("SpaceInvadersScene");
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
