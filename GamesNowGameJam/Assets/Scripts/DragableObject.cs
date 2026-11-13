using UnityEngine;
using UnityEngine.InputSystem;

public class DragableObject : MonoBehaviour
{

    private Vector3 mouseOffset;    

    private void OnMouseDown()
    {
        mouseOffset = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - mouseOffset.x, 
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).y-mouseOffset.y,
            0);
        
    }
}
