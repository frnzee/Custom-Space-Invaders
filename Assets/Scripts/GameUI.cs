using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _startGameMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private CanvasGroup _startMenuCanvasGroup;
    [SerializeField] private CanvasGroup _gameOverMenuCanvasGroup;
    [SerializeField] private GameObject _spinningEarth;
    [SerializeField] private GameObject _bunkers;

    private bool _fadeIn = false;
    private bool _fadeOut = false;

    public void InitializeMenu()
    {
        _gameOverMenu.SetActive(false);
        _startGameMenu.SetActive(true);
        _gameOverMenuCanvasGroup.alpha = 0;
    }

    public void StartMenuFadeOut()
    {
        _spinningEarth.SetActive(true);
        _bunkers.SetActive(true);

        _fadeOut = true;
        if (_startMenuCanvasGroup.alpha >= 0)
        {
            _startMenuCanvasGroup.alpha -= Time.deltaTime;
            if (_startMenuCanvasGroup.alpha <= 0)
            {
                _fadeOut = false;
                _startGameMenu.SetActive(false);
            }
        }
    }

    public void GameOverMenuFadeIn()
    {
        _fadeIn = true;
        _gameOverMenu.SetActive(true);
        if (_gameOverMenuCanvasGroup.alpha <= 1)
        {
            _gameOverMenuCanvasGroup.alpha += Time.deltaTime;
            if (_gameOverMenuCanvasGroup.alpha >= 1)
            {
                _fadeIn = false;
                _spinningEarth.SetActive(false);
                _bunkers.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (_fadeOut)
        {
            StartMenuFadeOut();
        }
        if (_fadeIn)
        {
            GameOverMenuFadeIn();
        }
    }
}
