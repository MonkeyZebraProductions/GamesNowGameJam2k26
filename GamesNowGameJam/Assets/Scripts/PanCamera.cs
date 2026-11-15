using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanCamera : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction panAction, zoomAction;
    private bool _isPaning, _lockZoom, _canPan;
    private Vector2 mouseStartPos, mouseDelta, cameraStartPos;
    [SerializeField]
    private float mouseDivisor = 10f;
    [SerializeField]
    private float zoomDivisor = 2f;
    [SerializeField]
    private float minClamp = 5f;
    [SerializeField]
    private float maxClamp = 30f;
    [SerializeField]
    private Vector3 offset;
    private Vector3 storedPosition;
    [SerializeField]
    private GameObject Pallet;
    private void Awake()
    {
        //Assigns variables at start of Runtime
        playerInput = GetComponent<PlayerInput>();
        panAction = playerInput.actions["PanCameraButton"];
        zoomAction = playerInput.actions["ZoomCamera"];
        _canPan = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_lockZoom)
        {
            float zoomChange = zoomAction.ReadValue<float>() / zoomDivisor;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomChange, minClamp, maxClamp);
        }

        if (_isPaning && _canPan)
        {
            mouseDelta = Mouse.current.position.ReadValue() - mouseStartPos;
            Vector3 cameraPos = cameraStartPos + mouseDelta / (mouseDivisor - Camera.main.orthographicSize);
            Camera.main.transform.position = new Vector3(cameraPos.x, cameraPos.y, -10);
        }
        if (DOTween.IsTweening(Camera.main.transform) && Pallet.activeSelf)
        {
            Pallet.SetActive(false);
        }
        else if (!DOTween.IsTweening(Camera.main.transform) && !Pallet.activeSelf && _canPan)
        {
            Pallet.SetActive(true);
        }
    }

    void PanButtonDown()
    {
        _isPaning = true;
        cameraStartPos = Camera.main.transform.position;
        mouseStartPos = Mouse.current.position.ReadValue();
    }
    void PanButtonUp()
    {
        _isPaning = false;
    }
    public IEnumerator LockZoom(float LockTime)
    {
        _lockZoom = true;
        yield return new WaitForSeconds(LockTime);
        _lockZoom = false;
    }
    void OnEnable()
    {

        playerInput.actions.Enable();
        panAction.started += context => PanButtonDown();
        panAction.canceled += context => PanButtonUp();
    }

    void OnDisable()
    {
        playerInput.actions.Disable();
        panAction.started -= context => PanButtonDown();
        panAction.canceled -= context => PanButtonUp();
    }

    public void MoveAndZoomToTarget(Vector2 targetPosition)
    {
        if (!_canPan) return;

        Camera camera = Camera.main;
        camera.transform.position = storedPosition;
        Vector3 realTarget = new Vector3(targetPosition.x, targetPosition.y, -10f) + offset;
        camera.transform.DOMove(realTarget, 1f);
        camera.DOOrthoSize(5f, 1f);
        _canPan = false;
        _lockZoom = true;
        Pallet.SetActive(false);
    }

    public void ZoomOutOfTarget()
    {
        Camera camera = Camera.main;
        camera.transform.DOMove(new Vector3(storedPosition.x, storedPosition.y, -10f), 1f);
        camera.DOOrthoSize(20f, 1f);
        _canPan = true;
        _lockZoom = false;
    }
}
