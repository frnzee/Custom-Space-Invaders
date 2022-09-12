using UnityEngine;

public class LivesUp : MonoBehaviour
{
    private void Start()
    {
        if (SpaceShip.Instance._lives < 5)
        {
            ++SpaceShip.Instance._lives;
            GameStatsUI.Instance.UpdateHealthLives(SpaceShip.Instance._health, SpaceShip.Instance._lives);
            Destroy(gameObject);
        }
        else if (SpaceShip.Instance._lives == 5)
        {
            SpaceShip.Instance._health = 5;
            GameStatsUI.Instance.UpdateHealthLives(SpaceShip.Instance._health, SpaceShip.Instance._lives);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
