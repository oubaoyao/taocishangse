using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Es.InkPainter.Sample;
using System.IO;
using System;

public class AppreciatePanel : BasePanel
{
    public Button /*Model_Right_Button, Model_Left_Button,*/ Image_Right_Button, Image_Left_Button, BackButton;
    public Button[] ImageButtonGroup;
    public List<Texture> WorksTexture = new List<Texture>();
    public RawImage[] ImageGroup;
    private Texture[] WorksDisplayTextureArray;
    private Texture[] WorksTextureArray;
    public Transform ChooseIngImage;
    public float[] ChooseIngImageX = { -267.9f, -134.2f, 1.0f, 134.1f, 269.0f };

    private int Index = 0;
    private int CurrentModelNumber = 0;

    public Animation Appreciatetiltle;

    private Transform CurrentModel = null;

    public override void InitFind()
    {
        base.InitFind();
        //Model_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Model_Right_Button");
        //Model_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Model_Left_Button");
        Image_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Right_Button");
        Image_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Left_Button");
        BackButton = FindTool.FindChildComponent<Button>(transform, "Buttons/BackButton");

        ImageButtonGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<Button>();
        ImageGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<RawImage>();
        ChooseIngImage = FindTool.FindChildNode(transform, "ChooseIng");

        Appreciatetiltle = FindTool.FindChildComponent<Animation>(transform, "Appreciatetiltle");
    }

    public override void InitEvent()
    {
        base.InitEvent();

        Image_Right_Button.onClick.AddListener(() => {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            Right();
        });

        Image_Left_Button.onClick.AddListener(() => {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            Left();
        });

        BackButton.onClick.AddListener(() => {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            ModelControl.Instance.CloseModel2();
            TCSSstate.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);

        });
    }

    public override void Open()
    {
        base.Open();
        //ChooseIngImage.GetComponent<Image>().enabled = false;
        Appreciatetiltle.Play();
        ChooseIngImage.localPosition = new Vector3(-267.8f, -379.7f);
        Index = 0;
        ModelControl.Instance.ColorSelector.SetActive(false);
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
                        ImageGroup[i].texture = WorksDisplayTextureArray[WorksDisplayTextureArray.Length - 1 - i];
                    }
                }
            }

            ImageAddListen(ImageButtonGroup, Index);
            foreach (Transform item in ModelControl.Instance.ModelGroup2)
            {
                if (item.name == WorksDataControl.Instance.worksDatas[WorksDisplayTextureArray.Length - 1].Model_name)
                {
                    CurrentModel = item;
                    item.gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = WorksTextureArray[WorksDisplayTextureArray.Length - 1];
                    item.gameObject.SetActive(true);
                    CurrentModelNumber = WorksDisplayTextureArray.Length - 1;
                }
            }
        }

        EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
    }

    private void OnUpdate(float timeProcess)
    {
        if(CurrentModel!=null)
        {
            CurrentModel.Rotate(Vector3.back);
        }
    }

    void InitButtons(Button btn, int i, int index)
    {
        btn.onClick.AddListener(delegate () {
            ModelControl.Instance.CloseModel2();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            //ChooseIngImage.GetComponent<Image>().enabled = true;
            if (i+index < WorksDisplayTextureArray.Length && WorksDisplayTextureArray != null)
            {
                foreach (Transform item in ModelControl.Instance.ModelGroup2)
                {
                    if (item.name == WorksDataControl.Instance.worksDatas[WorksDisplayTextureArray.Length - 1 - (i + index)].Model_name)
                    {
                        CurrentModel = item;
                        item.gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = WorksTextureArray[WorksDisplayTextureArray.Length - 1 - (i + index)];
                        item.gameObject.SetActive(true);
                        CurrentModelNumber = WorksDisplayTextureArray.Length - 1 - (i + index);
                    }
                }
            }
            ChooseIngImage.localPosition = new Vector3(ChooseIngImageX[i], -379.7f, 0);
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
                    ImageGroup[i].texture = WorksDisplayTextureArray[WorksDisplayTextureArray.Length - 1 - (i + Index)];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
            CurrentModelNumber++;
            ModelControl.Instance.CloseModel2();
            foreach (Transform item in ModelControl.Instance.ModelGroup2)
            {
                if (item.name == WorksDataControl.Instance.worksDatas[CurrentModelNumber].Model_name)
                {
                    CurrentModel = item;
                    item.gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = WorksTextureArray[CurrentModelNumber];
                    item.gameObject.SetActive(true);
                   
                }
            }
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
                    ImageGroup[i].texture = WorksDisplayTextureArray[WorksDisplayTextureArray.Length - 1 - (i + Index)];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
            CurrentModelNumber--;
            ModelControl.Instance.CloseModel2();
            foreach (Transform item in ModelControl.Instance.ModelGroup2)
            {
                if (item.name == WorksDataControl.Instance.worksDatas[CurrentModelNumber].Model_name)
                {
                    CurrentModel = item;
                    item.gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = WorksTextureArray[CurrentModelNumber];
                    item.gameObject.SetActive(true);

                }
            }
        }

    }

    public override void Hide()
    {
        base.Hide();
        EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
        Appreciatetiltle.Stop();
    }

    //public void PointDown_Right()
    //{
    //    ModelViewControls.Instance.Start_Rotate_Right();
    //}

    //public void PointDown_Left()
    //{
    //    ModelViewControls.Instance.Start_Rotate_Left();
    //}

    //public void PointUp()
    //{
    //    ModelViewControls.Instance.Stop_Rotate();
    //}
}
