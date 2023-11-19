using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class OnVelocityChange : UnityEvent<Vector2> {

}

public class Joystick : MonoBehaviour
{

    #region 字段
    // Handle最大的移动距离
    public float max_distance;
    // Handle最小的移动距离
    public float min_distance;
    // Handle
    JoystickHandle joystickHandle;
    // 方向
    private Transform direction;

    #endregion

    #region 事件
    public OnVelocityChange onVelocityChange; // 摇杆数据改变的事件
    #endregion 

    #region 属性

    public JoystickHandle JoystickHandle {
        get {
            return joystickHandle;
        }
    }

    #endregion

    #region Unity回调
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateEvents();
        UpdateDirection();
    }

    #endregion

    #region 方法
    // 初始化
    public void Init()
    {
        joystickHandle = transform.GetComponentInChildren<JoystickHandle>();
        if (joystickHandle == null)
        {
            throw new System.Exception(" 未查询到joystickHandle! ");
        }

        direction = transform.Find("direction");
    }
    // 更新事件
    public void UpdateEvents() {
        if (onVelocityChange != null)
        {
            onVelocityChange.Invoke(joystickHandle.velocity);
        }
    }
    // 更新方向
    public void UpdateDirection() {
        direction.eulerAngles = new Vector3(0, 0, joystickHandle.angle);
        if (joystickHandle.velocity.Equals(Vector2.zero))
        {
            direction.gameObject.SetActive(false);
        }
        else {
            direction.gameObject.SetActive(true);
        }
    }
    #endregion


}
