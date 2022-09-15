using UnityEngine;
using TMPro;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelStatement;
    [SerializeField] private TextMeshProUGUI _counter;
    [SerializeField] private GameObject _counterPanel;

    private int _currentLevel;
    private float _cooldownTimer = 1f;
    private int _countNumber = 3;
    private bool _buttonPressed = false;

    public void LevelStart(int level, bool buttonPressed)
    {
        _counterPanel.SetActive(true);
        _currentLevel = level;
        _buttonPressed = buttonPressed;
    }

    private void Update()
    {
        if (_buttonPressed && _countNumber >= 0)
        {
            _levelStatement.text = "LEVEL " + _currentLevel;
            if (_countNumber == 0)
            {
                _counter.text = "GO!";
            }
            else
            {
                _counter.text = _countNumber.ToString();
            }
            if (_cooldownTimer <= 0)
            {
                --_countNumber;
                _cooldownTimer = 1f;
            }

            _cooldownTimer -= Time.deltaTime;
        }
        else
        {
            _buttonPressed = false;
            _countNumber = 3;
            _counterPanel.SetActive(false);
            GameManager.Instance._currentGameState = GameManager.GameState.Game;
        }
    }
}
