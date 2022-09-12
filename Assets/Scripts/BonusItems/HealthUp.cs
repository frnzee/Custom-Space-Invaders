using UnityEngine;

public class HealthUp : MonoBehaviour
{
    private void Start()
    {
        if (SpaceShip.Instance._health < 5)
        {
            ++SpaceShip.Instance._health;
            GameStatsUI.Instance.UpdateHealthLives(SpaceShip.Instance._health, SpaceShip.Instance._lives);
            Destroy(gameObject);
        }
        else if (SpaceShip.Instance._health == 5 && SpaceShip.Instance._lives < 5)
        {
            ++SpaceShip.Instance._lives;
            SpaceShip.Instance._health = 1;
            GameStatsUI.Instance.UpdateHealthLives(SpaceShip.Instance._health, SpaceShip.Instance._lives);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
