using UnityEngine;

public class Invader : MonoBehaviour
{
    private const int BonusItemDropChance = 14;
    private const float DestroyTime = 0.5f;

    [SerializeField] private BonusItem _bonusItemPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    private GameObject _explosion;
    
    public void Kill()
    {
        if (Random.Range(0, 100) < BonusItemDropChance)
        {
            var bonusItem = Instantiate(_bonusItemPrefab, transform.position, Quaternion.identity);
        }

        GameManager.Instance.invaders.Remove(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SpaceShip>())
        {
            other.gameObject.GetComponent<SpaceShip>().ShipTakesDamage();

            _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_explosion, DestroyTime);

            GameManager.Instance.invaders.Remove(this);
            Destroy(gameObject);
        }

        if (other.gameObject.GetComponent<Boundary>())
        {
            GameManager.Instance.GameOver();
        }
    }
}
