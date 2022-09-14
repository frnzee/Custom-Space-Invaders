using UnityEngine;

public class BonusItem : MonoBehaviour
{
    private const float BonusItemSpeed = 5f;

    [SerializeField] private GameObject[] _bonusItemPrefabs;
    [SerializeField] private Sprite[] _bonusItemSprites;

    private SpriteRenderer _currentItemSprite;
    private int _currentItem;

    public void Start()
    {
        _currentItem = Random.Range(0, _bonusItemSprites.Length);
        _currentItemSprite = GetComponent<SpriteRenderer>();
        _currentItemSprite.sprite = _bonusItemSprites[_currentItem];

        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SpaceShip>())
        {
            Instantiate(_bonusItemPrefabs[_currentItem]);
            Destroy(gameObject);
        }
        if (other.gameObject.GetComponent<Boundary>())
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(BonusItemSpeed * Time.deltaTime * Vector2.down);
    }
}
