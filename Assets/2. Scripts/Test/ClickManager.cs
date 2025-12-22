using System;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;

    private int _leftClickCnt = 0;
    private int _rightClickCnt = 0;

    public int LeftClickCnt => _leftClickCnt;

    public int RightClickCnt => _rightClickCnt;

    public event Action OnDataChanged;
    
    private void Awake()
    {

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _leftClickCnt += 1;
            
            OnDataChanged?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _rightClickCnt += 1;
            
            OnDataChanged?.Invoke();
        }
    }
}