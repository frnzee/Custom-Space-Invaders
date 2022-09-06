using UnityEngine;

public class Bunker : MonoBehaviour
{
    [SerializeField] private Sprite[] _conditions;
    [SerializeField] private GameObject _explosionPrefab;

    private SpriteRenderer currentCondition;
    private GameObject _explosion;

    private int _healhtLevel;


    private void Start()
    {
        _healhtLevel = 6;
        currentCondition = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Missile") ||
            other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            --_healhtLevel;
            if (_healhtLevel <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                currentCondition.sprite = _conditions[_healhtLevel];
            }
        }
        
        if (other.gameObject.CompareTag("Invaders"))
        {
            --_healhtLevel;
            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, 0.5f);

            if (_healhtLevel <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                currentCondition.sprite = _conditions[_healhtLevel];
            }

        }
    }
}
