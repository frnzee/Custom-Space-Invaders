using UnityEngine;
[System.Serializable]
public class SpaceShipStats
{
    [Range(1, 5)]
    public int maxHealth;
    [Range(1, 5)]
    public int maxLives;
    [HideInInspector]
    public int currentHealth = 3;
    [HideInInspector]
    public int currentLives = 3;

    public float shipSpeed;
    public float missileSpeed;
}
