using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_OptionPopup : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _gameExitButton;

    private void Awake()
    {
        _continueButton.onClick.AddListener(GameContinue);
        _restartButton.onClick.AddListener(GameRestart);
        _gameExitButton.onClick.AddListener(GameExit);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void GameContinue()
    {
        GameManager.Instance.Continue();
    }

    private void GameRestart()
    {
        GameManager.Instance.Restart();
    }

    private void GameExit()
    {
        GameManager.Instance.Quit();
    }
}
