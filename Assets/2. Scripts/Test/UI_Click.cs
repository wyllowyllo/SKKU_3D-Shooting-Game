using System;
using TMPro;
using UnityEngine;

public class UI_Click : MonoBehaviour
{
    public TextMeshProUGUI _leftClickText;
    public TextMeshProUGUI _rightClickText;
    
    private void Start()
    {
       Refresh();

       ClickManager.Instance.OnDataChanged += Refresh;
    }

    private void Refresh()
    {
        _leftClickText.text = $"왼쪽 클릭 : {ClickManager.Instance.LeftClickCnt}";
        _rightClickText.text = $"오른쪽 클릭 : {ClickManager.Instance.RightClickCnt}";
    }
}