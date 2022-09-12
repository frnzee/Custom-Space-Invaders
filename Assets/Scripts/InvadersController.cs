using UnityEngine;

public class InvadersController : MonoBehaviour
{
    [SerializeField] private GameObject _missilePrefab;

    private Vector3 _horizontalDistance = new(0.5f, 0, 0);
    private Vector3 _verticalDistance = new(0, 0.25f, 0);

    private bool _isMovingRight;

    private float _moveTimer = 1f;
    private float _moveSpeed = 0.025f;
    private float _maxMoveSpeed = 0.4f;
    private float _missilesCooldown = 3f;
    private readonly float _defaultMissilesCooldown = 3f;

    private const float MaxLeft = -24f;
    private const float MaxRight = 22f;
    private const float MissilesCooldownReducer = 0.05f;

    private void MoveInvaders()
    {
        if (GameManager.Instance.invaders.Count > 0)
        {
            int hitSide = 0;

            foreach (var invader in GameManager.Instance.invaders)
            {
                if (_isMovingRight)
                {
                    invader.transform.position += _horizontalDistance;
                }
                else
                {
                    invader.transform.position -= _horizontalDistance;
                }
                if (invader.transform.position.x > MaxRight || invader.transform.position.x < MaxLeft)
                {
                    ++hitSide;
                }
            }

            if (hitSide > 0)
            {
                foreach (Invader invader in GameManager.Instance.invaders)
                {
                    invader.transform.position -= _verticalDistance;
                }
                _isMovingRight = !_isMovingRight;

                _missilesCooldown -= MissilesCooldownReducer;
            }
        }
    }

    private void MissileShoot()
    {
        if (GameManager.Instance.invaders.Count > 0)
        {
            Vector2 position = GameManager.Instance.invaders[Random.Range(0, GameManager.Instance.invaders.Count)].transform.position;
            Instantiate(_missilePrefab, position, Quaternion.identity);
        }
    }

    private float GetMoveSpeed()
    {
        return Mathf.Max(GameManager.Instance.invaders.Count * _moveSpeed, _maxMoveSpeed);
    }

    public void UpdateInvadersSpeed(float speedReducer, float maxSpeedReducer, float shootCooldownReducer)
    {
        _moveSpeed -= speedReducer;
        _maxMoveSpeed -= maxSpeedReducer;
        _missilesCooldown -= shootCooldownReducer;
    }

    private void Update()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Game)
        {
            _moveTimer -= Time.deltaTime;
            if (_moveTimer <= 0)
            {
                MoveInvaders();
                _moveTimer = GetMoveSpeed();
            }

            _missilesCooldown -= Time.deltaTime;
            if (_missilesCooldown <= 0)
            {
                MissileShoot();
                _missilesCooldown = _defaultMissilesCooldown;
            }
        }
    }
}
