using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    [Header("Game State")]
    [SerializeField] private EGameState _state;
    [SerializeField] private TextMeshProUGUI _stateText;

    [Header("UI")]
    [SerializeField] private UI_OptionPopup _optionPopupUI;
    
    
    private bool _isTopMode = false;
    public EGameState State => _state;
    public bool IsTopMode{ get => _isTopMode; }
    public static GameManager Instance => _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;
        
        LockCursor();
    }
    private void Start()
    {
        _state = EGameState.Ready;
        _optionPopupUI.Hide();

        StartCoroutine(StartToPlay_Coroutine());
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_optionPopupUI.gameObject.activeSelf)
            {
                Continue();
            }
            else
            {
                Pause();
                _optionPopupUI.Show();
               
            }
        }
    }
    
    public void GameOver()
    {
        _stateText.gameObject.SetActive(true);
        _stateText.text = "Game Over!";
        _state = EGameState.GameOver;
        
    }

    public void SwitchGameMode(EGameState gameState)
    {
        if (gameState == EGameState.Auto)
        {
            Cursor.lockState = CursorLockMode.Confined; 
            Cursor.visible = true;
            _state = EGameState.Auto;

            _isTopMode = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
            _state = EGameState.Playing;
            
            _isTopMode = false;
        }
       
        
        
    }
    
    
    private IEnumerator StartToPlay_Coroutine()
    {
        _stateText.text = "Ready..";
        yield return new WaitForSeconds(2f);
        
        _stateText.text = "Go!";
        yield return new WaitForSeconds(0.2f);
        _state = EGameState.Playing;
        _stateText.gameObject.SetActive(false);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        UnlockCursor();
    }

    public void Continue()
    {
        Time.timeScale = 1;
        _optionPopupUI.Hide();

        // 게임 모드에 따라 커서 상태 복원
        if (_state == EGameState.Playing)
        {
            LockCursor();
        }
        else if (_state == EGameState.Auto)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); // 어플리케이션 종료
        #endif
    }
}
