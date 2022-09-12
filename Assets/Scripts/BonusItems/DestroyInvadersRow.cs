using UnityEngine;

public class DestroyInvadersRow : MonoBehaviour
{
    private const float DestroyTime = 0.5f;

    [SerializeField] private GameObject _explosionPrefab;

    private Vector2 _minPosition = new(0, 999999);

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

            for (int i = 0; i < GameManager.Instance.invaders.Count;)
            {
                if (GameManager.Instance.invaders[i].transform.position.y <= _minPosition.y)
                {
                    Vector3 currentPosition = GameManager.Instance.invaders[i].transform.position;

                    GameObject _explosion = Instantiate(_explosionPrefab, currentPosition, Quaternion.identity);

                    Destroy(_explosion, DestroyTime);

                    Destroy(GameManager.Instance.invaders[i].gameObject);
                    GameManager.Instance.invaders.Remove(GameManager.Instance.invaders[i]);
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
