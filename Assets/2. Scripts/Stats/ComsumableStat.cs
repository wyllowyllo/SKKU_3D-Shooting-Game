using System;
using UnityEngine;

[System.Serializable]
public class ComsumableStat
{
    [SerializeField] private float _value;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _regenValue;

    private event Action _onDataChanged;
    
    public float Value => _value;
    public float MaxValue => _maxValue;
    public float RegenValue => _regenValue;

   

    public void Initialize(Action onDataChanged = null)
    {
        _value = MaxValue;
        _onDataChanged = onDataChanged;
    }
    public void Regenerate(float time)
    {
        _value = Value + RegenValue * time;
        _value = Mathf.Min(MaxValue, Value);
        
        _onDataChanged?.Invoke();
    }

    public bool TryConsume(float amount)
    {
        if (Value < amount) return false;
        
            Consume(amount);
            return true;
        
    }

    private void Consume(float amount)
    {
        _value = _value - amount;
        _onDataChanged?.Invoke();
    }

    public void IncreaseMax(float amount)
    {
        _maxValue = MaxValue + amount;
        _onDataChanged?.Invoke();
    }
    public void DecreaseMax(float amount)
    {
        _maxValue = MaxValue - amount;
        _onDataChanged?.Invoke();
    }

    public void Increase(float amount)
    {
        _value = _value + amount;
        _onDataChanged?.Invoke();
    }
    public void Decrease(float amount)
    {
        _value = _value - amount;
        _onDataChanged?.Invoke();
    }
    
    
}
