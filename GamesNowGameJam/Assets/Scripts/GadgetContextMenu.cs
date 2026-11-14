using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GadgetContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    [SerializeField] private TMP_Text _titleTextRef;

    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _closeButton;

    //[SerializeField] private LayerMask _layerMask;

    [SerializeField] private GadgetWindow[] _windows;

    private Gadget _lastSelectedGadget;
    private PanCamera _panCamera;

    private void Awake()
    {
        _sellButton.onClick.AddListener(Sell);
        _closeButton.onClick.AddListener(ClosePanel);
        _panCamera = FindAnyObjectByType<PanCamera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1000);

            if (hit.collider == null) return;

            var gadgetSelectionArea = hit.collider.GetComponent<GadgetSelectionArea>();

            var gadget = gadgetSelectionArea.Gadget;

            if (!gadget.Placed) return;

            InitializePanel(gadget);
        }
    }

    private void ShowPanel()
    {
        _body.SetActive(true);
    }

    private void InitializePanel(Gadget gadget)
    {
        _lastSelectedGadget = gadget;

        _titleTextRef.SetText(gadget.GadgetName);

        ShowPanel();


        foreach (var window in _windows)
        {
            window.Initialize(gadget);
        }

        _panCamera.MoveAndZoomToTarget(gadget.transform.position);
    }

    public void ClosePanel()
    {
        _lastSelectedGadget = null;

        _body.SetActive(false);

        _panCamera.ZoomOutOfTarget();
    }

    private void Sell()
    {
        Destroy(_lastSelectedGadget.gameObject);
        ClosePanel();
    }
}
