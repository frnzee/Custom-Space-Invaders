using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvadersBase : MonoBehaviour
{
    [SerializeField] private GameObject _missilePrefab;

    private Vector3 _horizontalDistance = new(0.5f, 0, 0);
    private Vector3 _verticalDistance = new(0, 0.25f, 0);

    private bool _isMovingRight;

    private float _moveTimer = 1f;
    private float _moveSpeed = 0.02f;
    private float _maxMoveSpeed = 0.3f;
    private float _missilesCooldown = 3f;
    private float _defaultMissilesCooldown = 3f;

    private const float MaxLeft = -25f;
    private const float MaxRight = 25f;
    private const float MissilesCooldownReducer = 0.05f;

    private void MoveInvaders()
    {
        if (GameManager.invaders.Count > 0)
        {
            int hitSide = 0;

            foreach (var invader in GameManager.invaders)
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
                foreach (Invader invader in GameManager.invaders)
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
        if (GameManager.invaders.Count > 0)
        {
            Vector2 position = GameManager.invaders[Random.Range(0, GameManager.invaders.Count)].transform.position;
            Instantiate(_missilePrefab, position, Quaternion.identity);
        }
    }

    private float GetMoveSpeed()
    {
        float moveSpeed = GameManager.invaders.Count * _moveSpeed;

        if (moveSpeed < _maxMoveSpeed)
        {
            return _maxMoveSpeed;
        }

        return moveSpeed;
    }

    public void UpdateInvadersSpeed(float speedReducer, float maxSpeedReducer, float shootCooldownReducer)
    {
        _moveSpeed -= speedReducer;
        _maxMoveSpeed -= maxSpeedReducer;
        _missilesCooldown -= shootCooldownReducer;
    }

    private void Update()
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
