using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public record Car
{
    public int requiredWheels;
    public bool fullEngine;
    public bool colouredCar;
    public Color requiredCarColour;
}

public class BaseZone : MonoBehaviour
{

    [Range(0f, 5f)]
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

    [SerializeField]
    [Range(2f, 10f)]
    private float launchForce;

    [SerializeField]
    private Resource money;

    [SerializeField]
    private int destroyAmount;

    public Car CarRecepie;
    private Car BaseCar = new Car();
    private Car CurrentCar = new Car();

    private List<GameObject> AllCarParts = new List<GameObject>();
    private BoxCollider2D boxCollider2D;


    private CallAfterDelay callAfterDelay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        foreach (var entryPoint in EntryPoints)
        {
            if (entryPoint.gameObject.activeSelf)
            {
                entryPoint.TriggerEnter += PutInZone;
                break;
            }
        }
        foreach (var exitPoint in ExitPoints)
        {
            if (exitPoint.gameObject.activeSelf)
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

    private void PutInZone(GameObject go, Rigidbody2D rb2D)
    {

        containedObject = go;
        containedRB2D = rb2D;

        if (containedObject == null) return;

        //containedRB2D.linearVelocity = Vector3.zero;

        //containedObject.transform.position = GetRandomPointInCollider(boxCollider2D,1);
        //containedRB2D.useFullKinematicContacts = false;
        if (ExecutionTime != 0)
        {
            callAfterDelay = CallAfterDelay.Create(ExecutionTime, () =>
            {
                EventExecute();
            });
        }
        else
        {
            ExecutionEvent?.Invoke();
        }
    }

    Vector2 GetRandomPointInCollider(Collider2D collider, float offset)
    {
        Vector2 RandomPoint = new Vector2(Random.Range(collider.bounds.min.x + offset, collider.bounds.max.x - offset),
            Random.Range(collider.bounds.min.y + offset, collider.bounds.max.y - offset));
        return RandomPoint;
    }

    private void EventExecute()
    {
        if (containedObject == null) return;

        ExecutionEvent?.Invoke();
        if (ActiveExitPoint != null)
        {
            Vector2 forceDirection = ActiveExitPoint.position - ExecutionPosition.position;
            containedRB2D.transform.position = ActiveExitPoint.position;
            //containedRB2D.AddForce(forceDirection.normalized * launchForce, ForceMode2D.Impulse);
        }
        containedRB2D.useFullKinematicContacts = true;
    }

    public void ColourCar()
    {
        if (containedObject == null) return;

        Part part = containedObject.GetComponent<Part>();
        if (part != null && part.partType == PartType.Chasis)
        {
            Debug.Log("Should Color");
            part.ColourCar(carColor);
        }
        else
        {
            //money.Add(-destroyAmount);
            Destroy(containedObject);
        }
    }

    public void FillEngine()
    {
        if (containedObject == null) return;

        Part part = containedObject.GetComponent<Part>();
        if (part != null && part.partType == PartType.Engine)
        {
            part._isFeuled = true;
        }
        else
        {
            Destroy(containedObject);
            //money.Add(-destroyAmount);
        }
    }

    public void AddToCompletion()
    {
        if (containedObject == null) return;

        Part part = containedObject.GetComponent<Part>();

        switch (part.partType)
        {
            case PartType.Engine:
                if (part._isFeuled && !CurrentCar.fullEngine)
                {
                    AllCarParts.Add(containedObject);
                    CurrentCar.fullEngine = true;
                    break;
                }
                else
                {
                    Debug.Log("Should destroy Engine");
                    //money.Add(-destroyAmount);
                    Destroy(containedObject);
                    break;
                }
            case PartType.Chasis:
                if (part.CurrentColor != CurrentCar.requiredCarColour && !CurrentCar.colouredCar && part._isColoured)
                {
                    AllCarParts.Add(containedObject);
                    CurrentCar.colouredCar = true;
                    CurrentCar.requiredCarColour = part.CurrentColor;
                    break;
                }
                else
                {
                    Debug.Log("Should destroy Chasis");
                    //money.Add(-destroyAmount);
                    Destroy(containedObject);
                    break;
                }
            case PartType.Wheel:
                if (CurrentCar.requiredWheels < 4 && part._isFilled)
                {
                    AllCarParts.Add(containedObject);
                    CurrentCar.requiredWheels++;
                    break;
                }
                else
                {
                    //money.Add(-destroyAmount);
                    Debug.Log("Should destroy Wheel");
                    Destroy(containedObject);
                    break;
                }
        }


        if (CurrentCar == CarRecepie)
        {
            Debug.Log("Winner");
            money.Add(1000);
            CurrentCar = BaseCar;

            foreach (GameObject carPart in AllCarParts)
            {
                Destroy(carPart);
            }
        }
    }
}
