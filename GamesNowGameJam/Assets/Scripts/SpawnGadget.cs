using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnGadget : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    private GameObject gadgetToSpawn;
    private GameObject gadget;
    public void OnPointerDown(PointerEventData eventData)
    {
        gadget = Instantiate(gadgetToSpawn,transform.position,Quaternion.identity);
        //gadget.
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
        if (gadget != null) { gadget.GetComponent<DragableObject>().enabled = false; }
    }
}
