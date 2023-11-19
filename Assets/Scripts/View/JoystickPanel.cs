using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPanel : ViewBase
{

    public Joystick joystick;

    private Canvas canvas;

    public override void Show()
    {
        //base.Show();
        Invoke("ShowExc", 2);
    }
    private void ShowExc() {
        base.Show();
    }

    private void Start()
    {
        canvas = transform.GetComponentInParent<Canvas>();

        joystick.JoystickHandle.onBeginDrag += this.OnBeginDragHandle;
        joystick.JoystickHandle.onEndDrag += this.OnEndDragHandle;


    }

    public void OnBeginDragHandle() {
        joystick.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void OnEndDragHandle()
    {
        joystick.GetComponent<CanvasGroup>().alpha = 0.5f;
    }


    public void OnJoystickAreaClick() {
        Debug.Log(" OnJoystickAreaClick ");
        joystick.transform.position = canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, canvas.planeDistance));
    }

}
