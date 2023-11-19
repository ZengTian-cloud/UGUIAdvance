using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour ,IBeginDragHandler , ICanvasRaycastFilter ,IDragHandler
{

    private Vector3 offset;
    private RectTransform rect;

    private bool isDraging = false;

    public Action onStartDrag;
    public Action onDrag;
    public Action onEndDrag;

    private Camera uiCamera;
    private Canvas canvas;
    private void Awake()
    {
        rect = transform.GetComponent<RectTransform>();
        if ( rect == null )
        {
            throw new System.Exception("只能拖拽UI物体!");
        }
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
    }

    private void Update()
    {
        if ( isDraging )
        {
            // rect.position += (Input.mousePosition - mousePosition);
            rect.position = uiCamera.ScreenToWorldPoint(Input.mousePosition+ new Vector3(0, 0, canvas.planeDistance));
             
            if (onDrag != null) { onDrag(); }
        }

        if ( Input.GetMouseButtonUp(0) && isDraging )
        {
            if (onEndDrag != null) { onEndDrag(); }
            isDraging = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onStartDrag != null) { onStartDrag(); }
        isDraging = true;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !isDraging;
    }

    public void OnDrag(PointerEventData eventData)
    {
         
    }
}
