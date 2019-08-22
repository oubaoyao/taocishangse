﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class ChooseUIPanel : BasePanel
{
    public GameScrollItem ScrollItem;
    public Button LeftButton, RightButton, DetermineButton;
    public Button[] ImageButtonGroup;
    public GamePanel gamePanel;

    public override void InitFind()
    {
        base.InitFind();
        ScrollItem = FindTool.FindChildComponent<GameScrollItem>(transform, "ImageGroup");
        LeftButton = FindTool.FindChildComponent<Button>(transform, "LeftAndRight/left");
        RightButton = FindTool.FindChildComponent<Button>(transform, "LeftAndRight/Right");
        ImageButtonGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<Button>();
        DetermineButton = FindTool.FindChildComponent<Button>(transform, "DetermineButton");

        gamePanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        DetermineButton.onClick.AddListener(() => {
            Hide();
            gamePanel.gameuiPanel.Open();
        });

        LeftButton.onClick.AddListener(() => {
            ScrollItem.Left();
        });

        RightButton.onClick.AddListener(() => {
            ScrollItem.Right();
        });
    }

    public override void Open()
    {
        base.Open();
        gamePanel.InitModel();
    }

    public override void Hide()
    {
        base.Hide();
    }
}