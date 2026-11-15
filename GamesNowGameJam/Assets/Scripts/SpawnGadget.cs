using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnGadget : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Gadget gadgetToSpawn;
    private Gadget spawnedGadget;
    private GadgetPallet gadgetPallet;

    [SerializeField]
    private Resource money;
    private void Start()
    {
        gadgetPallet = GetComponentInParent<GadgetPallet>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (money.TrySpend(gadgetToSpawn.Price))
        {
            spawnedGadget = Instantiate(gadgetToSpawn, transform.position, Quaternion.identity);
            spawnedGadget.GetComponent<DragableObject>().pallet = gadgetPallet.gameObject;
            gadgetPallet.OpenClosePallet();

        }
       
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
        if (spawnedGadget != null && spawnedGadget.GetComponent<DragableObject>() != null)
        {
            spawnedGadget.Place();
            spawnedGadget.GetComponent<DragableObject>().enabled = false;
        }
    }
}
