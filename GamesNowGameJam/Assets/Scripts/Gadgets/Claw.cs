using System;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Claw : Gadget
{
    [SerializeField] private Transform _claw;
    [SerializeField] private BoxCollider2D _boxCollider;

    [SerializeField] private SpriteRenderer _pillarRenderer;
    [SerializeField] private SpriteRenderer _clawSpriteRenderer;
    [SerializeField] private Sprite _grabbedSprite;
    [SerializeField] private Sprite _releasedSprite;

    [SerializeField] private float _minExtendTime = 0.5f;
    [SerializeField] private float _maxExtendTime = 1f;

    private Part _caughtPart;
    private float _clawDefaultYPosition;
    private bool _active;

    private Tween _pillarTween;
    private Vector2 _pillarDefaultSize;

    public override string GadgetName => "Claw";

    public override string FirstParametreName => string.Empty;

    public override string SecondParametreName => string.Empty;

    public override float FirstMinimumParametreValue => throw new NotImplementedException();

    public override float FirstMaximumParametreValue => throw new NotImplementedException();

    public override float SecondMinimumParametreValue => throw new NotImplementedException();

    public override float SecondMaxParametreValue => throw new NotImplementedException();

    public override Action<float> FirstParametreMethod => throw new NotImplementedException();

    public override Action<float> SecondParametreMethod => throw new NotImplementedException();

    public override bool HasSecondParametre => false;

    public override float FirstParametreValue => throw new NotImplementedException();

    public override float SecondParametreValue => throw new NotImplementedException();

    private void Start()
    {
        _clawDefaultYPosition = _claw.localPosition.y;
        _pillarDefaultSize = _pillarRenderer.size;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_placed) return;

        if (collision.TryGetComponent<Part>(out var component) && !_active)
        {
            if (component._rigidBody.gravityScale == 0) return;

            var part = component.transform;

            part.SetParent(transform, true);
            var maxReach = part.localPosition.y;
            part.SetParent(null, true);

            var time = CalculateTime(maxReach);

            Activate(component, maxReach, time);
        }
    }


    private void Activate(Part part, float maxReach, float extendTime)
    {
        if (_active) return;

        _active = true;

        Animate(maxReach, extendTime);

        _claw.DOLocalMoveY(maxReach, extendTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            GrabPart(part);
            Retract();
        });
    }

    private void Retract()
    {
        _claw.DOLocalMoveY(_clawDefaultYPosition, _maxExtendTime * 3).SetEase(Ease.Linear).OnComplete(ReleasePart);
    }


    private void GrabPart(Part part)
    {
        _caughtPart = part;


        _caughtPart.ResetVelocity();
        _caughtPart._rigidBody.gravityScale = 0;

        _caughtPart.transform.parent = _claw;

        _caughtPart.transform.DOLocalMove(Vector2.zero, 0.1f);

        // visual change

        _clawSpriteRenderer.sprite = _grabbedSprite;
    }

    private void ReleasePart()
    {
        _active = false;

        _caughtPart.transform.parent = null;
        _caughtPart._rigidBody.gravityScale = 1;


        // visual change

        _clawSpriteRenderer.sprite = _releasedSprite;
    }

    private float CalculateTime(float height)
    {
        var maxExtend = _boxCollider.bounds.max.y;
        var time = _maxExtendTime * height / maxExtend;
        time = Mathf.Clamp(time, _minExtendTime, _maxExtendTime);
        return time;
    }

    private void Animate(float targetDistance, float extendTime)
    {
        _pillarTween?.Kill();

        _pillarTween = DOTween.To(
            () => _pillarRenderer.size,
            x => _pillarRenderer.size = x,
            new Vector2(_pillarDefaultSize.x, targetDistance),
            extendTime
        )
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            _pillarTween = DOTween.To(
                () => _pillarRenderer.size,
                x => _pillarRenderer.size = x,
                _pillarDefaultSize,
                _maxExtendTime * 3
            ).SetEase(Ease.Linear);
        });
    }
}
