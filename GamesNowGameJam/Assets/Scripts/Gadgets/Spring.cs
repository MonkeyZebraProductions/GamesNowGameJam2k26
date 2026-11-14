using System;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public abstract class Gadget : MonoBehaviour
{
    protected bool _placed = false;

    public abstract string GadgetName { get; }

    public abstract string FirstParametreName { get; }
    public abstract string SecondParametreName { get; }

    public abstract float FirstMinimumParametreValue { get; }
    public abstract float FirstMaximumParametreValue { get; }
    public abstract float SecondMinimumParametreValue { get; }
    public abstract float SecondMaxParametreValue { get; }

    public abstract Action<float> FirstParametreMethod { get; }
    public abstract Action<float> SecondParametreMethod { get; }

    public abstract bool HasSecondParametre { get; }
    public bool Placed => _placed;

    public void Place()
    {
        _placed = true;
    }
}


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

    void Awake()
    {
        SetSpringPower(_defaultSpringPower);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Placed) return;

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
        power = Mathf.Floor(power);
        _currentSpringPower = power;
    }

}
