using Unity.VisualScripting;
using UnityEngine;

public enum PartType
{
    Wheel,
    Engine,
    Chasis
}
public class Part : MonoBehaviour
{
    public PartType partType;
    public float Health = 100;
    public Color CurrentColor;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    private float minDamageSpeed = 5;
    [HideInInspector]
    public bool _isFeuled;
    [HideInInspector]
    public bool _isColoured;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentColor = spriteRenderer.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the impact speed between the two objects
        float impactSpeed = collision.relativeVelocity.magnitude;

        if(impactSpeed > minDamageSpeed)
        {
            Health-=impactSpeed;
        }
        Debug.Log(Health);

        if(Health < 0)
        {
            Destroy(gameObject);
        }
    }

    public void ColourCar(Color color)
    {
        Debug.Log("Color");
        _isColoured = true;
        CurrentColor = color;
        spriteRenderer.color = color;
    }

    //public void
}
