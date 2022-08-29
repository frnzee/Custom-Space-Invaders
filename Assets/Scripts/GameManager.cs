using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] invaderPrefabs;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    public static List<GameObject> invaders = new List<GameObject>();

    private bool _isMissileLaunched = false;
    private float _missilesCooldown = 3f;

    private void Awake()
    {
        float width = 3 * (columns - 1);
        float height = 3 * (rows - 1);

        for (int row = 0; row < rows; ++row)
        {
            Vector2 centerOffset = new Vector2(-width / 2, -height / 2);
            Vector2 rowPosition = new Vector2(centerOffset.x, (3 * row) + centerOffset.y);

            for (int column = 0; column < columns; ++column)
            {
                GameObject invader = Instantiate(invaderPrefabs[row], transform);
                Vector2 position = rowPosition;
                position.x += 3 * column;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        foreach (GameObject invader in GameObject.FindGameObjectsWithTag("Invaders"))
        {
            invaders.Add(invader);
        }
    }

    private IEnumerator Shoot()
    {
        if (invaders.Count > 0)
        {
            _isMissileLaunched = true;

            Vector2 position = invaders[Random.Range(0, invaders.Count)].transform.position;
            Instantiate(missilePrefab, position, Quaternion.identity);
            yield return new WaitForSeconds(_missilesCooldown);

            _isMissileLaunched = false;
        }
    }
    public void GameOver()
    {
        Reset();
    }

    private void Update()
    {
        if (!_isMissileLaunched)
        {
            StartCoroutine(Shoot());
        }
        if (invaders.Count <= 0)
        {
            Reset();
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("SpaceInvadersScene");
    }
}
