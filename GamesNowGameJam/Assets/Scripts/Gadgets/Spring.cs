using System;
using DG.Tweening;
using UnityEngine;


public class Spring : Gadget
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


    //Power
    public override string FirstParametreName => "Power";
    public override Action<float> FirstParametreMethod => SetSpringPower;


    public override string SecondParametreName => string.Empty;
    public override Action<float> SecondParametreMethod => null;

    public override float FirstMinimumParametreValue => _defaultSpringPower;

    public override float SecondMinimumParametreValue => -999;

    public override float FirstMaximumParametreValue => 35; // do with ScriptableObjects;

    public override float SecondMaxParametreValue => 999; // do with ScriptableObjects

    public override bool HasSecondParametre => false;

    public override string GadgetName => "Spring";

    public override float FirstParametreValue => _currentSpringPower;

    public override float SecondParametreValue => throw new NotImplementedException();

    void Awake()
    {
        SetSpringPower(_defaultSpringPower);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_placed) return;

        if (other.TryGetComponent<Part>(out var part))
        {
            PushPart(part);
            Animate();
        }
    }

    private void PushPart(Part part)
    {
        part.ResetVelocity();
        var upDirection = transform.up;
        var force = upDirection * _currentSpringPower;
        part.Push(force, ForceMode2D.Impulse);
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
        power = Mathf.Floor(power);
        _currentSpringPower = power;
    }

}
