using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class GameUIPanel : BasePanel
{
    public Button Backbutton, CompleteButton, EraserButton,RightButton,LeftButton;
    public GamePanel gamePanel;
    public CompletePanel completePanel;

    public override void InitFind()
    {
        base.InitFind();
        Backbutton = FindTool.FindChildComponent<Button>(transform, "buttons/BackButton");
        CompleteButton = FindTool.FindChildComponent<Button>(transform, "buttons/CompleteButton");
        EraserButton = FindTool.FindChildComponent<Button>(transform, "buttons/EraserButton");
        RightButton = FindTool.FindChildComponent<Button>(transform, "buttons/RightButton");
        LeftButton = FindTool.FindChildComponent<Button>(transform, "buttons/LeftButton");

        completePanel = FindTool.FindChildComponent<CompletePanel>(transform, "CompletePanel");
        gamePanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        Backbutton.onClick.AddListener(() => {
            Hide();
            gamePanel.chooseuipanel.Open();
            //TexturePainter.Instance.SaveTexture();
            gamePanel.ResetMaterial();
        });

        CompleteButton.onClick.AddListener(() => {
            StartCoroutine("getScreenTexture");
        });

        EraserButton.onClick.AddListener(() => {
            //TexturePainter.Instance.brushColor = Color.white;
        });

        RightButton.onClick.AddListener(() => {
            gamePanel.ratation_right();
        });

        LeftButton.onClick.AddListener(() => {
            gamePanel.rotation_left();
        });
    }

    public override void Open()
    {
        base.Open();
        //TexturePainter.Instance.Colorselector.SetActive(true);
        //TexturePainter.Instance.IsGamestart = true;
    }

    public override void Hide()
    {
        base.Hide();
        //TexturePainter.Instance.IsGamestart = false;
        //TexturePainter.Instance.Colorselector.SetActive(false);
        gamePanel.chooseuipanel.Open();
    }

    IEnumerator getScreenTexture()
    {
        yield return new WaitForEndOfFrame();
        //需要正确设置好图片保存格式
        Texture2D t = new Texture2D(500, 500, TextureFormat.RGB24, false);
        //按照设定区域读取像素；注意是以左下角为原点读取
        t.ReadPixels(new Rect(300, 300, 500, 500), 0, 0);
        t.Apply();
        //二进制转换
        byte[] byt = t.EncodeToJPG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/SaveImage/" + Time.time + ".jpg", byt);

    }
}
