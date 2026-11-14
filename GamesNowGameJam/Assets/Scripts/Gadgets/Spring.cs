using System;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public abstract class Gadget : MonoBehaviour
{
    public abstract string FirstParametreName { get; }
    public abstract string SecondParametreName { get; }

    public Action<float> FirstParametreMethod;
    public Action<float> SecondParametreMethod;
}


public class Spring : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _platform;
    [SerializeField] private SpriteRenderer _metalSprintSpriteRef;


    [Header("SPRING PARAMETRES")]
    [SerializeField] private float _defaultSpringPower = 2f;
    [SerializeField] private float _currentSpringPower;

    [Header("ANIMATION SETTINGS")]
    [SerializeField] private float _animationSpeed = 0.1f;

    [SerializeField] private float _platformDefaultPosition = 1f;
    [SerializeField] private float _platformMaxExtention = 3f;

    private Tween _springTween;

    public float DefaultSpringPower => _defaultSpringPower;

    public float CurrentSpringPower => _currentSpringPower;

    void Awake()
    {
        SetSpringPower(_defaultSpringPower);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Rigidbody2D>(out var part))
        {
            PushPart(part);
            Animate();
        }
    }

    private void PushPart(Rigidbody2D part)
    {
        part.linearVelocity = Vector2.zero;
        var upDirection = transform.up;
        part.AddForce(upDirection * _currentSpringPower, ForceMode2D.Impulse);
    }

    private void Animate()
    {
        _platform.DOLocalMoveY(_platformMaxExtention, _animationSpeed).From(_platformDefaultPosition).OnComplete(() =>
        {
            _platform.DOLocalMoveY(_platformDefaultPosition, _animationSpeed);
        });

        ExtendSpring(_platformMaxExtention);
    }


    private void ExtendSpring(float targetDistance)
    {
        _springTween?.Kill();

        _springTween = DOTween.To(
            () => _metalSprintSpriteRef.size,
            x => _metalSprintSpriteRef.size = x,
            new Vector2(_metalSprintSpriteRef.size.x, targetDistance),
            _animationSpeed
        ).OnComplete(() =>
        {
            _springTween = DOTween.To(
            () => _metalSprintSpriteRef.size,
            x => _metalSprintSpriteRef.size = x,
            new Vector2(_metalSprintSpriteRef.size.x, _platformDefaultPosition),
            _animationSpeed
        );
        });
    }

    public void SetSpringPower(float power)
    {
        _currentSpringPower = power;
    }
}
