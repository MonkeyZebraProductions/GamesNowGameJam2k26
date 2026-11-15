
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "ScriptableObjects/Resource")]
public class Resource : ScriptableObject
{
    [Header("RESOURCE SETTINGS")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _amount;

    public Sprite Icon => _icon;
    public int Amount => _amount;

    public event Action<int> OnAmountChanged;

    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        _amount += amount;
        OnAmountChanged?.Invoke(_amount);
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || _amount < amount)
            return false;

        _amount -= amount;
        OnAmountChanged?.Invoke(_amount);

        return true;
    }

    public void SetAmount(int amount)
    {
        _amount = Mathf.Max(0, amount);
        OnAmountChanged?.Invoke(_amount);
    }
}
