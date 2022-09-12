using UnityEngine;

public class Bunker : MonoBehaviour
{
    private const float DestroyTime = 0.5f;

    [SerializeField] private Sprite[] _conditions;
    [SerializeField] private GameObject _explosionPrefab;

    private SpriteRenderer _currentCondition;
    private GameObject _explosion;

    private int _healthLevel = 6;

    private void Start()
    {
        _currentCondition = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Missile>() ||
            other.gameObject.GetComponent<Laser>())
        {
            --_healthLevel;
            if (_healthLevel <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _currentCondition.sprite = _conditions[_healthLevel];
            }
        }
        
        if (other.gameObject.GetComponent<Invader>())
        {
            --_healthLevel;

            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, DestroyTime);

            if (_healthLevel <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _currentCondition.sprite = _conditions[_healthLevel];
            }

        }
    }
}
