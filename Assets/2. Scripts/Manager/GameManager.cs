using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    [SerializeField] private EGameState _state;
    [SerializeField] private TextMeshProUGUI _stateText;
    public EGameState State => _state;

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
