using UnityEngine;
using UnityEngine.InputSystem;

public class DragableObject : MonoBehaviour
{

    private Vector3 mouseOffset;
    //[HideInInspector]
    public bool spawned;
    private PanCamera panCamera;

    private bool _zoomedIn;

    [HideInInspector]
    public GameObject pallet;
    private void Start()
    {
        panCamera = FindAnyObjectByType<PanCamera>();
    }

    private void OnMouseEnter()
    {
        if (!spawned)
        {
            mouseOffset = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        }

    }

    private void Update()
    {
        if (!spawned)
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - mouseOffset.x,
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).y - mouseOffset.y,
            0);
        }
    }

}
