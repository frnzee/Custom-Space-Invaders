using UnityEngine;

public class Invader : MonoBehaviour
{
    public void Kill()
    {
        GameManager.invaders.Remove(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SpaceShip") ||
            other.gameObject.CompareTag("Boundary"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
