using UnityEngine;

public class DestroyInvadersRow : MonoBehaviour
{
    private const float DestroyTime = 0.5f;

    [SerializeField] private GameObject _destroyRowPrefab;
    [SerializeField] private BonusItem _bonusItemPrefab;

    private Vector2 _minPosition = new(0, Mathf.Infinity);

    private void Start()
    {
        if (GameManager.Instance.invaders.Count > 0)
        {
            foreach (var invader in GameManager.Instance.invaders)
            {
                if (invader.transform.position.y < _minPosition.y)
                {
                    _minPosition = invader.transform.position;
                }
            }

            var i = 0;
            while (i < GameManager.Instance.invaders.Count)
            {
                if (GameManager.Instance.invaders[i].transform.position.y <= _minPosition.y)
                {
                    Vector3 laserPosition = new Vector3(2.5f, GameManager.Instance.invaders[i].transform.position.y);

                    GameObject destroyRowLaser = Instantiate(_destroyRowPrefab, laserPosition, Quaternion.identity);
                    Destroy(destroyRowLaser, DestroyTime * 2);

                    GameManager.Instance.invaders[i].Kill();
                }
                else
                {
                    i++;
                }
            }
        }

        Destroy(gameObject);
    }
}
