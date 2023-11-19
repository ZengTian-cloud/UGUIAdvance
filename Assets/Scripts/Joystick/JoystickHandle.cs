using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    #region 字段

    // 摇杆
    private Joystick joystick;
    // 拖拽时鼠标按下的位置
    private Vector3 mousePosition;
    // 当前的 Handle 的 RectTransform
    private RectTransform rect;
    // Canvas
    private Canvas canvas;

    // 角度
    public float angle;
    // 限制 Handle 移动 x 的 最大距离
    private float limitX;
    // 限制 Handle 移动 y 的 最大距离
    private float limitY;

    // 结束拖拽 移回原点的时间
    private float moveTime = 0.2f;
    // 结束拖拽 移回原点的计时
    private float moveTimer = 0;
    // 结束拖拽 移回原点的开始位置
    private Vector3 startMovePos;
    // 是不是移回原点
    private bool isMoveToOrigin = false;
    // 摇杆的数据
    public Vector2 velocity;

    #endregion

    #region 事件

    public Action onBeginDrag;
    public Action onDrag;
    public Action onEndDrag;

    #endregion

    #region Unity回调
    private void Start()
    {
        rect = transform.GetComponent<RectTransform>();
        joystick = transform.GetComponentInParent<Joystick>();
        canvas = transform.GetComponentInParent<Canvas>();
        if (canvas == null) { throw new System.Exception(" 未查询到Canvas组件! "); }
    }

    private void Update()
    {
        CaculateAngle();
        LimitHandlePos();
        MovingOrign();
        if (Vector3.Distance(transform.localPosition, Vector3.zero) > joystick.min_distance)
        {
            velocity = new Vector2(transform.localPosition.x / joystick.max_distance, transform.localPosition.y / joystick.max_distance);
        }
        else {
            velocity = Vector2.zero;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePosition = Input.mousePosition;
        if (onBeginDrag != null) { onBeginDrag(); }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rect.anchoredPosition += (Vector2)(Input.mousePosition - mousePosition);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
        transform.localPosition = localPoint;
        //mousePosition = Input.mousePosition;
        if (onDrag != null)
        {
            onDrag();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 移动到原点
        MoveToOrigin();
        if (onEndDrag != null)
        {
            onEndDrag();
        }
    }
    #endregion

    #region 方法
    // 计算角度
    public void CaculateAngle() {
        angle = Mathf.Atan2(transform.localPosition.y, transform.localPosition.x) * Mathf.Rad2Deg;

    }
    // 限制位置
    public void LimitHandlePos()
    {
        if (Vector3.Distance(transform.localPosition, Vector3.zero) >= joystick.max_distance)
        {
            limitY = Mathf.Sin(angle * Mathf.Deg2Rad) * joystick.max_distance;
            limitX = Mathf.Cos(angle * Mathf.Deg2Rad) * joystick.max_distance;

            transform.localPosition = new Vector3(limitX, limitY, 0);

        }
    }
    // 移回原点
    public void MovingOrign() {
        if (isMoveToOrigin)
        {
            moveTimer += Time.deltaTime * 1 / moveTime;
            transform.localPosition = Vector3.Lerp(startMovePos, Vector3.zero, moveTimer);
            if ( moveTimer >= 1 )
            {
                isMoveToOrigin = false;
                moveTimer = 0;
            }
        }
    }
    // 开始移动
    public void MoveToOrigin() {
        isMoveToOrigin = true;
        startMovePos = transform.localPosition;
    }
    #endregion

}
