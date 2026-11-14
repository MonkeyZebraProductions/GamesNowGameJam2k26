using TMPro;
using UnityEngine;

public class GadgetParametreWindow : MonoBehaviour
{
    [SerializeField] private Spring _spring;
    [SerializeField] private Handwheel _handwheel;
    [SerializeField] private TMP_Text _textRef;

    private void Start()
    {
        _handwheel.OnValueChanged += RotateGadget;
    }

    private void RotateGadget(float value, bool isIncreased)
    {
        var rotation = new Vector3(0, 0, -value);
        _spring.transform.localEulerAngles = rotation;

    }

}
