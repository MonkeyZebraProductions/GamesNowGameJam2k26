using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Part>())
        {
            Destroy(collision.gameObject);
        }
    }
}
