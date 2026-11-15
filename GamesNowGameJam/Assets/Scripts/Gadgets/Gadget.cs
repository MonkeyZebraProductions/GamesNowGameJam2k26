using System;
using UnityEngine;

[SelectionBase]
public abstract class Gadget : MonoBehaviour
{
    [SerializeField] protected int _price = 10;
    [SerializeField] protected bool _placed = false;

    public abstract string GadgetName { get; }

    public abstract string FirstParametreName { get; }
    public abstract string SecondParametreName { get; }

    public abstract float FirstParametreValue { get; }
    public abstract float FirstMinimumParametreValue { get; }
    public abstract float FirstMaximumParametreValue { get; }

    public abstract float SecondParametreValue { get; }
    public abstract float SecondMinimumParametreValue { get; }
    public abstract float SecondMaxParametreValue { get; }

    public abstract Action<float> FirstParametreMethod { get; }
    public abstract Action<float> SecondParametreMethod { get; }

    public abstract bool HasSecondParametre { get; }
    public bool Placed => _placed;

    protected int Price => _price;

    public void Place()
    {
        _placed = true;
    }
}
