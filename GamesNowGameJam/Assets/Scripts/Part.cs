using UnityEngine;

public enum PartType
{
    Wheel,
    Engine,
    Chasis
}

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class Part : MonoBehaviour
{
    public PartType partType;
    public float Health = 100;
    public Color CurrentColor;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private float minDamageSpeed = 5;
    [HideInInspector] public bool _isFeuled;
    public SpriteRenderer _filledWheelSprite;
    public bool _isColoured;
    public bool _isFilled;

    [HideInInspector] public Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CurrentColor = spriteRenderer.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the impact speed between the two objects
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > minDamageSpeed)
        {
            Health -= impactSpeed;
        }

        Debug.Log(Health);

        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }

    public void ColourCar(Color color)
    {
        Debug.Log("Color");

        if (partType != PartType.Chasis) return;

        spriteRenderer.color = color;
        CurrentColor = color;
        _isColoured = true;
    }

    public void ResetVelocity()
    {
        _rigidBody.linearVelocity = Vector3.zero;
    }

    public void Push(Vector2 force, ForceMode2D forceMode)
    {
        _rigidBody.AddForce(force, forceMode);
    }

    public void TryFill()
    {
        if (partType != PartType.Wheel) return;

        _isFilled = true;

        spriteRenderer.gameObject.SetActive(false);
        _filledWheelSprite.gameObject.SetActive(true);
    }

    public void TryColor(Color color)
    {

    }
}
