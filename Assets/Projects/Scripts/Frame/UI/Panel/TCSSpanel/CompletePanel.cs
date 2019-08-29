﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;

public class CompletePanel : BasePanel
{
    public Button RemakeButtom, AppreciateButton, ReturnButton;
    public GameUIPanel gameuipanel;
    public Animator starAniamtor;

    public override void InitFind()
    {
        base.InitFind();
        RemakeButtom = FindTool.FindChildComponent<Button>(transform, "BGImage/RemakeButtom");
        AppreciateButton = FindTool.FindChildComponent<Button>(transform, "BGImage/AppreciateButton");
        ReturnButton = FindTool.FindChildComponent<Button>(transform, "BGImage/ReturnButton");

        gameuipanel = FindTool.FindParentComponent<GameUIPanel>(transform, "GameUIPanel");
        starAniamtor = FindTool.FindChildComponent<Animator>(transform, "BGImage/StarAnima");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        RemakeButtom.onClick.AddListener(() => {
            Hide();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            GamePanel.CurrentModel.gameObject.SetActive(true);
            ModelControl.Instance.ResetMaterial(); 
            gameuipanel.Open();
        });

        AppreciateButton.onClick.AddListener(() => {
            Hide();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.AppreciatePanel);
        });

        ReturnButton.onClick.AddListener(() => {
            Hide();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);      
        });
    }

    public override void Open()
    {
        base.Open();
        //Cursor.visible = false;
        starAniamtor.SetBool("newstate-starAnimation", true);
        starAniamtor.SetBool("starlooperanimation-newstate", false);
    }

    public override void Hide()
    {
        base.Hide();
        starAniamtor.SetBool("newstate-starAnimation", false);
        starAniamtor.SetBool("starlooperanimation-newstate", true);
    }
}
