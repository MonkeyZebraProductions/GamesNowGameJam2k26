using System.Collections;
using UnityEngine;

[SelectionBase]
public class AirPump : MonoBehaviour
{
    [SerializeField] private float _pumpInterval = 1;
    [SerializeField] private float _pumpWorkingTime = 2;
    [SerializeField] private float _pumpPower;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private GameObject _pumpEffect;

    [SerializeField] private bool _resetsVelocity;

    private WaitForSeconds _waitForPumpInterval;
    private WaitForSeconds _waitForPumpWorking;

    void Start()
    {
        _waitForPumpInterval = new(_pumpInterval);
        _waitForPumpWorking = new(_pumpWorkingTime);

        StartCoroutine(PumpCoroutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out var component))
        {
            if (_resetsVelocity)
            {
                component.linearVelocity = Vector3.zero;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out var component))
        {
            var direction = transform.up;

            component.AddForce(direction * Mathf.Abs(_pumpPower), ForceMode2D.Force);
        }
    }


    private IEnumerator PumpCoroutine()
    {
        yield return _waitForPumpInterval;

        Activate();

        yield return _waitForPumpWorking;

        Deactivate();

        yield return PumpCoroutine();
    }

    private void Deactivate()
    {
        Debug.Log("Stopped Working");

        //visuals here

        _pumpEffect.gameObject.SetActive(false);

        _collider2D.enabled = false;
    }

    private void Activate()
    {
        //visuals here

        _pumpEffect.gameObject.SetActive(true);

        Debug.Log("Started Working");

        _collider2D.enabled = true;
    }
}
