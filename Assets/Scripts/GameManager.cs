using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
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

    private const float SpeedReducer = 0.005f;
    private const float MaxSpeedReducer = 0.02f;
    private const float MissilesCooldownReducer = 0.05f;
    private const int ShiftSpaceShipDown = -12;
    private const int InvadersInterval = 3;

    [SerializeField] private GameObject _invadersSpawnField;
    [SerializeField] private Invader[] _invaderPrefabs;
    [SerializeField] private SpaceShip _spaceShipPrefab;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private InvadersController _invadersController;
    [SerializeField] private LevelCounter _levelCounter;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;

    public List<Invader> invaders = new();

    public GameState _currentGameState = GameState.None;

    private SpaceShip _spaceShip;
    private Vector2 _defaultShipPosition;

    private int _currentLevel = 1;

    private float _width;
    private float _height;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _gameUI.InitializeMenu();
        GameStatsUI.Instance.DisableGameStats();
    }

    public void StartGame()
    {
        _levelCounter.LevelStart(_currentLevel, true);
        GameStatsUI.Instance.InitGameStats();
        _gameUI.StartMenuFadeOut();

        if (!_spaceShip)
        {
            SpawnSpaceShip();
        }

        SpawnInvaders();
    }

    private void SpawnSpaceShip()
    {
        _defaultShipPosition = new Vector2(0, ShiftSpaceShipDown);
        _spaceShip = Instantiate(_spaceShipPrefab, transform);
        _spaceShip.transform.position = _defaultShipPosition;
    }

    private void SpawnInvaders()
    {
        _width = InvadersInterval * (_columns - 1);
        _height = InvadersInterval * (_rows - 1);

        for (int i = 0; i < _rows; ++i)
        {
            var centerOffset = new Vector2(-_width / 2, -_height / 2);
            var rowPosition = new Vector2(centerOffset.x, centerOffset.y + 3 * i);

            for (int j = 0; j < _columns; ++j)
            {
                if (Random.Range(0, 2) == 1)
                {
                    Invader invader = Instantiate(_invaderPrefabs[i], _invadersSpawnField.transform);
                    Vector2 position = rowPosition;
                    position.x += InvadersInterval * j;
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

        foreach (var invader in invaders)
        {
            Destroy(invader.gameObject);
        }

        invaders.Clear();

        Destroy(_spaceShip);
    }

    private void Win()
    {
        _currentGameState = GameState.None;

        _invadersController.UpdateInvadersSpeed(SpeedReducer, MaxSpeedReducer, MissilesCooldownReducer);

        ++_currentLevel;

        _spaceShip.transform.position = _defaultShipPosition;

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
