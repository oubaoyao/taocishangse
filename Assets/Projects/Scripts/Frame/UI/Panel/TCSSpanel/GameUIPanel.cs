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
    public Slider sizeSlider;

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
        sizeSlider = FindTool.FindChildComponent<Slider>(transform, "buttons/Slider");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        Backbutton.onClick.AddListener(() => {
            Hide();
            ModelControl.Instance.ResetMaterial();
            gamePanel.chooseuipanel.Open();
            //TexturePainter.Instance.SaveTexture();
            
        });

        CompleteButton.onClick.AddListener(() => {

            SaveModelData();

        });

        EraserButton.onClick.AddListener(() => {
            if (MousePainter.Instance.erase == true)
                MousePainter.Instance.erase = false;
            else
                MousePainter.Instance.erase = true;
        });

        //RightButton.onClick.AddListener(() => {
        //    //gamePanel.ratation_right();
        //});

        

        //LeftButton.onClick.AddListener(() => {
        //    //gamePanel.rotation_left();
        //});

        
    }

    public override void Open()
    {
        base.Open();
        sizeSlider.value = 0.5f;
        
        ModelControl.Instance.ColorSelector.SetActive(true);
        MousePainter.Instance.IsGamestart = true;
        ColorSelector.myslf.InitColor();
    }

    public override void Hide()
    {
        base.Hide();
        MousePainter.Instance.IsGamestart = false;
        gamePanel.chooseuipanel.Open();
        ModelControl.Instance.ColorSelector.SetActive(false);
    }

    IEnumerator getScreenTexture(string path)
    {
        yield return new WaitForEndOfFrame();
        //需要正确设置好图片保存格式
        Texture2D t = new Texture2D(500, 500, TextureFormat.RGB24, false);
        //按照设定区域读取像素；注意是以左下角为原点读取
        t.ReadPixels(new Rect(250, 500, 500, 500), 0, 0);
        t.Apply();
        WorksDataControl.Instance.WorksDisplayTexture.Add(t);
        //二进制转换
        byte[] byt = t.EncodeToJPG();
        
        System.IO.File.WriteAllBytes(path, byt);
        GamePanel.CurrentModel.gameObject.SetActive(false);
        completePanel.Open();
    }

    private void SaveModelData()
    {
        WorksData worksData = new WorksData();
        string str = Time.time.ToString();
        string str1 = Application.streamingAssetsPath + "/SaveImage/" + str + ".jpg";
        string str2 = Application.streamingAssetsPath + "/SavePng/" + str + ".png";
        worksData.Model_name = GamePanel.CurrentModel.name;
        worksData.Jpg_path = str;
        worksData.Texture_Path = str;

        WorksDataControl.Instance.worksDatas.Add(worksData);
        StartCoroutine(getScreenTexture(str1));

        if (GamePanel.CurrentModel.GetComponent<InkCanvas>() != null)
        {
            GamePanel.CurrentModel.GetComponent<InkCanvas>().SaveRenderTextureToPNG(str2);
        }
        else
            Debug.Log("组件InkCanvas丢失!!!!");

    }

    public void UpdateSizeSlider()
    {
        MousePainter.Instance.brush.Scale = sizeSlider.value/5;
    }

    public void PointDown_Right()
    {
        ModelViewControls.Instance.Start_Rotate_Right();
    }

    public void PointDown_Left()
    {
        ModelViewControls.Instance.Start_Rotate_Left();
    }

    public void PointUp()
    {
        ModelViewControls.Instance.Stop_Rotate();
    }
}
