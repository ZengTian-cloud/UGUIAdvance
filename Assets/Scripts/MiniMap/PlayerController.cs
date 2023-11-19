using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    Rigidbody rigidbody;

    float h;
    float v;

    Vector3 move;

    public float speed = 3f;

    Animator animator;

    private Vector3 mousePosition;

    private bool isDraging = false;

    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        animator = transform.GetComponent<Animator>();

        mousePosition = Input.mousePosition;
    }

    public void  Move()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");

        move = new Vector3(h, 0, v);
        move = transform.TransformDirection(move);
        rigidbody.MovePosition(transform.position + move * Time.deltaTime * speed);

        

        if (Mathf.Approximately(h,0.0f)   && Mathf.Approximately(v, 0.0f))
        {
            // 静止的状态
            animator.SetInteger("animation", 1);
        }
        else if (h == 0.0f && v != 0.0f)
        {   // 前后走
            if (v > 0)
            {
                animator.SetInteger("animation", 6);
            }
            else
            {
                animator.SetInteger("animation", 9);
            }

        }
        else if (h != 0.0f && v == 0.0f) // 左右走
        {
            if (h > 0)
            {
                animator.SetInteger("animation", 8);
            }
            else
            {
                animator.SetInteger("animation", 7);
            }
        }
        else
        {
            // 播放走的动画
            animator.SetInteger("animation", 6);
        }
    }

    public void Rotation() {

        if ( Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() )
        {
            mousePosition = Input.mousePosition;
            isDraging = true;
        }

        if ( Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && isDraging )
        {
            // 当鼠标进行移动的时候进行旋转
            //  Input.mousePosition - mousePosition
            transform.eulerAngles += new Vector3(0f, (Input.mousePosition.x - mousePosition.x) * Time.deltaTime * 2,0);
            //transform.rotation =  Quaternion.Euler( , 0f));
            mousePosition = Input.mousePosition;
        }
        if ( Input.GetMouseButtonUp(0) )
        {
            isDraging = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();

    }


    public void OnJoystickVelocityChange( Vector2 velocity )
    {
        //Debug.Log(velocity);
        h = velocity.x;
        v = velocity.y;
    }

}
