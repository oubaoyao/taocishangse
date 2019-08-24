using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;
using System.IO;

public class AppreciatePanel : BasePanel
{
    public Button Model_Right_Button, Model_Left_Button, Image_Right_Button, Image_Left_Button, BackButton;
    public Button[] ImageButtonGroup;
    public List<Texture> WorksTexture = new List<Texture>();
    public RawImage[] ImageGroup;
    private Texture[] WorksDisplayTextureArray;
    private Texture[] WorksTextureArray;


    private int Index = 0;

    public override void InitFind()
    {
        base.InitFind();
        Model_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Model_Right_Button");
        Model_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Model_Left_Button");
        Image_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Right_Button");
        Image_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Left_Button");
        BackButton = FindTool.FindChildComponent<Button>(transform, "Buttons/BackButton");

        ImageButtonGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<Button>();
        ImageGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<RawImage>();
    }

    public override void InitEvent()
    {
        base.InitEvent();
        //Model_Right_Button.onClick.AddListener(() => {

        //});

        //Model_Left_Button.onClick.AddListener(() => {

        //});

        Image_Right_Button.onClick.AddListener(() => {
            Right();
        });

        Image_Left_Button.onClick.AddListener(() => {
            Left();
        });

        BackButton.onClick.AddListener(() => {
  
            ModelControl.Instance.CloseModel2();
            TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);

        });
    }

    public override void Open()
    {
        base.Open();
        Index = 0;
        ModelControl.Instance.ColorSelector.SetActive(false);
        //ModelViewControls.Instance.Reset();
        //ModelControl.Instance.ResetMaterial();
        if (WorksDataControl.Instance.WorksDisplayTexture.Count > 0)
        {
            WorksDisplayTextureArray = null;
            WorksTextureArray = null;
            WorksDisplayTextureArray = WorksDataControl.Instance.WorksDisplayTexture.ToArray();
            WorksTextureArray = WorksDataControl.Instance.WorksTexture.ToArray();

            if (WorksDisplayTextureArray.Length != 0)
            {
                for (int i = 0; i < ImageGroup.Length; i++)
                {
                    if (i < WorksDisplayTextureArray.Length)
                    {
                        ImageGroup[i].texture = WorksDisplayTextureArray[i];
                    }
                }
            }

            ImageAddListen(ImageButtonGroup, Index);
        }
    }

    void InitButtons(Button btn, int i, int index)
    {
        btn.onClick.AddListener(delegate () {
            ModelControl.Instance.CloseModel2();
            if (i+index < WorksDisplayTextureArray.Length && WorksDisplayTextureArray != null)
            {
                foreach (Transform item in ModelControl.Instance.ModelGroup2)
                {
                    if (item.name == WorksDataControl.Instance.worksDatas[i + index].Model_name)
                    {
                        item.gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = WorksTextureArray[i + index];
                        item.gameObject.SetActive(true);
                    }
                }
            }
        });
    }

    public  void ImageAddListen(Button[] buttons, int index)
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

    public void Left()
    {
        if (WorksDisplayTextureArray.Length != 0)
        {
            Index--;
            if (Index < 0)
            {
                Index = 0;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < WorksDisplayTextureArray.Length)
                {
                    ImageGroup[i].texture = WorksDisplayTextureArray[i + Index];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
        }

    }

    public void Right()
    {
        if (WorksDisplayTextureArray.Length != 0)
        {
            Index++;
            if (Index + ImageGroup.Length > WorksDisplayTextureArray.Length)
            {
                Index--;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < WorksDisplayTextureArray.Length)
                {
                    ImageGroup[i].texture = WorksDisplayTextureArray[i + Index];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
        }

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
