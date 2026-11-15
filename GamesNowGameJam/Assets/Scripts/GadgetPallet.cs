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
    [HideInInspector]
    public RectTransform palletParent;
    public RectTransform startPos, endPos;
    public bool _isOpen;
    [SerializeField]
    private PanCamera panCamera;

    [SerializeField]
    private float tweenTime = 0.3f;
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
        palletParent.transform.DOMove(_isOpen ? startPos.position : endPos.position, tweenTime);
        openClosePallet.GetComponentInChildren<TextMeshProUGUI>().transform.DOLocalRotate(_isOpen ? Vector3.zero : new Vector3(0, 0, 180), tweenTime);
        _isOpen = !_isOpen;
        if (panCamera != null)
        {
            panCamera.StartCoroutine(panCamera.LockZoom(tweenTime));
        }
        
    }

}
