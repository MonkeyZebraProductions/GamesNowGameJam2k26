
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Handwheel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("REFERENCES")]
    [SerializeField] private RectTransform _wheel;

    [Header("VALUE SETTINGS")]
    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 100f;
    [SerializeField] private float _value;

    public event Action<float, bool> OnValueChanged;

    public float Value => _value;

    private float _previousAngle;

    public void OnPointerDown(PointerEventData eventData)
    {
        _previousAngle = GetPointerAngle(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float currentAngle = GetPointerAngle(eventData);
        float deltaAngle = Mathf.DeltaAngle(_previousAngle, currentAngle);

        float modifiedValue = 0.1f * deltaAngle;

        float newValue = Mathf.Clamp(
            _value - modifiedValue,
            _minValue,
            _maxValue);

        // Calculate the actual value change after clamping.
        float actualValueChange = newValue - _value;

        // Convert the actual value change back into rotation.
        float actualRotation = -actualValueChange / 0.1f;

        _value = newValue;

        _wheel.Rotate(0f, 0f, actualRotation);

        var isIncreased = _previousAngle > currentAngle;

        _previousAngle = currentAngle;


        OnValueChanged?.Invoke(_value, isIncreased);
    }

    private float GetPointerAngle(PointerEventData eventData)
    {
        Vector2 direction = eventData.position -
            RectTransformUtility.WorldToScreenPoint(
                eventData.pressEventCamera,
                _wheel.position);

        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void Initialize(float startingValue)
    {
        _value = startingValue;
    }
}
