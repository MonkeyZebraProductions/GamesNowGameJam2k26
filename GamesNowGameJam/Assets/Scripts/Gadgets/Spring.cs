using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Spring : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _platform;
    [SerializeField] private SpriteRenderer _metalSprintSpriteRef;


    [Header("SPRING PARAMETRES")]
    [SerializeField] private float _springPower = 2f;

    [Header("ANIMATION SETTINGS")]
    [SerializeField] private float _animationSpeed = 0.1f;

    [SerializeField] private float _platformDefaultPosition = 1f;
    [SerializeField] private float _platformMaxExtention = 3f;

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
        part.AddForce(upDirection * _springPower, ForceMode2D.Impulse);
    }

    private void Animate()
    {
        _platform.DOLocalMoveY(_platformMaxExtention, _animationSpeed).From(_platformDefaultPosition).OnComplete(() =>
        {
            _platform.DOLocalMoveY(_platformDefaultPosition, _animationSpeed);
        });

        ExtendSpring(_platformMaxExtention);
    }

    private Tween _springTween;

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
}
