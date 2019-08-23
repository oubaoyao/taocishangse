using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;

public class AppreciatePanel : BasePanel
{
    public Button Model_Right_Button, Model_Left_Button, Image_Right_Button, Image_Left_Button, BackButton;
    public Button[] ImageButtonGroup;
    public List<Texture> WorksTexture = new List<Texture>();
    public RawImage[] ImageGroup;
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
        Model_Right_Button.onClick.AddListener(() => {

        });

        Model_Left_Button.onClick.AddListener(() => {

        });

        Image_Right_Button.onClick.AddListener(() => {
            Right();
        });

        Image_Left_Button.onClick.AddListener(() => {
            Left();
        });

        BackButton.onClick.AddListener(() => {
            TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);
        });
    }

    public override void Open()
    {
        base.Open();
        Index = 0;
        if (WorksDataControl.Instance.worksDatas!=null)
        {
            Debug.Log("111");
            for (int i = 0; i < WorksDataControl.Instance.worksDatas.Count; i++)
            {
                WorksTexture.Add(Resources.Load<Texture>(WorksDataControl.Instance.worksDatas[i].Texture_Path));
            }
            WorksTextureArray = null;
            WorksTextureArray = WorksTexture.ToArray();
            
            if (WorksTextureArray.Length != 0)
            {
                for (int i = 0; i < ImageGroup.Length; i++)
                {
                    if (i < WorksTextureArray.Length)
                    {
                        ImageGroup[i].texture = WorksTextureArray[i];
                    }
                }
            }

            ImageAddListen(ImageButtonGroup, Index);
        }
    }

     void InitButtons(Button btn, int i, int index)
    {
        btn.onClick.AddListener(delegate () {
            foreach (Transform item in ModelControl.Instance.ModelGroup)
            {
                item.gameObject.SetActive(false);
                //item.GetComponent<MeshRenderer>().enabled = false;
            }
            //Debug.LogFormat("i=={0},index=={1},i+index=={2}", i, index, i + index);
            //ModelControl.Instance.ModelGroup[i + index].gameObject.SetActive(true);
            //ModelControl.Instance.ModelGroup[i + index].GetComponent<MeshRenderer>().enabled = true;
            MousePainter.Instance.ResetMaterial();
            if(i+index < WorksTextureArray.Length && WorksTextureArray!=null)
            {
                foreach (Transform item in ModelControl.Instance.ModelGroup)
                {
                    if (item.name == WorksDataControl.Instance.worksDatas[i + index].Model_name)
                    {
                        item.gameObject.SetActive(true);
                        item.gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = Resources.Load<Texture>(WorksDataControl.Instance.worksDatas[i + index].Texture_Path);
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
        if (WorksTextureArray.Length != 0)
        {
            Index--;
            if (Index < 0)
            {
                Index = 0;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < WorksTextureArray.Length)
                {
                    ImageGroup[i].texture = WorksTextureArray[i + Index];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
        }

    }

    public void Right()
    {
        if (WorksTextureArray.Length != 0)
        {
            Index++;
            if (Index + ImageGroup.Length >= WorksTextureArray.Length)
            {
                Index--;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < WorksTextureArray.Length)
                {
                    ImageGroup[i].texture = WorksTextureArray[i + Index];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
        }

    }
}
