using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField]
    private bool moveLeft;
    [SerializeField]
    [Range(0.5f, 20f)]
    private float speed = 5f;
    private List<Rigidbody2D> storedRigidbodies;
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.rigidbody.MovePosition((Vector2)collision.transform.position + (moveLeft ? Vector2.left:Vector2.right)*Time.fixedDeltaTime*speed);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb2D = collision.rigidbody;
        rb2D.AddForceX(speed-rb2D.linearVelocityX, ForceMode2D.Impulse);
    }
}
