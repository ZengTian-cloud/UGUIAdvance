using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTest : MonoBehaviour, IPointerUpHandler , 
    //IPointerClickHandler,
    //IPointerDownHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
        Debug.Log(" 开始拖拽! ");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(" 结束拖拽! ");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(" 鼠标抬起 ");
    }
}
