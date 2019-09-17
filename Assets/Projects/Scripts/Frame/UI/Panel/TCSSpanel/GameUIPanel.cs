using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;
using Es.InkPainter;
using System;
using UnityEngine.EventSystems;

public class GameUIPanel : BasePanel
{
    public Button Backbutton, CompleteButton, EraserButton, RightButton, LeftButton, PaintButton;
    public Button[] PaintSizeButton, EraserSizeButton;
    public GamePanel gamePanel;
    public CompletePanel completePanel;

    public Texture2D PaintTexture, EraserTexture;
    private float[] PaintSize = { 0.025f, 0.05f, 0.075f, 0.1f }, EraserSize = { 0.05f, 0.15f };
    private SwitchSprite[] PanintswitchSprites, EraserswitchSprites;
    private List<SwitchSprite> EraserAndPaint = new List<SwitchSprite>();

    private Vector3 CurrentModelPosition = new Vector3(0, -2.12f, 3.91f);

    private float CurrentPaintSize, CurrentEraserSize/*, RotateValue = 0*/;

    public CanvasGroup PaintGroup, EraserGroup,tipgroup;
    public Animator tips1Anima;
    public Animation startcreattiltle;

    Texture2D cursor;
    private int width = Screen.width/5;
    private int height = Screen.width / 5;

    public override void InitFind()
    {
        base.InitFind();
        Backbutton = FindTool.FindChildComponent<Button>(transform, "buttons/BackButton");
        CompleteButton = FindTool.FindChildComponent<Button>(transform, "buttons/CompleteButton");
        EraserButton = FindTool.FindChildComponent<Button>(transform, "buttons/EraserButton");
        PaintButton = FindTool.FindChildComponent<Button>(transform, "buttons/PaintButton");
        //RightButton = FindTool.FindChildComponent<Button>(transform, "buttons/RightButton");
        //LeftButton = FindTool.FindChildComponent<Button>(transform, "buttons/LeftButton");

        PaintSizeButton = FindTool.FindChildNode(transform, "buttons/PaintSize/SizeGroup").GetComponentsInChildren<Button>();
        EraserSizeButton = FindTool.FindChildNode(transform, "buttons/EraserSize/SizeGroup").GetComponentsInChildren<Button>();

        PanintswitchSprites = FindTool.FindChildNode(transform, "buttons/PaintSize/SizeGroup").GetComponentsInChildren<SwitchSprite>();
        EraserswitchSprites = FindTool.FindChildNode(transform, "buttons/EraserSize/SizeGroup").GetComponentsInChildren<SwitchSprite>();

        completePanel = FindTool.FindChildComponent<CompletePanel>(transform, "CompletePanel");
        gamePanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");

        PaintGroup = FindTool.FindChildNode(transform, "buttons/PaintSize").GetComponentInChildren<CanvasGroup>();
        EraserGroup = FindTool.FindChildNode(transform, "buttons/EraserSize").GetComponentInChildren<CanvasGroup>();
        tipgroup = FindTool.FindChildComponent<CanvasGroup>(transform, "tipGroup");

        tips1Anima = FindTool.FindChildComponent<Animator>(transform, "tipGroup/tips1");

        startcreattiltle = FindTool.FindChildComponent<Animation>(transform, "startcreattiltle");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        Backbutton.onClick.AddListener(() =>
        {
            Hide();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            gamePanel.chooseuipanel.Open();
        });

        CompleteButton.onClick.AddListener(() =>
        {
            MousePainter.Instance.IsGamestart = false;
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            startcreattiltle.Stop();
            PaintGroup.alpha = 0;
            EraserGroup.alpha = 0;
            SaveModelData();
        });

        EraserButton.onClick.AddListener(() =>
        {
            ChooseEraser();
        });

        PaintButton.onClick.AddListener(() =>
        {
            ChoosePaint();
        });

        for (int i = 0; i < PaintSizeButton.Length; i++)
        {
            InitPaintButton(PaintSizeButton[i], i);
        }

        for (int i = 0; i < EraserSizeButton.Length; i++)
        {
            InitEraserButton(EraserSizeButton[i], i);
        }

        EraserAndPaint.Add(PaintButton.gameObject.GetComponent<SwitchSprite>());
        EraserAndPaint.Add(EraserButton.gameObject.GetComponent<SwitchSprite>());
        //PaintRawImagePosition = PaintRawImage.gameObject.transform.localPosition;
    }

    private void ChooseEraser()
    {
        AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        MousePainter.Instance.erase = true;
        cursor = EraserTexture;
        MousePainter.Instance.brush.Scale = CurrentEraserSize;
        EraserButton.Select();
        foreach (SwitchSprite item in EraserAndPaint)
        {
            item.InitButtonSprite();
        }
        EraserAndPaint[1].DownButtonSprite();
    }

    private void ChoosePaint()
    {
        AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        MousePainter.Instance.erase = false;
        cursor = PaintTexture;
        MousePainter.Instance.brush.Scale = CurrentPaintSize;
        
        foreach (SwitchSprite item in EraserAndPaint)
        {
            item.InitButtonSprite();
        }
        EraserAndPaint[0].DownButtonSprite();
    }

    private void InitPaintButton(Button button, int a)
    {
        button.onClick.AddListener(() =>
        {
            ChoosePaint();
            if (!MousePainter.Instance.erase)
            {
                foreach (SwitchSprite item in PanintswitchSprites)
                {
                    item.InitButtonSprite();
                }
                button.gameObject.GetComponent<SwitchSprite>().DownButtonSprite();
                MousePainter.Instance.brush.Scale = PaintSize[a] / 5;
                CurrentPaintSize = PaintSize[a] / 5;
            }
        });

    }

    private void InitEraserButton(Button button, int a)
    {
        button.onClick.AddListener(() =>
        {
            ChooseEraser();
            if (MousePainter.Instance.erase)
            {
                foreach (SwitchSprite item in EraserswitchSprites)
                {
                    item.InitButtonSprite();
                }
                button.gameObject.GetComponent<SwitchSprite>().DownButtonSprite();
                MousePainter.Instance.brush.Scale = EraserSize[a] / 5;
                CurrentEraserSize = EraserSize[a] / 5;
            }
        });
    }

    private void InitPaintSize()
    {
        foreach (SwitchSprite item in PanintswitchSprites)
        {
            item.InitButtonSprite();
        }

        foreach (SwitchSprite item in EraserswitchSprites)
        {
            item.InitButtonSprite();
        }

        foreach (SwitchSprite item in EraserAndPaint)
        {
            item.InitButtonSprite();
        }
        PanintswitchSprites[1].DownButtonSprite();
        EraserswitchSprites[0].DownButtonSprite();
        EraserAndPaint[0].DownButtonSprite();

        MousePainter.Instance.erase = false;
        MousePainter.Instance.brush.Scale = PaintSize[1] / 5;
        CurrentPaintSize = MousePainter.Instance.brush.Scale;
        CurrentEraserSize = EraserSize[0] / 5;
        cursor = PaintTexture;
    }

    public override void Open()
    {
        base.Open();
        OpenTips();
        InitPaintSize();
        startcreattiltle.Play();
        //Cursor.SetCursor(PaintTexture, Vector2.zero, CursorMode.Auto);
        ModelControl.Instance.ColorSelector.SetActive(true);
        ModelControl.Instance.Buttons.SetActive(true);
        MousePainter.Instance.IsGamestart = true;
        //EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "Aupdate", Aupdate);
        //EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "Aupdate", Aupdate);

        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, CloseTips);
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 15, CloseTips);
    }

    //private void Aupdate(float timeProcess)
    //{
    //    if (MousePainter.Instance.IsGamestart)
    //    {
    //        GamePanel.CurrentModel.Rotate(Vector3.forward * Time.deltaTime * RotateValue);
    //    }
    //}

    public override void Hide()
    {
        base.Hide();
        startcreattiltle.Stop();
        MousePainter.Instance.IsGamestart = false;
        gamePanel.chooseuipanel.Open();
        ModelControl.Instance.ColorSelector.SetActive(false);
        ModelControl.Instance.Buttons.SetActive(false);
        //EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "Aupdate", Aupdate);
    }

    IEnumerator getScreenTexture(string path)
    {
        yield return new WaitForEndOfFrame();
        //需要正确设置好图片保存格式
        Texture2D t = new Texture2D(675, 675, TextureFormat.RGB24, false);
        //按照设定区域读取像素；注意是以左下角为原点读取
        t.ReadPixels(new Rect(0.17f * Screen.width, 0.25f * Screen.height, 675, 675), 0, 0);
        t.Apply();
        WorksDataControl.Instance.WorksDisplayTexture.Add(t);
        //二进制转换
        byte[] byt = t.EncodeToJPG();

        System.IO.File.WriteAllBytes(path, byt);
        GamePanel.CurrentModel.gameObject.SetActive(false);
        ModelControl.Instance.ColorSelector.SetActive(false);
        ModelControl.Instance.Buttons.SetActive(false);
        completePanel.Open();
    }

    private void SaveModelData()
    {
        CloseTips();
        if(GamePanel.CurrentModel.name == "Cylinder003")
        {
            GamePanel.CurrentModel.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            GamePanel.CurrentModel.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        WorksData worksData = new WorksData();
        string str = Time.time.ToString();
        string str1 = Application.streamingAssetsPath + "/SaveImage/" + str + ".jpg";
        
        worksData.Model_name = GamePanel.CurrentModel.name;
        worksData.Jpg_path = str;

        InkCanvas[] inkCanvas = GamePanel.CurrentModel.GetComponentsInChildren<InkCanvas>();
        List<string> temp = new List<string>();
        List<Texture2D> texture2Ds = new List<Texture2D>();
        DateTime dt = DateTime.Now;  
        for (int i = 0; i < inkCanvas.Length; i++)
        {
            string str3 = dt.ToFileTime().ToString() + i.ToString() + Time.time.ToString();
            Debug.Log("ToUniversalTime===" + dt.ToUniversalTime().ToString());
            string str2 = Application.streamingAssetsPath + "/SavePng/" + str3 + ".png";
            temp.Add(str3);
            texture2Ds.Add(inkCanvas[i].SaveRenderTextureToPNG(str2));
        }

        worksData.Texture_Path = temp.ToArray();

        WorksDataControl.Instance.worksDatas.Add(worksData);
        Texture2DGroup texture2DGroup = new Texture2DGroup();
        texture2DGroup.texture2Ds = texture2Ds.ToArray();
        WorksDataControl.Instance.WorksTexture.Add(texture2DGroup);

        if (WorksDataControl.Instance.worksDatas.Count > 10)
        {
            WorksDataControl.Instance.DeleteTexture();
        }
        StartCoroutine(getScreenTexture(str1));
    }

    //public void PointDown_Right()
    //{
    //    AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
    //    GamePanel.CurrentModel.Rotate(Vector3.forward * -18);
    //    RotateValue = -20;

    //    //ModelViewControls.Instance.Start_Rotate_Right();
    //}

    //public void PointDown_Left()
    //{
    //    AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
    //    GamePanel.CurrentModel.Rotate(Vector3.forward * 18);
    //    RotateValue = 20;
    //    //GamePanel.CurrentModel.Rotate(Vector3.back * Time.deltaTime);
    //    //ModelViewControls.Instance.Start_Rotate_Left();
    //}

    //public void PointUp()
    //{
    //    RotateValue = 0;
    //    //ModelViewControls.Instance.Stop_Rotate();
    //}

    private void CloseTips()
    {
        tipgroup.alpha = 0;
        tipgroup.blocksRaycasts = false;

    }

    private void OpenTips()
    {
        tipgroup.alpha = 1;
        tipgroup.blocksRaycasts = false;
    }

    void OnGUI()
    {
        if(MousePainter.Instance.IsGamestart)
        {
            GUI.DrawTexture(new Rect(Event.current.mousePosition.x - width/5, Event.current.mousePosition.y - height/5, width, height), cursor);
            //GUI.DrawTexture(new Rect(Event.current.mousePosition.x  , Event.current.mousePosition.y  , width, height), cursor);
        }
        
    }
}
