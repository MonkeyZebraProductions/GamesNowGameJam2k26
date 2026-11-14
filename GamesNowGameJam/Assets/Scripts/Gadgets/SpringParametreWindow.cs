using TMPro;
using UnityEngine;

public class SpringParametreWindow : MonoBehaviour
{
    [SerializeField] private Spring _spring;
    [SerializeField] private Handwheel _handwheel;
    [SerializeField] private TMP_Text _textRef;


    private void Start()
    {
        _handwheel.Initialize(_spring.DefaultSpringPower);

        ModifySpringPower(_handwheel.Value, false);

        _handwheel.OnValueChanged += ModifySpringPower;
    }

    private void ModifySpringPower(float value, bool isIncreased)
    {
        value = Mathf.Floor(value);
        _spring.SetSpringPower(value);

        _textRef.SetText($"Power {_spring.CurrentSpringPower}");
    }
}
