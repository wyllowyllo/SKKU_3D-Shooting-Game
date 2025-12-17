using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    [SerializeField] private EGameState _state;
    [SerializeField] private TextMeshProUGUI _stateText;

    private bool _isTopMode = false;
    public EGameState State => _state;
    public bool IsTopMode{ get => _isTopMode; set => _isTopMode = value; }
    public static GameManager Instance => _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;
    }
    private void Start()
    {
        _state = EGameState.Ready;
       

        StartCoroutine(StartToPlay_Coroutine());
    }

    
    public void GameOver()
    {
        _stateText.gameObject.SetActive(true);
        _stateText.text = "Game Over!";
        _state = EGameState.GameOver;
        
    }
    
    
    
    private IEnumerator StartToPlay_Coroutine()
    {
        _stateText.text = "Ready";
        yield return new WaitForSeconds(2f);
        
        _stateText.text = "Go!";
        yield return new WaitForSeconds(0.2f);
        _state = EGameState.Playing;
        _stateText.gameObject.SetActive(false);
    }

   
    
    
}
