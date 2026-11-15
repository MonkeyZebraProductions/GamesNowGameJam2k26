using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class AirPump : Gadget
{
    [SerializeField] private float _pumpInterval = 1;
    [SerializeField] private float _pumpWorkingTime = 2;
    [SerializeField] private float _pumpPower;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private GameObject _pumpEffect;

    [SerializeField] private bool _resetsVelocity;

    private WaitForSeconds _waitForPumpInterval;
    private WaitForSeconds _waitForPumpWorking;

    public override string FirstParametreName => "Interval";

    public override float FirstMinimumParametreValue => 1f;
    public override float FirstMaximumParametreValue => 2f;
    public override Action<float> FirstParametreMethod => ChangeInterval;


    public override string SecondParametreName => string.Empty;

    public override float SecondMinimumParametreValue => -999;

    public override float SecondMaxParametreValue => 999;


    public override Action<float> SecondParametreMethod => null;

    public override bool HasSecondParametre => false;
    public override string GadgetName => "AirPump";

    public override float FirstParametreValue => _pumpInterval;

    public override float SecondParametreValue => throw new NotImplementedException();

    void Start()
    {
        _waitForPumpInterval = new(_pumpInterval);
        _waitForPumpWorking = new(_pumpWorkingTime);

        StartCoroutine(PumpCoroutine());
    }

    private void ChangeInterval(float value)
    {
        _pumpInterval = value;
        _waitForPumpInterval = new(_pumpInterval);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_placed) return;

        if (collision.TryGetComponent<Part>(out var component))
        {
            if (_resetsVelocity)
            {
                component.ResetVelocity();
            }

            component.TryFill();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!_placed) return;

        if (collision.TryGetComponent<Part>(out var component))
        {
            var direction = transform.up;
            var force = direction * Mathf.Abs(_pumpPower);

            component.Push(force, ForceMode2D.Force);
        }
    }


    private IEnumerator PumpCoroutine()
    {
        yield return new WaitUntil(() => _placed);

        yield return _waitForPumpInterval;

        Activate();

        yield return _waitForPumpWorking;

        Deactivate();

        yield return PumpCoroutine();
    }

    private void Deactivate()
    {
        //visuals here

        _pumpEffect.gameObject.SetActive(false);

        _collider2D.enabled = false;
    }

    private void Activate()
    {
        //visuals here

        _pumpEffect.gameObject.SetActive(true);

        _collider2D.enabled = true;
    }
}
