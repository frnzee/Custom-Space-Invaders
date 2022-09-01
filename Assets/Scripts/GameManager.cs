using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] invaderPrefabs;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject GameMenu;
    [SerializeField] private GameObject Bunkers;
    [SerializeField] private CanvasGroup GameMenuCanvas;
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    public static List<GameObject> invaders = new List<GameObject>();

    private Vector3 hDistance = new Vector3(0.5f, 0, 0);
    private Vector3 vDistance = new Vector3(0, 0.1f, 0);

    private bool _isMissileLaunched = false;
    private bool _isMovingRight;
    private bool _fadeOut = false;

    private float _width;
    private float _height;
    private float _missilesCooldown = 3f;
    private float _maxLeft = -25f;
    private float _maxRight = 25f;
    private float _moveTimer = 1f;
    private float _moveSpeed = 0.05f;
    private float _maxMoveSpeed = 0.1f;

    private void Awake()
    {
        _width = 3 * (columns - 1);
        _height = 3 * (rows - 1);
    }

    private void Start()
    {

    }

    public void StartGame()
    {
        _fadeOut = true;
        
        for (int row = 0; row < rows; ++row)
        {
            Vector2 centerOffset = new Vector2(-_width / 2, -_height / 2);
            Vector2 rowPosition = new Vector2(centerOffset.x, (3 * row) + centerOffset.y);

            for (int column = 0; column < columns; ++column)
            {
                GameObject invader = Instantiate(invaderPrefabs[row], transform);
                Vector2 position = rowPosition;
                position.x += 3 * column;
                invader.transform.localPosition = position;
            }
        }

        foreach (GameObject invader in GameObject.FindGameObjectsWithTag("Invaders"))
        {
            invaders.Add(invader);
        }

        Bunkers.SetActive(true);
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
            }
            _moveTimer = GetMoveSpeed();
            _missilesCooldown -= 0.005f;
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
        Reset();
    }

    public void Win()
    {
        Reset();
    }

    private void Update()
    {
        if (_fadeOut)
        {
            if (GameMenuCanvas.alpha >= 0)
            {
                GameMenuCanvas.alpha -= Time.deltaTime;
                if (GameMenuCanvas.alpha <= 0)
                {
                    _fadeOut = false;
                    GameMenu.SetActive(false);
                }

            }
        }
        if (!_isMissileLaunched)
        {
            StartCoroutine(MissileShoot());
        }
        if (invaders.Count <= 0)
        {
            //Reset();
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
