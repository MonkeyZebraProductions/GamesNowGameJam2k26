using UnityEngine;

public class GadgetParametreWindow : GadgetWindow
{
    [SerializeField] private bool _modifySecondParametreInstead;

    protected override void UpdateText(float value)
    {
        value = Mathf.Floor(value);

        var parametreName = !_modifySecondParametreInstead ? _gadget.FirstParametreName : _gadget.SecondParametreName;

        _textRef.SetText($"{parametreName}: {value}");
    }

    public override void Initialize(Gadget gadget)
    {
        _gadget = gadget;

        if (string.IsNullOrEmpty(gadget.FirstParametreName))
        {
            gameObject.SetActive(false);

            return;
        }

        if (!gadget.HasSecondParametre && _modifySecondParametreInstead)
        {
            gameObject.SetActive(false);

            return;
        }

        _handwheel.ResetSubscriptions();

        if (!_modifySecondParametreInstead)
        {
            _handwheel.OnValueChanged += _gadget.FirstParametreMethod;
            UpdateText(_gadget.FirstMinimumParametreValue);
            _handwheel.Initialize(_gadget.FirstParametreValue, _gadget.FirstMinimumParametreValue, _gadget.FirstMaximumParametreValue);
        }
        else
        {
            _handwheel.OnValueChanged += _gadget.SecondParametreMethod;
            UpdateText(_gadget.SecondMinimumParametreValue);
            _handwheel.Initialize(_gadget.SecondParametreValue, _gadget.SecondMinimumParametreValue, _gadget.SecondMaxParametreValue);
        }

        _handwheel.OnValueChanged += UpdateText;
    }
}
