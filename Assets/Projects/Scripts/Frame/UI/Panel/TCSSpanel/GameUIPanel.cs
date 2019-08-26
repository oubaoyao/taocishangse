using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;
using Es.InkPainter;
using System;

public class GameUIPanel : BasePanel
{
    public Button Backbutton, CompleteButton, EraserButton,RightButton,LeftButton, PaintButton;
    public Button[] PaintSizeButton;
    public GamePanel gamePanel;
    public CompletePanel completePanel;

    public Texture2D PaintTexture, EraserTexture;
    private float[] PaintSize = { 0.1f, 0.25f, 0.5f, 0.75f };
    private SwitchSprite[] switchSprites;
    private List<SwitchSprite> EraserAndPaint = new List<SwitchSprite>();

    public override void InitFind()
    {
        base.InitFind();
        Backbutton = FindTool.FindChildComponent<Button>(transform, "buttons/BackButton");
        CompleteButton = FindTool.FindChildComponent<Button>(transform, "buttons/CompleteButton");
        EraserButton = FindTool.FindChildComponent<Button>(transform, "buttons/EraserButton");
        PaintButton = FindTool.FindChildComponent<Button>(transform, "buttons/PaintButton");
        RightButton = FindTool.FindChildComponent<Button>(transform, "buttons/RightButton");
        LeftButton = FindTool.FindChildComponent<Button>(transform, "buttons/LeftButton");
        PaintSizeButton = FindTool.FindChildNode(transform, "PaintSize/SizeGroup").GetComponentsInChildren<Button>();

        switchSprites = FindTool.FindChildNode(transform, "PaintSize/SizeGroup").GetComponentsInChildren<SwitchSprite>();

        completePanel = FindTool.FindChildComponent<CompletePanel>(transform, "CompletePanel");
        gamePanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        Backbutton.onClick.AddListener(() => {
            Hide();
            ModelControl.Instance.ResetMaterial();
            gamePanel.chooseuipanel.Open();           
        });

        CompleteButton.onClick.AddListener(() => {

            SaveModelData();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        });

        EraserButton.onClick.AddListener(() => {
            MousePainter.Instance.erase = true;
            Cursor.SetCursor(EraserTexture, Vector2.zero, CursorMode.Auto);
            EraserButton.Select();
            foreach (SwitchSprite item in EraserAndPaint)
            {
                item.InitButtonSprite();
            }
            EraserAndPaint[1].DownButtonSprite();
        });

        PaintButton.onClick.AddListener(() => {
            MousePainter.Instance.erase = false;
            Cursor.SetCursor(PaintTexture, Vector2.zero, CursorMode.Auto);
            foreach (SwitchSprite item in EraserAndPaint)
            {
                item.InitButtonSprite();
            }
            EraserAndPaint[0].DownButtonSprite();
        });

        for (int i = 0; i < PaintSizeButton.Length; i++)
        {
            InitButton(PaintSizeButton[i], i);      
        }

        EraserAndPaint.Add(PaintButton.gameObject.GetComponent<SwitchSprite>());
        EraserAndPaint.Add(EraserButton.gameObject.GetComponent<SwitchSprite>());
        //PaintRawImagePosition = PaintRawImage.gameObject.transform.localPosition;
    }

    private void InitButton(Button button, int a)
    {
        button.onClick.AddListener(() => {
            foreach (SwitchSprite item in switchSprites)
            {
                item.InitButtonSprite();
            }
            button.gameObject.GetComponent<SwitchSprite>().DownButtonSprite();
            MousePainter.Instance.brush.Scale = PaintSize[a]/5;
        });

    }

    private void InitPaintSize()
    {
        foreach (SwitchSprite item in switchSprites)
        {
            item.InitButtonSprite();
        }

        foreach (SwitchSprite item in EraserAndPaint)
        {
            item.InitButtonSprite();
        }
        switchSprites[0].DownButtonSprite();
        EraserAndPaint[0].DownButtonSprite();
        MousePainter.Instance.erase = false;
        MousePainter.Instance.brush.Scale = PaintSize[0]/5;
    }

    public override void Open()
    {
        base.Open();
        Cursor.visible = true;
        InitPaintSize();
        Cursor.SetCursor(PaintTexture, Vector2.zero, CursorMode.Auto);
        ModelControl.Instance.ColorSelector.SetActive(true);
        MousePainter.Instance.IsGamestart = true;
        
    }

    public override void Hide()
    {
        base.Hide();
        //Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        MousePainter.Instance.IsGamestart = false;
        gamePanel.chooseuipanel.Open();
        ModelControl.Instance.ColorSelector.SetActive(false);
    }

    IEnumerator getScreenTexture(string path)
    {
        yield return new WaitForEndOfFrame();
        //需要正确设置好图片保存格式
        Texture2D t = new Texture2D(248, 248, TextureFormat.RGB24, false);
        //按照设定区域读取像素；注意是以左下角为原点读取
        t.ReadPixels(new Rect(0.25f*Screen.width, 0.4f * Screen.height, 248, 248), 0, 0);
        t.Apply();
        WorksDataControl.Instance.WorksDisplayTexture.Add(t);
        //二进制转换
        byte[] byt = t.EncodeToJPG();
        
        System.IO.File.WriteAllBytes(path, byt);
        GamePanel.CurrentModel.gameObject.SetActive(false);
        ModelControl.Instance.ColorSelector.SetActive(false);
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
