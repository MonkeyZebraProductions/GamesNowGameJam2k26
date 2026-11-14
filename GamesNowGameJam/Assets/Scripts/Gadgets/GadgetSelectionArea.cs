using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GadgetSelectionArea : MonoBehaviour
{
    [SerializeField] private Gadget _gadget;

    public Gadget Gadget => _gadget;
}
