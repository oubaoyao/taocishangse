using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class AppreciatePanel : BasePanel
{
    public Button Model_Right_Button, Model_Left_Button, Image_Right_Button, Image_Left_Button, BackButton;

    public override void InitFind()
    {
        base.InitFind();
        Model_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Model_Right_Button");
        Model_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Model_Left_Button");
        Image_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Right_Button");
        Image_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Left_Button");
        BackButton = FindTool.FindChildComponent<Button>(transform, "Buttons/BackButton");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        Model_Right_Button.onClick.AddListener(() => {

        });

        Model_Left_Button.onClick.AddListener(() => {

        });

        Image_Right_Button.onClick.AddListener(() => {

        });

        Image_Left_Button.onClick.AddListener(() => {

        });

        BackButton.onClick.AddListener(() => {
            TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);
        });
    }
}
