using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : ViewBase
{

    public GonggaoPanel gonggaoPanel;
    public FriendsPanel friendsPanel;
    public TaskView taskView;
    public BagPanel bagPanel;
    public MiniMapPanel mapPanel;
    public JoystickPanel joystickPanel;
    public void OnBtnGonggaoClick() {
        gonggaoPanel.Show();
    }

    public void OnBtnFriendsClick() {
        friendsPanel.Show();
    }

    public void OnBtnTaskClick() {
        taskView.Show();
    }

    public void OnBtnBagPanelClick() {
        bagPanel.Show();
        this.Hide();
    }

    public void OnBtnStartGameClick() {

        mapPanel.Show();
        joystickPanel.Show();
        this.Hide();
    }

    public override void Hide()
    {
        //base.Hide();
        transform.GetComponent<Animator>().SetBool("isShow", false);
    }

    public override void Show()
    {
        //base.Show();
        transform.GetComponent<Animator>().SetBool("isShow", true);
    }

}
