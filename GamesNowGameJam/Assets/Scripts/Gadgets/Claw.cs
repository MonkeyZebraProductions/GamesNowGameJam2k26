using DG.Tweening;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [SerializeField] private Transform _claw;
    [SerializeField] private BoxCollider2D _boxCollider;
    //[SerializeField] private float _maxReach;
    [SerializeField] private float _minExtendTime = 0.5f;
    [SerializeField] private float _maxExtendTime = 1f;

    private Rigidbody2D _caughtPart;
    private float _clawDefaultYPosition;
    private bool _active;

    private void Start()
    {
        _clawDefaultYPosition = _claw.localPosition.y;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out var component) && !_active)
        {
            var part = component.transform;

            part.SetParent(transform, true);
            var maxReach = part.localPosition.y;
            part.SetParent(null, true);

            var time = CalculateTime(maxReach);

            Activate(component, maxReach, time);
        }
    }


    private void Activate(Rigidbody2D rigidbody2D, float maxReach, float extendTime)
    {
        if (_active) return;

        _active = true;

        _claw.DOLocalMoveY(maxReach, extendTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            GrabPart(rigidbody2D);
            Retract();
        });
    }

    private void Retract()
    {
        _claw.DOLocalMoveY(_clawDefaultYPosition, _minExtendTime).SetEase(Ease.Linear).OnComplete(ReleasePart);
    }


    private void GrabPart(Rigidbody2D rigidbody2D)
    {
        _caughtPart = rigidbody2D;


        _caughtPart.linearVelocity = Vector2.zero;
        _caughtPart.gravityScale = 0;

        _caughtPart.transform.parent = _claw;

        _caughtPart.transform.DOLocalMove(Vector2.zero, 0.1f);
    }

    private void ReleasePart()
    {
        _active = false;

        _caughtPart.transform.parent = null;
        _caughtPart.gravityScale = 1;
    }

    private float CalculateTime(float height)
    {
        var maxExtend = _boxCollider.bounds.max.y;
        var time = _maxExtendTime * height / maxExtend;
        Debug.Log("time");
        time = Mathf.Clamp(time, _minExtendTime, _maxExtendTime);

        return time;
    }
}
