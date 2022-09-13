using UnityEngine;
using UnityEngine.UI;

public class GameStatsUI : MonoBehaviour
{
    private static GameStatsUI _instance;
    public static GameStatsUI Instance
    {
        get
        {
            if (!_instance)
            {
                Debug.LogError("Instance not specified");
            }
            return _instance;
        }
    }

    [SerializeField] private Sprite[] _healthSprites;
    [SerializeField] private Sprite[] _livesSprites;

    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _livesBar;

    [SerializeField] private Image _healthBarFill;
    [SerializeField] private Image _livesBarFill;

    private void Awake()
    {
        _instance = this;
    }

    public void InitGameStats()
    {
        _healthBar.SetActive(true);
        _livesBar.SetActive(true);
    }

    public void UpdateHealthLives(int health, int lives)
    {
        _healthBarFill.sprite = _healthSprites[health];
        _livesBarFill.sprite = _livesSprites[lives];
    }

    public void DisableGameStats()
    {
        _healthBar.SetActive(false);
        _livesBar.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
