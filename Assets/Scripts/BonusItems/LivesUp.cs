using UnityEngine;

public class LivesUp : MonoBehaviour
{
    private void Start()
    {
        if (SpaceShip.Instance.Lives <= 5)
        {
            SpaceShip.Instance.LivesUp();
            GameStatsUI.Instance.UpdateHealthLives(SpaceShip.Instance.Health, SpaceShip.Instance.Lives);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
