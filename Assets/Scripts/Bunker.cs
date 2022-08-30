using UnityEngine;

public class Bunker : MonoBehaviour
{
    public Sprite[] conditions;
    private int healhtLevel;
    public SpriteRenderer currentCondition;
    void Start()
    {
        healhtLevel = 6;
        currentCondition = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Missile") ||
            collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            --healhtLevel;
            if (healhtLevel <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                currentCondition.sprite = conditions[healhtLevel];
            }
        }
    }
}
