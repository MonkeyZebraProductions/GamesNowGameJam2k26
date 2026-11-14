using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetPallet : MonoBehaviour
{
    [SerializeField]
    private Button openClosePallet;
    private RectTransform palletParent;
    public RectTransform startPos, endPos;
    private bool _isOpen;
    [SerializeField]
    private PanCamera panCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        palletParent = GetComponent<RectTransform>();
        
        openClosePallet.onClick.AddListener(OpenClosePallet);    
        
        if(panCamera == null)
        {
            panCamera=FindAnyObjectByType<PanCamera>();
        }
    }    

    public void OpenClosePallet()
    {
        palletParent.transform.DOMove(_isOpen ? startPos.position : endPos.position,1f);
        openClosePallet.GetComponentInChildren<TextMeshProUGUI>().transform.DOLocalRotate(_isOpen ? Vector3.zero : new Vector3(0, 0, 180), 0.3f);
        _isOpen = !_isOpen;
        if (panCamera != null)
        {
            panCamera.StartCoroutine(panCamera.LockZoom(1f));
        }
        
    }

}
