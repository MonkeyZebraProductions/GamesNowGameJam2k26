using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GadgetPallet : MonoBehaviour
{
    [SerializeField]
    private Button openClosePallet;
    private RectTransform palletParent;
    private Vector2 startPos, endPos;
    private bool _isOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        palletParent = GetComponent<RectTransform>();
        startPos = palletParent.position;
        endPos = palletParent.position + (Vector3.up*4);
        openClosePallet.onClick.AddListener(OpenClosePallet);        
    }    

    public void OpenClosePallet()
    {
        palletParent.transform.DOMove(_isOpen ? startPos : endPos,1f);
        openClosePallet.GetComponentInChildren<TextMeshProUGUI>().transform.DOLocalRotate(_isOpen ? Vector3.zero : new Vector3(0, 0, 180), 0.3f);
        _isOpen = !_isOpen;
    }

}
