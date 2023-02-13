using UnityEngine;

public class HealthUp : MonoBehaviour
{
    private void Start()
    {
        if (SpaceShip.Instance.Health <= 5)
        {
            SpaceShip.Instance.HealUp();
            GameStatsUI.Instance.UpdateHealthLives(SpaceShip.Instance.Health, SpaceShip.Instance.Lives);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
