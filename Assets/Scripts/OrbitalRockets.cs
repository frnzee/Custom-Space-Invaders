using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalRockets : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 10f;
    private int _killCounter = 0;
    void Update()
    {
        transform.RotateAround(target.position, Vector3.back, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Invaders"))
        {
            if (_killCounter < 4)
            {
                collision.gameObject.GetComponent<Invaders>().Kill();
                ++_killCounter;
            }
            else
            {
                collision.gameObject.GetComponent<Invaders>().Kill();
                Destroy(gameObject);
            }
        }
    }
}
