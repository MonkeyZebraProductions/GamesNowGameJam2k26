using UnityEngine;

[SelectionBase]
public class Spring : MonoBehaviour
{
    [SerializeField] private float _springPower = 2f;
    [SerializeField] private float _castRadius = 3f;
    [SerializeField] private Transform _castPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Rigidbody2D>(out var component))
        {
            component.linearVelocity = Vector2.zero;

            var upDirection = transform.up;
            component.AddForce(upDirection * _springPower, ForceMode2D.Impulse);
        }
    }
}
