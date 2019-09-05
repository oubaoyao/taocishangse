using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;


public class GamePanel : BasePanel
{
    public static Transform CurrentModel = null;

    public ChooseUIPanel chooseuipanel;
    public GameUIPanel gameuiPanel;
    public Animation Choosetiltle;

    public override void InitFind()
    {
        base.InitFind();
        chooseuipanel = FindTool.FindChildComponent<ChooseUIPanel>(transform, "ChooseUIPanel");
        gameuiPanel = FindTool.FindChildComponent<GameUIPanel>(transform, "GameUIPanel");
        Choosetiltle = FindTool.FindChildComponent<Animation>(transform, "Choosetiltle");
    }

    public override void Open()
    {
        base.Open();
        gameuiPanel.Hide();
        gameuiPanel.completePanel.Hide();
        InitModel();
        Choosetiltle.Play();
    }

    public override void Hide()
    {
        base.Hide();
        gameuiPanel.Hide();
        gameuiPanel.completePanel.Hide();
        ModelControl.Instance.CloseModel();
        Choosetiltle.Stop();
    }

    public void InitModel()
    {
        ImageAddListen(chooseuipanel.ImageButtonGroup, 0);
        //ModelViewControls.Instance.Reset();
        ModelControl.Instance.CloseModel();
        ModelControl.Instance.ModelGroup[0].gameObject.SetActive(true);
        CurrentModel = ModelControl.Instance.ModelGroup[0];
        chooseuipanel.ChooseIngImage.localPosition = new Vector3(-260f, -356f);
    }


    void InitButtons(Button btn,int i,int index)
    {
        btn.onClick.AddListener(delegate () {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            foreach (Transform item in ModelControl.Instance.ModelGroup)
            {
                item.gameObject.SetActive(false);
            }
            //Debug.LogFormat("i=={0},index=={1},i+index=={2}", i, index, i + index);
            ModelControl.Instance.ModelGroup[i + index].gameObject.SetActive(true);
            CurrentModel = ModelControl.Instance.ModelGroup[i + index];
            chooseuipanel.ChooseIngImage.localPosition = new Vector3(chooseuipanel.ChooseIngImageX[i], -356f, 0);
        });
    }

    public  void ImageAddListen(Button[] buttons,int index)
    {
        foreach (Button item in buttons)
        {
            item.onClick.RemoveAllListeners();
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            InitButtons(buttons[i], i, index);
        }
    }


}
