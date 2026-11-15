using TMPro;
using UnityEngine;

public class GadgetWindow : MonoBehaviour
{
    [SerializeField] protected Handwheel _handwheel;
    [SerializeField] protected TMP_Text _textRef;
    protected Gadget _gadget;

    private void RotateGadget(float value)
    {
        var rotation = new Vector3(0, 0, -value);
        _gadget.transform.localEulerAngles = rotation;
    }

    protected virtual void UpdateText(float value)
    {
        value = Mathf.Floor(value);
        _textRef.SetText($"Rotation: {value}");
    }

    public virtual void Initialize(Gadget gadget)
    {
        _gadget = gadget;

        UpdateText(0);
        _handwheel.ResetSubscriptions();
        _handwheel.Initialize(gadget.transform.rotation.z, 0, 360);
        _handwheel.OnValueChanged += RotateGadget;
        _handwheel.OnValueChanged += UpdateText;
    }
}
