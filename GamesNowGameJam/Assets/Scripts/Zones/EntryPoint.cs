using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public delegate void TriggerEnterDelegate(GameObject go,Rigidbody2D rb2D);    
    public TriggerEnterDelegate TriggerEnter;
    [SerializeField]
    private string comparisonTag;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(comparisonTag))
        {
            TriggerEnter(collision.gameObject,collision.attachedRigidbody);
        }
        else
        {
            //DestroyScript
            Destroy(collision.gameObject);
        }
    }
}
