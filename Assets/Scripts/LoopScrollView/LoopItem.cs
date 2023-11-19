using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopItem : MonoBehaviour
{

    #region 字段

    private RectTransform rect;
    private RectTransform viewRect;

    private Vector3[] rectCorners;
    private Vector3[] viewCorners;

    private LoopScrollViewType scrollViewType;

    #endregion



    #region 事件

    public Action onAddHead;
    public Action onRemoveHead;
    public Action onAddLast;
    public Action onRemoveLast;

    #endregion

    private void Awake()
    {
        rect = transform.GetComponent<RectTransform>();
        viewRect = transform.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();
        // 初始化数组
        rectCorners = new Vector3[4];
        viewCorners = new Vector3[4];
    }

    void Update()
    {
        LinstenerCorners();
    }

    public void LinstenerCorners() {
        // 获取自身的边界
        rect.GetWorldCorners(rectCorners);
        // 获取显示区域的边界
        viewRect.GetWorldCorners(viewCorners);

        if ( IsFirst() )
        {
            switch (scrollViewType)
            {
                case LoopScrollViewType.Horizontal:

                    if ( rectCorners[3].x < viewCorners[0].x )
                    {
                        // 把头节点隐藏掉
                        if (onRemoveHead != null) { onRemoveHead(); }
                    }

                    if ( rectCorners[0].x > viewCorners[0].x )
                    {
                        // 增加一个头节点
                        if (onAddHead != null) { onAddHead(); }
                    }


                    break;
                case LoopScrollViewType.Vertical:
                    if (rectCorners[0].y > viewCorners[1].y)
                    {
                        // 把头节点给隐藏掉
                        if (onRemoveHead != null) { onRemoveHead(); }
                    }
                    if (rectCorners[1].y < viewCorners[1].y)
                    {
                        // 添加头节点
                        if (onAddHead != null) { onAddHead(); }
                    }
                    break;
            }


        }

        if ( IsLast() )
        {

            switch (scrollViewType)
            {
                case LoopScrollViewType.Horizontal:

                    // 添加尾部
                    if (rectCorners[3].x < viewCorners[3].x)
                    {
                        if (onAddLast != null) { onAddLast(); }
                    }
                    // 回收尾部
                    if (rectCorners[0].x > viewCorners[3].x)
                    {
                        if (onRemoveLast != null) { onRemoveLast(); }
                    }

                    break;
                case LoopScrollViewType.Vertical:
                    // 添加尾部
                    if (rectCorners[0].y > viewCorners[0].y)
                    {
                        if (onAddLast != null) { onAddLast(); }
                    }
                    // 回收尾部
                    if (rectCorners[1].y < viewCorners[0].y)
                    {
                        if (onRemoveLast != null) { onRemoveLast(); }
                    }
                    break;
            }


        }


    }

    public bool IsFirst() {

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf) {
                if ( transform.parent.GetChild(i) == transform )
                {
                    return true;
                }
                break;
            }
        }

        return false;
    }

    public bool IsLast() {

        for (int i = transform.parent.childCount - 1; i >= 0; i--)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    // 设置LoopScrollView的类型
    public void SetLoopScrollViewType( LoopScrollViewType loopScrollViewType ) {
        this.scrollViewType = loopScrollViewType;
    }
}
