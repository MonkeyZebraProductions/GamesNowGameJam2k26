using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BaseZone : MonoBehaviour
{

    [Range(0.5f,5f)]
    public float ExecutionTime;
    [SerializeField]
    private EntryPoint[] EntryPoints;
    [SerializeField]
    private Transform[] ExitPoints;

    [SerializeField]
    private UnityEvent ExecutionEvent;
    private GameObject containedObject;
    private Rigidbody2D containedRB2D;
    [SerializeField]
    private Transform ExecutionPosition;
    private Transform ActiveExitPoint;
    [SerializeField]
    private Color carColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var entryPoint in EntryPoints)
        {
            if(entryPoint.gameObject.activeSelf)
            {
                entryPoint.TriggerEnter += PutInZone;
                break;
            }
        }
        foreach (var exitPoint in ExitPoints)
        {
            if(exitPoint.gameObject.activeSelf)
            {
                ActiveExitPoint = exitPoint;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   private void PutInZone(GameObject go,Rigidbody2D rb2D)
    {
        containedObject = go;
        containedRB2D = rb2D;
        containedRB2D.linearVelocity = Vector3.zero;
        containedObject.transform.position = ExecutionPosition.position;
        CallAfterDelay.Create(ExecutionTime, () => { EventExecute(); });
    }

    private void EventExecute()
    {
        //ExecutionEvent?.Invoke();
        Vector2 forceDirection = ActiveExitPoint.position - ExecutionPosition.position;
        containedRB2D.AddForce(forceDirection.normalized*20,ForceMode2D.Impulse);
    }

    public void ColourCar()
    {
        //containedObject.GetComponent<Part>.ColourCar(carColor)
    }

    public void FillEngine()
    {
        //containedObject.GetComponent<Part>.FillEngine
    }
}
