using UnityEngine;

public class SpriteColliderResizer : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _boxCollider2D.size = new Vector3(_spriteRenderer.bounds.size.x / transform.lossyScale.x,
                                     _spriteRenderer.bounds.size.y / transform.lossyScale.y,
                                     _spriteRenderer.bounds.size.z / transform.lossyScale.z);
    }
}
