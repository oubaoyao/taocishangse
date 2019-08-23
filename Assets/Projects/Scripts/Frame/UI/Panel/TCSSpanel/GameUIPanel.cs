using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;
using Es.InkPainter;

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
            MousePainter.Instance.ResetMaterial();
        });

        CompleteButton.onClick.AddListener(() => {

            SaveModelData();

        });

        EraserButton.onClick.AddListener(() => {
            if (MousePainter.Instance.erase == true)
                MousePainter.Instance.erase = true;
            else
                MousePainter.Instance.erase = false;
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
        MousePainter.Instance.IsGamestart = true;
    }

    public override void Hide()
    {
        base.Hide();
        MousePainter.Instance.IsGamestart = false;
        //TexturePainter.Instance.Colorselector.SetActive(false);
        gamePanel.chooseuipanel.Open();
    }

    IEnumerator getScreenTexture(string path)
    {
        yield return new WaitForEndOfFrame();
        //需要正确设置好图片保存格式
        Texture2D t = new Texture2D(500, 500, TextureFormat.RGB24, false);
        //按照设定区域读取像素；注意是以左下角为原点读取
        t.ReadPixels(new Rect(250, 500, 500, 500), 0, 0);
        t.Apply();
        //二进制转换
        byte[] byt = t.EncodeToJPG();
        
        System.IO.File.WriteAllBytes(path, byt);
        GamePanel.CurrentModel.gameObject.SetActive(false);
        completePanel.Open();
    }

    private void SaveModelData()
    {
        WorksData worksData = new WorksData();
        string str1 = Application.dataPath + "/Resources/SaveImage/" + Time.time + ".jpg";
        string str2 = Application.dataPath + "/Resources/SavePng/" + Time.time + ".png";
        worksData.Model_name = GamePanel.CurrentModel.name;
        worksData.Jpg_path = str1;
        worksData.Texture_Path = str2;

        WorksDataControl.Instance.worksDatas.Add(worksData);
        StartCoroutine(getScreenTexture(str1));

        if (GamePanel.CurrentModel.GetComponent<InkCanvas>() != null)
        {
            GamePanel.CurrentModel.GetComponent<InkCanvas>().SaveRenderTextureToPNG(str2);
        }
        else
            Debug.Log("组件InkCanvas丢失!!!!");

    }
}
