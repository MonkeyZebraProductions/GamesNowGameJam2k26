using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnGadget : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    private GameObject gadgetToSpawn;
    private GameObject gadget;
    private GadgetPallet gadgetPallet;
    private void Start()
    {
        gadgetPallet=GetComponentInParent<GadgetPallet>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        gadget = Instantiate(gadgetToSpawn,transform.position,Quaternion.identity);
        gadgetPallet.OpenClosePallet();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
        if (gadget != null) { gadget.GetComponent<DragableObject>().enabled = false; }
    }
}
