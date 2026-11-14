using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Magnet : Gadget
{
    [SerializeField] private CircleCollider2D _magnetFieldTrigger;
    [SerializeField] private Transform _magnetFieldVisual;
    [SerializeField] private float _magnetPower = 2f;
    [SerializeField] private float _magnetRadius = 2f;
    [SerializeField] private int _canHandleAmountOfObjects = 4;
    [SerializeField] private bool _active = true;
    [SerializeField] private float _shutdownTime = 3f;

    private int _objectsInField = 0;

    public override string FirstParametreName => "Power";
    public override float FirstMinimumParametreValue => 1;
    public override float FirstMaximumParametreValue => 25;
    public override Action<float> FirstParametreMethod => SetMagnetPower;


    public override string SecondParametreName => "Radius";
    public override float SecondMinimumParametreValue => 1;
    public override float SecondMaxParametreValue => 5;
    public override Action<float> SecondParametreMethod => SetMagnetRadius;

    public override bool HasSecondParametre => true;
    public override string GadgetName => "Magnet";

    private void Awake()
    {
        _magnetFieldTrigger.radius = _magnetRadius;
    }

    private void SetMagnetPower(float value)
    {
        _magnetPower = value;
    }

    private void SetMagnetRadius(float value)
    {
        _magnetRadius = value;
        _magnetFieldTrigger.radius = _magnetRadius;
        _magnetFieldVisual.localScale = _magnetFieldTrigger.radius * 2 * Vector3.one;
    }



    void OnTriggerStay2D(Collider2D collision)
    {
        if (_active && collision.TryGetComponent<Rigidbody2D>(out var component))
        {
            var center = transform.position;
            var partPosition = component.transform.position;

            var directionTowardsCenter = center - partPosition;
            directionTowardsCenter = directionTowardsCenter.normalized;

            component.AddForce(directionTowardsCenter * Mathf.Abs(_magnetPower), ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_active && collision.TryGetComponent<Rigidbody2D>(out var component))
        {
            _objectsInField++;
            _objectsInField = Mathf.Clamp(_objectsInField, 0, 100);

            if (_objectsInField > _canHandleAmountOfObjects)
            {
                Shutdown();
            }
        }
    }

    private void Shutdown()
    {
        var waitForShutdown = new WaitForSeconds(_shutdownTime);

        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            _active = false;
            _objectsInField = 0;
            _magnetFieldVisual.gameObject.SetActive(false);
            yield return waitForShutdown;
            _magnetFieldVisual.gameObject.SetActive(true);
            _active = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out var component))
        {
            _objectsInField--;
            _objectsInField = Mathf.Clamp(_objectsInField, 0, 100);
        }
    }
}
