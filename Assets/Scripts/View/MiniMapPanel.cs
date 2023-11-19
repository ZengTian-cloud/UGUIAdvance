using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPanel : ViewBase
{
    public override void Show()
    {
        //base.Show();
        Invoke("ShowExc", 2);
        Camera.main.GetComponent<Animator>().SetBool("show_minimap", true);
    }
    public void ShowExc()
    {
        base.Show();

        GameObject.Find("Riko").GetComponent<PlayerController>().enabled = true;

        // 把相机的Animator 给禁用掉
        Camera.main.GetComponent<Animator>().enabled = false;
        // 把相机设置成人物的子节点
        Camera.main.transform.SetParent(GameObject.Find("Riko").transform);
    }
}
