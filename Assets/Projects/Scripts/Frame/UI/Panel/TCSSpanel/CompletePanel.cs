using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;
using System;

public class CompletePanel : BasePanel
{
    public Button RemakeButtom, AppreciateButton, ReturnButton;
    public GameUIPanel gameuipanel;
    public Animator starAniamtor;
    public RawImage DisplayRawImage;
    public CanvasGroup zhuangshicanvas;
    public Animation tiltle;

    public override void InitFind()
    {
        base.InitFind();
        RemakeButtom = FindTool.FindChildComponent<Button>(transform, "BGImage/RemakeButtom");
        AppreciateButton = FindTool.FindChildComponent<Button>(transform, "BGImage/AppreciateButton");
        ReturnButton = FindTool.FindChildComponent<Button>(transform, "BGImage/ReturnButton");

        gameuipanel = FindTool.FindParentComponent<GameUIPanel>(transform, "GameUIPanel");
        starAniamtor = FindTool.FindChildComponent<Animator>(transform, "BGImage/StarAnima");

        DisplayRawImage = FindTool.FindChildComponent<RawImage>(transform, "DisplayGroup/DisplayRaw");

        zhuangshicanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "DisplayGroup/zhuangshi");
        tiltle = FindTool.FindChildComponent<Animation>(transform, "DisplayGroup/tiltle");
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
        tiltle.Play();
        //Cursor.visible = false;
        starAniamtor.SetBool("newstate-starAnimation", true);
        starAniamtor.SetBool("starlooperanimation-newstate", false);
        DisplayRawImage.texture = WorksDataControl.Instance.WorksDisplayTexture[WorksDataControl.Instance.WorksDisplayTexture.Count - 1];

        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 3.0f, Displayzhuangshi);
        AudioManager.PlayAudio("陶瓷星星出现-结束升级失败胜利_0 _19.mp3_爱给网_aigei_com", transform, MTFrame.MTAudio.AudioEnunType.Effset);
    }

    private void Displayzhuangshi()
    {
        zhuangshicanvas.alpha = 1;
        AudioManager.PlayAudio("勋章-正确的声音2", transform, MTFrame.MTAudio.AudioEnunType.Effset);
    }

    public override void Hide()
    {
        base.Hide();
        tiltle.Stop();
        AudioManager.StopAudio("陶瓷星星出现-结束升级失败胜利_0 _19.mp3_爱给网_aigei_com", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        AudioManager.StopAudio("陶瓷制作-星星出现", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, Displayzhuangshi);
        starAniamtor.SetBool("newstate-starAnimation", false);
        starAniamtor.SetBool("starlooperanimation-newstate", true);
        gameuipanel.EraserGroup.alpha = 1;
        gameuipanel.PaintGroup.alpha = 1;
        zhuangshicanvas.alpha = 0;
    }
}
